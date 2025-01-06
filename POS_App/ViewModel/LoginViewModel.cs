using System;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using POS_App.Command;
using POS_App.Model;
using Microsoft.UI.Xaml.Controls;
using Windows.System;
using POS_App.Service;
using System.ComponentModel;
using Windows.Storage;
using System.Diagnostics;


namespace POS_App.ViewModel;



public class LoginViewModel
{


    public class LoginParameters : INotifyPropertyChanged
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Noti {  get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }

   
    private DatabaseManager _dbManager;

    public ICommand LoginCommand { get; private set; }
    public event Action OnLoginSuccessful;
       
  
    public LoginParameters LoginParams { get; set; }

    public LoginViewModel()
    {
        _dbManager = new DatabaseManager();

        LoginParams = new LoginParameters(); // Khởi tạo LoginParameters

        LoginCommand = new RelayCommand(ExecuteLogin);
    }        

      
    public void ExecuteLogin(object parameter)
    {
        Model.User user = null;

        var loginParams = parameter as LoginParameters;
        if (loginParams != null)
        {
            string email = loginParams.Username;
            if(string.IsNullOrEmpty(email))
            {
                loginParams.Noti = "Please enter your email!";
                return;
            }
            string password = loginParams.Password;
            if (string.IsNullOrEmpty(password))
            {
                loginParams.Noti = "Please enter your password!";
                return;
            }

            var connection = _dbManager.GetConnection();
            if (connection != null)
            {
                string query = "SELECT * FROM users WHERE email=@email";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@email", email);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new Model.User
                        {
                            Id = reader.GetInt32("id"),
                            Email = reader.GetString("email"),
                            PassWord = reader.GetString("password"),
                            Salt=reader.GetString("salt"),
                            FirstName=reader.GetString("first_name"),
                            Role= reader.GetString("role")
                        };
                    }
                }

                if (user == null)
                {
                    loginParams.Noti = "Email or Password is incorrect!";
                }
                else
                {
                    
                


                    string passHashed = Hasher.Hash(password + user.Salt);

                    if(passHashed == user.PassWord)
                    {
                        var localSettings = ApplicationData.Current.LocalSettings;
                        localSettings.Values.Clear();

                        localSettings.Values["UserId"] = user.Id;
                        localSettings.Values["Username"] = user.Email;
                        localSettings.Values["FirstName"] = user.FirstName;
                        localSettings.Values["Role"] = user.Role;
                        OnLoginSuccessful?.Invoke();
                    }
                    else
                    {
                        loginParams.Noti = "Email or Password is incorrect!";
                    }

                }

                connection.Close();
            }
        }
    }
    
}
