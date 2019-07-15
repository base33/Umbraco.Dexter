using Dexter.Core.Interfaces;
using Dexter.Core.Models.Config;
using Dexter.Core.Resolvers;
using Dexter.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Provider
{
    public class DexterConfigProvider
    {
        protected const string DEFAULTFILEPATH = "~\\config\\dexter\\dexter.config.json";
        protected const string DEFAULTINDEXFILEPATH = "~\\config\\dexter\\indexes\\";

        protected DexterConfig RootConfig { get; }
        protected FileSystemService FileSystemService { get; }
        protected ICacheProvider CacheProvider { get; }

        public DexterConfigProvider(FileSystemService fileSystemService, ICacheProvider cacheProvider = null)
        {
            RootConfig = fileSystemService.MapFile(DEFAULTFILEPATH).ReadAsJson<DexterConfig>();
            FileSystemService = fileSystemService;
            CacheProvider = CacheProvider ?? new HttpRequestCacheProvider();
        }

        public Index GetIndexConfig(string alias)
        {
            var cacheName = "DEXTER_INDEX_" + alias;
            Index indexConfig = CacheProvider.Get<Index>(cacheName);

            if (indexConfig != null)
                return indexConfig;

            indexConfig = FileSystemService.MapFile($"{DEFAULTINDEXFILEPATH}{alias}.index.json").ReadAsJson<Index>();

            var indexAllContentType = indexConfig.ContentTypes.FirstOrDefault(x => string.IsNullOrWhiteSpace(x.Alias));

            if(indexAllContentType != null)
            { 
                foreach(var type in indexConfig.ContentTypes.Where(x => !string.IsNullOrWhiteSpace(x.Alias)))
                {
                    type.Properties.AddRange(indexAllContentType.Properties);
                }
            }

            indexConfig.Alias = IndexResolver.GetIndexName(alias);

            CacheProvider.Set(cacheName, indexConfig);

            return indexConfig;
        }

        public DexterConfig GetRootConfig()
        {
            return RootConfig;
        }
    }
}
