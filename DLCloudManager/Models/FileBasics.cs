using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DLCloudManager.Models
{
    class FileBasics
    {
        //Metódusok a kilistázáshoz
        static public List<Local> DirListing(ref string path, List<FileAttributes> f, string actualdir)
        {
            //Section 1
            DirectoryInfo[] directories;
            DirectoryInfo dirInfo = new DirectoryInfo(path);

            try
            {
                directories = dirInfo.GetDirectories();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access Denied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                directories = new DirectoryInfo(actualdir).GetDirectories();
                path = actualdir;
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                directories = new DirectoryInfo(actualdir).GetDirectories();
                path = actualdir;
            }
            List<Local> dirlist = new List<Local>();
            //Section 2
            foreach (DirectoryInfo a in directories)
            {
                Boolean skipDir = false;
                foreach (FileAttributes attr in f)
                {
                    if ((a.Attributes & attr) == attr)
                    {
                        skipDir = true;
                    }
                }
                if (skipDir == false)
                {
                    dirlist.Add(new Local("/img/Folder.png", a.FullName, a.Name, "", "dir", ""));
                }
            }
            return dirlist;
        }
        /// <summary>
        /// Works like DirListing, but for files
        /// </summary>
        /// <param name="path"></param>
        /// <param name="f"></param>
        /// <param name="actualdir"></param>
        /// <returns></returns>
        static public List<Local> FileListing(ref string path, List<FileAttributes> f, string actualdir)
        {

            FileInfo[] files;
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            try
            {
                files = dirInfo.GetFiles();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Access Denied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                files = new DirectoryInfo(actualdir).GetFiles();
                path = actualdir;
            }
            catch (Exception)
            {
                MessageBox.Show("Wrong Path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                files = new DirectoryInfo(actualdir).GetFiles();
                path = actualdir;
            }
            List<Local> filelist = new List<Local>();
            foreach (FileInfo a in files)
            {
                Boolean skipDir = false;
                foreach (FileAttributes attr in f)
                {
                    if ((a.Attributes & attr) == attr)
                    {
                        skipDir = true;
                    }
                }
                if (skipDir == false)
                {
                    filelist.Add(new Local("", a.FullName, a.Name, a.Length.ToString(), a.Extension, a.LastWriteTime.ToString()));
                }
            }
            return filelist;
        }
        static public List<Local> FullListing(ref string actualDir, ref List<Local> directories, ref List<Local> files, ref string prevDir, List<FileAttributes> F)
        {
            //Összeállítjuk a megjelenítendő listát
            string path = actualDir;
            directories = FileBasics.DirListing(ref path, F, prevDir);
            files = FileBasics.FileListing(ref path, F, prevDir);
            actualDir = path;
            List<Local> tempList = new List<Local>();
            tempList.Add(new Local("", "", "..", "", "", ""));
            foreach (Local a in directories)
            {
                tempList.Add(a);
            }
            foreach (Local a in files)
            {
                tempList.Add(a);

            }
            prevDir = actualDir;
            return tempList;
        }
        

        //Navigálás a fájlok között
        static public void Navigation(Local selectedItem, ref string actualDir, ref string prevDir, ref List<Local> directories, ref List<Local> files, List<FileAttributes> F,ref List<Local> FilesAndDirectories)
        {
            //Könyvtárak közti navigációért felelős metódus
            string s = selectedItem.NameOfLocal;
            if (s.Equals(".."))
            {
                //szülő könyvtárba mozgás
                if (null != Directory.GetParent(actualDir))
                {
                    actualDir = Directory.GetParent(actualDir).ToString();
                    FilesAndDirectories = FullListing(ref actualDir, ref directories, ref files, ref prevDir, F);
                }
            }
            else if (selectedItem.ExtensionOfLocal.Equals("dir"))
            {
                //alkönyvtárba mozgás
                Local dir = directories.Find(x => x.NameOfLocal.Equals(s));
                actualDir = dir.PathOfLocal;
                FilesAndDirectories = FullListing(ref actualDir, ref directories, ref files, ref prevDir, F);
            }
            else if (selectedItem.ExtensionOfLocal.Equals("drive"))
            {
                
                    actualDir = selectedItem.NameOfLocal;
                    FilesAndDirectories = FullListing(ref actualDir, ref directories, ref files, ref prevDir, F);
                
            }
            else
            {
                //fájl futtatás
                Local file = files.Find(x => x.NameOfLocal.Equals(s));
                try
                {
                    Process.Start(file.PathOfLocal);
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    MessageBox.Show("There is no application for this extension: " + file.ExtensionOfLocal);
                }
            }
        }


        //Fájlműveletek
        static public void Copy(string source, string name, string destination, Boolean overwriteable)
        {

            string target = System.IO.Path.Combine(destination, name); //új elérési út
            FileInfo fi = new FileInfo(source);
            if (fi.Attributes.HasFlag(FileAttributes.Directory))   //ha a másolandó elem könyvtár
            {
                if (!System.IO.Directory.Exists(target))
                {
                    System.IO.Directory.CreateDirectory(target);
                }
                else if (overwriteable == false)
                {
                    int i = 1;
                    string temptarget = target;
                    while (System.IO.Directory.Exists(temptarget))
                    {
                        temptarget = target;
                        DirectoryInfo tempdir = new DirectoryInfo(temptarget);
                        string tempname = tempdir.Name + "(" + i + ")";
                        temptarget = System.IO.Path.Combine(destination, tempname);
                        i++;
                    }
                    target = temptarget;
                    System.IO.Directory.CreateDirectory(target);
                }
                DirectoryInfo dirInfo = new DirectoryInfo(source);
                DirectoryInfo[] subDirs = dirInfo.GetDirectories();
                foreach (DirectoryInfo d in subDirs)
                {
                    Copy(d.FullName, d.Name, target, overwriteable); //alkönyvtárak miatti rekurzió
                }
                string[] files = System.IO.Directory.GetFiles(source);
                foreach (string s in files)
                {
                    Copy(s, System.IO.Path.GetFileName(s), target, overwriteable);
                }

            }
            else
            {
                if (overwriteable == false)
                {
                    int i = 1;
                    string temptarget = target;
                    while (System.IO.File.Exists(temptarget))
                    {
                        temptarget = target;
                        string tempname = Path.GetFileNameWithoutExtension(temptarget) + "(" + i + ")" + Path.GetExtension(temptarget);
                        temptarget = System.IO.Path.Combine(destination, tempname);
                        i++;
                    }
                    target = temptarget;
                }

                System.IO.File.Copy(source, target, overwriteable); //fájl másolás

            }

        }
        static public void CopyMultipleElements(List<Local> selectedSourceItems, List<Local> destinationItems, string destinationDir)
        {
            bool overwriteable = true;
            bool existingElement = false;
            //megnézzük léteznek e már az állományok a célnál
            foreach (Local s in selectedSourceItems)
            {
                foreach (Local d in destinationItems)
                {
                    if (s.NameOfLocal.Equals(d.NameOfLocal))
                    {
                        existingElement = true;
                    }
                }
            }
            if (existingElement == true)
            {
                MessageBoxResult r = MessageBox.Show("Do you want to overwrite the existing files?", "Warning: File already exists!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (r == MessageBoxResult.Yes)
                {
                    overwriteable = true;
                }
                else
                {
                    overwriteable = false;
                }
            }
            //másoljuk a kiválasztott elemeket
            foreach (Local item in selectedSourceItems)
            {
                try
                {
                    FileBasics.Copy(item.PathOfLocal, item.NameOfLocal, destinationDir, overwriteable);
                }

                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access Denied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        static public void Move(string source, string name, string destination, Boolean overwriteable)
        {

            string target = System.IO.Path.Combine(destination, name);
            FileInfo fi = new FileInfo(source);
            if (fi.Attributes.HasFlag(FileAttributes.Directory))
            {
                if (System.IO.Directory.Exists(target) && overwriteable)
                {
                    DirectoryInfo tempdir = new DirectoryInfo(target);
                    DirectoryInfo tempsource = new DirectoryInfo(source);
                    tempdir.Delete(true);

                    System.IO.Directory.CreateDirectory(target);
                    DirectoryInfo[] tempdirlist = tempsource.GetDirectories();
                    foreach (DirectoryInfo i in tempdirlist)
                    {
                        Move(i.FullName, i.Name, target, overwriteable);
                    }
                    FileInfo[] tempfilelist = tempsource.GetFiles();
                    foreach (FileInfo f in tempfilelist)
                    {
                        Move(f.FullName, f.Name, target, overwriteable);
                    }
                    tempsource.Delete(true);

                }
                else if (!System.IO.Directory.Exists(target))
                {
                    System.IO.Directory.CreateDirectory(target);
                    DirectoryInfo tempdir = new DirectoryInfo(target);
                    DirectoryInfo tempsource = new DirectoryInfo(source);
                    DirectoryInfo[] tempdirlist = tempsource.GetDirectories();
                    foreach (DirectoryInfo i in tempdirlist)
                    {
                        Move(i.FullName, i.Name, target, overwriteable);
                    }
                    FileInfo[] tempfilelist = tempsource.GetFiles();
                    foreach (FileInfo f in tempfilelist)
                    {
                        Move(f.FullName, f.Name, target, overwriteable);
                    }
                    tempsource.Delete(true);
                }

            }
            else
            {
                if (System.IO.File.Exists(target) && overwriteable)
                {
                    try
                    {
                        FileInfo tempfile = new FileInfo(target);
                        tempfile.Delete();
                        System.IO.File.Move(source, target);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Access Denied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else if (!System.IO.File.Exists(target))
                {
                    try
                    {
                        System.IO.File.Move(source, target);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Access Denied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            }
        }
        static public void MoveMultipleElements(List<Local> selectedSourceItems, List<Local> destinationItems, string destinationDir)
        {
            bool overwriteable = false;
            bool existingElement = false;
            foreach (Local s in selectedSourceItems)
            {
                foreach (Local d in destinationItems)
                {
                    if (s.NameOfLocal.Equals(d.NameOfLocal))
                    {
                        existingElement = true;
                    }
                }
            }
            if (existingElement == true)
            {
                MessageBoxResult r = MessageBox.Show("Do you want to overwrite the existing files?", "Warning: File already exists!", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                if (r == MessageBoxResult.Yes)
                {
                    overwriteable = true;
                }
                else
                {
                    overwriteable = false;
                }
            }
            foreach (Local item in selectedSourceItems)
            {
                try
                {
                    FileBasics.Move(item.PathOfLocal, item.NameOfLocal, destinationDir, overwriteable);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access Denied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        static public void Delete(string source)
        {

            FileInfo fi = new FileInfo(source);
            if (fi.Attributes.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo di = new DirectoryInfo(source);
                try
                {
                    di.Delete(true);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access Denied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                try
                {
                    fi.Delete();
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access Denied", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        static public void DeleteMultipleElement(List<Local> selectedSourceItems)
        {
            MessageBoxResult r = MessageBox.Show("Do you really want to delete the selected items?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
            if (r == MessageBoxResult.Yes)
            {
                foreach (Local item in selectedSourceItems)
                {

                    FileBasics.Delete(item.PathOfLocal);
                }
            }
        }
        static public void CreateNewDirectory(string actualDirectory, string name)
        {
            string target = System.IO.Path.Combine(actualDirectory, name);
            int i = 0;
            string temptarget = target;
            while (System.IO.Directory.Exists(temptarget))
            {
                i++;
                temptarget = target + "(" + i + ")";
            }
            if (i > 0)
            {
                target = temptarget;
            }
            System.IO.Directory.CreateDirectory(target);

        }
        static public void Rename(string path, string oldname, string newname)
        {
            string originalPath = System.IO.Path.Combine(path, oldname);
            string renamedPath = System.IO.Path.Combine(path, newname);
            FileInfo fi = new FileInfo(originalPath);
            if (fi.Attributes.HasFlag(FileAttributes.Directory))
            {
                System.IO.Directory.Move(originalPath, renamedPath);
            }
            else
            {
                FileInfo ri = new FileInfo(renamedPath);
                if (!ri.Extension.Equals(fi.Extension))
                {
                    MessageBoxResult r = MessageBox.Show("Do you really want to change the file extension?", "Warning: Wrong extension", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes);
                    if (r == MessageBoxResult.Yes)
                    {
                        System.IO.File.Move(originalPath, renamedPath);
                    }
                }
                else
                {
                    System.IO.File.Move(originalPath, renamedPath);
                }

            }
        }
        static public void CreateNewTxtFile()
        {
            using (Process CreateProcess = Process.Start("Notepad.exe"))
            {   
                while (!CreateProcess.HasExited)
                {
                    CreateProcess.Refresh();
                    Thread.Sleep(2000);
                }
                CreateProcess.Close();
            
            }
        }
        static public List<Local> FindDrives()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            List<Local> driveList = new List<Local>();
            foreach (DriveInfo d in drives)
            {
                driveList.Add(new Local("", d.Name, d.Name, "", "drive", ""));

            }
            return driveList;
        }





    }
}
