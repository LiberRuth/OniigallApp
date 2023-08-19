using HtmlAgilityPack;
using Newtonsoft.Json;
using OniigallApp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

#nullable enable
namespace dcSniper.API
{
    internal class RequestComments
    {
        private string? galltype_valueKey;
        private string? esno_valueKey;

        public string? errorMessage = null;

        public class Comment
        {
            public string? no { get; set; }
            public string? parent { get; set; }
            public string? user_id { get; set; }
            public string? name { get; set; }
            public string? ip { get; set; }
            public string? reg_date { get; set; }
            public string? nicktype { get; set; }
            public string? t_ch1 { get; set; }
            public string? t_ch2 { get; set; }
            public string? vr_type { get; set; }
            public object? voice { get; set; }
            public string? rcnt { get; set; }
            public string? c_no { get; set; }
            public int? depth { get; set; }
            public string? del_yn { get; set; }
            public string? is_delete { get; set; }
            public string? password_pop { get; set; }
            public string? memo { get; set; }
            public string? my_cmt { get; set; }
            public string? del_btn { get; set; }
            public string? mod_btn { get; set; }
            public string? a_my_cmt { get; set; }
            public string? reply_w { get; set; }
            public string? gallog_icon { get; set; }
            public bool vr_player { get; set; }
            public string? vr_player_tag { get; set; }
        }

        public class RootObject
        {
            public int total_cnt { get; set; }
            public int comment_cnt { get; set; }
            public List<Comment>? comments { get; set; }
        }

        private async Task CommentHTMLData(string URL) 
        {
            HttpClient client = new HttpClient();
            string UA = RandomUA.UserAgent();
            client.DefaultRequestHeaders.Add("Accept", "*/*");
            client.DefaultRequestHeaders.Add("User-Agent", UA);
            var response = await client.GetAsync(URL);
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            galltype_valueKey = doc.DocumentNode.SelectSingleNode("//input[@id='_GALLTYPE_']").Attributes["value"].Value;
            esno_valueKey = doc.DocumentNode.SelectSingleNode("//input[@id='e_s_n_o']").Attributes["value"].Value;
        }

        public async Task<string> RequestText(string target_url, int page)
        {
            string UA = RandomUA.UserAgent();
            Debug.WriteLine($"Comment > {UA}");
            HttpClient client = new HttpClient();
            string url = "https://gall.dcinside.com/board/comment/";

            string pattern = @"\bid=(\w+)&no=(\d+)\b";
            Match match = Regex.Match(target_url, pattern);
            string boardId = match.Groups[1].Value;
            string postId = match.Groups[2].Value;

            await CommentHTMLData(target_url);

            var postData = new Dictionary<string, string>
            {
                { "id", boardId },
                { "no", postId },
                { "cmt_id", boardId },
                { "cmt_no", postId },
                { "focus_cno", "" },
                { "focus_pno", "" },
                { "e_s_n_o", esno_valueKey! },
                { "comment_page", page.ToString() },
                { "sort", "D" },
                { "prevCnt", "0" },
                { "board_type", "0" },
                { "_GALLTYPE_", galltype_valueKey! }
            };

            client.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
            client.DefaultRequestHeaders.Add("User-Agent", UA);
            client.DefaultRequestHeaders.Add("Host", "gall.dcinside.com");
            client.DefaultRequestHeaders.Add("Accept-Language", "ko-KR,ko;q=0.9");
            client.DefaultRequestHeaders.Add("Dnt", "1");
            client.DefaultRequestHeaders.Add("Origin", "https://gall.dcinside.com");
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Referrer = new Uri("https://gall.dcinside.com/");

            var content = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                //Console.WriteLine("요청이 실패하였습니다. 상태 코드: " + response.StatusCode);
                errorMessage = response.StatusCode.ToString();
                return null!;
            }

        }
    }
}
