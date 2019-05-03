using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DLCloudManager.Models
{
    class FileGoogleD
    {
        static string[] Scopes = { DriveService.Scope.Drive };

        DriveService service;
        

        public void Starter(string userName, string clientSecretJson)
        {
            try
            {                                           
                UserCredential credential;
                using (var stream = new FileStream(clientSecretJson, FileMode.Open, FileAccess.Read))
                {
                    string credPath = "creds.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(GoogleClientSecrets.Load(stream).Secrets,
                                                                             Scopes,
                                                                             userName,
                                                                             CancellationToken.None,
                                                                             new FileDataStore(credPath, true)).Result;
                }
                this.service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "DLCloudManager"
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Create Google Drive service failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }
        public Local ChangeFileToLocal(Google.Apis.Drive.v3.Data.File file)
        {
            Local temp;
            if (file.MimeType.Equals("application/vnd.google-apps.folder"))
            {
                temp = new Local("/img/52.png", "", file.Name, "", "dir", "");
                temp.Id = file.Id;
            }
            else
            {
                temp = new Local("", "", file.Name, file.Size.ToString(), file.FullFileExtension, file.ModifiedTime.ToString());
                temp.Id = file.Id;
            }
            return temp;
        }
        public List<Local> FileListingFull(string parentID)
        {
            FilesResource.ListRequest listRequest = this.service.Files.List();
            listRequest.Q = "'" + parentID + "' in parents";
            listRequest.Fields = "nextPageToken, files(id, name, size, fullFileExtension, modifiedTime, mimeType, parents)";
           
            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;
            
            List<Local> filelist = new List<Local>();
            filelist.Add(new Local("", "", "..", "", "", ""));
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                        filelist.Add(ChangeFileToLocal(file));
                }
            }
            
            return filelist;
        }
        public List<Local> Navigation(Local selectedItem, ref List<CloudType> pathIDs, List<Local> currentList)
        {
            if (selectedItem.ExtensionOfLocal != null && selectedItem.ExtensionOfLocal.Equals("dir"))
            {
                CloudType CT = new CloudType();
                CT.ID1 = selectedItem.Id;
                CT.Name = selectedItem.NameOfLocal;
                pathIDs.Add(CT);
                return FileListingFull(selectedItem.Id);
            }
            else if (selectedItem.NameOfLocal.Equals(".."))
            {
                if (pathIDs.Count > 1)
                {
                    pathIDs.RemoveAt(pathIDs.LastIndexOf(pathIDs.Last()));
                }
                List<Local> temp = FileListingFull(pathIDs.Last().ID1);
                return temp;
            }
            else
            {
                MessageBox.Show("You need to copy the file to your computer before editing!");
                return currentList;
            }
        }
        



        public void Upload(Local uploadFile, string parentID)
        {
            string uploadedType;
            switch (uploadFile.ExtensionOfLocal)
            {
                case "xls": uploadedType = "application/vnd.ms-excel"; break;
                case "xlsx": uploadedType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; break;
                case "xml": uploadedType = "text/xml"; break;
                case "ods": uploadedType = "application/vnd.oasis.opendocument.spreadsheet"; break;
                case "csv": 
                case "tmpl":
                case "txt": uploadedType = "text/plain"; break;
                case "pdf": uploadedType = "application/pdf"; break;
                case "php": uploadedType = "application/x-httpd-php"; break;
                case "jpg": uploadedType = "image/jpeg"; break;
                case "png": uploadedType = "image/png"; break;
                case "gif": uploadedType = "image/gif"; break;
                case "bmp": uploadedType = "image/bmp"; break;
                case "doc": uploadedType = "application/msword"; break;
                case "js": uploadedType = "text/js"; break;
                case "swf": uploadedType = "application/x-shockwave-flash"; break;
                case "mp3": uploadedType = "audio/mpeg"; break;
                case "zip": uploadedType = "application/zip"; break;
                case "rar": uploadedType = "application/rar"; break;
                case "tar": uploadedType = "application/tar"; break;
                case "arj": uploadedType = "application/arj"; break;
                case "cab": uploadedType = "application/cab"; break;
                case "html":
                case "htm": uploadedType = "text/html"; break;
                default:
                    uploadedType = "application / octet - stream";
                    break;
            }

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = uploadFile.NameOfLocal,
                Size = long.Parse(uploadFile.SizeOfLocal)

            };

            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(uploadFile.PathOfLocal, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, uploadedType);
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;

            Console.WriteLine(file.Id);
        }
        public void Download(string path, string name, string parentID)
        {

        }
        public void Copy(string fileId, string folderId, string sourceName)
        {/*
            // Retrieve the existing parents to remove
            Google.Apis.Drive.v3.FilesResource.GetRequest getRequest = service.Files.Get(fileId);
            getRequest.Fields = "parents";
            Google.Apis.Drive.v3.Data.File file = getRequest.Execute();

            // Copy the file to the new folder
            Google.Apis.Drive.v3.FilesResource.UpdateRequest updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), fileId);
            updateRequest.Fields = "id, parents";
            updateRequest.AddParents = folderId;
            //updateRequest.RemoveParents = previousParents;
            file = updateRequest.Execute();*/
            Google.Apis.Drive.v3.Data.File copiedFile = new Google.Apis.Drive.v3.Data.File();
            copiedFile.Name = sourceName + " masolat";
            try
            {
                service.Files.Copy(copiedFile, fileId).Execute();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

        }
        public void CopyMultipleElement(List<Local> selectedItems, string destinationID)
        {
            //Upload
        }
        public void CopyMultipleElement(List<Local> selectedItems, string sourceID, string localPath)
        {
            //Download
        }
        public void CopyMultipleElement(List<Local> selectedItems, string sourceID, string destinationID, bool noLocal)
        {
            //Copy
        }
        public void Delete()
        {

        }
        public void Rename()
        {

        }
        public void CreateFolder()
        {

        }
        public void Search()
        {

        }
       

    }
}
