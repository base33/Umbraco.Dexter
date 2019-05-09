using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Interfaces
{
    public interface ICacheProvider
    {
        void Set<T>(string key, T value);
        T Get<T>(string key);
    }
}
