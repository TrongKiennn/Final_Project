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
using MySql.Data.MySqlClient;
using System.Data;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Windows.Storage;
using System.IO;
using Windows.System;


namespace POS_App.ViewModel
{


    public class StatisticViewModel : INotifyPropertyChanged
    {
        IDao_Order _Dao_Order;
        IDao_Order_Item _Dao_Order_Item;
        public ObservableCollection<Drinks> Drinks { get; set; }
        private DateTimeOffset _selectedDay;
        public DateTimeOffset SelectedDay
        {
            get { return _selectedDay; }
            set
            {
                if (_selectedDay != value)
                {
                    _selectedDay = value;
                    OnPropertyChanged(nameof(SelectedDay)); 
                    LoadData(); 
                }
            }
        }
        private decimal _totalByDay;
        public decimal TotalByDay
        {
            get => _totalByDay;
            set => SetProperty(ref _totalByDay, value);
        }
        private decimal _totalByMonth;
        public decimal TotalByMonth
        {
            get => _totalByMonth;
            set => SetProperty(ref _totalByMonth, value);
        }
        private decimal _totalByYear;
        public decimal TotalByYear
        {
            get => _totalByYear;
            set => SetProperty(ref _totalByYear, value);
        }

        private int _totalOrdersByDay;
        public int TotalOrdersByDay
        {
            get => _totalOrdersByDay;
            set => SetProperty(ref _totalOrdersByDay, value);
        }
        private int _totalOrdersByMonth;
        public int TotalOrdersByMonth
        {
            get => _totalOrdersByMonth;
            set => SetProperty(ref _totalOrdersByMonth, value);
        }

        public StatisticViewModel()
        {
            _Dao_Order = ServiceFactory.GetChildOf(typeof(IDao_Order)) as IDao_Order;
            _Dao_Order_Item = ServiceFactory.GetChildOf(typeof(IDao_Order_Item)) as IDao_Order_Item;
            SelectedDay = DateTime.Today;
            LoadData();
           
        }

        
        public void LoadData()
        {
            Debug.WriteLine("LoadData");
            Debug.WriteLine(SelectedDay);

            var(data1, data2, data3,data4,data5) = _Dao_Order.GetTotalAmountAndOrdersByDate(SelectedDay);
            TotalByDay = data1;
            TotalByMonth = data2;
            TotalByYear = data3;
            TotalOrdersByDay = data4;
            TotalOrdersByMonth = data5;
            var items = _Dao_Order_Item.GetTopSellingDrinks();
            Drinks = new ObservableCollection<Drinks>(items);
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
    }
}
