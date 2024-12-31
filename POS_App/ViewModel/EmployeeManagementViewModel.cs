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
using Castle.Core.Resource;


namespace POS_App.ViewModel
{


    public class EmployeeManagementViewModel : INotifyPropertyChanged
    {
        IDao_Employee_Information _Dao_Employee_Information;
        IDao_Employee_Attendance _Dao_Employee_Attendance;
        IDao_User _Dao_User;

        public ICommand TemporarySaveEmployeeAccount { get; set; }
        public ICommand SaveEmployeeInfoAndAccount { get; set; }
        public ICommand EmployeeClickCommand { get; set; }
        public ICommand CheckInCommand { get; set; }
        public ICommand CheckOutCommand { get; set; }
        public ICommand ConfirmDeleteCommand { get; set; }

        public ObservableCollection<employeeInfo> Employees { get; set; }

        public int TotalEmployees { get; set; }
        public ErrorHandling ErrorHandling { get; set; }

        private Model.User _newEmployee;
        public Model.User NewEmployee
        {
            get => _newEmployee;
            set => SetProperty(ref _newEmployee, value);
        }

        private employeeInfo _newEmployeeInfo;
        public employeeInfo NewEmployeeInfo
        {
            get => _newEmployeeInfo;
            set => SetProperty(ref _newEmployeeInfo, value);
        }

        private employeeInfo _selectedEmployee;
        public employeeInfo SelectedEmployee
        {
            get => _selectedEmployee;
            set => SetProperty(ref _selectedEmployee, value);
        }

        private employeeAttendance _selectedEmployeeAttend;
        public employeeAttendance SelectedEmployeeAttend
        {
            get => _selectedEmployeeAttend;
            set => SetProperty(ref _selectedEmployeeAttend, value);
        }

        private string _userRole;
        public string UserRole
        {
            get => _userRole;
            set => SetProperty(ref _userRole, value);
        }

        public bool IsCheckAccount;
        public bool IsCheckEmployeeInfo = false;

        private ErrorHandling _checkAccountInfo;
        public ErrorHandling CheckAccountInfo
        {
            get => _checkAccountInfo;
            set => SetProperty(ref _checkAccountInfo, value);
        }

        private ErrorHandling _checkEmployeeInfo;
        public ErrorHandling CheckEmployeeInfo
        {
            get => _checkEmployeeInfo;
            set => SetProperty(ref _checkEmployeeInfo, value);
        }

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

        public EmployeeManagementViewModel()
        {
            _Dao_Employee_Information = ServiceFactory.GetChildOf(typeof(IDao_Employee_Information)) as IDao_Employee_Information;
            _Dao_User = ServiceFactory.GetChildOf(typeof(IDao_User)) as IDao_User;
            _Dao_Employee_Attendance = ServiceFactory.GetChildOf(typeof(IDao_Employee_Attendance)) as IDao_Employee_Attendance;

            TemporarySaveEmployeeAccount = new RelayCommand(_=>OnTemporarySaveEmployeeAccount());
            SaveEmployeeInfoAndAccount = new RelayCommand(_ => OnSaveEmployeeInfoAndAccount());
            ConfirmDeleteCommand = new RelayCommand(_ => OnConfirmDelete());
            EmployeeClickCommand = new RelayCommand<employeeInfo>(OnEmployeeClick);
            CheckInCommand = new RelayCommand(_ => OnCheckIn());
            CheckOutCommand = new RelayCommand(_ => OnCheckOut());

            NewEmployee = new Model.User();
            NewEmployeeInfo = new employeeInfo();
            CheckAccountInfo = new ErrorHandling();
            var localSettings = ApplicationData.Current.LocalSettings;
            ErrorHandling = new ErrorHandling();

            if (localSettings.Values.ContainsKey("Role"))
            {
                UserRole = localSettings.Values["Role"] as string;
            }
            LoadData();
        }

        private void OnEmployeeClick(employeeInfo selectedEmployee)
        {
            ErrorHandling.ErrorMessage = "";
            SelectedEmployee = selectedEmployee;
            var(total_working_days, total_working_hours) = _Dao_Employee_Attendance.GetEmployeeAttendence(selectedEmployee.User_Id);
            var employeeAttend = _Dao_Employee_Attendance.FindEmployeeAttendanceByUser_IdToday(SelectedEmployee.User_Id);
            if (employeeAttend != null)
            {
                SelectedEmployeeAttend = employeeAttend;
                SelectedEmployeeAttend.MonthlyAttendance = total_working_days;
                SelectedEmployeeAttend.TotalHours = total_working_hours;
            }
            else
            {
                SelectedEmployeeAttend = new employeeAttendance
                {
                    MonthlyAttendance = total_working_days,
                    TotalHours = total_working_hours
                };
            }
        }
        public void LoadData()
        {
            var (items,count) = _Dao_Employee_Information.GetEmployees();
            TotalEmployees = count;
            Employees = new ObservableCollection<employeeInfo>(items);
            foreach (employeeInfo item in items) {
                Debug.WriteLine(item.FullName);
            }
        }

        private void PerformSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {

                LoadData();
            }
            else
            {
                var filteredEmployees = _Dao_Employee_Information.GetEmployeeByFilter(SearchText);
                Employees = new ObservableCollection<employeeInfo>(filteredEmployees);
            }
        }
        private void OnTemporarySaveEmployeeAccount()
        {
            var User = _Dao_User.FindUserByEmail(NewEmployee.Email);
            if (!ValidatorEmail.IsValidEmail(NewEmployee.Email))
            {
                CheckAccountInfo.ErrorMessage = "Please enter a valid email address.";
                IsCheckAccount = false;
                return;
            }
            var validations = new List<(string FieldValue, string ErrorMessage)>
            {
                (NewEmployee.Email, "Email cannot be blank."),
                (NewEmployee.PassWord, "Password cannot be blank."),
                (NewEmployee.ConfirmPassword, "Confirm Password cannot be blank."),
                (NewEmployee.FirstName, "First Name cannot be blank."),
                (NewEmployee.LastName, "Last Name cannot be blank."),
                (NewEmployee.Role, "Position cannot be blank.")
            };

            foreach (var (fieldValue, errorMessage) in validations)
            {
                if (string.IsNullOrWhiteSpace(fieldValue))
                {
                    CheckAccountInfo.ErrorMessage = errorMessage;
                    IsCheckAccount = false;
                    return;
                }
            }


            if (User != null)
            {
                CheckAccountInfo.ErrorMessage = "Email already exists.";
                IsCheckAccount = false;
                return;
            }

            if (!ValidatorPass.IsValidPassword(NewEmployee.PassWord))
            {
                CheckAccountInfo.ErrorMessage = "Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, one digit, and one special character.";
                IsCheckAccount = false;
                return;
            }

            if (NewEmployee.PassWord != NewEmployee.ConfirmPassword)
            {
                CheckAccountInfo.ErrorMessage = "Passwords do not match.";
                IsCheckAccount = false;
                return;
            }
            if(NewEmployee.Role!= "employee" && NewEmployee.Role != "manager")
            {
                CheckAccountInfo.ErrorMessage = "Position must be 'employee' or 'manager'.";
                IsCheckAccount = false;
                return;
            }

            IsCheckAccount = true;
            NewEmployeeInfo.FullName = NewEmployee.FirstName + " " + NewEmployee.LastName;
        }

        private void OnSaveEmployeeInfoAndAccount()
        {
            var validations = new List<(string FieldValue, string ErrorMessage)>
            {
                (NewEmployeeInfo.FullName, "Email cannot be blank."),
                (NewEmployeeInfo.PhoneNumber, "Phone number cannot be blank."),
                (NewEmployeeInfo.Gender, "Gender cannot be blank."),
                (NewEmployeeInfo.Position, "Position cannot be blank.")
            };

            foreach (var (fieldValue, errorMessage) in validations)
            {
                if (string.IsNullOrWhiteSpace(fieldValue))
                {
                    CheckAccountInfo.ErrorMessage = errorMessage;
                    IsCheckEmployeeInfo = false;
                    return;
                }
            }

            if (NewEmployeeInfo.Position != "employee" && NewEmployeeInfo.Position != "manager")
            {
                CheckAccountInfo.ErrorMessage = "Position must be 'employee' or 'manager'.";
                IsCheckEmployeeInfo = false;
                return;
            }

            if (NewEmployeeInfo.Gender != "Male" && NewEmployeeInfo.Gender != "Female" && NewEmployeeInfo.Gender != "Other")
            {
                CheckAccountInfo.ErrorMessage = "Gender must be 'Male' or 'Female' or 'Other'.";
                IsCheckEmployeeInfo = false;
                return;
            }

            string salt = genSalt.GenSalt(50);
            NewEmployee.Salt = salt;
            NewEmployee.PassWord = Hasher.Hash(NewEmployee.PassWord + salt);

            try {
                NewEmployeeInfo.User_Id = _Dao_User.CreateUser(NewEmployee);
            } catch {
                CheckAccountInfo.ErrorMessage = "An error occurred while creating your account. Please try again.";
                IsCheckEmployeeInfo = false;
                return;
            }
            try {
                _Dao_Employee_Information.CreateEmployeeInfo(NewEmployeeInfo);
            } catch
            {
                CheckAccountInfo.ErrorMessage = "Some required fields are missing. Please complete all the fields and try again.";
                IsCheckEmployeeInfo = false;
                return;
            }
           
            IsCheckEmployeeInfo = true;
            LoadData();
        }

        private void OnCheckIn() {
            ErrorHandling.ErrorMessage = "";
            if (SelectedEmployee == null)
            {
                ErrorHandling.ErrorMessage = "Please select an employee.";
                return;
            }
            var employeeAttend = _Dao_Employee_Attendance.FindEmployeeAttendanceByUser_IdToday(SelectedEmployee.User_Id);
            if (employeeAttend!=null && employeeAttend.CheckInTime!=null)
            {
                ErrorHandling.ErrorMessage = "Employee has already checked in.";
                SelectedEmployeeAttend = employeeAttend;
                var (total_working_days, total_working_hours) = _Dao_Employee_Attendance.GetEmployeeAttendence(SelectedEmployee.User_Id);
                SelectedEmployeeAttend.MonthlyAttendance = total_working_days;
                SelectedEmployeeAttend.TotalHours = total_working_hours;
                return;
            }

            _Dao_Employee_Attendance.CreateEmployeeAttendance(SelectedEmployee.User_Id);
            _Dao_Employee_Attendance.UpdateCheckInTime(SelectedEmployee.User_Id);

            employeeAttend = _Dao_Employee_Attendance.FindEmployeeAttendanceByUser_IdToday(SelectedEmployee.User_Id);
            SelectedEmployeeAttend = employeeAttend;
      
        }
        private void OnCheckOut()
        {
            ErrorHandling.ErrorMessage = "";
            var employeeAttend = _Dao_Employee_Attendance.FindEmployeeAttendanceByUser_IdToday(SelectedEmployee.User_Id);
            if (employeeAttend == null)
            {
                ErrorHandling.ErrorMessage = "Employee has not checked in yet.";
                return;
            }
            if(employeeAttend.CheckOutTime != null)
            {
                ErrorHandling.ErrorMessage = "Employee has already checked out.";
                return;
            }
            _Dao_Employee_Attendance.UpdateCheckOutTime(SelectedEmployee.User_Id);
            employeeAttend = _Dao_Employee_Attendance.FindEmployeeAttendanceByUser_IdToday(SelectedEmployee.User_Id);
            SelectedEmployeeAttend = employeeAttend;

            var (total_working_days, total_working_hours) = _Dao_Employee_Attendance.GetEmployeeAttendence(SelectedEmployee.User_Id);
            SelectedEmployeeAttend.MonthlyAttendance = total_working_days;
            SelectedEmployeeAttend.TotalHours = total_working_hours;
         
        }

        private void OnConfirmDelete()
        {
            if (SelectedEmployee != null)
            {
                _Dao_User.DeleteUserById(SelectedEmployee.User_Id);
                SelectedEmployeeAttend = new employeeAttendance();
                SelectedEmployee=new employeeInfo();
                LoadData();
            }
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
