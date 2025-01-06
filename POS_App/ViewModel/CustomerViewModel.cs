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


    public class CustomerViewModel : INotifyPropertyChanged
    {
        IDao_Customer _Dao_Customer;
        IDao_Rank _Dao_Rank;

        public ICommand CustomerClickCommand { get; set; }
        public ICommand SaveCustomerCommand { get; set; }
        public ICommand UpdateCustomerCommand { get; set; }
        public ICommand SaveSettingCommand { get; set; }
        public ObservableCollection<Model.Customer> Customers { get; set; }

        private Model.Rank _bronzeRank;
        public Model.Rank BronzeRank
        {
            get => _bronzeRank;
            set => SetProperty(ref _bronzeRank, value);
        }
        private Model.Rank _silverRank;
        public Model.Rank SilverRank
        {
            get => _silverRank;
            set => SetProperty(ref _silverRank, value);
        }
        private Model.Rank _goldRank;
        public Model.Rank GoldRank
        {
            get => _goldRank;
            set => SetProperty(ref _goldRank, value);
        }

        private int _totalBronze;
        public int TotalBronze
        {
            get => _totalBronze;
            set => SetProperty(ref _totalBronze, value);
        }

        private int _totalSilver;
        public int TotalSilver
        {
            get => _totalSilver;
            set => SetProperty(ref _totalSilver, value);
        }

        private int _totalGold;
        public int TotalGold
        {
            get => _totalGold;
            set => SetProperty(ref _totalGold, value);
        }
        private decimal _pointPerDollar;
        public decimal PointPerDollar
        {
            get => _pointPerDollar;
            set => SetProperty(ref _pointPerDollar, value);
        }

        private string _userRole;
        public string UserRole
        {
            get => _userRole;
            set => SetProperty(ref _userRole, value);
        }

        private Model.Customer _selectedCustomer;
        public Model.Customer SelectedCustomer
        {
            get => _selectedCustomer;
            set => SetProperty(ref _selectedCustomer, value);
        }

        private Model.Customer _newCustomer;
        public Model.Customer NewCustomer
        {
            get => _newCustomer;
            set => SetProperty(ref _newCustomer, value);
        }

        private Model.Customer _preCustomer;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    PerformSearch();
                }
            }
        }

        private ErrorHandling _checkCusInfo;
        public ErrorHandling CheckCusInfo
        {
            get => _checkCusInfo;
            set => SetProperty(ref _checkCusInfo, value);
        }
        public bool isCheck = false;
        public bool isManagerOrAdmin = false;
        public CustomerViewModel()
        {
            _Dao_Customer = ServiceFactory.GetChildOf(typeof(IDao_Customer)) as IDao_Customer;
            _Dao_Rank = ServiceFactory.GetChildOf(typeof(IDao_Rank)) as IDao_Rank;

            SaveCustomerCommand = new RelayCommand<Model.Customer>(_=>OnSaveCustomer());
            CustomerClickCommand = new RelayCommand<Model.Customer>(OnCustomerClick);
            UpdateCustomerCommand = new RelayCommand<Model.Customer>(_=>UpdateCustomer());
            SaveSettingCommand = new RelayCommand<Model.Rank>(_=>SaveSetting());

            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey("Role"))
            {
                UserRole = localSettings.Values["Role"] as string;
            }
            LoadData();
            
            NewCustomer = new Model.Customer();
            CheckCusInfo = new ErrorHandling();

            
        }

        public void checkRole()
        {
            if (UserRole == "Manager" || UserRole == "Admin")
            {
                isManagerOrAdmin = true;
                return;
            }
            CheckCusInfo.ErrorMessage = "You don't have permission to go to this!";


        }
        private void OnCustomerClick(Model.Customer selectedCustomer)
        {
            SelectedCustomer = selectedCustomer;
            _preCustomer = SelectedCustomer;
        }

        private void OnSaveCustomer()
        {
            isCheck = false;

            var validations = new List<(string FieldValue, string ErrorMessage)>
            {
                 (NewCustomer.FullName, "Full name cannot be blank."),
                (NewCustomer.PhoneNumber, "Phone number cannot be blank."),
                (NewCustomer.Email, "Email cannot be blank."),
            };

            foreach (var (fieldValue, errorMessage) in validations)
            {
                if (string.IsNullOrWhiteSpace(fieldValue))
                {
                    CheckCusInfo.ErrorMessage = errorMessage;
                    return;
                }
            }

            if (!ValidatorEmail.IsValidEmail(NewCustomer.Email))
            {
                CheckCusInfo.ErrorMessage = "Please enter a valid email address.";
                return;
            }

            var findCustomer = _Dao_Customer.FindCustomerByPhone(NewCustomer.PhoneNumber);
            if (findCustomer != null)
            {
                CheckCusInfo.ErrorMessage = "This customer is already a registered member.";
                return;
            }
            else
            {
                try
                {
                    _Dao_Customer.CreateCustomer(NewCustomer);
                    LoadData();
                    isCheck = true;
                    CheckCusInfo.ErrorMessage = "";
                }
                catch (Exception ex)
                {
                    CheckCusInfo.ErrorMessage = "We encountered an issue while processing your membership registration. Please ensure all information is correct and try again.";
                    Debug.WriteLine(ex.Message);
                }
                    
            }
            
        }
        private void UpdateCustomer() {
            isCheck = false;
            if (SelectedCustomer != null)
            {
                try
                {
                    _Dao_Customer.UpdateCustomerInfo(SelectedCustomer, _preCustomer.PhoneNumber);
                    LoadData();
                    isCheck = true;
                    CheckCusInfo.ErrorMessage = "";
                }
                catch(Exception ex)
                {
                    CheckCusInfo.ErrorMessage = "An error occurred while updating your information. Please try again.";
                    Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                CheckCusInfo.ErrorMessage = "Please select a customer to update.";
            }
           
        }
        public void LoadData()
        {
           var (customers, totalBronze, totalSilver, totalGold) = _Dao_Customer.GetAllCustomer();
            Customers = new ObservableCollection<Model.Customer>(customers);
            TotalBronze = totalBronze;
            TotalSilver = totalSilver;
            TotalGold = totalGold;

            BronzeRank = _Dao_Rank.GetRank("Bronze");
            SilverRank = _Dao_Rank.GetRank("Silver");
            GoldRank = _Dao_Rank.GetRank("Gold");
            PointPerDollar = BronzeRank.PointPerDollar;
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                
                LoadData();
            }
            else
            {
                var filteredCustomers = _Dao_Customer.GetCustomerByFilter(SearchText);
                Customers = new ObservableCollection<Model.Customer>(filteredCustomers);
            }
        }

        private void SaveSetting()
        {
            _Dao_Rank.UpdateRankSetting(BronzeRank, PointPerDollar);
            _Dao_Rank.UpdateRankSetting(SilverRank, PointPerDollar);
            _Dao_Rank.UpdateRankSetting(GoldRank, PointPerDollar);
            LoadData();
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
