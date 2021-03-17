using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Dexter.Core.Provider;
using Dexter.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using Umbraco.Core.Services;
using Dexter.Core.Models.Config;
using Umbraco.Core.Models;
using Elasticsearch.Net;
using System.Configuration;
using Newtonsoft.Json;
using Dexter.Core.Resolvers;
using Dexter.Core.Models.Query;

namespace Dexter.Core.Controllers.Api
{
    public class DexterBackofficeApiController : UmbracoAuthorizedApiController
    {
        protected DexterConfigProvider ConfigProvider;

        public DexterBackofficeApiController()
        {
            var fileSystemService = new FileSystemService(new DirectoryInfo(HttpContext.Current.Server.MapPath("~")));
            ConfigProvider = new DexterConfigProvider(fileSystemService);
        }

        private ElasticLowLevelClient LoadClient()
        {
            var uri = new Uri(ConfigurationManager.AppSettings["ElasticSearchConnection"]);
            var config = new ConnectionConfiguration(uri);
            return new ElasticLowLevelClient(config);
        }

        private string GetRangeQuery()
		{
            var rangeQuery = new RangeQueryWrapper();
            rangeQuery.Query.Range.DexterTTL.Boost = 1.0;
            rangeQuery.Query.Range.DexterTTL.Format = "yyyy-MM-dd";
            rangeQuery.Query.Range.DexterTTL.Lt = DateTime.Now.ToString("yyyy-MM-dd");
            var json = JsonConvert.SerializeObject(rangeQuery);
            return json;
        }


        [HttpGet]
        public void UpdateTTLDocuments()
        {
            var client = LoadClient();
            var query = GetRangeQuery();
            var indexes = GetIndexes();
            foreach (var index in indexes)
            {
                var indexName = IndexResolver.GetIndexName(index.Name);

                //var results = client.Search<BytesResponse>(indexName, json);
                //var response = Encoding.UTF8.GetString(results.Body);

                //do this once query is confirmed and working
                var results = client.DeleteByQuery<BytesResponse>(indexName, query);
                var response = Encoding.UTF8.GetString(results.Body);
            }
        }


        [HttpGet]
        public void Reindex(string index)
        {
            var config = ConfigProvider.GetIndexConfig(index);

            var indexService = new IndexService(ConfigProvider);

            indexService.ClearIndex(config.Alias);

            if(config.ContentTypes.Count > 0)
            {
                ReindexContent(config, indexService, index);
            }

            if (config.MediaTypes.Count > 0)
            {
                ReindexMedia(config, indexService, index);
            }
        }

        private void ReindexContent(Index config, IndexService indexService, string index)
        {
            var contentTypes = Services.ContentTypeService.GetAllContentTypes().ToArray();
            foreach (var contentType in config.ContentTypes)
            {
                var contentTypeId = contentTypes?.FirstOrDefault(c => c.Alias == contentType.Alias)?.Id ?? 0;

                if (!string.IsNullOrWhiteSpace(contentType.Alias) && contentTypeId == 0)
                    continue;

                var contents = Services.ContentService.GetContentOfContentType(contentTypeId).Where(c => c.Published);

                if (string.IsNullOrWhiteSpace(contentType.Alias))
                    contents = GetNodesByContentTypes(
                        contentTypes.Except(
                                contentTypes.Where(ct => config.ContentTypes.Select(c => c.Alias).Contains(ct.Alias))
                            ).Select(x => x.Id));

                foreach (var content in contents)
                {
                    indexService.Index(content, Models.Indexable.Source.Content, index);
                }
            }
        }

        private void ReindexMedia(Index config, IndexService indexService, string index)
        {
            var mediaTypes = Services.ContentTypeService.GetAllMediaTypes().ToArray();
            foreach (var mediaType in config.MediaTypes)
            {
                var mediaTypeId = mediaTypes?.FirstOrDefault(c => c.Alias == mediaType.Alias)?.Id ?? 0;

                if (mediaTypeId == 0)
                    continue;

                var medias = Services.MediaService.GetMediaOfMediaType(mediaTypeId);

                foreach (var media in medias)
                {
                    indexService.Index(media, Models.Indexable.Source.Media, index);
                }
            }
        }

        public IEnumerable<Models.Backoffice.Index> GetIndexes(string indexName = "")
        {
            var config = ConfigProvider.GetRootConfig();
            var indexService = new IndexService(ConfigProvider);
            var indexer = indexService.LoadIndexer(config);

            var indexes = new List<Models.Backoffice.Index>();

            foreach(var name in config.Indexes)
            {
                if (indexName != "" && name != indexName)
                    continue;

                var indexConfig = ConfigProvider.GetIndexConfig(name);

                var index = new Models.Backoffice.Index();
                index.Name = name;
                index.DocumentsIndexed = indexer.GetNumberOfDocumentsStored(indexConfig.Alias);
                indexes.Add(index);
            }

            return indexes;
        }

        private IEnumerable<IContent> GetNodesByContentTypes(IEnumerable<int> contentTypes)
        {
            var contentNodes = new List<IContent>();
            foreach (var id in contentTypes)
            {
                var nodes = Services.ContentService.GetContentOfContentType(id).Where(c => c.Published);
                contentNodes.AddRange(nodes);
            }
            return contentNodes;
        }
    }
}
