using DemoListBinding1610.Model;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace DemoListBinding1610
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {
        public MainViewModel ViewModel { get; set; }
        public OrderPage()
        {
            this.InitializeComponent();
            ViewModel = new MainViewModel();
            // DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel = e.Parameter as MainViewModel;
            DataContext = ViewModel;
        }

        //Event handler for Continue to Payment button
        private async void OnContinueToPaymentClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create a ListView to display the selected items
                ListView billListView = new ListView
                {
                    ItemsSource = ViewModel.SelectedItems,  // Bind to SelectedItems from ViewModel
                    SelectionMode = ListViewSelectionMode.None
                };

                // Create and show the ContentDialog with the list of selected items
                ContentDialog billDialog = new ContentDialog
                {
                    Title = "Bill Details",
                    Content = new StackPanel
                    {
                        Children =
                        {
                            new TextBlock
                            {
                                Text = "Your Selected Items:",
                                FontSize = 18,
                                Margin = new Thickness(0, 0, 0, 10)
                            },
                            billListView
                        }
                    },
                    PrimaryButtonText = "Choose Payment Method",
                    SecondaryButtonText = "Cancel",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = this.XamlRoot
                };

                // Show the ContentDialog and handle the result
                ContentDialogResult result = await billDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    // Show another dialog to choose payment method
                    await ShowPaymentMethodDialog();
                }
                else
                {
                    // Payment was canceled
                    await new ContentDialog
                    {
                        Title = "Payment Canceled",
                        Content = "Your payment was canceled.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                // Handle any errors that occur
                await new ContentDialog
                {
                    Title = "Error",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }

        private async Task ShowPaymentMethodDialog()
        {
            // Create and show the Payment Method dialog
            ContentDialog paymentMethodDialog = new ContentDialog
            {
                Title = "Choose Payment Method",
                Content = new StackPanel
                {
                    Children =
            {
                new TextBlock
                {
                    Text = "Please select your payment method:",
                    FontSize = 18,
                    Margin = new Thickness(0, 0, 0, 10)
                },
                new RadioButton { Content = "Cash", GroupName = "PaymentMethod", IsChecked = true },
                new RadioButton { Content = "Bank QR Code", GroupName = "PaymentMethod" }
            }
                },
                PrimaryButtonText = "Next",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            ContentDialogResult methodResult = await paymentMethodDialog.ShowAsync();

            if (methodResult == ContentDialogResult.Primary)
            {
                // Determine the selected payment method
                var stackPanel = (StackPanel)paymentMethodDialog.Content;
                RadioButton cashOption = (RadioButton)stackPanel.Children[1];
                RadioButton qrOption = (RadioButton)stackPanel.Children[2];

                if (cashOption.IsChecked == true)
                {
                    await ShowCashPaymentDialog();
                }
                else if (qrOption.IsChecked == true)
                {
                    await ShowQRCodePaymentDialog();
                }
            }
        }

        private async Task ShowCashPaymentDialog()
        {
            TextBox amountTenderedBox = new TextBox
            {
                PlaceholderText = "Enter Amount Tendered",
                Margin = new Thickness(0, 10, 0, 10)
            };

            ContentDialog cashDialog = new ContentDialog
            {
                Title = "Cash Payment",
                Content = new StackPanel
                {
                    Children =
            {
                new TextBlock { Text = "Amount Tendered:", FontSize = 18 },
                amountTenderedBox
            }
                },
                PrimaryButtonText = "Confirm",
                SecondaryButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = this.XamlRoot
            };

            ContentDialogResult cashResult = await cashDialog.ShowAsync();

            if (cashResult == ContentDialogResult.Primary)
            {
                // Validate and calculate change
                if (double.TryParse(amountTenderedBox.Text, out double amountTendered))
                {
                    double totalBill = 100; // Replace with your method to get total bill amount
                    double change = amountTendered - totalBill;

                    await new ContentDialog
                    {
                        Title = "Payment Successful",
                        Content = change >= 0
                            ? $"Payment successful! Change: {change:C2}"
                            : "Insufficient amount tendered.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                }
                else
                {
                    await new ContentDialog
                    {
                        Title = "Invalid Input",
                        Content = "Please enter a valid number.",
                        CloseButtonText = "Ok",
                        XamlRoot = this.XamlRoot
                    }.ShowAsync();
                }
            }
        }

        private async Task ShowQRCodePaymentDialog()
        {
            Image qrCodeImage = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/qr-code-sample.png")), // Replace with your QR code image
                Stretch = Stretch.Uniform,
                Height = 200
            };

            ContentDialog qrDialog = new ContentDialog
            {
                Title = "Bank QR Code Payment",
                Content = new StackPanel
                {
                    Children =
            {
                new TextBlock { Text = "Scan the QR code to complete your payment.", FontSize = 18, Margin = new Thickness(0, 0, 0, 10) },
                qrCodeImage
            }
                },
                CloseButtonText = "Ok",
                DefaultButton = ContentDialogButton.Close,
                XamlRoot = this.XamlRoot
            };

            await qrDialog.ShowAsync();
        }




        private async void OnProductTapped(object sender, RoutedEventArgs e)
        {
            // Open the ContentDialog when the product is tapped
            var result = await OrderDetailDialog.ShowAsync();

            if (result == ContentDialogResult.Primary) // Save button was clicked
            {
                // Collect the selections and note
                string sugar = Sugar100.IsChecked == true ? "100%" : "50%";
                string ice = NoIce.IsChecked == true ? "No Ice" :
                             SeparateIce.IsChecked == true ? "Separate Ice" :
                             ShareIce.IsChecked == true ? "Share Ice" :
                             "Little Ice";
                string takeawayOrStay = TakeAway.IsChecked == true ? "Take Away" : "Stay Here";
                string note = NoteTextBox.Text;

            }
            else if (result == ContentDialogResult.Secondary) // Cancel button clicked
            {
                // Optionally handle cancellation
                //ToastHelper.ShowToast("Order was canceled.");
            }
        }

        // This will handle Save button click inside the ContentDialog
        private void OnSaveClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // You can handle saving the order here
        }

        // This will handle Cancel button click inside the ContentDialog
        private void OnCancelClicked(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Handle cancellation logic if needed
        }

        private void nextButton_Click(object sender, RoutedEventArgs e) { }
        private void previousButton_Click(object sender, RoutedEventArgs e) { }
        private void pagesComboBox_SelectionChanged(object sender, RoutedEventArgs e) { }
        private void backToDashboardButton_Click(object sender, RoutedEventArgs e) { }
        private void getAllCoffeeButton_Click(object sender, RoutedEventArgs e) { }
        private void getAllTeaButton_Click(object sender, RoutedEventArgs e) { }
        private void getAllJuiceButton_Click(object sender, RoutedEventArgs e) { }
        private void getAllSmoothieButton_Click(object sender, RoutedEventArgs e) { }
        private void getAllMilkTeaButton_Click(object sender, RoutedEventArgs e) { }
        private void getAllCroissantButton_Click(object sender, RoutedEventArgs e) { }
        private void searchButton_Click(object sender, RoutedEventArgs e) { ViewModel.Search(); }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}