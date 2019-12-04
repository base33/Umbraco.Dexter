using Dexter.Core.Provider;
using Dexter.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Umbraco.Core;
using Umbraco.Core.Events;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace Dexter.Core
{
    public class StartUp : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            base.ApplicationStarted(umbracoApplication, applicationContext);

            Umbraco.Core.Services.ContentService.Published += ContentService_Published;
            Umbraco.Core.Services.ContentService.UnPublished += ContentService_UnPublished;
            Umbraco.Core.Services.ContentService.Trashed += ContentService_Trashed;

            Umbraco.Core.Services.MediaService.Saved += MediaService_Saved;
            Umbraco.Core.Services.MediaService.Deleted += MediaService_Deleted;
            Umbraco.Core.Services.MediaService.Trashed += MediaService_Trashed;
        }

        // ADD!
        private void ContentService_Published(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            var fileSystemService = new FileSystemService(new DirectoryInfo(HttpRuntime.AppDomainAppPath));
            var configProvider = new DexterConfigProvider(fileSystemService);
            var indexService = new IndexService(configProvider);

            foreach (var entity in e.PublishedEntities)
            {
                indexService.Index(entity, Models.Indexable.Source.Content);
            }
        }

        // REMOVE!
        private void ContentService_Trashed(Umbraco.Core.Services.IContentService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IContent> e)
        {
            var fileSystemService = new FileSystemService(new DirectoryInfo(HttpRuntime.AppDomainAppPath));
            var configProvider = new DexterConfigProvider(fileSystemService);
            var indexService = new IndexService(configProvider);

            foreach (var moveInfo in e.MoveInfoCollection)
            {
                indexService.Remove(moveInfo.Entity);
            }
        }

        private void ContentService_UnPublished(Umbraco.Core.Publishing.IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<Umbraco.Core.Models.IContent> e)
        {
            var fileSystemService = new FileSystemService(new DirectoryInfo(HttpRuntime.AppDomainAppPath));
            var configProvider = new DexterConfigProvider(fileSystemService);
            var indexService = new IndexService(configProvider);

            foreach (var entity in e.PublishedEntities)
            {
                indexService.Remove(entity);
            }
        }

        private void MediaService_Saved(IMediaService sender, SaveEventArgs<IMedia> e)
        {
            var fileSystemService = new FileSystemService(new DirectoryInfo(HttpRuntime.AppDomainAppPath));
            var configProvider = new DexterConfigProvider(fileSystemService);
            var indexService = new IndexService(configProvider);

            foreach (var entity in e.SavedEntities)
            {
                indexService.Index(entity, Models.Indexable.Source.Media);
            }
        }

        private void MediaService_Deleted(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.DeleteEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            var fileSystemService = new FileSystemService(new DirectoryInfo(HttpRuntime.AppDomainAppPath));
            var configProvider = new DexterConfigProvider(fileSystemService);
            var indexService = new IndexService(configProvider);

            foreach (var entity in e.DeletedEntities)
            {
                indexService.Remove(entity);
            }
        }

        private void MediaService_Trashed(Umbraco.Core.Services.IMediaService sender, Umbraco.Core.Events.MoveEventArgs<Umbraco.Core.Models.IMedia> e)
        {
            var fileSystemService = new FileSystemService(new DirectoryInfo(HttpRuntime.AppDomainAppPath));
            var configProvider = new DexterConfigProvider(fileSystemService);
            var indexService = new IndexService(configProvider);

            foreach (var entity in e.MoveInfoCollection)
            {
                indexService.Remove(entity.Entity);
            }
        }
    }
}
