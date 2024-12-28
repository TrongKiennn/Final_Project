using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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
        public VIPCustomer()
        {
            this.InitializeComponent();
        }


        private void OnSettingButtonClick(object sender, RoutedEventArgs e)
        {
            DefaultStatePanel.Visibility = Visibility.Collapsed;
            SettingStatePanel.Visibility = Visibility.Visible;
        }

        private void OnBackButtonClick(object sender, RoutedEventArgs e)
        {
            DefaultStatePanel.Visibility = Visibility.Visible;
            SettingStatePanel.Visibility = Visibility.Collapsed;
        }

    }
}
