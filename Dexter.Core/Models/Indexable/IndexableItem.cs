using Dexter.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Dexter.Core.Models.Indexable
{
    public class IndexableItem : IIndexableItem
    {
        protected IContent Content { get; set; }
        protected Dictionary<string, object> Columns { get; set; }
        private string type = null;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get { return !string.IsNullOrEmpty(type) ? type : "Unknown"; } set { type = value; } }

        public DateTime? TimeToLive { get; set; }

        public IndexableItem(int id, string name, string type)
        {
            Id = id;
            Name = name;
            Type = type;
            Columns = new Dictionary<string, object>();
        }

        public void AddOrUpdate<T>(string key, T obj)
        {
            if(Columns.ContainsKey(key))
            {
                Columns[key] = obj;
            }
            else
            {
                Columns.Add(key, obj);
            }
        }

        public void Remove(string key)
        {
            Columns.Remove(key);
        }

        public Dictionary<string, object> ToDictionary()
        {
            var item = new Dictionary<string, object>()
            {
                { "name", Name }
            };

			if (TimeToLive.HasValue)
			{
                item.Add("_dexter_ttl", TimeToLive.Value);
            }

            return item.Concat(Columns).ToDictionary(kv => kv.Key, kv => kv.Value);
        }
    }
}
