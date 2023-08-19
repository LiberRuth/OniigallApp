using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OniigallApp.UIFrame
{
    class comment_page
    {
        public int id { get; set; }
        public int number { get; set; }

        public override string ToString()
        {
            return number.ToString();
        }
    }
}
