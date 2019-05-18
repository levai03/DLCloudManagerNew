using System;
using System.Collections;
using System.ComponentModel;
using System.Xml;

namespace DLCloudManager
{
    
    partial class ApplicationToInstall: System.Configuration.Install.Installer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);

            try
            {
                AddConfigurationFileDetails();
            }
            catch (Exception e)
            {
                
                base.Rollback(savedState);
            }
        }
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }
        private void AddConfigurationFileDetails()
        {
            try
            {
                string DEFTOK = Context.Parameters["DEFTOK"];

                 
                string assemblypath = Context.Parameters["assemblypath"];
                string appConfigPath = assemblypath + ".config";

                
                XmlDocument doc = new XmlDocument();
                doc.Load(appConfigPath);

                XmlNode configuration = null;
                foreach (XmlNode node in doc.ChildNodes)
                    if (node.Name == "configuration")
                        configuration = node;

                if (configuration != null)
                {
                    
                    XmlNode settingNode = null;
                    foreach (XmlNode node in configuration.ChildNodes)
                    {
                        if (node.Name == "appSettings")
                            settingNode = node;
                    }

                    if (settingNode != null)
                    {
                        foreach (XmlNode node in settingNode.ChildNodes)
                        {
                            if (node.Attributes == null)
                                continue;
                            XmlAttribute attribute = node.Attributes["value"]; 
                            if (node.Attributes["key"] != null)
                            { 
                                switch (node.Attributes["key"].Value)
                                {
                                    case "DEFTOK":
                                        attribute.Value = DEFTOK;
                                        break;
                                }
                            }
                        }
                    }
                    doc.Save(appConfigPath);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}