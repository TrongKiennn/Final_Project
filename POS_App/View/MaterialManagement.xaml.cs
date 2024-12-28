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
    public sealed partial class MaterialManagement : Page
    {
        public MaterialManagementViewModel ViewModel { get; set; }

        public MaterialManagement()
        {
            this.InitializeComponent();
            ViewModel = new MaterialManagementViewModel();
            this.DataContext = ViewModel;
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Ingredient clickedIngredient)
            {
                
                var viewModel = DataContext as MaterialManagementViewModel;
                if (viewModel != null)
                {
                    viewModel.IngredientClickCommand.Execute(clickedIngredient);
                }
            }
        }
        private async void OnContinueToDeleteClicked(object sender, RoutedEventArgs e)
        {
            try
            {

                ContentDialogResult result = await ConfirmDialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {

                }
                else
                {
                    
                    //await new ContentDialog
                    //{
                    //    Title = "Payment Canceled",
                    //    Content = "Your payment was canceled.",
                    //    CloseButtonText = "Ok",
                    //    XamlRoot = this.XamlRoot
                    //}.ShowAsync();
                }
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
