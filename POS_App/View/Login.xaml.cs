using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using POS_App.View;
using POS_App.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using static POS_App.ViewModel.LoginViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace POS_App
{
    public sealed partial class Login : Page
    {
        public LoginViewModel uVm { get; set; }

        public Login()
        {
            this.InitializeComponent();
            uVm = new LoginViewModel();
            uVm.OnLoginSuccessful += NavigateToDashboard;
            DataContext = uVm; 
        }

        private void NavigateToDashboard(Model.User user)
        {
            DashBoardPage dashboardPage = new DashBoardPage();
            Frame.Navigate(typeof(MainPage), user);
        }
        private void registerButtonClick(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            Frame.Navigate(typeof(Register));
        }
    }
}