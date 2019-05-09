using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.IndexStrategy
{
    public class IndexFieldEvent
    {
        public Umbraco.Core.Models.Property UmbracoProperty { get; set; }
        public object Value { get; set; }
        public bool Cancel { get; set; }
    }
}
