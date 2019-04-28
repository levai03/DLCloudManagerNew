using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateDirWindow.xaml
    /// </summary>
    public partial class CreateDirWindow : Window
    {
        public CreateDirWindow()
        {
            InitializeComponent();
        }
        public event EventHandler CreateClicked;
        string tempName;
        public string TempName
        {
            get { return tempName; }
            set { tempName = value; }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(CreateClicked != null)
            {
                TempName = Tx1.Text;
                CreateClicked(this, new EventArgs());
            }
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
