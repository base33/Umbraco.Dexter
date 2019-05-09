using Dexter.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.AzureSearchIndexer
{
    public class AzureSearchIndexer : IIndexer
    {
        public void Index(string indexName, IIndexableItem item)
        {
            throw new NotImplementedException();
        }

        public void Remove(string indexName, string type, int id)
        {
            throw new NotImplementedException();
        }

        public void Clear(string indexName)
        {
            throw new NotImplementedException();
        }

        public int GetNumberOfDocumentsStored(string indexName)
        {
            throw new NotImplementedException();
        }
    }
}
