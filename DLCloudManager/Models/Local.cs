using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLCloudManager.Models
{
    public class Local : INotifyPropertyChanged
    {
        private string imgSource;
        private string pathOfLocal;
        private string nameOfLocal;
        private string sizeOfLocal;
        private string extensionOfLocal;
        private string dateOfCreation;
        private string id;

        public Local(string i, string p, string n, string s, string e, string d)
        {
            this.imgSource = i;
            this.pathOfLocal = p;
            this.nameOfLocal = n;
            this.sizeOfLocal = s;
            this.extensionOfLocal = e;
            this.dateOfCreation = d;
        }

        public string ImgSource
        {
            get
            {
                return imgSource;
            }
        }
        public string PathOfLocal
        {
            get
            {
                return pathOfLocal;
            }
        }
        public string NameOfLocal
        {
            get
            {
                return nameOfLocal;
            }
        }
        public string SizeOfLocal
        {
            get
            {
                return sizeOfLocal;
            }
        }
        public string ExtensionOfLocal
        {
            get
            {
                return extensionOfLocal;
            }
        }
        public string DateOfCreation
        {
            get
            {
                return dateOfCreation;
            }
        }
        public string Id { get => id; set => id = value; }

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
