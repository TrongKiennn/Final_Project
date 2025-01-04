using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using System;
using System.IO;
using Supabase.Storage;
using System.Collections.Generic;
using System.Diagnostics;


namespace POS_App.View
{
    public sealed partial class UpdateProduct : Page
    {
        private string _selectedImagePath;
        private readonly Supabase.Client _storageClient;
        public  UpdateProduct()
        {
            this.InitializeComponent();
            // _storageClient = new Client(
            //    "https://cugbtwyqnjtevpcxcjmu.supabase.co",
            //     new Dictionary<string, string>
            //     {
            //         { "apikey","eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImN1Z2J0d3lxbmp0ZXZwY3hjam11Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzU5MjcyNjYsImV4cCI6MjA1MTUwMzI2Nn0.ZdhT9tNK423uqM8EHoWydpLsDAS4eaZeTNShb9S9C6s"},
            //     }
            //);

            var url = "https://cugbtwyqnjtevpcxcjmu.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImN1Z2J0d3lxbmp0ZXZwY3hjam11Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTczNTkyNzI2NiwiZXhwIjoyMDUxNTAzMjY2fQ.ma5fYXzPDhlz5_kv3OOmzTae3imO8Xf35rCtMGWnrkw";

            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

             _storageClient = new Supabase.Client(url, key, options);
            _storageClient.InitializeAsync();
        }

        private async void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };

        
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");


            var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
            InitializeWithWindow.Initialize(picker, hwnd);

            StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                _selectedImagePath = file.Path;

                using var stream = await file.OpenStreamForReadAsync();
                var bitmap = new BitmapImage();
                bitmap.SetSource(stream.AsRandomAccessStream());
                PreviewImage.Source = bitmap;
            }
        }

        private async void UploadImage_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedImagePath))
            {
                var dialog = new ContentDialog
                {
                    Title = "Thông báo",
                    Content = "Vui lòng chọn ảnh trước khi tải lên.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            var fileName = Path.GetFileName(_selectedImagePath);
            //var imagePath = Path.Combine("Drink_Image", fileName);

            var fileStream = new FileStream(_selectedImagePath, FileMode.Open, FileAccess.Read);


            byte[] fileBytes = new byte[fileStream.Length];
            await fileStream.ReadAsync(fileBytes, 0, fileBytes.Length);
            fileStream.Close();

            try
            {
                var result = await _storageClient.Storage.From("image").Upload(fileBytes, $"Drink_Image/{fileName}");
                Debug.WriteLine(result);

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

    }
}
