using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OniigallApp.UIFrame
{
    internal class group_list
    {
        public string idNumber { get; set; }
        public string title { get; set; }
        public string userName { get; set; }
        public string views { get; set; }
        public string reply { get; set; }
        public string recommend { get; set; }
        public string time { get; set; }
        public string subject { get; set; }
        public string detailUrl { get; set; }

        public override string ToString()
        {
            return detailUrl;
        }
    }
}
