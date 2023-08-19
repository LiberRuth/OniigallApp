using OniigallApp.UIFrame;
using OniigallApp.MediaRendering;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Storage;
using System.Threading;
using System.Text;

namespace OniigallApp.Views;

public partial class MediaFilePage : ContentPage
{
    ImageRendering imageRequest = new ImageRendering();
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();    
    ObservableCollection<file_group> file_Group = new ObservableCollection<file_group>();

    public MediaFilePage(List<IDictionary<string, string>> fileData)
	{
		InitializeComponent();
        Type_Picker_add(fileData);
    }

    private void Type_Picker_add(List<IDictionary<string, string>> filesData)
    {
        foreach (var itemData in filesData)
        {
            foreach (var item in itemData)
            {
                file_Group.Add(new file_group() { fileName = item.Key, fileUrl = item.Value });
            }
        }
        filePicker.ItemsSource = file_Group;
    }

    private async void OnPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        webImagePreview.IsVisible = false; loadingbar.IsVisible = true;
        webImagePreview.Source = await imageRequest.LoadImage(filePicker.SelectedItem.ToString());
        webImagePreview.IsVisible = true; loadingbar.IsVisible = false;
    }

    private async void Button_Download(object sender, EventArgs e)
    {
        var byteimageStr = imageRequest.LoadImageStringByte();
        if (byteimageStr == null) { await DisplayAlert("Á¤º¸", "Null", "OK"); return; }
        btnDownloadloadingbar.IsVisible = true;
        BtnDownload.IsVisible = false;
        using var stream = new MemoryStream(byteimageStr);
        var selectedFile = (file_group)filePicker.ItemsSource[filePicker.SelectedIndex];
        string selectedFileName = selectedFile.fileName;
        var fileSaverResult = await FileSaver.Default.SaveAsync(selectedFileName, stream, cancellationTokenSource.Token);
        btnDownloadloadingbar.IsVisible = false;
        BtnDownload.IsVisible = true;
    }
}