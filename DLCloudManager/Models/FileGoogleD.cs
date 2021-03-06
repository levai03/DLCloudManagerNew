﻿using System.Windows;
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
                temp = new Local("/img/Folder.png", "", file.Name, "", "dir", "");
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
        public string SelectContentType(string extension)
        {
            string uploadedType;
            switch (extension)
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
            return uploadedType;
        }
        public void Upload(Local uploadFile, string parentID)
        {
            string uploadedType = SelectContentType(uploadFile.ExtensionOfLocal);

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = uploadFile.NameOfLocal + "." + uploadFile.ExtensionOfLocal,
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = new System.IO.FileStream(uploadFile.PathOfLocal, System.IO.FileMode.Open))
            {
                request = service.Files.Create(fileMetadata, stream, uploadedType);
                request.Fields = "id, parents";
                request.Upload();
            }
            var file = request.ResponseBody;
            if (!parentID.Equals("root") && file != null)
            {
                Move(file.Id, parentID, file.Parents.First());
            }

        }
        public void Move(string fileId, string folderID, string prevFolderId)
        {
            Google.Apis.Drive.v3.FilesResource.UpdateRequest updateRequest = service.Files.Update(new Google.Apis.Drive.v3.Data.File(), fileId);
            updateRequest.Fields = "id, parents";
            updateRequest.AddParents = folderID;
            updateRequest.RemoveParents = prevFolderId;
            updateRequest.Execute();

        }
        public void Copy(string fileId, string folderId, string sourceName)
        {
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
        public void Download(string pathOfTargetDir, string name, string downloadedID)
        {
            var request = service.Files.Get(downloadedID);
            var stream = new System.IO.MemoryStream();
            string newName = pathOfTargetDir + "\\" + name;
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Completed:
                        {
                            SaveDownloadedFile(stream, newName);
                            break;
                        }
                    case Google.Apis.Download.DownloadStatus.Failed:
                        {
                            MessageBox.Show("The download failed: " + name);
                            break;
                        }
                }
            };
            request.Download(stream);

        }
        private void SaveDownloadedFile(System.IO.MemoryStream stream, string path)
        {
            using (System.IO.FileStream file = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }
        public void Update(string fileID, string filePath, string extensionOfLocal, ref bool workError)
        {
            string uploadType = SelectContentType(extensionOfLocal);
            try
            {
                Google.Apis.Drive.v3.Data.File SelectedFile = service.Files.Get(fileID).Execute();
                byte[] byteArray = System.IO.File.ReadAllBytes(filePath);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);
                FilesResource.UpdateMediaUpload request = service.Files.Update(SelectedFile, fileID, stream, uploadType);
                request.Upload();
            }
            catch (Exception e)
            {
                workError = true;
                MessageBox.Show("Upload Failed: " + e.Message);
            }
        }
        public void Rename(string fileID, string fileName, string extensionOfLocal, string newName)
        {
            string uploadType = SelectContentType(extensionOfLocal);
            try
            {
                Google.Apis.Drive.v3.Data.File SelectedFile = service.Files.Get(fileID).Execute();
                SelectedFile.Name = newName + "." + extensionOfLocal;
                FilesResource.UpdateRequest request = service.Files.Update(SelectedFile,fileID);
                request.Execute();
            }
            catch (Exception e)
            {
                MessageBox.Show("Rename of the file is failed: " + fileName);
            }
        }
        public void Delete(string fileID, string  fileName, ref bool workError)
        {
            try
            {
                service.Files.Delete(fileID).Execute();
            }
            catch (Exception e)
            {
                workError = true;
                MessageBox.Show("Delete of the file is failed: " + fileName);
            }
        }
        public string CreateFolder(string newFolderName, string actualDirID)
        {
            List<Local> preTemp = FileListingFull("root");
            //nincs responsebody
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = newFolderName,
                MimeType = "application/vnd.google-apps.folder"
            };
            FilesResource.CreateRequest request;
            request = service.Files.Create(fileMetadata);
            request.Fields = "id, parents";
            request.Execute();
            List<Local> postTemp = FileListingFull("root");
            string newItemID = "";
            bool newItemFound;
            foreach (var postItem in postTemp)
            {
                newItemFound = false;
                foreach (var preItem in preTemp)
                {
                    if (preItem.Id.Equals(postItem.Id)) newItemFound = true;
                }
                if (newItemFound == false)
                {
                    newItemID = postItem.Id;
                    break;
                }
            }
            if (!newItemID.Equals("")) Move(newItemID, actualDirID, "root");

            return newItemID;

        }














        public void UploadLargeFile()
        {
            MessageBox.Show("Large file uploading (>5MB) currently not available! It will be unlocked with a later patch.");
        }





        

        /// <summary>
        /// Uploader
        /// </summary>
        /// <param name="selectedItems"></param>
        /// <param name="destinationID"></param>
        public void CopyMultipleElement(List<Local> selectedItems, string destinationID, List<FileAttributes> F, ref bool workError)
        {
            //Upload
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    if (item.ExtensionOfLocal.Equals("dir"))
                    {
                        string newFolder = CreateFolder(item.NameOfLocal, destinationID);

                        string selectedItem =item.PathOfLocal + "\\" + item.NameOfLocal;
                        List<Local> dirs = new List<Local>();
                        List<Local> files = new List<Local>();
                        List<Local> recFiles = FileBasics.FullListing(ref newFolder, ref dirs, ref files, ref newFolder, F);
                        CopyMultipleElement(recFiles, newFolder, F, ref workError);
                    }
                    else
                    {
                        try
                        {
                            Upload(item, destinationID);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Dropbox Access Error: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            workError = true;
                        }
                    }

                }
            }
        }
        /// <summary>
        /// Downloader
        /// </summary>
        /// <param name="selectedItems"></param>
        /// <param name="sourceID"></param>
        /// <param name="localPath"></param>
        public void CopyMultipleElement(List<Local> selectedItems, string sourceID, string localPath, ref bool workError)
        {
            //Download
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    if (item.ExtensionOfLocal.Equals("dir"))
                    {
                        FileBasics.CreateNewDirectory(localPath, item.NameOfLocal);

                        List<Local> recFiles = FileListingFull(item.Id);
                        string newPath = localPath + "\\" + item.NameOfLocal;
                        CopyMultipleElement(recFiles,item.Id, newPath, ref workError);
                    }
                    else
                    {
                        try
                        {
                            Download(localPath,item.NameOfLocal, item.Id);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Dropbox Access Error: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            workError = true;
                        }
                    }

                }
            }
        }
        /// <summary>
        /// Copy
        /// </summary>
        /// <param name="selectedItems"></param>
        /// <param name="sourceID"></param>
        /// <param name="destinationID"></param>
        /// <param name="noLocal"></param>
        public void CopyMultipleElement(List<Local> selectedItems, string destinationID, bool noLocal, ref bool workError)
        {
            //Copy
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    if (item.ExtensionOfLocal.Equals("dir"))
                    {
                        string newFolder = CreateFolder(item.NameOfLocal, destinationID);

                        List<Local> recFiles = FileListingFull(item.Id);
                        CopyMultipleElement(recFiles, item.Id,noLocal,ref workError);
                    }
                    else
                    {
                        try
                        {
                            Copy(item.Id, destinationID,item.NameOfLocal);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Dropbox Access Error: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            workError = true;
                        }
                    }

                }
            }
        }
        
        public void DeleteMultipleElement(List<Local> selectedItems, ref bool workError)
        {
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    try
                    {
                        Delete(item.Id, item.NameOfLocal, ref workError);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Dropbox Access Error" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        workError = true;
                    }
                }
            }
        }
        public void MoveMultipleElement(List<Local> selectedItems,string sourceID, string destinationID,ref bool workError)
        {
            //csak Belül
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    try
                    {
                        Move(item.Id, destinationID, sourceID);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Dropbox Access Error" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        workError = true;
                    }
                }
            }
        }

        

        
    }
}
