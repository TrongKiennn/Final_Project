using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using System;

namespace POS_App.View
{
    public sealed partial class UpdateProduct : Page
    {
        public UpdateProduct()
        {
            this.InitializeComponent();
        }

        private async void AddPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create the FileOpenPicker
                var picker = new FileOpenPicker
                {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.PicturesLibrary
                };

                // Add allowed file types
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                // Initialize the picker with the correct window handle for WinUI apps
                var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
                InitializeWithWindow.Initialize(picker, hwnd);

                // Pick a file
                StorageFile file = await picker.PickSingleFileAsync();

                if (file != null)
                {
                    // If file is selected, display the file name
                    this.textBlock.Text = "Picked photo: " + file.Name;

                    // Optionally, set the image source
                    var bitmapImage = new BitmapImage();
                    using (var stream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        await bitmapImage.SetSourceAsync(stream);
                    }

                    SelectedImage.Source = bitmapImage;
                    SelectedImage.Visibility = Visibility.Visible;
                }
                else
                {
                    // If no file was selected, show cancellation message
                    this.textBlock.Text = "Operation cancelled.";
                }
            }
            catch (Exception ex)
            {
                // Handle any error that may have occurred during the process
                this.textBlock.Text = "Error: " + ex.Message;
            }
        }

        private void UpdateInfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle update information button click
        }
    }
}
