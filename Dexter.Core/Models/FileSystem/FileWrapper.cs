using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Models.FileSystem
{
    public class FileWrapper
    {
        public FileInfo File { get; set; }
        public bool Exists => File.Exists;

        public FileWrapper(FileInfo file)
        {
            File = file;
        }

        public byte[] ReadAllBytes()
        {
            return System.IO.File.ReadAllBytes(File.FullName);
        }

        public string ReadAllText()
        {
            return System.IO.File.ReadAllText(File.FullName);
        }

        public T ReadAsJson<T>(params JsonConverter[] jsonConverters)
        {
            var json = ReadAllText();
            return JsonConvert.DeserializeObject<T>(json, converters : jsonConverters);
        }
    }
}
