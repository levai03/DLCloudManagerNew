using DLCloudManager.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly DelegateCommand _selectTempPathCommand;
        public ICommand Listing1Command => _listing1Command;
        public ICommand Listing2Command => _listing2Command;
        public ICommand CopyCommand => _copyCommand;
        public ICommand MoveCommand => _moveCommand;
        public ICommand DeleteCommand => _deleteCommand;
        public ICommand RenameCommand => _renameCommand;
        public ICommand CreateDirectoryCommand => _createDirectoryCommand;
        public ICommand CreateTxtCommand => _createTxtCommand;
        public ICommand SelectTempPathCommand => _selectTempPathCommand;
        public MainViewModel()
        {
            dropBoxToken = "Vp27yfQf79AAAAAAAAAACkn4rz-SxmpM1gVZPUff1tb952xqQ3JVViDNxAlfUhxl";
            tempPath = "C:\\temp";
            PrevDirectory1 = "C:\\";
            ActualDirectory1 = "C:\\";
            PrevDirectory2 = "C:\\";
            ActualDirectory2 = "C:\\";
            FGD = new FileGoogleD();
            FOD = new FileOneD();
            FDB = new FileDropB();
            List<FileAttributes> tempAttributes = new List<FileAttributes>();
            F = tempAttributes;
            HiddenAttribute = false;
            ReadOnlyAttribute = true;
            SystemAttribute = false;
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
            _selectTempPathCommand = new DelegateCommand(OnSelectTempPath);
            
        }

        //Osztályszintű változók
        string dropBoxToken;
        List<Local> directories1 = new List<Local>();
        List<Local> directories2 = new List<Local>();
        List<Local> files1 = new List<Local>();
        List<Local> files2 = new List<Local>();
        string tempName;
        CreateDirWindow tempWindow;
        PathDialogWindow tempPathDialog;
        FileGoogleD FGD;
        FileOneD FOD;
        FileDropB FDB;
        List<CloudType> cloudPaths1;
        List<CloudType> cloudPaths2;
        string tempPath;

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
        string workProgress;
        bool readOnlyAttribute;
        bool hiddenAttribute;
        bool systemAttribute;


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
                    FilesAndDirectories1 = FGD.FileListingFull(cloudPaths1.Last().ID1);
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
                if (dropbox)
                {
                    ActualDirectory1 = "";
                    FilesAndDirectories1 = FDB.ListFolder(ActualDirectory1, dropBoxToken);

                }
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
                if (dropbox2)
                {
                    ActualDirectory2 = "";
                    FilesAndDirectories2 = FDB.ListFolder(ActualDirectory2, dropBoxToken);

                }
            }
        }
        public string WorkProgress
        {
            get
            {
                return workProgress;
            }
            set
            {
                workProgress = value;
                OnPropertyChanged("WorkProgress");
            }
        }
        public bool ReadOnlyAttribute
        {
            get
            {
                return readOnlyAttribute;
            }
            set
            {
                readOnlyAttribute = value;
                OnPropertyChanged("ReadOnlyAttribute");
                if (readOnlyAttribute)
                {
                    F.Remove(FileAttributes.ReadOnly);
                }
                else
                {
                    F.Add(FileAttributes.ReadOnly);
                }
            }
        }
        public bool HiddenAttribute
        {
            get
            {
                return hiddenAttribute;
            }
            set
            {
                hiddenAttribute = value;
                OnPropertyChanged("HiddenAttribute");
                if (hiddenAttribute)
                {
                    F.Remove(FileAttributes.Hidden);
                }
                else
                {
                    F.Add(FileAttributes.Hidden);
                }
            }
        }
        public bool SystemAttribute
        {
            get
            {
                return systemAttribute;
            }
            set
            {
                systemAttribute = value;
                OnPropertyChanged("SystemAttribute");
                if (systemAttribute)
                {
                    F.Remove(FileAttributes.System);
                }
                else
                {
                    F.Add(FileAttributes.System);
                }
            }
        }

        //Metódusok
        private void OnListing1(object commandParameter)
        {
            if (normal)
            {
                FilesAndDirectories1 = FileBasics.FullListing(ref actualDirectory1, ref directories1, ref files1, ref prevDirectory1, F);
                OnPropertyChanged("ActualDirectory1");
            }
            else if (googleDrive)
            {
                FilesAndDirectories1 = FGD.FileListingFull(cloudPaths1.Last().ID1);
            }
            else if (oneDrive)
            {

            }
            else
            {
                FilesAndDirectories1 = FDB.ListFolder(actualDirectory1, dropBoxToken);
            }
        }
        private void OnListing2(object commandParameter)
        {
            
            if (normal2)
            {
                FilesAndDirectories2 = FileBasics.FullListing(ref actualDirectory2, ref directories2, ref files2, ref prevDirectory2, F);
                OnPropertyChanged("ActualDirectory2");
            }
            else if (googleDrive)
            {
                FilesAndDirectories2 = FGD.FileListingFull(cloudPaths2.Last().ID1);
            }
            else if (oneDrive)
            {

            }
            else
            {
                FilesAndDirectories2 = FDB.ListFolder(actualDirectory2, dropBoxToken);
            }
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
            else if (oneDrive == true)
            {
                FilesAndDirectories1 = FOD.Navigation(selectedItem, ref cloudPaths1, filesAndDirectories1);
                ActualDirectory1 = cloudPaths1.Last().Name;
            }
            else
            {
                FilesAndDirectories1 = FDB.Navigation(ref actualDirectory1,prevDirectory1,selectedItem,filesAndDirectories1,dropBoxToken);
                OnPropertyChanged("ActualDirectory1");
            }
        }
        public void OnNavigation2(Local selectedItem)
        {
            if (normal2 == true)
            {
                FileBasics.Navigation(selectedItem, ref actualDirectory2, ref prevDirectory2, ref directories2, ref files2, f, ref filesAndDirectories2);
                OnPropertyChanged("ActualDirectory2");
                OnPropertyChanged("FilesAndDirectories2");
            }
            else if (googleDrive2 == true)
            {
                FilesAndDirectories2 = FGD.Navigation(selectedItem, ref cloudPaths2, filesAndDirectories2);
                ActualDirectory2 = cloudPaths2.Last().Name;
            }
            else if (oneDrive2 == true)
            {
                FilesAndDirectories2 = FOD.Navigation(selectedItem, ref cloudPaths2, filesAndDirectories2);
                ActualDirectory2 = cloudPaths2.Last().Name;
            }
            else
            {
                FilesAndDirectories2 = FDB.Navigation(ref actualDirectory2, prevDirectory2, selectedItem, filesAndDirectories2, dropBoxToken);
                OnPropertyChanged("ActualDirectory2");
            }
        }
        private void PathFinder1(ref bool workError)
        {
            if (normal)
            {
                if (normal2) //másolás local
                {
                    FileBasics.CopyMultipleElements(selectedItemList, filesAndDirectories2, actualDirectory2);
                }
                else if (GoogleDrive2) //feltöltés
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1);
                }
                else if (OneDrive2) //feltöltés
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1);
                }
                else //feltöltés
                {
                    FDB.CopyMultipleElementUpload(selectedItemList, actualDirectory1, actualDirectory2, dropBoxToken, ref workError);
                }
            }
            else if (googleDrive)
            {
                if (normal2) //letöltés
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1, actualDirectory2);
                }
                else if (GoogleDrive2) //másolás Google
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1, cloudPaths2.Last().ID1, true);
                }
                else if (OneDrive2) //áttöltés Google - One
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1, tempPath);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories1, ref files1, ref tempPath, F);
                    FOD.CopyMultipleElement(uploadItems, cloudPaths2.Last().ID1);
                    FileBasics.Delete(tempPath);
                }
                else //Áttöltés Google - Drop
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1, tempPath);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories1, ref files1, ref tempPath, F);
                    FDB.CopyMultipleElementUpload(selectedItemList, actualDirectory1, actualDirectory2, dropBoxToken, ref workError);
                    FileBasics.Delete(tempPath);
                }

            }
            else if (oneDrive)
            {
                if (normal2) //letöltés
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1, actualDirectory2);
                }
                else if (GoogleDrive2) //áttöltés One - Google
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1, tempPath);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories1, ref files1, ref tempPath, F);
                    FGD.CopyMultipleElement(uploadItems, cloudPaths2.Last().ID1);
                    FileBasics.Delete(tempPath);
                    
                }
                else if (OneDrive2) //másolás One
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1, cloudPaths2.Last().ID1, true);
                }
                else //Áttöltés One - Drop
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1, tempPath);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories1, ref files1, ref tempPath, F);
                    FDB.CopyMultipleElementUpload(selectedItemList, actualDirectory1, actualDirectory2, dropBoxToken, ref workError);
                    FileBasics.Delete(tempPath);
                }
            }
            else
            {
                if (normal2) //letöltés
                {
                    FDB.CopyMultipleElement(selectedItemList, actualDirectory1, actualDirectory2, dropBoxToken, ref workError);
                }
                else if (GoogleDrive2) //áttöltés Drop - Google
                {
                    FDB.CopyMultipleElement(selectedItemList, actualDirectory1, actualDirectory2, dropBoxToken, ref workError);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories1, ref files1, ref tempPath, F);
                    FGD.CopyMultipleElement(uploadItems, cloudPaths2.Last().ID1);
                    FileBasics.Delete(tempPath);

                }
                else if (OneDrive2) //Áttöltés Drop - One
                {
                    FDB.CopyMultipleElement(selectedItemList, actualDirectory1, actualDirectory2, dropBoxToken, ref workError);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories1, ref files1, ref tempPath, F);
                    FOD.CopyMultipleElement(uploadItems, cloudPaths2.Last().ID1);
                    FileBasics.Delete(tempPath);
                }
                else //Másolás Drop
                {
                    FDB.CopyMultipleElement(selectedItemList, actualDirectory1, actualDirectory2, dropBoxToken,true, ref workError);
                }
            }
        }
        private void PathFinder2(ref bool workError)
        {
            if (normal2)
            {
                if (normal) //másolás local
                {
                    FileBasics.CopyMultipleElements(selectedItemList, filesAndDirectories1, actualDirectory1);
                }
                else if (GoogleDrive) //feltöltés
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1);
                }
                else if (OneDrive) //feltöltés
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths1.Last().ID1);
                }
                else //feltöltés
                {
                    FDB.CopyMultipleElementUpload(selectedItemList, actualDirectory2, actualDirectory1, dropBoxToken, ref workError);
                }
            }
            else if (googleDrive2)
            {
                if (normal) //letöltés
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1, actualDirectory1);
                }
                else if (GoogleDrive) //másolás Google
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1, cloudPaths1.Last().ID1, true);
                }
                else if (OneDrive) //áttöltés Google - One
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1, tempPath);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories2, ref files2, ref tempPath, F);
                    FOD.CopyMultipleElement(uploadItems, cloudPaths1.Last().ID1);
                    FileBasics.Delete(tempPath);
                }
                else //Áttöltés Google - Drop
                {
                    FGD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1, tempPath);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories2, ref files2, ref tempPath, F);
                    FDB.CopyMultipleElementUpload(selectedItemList,actualDirectory2,actualDirectory1,dropBoxToken,ref workError);
                    FileBasics.Delete(tempPath);
                }

            }
            else if (oneDrive)
            {
                if (normal) //letöltés
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1, actualDirectory1);
                }
                else if (GoogleDrive) //áttöltés One - Google
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1, tempPath);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories2, ref files2, ref tempPath, F);
                    FGD.CopyMultipleElement(uploadItems, cloudPaths1.Last().ID1);
                    FileBasics.Delete(tempPath);

                }
                else if (OneDrive) //másolás One
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1, cloudPaths1.Last().ID1, true);
                }
                else //Áttöltés One - Drop
                {
                    FOD.CopyMultipleElement(selectedItemList, cloudPaths2.Last().ID1, tempPath);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories2, ref files2, ref tempPath, F);
                    FDB.CopyMultipleElementUpload(selectedItemList, actualDirectory2, actualDirectory1, dropBoxToken, ref workError);
                    FileBasics.Delete(tempPath);
                }
            }
            else
            {
                if (normal) //letöltés
                {
                    FDB.CopyMultipleElement(selectedItemList, actualDirectory2,actualDirectory1,dropBoxToken, ref workError);
                }
                else if (GoogleDrive) //áttöltés Drop - Google
                {
                    FDB.CopyMultipleElement(selectedItemList, actualDirectory2, actualDirectory1, dropBoxToken, ref workError);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories2, ref files2, ref tempPath, F);
                    FGD.CopyMultipleElement(uploadItems, cloudPaths1.Last().ID1);
                    FileBasics.Delete(tempPath);

                }
                else if (OneDrive) //Áttöltés Drop - One
                {
                    FDB.CopyMultipleElement(selectedItemList, actualDirectory2, actualDirectory1, dropBoxToken, ref workError);
                    List<Local> uploadItems = FileBasics.FullListing(ref tempPath, ref directories2, ref files2, ref tempPath, F);
                    FOD.CopyMultipleElement(uploadItems, cloudPaths1.Last().ID1);
                    FileBasics.Delete(tempPath);
                }
                else //Másolás Drop
                {
                    FDB.CopyMultipleElement(selectedItemList, actualDirectory2, actualDirectory1, dropBoxToken, true, ref workError);
                }
            }
        }
        private void OnCopy(object commandParameter)
        {
            WorkProgress = "Work in Progress!";
            ExtensionMethods.Refresh(new UIElement());
            bool workError = false;
            if (selectedItemList != null && selectedItemList.Count != 0)
            {
                if (activeListview == 1)
                {
                    PathFinder1(ref workError);
                    OnListing2(commandParameter); //másolás után frissítjük a listát
                }
                else if (activeListview == 2)
                {
                    PathFinder2(ref workError);
                    OnListing1(commandParameter);
                }
            }
            else
            {
                workError = true;
                MessageBox.Show("There is no selected item!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ProgressStatus(workError);
            
        }
        private void OnMove(object commandParameter)
        {
            WorkProgress = "Work in Progress!";
            ExtensionMethods.Refresh(new UIElement());
            bool workError = false;
            if (selectedItemList != null && selectedItemList.Count != 0)
            {
                if (activeListview == 1)
                {
                    if (normal && normal2 && !actualDirectory1.Equals(actualDirectory2))
                    {
                        FileBasics.MoveMultipleElements(selectedItemList, filesAndDirectories2, actualDirectory2);
                    }
                    else if (googleDrive && googleDrive2 && !cloudPaths1.Last().ID1.Equals(cloudPaths2.Last().ID1))
                    {
                        FGD.MoveMultipleElement(selectedItemList, cloudPaths2.Last().ID1);
                    }
                    else if (oneDrive && oneDrive2 && !cloudPaths1.Last().ID1.Equals(cloudPaths2.Last().ID1))
                    {
                        FOD.MoveMultipleElement(selectedItemList, cloudPaths2.Last().ID1);
                    }
                    else if (dropbox && dropbox2 && !actualDirectory1.Equals(actualDirectory2))
                    {
                        FDB.MoveMultipleElement(selectedItemList, actualDirectory1, actualDirectory2, dropBoxToken, ref workError);
                    }
                    else
                    {
                        workError = true;
                        MessageBox.Show("Move only available for the same account or device!", "Warrning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else if (activeListview == 2)
                {
                    if (normal && normal2 && !actualDirectory1.Equals(actualDirectory2))
                    {
                        FileBasics.MoveMultipleElements(selectedItemList, filesAndDirectories1, actualDirectory1);
                    }
                    else if (googleDrive && googleDrive2 && !cloudPaths1.Last().ID1.Equals(cloudPaths2.Last().ID1))
                    {
                        FGD.MoveMultipleElement(selectedItemList, cloudPaths1.Last().ID1);
                    }
                    else if (oneDrive && oneDrive2 && !cloudPaths1.Last().ID1.Equals(cloudPaths2.Last().ID1))
                    {
                        FOD.MoveMultipleElement(selectedItemList, cloudPaths1.Last().ID1);
                    }
                    else if (dropbox && dropbox2 && !actualDirectory1.Equals(actualDirectory2))
                    {
                        FDB.MoveMultipleElement(selectedItemList, actualDirectory2, actualDirectory1, dropBoxToken, ref workError);
                    }
                    else
                    {
                        workError = true;
                        MessageBox.Show("Move only available for the same account or device!", "Warrning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                
            }
            else
            {
                workError = true;
                MessageBox.Show("There is no selected item!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OnListing1(commandParameter); //másolás után frissítjük a listát
            OnListing2(commandParameter);
            ProgressStatus(workError);
        }
        private void OnDelete(object commandParameter)
        {
            WorkProgress = "Work in Progress!";
            ExtensionMethods.Refresh(new UIElement());
            bool workError = false;
            if (selectedItemList !=null && selectedItemList.Count != 0)
            {
                if (activeListview == 1)
                {
                    if (normal)
                    {
                        FileBasics.DeleteMultipleElement(selectedItemList);
                    }
                    else if (googleDrive)
                    {
                        FGD.DeleteMultipleElement();
                    }
                    else if (oneDrive)
                    {
                        FOD.DeleteMultipleElement();
                    }
                    else
                    {
                        FDB.DeleteMultipleElement(selectedItemList,actualDirectory1,dropBoxToken,ref workError);
                    }
                    
                }
                else if (activeListview == 2)
                {
                    if (normal2)
                    {
                        FileBasics.DeleteMultipleElement(selectedItemList);
                    }
                    else if (googleDrive2)
                    {
                        FGD.DeleteMultipleElement();
                    }
                    else if (oneDrive2)
                    {
                        FOD.DeleteMultipleElement();
                    }
                    else
                    {
                        FDB.DeleteMultipleElement(selectedItemList, actualDirectory2, dropBoxToken, ref workError);
                    }
                    
                }
            }
            else
            {
                workError = true;
                MessageBox.Show("There is no selected item!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            OnListing1(commandParameter);
            OnListing2(commandParameter);
            ProgressStatus(workError);
        }
        private void OnRename(object commandParameter)
        {
            WorkProgress = "Work in Progress!";
            ExtensionMethods.Refresh(new UIElement());
            bool workError = false;
            //Input ablak
            tempName = "";
            tempWindow = new CreateDirWindow();
            tempWindow.CreateClicked += new EventHandler(NameAdded);
            tempWindow.ShowDialog();


            if (!tempName.Equals("") && selectedItemList != null && selectedItemList.Count != 0)
            {
                Local tempItem = selectedItemList.First();
                if (activeListview == 1)
                {
                    if (normal)
                    {
                        FileBasics.Rename(actualDirectory1, tempItem.NameOfLocal, tempName);
                    }
                    else if (googleDrive)
                    {
                        FGD.Rename(cloudPaths1.Last().ID1, tempItem.NameOfLocal, tempName);
                    }
                    else if (oneDrive)
                    {
                        FOD.Rename(cloudPaths1.Last().ID1, tempItem.NameOfLocal, tempName);
                    }
                    else
                    {
                        FDB.Rename(actualDirectory1, tempItem.NameOfLocal, tempName, dropBoxToken, ref workError);
                    }
                    OnListing1(commandParameter);
                }
                else if(activeListview == 2)
                {
                    if (normal2)
                    {
                        FileBasics.Rename(actualDirectory2, tempItem.NameOfLocal, tempName);
                    }
                    else if (googleDrive2)
                    {
                        FGD.Rename(cloudPaths2.Last().ID1, tempItem.NameOfLocal, tempName);
                    }
                    else if (oneDrive2)
                    {
                        FOD.Rename(cloudPaths2.Last().ID1, tempItem.NameOfLocal, tempName);
                    }
                    else
                    {
                        FDB.Rename(actualDirectory2, tempItem.NameOfLocal, tempName, dropBoxToken, ref workError);
                    }
                    OnListing2(commandParameter);
                }
                
            }
            else if (tempWindow.DialogResult == true)
            {
                workError = true;
                MessageBox.Show("Name is empty or there isn't any selected document.", "Renaming Failed");
            }
            ProgressStatus(workError);

        }
        private void OnCreateNewDir(object commandParameter)
        {
            WorkProgress = "Work in Progress!";
            ExtensionMethods.Refresh(new UIElement());
            bool workError = false;
            tempName = "";
            tempWindow = new CreateDirWindow();
            tempWindow.CreateClicked += new EventHandler(NameAdded);
            tempWindow.ShowDialog();


            if (!tempName.Equals(""))
            {
                if (activeListview == 1)
                {
                    if (normal)
                    {
                        FileBasics.CreateNewDirectory(actualDirectory1, tempName);
                    }
                    else if (googleDrive)
                    {
                        FGD.CreateNewDirectory(cloudPaths1.Last().ID1, tempName);
                    }
                    else if (oneDrive)
                    {
                        FOD.CreateNewDirectory(cloudPaths1.Last().ID1, tempName);
                    }
                    else
                    {
                        FDB.CreateNewDirectory(actualDirectory1, tempName, dropBoxToken, ref workError);
                    }
                    OnListing1(commandParameter);
                }
                else if (activeListview == 2)
                {
                    
                    if (normal2)
                    {
                        FileBasics.CreateNewDirectory(actualDirectory2, tempName);
                    }
                    else if (googleDrive2)
                    {
                        FGD.CreateNewDirectory(cloudPaths2.Last().ID1, tempName);
                    }
                    else if (oneDrive2)
                    {
                        FOD.CreateNewDirectory(cloudPaths2.Last().ID1, tempName);
                    }
                    else
                    {
                        FDB.CreateNewDirectory(actualDirectory2, tempName,dropBoxToken,ref workError);
                    }
                    OnListing2(commandParameter);
                }
            }
            else if (tempWindow.DialogResult == true)
            {
                workError = true;
                MessageBox.Show("Name is empty.", "Creation Failed");
            }
            ProgressStatus(workError);
        }
        private void OnNewTxt(object commandParametere)
        {
            FileBasics.CreateNewTxtFile();
            if(normal) FilesAndDirectories1 = FileBasics.FullListing(ref actualDirectory1, ref directories1, ref files1, ref prevDirectory1, F);
            if(normal2) FilesAndDirectories2 = FileBasics.FullListing(ref actualDirectory2, ref directories2, ref files2, ref prevDirectory2, F);
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
        private void OnProgressStart(object commandParametere)
        {
            WorkProgress = "Work in progress!";
        }

        //Segéd metódusok
        private void NameAdded(object sender, EventArgs e)
        {
            tempName = tempWindow.TempName;
        }
        /// <summary>
        /// Segéd metódus az átnevezéshez és az új könyvtár létrehozásához
        /// </summary>
        private void PathAdded(object sender, EventArgs e)
        {
            tempName = tempPathDialog.TempPath;
        }
        private void ProgressStatus(bool WorkError)
        {
            if (WorkError)
            {
                WorkProgress = "Work is Failed!";
            }
            else
            {
                WorkProgress = "Work is Successful!";
            }
        }
        public void OnSelectTempPath(object commandParameter)
        {
            tempName = "";
            tempPathDialog = new PathDialogWindow();
            tempPathDialog.OKClicked += new EventHandler(PathAdded);
            tempPathDialog.ShowDialog();
            if (!tempName.Equals(""))
            {
                tempPath = tempName + "\\temp";
            }
        }

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
