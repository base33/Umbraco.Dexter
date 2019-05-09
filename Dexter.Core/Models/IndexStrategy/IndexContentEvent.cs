using Dexter.Core.Interfaces;
using Dexter.Core.Models.Indexable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Dexter.Core.Models.IndexStrategy
{
    public class IndexContentEvent
    {
        public IContentBase Content { get; set; }
        public IIndexableItem IndexItem { get; set; }
        public bool Cancel { get; set; }
    }
}
