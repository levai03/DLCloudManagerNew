using DLCloudManager.Models;
using DLCloudManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLCloudManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel viewModel;
        

        
        
        
        public MainWindow()
        {
            viewModel = new MainViewModel();
            





            DataContext = viewModel;
            InitializeComponent();




            //FileOneD d = new FileOneD();
            //d.Starter();
            //System.Timers.Timer timer = new System.Timers.Timer(30000);
            //timer.Elapsed += proba;
            //timer.Enabled=true;

        }

        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }




        private void proba(object sender, System.Timers.ElapsedEventArgs e)
        {
            MessageBox.Show("jej");
        }
                
        private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (((ListView)sender).Name.Equals("Lw1"))
            {
                Local temp = (Local)Lw1.SelectedItem;
                viewModel.OnNavigation1(temp);
            }
            else
            {
                Local temp = (Local)Lw2.SelectedItem;
                viewModel.OnNavigation2(temp);
            }
        }
        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (((ListView)sender).Name.Equals("Lw1"))
            {
                viewModel.ActiveListview = 1;
            }
            else
            {
                viewModel.ActiveListview = 2;
            }
            
        }
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            viewModel.SelectedItemList = ((ListView)sender).SelectedItems.Cast<Local>().ToList();
        }





       
       
        
        
                
        private void Tb1_KeyDown(object sender, KeyEventArgs e)
        {/*
            if(e.Key == Key.Enter)
            {
                Listing1(sender, e);

            }*/
        }
        
        private void Tb2_KeyDown(object sender, KeyEventArgs e)
        {/*
            if (e.Key == Key.Enter)
            {
                Listing2(sender, e);
            }*/
        }
        
                
        private void NewTxt_Click(object sender, RoutedEventArgs e)
        {
            
            Process.Start("notepad.exe");
        }
        
        private void Drive2_Click(object sender, RoutedEventArgs e)
        {
            List<Local> drives = FileBasics.FindDrives();
            Lw2.Items.Clear();
            foreach (Local d in drives)
            {
                Lw2.Items.Add(d);
            }
        }
        
        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            List<Local> drives = FileBasics.FindDrives();
            Lw1.Items.Clear();
            foreach (Local d in drives)
            {
                Lw1.Items.Add(d);
            }
        }

        private void GDrive1_Click(object sender, RoutedEventArgs e)
        {
            //fGD.Starter();
            //List<Local> l = fGD.FileListing();
        }

        private void Cp_Button_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
