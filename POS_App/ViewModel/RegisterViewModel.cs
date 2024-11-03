using MySql.Data.MySqlClient;
using POS_App.Command;
using POS_App.Model;
using POS_App.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POS_App.ViewModel
{
    public class RegisterViewModel
    {
        public class RegisterParameters : INotifyPropertyChanged
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string email { get; set; }
            public string password { get; set; }

            public string Noti { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

        }


        private DatabaseManager _dbManager;

        public ICommand RegisterCommand { get; private set; }
        public event Action OnRegisterSuccessful;

        
        public RegisterParameters RegisterParams { get; set; }

        public RegisterViewModel()
        {
            _dbManager = new DatabaseManager();

            RegisterParams = new RegisterParameters(); // Khởi tạo LoginParameters

            RegisterCommand = new RelayCommand(ExecuteRegister);
        }


        private void ExecuteRegister(object parameter)
        {
            Model.User user = null;

            var registerParams = parameter as RegisterParameters;
            if (registerParams != null)
            {
                string email = registerParams.email;


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
                                Email = reader.GetString("email"),
                            };
                        }
                    }

                    if (user == null)
                    {
                        string password = registerParams.password;
                        string firstName = registerParams.first_name;
                        string lastName = registerParams.last_name;

                        string salt = genSalt.GenSalt(50);

                        user = new Model.User
                        {
                            Email =email,
                            PassWord=Hasher.Hash(password+salt),
                            FirstName=firstName,
                            LastName=lastName,
                            Salt=salt
                        };

                       
                        query = "INSERT INTO users (email, password, first_name, last_name, salt) VALUES (@Email, @PassWord, @FirstName, @LastName, @Salt)";
                        using (cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@Email", user.Email);
                            cmd.Parameters.AddWithValue("@PassWord", user.PassWord);
                            cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                            cmd.Parameters.AddWithValue("@LastName", user.LastName);
                            cmd.Parameters.AddWithValue("@Salt", user.Salt);

                            cmd.ExecuteNonQuery();  // Thực thi câu lệnh
                        }
                        OnRegisterSuccessful?.Invoke();


                    }
                    else
                    {
                        registerParams.Noti = "Account already exists";
                    }

                    connection.Close();
                }
            }
        }

    }
}
