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
using POS_App.Service.DataAccess;
using System.Runtime.CompilerServices;

namespace POS_App.ViewModel;

public class EventSchedulingViewModel : INotifyPropertyChanged
{
    IDao_Events _Dao_Events;
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
    public ObservableCollection<Grid> Events { get; set; }

    public ObservableCollection<Event> listEvents { get; set; }

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

    private ErrorHandling _checkEventInfo;
    public ErrorHandling CheckEventInfo
    {
        get => _checkEventInfo;
        set => SetProperty(ref _checkEventInfo, value);
    }

    public EventSchedulingViewModel()
    {
      
        EventParams = new EventParameters();

        InfoParams = new EventParameters();

        SaveCommand = new RelayCommand(ExecuteEvent);

        ReturnCommand = new RelayCommand(ReturnEventSchedulingPage);

        Events = new ObservableCollection<Grid>();
        _Dao_Events = ServiceFactory.GetChildOf(typeof(IDao_Events)) as IDao_Events;

        IsDefaultViewVisible = true;
        IsEventViewVisible = false;

        LoadEvents(null);
    }

    private void ExecuteEvent(object parameter)
    {
        CheckEventInfo = new ErrorHandling();

        Model.Event Event = null;
        var EventParams = parameter as EventParameters;
        if (EventParams != null)
        {

            var validations = new List<(string FieldValue, string ErrorMessage)>
            {
                 (EventParams.Name, "Name cannot be blank."),
                (EventParams.PhoneNumber, "Phone number cannot be blank."),
                (EventParams.Email, "Email cannot be blank."),
                (EventParams.Type, "Main content of event cannot be blank."),
                (EventParams.Date, "Date cannot be blank."),
                (EventParams.Time, "Time cannot be blank."),
            };

            foreach (var (fieldValue, errorMessage) in validations)
            {
                if (string.IsNullOrWhiteSpace(fieldValue))
                {
                    CheckEventInfo.ErrorMessage = errorMessage;
                    return;
                }
            }

           
            if (!DateTime.TryParseExact(EventParams.Date, "yyyy-M-d", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                CheckEventInfo.ErrorMessage = "Date must be in the format yyyy-M-d.";
                return;
            }



            if (!DateTime.TryParseExact(EventParams.Time, "H:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                CheckEventInfo.ErrorMessage = "Time must be in the format H:mm:ss.";
                return;
            }


          
            string dateTimeString = $"{EventParams.Date} {EventParams.Time}";
            DateTime eventDateTime = DateTime.ParseExact(dateTimeString, "yyyy-M-d H:mm:ss", CultureInfo.InvariantCulture);


            if (eventDateTime<DateTime.Now)
            {
                CheckEventInfo.ErrorMessage = "Date and time must be greater than current date and time.";
                return;
            }

            if (EventParams.TableNumber <= 0)
            {
                CheckEventInfo.ErrorMessage = "Table number must be greater than 0.";
                return;
            }

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
            
            _Dao_Events.AddEvent(Event);
            OnEventSchedulingSuccessful?.Invoke();

            LoadEvents(null);
            ResetForm(null);

            CheckEventInfo = new ErrorHandling();
        }
    }

    private void LoadEvents(object parameter)
    {
        Events.Clear(); 
        try
        {
            var result = _Dao_Events.GetAllEvent();
            listEvents=new ObservableCollection<Event>(result);


            foreach (var eventItem in listEvents)
            {
                    Grid eventGrid = CreateEventGrid(eventItem);
                    Events.Add(eventGrid);
                
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Error retrieving Events: " + ex.Message);
        }
    }

    private Grid CreateEventGrid(Event eventItem)
    {
        Grid eventGrid = new Grid
        {
            Background = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x98, 0x66, 0x50)),
            Height = 150,
            Margin = new Thickness(10, 10, 10, 0),
            CornerRadius = new CornerRadius(12)
        };

        eventGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100) });
        eventGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(50) });

        // Panel hiển thị thông tin sự kiện
        StackPanel infoPanel = new StackPanel
        {
            Padding = new Thickness(10, 10, 10, 0),
            Background = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9))
        };
        Grid.SetRow(infoPanel, 0);

        infoPanel.Children.Add(new TextBlock
        {
            Text = "Event Name: " + eventItem.Name,
            Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)),
            FontSize = 16
        });

        infoPanel.Children.Add(new TextBlock
        {
            Text = "Appointment: " + eventItem.Date + " " + eventItem.Time,
            Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)),
            FontSize = 16
        });

        infoPanel.Children.Add(new TextBlock
        {
            Text = "Booker: " + eventItem.Name,
            Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)),
            FontSize = 16
        });

        infoPanel.PointerPressed += OnStackPanelPressed;

        infoPanel.Tag = eventItem.Id.ToString();
        eventGrid.Children.Add(infoPanel);

        // Panel chứa các nút chức năng
        StackPanel buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Center,
            Margin = new Thickness(10, 0, 0, 0)
        };
        Grid.SetRow(buttonPanel, 1);

        // Nút "Complete"
        Button completeButton = new Button
        {
            Width = 120,
            Height = 40,
            Background = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0x79, 0x47, 0x30)),
            CornerRadius = new CornerRadius(20),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 5, 5, 5),
            Tag = eventItem.Id.ToString()
        };
        completeButton.Click += CompleteButton_Click;

        completeButton.Content = new TextBlock
        {
            Text = "Complete",
            Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)),
            FontWeight = FontWeights.Bold,
            FontSize = 16,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        // Nút "Cancel"
        Button cancelButton = new Button
        {
            Width = 120,
            Height = 40,
            Background = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xFF, 0x45, 0x45)),
            CornerRadius = new CornerRadius(20),
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5, 5, 5, 5),
            Tag = eventItem.Id.ToString()
        };
        cancelButton.Click += CancelButton_Click;

        cancelButton.Content = new TextBlock
        {
            Text = "Cancel",
            Foreground = new SolidColorBrush(ColorHelper.FromArgb(0xFF, 0xff, 0xff, 0xd9)),
            FontWeight = FontWeights.Bold,
            FontSize = 16,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };

        // Thêm các nút vào panel
        buttonPanel.Children.Add(completeButton);
        buttonPanel.Children.Add(cancelButton);
        eventGrid.Children.Add(buttonPanel);

        return eventGrid;
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

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        int id = Convert.ToInt32((sender as Button).Tag);
        _Dao_Events.UpdateEventStatus(id, "cancel");

        LoadEvents(null);
    }
    private void CompleteButton_Click(object sender, RoutedEventArgs e)
    {
        int id = Convert.ToInt32((sender as Button).Tag);
        _Dao_Events.UpdateEventStatus(id,"complete");

        LoadEvents(null);
    }

    public void OnStackPanelPressed(object sender, PointerRoutedEventArgs e)
    {
        if (IsDefaultViewVisible)
        {
            IsDefaultViewVisible = false;
            IsEventViewVisible = true;
        }

        int id = Convert.ToInt32((sender as StackPanel).Tag);
        Model.Event Event = _Dao_Events.GetEventById(id);
       
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
            //LoadEvents(null);
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
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
}
    
