using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;

namespace POS_App.Service
{
    public class UploadImageHelper : INotifyPropertyChanged
    {



        public async Task<(string ImageUrl, string ErrorMessage)> GetImageUrlFromSupaBase()
        {
            string imgUrl = string.Empty;
            string errorHandling = string.Empty;
            string selectedImagePath = string.Empty;

            // Supabase client initialization
            var url = "https://cugbtwyqnjtevpcxcjmu.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImN1Z2J0d3lxbmp0ZXZwY3hjam11Iiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTczNTkyNzI2NiwiZXhwIjoyMDUxNTAzMjY2fQ.ma5fYXzPDhlz5_kv3OOmzTae3imO8Xf35rCtMGWnrkw";

            var options = new Supabase.SupabaseOptions
            {
                AutoConnectRealtime = true
            };

            var storageClient = new Supabase.Client(url, key, options);
            await storageClient.InitializeAsync();

            // File picker
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
                selectedImagePath = file.Path;
            }

            if (string.IsNullOrEmpty(selectedImagePath))
            {
                errorHandling = "No image selected";
                return (string.Empty, errorHandling);
            }

            var fileName = Path.GetFileName(selectedImagePath);
            var imagePath = $"Drink_Image/{fileName}";

            try
            {
              
                var files = await storageClient.Storage.From("image").List("Drink_Image");
                if (files.Any(f => f.Name == fileName))
                {
                    imgUrl = $"{url}/storage/v1/object/public/image/{imagePath}";
                    Debug.WriteLine($"File already exists. URL: {imgUrl}");
                    return (imgUrl, string.Empty);
                }

                byte[] fileBytes = await File.ReadAllBytesAsync(selectedImagePath);

                var result = await storageClient.Storage.From("image").Upload(fileBytes, imagePath);

                if (!string.IsNullOrEmpty(result))
                {
                    imgUrl = $"{url}/storage/v1/object/public/image/{imagePath}";
                    Debug.WriteLine($"Uploaded successfully. URL: {imgUrl}");
                }
                else
                {
                    errorHandling = "Failed to upload image";
                }
            }
            catch (Exception ex)
            {
                errorHandling = $"Upload failed: {ex.Message}";
                Debug.WriteLine(errorHandling);
            }

            return (imgUrl, errorHandling);
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
