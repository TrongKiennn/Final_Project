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
using static POS_App.Service.DataAccess.IDao_Drinks;
using static POS_App.Service.DataAccess.IDao_Order;
using static POS_App.Service.DataAccess.IDao_Order_Item;
using MySql.Data.MySqlClient;
using System.Data;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Windows.Storage;
using System.IO;
using Windows.System;


namespace POS_App;

public partial class TableManagerViewModel : INotifyPropertyChanged
{
    IDao_Drinks _dao;
    IDao_Order _Order_Dao;
    IDao_Order_Item _Order_Item_Dao;
    IDao_Tables _Table_Dao;
    IDao_Events _Dao_Events;
    public ICommand SelectTableCommand { get; set; }
    public ICommand ClearTableCommand { get; set; }
    public ObservableCollection<Table> Tables { get; set; }
    public ObservableCollection<orderItem> OrderItems { get; set; }
    public ObservableCollection<Grid> ordersItemsGrid { get; set; }

    public ObservableCollection<Event> upcomingEvents { get; set; }

    private OrderDetail _orderDetails = new OrderDetail();
    public OrderDetail OrderDetails
    {
        get => _orderDetails;
        set => SetProperty(ref _orderDetails, value);
    }

    private Table _selectedTable = new Table();
    public Table SelectedTable
    {
        get => _selectedTable;
        set => SetProperty(ref _selectedTable, value);
    }

    private Order _order;
    public Order order
    {
        get => _order;
        set
        {
            if (_order != value)
            {

                _order = value;
                OnPropertyChanged(nameof(order));
            }
        }
    }
    public TableManagerViewModel()
    {
       
        OrderDetails = new OrderDetail();
        SelectTableCommand = new RelayCommand(LoadordersItems);
        ClearTableCommand = new RelayCommand(ClearTable);

        ordersItemsGrid = new ObservableCollection<Grid>();

        _dao = ServiceFactory.GetChildOf(typeof(IDao_Drinks)) as IDao_Drinks;
        _Order_Dao = ServiceFactory.GetChildOf(typeof(IDao_Order)) as IDao_Order;
        _Order_Item_Dao = ServiceFactory.GetChildOf(typeof(IDao_Order_Item)) as IDao_Order_Item;
        _Table_Dao = ServiceFactory.GetChildOf(typeof(IDao_Tables)) as IDao_Tables;
        _Dao_Events = ServiceFactory.GetChildOf(typeof(IDao_Events)) as IDao_Events;
        StartTableAllocationChecker();
    }

   


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


    public void LoadData()
    {
        var (items, count) = _Table_Dao.GetTable();
        Tables = new ObservableCollection<Table>(items);
    }


    private async void ClearTable(object parameter)
    {
        SelectedTable.order_id = 0;
        SelectedTable.status = "available";
        await _Table_Dao.UpdateTableStatusAsync(SelectedTable);
        LoadData();
        
    }
    private void LoadordersItems(object parameter)
    {
       
        var orderItems = _Order_Item_Dao.GetOrderItemByOrderId(SelectedTable.order_id);
        OrderItems = new ObservableCollection<orderItem>(orderItems);
        order = new Order();
        ordersItemsGrid.Clear();
        try
        {
            foreach (orderItem orderItem in OrderItems)
            {
                order.Subtotal +=orderItem.TotalPerProduct;
                Grid orderGrid = GenerateOrderGrids(orderItem);
                ordersItemsGrid.Add(orderGrid);
            }
            OnPropertyChanged(nameof(ordersItemsGrid));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error retrieving ordersItems: " + ex.Message);
        }
    }


    private Grid GenerateOrderGrids(orderItem orderItem)
    {
        Grid orderGrid = new Grid
        {
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(10, 5, 10, 5),
            Width = double.NaN

        };

        orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

        // Ảnh sản phẩm
        var imageBorder = new Border
        {
            Width = 62,
            Height = 62,
            CornerRadius = new CornerRadius(8),
            Background = new SolidColorBrush(Colors.LightGray),
            Margin = new Thickness(15, 0, 5, 0)
        };
        var image = new Microsoft.UI.Xaml.Controls.Image
        {
            Width = 60,
            Height = 60,
            Source = new BitmapImage(new Uri(orderItem.Drinks.imageUrl)),
            Stretch = Stretch.UniformToFill
        };
        imageBorder.Child = image;

        Debug.WriteLine($"ImgUrl: {orderItem.Drinks.imageUrl}");

        Grid.SetColumn(imageBorder, 1);

        // Thông tin sản phẩm
        var infoPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            HorizontalAlignment = HorizontalAlignment.Left,
            Width = 150,
            Margin= new Thickness(10,0,0,0)
        };

        infoPanel.Children.Add(new TextBlock { Text = orderItem.Drinks.name, FontWeight = FontWeights.Bold, FontSize = 14, MaxLines = 2, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5), Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)) });
        infoPanel.Children.Add(new TextBlock { Text = $"Giá tiền: ${orderItem.Drinks.price.ToString()}", Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)), FontSize = 12 });
        infoPanel.Children.Add(new TextBlock { Text = $"Số lượng: {orderItem.OrderDetail.Quantity.ToString()}", Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)), FontSize = 12 });


        orderGrid.Children.Add(imageBorder);
        orderGrid.Children.Add(infoPanel);
       
        return orderGrid;
    }

    public void StartTableAllocationChecker()
    {
        LoadData();
        DispatcherTimer timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(1)
        };
        timer.Tick += async (s, e) => await CheckAndAllocateTablesAsync();
        timer.Start();
    }
    public async Task CheckAndAllocateTablesAsync()
    {
        LoadData();
        try
        {
            DateTime now = DateTime.Now;
            DateTime oneHourAfter = now.AddHours(1);

            var item= await _Dao_Events.GetEventsWithinTimeFrameAsync(now, oneHourAfter);
            upcomingEvents = new ObservableCollection<Event>(item);

            foreach (var ev in upcomingEvents)
            {
                var availableTables = Tables.Where(t => t.status == "available").ToList();

                if (availableTables.Count >= ev.TableNumber)
                {
                    for (int i = 0; i < ev.TableNumber; i++)
                    {
                        availableTables[i].status = "occupied";
                        await _Table_Dao.UpdateTableStatusAsync(availableTables[i]);
                    }
                    await _Dao_Events.UpdateEventStatus(ev.Id, "near_due");
                    LoadData();
                }
                else
                {
                    // Không đủ bàn, thông báo
                    //MessageBox.Show($"Không đủ bàn cho sự kiện: {ev.EventName}. Vui lòng sắp xếp thêm bàn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Lỗi khi kiểm tra và cập nhật bàn: {ex.Message}");
        }
    }
    

}