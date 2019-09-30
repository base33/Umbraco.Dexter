using Dexter.Core.Interfaces;
using Dexter.Core.Models.Config;
using Dexter.Core.Models.Indexable;
using Dexter.Core.Models.IndexStrategy;
using Dexter.Core.Provider;
using Dexter.Core.Resolvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using IContent = Umbraco.Core.Models.IContent;
using ContentType = Dexter.Core.Models.Config.ContentType;

namespace Dexter.Core.Services
{
    public class IndexService
    {
        protected DexterConfigProvider ConfigProvider { get; }
        protected IndexStrategyResolver IndexStrategyResolver { get; set; }

        public IndexService(DexterConfigProvider elasticSearchConfigProvider)
        {
            ConfigProvider = elasticSearchConfigProvider;
            IndexStrategyResolver = new IndexStrategyResolver(ConfigProvider.GetRootConfig());
        }

        /// <summary>
        /// Indexes a content item
        /// </summary>
        /// <param name="content">The content item</param>
        /// <param name="indexName">Optional index name to index specific index, ignoring others</param>
        public void Index(IContentBase content, Source source, string indexName = null)
        {
            var config = ConfigProvider.GetRootConfig();

            var indexer = LoadIndexer(config);

            var indexes = indexName != null ? new[] {indexName} : config.Indexes.ToArray();

            foreach (var indexAlias in indexes)
            {
                var indexConfig = ConfigProvider.GetIndexConfig(indexAlias);

                var contentTypeConfig = source == Source.Content
                    ? indexConfig.ContentTypes.FirstOrDefault(c => c.Alias == content.GetContentType().Alias || c.Aliases.Contains(content.GetContentType().Alias))
                    : indexConfig.MediaTypes.FirstOrDefault(c => c.Alias == content.GetContentType().Alias || c.Aliases.Contains(content.GetContentType().Alias));

                if (contentTypeConfig == null)
                {
                    contentTypeConfig = indexConfig.ContentTypes.FirstOrDefault(c => string.IsNullOrWhiteSpace(c.Alias));
                    if (contentTypeConfig == null)
                        continue;
                }

                var indexableItem = GetIndexableItem(indexConfig, contentTypeConfig, content);

                switch(source)
                {
                    case Source.Content:
                        indexableItem.AddOrUpdate("_umbracoSource", "content");
                        break;
                    case Source.Media:
                        indexableItem.AddOrUpdate("_umbracoSource", "media");
                        break;
                }

                var indexContentEvent = new IndexContentEvent
                {
                    Content = content,
                    IndexItem = indexableItem,
                    Cancel = false
                };
                foreach(var contentIndexStrategy in contentTypeConfig.IndexStrategies.Concat(indexConfig.IndexStrategies))
                {
                    var strategy = IndexStrategyResolver.GetContentIndexStrategy(contentIndexStrategy);
                    strategy.Execute(indexContentEvent);

                    if (indexContentEvent.Cancel)
                        break;
                }

                if (!indexContentEvent.Cancel)
                    indexer.Index(indexConfig.Alias, indexableItem);
            }
        }

        public void Remove(IContentBase content)
        {
            var config = ConfigProvider.GetRootConfig();

            var indexer = LoadIndexer(config);
            
            foreach(var indexAlias in config.Indexes)
            {
                var indexConfig = ConfigProvider.GetIndexConfig(indexAlias);
                indexer.Remove(indexConfig.Alias, content.GetContentType().Alias, content.Id);
            }
        }

        public void ClearIndex(string indexName)
        {
            var config = ConfigProvider.GetRootConfig();

            var indexer = LoadIndexer(config);

            indexer.Clear(indexName);
        }

        protected IIndexableItem GetIndexableItem(Index indexConfig, ContentType contentTypeConfig, IContentBase content)
        {
            var itemToIndex = new IndexableItem(content.Id, content.Name, content.GetContentType().Alias);

            Dictionary<string, object> fieldGroups = new Dictionary<string, object>();

            var propertiesToIndex = contentTypeConfig.IncludeAllProperties
                ? content.Properties.ToList()
                : content.Properties.Where(p => contentTypeConfig.Properties.Select(c => c.Alias).Contains(p.Alias));
            
            foreach(var property in propertiesToIndex)
            {
                var indexField = new IndexFieldEvent
                {
                    UmbracoProperty = property,
                    Value = property.Value
                };

                var propertyConfig = contentTypeConfig.Properties.FirstOrDefault(p => p.Alias == property.Alias);

                //Execute the property Index Strategy if one is configured
                if(propertyConfig != null && !string.IsNullOrEmpty(propertyConfig.IndexStrategy))
                {
                    var strategy = IndexStrategyResolver.GetPropertyIndexStrategy(propertyConfig.IndexStrategy);

                    if (strategy != null)
                        strategy.Execute(indexField);
                }

                //Add the value to a group field if one is configured
                if(propertyConfig != null && !string.IsNullOrEmpty(propertyConfig.Group))
                {
                    if(fieldGroups.ContainsKey(propertyConfig.Group))
                    {
                        fieldGroups[propertyConfig.Group] += $" {indexField.Value}";
                    }
                    else
                    {
                        fieldGroups.Add(propertyConfig.Group, indexField.Value);
                    }
                }

                if(propertyConfig == null || (propertyConfig != null && !propertyConfig.ExcludeField))
                    itemToIndex.AddOrUpdate(property.Alias, indexField.Value);
            }

            foreach(var fieldGroup in fieldGroups)
            {
                itemToIndex.AddOrUpdate(fieldGroup.Key, fieldGroup.Value);
            }

            return itemToIndex;
        }
        
        public IIndexer LoadIndexer(DexterConfig config)
        {
            var type = TypeResolver.ResolveType<IIndexer>(config.Indexer.Type.Assembly, config.Indexer.Type.Type);

            var constructors = type.GetConstructors();

            if (!constructors.Any())
                return (IIndexer)Activator.CreateInstance(type, config.Indexer.Settings);

            var constructor = constructors.FirstOrDefault();

            var parameters = constructor.GetParameters();

            if (!parameters.Any())
                return (IIndexer)Activator.CreateInstance(type, config.Indexer.Settings);

            var typedSettings = config.Indexer.Settings.ToObject(parameters.First().ParameterType);
            return (IIndexer)Activator.CreateInstance(type, typedSettings);
        }
    }
}
