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
            //FileGoogleD FGD = new FileGoogleD();
           // FGD.Starter();
           // FGD.FileListingFull();
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
        private void Drive_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Name.Equals("Drive"))
            {
                viewModel.OnListDrive(1);
            }
            else
            {
                viewModel.OnListDrive(2);
            }

        }
        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Shortcuts: "
                + Environment.NewLine 
                + Environment.NewLine + "Copy - F5"
                + Environment.NewLine + "Move - F6"
                + Environment.NewLine + "Delete - F8"
                + Environment.NewLine + "Rename - F2"
                + Environment.NewLine + "Create New Directory - F7"
                + Environment.NewLine + "Create New TxtFile - Ctrl + N"
                + Environment.NewLine + "List all Drives - Ctrl + D"
                + Environment.NewLine + "Help - F3","Help");

        }
        private void Tb1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (e.Key == Key.Enter)
                {
                    ((TextBox)sender).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    if (((TextBox)sender).Name.Equals("Tb1")){
                        if (Bt1.Command != null)
                        {
                            Bt1.Command.Execute(null);
                        }
                    }
                    else
                    {
                        if (Bt2.Command != null)
                        {
                            Bt2.Command.Execute(null);
                        }
                    }
                }

            }
        }
        
        
        
                
                
       
        

        

        

        private void Cp_Button_Click(object sender, RoutedEventArgs e)
        {
            viewModel.WorkProgress = "Work in progress!";
            
        }
    }
}
