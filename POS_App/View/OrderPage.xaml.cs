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
using WinRT.Interop;

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

       
    }
}
