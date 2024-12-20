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
using System.Diagnostics;
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
    public sealed partial class TableManagement : Page
    {

        // Mock data for preview
        public TableManagerViewModel TableManagerViewModel { get; set; }
        public TableManagement()
        {
            this.InitializeComponent();
            TableManagerViewModel = new TableManagerViewModel();
            DataContext = TableManagerViewModel;

        }

        private void OnTableItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Table clickedTable)
            {
                var viewModel = DataContext as TableManagerViewModel;
                if (viewModel != null)
                {
                    viewModel.SelectedTable = clickedTable;
                }

                viewModel.SelectTableCommand.Execute(clickedTable);
            }
        }

    }
}
