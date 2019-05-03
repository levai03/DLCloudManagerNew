using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLCloudManager.Models
{
    class CloudType
    {
        private string ID;
        private string name;

        public string ID1 { get => ID; set => ID = value; }
        public string Name { get => name; set => name = value; }
        public CloudType()
        {

        }
        public CloudType(string id, string name)
        {
            this.ID = id;
            this.name = name;
        }
    }
}
