using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using POS_App.Model;
using POS_App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_App.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VIPCustomer : Page
    {
        public CustomerViewModel ViewModel { get; set; }
        public VIPCustomer()
        {
            this.InitializeComponent();
            ViewModel = new CustomerViewModel();
            this.DataContext = ViewModel;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Model.Customer clickedCustomer)
            {

                var viewModel = DataContext as CustomerViewModel;
                if (viewModel != null)
                {
                    viewModel.CustomerClickCommand.Execute(clickedCustomer);
                }
            }
        }


        private void GoToVIPSetting(object sender, RoutedEventArgs e)
        {
            CustomerInformation.Visibility = Visibility.Collapsed;
            SettingVIPRank.Visibility = Visibility.Visible;
        }

        private void GoToAddCustomer(object sender, RoutedEventArgs e)
        {
            CustomerInformation.Visibility = Visibility.Collapsed;
            AddCustomerInformation.Visibility = Visibility.Visible;
        }

        private void ReturnToCustomerInformation(object sender, RoutedEventArgs e)
        {
            CustomerInformation.Visibility = Visibility.Visible;
            AddCustomerInformation.Visibility = Visibility.Collapsed;
            createCustomerSuccessful.Visibility = Visibility.Collapsed;
            SettingVIPRank.Visibility = Visibility.Collapsed;
        }

        private void ReturnToAddCustomer(object sender, RoutedEventArgs e)
        {
            createCustomerSuccessful.Visibility = Visibility.Collapsed;
            AddCustomerInformation.Visibility = Visibility.Visible;

        }

        private void GoToSeccessCustomer(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveCustomerCommand.Execute(null);
            AddCustomerInformation.Visibility = Visibility.Collapsed;
            createCustomerSuccessful.Visibility = Visibility.Visible;
        }
    }
}
