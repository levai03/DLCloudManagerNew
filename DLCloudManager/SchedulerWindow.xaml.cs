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
    /// Interaction logic for SchedulerWindow.xaml
    /// </summary>
    public partial class SchedulerWindow : Window
    {
        public event EventHandler StartClicked;
        public event EventHandler StopClicked;
        int tempTime;
        public int TempTime
        {
            get { return tempTime; }
            set { tempTime = value; }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (StartClicked != null)
            {
                int myint;
                if (int.TryParse(Tx1.Text,out myint))
                {
                    TempTime = int.Parse(Tx1.Text);
                    StartClicked(this, new EventArgs());
                }
                else
                {
                    MessageBox.Show("Wrong Number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            this.DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        public SchedulerWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            if (StopClicked != null)
            {
                StopClicked(this, new EventArgs());
            }
            this.DialogResult = true;
        }
    }
}
