using dcSniper.API;
using System.Collections.ObjectModel;
using OniigallApp.UIFrame;
using System.Diagnostics;
using OniigallApp.Setting;

namespace OniigallApp.Views;

public partial class RecommendPage : ContentPage
{
    int page = 1;
    string gallURL = GallURL.RecommendURL;
    ObservableCollection<recommend_group_list> recommend_Group_List = new ObservableCollection<recommend_group_list>();

    public RecommendPage()
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
        //Title = $"{gallList.Gall_title()} 갤러리 - 개념글 ";
        Debug.WriteLine($"GET > {URL}");
        foreach (var data in gallListData)
        {
            bool isDuplicate = recommend_Group_List.Any(x => x.idNumber == data["Num"]);
            if (!isDuplicate)
            {
                if (data["Subject"] != "공지" & data["Num"] != "공지")
                {
                    recommend_Group_List.Add(new recommend_group_list()
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
            galllistViews.ItemsSource = recommend_Group_List;
            loadingbar.IsRunning = false;
            if (isScrolled) { isScrolled = false; }
        });

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

    private bool isScrolled = false;
    private async void galllistViews_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        if (isScrolled) return;
        double remainingItemsThreshold = 1.0;
        double remainingItems = recommend_Group_List.Count - e.LastVisibleItemIndex;
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
        recommend_Group_List = new ObservableCollection<recommend_group_list>();
        galllistViews.ItemsSource = null;
        page = 1;
        await Task.Delay(1500);
        GallListRun(gallURL);
    }
}