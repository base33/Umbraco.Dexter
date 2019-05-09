using Newtonsoft.Json;

namespace Dexter.Core.Models.Config
{
    public class Property
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("index-strategy")]
        public string IndexStrategy { get; set; }

        [JsonProperty("exclude-field")]
        public bool ExcludeField { get; set; }
    }
}