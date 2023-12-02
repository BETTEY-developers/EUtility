using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace EUtility.WinUI.Dialogs.File
{
    public interface IFileResult
    {
        StorageFile File { get; }

        Stream AsRawReadStream();
        Stream AsRawWriteStream();

        StreamReader AsReadStream();
        StreamWriter AsWriteStream();
    }
}
