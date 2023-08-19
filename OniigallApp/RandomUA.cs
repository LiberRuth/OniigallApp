﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OniigallApp
{
    internal class RandomUA
    {
        static Random rnd = new Random();

        private static string[] UserAgentList = new string[] {
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko; Google Web Preview) Chrome/27.0.1453 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5620.206 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5645.199 Safari/537.36 OPR/97.0.3475.102",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_14) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5648.209 Safari/537.36 Edg/113.0.1708.53",
            "Mozilla/5.0 (Windows NT 11.0; rv:102.0) Gecko/20100101 Firefox/102.0",
            "Mozilla/5.0 (X11; U; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5646.225 Safari/537.36",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/113.0.5626.198 Chrome/113.0.5626.198 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5632.213 Safari/537.36 Edg/111.0.1671.48",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5787.203 Safari/537.36 Edg/113.0.1754.58",
            "Mozilla/5.0 (X11; U; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5750.217 Safari/537.36",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5670.217 Safari/537.36 Edg/111.0.1686.34",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-GB; rv:120.0esr) Gecko/20102111 Firefox/120.0esr",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:121.0) Gecko/20070414 Firefox/121.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5640.201 Safari/537.36",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:111.0) Gecko/20171404 Firefox/111.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5653.204 Safari/537.36 Edg/113.0.1720.48",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; x64; rv:111.0) Gecko/20100101 Firefox/111.0",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.106 Safari/537.36 OPR/38.0.2220.41",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5665.210 Safari/537.36 OPR/96.0.3939.103",
            "Mozilla/5.0 (X11; CrOS x86_64 7077.95.0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.90 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_14_4) AppleWebKit/614.12 (KHTML, like Gecko) Version/14.0 Safari/614.12",
            "Mozilla/5.0 (X11; Linux aarch64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/88.0.4324.188 Safari/537.36 CrKey/1.54.250320",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5645.199 Safari/537.36 OPR/97.0.4349.54",
            "Mozilla/5.0 (X11; U; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5639.197 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5616.195 Safari/537.36",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5644.224 Safari/537.36 Edg/113.0.1697.43",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-US) Gecko/20112219 Firefox/119.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:108.0) Gecko/20100101 Firefox/108.0",
            "Mozilla/5.0 (Windows NT 11.0; rv:102.0) Gecko/20100101 Firefox/101.0",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64; rv:114.0) Gecko/20000101 Firefox/114.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_15) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5638.202 Safari/537.36 OPR/97.0.4204.63",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11; rv:118.0) Gecko/20100101 Firefox/118.0",
            "Mozilla/5.0 (X11; U; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5664.208 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5657.195 Safari/537.36 OPR/97.0.3324.44",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko EdgeClient/7222.2022.0715.1500",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko EdgeClient/7232.2022.1019.0458",
            "Mozilla/5.0 (X11; U; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Ubuntu Chromium/112.0.5628.224 Chrome/112.0.5628.224 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_7) AppleWebKit/537.36 (KHTML, like Gecko) Version/13.0.50 Safari/625.23.15",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Brave/107.0.5179.186 Safari/537.36",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5636.226 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_14) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5803.198 Safari/537.36 Edg/110.0.1762.61",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36",
            "Mozilla/5.0 (X11; U; Linux x86_64) Gecko/20101904 Firefox/122.0",
            "Mozilla/5.0 (X11; Linux x86_64; rv:114.0) Gecko/20170419 Firefox/114.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_12) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5622.223 Safari/537.36 Edg/112.0.1718.33",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5640.211 Safari/537.36 Edg/113.0.1669.40",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_12) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5664.203 Safari/537.36 OPR/97.0.4028.126",
            "Mozilla/5.0 (Windows NT 11.0; Win64; rv:112.0) Gecko/20110101 Firefox/112.0",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-US; rv:119.0) Gecko/20071114 Firefox/119.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5633.196 Safari/537.36",
            "Mozilla/5.0 (X11; Linux i686; en-GB) Gecko/20000904 Firefox/116.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36 Brave/107",
            "Mozilla/5.0 (Windows NT 11.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5665.202 Safari/537.36 OPR/96.0.3985.119+",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5634.194 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64; rv:111.0) Gecko/20100101 Firefox/111.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko EdgeClient/7220.2022.0308.1349",
            "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_11_6; rv:117.0esr) Gecko/20000101 Firefox/117.0esr",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_6_4) AppleWebKit/623.24 (KHTML, like Gecko) Version/11.1 Safari/623.24",
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5663.202 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_1_1; rv:118.0) Gecko/20110101 Firefox/120.0",
            "Mozilla/5.0 (X11; U; Linux i686; rv:112.0) Gecko/20012911 Firefox/112.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5654.214 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5648.201 Safari/537.36 OPR/96.0.3868.133",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_3) AppleWebKit/624.14.4 (KHTML, like Gecko) Version/12.6.97 Safari/624.14.4",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.0.0 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5651.223 Safari/537.36 OPR/97.0.4111.118",
            "Mozilla/5.0 (X11; Linux x86_64; en-US; rv:117.0) Gecko/20071619 Firefox/117.0",
            "Mozilla/5.0 (Windows NT 11.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5658.215 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_12) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5629.204 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:63.0) Gecko/20100101 Firefox/63.0",
            "Mozilla/5.0 (X11; Linux x86_64; en-US) Gecko/20002614 Firefox/110.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64; rv:121.0) Gecko/20000101 Firefox/121.0",
            "Mozilla/5.0 (X11; U; Linux x86_64; en-US) Gecko/20112104 Firefox/123.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5641.213 Safari/537.36 Edg/111.0.1710.37",
            "Mozilla/5.0 (X11; Linux x86_64) Gecko/20070419 Firefox/113.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_7_4; rv:111.0) Gecko/20110101 Firefox/111.0",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5617.211 Safari/537.36 OPR/97.0.3147.190",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64; rv:121.0esr) Gecko/20100101 Firefox/121.0esr",
            "Mozilla/5.0 (Windows NT 11.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5659.223 Safari/537.36 OPR/96.0.3412.30",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5634.209 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5663.202 Safari/537.36 Edg/112.0.1691.45",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_11) AppleWebKit/600.4.23 (KHTML, like Gecko) Version/12.2 Safari/629.23.14",
            "Mozilla/5.0 (Windows NT 10.0; rv:102.0) Gecko/20100101 Firefox/102.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5664.199 Safari/537.36 Edg/111.0.1713.38",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/115.0.5774.216 Safari/537.36 OPR/100.0.3321.173",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5645.210 Safari/537.36 OPR/97.0.3224.198",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_13; rv:119.0) Gecko/20000101 Firefox/119.0",
            "Mozilla/5.0 (X11; U; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5636.209 Safari/537.36",
            "Mozilla/5.0 (X11; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5666.216 Safari/537.36",
            "Mozilla/5.0 (Windows NT 11.0; Win64; rv:115.0) Gecko/20000101 Firefox/115.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5659.207 Safari/537.36 Edg/113.0.1698.61",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; Trident/7.0; rv:11.0) like Gecko EdgeClient/7232.2022.1019.0458",
            "Mozilla/5.0 (X11; U; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5653.205 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_5; rv:113.0) Gecko/20100101 Firefox/113.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:116.0) Gecko/20000101 Firefox/116.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:113.0esr) Gecko/20110101 Firefox/113.0esr",
            "Mozilla/5.0 (Windows NT 11.0; Win64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5643.224 Safari/537.36 OPR/97.0.4116.144",
            "Mozilla/5.0 (Windows NT 11.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5548.224 Safari/537.36",
            "Mozilla/5.0 (X11; Linux x86_64; en-US) Gecko/20161911 Firefox/113.0",
            "Mozilla/5.0 (Windows NT 10.0; Win64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5629.212 Safari/537.36",
            "Mozilla/5.0 (X11; U; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5645.196 Safari/537.36",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64; rv:111.0) Gecko/20000101 Firefox/111.0",
            "Mozilla/5.0 (X11; U; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5661.199 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5668.204 Safari/537.36 OPR/96.0.4186.190",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5510.206 Safari/537.36 Edg/113.0.1511.47",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_12) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5526.209 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64; rv:116.0esr) Gecko/20000101 Firefox/116.0esr",
            "Mozilla/5.0 (X11; U; Linux x86_64) Gecko/20010201 Firefox/113.0esr",
            "Mozilla/5.0 (X11; U; Linux i686; rv:115.0esr) Gecko/20161404 Firefox/115.0esr",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64; rv:117.0esr) Gecko/20000101 Firefox/117.0esr",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5626.204 Safari/537.36",
            "Mozilla/5.0 (Windows NT 10.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.5496.221 Safari/537.36 Edg/111.0.1545.36",
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36 Edg/114.0.1823.58",
            "Mozilla/5.0 (Windows NT 10.0; Win64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5529.221 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 11_0) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5643.212 Safari/537.36",
            "Mozilla/5.0 (X11; U; Linux x86_64) Gecko/20001914 Firefox/113.0",
            "Mozilla/5.0 (Windows NT 11.0; Win64; x64; rv:120.0) Gecko/20110101 Firefox/120.0",
            "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5540.209 Safari/537.36 Edg/110.0.1573.59",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_13) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5643.224 Safari/537.36 Edg/111.0.1686.52",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.5658.204 Safari/537.36 OPR/96.0.4135.50",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5511.211 Safari/537.36",
            "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_1) AppleWebKit/600.7.17 (KHTML, like Gecko) Version/14.6.76 Safari/620.36.4",
            "Mozilla/5.0 (Windows NT 11.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/112.0.5627.195 Safari/537.36 OPR/96.0.3602.25",
            "Mozilla/5.0 (X11; U; Linux i686) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/111.0.5504.197 Safari/537.36"};

        public static string UserAgent()
        {
            return UserAgentList[rnd.Next(0, UserAgentList.Length)];
        }
    }
}