using Dexter.Core.Models.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dexter.Core.Services
{
    public class FileSystemService
    {
        protected DirectoryInfo RootDirectory { get; }

        public FileSystemService(DirectoryInfo rootDirectory)
        {
            RootDirectory = rootDirectory;
        }

        public DirectoryInfo MapDirectory(string relDirectory)
        {
            return new DirectoryInfo(RootDirectory.Root + "\\" + relDirectory.Replace("~\\", ""));
        }

        public FileWrapper MapFile(string relFilePath)
        {
            var file = new FileInfo(RootDirectory.FullName + "\\" + relFilePath.Replace("~", "").Replace("/", "\\").TrimStart(new char[] { '\\' }));
            return new FileWrapper(file);
        }
    }
}
