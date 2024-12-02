using POS_App.Model;
using POS_App.Models;
using POS_App.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;

namespace POS_App;
public partial class OrderPageViewModel : INotifyPropertyChanged
{
    IDao _dao;
    public ObservableCollection<Employee> Employees
    {
        get; set;
    }

    public string Info
    {
        get
        {
            return $"Displaying {Employees.Count}/{RowsPerPage} of total {TotalItems} item(s)";
        }
    }

    public ObservableCollection<PageInfo> PageInfos { get; set; }
    public PageInfo SelectedPageInfoItem { get; set; }
    public string Keyword { get; set; } = "";
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; } = 0;
    public int RowsPerPage { get; set; }

    private bool _sortById = false;

    // ImageItems for Order Page
    public ObservableCollection<ImageItem> ImageItems { get; set; }

    public ObservableCollection<SelectedItem> SelectedItems { get; set; }
        = new ObservableCollection<SelectedItem>();

    public OrderPageViewModel()
    {
        RowsPerPage = 10;
        CurrentPage = 1;
        //_dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;

        LoadImageItems();

        // Adding some default selected items (can be replaced with actual data)
        AddSampleSelectedItems();
    }

    public void AddSampleSelectedItems()
    {
        // Add some sample items that might be selected
        SelectedItems.Add(new SelectedItem { ItemName = "Coffee - $5" });
        SelectedItems.Add(new SelectedItem { ItemName = "Tea - $3" });
        SelectedItems.Add(new SelectedItem { ItemName = "Croissant - $4" });


        // Check if items are added
        Debug.WriteLine($"Number of items in SelectedItems: {SelectedItems.Count}");
    }

    public event PropertyChangedEventHandler PropertyChanged;
    //Order page

    public void LoadImageItems()
    {
        // Sample data for 7 images
        ImageItems = new ObservableCollection<ImageItem>
        {
            new ImageItem { ImageSource = "../Assets/calendar.png", BackgroundColor = "LightBlue" },
            new ImageItem { ImageSource = "../Assets/lab-beaker.png", BackgroundColor = "LightGreen" },
            new ImageItem { ImageSource = "../Assets/employee.png", BackgroundColor = "LightCoral" },
            new ImageItem { ImageSource = "../Assets/order.png", BackgroundColor = "LightGray" },
            new ImageItem { ImageSource = "../Assets/table.png", BackgroundColor = "LightPink" },
            new ImageItem { ImageSource = "../Assets/market-analysis.png", BackgroundColor = "LightYellow" },
            new ImageItem { ImageSource = "../Assets/crown.png", BackgroundColor = "LightSalmon" }
        };

    }

    public class Product
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
        public string Price { get; set; }
        public string Quantity { get; set; }
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}