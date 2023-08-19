using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OniigallApp.UIFrame
{
    internal class file_group
    {
        public string fileName { get; set; }
        public string fileUrl { get; set; }

        public override string ToString()
        {
            return fileUrl;
        }
    }
}
