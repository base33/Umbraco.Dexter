using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.Config
{
    public class AssemblyTypeReference
    {
        [JsonProperty("assembly")]
        public string Assembly { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
