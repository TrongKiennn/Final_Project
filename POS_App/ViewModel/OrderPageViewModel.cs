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

public partial class OrderPageViewModel : INotifyPropertyChanged
{
    IDao_Drinks _dao;
    IDao_Order _Order_Dao;
    IDao_Order_Item _Order_Item_Dao;
    IDao_Tables _Table_Dao;
    IDao_Ingredients _Dao_Ingredients;
    IDao_Drink_Ingredient _Dao_Drink_Ingredient;
    public ICommand FilterCommand { get; }

    private readonly IDao_Drinks_Test _repository_Test;
    public ObservableCollection<Drinks> Drinks { get; set; }
    public ObservableCollection<Drinks> Drinks_Test { get; set; }
    public ObservableCollection<PageInfo> PageInfos { get; set; }
    public ObservableCollection<orderItem> ordersItems { get; set; }
    public ObservableCollection<Grid> ordersItemsGrid { get; set; }
    public ObservableCollection<Ingredient> Ingredients { get; set; }
    public ObservableCollection<Drink_Ingredient> Drink_Ingredients { get; set; }
    public PageInfo SelectedPageInfoItem { get; set; }
    public OrderService orderService { get; set; }

    private Drinks _selectedDrink=new Drinks();

    private bool _isAvailable=true;
    public bool IsAvailable
    {
        get => _isAvailable;
    }
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
    public ICommand CancelCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand ConfirmPaymentCommand { get; }
    public ICommand SearchCommand { get; }
    public ICommand SortByNameCommand { get; }
    public ICommand ContinueToPaymentCommand { get; }
    public ICommand DrinkClickCommand { get; set; }
    public string Keyword { get; set; } = "";

    public string typeName { get; set; } = null;
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; } = 0;
    public int RowsPerPage { get; set; }

    public bool _sortByName { get; set; } = false;

    public bool SortByName
    {
        get => _sortByName;
        set
        {
            if (_sortByName != value) 
            {
                _sortByName = value;

               
                if (_sortByName)
                {
                    _sortOptions["drink_name"] = SortType.Ascending;
                }
                else
                {
                    _sortOptions.Remove("drink_name");
                }

                LoadData(); 
            }
        }
    }

    public OrderPageViewModel(IDao_Drinks_Test repository)
    {
        RowsPerPage = 10;
        CurrentPage = 1;
        _repository_Test = repository;
        SearchCommand = new RelayCommand(_ => Search());
        SortByNameCommand = new RelayCommand(_ => Sort(true));
        LoadData();
    }
    public OrderPageViewModel()
    {
        RowsPerPage = 10;
        CurrentPage = 1;
        FilterCommand = new RelayCommand(ExecuteFilter);
        SaveCommand = new RelayCommand(_ => SaveOrderItem());
        ConfirmPaymentCommand = new RelayCommand(_ => SaveOrderToDb());
        CancelCommand = new RelayCommand(_=> ClearOrderDetails());
        ContinueToPaymentCommand = new RelayCommand(_ => AddTableIdToOrder());
        DrinkClickCommand = new RelayCommand<Drinks>(OnDrinkClick);

        ordersItemsGrid = new ObservableCollection<Grid>();
        ordersItems = new ObservableCollection<orderItem>(); 
        order = new Order();
        OrderDetails = new OrderDetail();

        _dao = ServiceFactory.GetChildOf(typeof(IDao_Drinks)) as IDao_Drinks;
        _Order_Dao = ServiceFactory.GetChildOf(typeof(IDao_Order)) as IDao_Order;
        _Order_Item_Dao = ServiceFactory.GetChildOf(typeof(IDao_Order_Item)) as IDao_Order_Item;
        _Table_Dao = ServiceFactory.GetChildOf(typeof(IDao_Tables)) as IDao_Tables;
        _Dao_Ingredients = ServiceFactory.GetChildOf(typeof(IDao_Ingredients)) as IDao_Ingredients;
        _Dao_Drink_Ingredient = ServiceFactory.GetChildOf(typeof(IDao_Drink_Ingredient)) as IDao_Drink_Ingredient;
        orderService = new OrderService(_Order_Dao, _Order_Item_Dao);
        LoadData();
    }

    private  void OnDrinkClick(Drinks selectedDrink)
    {
        Debug.WriteLine($"Drink clicked: {selectedDrink.status}");

        _isAvailable = true;
        var ingredeintItems = _Dao_Ingredients.GetIngredients();
        Ingredients = new ObservableCollection<Ingredient>(ingredeintItems);

        SelectedDrink = selectedDrink;
        var drinkIngreItem=_Dao_Drink_Ingredient.GetDrinkIngredientsByDrinkId(selectedDrink.id);
        Drink_Ingredients = new ObservableCollection<Drink_Ingredient>(drinkIngreItem);

        foreach (Drink_Ingredient drink_Ingredient in Drink_Ingredients)
        {
            var getIngredient= Ingredients.FirstOrDefault(i => i.ingredient_id == drink_Ingredient.ingredient_id);
            if(getIngredient.stock< drink_Ingredient.quantity)
            {
                _dao.UpdateDrinkStatus(selectedDrink.id, "unavailable");
                _isAvailable = false;
                LoadData();
                break;
            }
        }
    }
    private void AddTableIdToOrder()
    {
       
        if (order.LocationOptions == "Stay Here")
        {
            order.table_id = _Order_Dao.GetTableId();
        }
        else
        {
            order.table_id = 0;
        }
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

    public Dictionary<string, SortType> _sortOptions = new();
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
        var sortOptions = new Dictionary<string, SortType>
            {
                { "Name", SortType.Ascending }
            };

        if (_repository_Test != null)
        {
            var (item, _) = _repository_Test.GetDrink(CurrentPage, RowsPerPage, Keyword, sortOptions);
            Drinks = new ObservableCollection<Drinks>(item);
        }

        if (_dao != null)
        {


            var (items, count) = _dao.GetDrink(
                CurrentPage, RowsPerPage, Keyword,
                _sortOptions, typeName
            );

            if (items == null || !items.Any())
            {
                SelectedPageInfoItem.Page = 1;
                SelectedPageInfoItem.Total = 1;
                Drinks = new ObservableCollection<Drinks>();
            }
            else
            {
                Drinks = new ObservableCollection<Drinks>(items);
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
        }
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

    public void Sort(bool isSort)
    {
        SortByName = isSort; 
    }
    private void ClearOrderDetails()
    {
      
        SelectedDrink = null;
        OrderDetails = new OrderDetail();
    }

    private void SaveOrderItem()
    {
        if (SelectedDrink != null)
        {
            var newOrderItem = new orderItem
            {
                Drinks = SelectedDrink,
                OrderDetail = new OrderDetail
                {
                    SugarOptions = OrderDetails.SugarOptions,
                    IceOptions = OrderDetails.IceOptions,
                    Quantity = OrderDetails.Quantity,
                    Note = OrderDetails.Note
                },
                DrinkIngredients = Drink_Ingredients.ToList()
            };

            
            order.Subtotal += newOrderItem.TotalPerProduct;
            ordersItems.Add(newOrderItem);
            ClearOrderDetails();
            LoadordersItems(null);
        }
    }

    private void LoadordersItems(object parameter)
    {
        ordersItemsGrid.Clear();
        try
        {
            foreach (orderItem orderItem in ordersItems)
            {
                
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
            Margin = new Thickness(0, 5, 0, 5),
            Width = double.NaN
        };

        orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
        orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });

        // Ảnh sản phẩm
        var imageBorder = new Border
        {
            Width = 62,
            Height = 62,
            CornerRadius = new CornerRadius(8),
            Background = new SolidColorBrush(Colors.LightGray),
            Margin = new Thickness(0, 0, 5, 0)
        };
        var image = new Microsoft.UI.Xaml.Controls.Image
        {
            Width=60,
            Height=60,
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
            Width = 120
        };

        infoPanel.Children.Add(new TextBlock { Text = orderItem.Drinks.name, FontWeight = FontWeights.Bold, FontSize = 14, MaxLines = 2, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 5), Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)) });
        infoPanel.Children.Add(new TextBlock { Text = $"Giá tiền: ${orderItem.Drinks.price.ToString()}", Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)), FontSize = 12 });
        infoPanel.Children.Add(new TextBlock { Text = $"Số lượng: {orderItem.OrderDetail.Quantity.ToString()}", Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)), FontSize = 12 });


        Grid.SetColumn(infoPanel, 2);

        var deleteButton = new Button
        {
            Content = "x",
            Background = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)),
            Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)),
            FontSize = 14, // Tăng FontSize phù hợp
            Width = 32,
            Height = 32,
            CornerRadius = new CornerRadius(16),
            Margin = new Thickness(5, 0, 5, 0),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        deleteButton.Click += (s, e) => DeleteOrder(orderItem); 

        Grid.SetColumn(deleteButton, 0);

      
        orderGrid.Children.Add(imageBorder);
        orderGrid.Children.Add(infoPanel);
        orderGrid.Children.Add(deleteButton);
        return orderGrid;
    }

    private void DeleteOrder(orderItem orderItem)
    {
        order.Subtotal -= orderItem.TotalPerProduct;
        ordersItems.Remove(orderItem);
        LoadordersItems(null);
    }

    private void ClearOrder()
    {
       ordersItems.Clear();
        order = new Order();
    }

    public void CreateData()
    {
        
        var localSettings = ApplicationData.Current.LocalSettings;


        int userid;


        if (localSettings.Values.TryGetValue("UserId", out object userIdObj) && userIdObj is int userId)
        {
            order._user_id = userId;
            Debug.WriteLine($"User ID retrieved: {userId}");
        }
        else
        {
            Debug.WriteLine("User ID not found or invalid in OrderPageViewModel.");
        }


        order.id=orderService.CreateOrderWithItems(order, ordersItems);
        if(order.LocationOptions=="Stay Here")
        {
            _Table_Dao.UpdateTableStatus(order.table_id, "reserved",order.id);
        }

        foreach (orderItem orderItem in ordersItems)
        {
           
            foreach (Drink_Ingredient drink_Ingredient in orderItem.DrinkIngredients)
            {
                _Dao_Ingredients.UpdateIngredientStockById(drink_Ingredient.ingredient_id, -drink_Ingredient.quantity);
            }
            Ingredients.Clear();
        }
       
    }

    private void SaveOrderToDb()
    {
        CreateData();
        GeneratePDF();
        ClearOrder();
        ClearOrderDetails();
        LoadordersItems(null);
    }

    private void GeneratePDF()
    {
        string Name = $"{order.id}bill.pdf";
        string folderPath = @"D:\C#\22120151_22120167\AllBill";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string pdfPath = Path.Combine(folderPath, Name);


        using (var document = new PdfDocument())
        {
           
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

           
            var font = new XFont("Arial", 12);
            var boldFont = new XFont("Arial", 12, XFontStyle.Bold);

           
            gfx.DrawString("KH COFFEE", boldFont, XBrushes.Black, new XPoint(40, 40));
            gfx.DrawString("PHIẾU THANH TOÁN", boldFont, XBrushes.Black, new XPoint(40, 60));
            gfx.DrawString("KH - A: 82 - ĐƯỜNG VẠNH ĐAI ĐHQG", font, XBrushes.Black, new XPoint(40, 80));

            // Bill number and time
            gfx.DrawString($"Số HĐ: {order.id}", font, XBrushes.Black, new XPoint(40, 100));


            gfx.DrawString("-------------------------------------------------", font, XBrushes.Black, new XPoint(40, 180));

            // Products
            gfx.DrawString("TT  Tên món               SL    Đ.Giá    T.Tiền", boldFont, XBrushes.Black, new XPoint(40, 200));

           
            int yPosition = 220;
            foreach (var item in ordersItems)
            {
                string formattedString = $"{item.OrderDetail.Quantity,-5} {item.Drinks.name,-25} {item.OrderDetail.Quantity,-5} x {item.Drinks.price,-8} = {item.TotalPerProduct,-10}";
                gfx.DrawString(formattedString, font, XBrushes.Black, new XPoint(40, yPosition));
                yPosition += 20;
            }


            gfx.DrawString("-------------------------------------------------", font, XBrushes.Black, new XPoint(40, yPosition));
            yPosition += 20;

            
            var total = order.Total;
            var cusPayment = order.CusPayment; 
            var amountOfChange = cusPayment - total;

            gfx.DrawString($"Tiền Thanh Toán: {total}", boldFont, XBrushes.Black, new XPoint(40, yPosition));
            gfx.DrawString($"Tiền khách đưa: {cusPayment}", font, XBrushes.Black, new XPoint(40, yPosition + 20));
            gfx.DrawString($"Tiền trả lại: {amountOfChange}", font, XBrushes.Black, new XPoint(40, yPosition + 40));

          

            // Lưu tệp PDF
            document.Save(pdfPath);
        }

        // Mở tệp PDF bằng trình xem mặc định
        Process.Start(new ProcessStartInfo
        {
            FileName = pdfPath,
            UseShellExecute = true
        });
    }


}