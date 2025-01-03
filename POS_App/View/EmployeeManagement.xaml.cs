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
using System.Runtime.CompilerServices;
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
    public sealed partial class EmployeeManagement : Page
    {
        public EmployeeManagementViewModel ViewModel { get; set; }
        public EmployeeManagement()
        {
            this.InitializeComponent();
            ViewModel = new EmployeeManagementViewModel();
            this.DataContext = ViewModel;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is employeeInfo clickedEmployee)
            {

                var viewModel = DataContext as EmployeeManagementViewModel;
                if (viewModel != null)
                {
                    viewModel.EmployeeClickCommand.Execute(clickedEmployee);
                }
            }
        }

        private void OnCreateEmployeeButtonClick(object sender, RoutedEventArgs e)
        {
            DefaultStatePanel.Visibility = Visibility.Collapsed;
            createAccount.Visibility = Visibility.Visible;
        }

        private void OnNextButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.TemporarySaveEmployeeAccount.Execute(null);

            if (ViewModel.IsCheckAccount)
            {
                DefaultStatePanel.Visibility = Visibility.Collapsed;
                createAccount.Visibility = Visibility.Collapsed;
                SetInformationToEmployee.Visibility = Visibility.Visible;
            }

        }

        private void OnBackToDefaultButtonClick(object sender, RoutedEventArgs e)
        {
            DefaultStatePanel.Visibility = Visibility.Visible;
            createAccount.Visibility = Visibility.Collapsed;
            SetInformationToEmployee.Visibility = Visibility.Collapsed;
        }

        private void OnBackToCreateAccountButtonClick(object sender, RoutedEventArgs e)
        {
            createAccount.Visibility = Visibility.Visible;
            SetInformationToEmployee.Visibility = Visibility.Collapsed;
        }

        private void OnSaveEmployeeInfoButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveEmployeeInfoAndAccount.Execute(null);

            if (ViewModel.IsCheckEmployeeInfo)
            {
                SetInformationToEmployee.Visibility = Visibility.Collapsed;
                createAccountSuccessful.Visibility = Visibility.Visible;
            }

        }

        private void OnBackToSetInformationButtonClick(object sender, RoutedEventArgs e)
        {
            SetInformationToEmployee.Visibility = Visibility.Visible;
            createAccountSuccessful.Visibility = Visibility.Collapsed;
        }
        private void OnReturnDefaultButtonClick(object sender, RoutedEventArgs e)
        {
            DefaultStatePanel.Visibility = Visibility.Visible;
            createAccountSuccessful.Visibility = Visibility.Collapsed;
        }

        private async void OnContinueToDeleteClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                ContentDialogResult result = await ConfirmDialog.ShowAsync();
            }
            catch (Exception ex)
            {
                await new ContentDialog
                {
                    Title = "Error",
                    Content = $"An error occurred: {ex.Message}",
                    CloseButtonText = "Ok",
                    XamlRoot = this.XamlRoot
                }.ShowAsync();
            }
        }
    }
}
