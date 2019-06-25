using Dexter.Core.Interfaces;
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
