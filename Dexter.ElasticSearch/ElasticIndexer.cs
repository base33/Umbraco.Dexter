using Elasticsearch.Net;
using Dexter.Core.Interfaces;
using Dexter.Core.Models.Config;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dexter.ElasticSearch.Settings;

namespace Dexter.ElasticSearch
{
    public class ElasticIndexer : IIndexer
    {
        protected Uri Uri { get; set; }
        protected ElasticLowLevelClient Client;

        public ElasticIndexer(ElasticSearchSettings settings)
        {
            string uri = settings.Uri;
            Uri = new Uri("https://" + uri);
            Client = LoadClient();
        }

        public virtual void Index(string indexName, IIndexableItem item)
		{
			if ((item.TimeToLive.HasValue && item.TimeToLive >= DateTime.Now) || !item.TimeToLive.HasValue)
			{
				var indexResponse = Client.Index<BytesResponse>(indexName, item.Type, item.Id.ToString(), PostData.Serializable(item.ToDictionary()));
				var resp = Encoding.UTF8.GetString(indexResponse.Body);
			}
		}

		public virtual void Remove(string indexName, string type, int id)
        {
            var removeResponse = Client.Delete<BytesResponse>(indexName, type, id.ToString());
        }

        public virtual void Clear(string indexName)
        {
            var removeResponse = Client.IndicesDelete<BytesResponse>(indexName);
        }

        public virtual int GetNumberOfDocumentsStored(string indexName)
        {
            var response = Client.Count<DynamicResponse>(indexName, null);

            if (!response.Success)
                return 0;

            return response.Body.count;
        }

        protected ElasticLowLevelClient LoadClient()
        {
            var config = new ConnectionConfiguration(Uri);
            return new ElasticLowLevelClient(config);
        }
    }
}
