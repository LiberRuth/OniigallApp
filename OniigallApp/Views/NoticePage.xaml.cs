using dcSniper.API;
using OniigallApp.Setting;
using OniigallApp.UIFrame;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace OniigallApp.Views;

public partial class NoticePage : ContentPage
{
    string gallURL = GallURL.NoticeURL;
    ObservableCollection<notice_group_list> notice_group_List = new ObservableCollection<notice_group_list>();

    public NoticePage()
	{
		InitializeComponent();
        GallListRun(gallURL);
    }

    private async void GallListRun(string URL)
    {
        loadingbar.IsRunning = true;

        GallList gallList = new GallList();
        await gallList.Information_listAsync(URL);
        if (gallList.errorMessage != null) { await DisplayAlert("HTTP Error", gallList.errorMessage, "OK"); return; }
        var gallListData = gallList.Gall_list();

        foreach (var data in gallListData)
        {
            bool isDuplicate = notice_group_List.Any(x => x.detailUrl == $"https://gall.dcinside.com/{data["GallURL"]}");
            if (!isDuplicate)
            {
                notice_group_List.Add(new notice_group_list()
                {
                    title = data["Title"],
                    reply = data["Reply"],
                    userName = data["User"],
                    views = $"조회수 {data["Count"]}",
                    recommend = $"추천수 {data["Recommend"]}",
                    time = data["Date"],
                    detailUrl = $"https://gall.dcinside.com{data["GallURL"]}"
                });
            }

        }

        MainThread.BeginInvokeOnMainThread(() =>
        {
            galllistViews.ItemsSource = notice_group_List;
            loadingbar.IsRunning = false;
        });

    }


    private async void GalllistViews_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (galllistViews.SelectedItem != null)
        {
            var selectedItem = galllistViews.SelectedItem;
            await Navigation.PushAsync(new DetailPage(selectedItem.ToString()));
            galllistViews.SelectedItem = null;
        }
    }

    private async void List_Refresh(object sender, EventArgs e)
    {
        notice_group_List = new ObservableCollection<notice_group_list>();
        galllistViews.ItemsSource = null;
        await Task.Delay(1500);
        GallListRun(gallURL);
    }
}