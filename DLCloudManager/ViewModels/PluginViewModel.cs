using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using System.ComponentModel.Composition.Hosting;
using InternalShared;

namespace DLCloudManager.ViewModels
{
    class PluginViewModel : INotifyPropertyChanged
    {
        public ICommand ListingPluginCommand => _listingPluginCommand;
        private readonly DelegateCommand _listingPluginCommand;
        private AggregateCatalog catalog;
        private CompositionContainer container;
        private IView PluginViewVar;

        
        [Import(typeof(IView),AllowDefault = true)]
        public IView PluginView
        {
          get { return PluginViewVar; }
          set
          {
             PluginViewVar = value;
             OnPropertyChanged("PluginView");
          }
        }
    
        public PluginViewModel()
        {
            //_listingPluginCommand = new DelegateCommand(OnPlugin);
            catalog = new AggregateCatalog();
            string pluginsPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            catalog.Catalogs.Add(new DirectoryCatalog(pluginsPath, "Plugin*.dll"));
            pluginsPath = Path.Combine(pluginsPath, "plugins");
            if (!Directory.Exists(pluginsPath))
            {
                Directory.CreateDirectory(pluginsPath);
            }
            catalog.Catalogs.Add(new DirectoryCatalog(pluginsPath, "Plugin*.dll"));
            container = new CompositionContainer(catalog);
        }
    
        public void OnPlugin()
        {
            container.ComposeParts(this);
    
        }



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
