using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using MySql.Data.MySqlClient;
using POS_App.Command;
using POS_App.Model;
using POS_App.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Microsoft.UI.Xaml.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Windows.UI.Text;
using Microsoft.UI.Text;

namespace POS_App.ViewModel;

public class EventSchedulingViewModel : INotifyPropertyChanged
{
    public class EventParameters : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int TableNumber { get; set; }
        public string Type { get; set; }
        public string Note { get; set; }
        

        public event PropertyChangedEventHandler PropertyChanged;
    }


    private DatabaseManager _dbManager;

    public ICommand SaveCommand { get; private set; }

    public ICommand ReturnCommand { get; private set; }
    public ObservableCollection<Grid> Orders { get; set; }

    public event Action OnEventSchedulingSuccessful;


    public EventParameters EventParams { get; set; }

    public EventParameters InfoParams { get; set; }

    private bool _isDefaultViewVisible;
    private bool _isEventViewVisible;

    public bool IsDefaultViewVisible
    {
        get => _isDefaultViewVisible;
        set
        {
            _isDefaultViewVisible = value;
            OnPropertyChanged(nameof(IsDefaultViewVisible));
        }
    }

    public bool IsEventViewVisible
    {
        get => _isEventViewVisible;
        set
        {
            _isEventViewVisible = value;
            OnPropertyChanged(nameof(IsEventViewVisible));
        }
    }

    public EventSchedulingViewModel()
    {
        _dbManager = new DatabaseManager();

        EventParams = new EventParameters(); // Khởi tạo LoginParameters

        InfoParams = new EventParameters();

        SaveCommand = new RelayCommand(ExecuteEvent);

        ReturnCommand = new RelayCommand(ReturnEventSchedulingPage);

        Orders = new ObservableCollection<Grid>();

        IsDefaultViewVisible = true;
        IsEventViewVisible = false;

        LoadOrders(null);
    }

    private void ExecuteEvent(object parameter)
    {

        Model.Event Event = null;
        var EventParams = parameter as EventParameters;
        if (EventParams != null)
        {

            var connection = _dbManager.GetConnection();
            if (connection != null)
            {

                string name = EventParams.Name;
                string phoneNumber = EventParams.PhoneNumber;
                string email = EventParams.Email;
                string type = EventParams.Type;
                string date = EventParams.Date;
                string time = EventParams.Time;
                int tableNumber = EventParams.TableNumber;
                string note = EventParams.Note;
                Event = new Model.Event
                {
                    Name = name,
                    PhoneNumber = phoneNumber,
                    Email = email,
                    Type = type,
                    Date = date,
                    Time = time,
                    TableNumber = tableNumber,
                    Note = note
                };

                if(Event.Note == null)
                {
                    Event.Note = "Không có ghi chú";
                }

                string dateTimeString = $"{Event.Date} {Event.Time}";
                DateTime eventDateTime = DateTime.ParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                string query = "INSERT INTO events (event_name,customer_name,phone_number ,email ,date,table_number,note ) VALUES (@eventName,@customerName, @phoneNumber, @Email, @Date, @tableNumber,@Note)";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@eventName", Event.Type);
                    cmd.Parameters.AddWithValue("@customerName", Event.Name);
                    cmd.Parameters.AddWithValue("@phoneNumber", Event.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", Event.Email);
                    cmd.Parameters.AddWithValue("@Date", eventDateTime);
                    cmd.Parameters.AddWithValue("@tableNumber", Event.TableNumber);
                    cmd.Parameters.AddWithValue("@Note", Event.Note);
                    cmd.ExecuteNonQuery();
                }
                OnEventSchedulingSuccessful?.Invoke();

                LoadOrders(null);
                ResetForm(null);
                connection.Close();
            }
        }
    }

    private void LoadOrders(object parameter)
    {
        Orders.Clear(); 
        try
        {
            var connection = _dbManager.GetConnection();
            if (connection != null)
            {
                string query = "SELECT * FROM events WHERE status='0'";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    foreach (DataRow row in dt.Rows)
                    {
                        Grid orderGrid = CreateOrderGrid(row);
                        Orders.Add(orderGrid); 
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error retrieving orders: " + ex.Message);
        }
    }

    private Grid CreateOrderGrid(DataRow row)
    {
        Grid orderGrid = new Grid
        {
            Background = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x98, 0x66, 0x50)),
            Height = 150,
            Margin = new Thickness(10, 10, 10, 0),
            CornerRadius = new CornerRadius(12)
        };

        orderGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
        orderGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

        StackPanel infoPanel = new StackPanel
        {
           
            Padding = new Thickness(10, 10, 10, 0),
            Background = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9))
            
        };
        Grid.SetRow(infoPanel, 0);

        infoPanel.Children.Add(new TextBlock { Text = "Tên sự kiện: " + row["event_name"].ToString(), Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)), FontSize=16 });
        infoPanel.Children.Add(new TextBlock { Text = "Ngày Giờ: " + row["date"].ToString(), Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)), FontSize = 16 });
        infoPanel.Children.Add(new TextBlock { Text = "Người đặt: " + row["customer_name"].ToString(), Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)), FontSize = 16 });

        infoPanel.PointerPressed += OnStackPanelPressed;

        infoPanel.Tag = row["id"].ToString();
        orderGrid.Children.Add(infoPanel);

        StackPanel buttonPanel = new StackPanel
        {
            Margin = new Thickness(10, 0, 0, 0)
        };
        Grid.SetRow(buttonPanel, 1);

        Button completeButton = new Button
        {
            Width = 120,
            Height = 40,
            Background = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)),
            CornerRadius = new CornerRadius(20),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(0, 5, 0, 0),
            Tag = row["id"].ToString()
        };
        completeButton.Click += CompleteButton_Click; // Lưu ý: Có thể xem xét chuyển đổi cách này sang command.

        completeButton.Content = new TextBlock
        {
            Text = "Complete",
            Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)),
            FontWeight= FontWeights.Bold,
            FontSize = 16,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        buttonPanel.Children.Add(completeButton);
        orderGrid.Children.Add(buttonPanel);

        return orderGrid;
    }

    

    private void ResetForm(object parameter)
    {
        EventParams.Name = string.Empty;
        EventParams.PhoneNumber = string.Empty;
        EventParams.Email = string.Empty;
        EventParams.Note = string.Empty;
        EventParams.TableNumber=0;
        EventParams.Time=string.Empty;
        EventParams.Date=string.Empty;
        EventParams.Type=string.Empty;
    }

    private void CompleteButton_Click(object sender, RoutedEventArgs e)
    {
        int id = Convert.ToInt32((sender as Button).Tag);
        string query = "UPDATE events SET status=1 WHERE id = @id";

        using (var connection = _dbManager.GetConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }

        LoadOrders(null);
    }

    public void OnStackPanelPressed(object sender, PointerRoutedEventArgs e)
    {
        if (IsDefaultViewVisible)
        {
            IsDefaultViewVisible = false;
            IsEventViewVisible = true;
        }

        int id = Convert.ToInt32((sender as StackPanel).Tag);
        Model.Event Event = null;
        string query = "SELECT * FROM events WHERE id=@id";
        using (var connection = _dbManager.GetConnection())
        {
            using (MySqlCommand cmd = new MySqlCommand(query, connection))
            {

                cmd.Parameters.AddWithValue("@id", id);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("date"));
                        Event = new Model.Event
                        {
                            Name = reader.GetString("customer_name"),
                            PhoneNumber = reader.GetString("phone_number"),
                            Email = reader.GetString("email"),
                            TableNumber = reader.GetInt32("table_number"),
                            Note = reader.GetString("note"),
                            Date = dateTime.ToString("yyyy-MM-dd"),
                            Time = dateTime.ToString("HH:mm:ss")
                        };
                        
                    }
                }
            }
        }

       
        InfoParams.Name = Event.Name;
        InfoParams.PhoneNumber=Event.PhoneNumber;
        InfoParams.Email=Event.Email;
        InfoParams.Date = Event.Date;
        InfoParams.Time = Event.Time;
        InfoParams.Note = Event.Note;
        InfoParams.TableNumber = Event.TableNumber;
        
    }

    private void ReturnEventSchedulingPage(object parameter)
    {
        if (IsEventViewVisible)
        {
            IsDefaultViewVisible = true;
            IsEventViewVisible = false;
            //LoadOrders(null);
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
    
