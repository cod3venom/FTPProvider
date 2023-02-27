using FluentFTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTPProvider.AbstractResource
{
    public class Directory
    {
        private readonly AsyncFTPProvider _ftpProvider;
        public Directory(AsyncFTPProvider ftpProvider)
        {
            this._ftpProvider = ftpProvider;
        }

        public async Task<bool> Exists(string path)
        {
            return await this._ftpProvider.Client.DirectoryExists(path); ;
        }
        public async Task Create(string path)
        {
            try
            {
                await this._ftpProvider.Client.CreateDirectory(path);
                this._ftpProvider.Logger.LogInformation(string.Format("Successfully created {0} directory", path));
            }
            catch(Exception exception)
            {
                this._ftpProvider.Logger.LogError(exception.ToString());
            }
        }
    }
}
