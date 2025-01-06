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
using POS_App.Model;


namespace POS_App.View
{
    public sealed partial class UpdateProduct : Page
    {

        public UpdateMenuViewModel UpdateMenuViewModel { get; set; }
        public  UpdateProduct()
        {
            this.InitializeComponent();
            UpdateMenuViewModel= new UpdateMenuViewModel();
            DataContext = UpdateMenuViewModel;
           
        }



        private async void OnDrinkItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Drinks clickedDrink)
            {

                var viewModel = DataContext as UpdateMenuViewModel;
                if (viewModel != null)
                {
                    viewModel.DrinkClickCommand.Execute(clickedDrink);
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




        private void GoToAddDrink(object sender, RoutedEventArgs e)
        {
            DrinkInformation.Visibility = Visibility.Collapsed;
            AddDrinkInformation.Visibility = Visibility.Visible;
        }

        private void ReturnToDrinkInformation(object sender, RoutedEventArgs e)
        {
            DrinkInformation.Visibility = Visibility.Visible;
            AddDrinkInformation.Visibility = Visibility.Collapsed;
            createDrinkSuccessful.Visibility = Visibility.Collapsed;
        }

        private void ReturnToAddDrink(object sender, RoutedEventArgs e)
        {
            createDrinkSuccessful.Visibility = Visibility.Collapsed;
            AddDrinkInformation.Visibility = Visibility.Visible;

        }

        private void GoToSeccessDrink(object sender, RoutedEventArgs e)
        {

            UpdateMenuViewModel.SaveDrinkCommand.Execute(null);
            if (UpdateMenuViewModel.IsAddSuccess == true)
            {
                AddDrinkInformation.Visibility = Visibility.Collapsed;
                createDrinkSuccessful.Visibility = Visibility.Visible;
            }
        }

    }
}
