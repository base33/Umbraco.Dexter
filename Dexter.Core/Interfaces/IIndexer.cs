using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Interfaces
{
    public interface IIndexer
    {
        void Index(string indexName, IIndexableItem item);

        void Remove(string indexName, string type, int id);

        void Clear(string indexName);

        int GetNumberOfDocumentsStored(string indexName);
    }
}
