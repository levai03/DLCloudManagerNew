﻿using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DLCloudManager.Models
{
    class FileOneD
    {
        //"7139ea72-31a7-4e53-a566-f38d04e942a5"
        OneDriveClient oneDriveClient;
        Item rootItem;
        public async void Starter(string accessToken)
        {
            string[] scopes = new string[] { "onedrive.readonly", "wl.signin" };
            var msaAuthenticationProvider = new MsaAuthenticationProvider(accessToken,
            "https://login.live.com/oauth20_desktop.srf",
            scopes);
            await msaAuthenticationProvider.AuthenticateUserAsync();
            oneDriveClient = new OneDriveClient(msaAuthenticationProvider);

            Getroot();
            
        }
        async void Getroot()
        {
            rootItem = await oneDriveClient
                             .Drive
                             .Root
                             .Request()
                             .GetAsync();
        }
        async void ListTask(string parentFolder, string token)
        {/*
            var list = await oneDriveClient.Drive.Items[parentFolder].Children.Request().GetAsync();
            

            IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

            List<Local> filelist = new List<Local>();
            filelist.Add(new Local("", "", "..", "", "", ""));
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    filelist.Add(ChangeFileToLocal(file));
                }
            }*/

        
        

        }
        
        








        public List<Local> FullListing(string directoryID)
        {

            List<Local> filelist = new List<Local>();
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
                return FullListing(selectedItem.Id);
            }
            else if (selectedItem.NameOfLocal.Equals(".."))
            {
                if (pathIDs.Count > 1)
                {
                    pathIDs.RemoveAt(pathIDs.LastIndexOf(pathIDs.Last()));
                }
                List<Local> temp = FullListing(pathIDs.Last().ID1);
                return temp;
            }
            else
            {
                MessageBox.Show("You need to copy the file to your computer before editing!");
                return currentList;
            }
        }
        public async void Download(string itemID)
        {
            var contentStream = await oneDriveClient
                              .Drive
                              .Items[""]
                              .Content
                              .Request()
                              .GetAsync();
        }
        public async void CopyMultipleElement(List<Local> selectedItems, string destinationID)
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

        internal void MoveMultipleElement(List<Local> selectedItemList, string iD1)
        {
            
        }

        internal void DeleteMultipleElement()
        {
            
        }

        internal void Rename(string iD1, string nameOfLocal, string tempName)
        {/*
            var updateItem = new Item { ParentReference = new ItemReference { Id = newParentId } };
            var itemWithUpdates = await oneDriveClient
                                            .Drive
                                            .Items[itemId]
                                            .Request()
                                            .UpdateAsync(updateItem);*/
        }

        internal void CreateNewDirectory(string iD1, string tempName)
        {/*
            var folderToCreate = new Item { Folder = new Folder() };
            var createdFolder = await oneDriveClient
                      .Drive
                      .Items[itemId]
                      .ItemWithPath("folder name")
                      .Request()
                      .CreateAsync(folderToCreate);*/
        }
    }
}
