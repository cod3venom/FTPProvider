using FluentFTP;
using LLoger.Interface;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FTPProvider
{
    public class AsyncFTPProvider : AsyncAbstractResource
    {

        public AsyncFtpClient Client;
        public CancellationToken CancellationToken;
        public readonly ILLoger Logger;
        public AsyncFTPProvider(ILLoger logger)
        {
            this.Logger = logger;
        }

        public async Task Login(string host, string userName, string password)
        {
            try
            {
                this.Client = new AsyncFtpClient(host, userName, password);
                this.CancellationToken = new CancellationToken();
                await this.Client.Connect(this.CancellationToken);

                this.Init(this);
                this.Logger.LogInformation(string.Format("{0} Successfully connected to FTP {0}", host));
            }
            catch(Exception exception)
            {
                this.Logger.LogInformation(exception.ToString());
            }
        }

        public void Logout()
        {
            try
            {
                if (!this.Client.IsConnected)
                {
                    return;
                }
                this.Client.Disconnect();
                this.Logger.LogInformation("{0} Successfully Disconnected from the FTP");

            }
            catch (Exception exception)
            {
                this.Logger.LogInformation(exception.ToString());
            }
        }
    }
}
