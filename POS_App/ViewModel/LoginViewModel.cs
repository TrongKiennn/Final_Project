﻿using System;
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
    public event Action<Model.User> OnLoginSuccessful;
       
  
    public LoginParameters LoginParams { get; set; }

    public LoginViewModel()
    {
        _dbManager = new DatabaseManager();

        LoginParams = new LoginParameters(); // Khởi tạo LoginParameters

        LoginCommand = new RelayCommand(ExecuteLogin);
    }        

      
    private void ExecuteLogin(object parameter)
    {
        Model.User user = null;

        var loginParams = parameter as LoginParameters;
        if (loginParams != null)
        {
            string email = loginParams.Username;
               

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
                            FirstName=reader.GetString("first_name")
                        };
                    }
                }

                if (user == null)
                {
                    loginParams.Noti = "UserName or Password is incorrect!";
                }
                else
                {
                    string password = loginParams.Password;

                    string passHashed = Hasher.Hash(password + user.Salt);

                    if(passHashed == user.PassWord)
                    {
                        OnLoginSuccessful?.Invoke(user);
                    }
                    else
                    {
                        loginParams.Noti = "UserName or Password is incorrect!";
                    }

                }

                connection.Close();
            }
        }
    }
    
}
