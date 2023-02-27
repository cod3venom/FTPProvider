using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPProvider.Event
{
    public class OnFileUploadFailed : EventArgs
    {
        public OnFileUploadFailed(FileInfo file, Exception exception)
        {
            this.File = file;
            this.Exception = exception;
        }

        public FileInfo File { get; private set; }
        public Exception Exception { get; private set; }
    }
}
