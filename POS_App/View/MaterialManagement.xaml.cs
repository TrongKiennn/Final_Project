using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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

        // Binding context for selected material
        public Material SelectedMaterial { get; set; }

        // Event handler for ItemClick
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedMaterial = e.ClickedItem as Material; // Get the clicked material
            MaterialPopup.IsOpen = true; // Open the popup
        }

        // Event handler for "OK" button in the popup
        private void OnPopupOkClick(object sender, RoutedEventArgs e)
        {
            // Handle the logic after clicking "OK"
            string enteredQuantity = QuantityTextBox.Text;
            // You can process the entered quantity or perform any other necessary action
            MaterialPopup.IsOpen = false; // Close the popup after handling the input
        }
    }
}
