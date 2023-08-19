using HtmlAgilityPack;
using OniigallApp;

#nullable enable
namespace dcSniper.API
{
    internal class AudioHTML
    {
        private HtmlNodeCollection? htmlNodes;

        public async Task HTML(string URL)
        {
            if (URL.Contains("&vr_open=1&type=W") == false) { URL += "&vr_open=1&type=W"; }

            string UA = RandomUA.UserAgent();
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            httpClient.DefaultRequestHeaders.Add("User-Agent", UA);
            httpClient.DefaultRequestHeaders.Referrer = new Uri("https://gall.dcinside.com/");
            try
            {
                var response = await httpClient.GetAsync(URL);
                response.EnsureSuccessStatusCode();

                var html = await response.Content.ReadAsStringAsync();
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                htmlNodes = doc.DocumentNode.SelectNodes("//body");

            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
            }

        }

        public string AudioURL()
        {
            string reHTML = "";
            foreach (var Nodes in htmlNodes!)
            {
                foreach (HtmlNode videolink in Nodes.SelectNodes(".//input[@value]"))
                {
                    HtmlAttribute attvideo = videolink.Attributes["value"];
                    reHTML = attvideo.Value;
                }
            }
            return reHTML;
        }
    }
}
