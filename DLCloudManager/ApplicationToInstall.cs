using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace DLCloudManager
{
    [RunInstaller(true)]
    public partial class ApplicationToInstall : System.Configuration.Install.Installer
    {
        public ApplicationToInstall()
        {
            InitializeComponent();
        }
    }
}
