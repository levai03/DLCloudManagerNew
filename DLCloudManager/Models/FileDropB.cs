using Dropbox.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DLCloudManager.Models
{
    class FileDropB
    {
        
        public List<Local> ListFolder(string actualDir, string token)
        {
            try
            {
                var task = Task.Run(() => ListTask(actualDir, token));
                task.Wait();
                return task.Result;
            }
            catch(Exception e)
            {
                MessageBox.Show("Dropbox Access Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
        async Task<List<Local>> ListTask(string parentFolder, string token)
        {
            List<Local> tempFileList = new List<Local>();
            tempFileList.Add(new Local("", "", "..", "", "", ""));
            using (var dbx = new DropboxClient(token))
            {
                var list = await dbx.Files.ListFolderAsync(parentFolder);

                foreach (var item in list.Entries.Where(i => i.IsFolder))
                {
                    Local f = new Local("/img/Folder.png","",item.Name,"","dir","");
                    tempFileList.Add(f);
                }
                foreach (var item in list.Entries.Where(i => i.IsFile))
                {
                    Local f = new Local("", "", item.Name, "", "", "");
                    tempFileList.Add(f);
                }
                return tempFileList;
            }

        }
        public List<Local> Navigation(ref string actualDir, string prevdir, Local selectedItem, List<Local> currentList, string token)
        {
            if (selectedItem.NameOfLocal.Equals(".."))
            {
                if (!actualDir.Equals(""))
                {
                    string[] s = actualDir.Split('/');
                    string newActualDir = s[0];
                    for (int i = 1; i < s.Length-2; i++)
                    {
                        newActualDir = newActualDir + "/" + s[i];
                    }
                    actualDir = newActualDir;
                    var templist = ListFolder(actualDir, token);
                    if (templist == null)
                    {
                        actualDir = prevdir;
                        return currentList;
                    }
                    else
                    {
                        prevdir = actualDir;
                        return templist;
                    }
                }
                else
                {
                    return currentList;
                }
                
            }
            else if (selectedItem.ExtensionOfLocal.Equals("dir"))
            {
                actualDir = actualDir + "/" + selectedItem.NameOfLocal;
                var templist = ListFolder(actualDir, token);
                int i = 1;
                if (templist == null)
                {
                    actualDir = prevdir;
                    return currentList;
                }
                else
                {
                    prevdir = actualDir;
                    return templist;
                }
            }
            else
            {
                MessageBox.Show("You need to copy the file to your computer before editing!");
                return currentList;
            }
        }
        async Task UploadTask(string path,string name, string destinationFolder, string token)
        {
            using (var dbx = new DropboxClient(token))
            {
                using (var uploadStream = new MemoryStream(File.ReadAllBytes(path + "\\" + name)))
                {
                    //150MB
                    var updatedFile = await dbx.Files.UploadAsync(destinationFolder + "/" + name, Dropbox.Api.Files.WriteMode.Overwrite.Instance, body: uploadStream);
                    //updatedFile.wa
                }
                
            }

        }
        async Task MoveTask(string fromPath, string toPath, string token)
        {
            Dropbox.Api.Files.RelocationArg tempArg = new Dropbox.Api.Files.RelocationArg(fromPath, toPath, true, true, true);
            using (var dbx = new DropboxClient(token))
            {
                var MovedFile = await dbx.Files.MoveV2Async(tempArg);
            }

        }
        async Task CopyTask(string fromPath, string toPath, string token)
        {
            Dropbox.Api.Files.RelocationArg tempArg = new Dropbox.Api.Files.RelocationArg(fromPath, toPath, true, true, true);
            using (var dbx = new DropboxClient(token))
            {
                var MovedFile = await dbx.Files.CopyV2Async(tempArg);
            }

        }
        async Task DownloadTask(Local selectedItem, string sourceDir, string destinationDir,string token)
        {
            using (var dbx = new DropboxClient(token))
            {
                string tempPath = destinationDir + "\\" + selectedItem.NameOfLocal;
                using (var response = await dbx.Files.DownloadAsync(sourceDir + "/" + selectedItem.NameOfLocal))
                {
                    var s = response.GetContentAsByteArrayAsync();
                    s.Wait();
                    var d = s.Result;
                    File.WriteAllBytes(tempPath, d);
                }
            }

        }
        async Task DeleteTask(string path, string token)
        {
            Dropbox.Api.Files.DeleteArg tempArg = new Dropbox.Api.Files.DeleteArg(path);
            using (var dbx = new DropboxClient(token))
            {
                var MovedFile = await dbx.Files.DeleteV2Async(tempArg);
            }

        }
        async Task CreateFolderTask(string path, string name, string token)
        {
            using (var dbx = new DropboxClient(token))
            {
                var CreatedFolder = await dbx.Files.CreateFolderV2Async(path + "/" + name, true);
            }

            
        }

        /// <summary>
        /// Uploader method
        /// </summary>
        /// <param name="selectedItems"></param>
        /// <param name="sourceDir"></param>
        /// <param name="destinationDir"></param>
        /// <param name="token"></param>
        /// <param name="workError"></param>
        public void CopyMultipleElementUpload(List<Local> selectedItems, string path, string destinationFolder, string token, ref bool workError)
        {
            //Upload
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    try
                    {
                        var task = Task.Run(() => UploadTask(path, item.NameOfLocal, destinationFolder, token));
                        task.Wait();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Dropbox Access Error: " + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        workError = true;
                    }
                }
            }
        }
        /// <summary>
        /// Downloader Method
        /// </summary>
        /// <param name="selectedItems"></param>
        /// <param name="sourceDir"></param>
        /// <param name="destinationDir"></param>
        /// <param name="token"></param>
        /// <param name="noLocal"></param>
        /// <param name="workError"></param>
        public void CopyMultipleElement(List<Local> selectedItems, string sourceDir, string destinationDir, string token, ref bool workError)
        {
            //Download
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    try
                    {
                        var task = Task.Run(() => DownloadTask(item, sourceDir, destinationDir, token));
                        task.Wait();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Dropbox Access Error" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        workError = true;
                    }
                }
            }
        }
        /// <summary>
        /// Copy method
        /// </summary>
        /// <param name="selectedItems"></param>
        /// <param name="sourceDir"></param>
        /// <param name="destDir"></param>
        /// <param name="token"></param>
        /// <param name="workError"></param>
        public void CopyMultipleElement(List<Local> selectedItems, string sourceDir, string destinationDir,string token, bool noLocal,ref bool workError)
        {
            //Copy
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    string fromPath = sourceDir + "/" + item.NameOfLocal;
                    string toPath = destinationDir + "/" + item.NameOfLocal;
                    try
                    {
                        var task = Task.Run(() => CopyTask(fromPath, toPath, token));
                        task.Wait();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Dropbox Access Error" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        workError = true;
                    }
                }
            }
        }
        

        internal void MoveMultipleElement(List<Local> selectedItems, string sourceDir, string destDir, string token, ref bool workError)
        {
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals(".."))
                {
                    string fromPath = sourceDir + "/" + item.NameOfLocal;
                    string toPath = destDir + "/" + item.NameOfLocal;
                    try
                    {
                        var task = Task.Run(() => MoveTask(fromPath,toPath, token));
                        task.Wait();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Dropbox Access Error" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        workError = true;
                    }
                }
            }
        }

        internal void DeleteMultipleElement(List<Local> selectedItems, string actualDir, string token, ref bool workError)
        {
            foreach (var item in selectedItems)
            {
                if (!item.NameOfLocal.Equals("..")) {
                    string fromPath = actualDir + "/" + item.NameOfLocal;
                    try
                    {
                        var task = Task.Run(() => DeleteTask(fromPath, token));
                        task.Wait();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Dropbox Access Error" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        workError = true;
                    }
                }
            }
        }

        internal void Rename(string actualDir, string oldName, string newName, string token, ref bool workError)
        {
            if (!oldName.Equals(".."))
            {
                string fromPath = actualDir + "/" + oldName;
                string toPath = actualDir + "/" + newName;
                try
                {
                    var task = Task.Run(() => MoveTask(fromPath, toPath, token));
                    task.Wait();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Dropbox Access Error" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    workError = true;
                }
            }
        }

        public void CreateNewDirectory(string actualDir, string tempName, string token, ref bool workError)
        {
            try
            {
                var task = Task.Run(() => CreateFolderTask(actualDir, tempName, token));
                task.Wait();
            }
            catch (Exception e)
            {
                MessageBox.Show("Dropbox Access Error" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                workError = true;
            }
        }
    }

    
}
