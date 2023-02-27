using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPProvider.Event
{
    public class OnUploadProgress : EventArgs
    {
        public OnUploadProgress(FileInfo file, double progress)
        {
            this.File = file;
            this.Progress = progress;
        }

        public FileInfo File { get; private set; }
        public double Progress { get; private set; }
    }
}
