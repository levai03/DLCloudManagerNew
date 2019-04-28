using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;

namespace DLCloudManager
{
    class FileOneD
    {
        /* public async void Starter()
        {
           string[] scopes = new string[] { "onedrive.readonly", "wl.signin" };
            var msaAuthenticationProvider = new MsaAuthenticationProvider(
            "7139ea72-31a7-4e53-a566-f38d04e942a5",
            "https://login.live.com/oauth20_desktop.srf",
            scopes);
            await msaAuthenticationProvider.AuthenticateUserAsync();
            var oneDriveClient = new OneDriveClient(msaAuthenticationProvider);

            var rootItem = await oneDriveClient
                             .Drive
                             .Root
                             .Request()
                             .GetAsync();
            Console.WriteLine(rootItem.Id);
        }*/
    }
}
