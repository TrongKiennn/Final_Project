using DemoListBinding1610.Model;
using DemoListBinding1610.Models;
using DemoListBinding1610.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DemoListBinding1610.IDao;

namespace DemoListBinding1610;
public partial class MainViewModel: INotifyPropertyChanged
{
    IDao _dao;
    public ObservableCollection<Employee> Employees
    {
        get; set;
    }

    public string Info { 
        get { 
            return $"Displaying {Employees.Count}/{RowsPerPage} of total {TotalItems} item(s)";
        }
    }

    public ObservableCollection<PageInfo> PageInfos { get; set; }
    public PageInfo SelectedPageInfoItem {get; set;}
    public string Keyword { get; set;} = "";
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; } = 0;
    public int RowsPerPage { get; set; }

    private bool _sortById = false;
    public bool SortById { 
        get => _sortById; 
        set {
            _sortById = value;
            if (value == true) {
                _sortOptions.Add("Name", SortType.Ascending);
            } else {
                if (_sortOptions.ContainsKey("Name")) {
                    _sortOptions.Remove("Name");
                }
            }

            LoadData();
        } 
    }

    private Dictionary<string, SortType> _sortOptions = new ();

    // ImageItems for Order Page
    public ObservableCollection<ImageItem> ImageItems { get; set; }

    public ObservableCollection<SelectedItem> SelectedItems { get; set; } 
        = new ObservableCollection<SelectedItem>();

    public MainViewModel()
    {
        RowsPerPage = 10;
        CurrentPage = 1;
        _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;

        LoadData();
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
    }

    public event PropertyChangedEventHandler PropertyChanged;

    public bool Remove(Employee info) {
        bool success = _dao.DeleteEmployee(info.ID); // DB

        if (success) { 
            Employees.Remove(info); // UI
        }
        return success;
    }

    public void GoToNextPage() {
        if (CurrentPage < TotalPages) {
            CurrentPage++;
            LoadData();
        }
    }

    public void LoadData() {
        
        var (items, count) = _dao.GetEmployees(
            CurrentPage, RowsPerPage, Keyword,
            _sortOptions
        );
        Employees = new ObservableCollection<Employee>(
            items
        );

        if (count != TotalItems) { // Recreate PageInfos list
            TotalItems = count;
            TotalPages = (TotalItems / RowsPerPage) + 
                (((TotalItems % RowsPerPage) == 0) ? 0 : 1);

            PageInfos = new (); 
            for(int i = 1; i <= TotalPages; i++) {
                PageInfos.Add(new PageInfo { 
                    Page = i,
                    Total = TotalPages
                });
            }
        }

        SelectedPageInfoItem = PageInfos[CurrentPage-1];        
    }

    public void GoToPage(int page) {
        CurrentPage = page;
        LoadData();
    }

    public void Search() {
        CurrentPage = 1;
        LoadData();
    }

    //Order page

    public void LoadImageItems()
    {
        // Sample data for 7 images
        ImageItems = new ObservableCollection<ImageItem>
    {
        new ImageItem { ImageSource = "ms-appx:///Assets/calendar.png", BackgroundColor = "LightBlue" },
        new ImageItem { ImageSource = "ms-appx:///Assets/lab-beaker.png", BackgroundColor = "LightGreen" },
        new ImageItem { ImageSource = "ms-appx:///Assets/employee.png", BackgroundColor = "LightCoral" },
        new ImageItem { ImageSource = "ms-appx:///Assets/order.png", BackgroundColor = "LightGray" },
        new ImageItem { ImageSource = "ms-appx:///Assets/table.png", BackgroundColor = "LightPink" },
        new ImageItem { ImageSource = "ms-appx:///Assets/market-analysis.png", BackgroundColor = "LightYellow" },
        new ImageItem { ImageSource = "ms-appx:///Assets/crown.png", BackgroundColor = "LightSalmon" }
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
