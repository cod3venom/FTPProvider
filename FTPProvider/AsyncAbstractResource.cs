using FTPProvider.AbstractResource;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPProvider
{
    public abstract class AsyncAbstractResource
    {
        public Directory Directory { get; private set; }
        public FileUploader FileUploader { get; private set; }

        protected void Init(AsyncFTPProvider ftpProvider)
        {
            this.Directory = new Directory(ftpProvider);
            this.FileUploader = new FileUploader(ftpProvider);
        }
    }
}
