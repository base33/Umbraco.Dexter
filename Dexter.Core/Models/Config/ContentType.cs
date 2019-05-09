using Newtonsoft.Json;
using System.Collections.Generic;

namespace Dexter.Core.Models.Config
{
    public class ContentType
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("index-all-properties")]
        public bool IncludeAllProperties { get; set; }

        [JsonProperty("properties")]
        public List<Property> Properties { get; set; } = new List<Property>();

        [JsonProperty("index-strategies")]
        public List<string> IndexStrategies { get; set; } = new List<string>();
    }
}