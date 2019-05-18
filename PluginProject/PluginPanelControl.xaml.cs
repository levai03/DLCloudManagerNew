using System;
using System.ComponentModel.Composition;
using InternalShared;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PluginProject
{
    /// <summary>
    /// Interaction logic for PluginPanelControl.xaml
    /// </summary>
    [Export(typeof(IView)), PartCreationPolicy(CreationPolicy.Any)]
    [ExportMetadata("Name", "Plugin Zipper")]
    public partial class PluginPanelControl : UserControl
    {
        string path;
        string name1;
        bool comp;

        public PluginPanelControl()
        {
            InitializeComponent();
        }

        public string Path { get => path; set => path = value; }
        public string Name1 { get => name1; set => name1 = value; }
        public bool Comp { get => comp; set => comp = value; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PluginCompresserModel.CompressFile(path, name1, comp);
        }
    }
}
