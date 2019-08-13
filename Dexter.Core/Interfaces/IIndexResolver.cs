using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Interfaces
{
    public interface IIndexResolver
    {
        string GetIndexName(string alias);
    }
}
