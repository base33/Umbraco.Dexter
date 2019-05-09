using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.Config
{
    public class Index
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("content-types")]
        public List<ContentType> ContentTypes { get; set; } = new List<ContentType>();

        [JsonProperty("media-types")]
        public List<ContentType> MediaTypes { get; set; } = new List<ContentType>();

        [JsonProperty("index-strategies")]
        public List<string> IndexStrategies { get; set; } = new List<string>();
    }
}
