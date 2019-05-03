﻿using DLCloudManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DLCloudManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //Commands
        private readonly DelegateCommand _listing1Command;
        private readonly DelegateCommand _listing2Command;
        private readonly DelegateCommand _copyCommand;
        private readonly DelegateCommand _moveCommand;
        private readonly DelegateCommand _deleteCommand;
        private readonly DelegateCommand _renameCommand;
        private readonly DelegateCommand _createDirectoryCommand;
        private readonly DelegateCommand _createTxtCommand;
        public ICommand Listing1Command => _listing1Command;
        public ICommand Listing2Command => _listing2Command;
        public ICommand CopyCommand => _copyCommand;
        public ICommand MoveCommand => _moveCommand;
        public ICommand DeleteCommand => _deleteCommand;
        public ICommand RenameCommand => _renameCommand;
        public ICommand CreateDirectoryCommand => _createDirectoryCommand;
        public ICommand CreateTxtCommand => _createTxtCommand;
        public MainViewModel()
        {
            PrevDirectory1 = "C:\\";
            ActualDirectory1 = "C:\\";
            PrevDirectory2 = "C:\\";
            ActualDirectory2 = "C:\\";
            FGD = new FileGoogleD();
            List<FileAttributes> tempAttributes = new List<FileAttributes>();
            tempAttributes.Add(FileAttributes.System);
            tempAttributes.Add(FileAttributes.Hidden);
            //tempAttributes.Add(FileAttributes.ReadOnly);
            F = tempAttributes;
            FilesAndDirectories1 = FileBasics.FullListing(ref actualDirectory1, ref directories1, ref files1, ref prevDirectory1, F);
            FilesAndDirectories2 = FileBasics.FullListing(ref actualDirectory2, ref directories2, ref files2, ref prevDirectory2, F);
            normal = true;
            normal2 = true;
            cloudPaths1 = new List<CloudType>();
            cloudPaths2 = new List<CloudType>();
            _listing1Command = new DelegateCommand(OnListing1);
            _listing2Command = new DelegateCommand(OnListing2);
            _copyCommand = new DelegateCommand(OnCopy);
            _moveCommand = new DelegateCommand(OnMove);
            _deleteCommand = new DelegateCommand(OnDelete);
            _renameCommand = new DelegateCommand(OnRename);
            _createDirectoryCommand = new DelegateCommand(OnCreateNewDir);
            _createTxtCommand = new DelegateCommand(OnNewTxt);
        }

        //Osztályszintű változók
        List<Local> directories1 = new List<Local>();
        List<Local> directories2 = new List<Local>();
        List<Local> files1 = new List<Local>();
        List<Local> files2 = new List<Local>();
        string tempName;
        CreateDirWindow tempWindow;
        FileGoogleD FGD;
        List<CloudType> cloudPaths1;
        List<CloudType> cloudPaths2;

        //Mezők
        List<FileAttributes> f;
        List<Local> filesAndDirectories1;
        List<Local> filesAndDirectories2;
        string prevDirectory1;
        string prevDirectory2;
        string actualDirectory1;
        string actualDirectory2;
        int activeListview;
        List<Local> selectedItemList;
        bool normal;
        bool googleDrive;
        bool oneDrive;
        bool dropbox;
        bool normal2;
        bool googleDrive2;
        bool oneDrive2;
        bool dropbox2;


        //Attribútumok
        public List<Local> FilesAndDirectories1
        {
            get
            {
                return filesAndDirectories1;
            }
            set
            {
                filesAndDirectories1 = value;
                OnPropertyChanged("FilesAndDirectories1");
            }
        }
        public string PrevDirectory1
        {
            get
            {
                return prevDirectory1;
            }
            set
            {
                prevDirectory1 = value;
            }
        }
        public string ActualDirectory1
        {
            get
            {
                return actualDirectory1;
            }
            set
            {
                actualDirectory1 = value;
                OnPropertyChanged("ActualDirectory1");
            }
        }
        public List<Local> FilesAndDirectories2
        {
            get
            {
                return filesAndDirectories2;
            }
            set
            {
                filesAndDirectories2 = value;
                OnPropertyChanged("FilesAndDirectories2");
            }
        }
        public string PrevDirectory2
        {
            get
            {
                return prevDirectory2;
            }
            set
            {
                prevDirectory2 = value;
            }
        }
        public string ActualDirectory2
        {
            get
            {
                return actualDirectory2;
            }
            set
            {
                actualDirectory2 = value;
                OnPropertyChanged("ActualDirectory2");
            }
        }
        public List<FileAttributes> F { get => f; set => f = value; }
        public int ActiveListview { get => activeListview; set => activeListview = value; }
        public List<Local> SelectedItemList { get => selectedItemList; set => selectedItemList = value; }
        public bool Normal
        {
            get
            {
                return normal;
            }
            set
            {
                normal = value;
                OnPropertyChanged("Normal");
            }
        }
        public bool GoogleDrive
        {
            get
            {
                return googleDrive;
            }
            set
            {
                googleDrive = value;
                OnPropertyChanged("GoogleDrive");
                if (googleDrive == true)
                {
                    FGD.Starter("David", "credentials.json");
                    cloudPaths1.Clear();
                    cloudPaths1.Add(new CloudType("root","root"));
                    ActualDirectory1 = "root";
                    FilesAndDirectories1 = FGD.FileListingFull(actualDirectory1);
                }
            }
        }
        public bool OneDrive
        {
            get
            {
                return oneDrive;
            }
            set
            {
                oneDrive = value;
                OnPropertyChanged("OneDrive");
            }
        }
        public bool Dropbox
        {
            get
            {
                return dropbox;
            }
            set
            {
                dropbox = value;
                OnPropertyChanged("Dropbox");
            }
        }
        public bool Normal2
        {
            get
            {
                return normal2;
            }
            set
            {
                normal2 = value;
                OnPropertyChanged("Normal2");
            }
        }
        public bool GoogleDrive2
        {
            get
            {
                return googleDrive2;
            }
            set
            {
                googleDrive2 = value;
                OnPropertyChanged("GoogleDrive2");
                if (googleDrive2 == true)
                {
                    FGD.Starter("David","credentials.json");
                }
                
            }
        }
        public bool OneDrive2
        {
            get
            {
                return oneDrive2;
            }
            set
            {
                oneDrive2 = value;
                OnPropertyChanged("OneDrive2");
            }
        }
        public bool Dropbox2
        {
            get
            {
                return dropbox2;
            }
            set
            {
                dropbox2 = value;
                OnPropertyChanged("Dropbox2");
            }
        }



        //Metódusok
        private void OnListing1(object commandParameter)
        {
            if (normal == true)
            {
                FilesAndDirectories1 = FileBasics.FullListing(ref actualDirectory1, ref directories1, ref files1, ref prevDirectory1, F);
                OnPropertyChanged("ActualDirectory1");
            }
            else if (googleDrive == true)
            {
                Local l = new Local("", @"F:\Target\asdasd.txt", "asdasd", "0", "txt", "2019.04.28");
                //FGD.Upload(l, "", true);
                //FGD.Upload(l, "");
                FGD.Copy("1SPKV0HK1A_K8vOMai208Lmx7ae0jcdOt","1SEdzhOl4Wik_LjNoJh0zQoX_uNMGbtCj","asdasd");
                FilesAndDirectories1 = FGD.FileListingFull(actualDirectory1);
            }
            else if (oneDrive == true)
            {

            }
            else
            {

            }
        }
        private void OnListing2(object commandParameter)
        {
            FilesAndDirectories2 = FileBasics.FullListing(ref actualDirectory2, ref directories2, ref files2, ref prevDirectory2, F);
            OnPropertyChanged("ActualDirectory2");
        }
        public void OnNavigation1(Local selectedItem)
        {
            if (normal == true)
            {
                FileBasics.Navigation(selectedItem, ref actualDirectory1, ref prevDirectory1, ref directories1, ref files1, f, ref filesAndDirectories1);
                OnPropertyChanged("ActualDirectory1");
                OnPropertyChanged("FilesAndDirectories1");
            }
            else if (googleDrive == true)
            {
                FilesAndDirectories1 = FGD.Navigation(selectedItem,ref cloudPaths1,filesAndDirectories1);
                ActualDirectory1 = cloudPaths1.Last().Name;
            }
        }
        public void OnNavigation2(Local selectedItem)
        {
            FileBasics.Navigation(selectedItem, ref actualDirectory2, ref prevDirectory2, ref directories2, ref files2, f, ref filesAndDirectories2);
            OnPropertyChanged("ActualDirectory2");
            OnPropertyChanged("FilesAndDirectories2");
        }
        private void OnCopy(object commandParameter)
        {
            if (activeListview == 1)
            {
                
                FileBasics.CopyMultipleElements(selectedItemList,filesAndDirectories2, actualDirectory2);
                OnListing2(commandParameter); //másolás után frissítjük a listát
            }
            else if (activeListview == 2)
            {
                FileBasics.CopyMultipleElements(selectedItemList, filesAndDirectories1, actualDirectory1);
                OnListing1(commandParameter);
            }
            
        }
        private void OnMove(object commandParameter)
        {
            if (activeListview == 1 && !actualDirectory1.Equals(actualDirectory2))
            {
                FileBasics.MoveMultipleElements(selectedItemList, filesAndDirectories2, actualDirectory2);
                OnListing1(commandParameter); //másolás után frissítjük a listát
                OnListing2(commandParameter);
            }
            else if (activeListview == 2 && !actualDirectory1.Equals(actualDirectory2))
            {
                FileBasics.MoveMultipleElements(selectedItemList, filesAndDirectories1, actualDirectory1);
                OnListing1(commandParameter); //másolás után frissítjük a listát
                OnListing2(commandParameter);
            }
        }
        private void OnDelete(object commandParameter)
        {
            if (selectedItemList !=null)
            {
                FileBasics.DeleteMultipleElement(selectedItemList);
            }
            if (activeListview == 1)
            {
                OnListing1(commandParameter);
            }
            else if (activeListview == 2)
            {
                OnListing2(commandParameter);
            }
        }
        private void OnRename(object commandParameter)
        {
            //Input ablak
            tempName = "";
            tempWindow = new CreateDirWindow();
            tempWindow.CreateClicked += new EventHandler(NameAdded);
            tempWindow.ShowDialog();


            if (!tempName.Equals("") && selectedItemList != null)
            {
                Local tempItem = selectedItemList.First();
                if (activeListview == 1)
                {
                    FileBasics.Rename(actualDirectory1, tempItem.NameOfLocal, tempName);
                    OnListing1(commandParameter);
                }
                else if(activeListview == 2)
                {
                    FileBasics.Rename(actualDirectory2, tempItem.NameOfLocal, tempName);
                    OnListing2(commandParameter);
                }
                
            }
            else if (tempWindow.DialogResult == true)
            {
                MessageBox.Show("Name is empty or there isn't any selected document.", "Renaming Failed");
            }
        }
        private void OnCreateNewDir(object commandParameter)
        {
            tempName = "";
            tempWindow = new CreateDirWindow();
            tempWindow.CreateClicked += new EventHandler(NameAdded);
            tempWindow.ShowDialog();


            if (!tempName.Equals(""))
            {
                if (activeListview == 1)
                {
                    FileBasics.CreateNewDirectory(actualDirectory1, tempName);
                    OnListing1(commandParameter);
                }
                else if (activeListview == 2)
                {
                    FileBasics.CreateNewDirectory(actualDirectory2, tempName);
                    OnListing2(commandParameter);
                }
            }
            else if (tempWindow.DialogResult == true)
            {
                MessageBox.Show("Name is empty.", "Creation Failed");
            }
        }
        private void OnNewTxt(object commandParametere)
        {
            FileBasics.CreateNewTxtFile();
            FilesAndDirectories1 = FileBasics.FullListing(ref actualDirectory1, ref directories1, ref files1, ref prevDirectory1, F);
            FilesAndDirectories2 = FileBasics.FullListing(ref actualDirectory2, ref directories2, ref files2, ref prevDirectory2, F);
        }
        public void OnListDrive(int panelNumber)
        {
            if (panelNumber == 1)
            {
                FilesAndDirectories1 = FileBasics.FindDrives();
                ActualDirectory1 = "Drives";
            }
            else
            {
                FilesAndDirectories2 = FileBasics.FindDrives();
                ActualDirectory2 = "Drives";
            }
        }
        private void NameAdded(object sender, EventArgs e)
        {
            tempName = tempWindow.TempName;
        }
        /// <summary>
        /// Segéd metódus az átnevezéshez és az új könyvtár létrehozásához
        /// </summary>
        


        //Tulajdonság figyelő
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
