using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.Config
{
    public class DexterConfig
    {
        [JsonProperty("indexer")]
        public IndexerConfig Indexer { get; set; }

        [JsonProperty("indexes")]
        public List<string> Indexes { get; set; }

        [JsonProperty("field-index-strategies")]
        public List<IndexStrategy> FieldIndexStrategies { get; set; }

        [JsonProperty("content-index-strategies")]
        public List<IndexStrategy> ContentIndexStrategies { get; set; }
    }
}
