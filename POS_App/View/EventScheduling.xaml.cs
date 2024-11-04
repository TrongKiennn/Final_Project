using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using POS_App.ViewModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventScheduling : Page
    {
        public EventSchedulingViewModel ViewModel { get; set; }

        public EventScheduling()
        {
            this.InitializeComponent();
            ViewModel = new EventSchedulingViewModel(); 
            this.DataContext = ViewModel;
        }

        private void StackPanel_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (DefaultView.Visibility == Visibility.Visible)
            {
                DefaultTitleView.Visibility = Visibility.Collapsed;
                EventTitleView.Visibility = Visibility.Visible;
                DefaultView.Visibility = Visibility.Collapsed;
                EventView.Visibility = Visibility.Visible;

                // Load data into EventView if necessary
                Fullname.Text = "Sample Name"; // replace with actual data
                PhoneNumber.Text = "123-456-7890"; // replace with actual data
                Email.Text = "example@example.com"; // replace with actual data
                Date.Text = "2024-12-01"; // replace with actual data
                Time.Text = "10:00 AM"; // replace with actual data
                NumberOfTables.Text = "5"; // replace with actual data
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            if (DefaultView.Visibility != Visibility.Visible)

            {
                DefaultTitleView.Visibility = Visibility.Visible;
                EventTitleView.Visibility = Visibility.Collapsed;
                DefaultView.Visibility = Visibility.Visible;
                EventView.Visibility = Visibility.Collapsed;
            }
        }
        private void CompleteButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
