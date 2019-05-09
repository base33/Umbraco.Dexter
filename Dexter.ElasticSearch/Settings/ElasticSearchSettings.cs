using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.ElasticSearch.Settings
{
    public class ElasticSearchSettings
    {
        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}
