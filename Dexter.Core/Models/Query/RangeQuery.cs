using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.Query
{
    public partial class RangeQuery
    {
        public RangeQuery()
        {
            Range = new Range();
        }

        [JsonProperty("range")]
        public Range Range { get; set; }
    }

    public partial class Range
    {
        public Range()
        {
            DexterTTL = new DexterTTL();
        }
        [JsonProperty("_dexter_ttl")]
        public DexterTTL DexterTTL { get; set; }
    }

    public partial class DexterTTL
    {
        [JsonProperty("boost")]
        public double Boost { get; set; }

        [JsonProperty("format")]
        public string Format { get; set; }

        [JsonProperty("gt", NullValueHandling = NullValueHandling.Ignore)]
        public string Gt { get; set; }

        [JsonProperty("gte", NullValueHandling = NullValueHandling.Ignore)]
        public string Gte { get; set; }

        [JsonProperty("lt", NullValueHandling = NullValueHandling.Ignore)]
        public string Lt { get; set; }

        [JsonProperty("lte", NullValueHandling = NullValueHandling.Ignore)]
        public string Lte { get; set; }
    }

}
