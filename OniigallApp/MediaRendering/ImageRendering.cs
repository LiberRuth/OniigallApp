using System;
using SkiaSharp;

namespace OniigallApp.MediaRendering
{
    internal class ImageRendering
    {
        private byte[] byteData;

        public async Task<StreamImageSource> LoadImage(string target_url)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36");
                client.DefaultRequestHeaders.Referrer = new Uri("https://gall.dcinside.com/");

                HttpResponseMessage response = await client.GetAsync(target_url);
                byte[] data = await response.Content.ReadAsByteArrayAsync();
                byteData = data;

                StreamImageSource imageSource = (StreamImageSource)ImageSource.FromStream(() => new MemoryStream(data));
                return imageSource;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }

        public byte[] LoadImageStringByte()
        {
            if (byteData != null)
            {
                return byteData;
            }
            else
            {
                return null;
            }
        }


        public int LoadImageWidth()
        {
            SKBitmap bitmap = SKBitmap.Decode(byteData);
            if (bitmap != null)
            {
                return bitmap.Width;
            }
            else
            {
                return 0;
            }
        }

        public int LoadImageHeight()
        {
            SKBitmap bitmap = SKBitmap.Decode(byteData);
            if (bitmap != null)
            {
                return bitmap.Height;
            }
            else
            {
                return 0;
            }
        }
    }
}
