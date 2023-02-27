using FTPProvider.Event;
using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace FTPProvider.AbstractResource
{
    public class FileUploader
    {

        public event EventHandler<OnUploadProgress> OnFileUploadProgress;
        public event EventHandler<OnUploadCompleted> OnFileUploadCompleted;
        public event EventHandler<OnFileUploadFailed> OnFileUploadFailed;

        private readonly AsyncFTPProvider _ftpProvider;

        private List<FileInfo> _allFiles;
        private FileInfo _currentFile;

        public FileUploader(AsyncFTPProvider ftpProvider)
        { 
            this._ftpProvider = ftpProvider;
            this._allFiles = new List<FileInfo>();
        }

        public async Task UploadFiles(List<FileInfo> files, string destination)
        {
            this._allFiles = files;
            try
            {
                this._ftpProvider.Logger.LogInformation(string.Format("Starting uploading files\n\n{0}\rto =======> {1} directory\n\n", string.Join(",\n", files.Select(file => file.Name)), destination));
                await this._ftpProvider.Client.UploadFiles(files, destination, FtpRemoteExists.Skip, progress: this.ProgressHandler(), token: this._ftpProvider.CancellationToken);
                await this._ftpProvider.Client.Disconnect();   
            }
            catch (Exception exception)
            {
                this._ftpProvider.Logger.LogError(exception);
                this.OnFileUploadFailed?.Invoke(this, new OnFileUploadFailed(this._currentFile, exception));
            }
        }

        public async Task UploadFile(FileInfo file, string destination)
        {
            try
            {
                this._currentFile = file;
                this._ftpProvider.Logger.LogInformation(string.Format("Starting uploading {0} =======> {1} directory\n\n", this._currentFile.Name, destination));
                await this._ftpProvider.Client.UploadFile(this._currentFile.FullName, destination, FtpRemoteExists.Skip, progress: this.ProgressHandler(), token: this._ftpProvider.CancellationToken);
                await this._ftpProvider.Client.Disconnect();
            }
            catch (Exception exception)
            {
                this._ftpProvider.Logger.LogError(exception);
                this.OnFileUploadFailed?.Invoke(this, new OnFileUploadFailed(this._currentFile, exception));
            }
        }
        private Progress<FtpProgress> ProgressHandler()
        {
            Progress<FtpProgress> progress = new Progress<FtpProgress>(p => {
                if (this._currentFile == null && this._allFiles.Count > 0)
                {
                    this._currentFile = this._allFiles[p.FileCount - 1];
                }

                if (this._currentFile == null) {
                    return;
                }

                if (p.Progress == 100) {
                    this._ftpProvider.Logger.LogInformation(string.Format("Uploading completed for {0} \n", this._currentFile.Name));
                    this.OnFileUploadCompleted?.Invoke(this, new OnUploadCompleted(this._currentFile, p.Progress));
                }

                else {
                    Console.Write("\r{0} - {1}% Uploaded", this._currentFile.Name, (int)p.Progress);
                    this.OnFileUploadProgress?.Invoke(this, new OnUploadProgress(this._currentFile, p.Progress));
                }
            });
            return progress;
        }
    }
}
