using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace DLCloudManager
{
    class FileGoogleD
    {
       /* static string[] Scopes = { DriveService.Scope.DriveReadonly };
        static string ApplicationName = "DLCloudManager";

        DriveService service;
        
        public void Starter()
        {
            
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }
            // Create Drive API service.
            this.service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            

        }
        public List<Local> FileListing()
        {
            // Define parameters of request.
            FilesResource.ListRequest listRequest = this.service.Files.List();
            listRequest.PageSize = 10;
            listRequest.Fields = "nextPageToken, files(id, name, size, fullFileExtension, modifiedTime)";

            // List files.
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            Console.WriteLine("Files:");
            List<Local> filelist = new List<Local>();
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    filelist.Add(new Local("", "", file.Name, file.Size.ToString(), file.FullFileExtension, file.ModifiedTime.ToString()));

                    Console.WriteLine("{0} ({1})", file.Name, file.Id);
                }
            }
            else
            {
                Console.WriteLine("No files found.");
            }
            return filelist;
        }*/

    }
}
