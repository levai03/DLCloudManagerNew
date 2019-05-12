using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DLCloudManager
{
    /// <summary>
    /// Interaction logic for PathDialogWindow.xaml
    /// </summary>
    public partial class PathDialogWindow : Window
    {
        public PathDialogWindow()
        {
            InitializeComponent();
        }
        public event EventHandler OKClicked;
        string tempPath;
        public string TempPath
        {
            get { return tempPath; }
            set { tempPath = value; }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (OKClicked != null)
            {
                
                TempPath = Tx1.Text;
                if (Directory.Exists(TempPath))
                {
                    OKClicked(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Wrong Path", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
