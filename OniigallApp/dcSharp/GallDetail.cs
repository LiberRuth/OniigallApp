using HtmlAgilityPack;
using OniigallApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

#nullable enable
namespace dcSniper.API
{
    internal class GallDetail
    {

        private HtmlNodeCollection? htmlNodes;
        private HtmlNodeCollection? contentsData;
        private HtmlNodeCollection? replynumData;
        private HtmlNodeCollection? mediaFileData;

        public string? errorMessage = null;

        public async Task Detail(string URL)
        {

            HttpClient httpClient = new HttpClient();
            string UA = RandomUA.UserAgent();
            //httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UA);
            httpClient.DefaultRequestHeaders.Add("User-Agent", UA);
            Debug.WriteLine($"Detail > {UA}");

            try
            {
                var response = await httpClient.GetAsync(URL);
                response.EnsureSuccessStatusCode();
                var gallhtml = await response.Content.ReadAsStringAsync();

                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(gallhtml);

                htmlNodes = doc.DocumentNode.SelectNodes("//div[@class='writing_view_box']/div[@class='write_div']");
                contentsData = doc.DocumentNode.SelectNodes("//div[@class='gallview_head clear ub-content']");
                replynumData = doc.DocumentNode.SelectNodes("//div[@class='btn_recommend_box recomuse_y morebox']/div[@class='inner_box'] " +
                    "| //div[@class='btn_recommend_box recomuse_n morebox']/div[@class='inner_box']");
                mediaFileData = doc.DocumentNode.SelectNodes("//div[@class='appending_file_box']/ul[@class='appending_file']/li/*");
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            
        }

        public async Task<List<IDictionary<string, string>>> DetailData() 
        {
            if (htmlNodes == null) { return null!; }

            string? all_html = null;
            List<IDictionary<string, string>> dataList = new List<IDictionary<string, string>>();

            foreach (var Nodes in htmlNodes!)
            {
                all_html = Nodes.OuterHtml;
            }

            string modifiedHtml = Regex.Replace(all_html!, @"<img\b([^>]*)>(.*?)", "&#&<img$1>$2&#&"); //이미지
            modifiedHtml = Regex.Replace(modifiedHtml, @"<video\b([^>]*)>(.*?)</video>", "&#&<video$1>$2</video>&#&"); //이미지 또는 미디어
            modifiedHtml = Regex.Replace(modifiedHtml, @"<iframe\b([^>]*)>(.*?)</iframe>", "&#&<iframe$1>$2</iframe>&#&"); //박스
            modifiedHtml = Regex.Replace(modifiedHtml, @"<embed\b(.*?)>", "&#&<embed$1></embed>&#&"); //유튜브 임베드
            // string modifiedHtml = Regex.Replace(html, @"<(.*?)\b([^>]*)>(.*?)</(.*?)>", "<div$1>$2</div>,");
            List<string> stringList = new List<string>(modifiedHtml.Split("&#&"));

            foreach (var item in stringList)
            {
                IDictionary<string, string> gallData = new Dictionary<string, string>();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(item);
                var imgHTML = doc.DocumentNode.SelectNodes("//img");
                var gifHTML = doc.DocumentNode.SelectSingleNode("//p/video | //p/div/video | //video[@playsinline='']");
                var embedHTML = doc.DocumentNode.SelectNodes("//embed");
                var iframeVideo = doc.DocumentNode.SelectNodes("//iframe[@style]");
                var iframeAudio = doc.DocumentNode.SelectNodes("//iframe[@width='280px' and  @height='54px']");

                if (imgHTML != null)
                {
                    foreach (HtmlNode imglink in imgHTML)
                    {
                        HtmlAttribute attImg = imglink.Attributes["src"];
                        gallData.Add("Image", attImg.Value);
                    }
                }
                else if (gifHTML != null) 
                {
                    HtmlAttribute attgif = gifHTML.Attributes["data-src"];
                    gallData.Add("GIF", attgif.Value);
                    /*
                    foreach (HtmlNode giflink in gifHTML)
                    {
                        HtmlAttribute attgif = giflink.Attributes["src"];
                        gallData.Add("GIF", attgif.Value);
                    }
                    */
                }
                else if (embedHTML != null)
                {
                    foreach (HtmlNode embedlink in embedHTML)
                    {
                        HtmlAttribute attembed = embedlink.Attributes["src"];
                        gallData.Add("Embed", attembed.Value);
                    }
                }
                else if (iframeVideo != null)
                {
                    foreach (HtmlNode vidlink in iframeVideo)
                    {
                        HtmlAttribute attvid = vidlink.Attributes["src"];
                        VideoHTML videoHTML = new VideoHTML();
                        await videoHTML.HTML(attvid.Value);
                        gallData.Add("Video", videoHTML.VideoURL());
                        gallData.Add("Video-Width", videoHTML.Width().ToString());
                        gallData.Add("Video-Height", videoHTML.Height().ToString());
                    }
                }
                else if (iframeAudio != null)
                {
                    foreach (HtmlNode audlink in iframeAudio)
                    {
                        HtmlAttribute attaud = audlink.Attributes["src"];
                        AudioHTML audioHTML = new AudioHTML();
                        await audioHTML.HTML(attaud.Value);
                        gallData.Add("Audio", audioHTML.AudioURL());
                    }
                }
                else
                {
                    gallData.Add("Html", item);
                }
                dataList.Add(gallData);
            }

            return dataList;
        }


        public IDictionary<string, string> GallUserData() 
        {
            if (contentsData == null) { return null!; }

            IDictionary<string, string> UserDataText = new Dictionary<string, string>();

            foreach (var Nodes in contentsData!)
            {
                
                var titleHeadtext = Nodes.SelectSingleNode(".//span[@class='title_headtext']"); //머리말
                var titleSubject = Nodes.SelectSingleNode(".//span[@class='title_subject']"); //제목

                UserDataText.Add("Title", titleHeadtext.InnerText + titleSubject.InnerText);

                var nickname_in = Nodes.SelectSingleNode(".//span[@class='nickname in']"); //고닉,반고닉
                var nickname = Nodes.SelectSingleNode(".//span[@class='nickname']"); //통피
                var nickname_ip = Nodes.SelectSingleNode(".//span[@class='ip']"); //ip

                if (nickname_in != null)
                {
                    UserDataText.Add("User", nickname_in.InnerText);
                    if (nickname_in.SelectSingleNode("//div[@class='fl']/a[@class='writer_nikcon ']/img[@src='https://nstatic.dcinside.com/dc/w/images/fix_nik.gif']") != null)
                    {
                        UserDataText.Add("UserTF", "True"); //고닉
                    }
                    else if (nickname_in.SelectSingleNode("//div[@class='fl']/a[@class='writer_nikcon ']/img[@src='https://nstatic.dcinside.com/dc/w/images/nik.gif']") != null)
                    {
                        UserDataText.Add("UserTF", "False"); //반고닉
                    }

                }
                else if (nickname != null && nickname_ip != null)
                {
                    UserDataText.Add("User", nickname.InnerText + nickname_ip.InnerText);
                }
                else if (nickname_ip == null) 
                {
                    UserDataText.Add("User", nickname!.InnerText);
                }
                else
                {
                    UserDataText.Add("User", "-");
                }

                var dateText = Nodes.SelectSingleNode(".//span[@class='gall_date']");

                if (dateText != null)
                {
                    HtmlAttribute attdate = dateText.Attributes["title"];
                    UserDataText.Add("Date", attdate.Value);
                }
                else 
                {
                    UserDataText.Add("Date", "-");
                }

                var gall_count = Nodes.SelectSingleNode(".//span[@class='gall_count']");

                if (gall_count != null) 
                {
                    UserDataText.Add("Count", gall_count.InnerText);
                }

                var gall_reply_num = Nodes.SelectSingleNode(".//span[@class='gall_reply_num']");

                if (gall_reply_num != null)
                {
                    UserDataText.Add("Replynum", gall_reply_num.InnerText);
                }

                var gall_comment = Nodes.SelectSingleNode(".//span[@class='gall_comment']");

                if (gall_comment != null)
                {
                    UserDataText.Add("Comment", gall_comment.InnerText);
                }
            }

            return UserDataText;
        }

        public IDictionary<string, string> RecommendBox()
        {
            if (replynumData == null) { return null!; }

            IDictionary<string, string> RecommendText = new Dictionary<string, string>();

            foreach (var Nodes in replynumData)
            {
                var up_num = Nodes.SelectSingleNode(".//div[@class='inner']/div[@class='up_num_box']/p[@class='up_num font_red']");
                var down_num = Nodes.SelectSingleNode(".//div[@class='inner']/div[@class='down_num_box']/p[@class='down_num']");

                if (up_num != null)
                {
                    RecommendText.Add("Up", up_num.InnerText);
                }
                else
                {
                    RecommendText.Add("Up", null!);
                }

                if (down_num != null)
                {
                    RecommendText.Add("Down", down_num.InnerText);
                }
                else
                {
                    RecommendText.Add("Down", null!);
                }
            }

            return RecommendText;
        }

        public List<IDictionary<string, string>> MediaFile()
        {
            if (mediaFileData == null) { return null!; }
            var fileData = new List<IDictionary<string, string>>();
            foreach (var itemMedia in mediaFileData)
            {
                var fileMetadata = new Dictionary<string, string>
                {
                    { itemMedia.InnerText, itemMedia.Attributes["href"].Value }
                };
                fileData.Add(fileMetadata);
            }
            return fileData;
        }

    }
}
