using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OniigallApp.Setting
{
    public static class GallURL
    {
        public static string URL { get; set; } = "https://gall.dcinside.com/mgallery/board/lists?id=onii";
        public static string RecommendURL
        {
            get { return $"{URL}&exception_mode=recommend"; }
        }
        public static string NoticeURL
        {
            get { return $"{URL}&exception_mode=notice"; }
        }
    }
}
