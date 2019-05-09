using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.Config
{
    public class IndexerConfig
    {
        [JsonProperty("type")]
        public AssemblyTypeReference Type { get; set; }

        [JsonProperty("settings")]
        public JToken Settings { get; set; }
    }
}
