using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EUtility.WinUI.Dialogs.File
{
    public class FileResult : IFileResult
    {
        public StorageFile _raw_File_Object = default(StorageFile);

        public FileResult(StorageFile raw_File_Object)
        {
            _raw_File_Object = raw_File_Object;
        }

        public StorageFile File => throw new NotImplementedException();

        public Stream AsRawReadStream()
        {
            return File.OpenStreamForReadAsync().Result;
        }

        public Stream AsRawWriteStream()
        {
            return File.OpenStreamForWriteAsync().Result;
        }

        public StreamReader AsReadStream()
        {
            return new(AsRawReadStream());
        }

        public StreamWriter AsWriteStream()
        {
            return new(AsRawWriteStream());
        }
    }
}
