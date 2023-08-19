using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OniigallApp.UIFrame
{
    internal class type_list
    {
        public string text { get; set; }
        public string id { get; set; }

        public override string ToString()
        {
            return id;
        }
    }
}
