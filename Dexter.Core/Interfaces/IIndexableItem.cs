using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Interfaces
{
    public interface IIndexableItem
    {
        int Id { get; }
        string Type { get; }

        DateTime? TimeToLive { get; set; }

        void AddOrUpdate<T>(string key, T obj);
        void Remove(string key);
        Dictionary<string, object> ToDictionary();
    }
}
