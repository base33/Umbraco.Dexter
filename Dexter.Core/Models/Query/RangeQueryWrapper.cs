using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.Query
{
    public partial class RangeQueryWrapper
    {
        public RangeQueryWrapper()
        {
            Query = new RangeQuery();
        }

        [JsonProperty("query")]
        public RangeQuery Query { get; set; }
    }
}
