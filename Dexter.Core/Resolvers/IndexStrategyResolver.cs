using Dexter.Core.Interfaces;
using Dexter.Core.Models.Config;
using Dexter.Core.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Resolvers
{
    public class IndexStrategyResolver
    {
        protected static Dictionary<string, IPropertyIndexStrategy> PropertyIndexStrategies { get; set; }
        protected static Dictionary<string, IContentIndexStrategy> ContentIndexStrategies { get; set; }

        protected DexterConfig Config { get; }

        public IndexStrategyResolver(DexterConfig config)
        {
            Config = config;
            PropertyIndexStrategies = new Dictionary<string, IPropertyIndexStrategy>();
            ContentIndexStrategies = new Dictionary<string, IContentIndexStrategy>();
        }

        public IPropertyIndexStrategy GetPropertyIndexStrategy(string alias)
        {
            if (PropertyIndexStrategies.ContainsKey(alias))
                return PropertyIndexStrategies[alias];

            var indexStrategyConfig = Config.FieldIndexStrategies.FirstOrDefault(s => s.Alias == alias);

            var indexStrategy = TypeResolver.ResolveTypeAndConstruct<IPropertyIndexStrategy>(indexStrategyConfig.Assembly, indexStrategyConfig.Type);

            PropertyIndexStrategies.Add(alias, indexStrategy);

            return indexStrategy;
        }

        public IContentIndexStrategy GetContentIndexStrategy(string alias)
        {
            if (ContentIndexStrategies.ContainsKey(alias))
                return ContentIndexStrategies[alias];

            var indexStrategyConfig = Config.ContentIndexStrategies.FirstOrDefault(s => s.Alias == alias);

            var indexStrategy = TypeResolver.ResolveTypeAndConstruct<IContentIndexStrategy>(indexStrategyConfig.Assembly, indexStrategyConfig.Type);

            ContentIndexStrategies.Add(alias, indexStrategy);

            return indexStrategy;
        }
    }
}
