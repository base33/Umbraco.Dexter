using Dexter.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dexter.Core.Provider
{
    public class HttpRequestCacheProvider : ICacheProvider
    {
        public T Get<T>(string key)
        {
            return (T)HttpContext.Current.Items[key];
        }

        public void Set<T>(string key, T value)
        {
            HttpContext.Current.Items[key] = value;
        }
    }
}
