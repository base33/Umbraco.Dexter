using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Dexter.Core.Provider;
using Dexter.Core.Services;
using Umbraco.Web;
using Umbraco.Web.WebApi;
using Umbraco.Core.Services;
using Dexter.Core.Models.Config;
using Umbraco.Core.Models;
using Elasticsearch.Net;
using System.Configuration;
using Newtonsoft.Json;
using Dexter.Core.Resolvers;
using Dexter.Core.Models.Query;

namespace Dexter.Core.Controllers.Api
{
    public class DexterApiController : UmbracoApiController
    {
        protected DexterConfigProvider ConfigProvider;

        public DexterApiController()
        {
            var fileSystemService = new FileSystemService(new DirectoryInfo(HttpContext.Current.Server.MapPath("~")));
            ConfigProvider = new DexterConfigProvider(fileSystemService);
        }

        private ElasticLowLevelClient LoadClient()
        {
            var uri = new Uri(ConfigurationManager.AppSettings["ElasticSearchConnection"]);
            var config = new ConnectionConfiguration(uri);
            return new ElasticLowLevelClient(config);
        }

        private string GetRangeQuery()
        {
            var rangeQuery = new RangeQueryWrapper();
            rangeQuery.Query.Range.DexterTTL.Boost = 1.0;
            rangeQuery.Query.Range.DexterTTL.Format = "yyyy-MM-dd";
            rangeQuery.Query.Range.DexterTTL.Lt = DateTime.Now.ToString("yyyy-MM-dd");
            var json = JsonConvert.SerializeObject(rangeQuery);
            return json;
        }


        [HttpGet]
        public IHttpActionResult UpdateTTLDocuments(string pwd)
        {
            if (pwd == "dxtttldoc")
            {
                var client = LoadClient();
                var query = GetRangeQuery();
                var indexes = GetIndexes();
                foreach (var index in indexes)
                {
                    var indexName = IndexResolver.GetIndexName(index.Name);

                    //var results = client.Search<BytesResponse>(indexName, json);
                    //var response = Encoding.UTF8.GetString(results.Body);

                    //do this once query is confirmed and working
                    var results = client.DeleteByQuery<BytesResponse>(indexName, query);
                    var response = Encoding.UTF8.GetString(results.Body);
                }

                return Json(new
                {
                    System.Net.HttpStatusCode.OK
                });
            }

            return Json(new
            {
                System.Net.HttpStatusCode.NotFound
            });
        }

        private IEnumerable<Models.Backoffice.Index> GetIndexes(string indexName = "")
        {
            var config = ConfigProvider.GetRootConfig();
            var indexService = new IndexService(ConfigProvider);
            var indexer = indexService.LoadIndexer(config);

            var indexes = new List<Models.Backoffice.Index>();

            foreach (var name in config.Indexes)
            {
                if (indexName != "" && name != indexName)
                    continue;

                var indexConfig = ConfigProvider.GetIndexConfig(name);

                var index = new Models.Backoffice.Index();
                index.Name = name;
                index.DocumentsIndexed = indexer.GetNumberOfDocumentsStored(indexConfig.Alias);
                indexes.Add(index);
            }

            return indexes;
        }
    }
}
