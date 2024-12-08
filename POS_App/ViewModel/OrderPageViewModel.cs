using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using POS_App.Command;
using POS_App.Model;
using POS_App.Models;
using POS_App.Service;
using POS_App.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Services.Maps;
using static POS_App.Service.DataAccess.IDao;
using MySql.Data.MySqlClient;
using System.Data;

namespace POS_App;

public partial class OrderPageViewModel : INotifyPropertyChanged
{
    IDao _dao;
    public ICommand FilterCommand { get; }
    public ObservableCollection<Drinks> Drinks { get; set; }
    public ObservableCollection<PageInfo> PageInfos { get; set; }
    public ObservableCollection<Order> Orders { get; set; }

    public ObservableCollection<Grid> ordersGrid { get; set; }
    public PageInfo SelectedPageInfoItem { get; set; }


    private Drinks _selectedDrink=new Drinks();
    public Drinks SelectedDrink
    {
        get => _selectedDrink;
        set => SetProperty(ref _selectedDrink, value);
    }

    private OrderDetail _orderDetails = new OrderDetail();
    public OrderDetail OrderDetails
    {
        get => _orderDetails;
        set => SetProperty(ref _orderDetails, value);
    }
    public ICommand CancelCommand { get; }
    public ICommand SaveCommand { get; }
    public string Keyword { get; set; } = "";

    public string typeName { get; set; } = null;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; } = 0;
    public int RowsPerPage { get; set; }

    private bool _sortById = false;
    public bool SortById
    {
        get => _sortById;
        set
        {
            _sortById = value;
            if (value == true)
            {
                _sortOptions.Add("Name", SortType.Ascending);
            }
            else
            {
                if (_sortOptions.ContainsKey("Name"))
                {
                    _sortOptions.Remove("Name");
                }
            }

            LoadData();
        }
    }

    public OrderPageViewModel()
    {
        RowsPerPage = 10;
        CurrentPage = 1;
        FilterCommand = new RelayCommand(ExecuteFilter);
        SaveCommand = new RelayCommand(_ => SaveOrder());
        OrderDetails = new OrderDetail();
        CancelCommand = new RelayCommand(_=> ClearOrderDetails());
        ordersGrid = new ObservableCollection<Grid>();
        Orders = new ObservableCollection<Order>();
        _dao = ServiceFactory.GetChildOf(typeof(IDao)) as IDao;

        LoadData();
    }
    private void ExecuteFilter(object parameter)
    {

        if (parameter is string type_Name)
        {
            typeName = type_Name=="All"?null:type_Name;
            CurrentPage = 1;
            LoadData();

        }
    }

    private Dictionary<string, SortType> _sortOptions = new();
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }



    public void GoToNextPage()
    {
        if (CurrentPage < TotalPages)
        {
            CurrentPage++;
            LoadData();
        }
    }

    public void GoToPreviousPage()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            LoadData();
        }
    }

    public void LoadData()
    {

        var (items, count) = _dao.GetDrink(
            CurrentPage, RowsPerPage, Keyword,
            _sortOptions, typeName
        );
        Drinks = new ObservableCollection<Drinks>(
            items
        );

        if (count != TotalItems)
        {
            TotalItems = count;
            TotalPages = (TotalItems / RowsPerPage) +
                (((TotalItems % RowsPerPage) == 0) ? 0 : 1);

            PageInfos = new();
            for (int i = 1; i <= TotalPages; i++)
            {
                PageInfos.Add(new PageInfo
                {
                    Page = i,
                    Total = TotalPages
                });
            }
        }

        SelectedPageInfoItem = PageInfos[CurrentPage - 1];
    }

    public void GoToPage(int page)
    {
        CurrentPage = page;
        LoadData();
    }

    public void Search()
    {
        CurrentPage = 1;
        LoadData();
    }
    private void ClearOrderDetails()
    {
        SelectedDrink = null;
        OrderDetails = new OrderDetail();
    }

    private void SaveOrder()
    {
        if (SelectedDrink != null)
        {
            var newOrder = new Order
            {
                Drinks = SelectedDrink,
                OrderDetail = new OrderDetail
                {
                    Sugar100 = OrderDetails.Sugar100,
                    Sugar50 = OrderDetails.Sugar50,
                    NoIce = OrderDetails.NoIce,
                    SeparateIce = OrderDetails.SeparateIce,
                    ShareIce = OrderDetails.ShareIce,
                    LittleIce = OrderDetails.LittleIce,
                    TakeAway = OrderDetails.TakeAway,
                    StayHere = OrderDetails.StayHere,
                    Note = OrderDetails.Note
                }
            };

            Orders.Add(newOrder);
            ClearOrderDetails();
            LoadOrders(null);
        }
    }

    private void LoadOrders(object parameter)
    {
        ordersGrid.Clear();
        try
        {
            foreach (Order order in Orders)
            {
                Debug.WriteLine($"Drink: {order.Drinks.name} - {order.Drinks.price}");
                Grid orderGrid = GenerateOrderGrids(order);
                ordersGrid.Add(orderGrid);
            }
            OnPropertyChanged(nameof(ordersGrid));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error retrieving orders: " + ex.Message);
        }
    }

    private Grid GenerateOrderGrids(Order order)
    {
        Grid orderGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 0, 10, 0),
            Width = double.NaN
        };

        orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

        // Ảnh sản phẩm
        var imageBorder = new Border
        {
            Width = 64,
            Height = 64,
            CornerRadius = new CornerRadius(8),
            Background = new SolidColorBrush(Colors.LightGray),
            Margin = new Thickness(0, 0, 10, 0)
        };
        var image = new Image
        {
            Width=40,
            Height=40,
            Source = new BitmapImage(new Uri(order.Drinks.imageUrl)),
            Stretch = Stretch.UniformToFill
        };
        imageBorder.Child = image;

        Debug.WriteLine($"ImgUrl: {order.Drinks.imageUrl}");

        Grid.SetColumn(imageBorder, 1);

        // Thông tin sản phẩm
        var infoPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Left,
            MaxWidth = 150
        };

        infoPanel.Children.Add(new TextBlock { Text = order.Drinks.name, FontWeight = FontWeights.Bold, FontSize = 14, MaxLines = 2, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5) });
        infoPanel.Children.Add(new TextBlock { Text = order.Drinks.price.ToString(), Foreground = new SolidColorBrush(Colors.Orange), FontSize = 12 });

        Grid.SetColumn(infoPanel, 2);

        var deleteButton = new Button
        {
            Background = new SolidColorBrush(Colors.Orange),
            Foreground = new SolidColorBrush(Colors.White),
            Width = 32,
            Height = 32,
            CornerRadius = new CornerRadius(16),
            Margin = new Thickness(0, 0, 5, 0),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        var deleteText = new TextBlock
        {
            Text = "x",
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        deleteButton.Content = deleteText;
        deleteButton.Click += (s, e) => DeleteOrder(order); 

        Grid.SetColumn(deleteButton, 0);

      
        orderGrid.Children.Add(imageBorder);
        orderGrid.Children.Add(infoPanel);
        orderGrid.Children.Add(deleteButton);
        return orderGrid;
    }

    private void DeleteOrder(Order order)
    {
        Orders.Remove(order);
        LoadOrders(null);
    }

}