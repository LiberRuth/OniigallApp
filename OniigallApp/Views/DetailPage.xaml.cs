using dcSniper.API;
using LiteHtmlMaui.Controls;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CommunityToolkit.Maui.Views;
using OniigallApp.UIFrame;
using System.Collections.ObjectModel;
using HtmlAgilityPack;
using OniigallApp.MediaRendering;

namespace OniigallApp.Views;

public partial class DetailPage : ContentPage
{
    private int commentPage = 0; 
    private int commentCountPage = 1;
    private string temporaryURL;
    private List<IDictionary<string, string>> mediaData;

    ImageRendering imageRequest = new ImageRendering();

    ObservableCollection<comment_group> comment_Group = new ObservableCollection<comment_group>();
    ObservableCollection<comment_page> comment_Page = new ObservableCollection<comment_page>();

    public DetailPage(string URL)
	{
		InitializeComponent();
        temporaryURL = URL;
        GallDataRequest(URL);
	}

	private void GallDataRequest(string target_url) 
	{
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            int index = 0;
            loadingbar.IsRunning = true;
#if ANDROID
            await Task.Delay(1500);
#elif WINDOWS || IOS || MACCATALYST || TIZEN
            await Task.Delay(100);
#endif
            GallDetail gallDetail = new GallDetail();
            await gallDetail.Detail(target_url);
            if (gallDetail.errorMessage != null) { await DisplayAlert("HTTP Erro", gallDetail.errorMessage, "OK"); return; }
            var user_data = gallDetail.GallUserData();
            var detail_data = await gallDetail.DetailData();
            var replynumBox = gallDetail.RecommendBox();
            mediaData = gallDetail.MediaFile();
            loadingbar.IsRunning = false;

            textTitle.Text = user_data["Title"];
            Title = user_data["Title"];
            userText.Text = user_data["User"];
            countText.Text = user_data["Count"];
            commentText.Text = user_data["Comment"];
            replynumText.Text = user_data["Replynum"];
            dateText.Text = user_data["Date"];

            string commentStr = user_data["Comment"].Replace("댓글 ", "");
            commentStr = commentStr.Replace(",", "");
            int commentCount = int.Parse(commentStr); 
            while (true) { commentPage++; commentCount -= 100; if (commentCount <= 0) { break; } }          

            foreach (var detailDataText in detail_data)
            {
                loadingbar.IsRunning = true;
                await Task.Delay(100);
                if (detailDataText.ContainsKey("Image"))
                {
                    var imgUrl = detailDataText["Image"];;
                    var imageView = new Image
                    {
                        Source = await imageRequest.LoadImage(imgUrl),
                        IsAnimationPlaying = true,
                        AutomationId = index.ToString()
                    };

                    imageView.HeightRequest = imageRequest.LoadImageHeight();
                    //imageView.WidthRequest = imageRequest.LoadImageHeight();

                    stackLayout.Children.Insert(index, imageView);
                }
                else if (detailDataText.ContainsKey("GIF"))
                {
                    var gifUrl = detailDataText["GIF"];
                    var gifView = new Image
                    {
                        Source = await imageRequest.LoadImage(gifUrl),
                        IsAnimationPlaying = true,
                        AutomationId = index.ToString()
                    };

                    gifView.HeightRequest = imageRequest.LoadImageHeight();
                    //gifView.WidthRequest = imageRequest.LoadImageWidth();

                    stackLayout.Children.Insert(index, gifView);
                }
                else if (detailDataText.ContainsKey("Embed"))
                {
                    var EmbedText = detailDataText["Embed"];
                    var labelView = new Label
                    {
                        Text = EmbedText
                    };
                    stackLayout.Children.Insert(index, labelView);
                }
                else if (detailDataText.ContainsKey("Video"))
                {
                    var textLabel = new Label
                    {
                        Text = "현재 [MP4] 미지원 합니다..."
                    };
                    stackLayout.Children.Insert(index, textLabel);
                }
                else if (detailDataText.ContainsKey("Audio")) 
                {
                    var audioUrl = detailDataText["Audio"];
                    var audioPaly = new MediaElement
                    {
                        Source = audioUrl,
                    };
#if ANDROID
                    audioPaly.HeightRequest = 300;
                    audioPaly.WidthRequest = 400;
#elif WINDOWS || IOS || MACCATALYST || TIZEN
                    audioPaly.WidthRequest = 400;
#endif
                    stackLayout.Children.Insert(index, audioPaly);
                }
                else if (detailDataText.ContainsKey("Html"))
                {
                    var HtmlView = detailDataText["Html"];
                    var htmlText = new LiteHtml
                    {
                        Html = HtmlView,
                        HorizontalOptions = LayoutOptions.Center,
                    };

                    stackLayout.Children.Insert(index, htmlText);
                }

                index++;
                loadingbar.IsRunning = false;
            }

            if (replynumBox.ContainsKey("Up"))
            {
                if (replynumBox["Up"] == null) { btnUpText.IsVisible = false; }
                btnUpText.Text = $"추천: {replynumBox["Up"]}";
            }
            else 
            {
                btnUpText.IsVisible = false;
            }

            if (replynumBox.ContainsKey("Down"))
            {
                if (replynumBox["Down"] == null) { btnDownText.IsVisible = false; }
                btnDownText.Text = $"비추천: {replynumBox["Down"]}";
            }
            else 
            {
                btnDownText.IsVisible = false;
            }

            PickerCommentPage();
        });

        MainThread.BeginInvokeOnMainThread(() =>
        {
            CommentRequest(target_url, commentCountPage);
        });
    }

    private void PickerCommentPage() 
    {
        for (int i = 1; i < commentPage + 1; i++)
        {
            comment_Page.Add(new comment_page()
            {
                id = i,
                number = i,
            });
        }
        commentPicker.ItemsSource = null;
        commentPicker.ItemsSource = comment_Page;
        commentPicker.SelectedIndex = 0;
    }


    private async Task<StreamImageSource> ImageConversion(string HTML)
    {
        HtmlDocument doc = new HtmlDocument();
        doc.LoadHtml(HTML);
        var imgTy = doc.DocumentNode.SelectSingleNode(".//img[@class='written_dccon']");
        var videoTy = doc.DocumentNode.SelectSingleNode(".//video[@class='written_dccon']");

        if (imgTy != null)
        {
            return await imageRequest.LoadImage(imgTy.Attributes["src"].Value);
        }
        else if (videoTy != null) 
        {
            return await imageRequest.LoadImage(videoTy.Attributes["data-src"].Value);
        }

        return null;
    }

    private async void CommentRequest(string URL, int Page)
    {      
        RequestComments requestComments = new RequestComments();
        var jsonText = await requestComments.RequestText(URL, Page);
        var commentsData = JsonConvert.DeserializeObject<RequestComments.RootObject>(jsonText!)!;
        if (requestComments.errorMessage != null) { await DisplayAlert("Comment HTTP Erro", requestComments.errorMessage, "OK"); return; }
        if (commentsData!.comments! == null) { return; }
        comment_loadingbar.IsVisible = true;

        foreach (var comments in commentsData!.comments!)
        {
            comment_loadingbar.IsRunning = true;
            string c_noType = "";
            string userIP = "";
            bool LabelVisible = false;
            bool ImageVisible = false;
            await Task.Delay(100);
            if (comments.no != "0" && comments.name != "댓글돌이")
            {
                var memoObj = await ImageConversion(comments.memo);

                if (comments.c_no != "0") { c_noType = "↑ "; }
                if (memoObj != null) { ImageVisible = true; }
                if (memoObj == null) { LabelVisible = true; }
                if (comments.ip != "") { userIP = $"({comments.ip})"; }

                comment_Group.Add(new comment_group()
                {
                    userName = $"{c_noType}{comments.name}{userIP}",
                    text = comments.memo,
                    image = memoObj,
                    labelVisible = LabelVisible,
                    ImageVisible = ImageVisible,
                    time = comments.reg_date,
                });
            }
            comment_loadingbar.IsRunning = false;;
            commentListView.ItemsSource = comment_Group;
        }
        commentListView.ItemsSource = comment_Group;
        comment_loadingbar.IsVisible = false;
 
    }

    private async void ToolbarItem_Clicked_Files(object sender, EventArgs e)
    {
        if (mediaData == null) { await DisplayAlert("정보", "Null", "OK"); return; }
        await Navigation.PushAsync(new MediaFilePage(mediaData));
    }

    private void BtnPicker(object sender, EventArgs e)
    {
        comment_Group = new ObservableCollection<comment_group>();
        commentListView.ItemsSource = null;
        int pageNumber = int.Parse(commentPicker.SelectedItem.ToString());
        CommentRequest(temporaryURL, pageNumber);
    }
}