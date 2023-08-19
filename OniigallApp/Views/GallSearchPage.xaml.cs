using dcSniper.API;
using OniigallApp.Setting;
using OniigallApp.UIFrame;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;

namespace OniigallApp.Views;

public partial class GallSearchPage : ContentPage
{
    int page;
    int pageMax;
    bool pageRepeat = false;
    string newURL;
    string gallURL = GallURL.URL;
    string temporary_url;
    ObservableCollection<type_list> type_Lists = new ObservableCollection<type_list>();
    ObservableCollection<search_group_list> search_Group_List = new ObservableCollection<search_group_list>();


    public GallSearchPage()
	{
		InitializeComponent();
        Type_list_add();
        //search_Group_Lists.Add(new search_group_list() { title = "제목", reply = "조회수", userName = "유저명", views = "조회수", recommend = "추천수", time = "시간", detailUrl = "게시물URL" });
        //galllistViews.ItemsSource = search_Group_Lists;      
    }

    private async void Entry_Completed(object sender, EventArgs e)
    {
        if (entry.Text == null | entry.Text == "") { await DisplayAlert("정보", "검색어를 입력하세요", "OK"); return; }

        string encodeText = HttpUtility.UrlEncode(entry.Text, Encoding.UTF8).ToUpper();
        encodeText = encodeText.Replace("%", ".");
        encodeText = encodeText.Replace("+", ".20");
        temporary_url = gallURL;
        page = 1;

        switch (searchtype.SelectedItem.ToString())
        {
            case "title_and_text":
                temporary_url += $"&s_type=search_subject_memo&s_keyword={encodeText}";
                break;
            case "title":
                temporary_url += $"&s_type=search_subject&s_keyword={encodeText}";
                break;
            case "text":
                temporary_url += $"&s_type=search_subject&s_keyword={encodeText}";
                break;
            case "user_name":
                temporary_url += $"&s_type=search_name&s_keyword={encodeText}";
                break;
        }

        if (gallCheckBox.IsChecked) { temporary_url += "&exception_mode=recommend"; }

        galllistViews.ItemsSource = null;
        search_Group_List = new ObservableCollection<search_group_list>();
        requestListAdd.IsVisible = true;
        Debug.WriteLine($"GET > {temporary_url}");
        GallListRun(temporary_url);
    }

    private async void GallListRun(string URL)
    {
        loadingbar.IsRunning = true;
        loadingbar.IsVisible = true;

        GallList gallList = new GallList();
        await gallList.Information_listAsync(URL);
        if (gallList.errorMessage != null) { await DisplayAlert("HTTP Error", gallList.errorMessage, "OK"); return; }
        var gallListData = gallList.Gall_list();
        if (gallListData == null) { loadingbar.IsRunning = false; loadingbar.IsVisible = false; return; }
        Debug.WriteLine($"GET > {URL}");

        foreach (var data in gallListData)
        {
            bool isDuplicate = search_Group_List.Any(x => x.idNumber == data["Num"]);
            if (!isDuplicate)
            {
                if (data["Subject"] != "공지" & data["Num"] != "공지")
                {
                    search_Group_List.Add(new search_group_list()
                    {
                        idNumber = data["Num"],
                        title = data["Title"],
                        reply = data["Reply"],
                        userName = data["User"],
                        views = $"조회수 {data["Count"]}",
                        recommend = $"추천수 {data["Recommend"]}",
                        time = data["Date"],
                        subject = data["Subject"],
                        detailUrl = $"https://gall.dcinside.com{data["GallURL"]}"
                    });
                }
            }

        }

        MainThread.BeginInvokeOnMainThread(() =>
        {

#if ANDROID
            galllistViews.ItemsSource = null;
#endif
            galllistViews.ItemsSource = search_Group_List;
            loadingbar.IsRunning = false;
            loadingbar.IsVisible = false;
   
        });

    }

    private void Type_list_add()
    {
        type_Lists.Add(new type_list() { text = "제목+내용", id = "title_and_text" });
        type_Lists.Add(new type_list() { text = "제목", id = "title" });
        type_Lists.Add(new type_list() { text = "내용", id = "text" });
        type_Lists.Add(new type_list() { text = "글쓴이", id = "user_name" });
        searchtype.ItemsSource = type_Lists;
        searchtype.SelectedIndex = 0;
    }

    private async void GalllistViews_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (galllistViews.SelectedItem != null)
        {
            var selectedItem = galllistViews.SelectedItem;
            Debug.WriteLine(selectedItem.ToString());
            await Navigation.PushAsync(new DetailPage(selectedItem.ToString()));
            galllistViews.SelectedItem = null;
        }
    }

    private async void Btn_RequestListAdd(object sender, EventArgs e)
    {

        var urls = await RequestListAnalyze(temporary_url);
        GallListRun(urls);
        //Debug.WriteLine(urls);
    }

    private async Task<string> RequestListAnalyze(string url) 
    {
        if (!pageRepeat)
        {
            GallList gallList = new GallList();
            await gallList.Information_listAsync(temporary_url);
            pageMax = gallList.MaxSearchPaging();
            newURL = gallList.NewPageSearch();
            newURL = newURL.Replace("&page=1", "");
            page = 1;
        }

        if (page < pageMax)
        {
            pageRepeat = true;
            page++;
            return $"{temporary_url}&page={page}";
        }
        pageRepeat = false;
        temporary_url = newURL;
        return newURL;

    }
}