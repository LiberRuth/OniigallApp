using System.Collections.ObjectModel;
using System.Diagnostics;
using OniigallApp.UIFrame;
using dcSniper.API;
using OniigallApp.Views;
using OniigallApp.Setting;

namespace OniigallApp;

public partial class MainPage : ContentPage
{
    int page = 1;
    string gallURL = GallURL.URL; 
    ObservableCollection<group_list> group_List = new ObservableCollection<group_list>();

    public MainPage()
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
        Title = $"{gallList.Gall_title()} 갤러리";
        Debug.WriteLine($"GET > {URL}");
        foreach (var data in gallListData)
        {
            bool isDuplicate = group_List.Any(x => x.idNumber == data["Num"]);
            if (!isDuplicate)
            {
                if (data["Subject"] != "공지" & data["Num"] != "공지")
                {
                    group_List.Add(new group_list()
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
            galllistViews.ItemsSource = group_List;
            loadingbar.IsRunning = false;
            if (isScrolled) { isScrolled = false; }
        });
    }

    private async void ListViews_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (galllistViews.SelectedItem != null)
        {
            var selectedItem = galllistViews.SelectedItem;
            Debug.WriteLine(selectedItem.ToString());
            await Navigation.PushAsync(new DetailPage(selectedItem.ToString()));
            galllistViews.SelectedItem = null;
        }
    }

    private bool isScrolled = false;
    private async void ListViews_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if (isScrolled) return;
        double remainingItemsThreshold = 1.0;
        double remainingItems = group_List.Count - e.LastVisibleItemIndex;
        if (remainingItems <= remainingItemsThreshold)
        {
            isScrolled = true;
            await Task.Delay(1000);
            LoadMoreItems();
            isScrolled = false;
        }
    }

    private void LoadMoreItems()
    {
        GallListRun($"{gallURL}&page={++page}");
    }

    private async void List_Refresh(object sender, EventArgs e)
    {
        isScrolled = true;
        group_List = new ObservableCollection<group_list>();
        galllistViews.ItemsSource = null;
        page = 1;
        await Task.Delay(1500);
        GallListRun(gallURL);
    }
}

