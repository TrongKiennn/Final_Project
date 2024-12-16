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
    public sealed partial class TableManagement : Page
    {

        // Mock data for preview
        public List<Table> Tables { get; set; }
        public TableManagement()
        {
            this.InitializeComponent();

            // Initialize mock data
            Tables = new List<Table>
            {
                new Table { TableNumber = "1", OrderCount = 3 },
                new Table { TableNumber = "2", OrderCount = 0 },
                new Table { TableNumber = "3", OrderCount = 2 },
                new Table { TableNumber = "4", OrderCount = 1 },
                new Table { TableNumber = "5", OrderCount = 0 },
                new Table { TableNumber = "6", OrderCount = 6 }
            };

            // Set DataContext for binding
            this.DataContext = this;
        }

        //private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        //{
        //    try
        //    {
        //        var clickedItem = e.ClickedItem as Table;
        //        if (clickedItem != null && clickedItem.TableNumber == "0") // "Add New" item
        //        {
        //            Tables.Insert(Tables.Count - 1, new Table
        //            {
        //                TableNumber = (Tables.Count).ToString(), // Assign next table number
        //                OrderCount = 0
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception (you can use any logging mechanism you prefer)
        //        System.Diagnostics.Debug.WriteLine($"Exception in GridView_ItemClick: {ex.Message}");
        //    }
        //}

        public class Table
        {
            public string TableNumber { get; set; }
            public int OrderCount { get; set; }
        }
    }
}
