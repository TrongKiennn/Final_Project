using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.System;
using POS_App.ViewModel;
using POS_App.Model;
using POS_App.View;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_App
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoardPage : Page
    {
        private Model.User _user;
       
        public DashBoardPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Model.User user)
            {
                _user = user;
                UserNameTextBlock.Text = $"Welcome, {_user.FirstName}";
            }
        }

        private void eventSchedulingButtonClick(object sender, RoutedEventArgs e)
        {
            EventScheduling eventScheduling = new EventScheduling();
            Frame.Navigate(typeof(EventScheduling));
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Login));
        }

    }
}
