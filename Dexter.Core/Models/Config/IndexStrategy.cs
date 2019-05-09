using Newtonsoft.Json;

namespace Dexter.Core.Models.Config
{
    public class IndexStrategy : AssemblyTypeReference
    {
        [JsonProperty("alias")]
        public string Alias { get; set; }
    }
}