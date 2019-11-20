using Dexter.Core.Models.Config;
using Dexter.Core.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Converters
{
    public class ContentTypeConverter : JsonConverter
    {
        protected FileSystemService FileSystemService { get; }
        protected string RootIndexPath { get; }

        public ContentTypeConverter(FileSystemService fileSystemService, string rootIndexPath)
        {
            FileSystemService = fileSystemService;
            RootIndexPath = rootIndexPath;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ContentType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {

                var o = JObject.Load(reader);
                var filePath = o["$ref"];
                if (filePath != null)
                {
                    var file = FileSystemService.MapFile(RootIndexPath + filePath.Value<string>().Trim('#'));
                    return file.ReadAsJson<ContentType>();
                }

                return o.ToObject<ContentType>();

            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
