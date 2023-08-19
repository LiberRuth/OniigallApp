using System;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OniigallApp;

#nullable enable
namespace dcSniper.API
{
    internal class GallList
    {
        private HtmlNodeCollection? htmlNodes_list;
        private HtmlNodeCollection? htmlNodes_title;
        private HtmlNodeCollection? htmlNodes_PageNumber;
        private HtmlNodeCollection? htmlNodes_PageSearchNumber;
        private HtmlNodeCollection? htmlNodes_NewPageSearch;
        private HtmlNodeCollection? htmlNodes_typeNetPageEnd;
        private HtmlNodeCollection? htmlNodes_typeSearchNetPageEnd;

        public string? errorMessage = null;

        public async Task Information_listAsync(string URL) 
        {
          
            HttpClient httpClient = new HttpClient();
            string UA = RandomUA.UserAgent();
            Debug.WriteLine($"List > {UA}");
            try
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", UA);
                var response = await httpClient.GetAsync(URL);
                response.EnsureSuccessStatusCode();
                var html = await response.Content.ReadAsStringAsync();

                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                htmlNodes_list = doc.DocumentNode.SelectNodes("//tbody/tr[@class='ub-content us-post']");
                htmlNodes_title = doc.DocumentNode.SelectNodes("//div/div[@class='tab_menubox']");
                htmlNodes_PageNumber = doc.DocumentNode.SelectNodes("//div[@class='bottom_paging_wrap']/div[@class='bottom_paging_box iconpaging']/*");
                htmlNodes_PageSearchNumber = doc.DocumentNode.SelectNodes("//div[@class='bottom_paging_wrap re']/div[@class='bottom_paging_box iconpaging']/*");
                htmlNodes_NewPageSearch = doc.DocumentNode.SelectNodes("//div/div[@class='bottom_paging_box iconpaging']/a[@class='search_next']");
                htmlNodes_typeNetPageEnd = doc.DocumentNode.SelectNodes("//div/a[@class='search_next']");
                htmlNodes_typeSearchNetPageEnd = doc.DocumentNode.SelectNodes("//div/div[@class='bottom_paging_box iconpaging']/a[@class='search_next']");

            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;  
            }

        }


        public string Gall_title() 
        {
            if (htmlNodes_title == null) { return null!; }

            string dataTitle = "";
            foreach (var Nodes in htmlNodes_title!)
            {
                dataTitle =  Nodes.SelectSingleNode(".//p[@class='gallname']").InnerText;
            }
            return dataTitle;       
        }


        public List<IDictionary<string, string>> Gall_list() 
        {
            if (htmlNodes_list == null) { return null!; }

            List<IDictionary<string, string>>? dataList = new List<IDictionary<string, string>>();

            foreach (var Nodes in htmlNodes_list!)
            {

                IDictionary<string, string> dataIDictionary = new Dictionary<string, string>();

                var gallNum = Nodes.SelectNodes(".//td[@class='gall_num']");
                foreach (var num in gallNum)
                {
                    dataIDictionary.Add("Num", num.InnerText);
                }

                var gallSubject = Nodes.SelectNodes(".//td[@class='gall_subject']");
                if (gallSubject != null)
                {
                    foreach (var subject in gallSubject)
                    {
                        dataIDictionary.Add("Subject", subject.InnerText);
                    }
                }
                else 
                {
                    dataIDictionary.Add("Subject", null!);
                }

                var gallTitle = Nodes.SelectNodes(".//td[@class]/a[@view-msg]");
                foreach (var title in gallTitle)
                {
                    dataIDictionary.Add("Title", title.InnerText);
                }

                var replyNumbox = Nodes.SelectNodes(".//td[@class='gall_tit ub-word']/a[@class='reply_numbox']/span");
                if (replyNumbox != null)
                {
                    foreach (var reply in replyNumbox)
                    {
                        dataIDictionary.Add("Reply", reply.InnerText);
                    }

                }
                else
                {
                    dataIDictionary.Add("Reply", "[0]");
                }

                var pageURL = Nodes.SelectNodes(".//td[@class='gall_tit ub-word'] | .//td[@class='gall_tit ub-word voice_tit']");
                if (pageURL != null)
                {
                    foreach (var hrefText in pageURL)
                    {
                        var galllink = hrefText.SelectSingleNode(".//a[@href]").Attributes["href"];
                        dataIDictionary.Add("GallURL", galllink.Value);
                    }
                }

                var userName_member = Nodes.SelectNodes(".//td[@class='gall_writer ub-writer']/span[@class='nickname in']");
                var userName_Nonmembers = Nodes.SelectNodes(".//td[@class='gall_writer ub-writer']/span[@class='nickname']");
                var userIP = Nodes.SelectNodes(".//td[@class='gall_writer ub-writer']/span[@class='ip']");
                var userName_notification = Nodes.SelectNodes(".//td[@class='gall_writer ub-writer']/b");

                if (userName_member != null)
                {
                    foreach (var user in userName_member)
                    {
                        dataIDictionary.Add("User", user.InnerText);
                    }
                }
                else if (userName_notification != null)
                {
                    foreach (var user in userName_notification)
                    {
                        dataIDictionary.Add("User", user.InnerText);
                    }

                }
                else
                {
                    string IP_plus = "";
                    foreach (var user in userName_Nonmembers)
                    {
                        IP_plus = user.InnerText;
                    }
                    if (userIP != null)
                    {
                        foreach (var txtIP in userIP)
                        {
                            IP_plus += txtIP.InnerText;
                        }
                    }
                    dataIDictionary.Add("User", IP_plus);

                }

                var gallDate = Nodes.SelectNodes(".//td[@class='gall_date']");
                foreach (var date in gallDate)
                {
                    dataIDictionary.Add("Date", date.InnerText);
                }

                var gallCount = Nodes.SelectNodes(".//td[@class='gall_count']");
                foreach (var count in gallCount)
                {
                    dataIDictionary.Add("Count", count.InnerText);
                }

                var gallRecommend = Nodes.SelectNodes(".//td[@class='gall_recommend']");
                foreach (var recommend in gallRecommend)
                {
                    dataIDictionary.Add("Recommend", recommend.InnerText);
                }

                dataList.Add(dataIDictionary);

            }

            return dataList;
        }

        public int MaxPaging()
        {
            if (htmlNodes_typeNetPageEnd != null)
            {
                foreach (var Nodes in htmlNodes_typeNetPageEnd)
                {
                    string nextURL = Nodes.Attributes["href"].Value; 
                    string pattern = @"page=(\d+)";
                    Match match = Regex.Match(nextURL, pattern);
                    string pageValue = match.Groups[1].Value;
                    int pageNumber = int.Parse(pageValue);
                    return pageNumber;
                }
            }
            else
            {
                int pageNumber = 0;
                foreach (var Nodes in htmlNodes_PageNumber!)
                {
                    pageNumber++;
                }
                return pageNumber;
            }
            return 1;
        }

        public int MaxSearchPaging()
        {
            if (htmlNodes_PageSearchNumber == null) return 0;
            int pageNumber = 0;
            foreach (var Nodes in htmlNodes_PageSearchNumber)
            {
                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(Nodes.OuterHtml);
                var next_paging = htmlDocument.DocumentNode.Descendants("a")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "") == "sp_pagingicon page_end");
                var paging_number = htmlDocument.DocumentNode.Descendants("a")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "") == "search_next");
                if (next_paging != null)
                {
                    string nextURL = next_paging.Attributes["href"].Value;
                    string pattern = @"page=(\d+)";
                    Match match = Regex.Match(nextURL, pattern);
                    string pageValue = match.Groups[1].Value;
                    return int.Parse(pageValue);
                }
                if (paging_number == null) pageNumber++;
            }
            return pageNumber;
        }

        public string NewPageSearch()
        {
            if (htmlNodes_NewPageSearch == null) { return null!; }
            string? newNextURL = "";
            foreach (var Nodes in htmlNodes_NewPageSearch!)
            {
                newNextURL = $"https://gall.dcinside.com{Nodes.Attributes["href"].Value}";
            }
            return newNextURL;
        }

        public bool TypeNextPageEnd()
        {
            if (htmlNodes_typeNetPageEnd != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TypeSearchNextPageEnd()
        {
            if (htmlNodes_typeSearchNetPageEnd != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
