using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Mysqlx.Crud;
using POS_App.Model;
using POS_App.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class OrderPage : Page
    {
        public OrderPageViewModel OrderPageViewModel { get; set; }
        public OrderPage()
        {
            this.InitializeComponent();
            OrderPageViewModel = new OrderPageViewModel();
            DataContext = OrderPageViewModel;
           
        }

        private void CashRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            
            CashAmountStackPanel.Visibility = Visibility.Visible;
        }

        private void CashRadioButton_Unchecked(object sender, RoutedEventArgs e)
        {

            CashAmountStackPanel.Visibility = Visibility.Collapsed;
        }

        private void StayHereButton_Checked(object sender, RoutedEventArgs e)
        {

            TableNumberStackPanel.Visibility = Visibility.Visible;
        }

        private void TakeAwayButton_Checked(object sender, RoutedEventArgs e)
        {

            TableNumberStackPanel.Visibility = Visibility.Collapsed;
        }

        private async void OnContinueToPaymentClicked(object sender, RoutedEventArgs e)
        {
            try
            {

                ContentDialogResult result = await PaymentDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                   
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

        private void NavigationView_SelectionChanged(object sender, NavigationViewSelectionChangedEventArgs e)
        {
            var selectedItem = e.SelectedItem as NavigationViewItem;

            if (selectedItem != null)
            {
                string content = selectedItem.Content.ToString();
                switch (content)
                {
                    case "Order":
                        Frame.Navigate(typeof(OrderPage));
                        break;
                    case "Material Management":
                        break;
                    case "Event Scheduling":
                        Frame.Navigate(typeof(EventScheduling));
                        break;
                    case "Statistics":
                        break;
                    case "Table Manager":
                        break;
                    case "Employee Management":
                        break;
                    case "VIP":
                        break;
                }
            }
        }


     
        //private async void OnProductTapped(object sender, RoutedEventArgs e)
        //{
        //    await OrderDetailDialog.ShowAsync();
        //}

        private void MainNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer.Tag != null)
            {
                string tag = args.InvokedItemContainer.Tag.ToString();

                // Navigate to corresponding page
                switch (tag)
                {
                }
            }
        }

        //handle the pane opening and closing events
        private void MainNavigationView_PaneOpening(object sender, object e)
        {
            // Ensure the state is set correctly when the pane is opening.
            VisualStateManager.GoToState(this, "PaneOpenState", true);
        }

        private void MainNavigationView_PaneClosing(object sender, NavigationViewPaneClosingEventArgs e)
        {
            // Ensure the state is set correctly when the pane is closing.
            VisualStateManager.GoToState(this, "PaneClosedState", true);
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

        private void nextButton_Click(object sender, RoutedEventArgs e) {
            OrderPageViewModel.GoToNextPage();
        }
        private void previousButton_Click(object sender, RoutedEventArgs e) {
            OrderPageViewModel.GoToPreviousPage();
        }

        bool init = false;
        private void pagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (init == false)
            {
                init = true;
                return;
            }
            if (pagesComboBox.SelectedIndex >= 0)
            {
                var item = pagesComboBox.SelectedItem as PageInfo;
                OrderPageViewModel.GoToPage(item.Page);
            }
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            OrderPageViewModel.Search();
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch)
            {
                OrderPageViewModel.Sort(toggleSwitch.IsOn);
            }
        }

        private void Image_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void OnDrinkItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Drinks clickedDrink)
            {

                var viewModel = DataContext as OrderPageViewModel;
                if (viewModel != null)
                {
                    viewModel.DrinkClickCommand.Execute(clickedDrink);
                    if (viewModel.IsAvailable == true)
                    {
                        await OrderDetailDialog.ShowAsync();
                    }
                    else
                    {
                        await UnAvailableOrderDetailDialog.ShowAsync();
                    }
                }
            }
        }

        private void SwitchToInterfaceB_Click(object sender, RoutedEventArgs e)
        {
            InterfaceA.Visibility = Visibility.Collapsed;
            InterfaceB.Visibility = Visibility.Visible;
        }

        private void SwitchToInterfaceA_Click(object sender, RoutedEventArgs e)
        {
            InterfaceB.Visibility = Visibility.Collapsed;
            InterfaceA.Visibility = Visibility.Visible;
        }
        private void OnAddIngredientClick(object sender, RoutedEventArgs e)
        {
            // Create a new StackPanel for the new ingredient
            var newIngredientPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };
            newIngredientPanel.Children.Add(new TextBox { Width = 810, PlaceholderText = "New Ingredient" });
            newIngredientPanel.Children.Add(new TextBox { Width = 100, PlaceholderText = "Quantity" });
            newIngredientPanel.Children.Add(new TextBox { Width = 100, PlaceholderText = "Unit" });

            // Add the new ingredient panel to the IngredientsPanel
            IngredientsPanel.Children.Add(newIngredientPanel);
        }
        private async void PickPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var picturesLibrary = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Pictures);
            if (picturesLibrary.RequestAddFolderAsync() != null)
            {
                var picker = new FileOpenPicker();
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.ViewMode = PickerViewMode.Thumbnail;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                StorageFile file = await picker.PickSingleFileAsync();

                if (file != null)
                {
                    textBlock.Text = "Picked photo: " + file.Name;
                }
                else
                {
                    textBlock.Text = "Operation cancelled.";
                }
            }
            else
            {
                textBlock.Text = "Access to pictures library denied.";
            }
        }
        //private async void PickPhotoButton_Click(object sender, RoutedEventArgs e)
        //{
        //    // Create a FileOpenPicker to allow the user to pick a file
        //    var picker = new FileOpenPicker();
        //    picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
        //    picker.ViewMode = PickerViewMode.Thumbnail;
        //    picker.FileTypeFilter.Add(".jpg");
        //    picker.FileTypeFilter.Add(".jpeg");
        //    picker.FileTypeFilter.Add(".png");

        //    // Make the picker available for desktop apps
        //    StorageFile file = await picker.PickSingleFileAsync();

        //    if (file != null)
        //    {
        //        // Application now has read/write access to the picked file
        //        textBlock.Text = "Picked photo: " + file.Name;
        //    }
        //    else
        //    {
        //        textBlock.Text = "Operation cancelled.";
        //    }
        //}
    }
}
