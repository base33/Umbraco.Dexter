using Dexter.Core.Interfaces;
using System.Web;

namespace Dexter.Core.Provider
{
    public class HttpRequestCacheProvider : ICacheProvider
    {
        public T Get<T>(string key)
        {
            if (HttpContext.Current == null)
                return default(T);
            return (T)HttpContext.Current.Items[key];
        }

        public void Set<T>(string key, T value)
        {
            if (HttpContext.Current != null)
                HttpContext.Current.Items[key] = value;
        }
    }
}
