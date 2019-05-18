using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace PluginProject
{
    public class PluginCompresserModel
    {
        public static void CompressFile(string path, string name, bool comp)
        {
            if (comp)
            {
                ZipFile.CreateFromDirectory(path, name);
            }
            else
            {
                ZipFile.ExtractToDirectory(name, path);
            }
        }
    }
}
