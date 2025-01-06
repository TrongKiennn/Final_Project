using Castle.Components.DictionaryAdapter.Xml;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Common;
using POS_App.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_App.Service.DataAccess.IDao_Order;

namespace POS_App.Service.DataAccess;

public class Dao_User : IDao_User
{
    public User FindUserByEmail(string email)
    {
        User user = null;
        try
        {
            var connectionString = GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"SELECT * FROM users WHERE email = @Email;";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32("id"),
                                Email = reader.GetString("email"),
                                PassWord = reader.GetString("password"),
                                FirstName = reader.GetString("first_name"),
                                LastName = reader.GetString("last_name"),
                                Salt = reader.GetString("salt"),
                                Role = reader.GetString("role")
                            };
                        }
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            throw new Exception("Database error: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred: " + ex.Message, ex);
        }
        return user;
    }

    public int CreateUser(User user)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
               

                var query = "INSERT INTO users (email, password, first_name, last_name, salt, role) " +
                            "VALUES (@Email, @PassWord, @FirstName, @LastName, @Salt,@Role)";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PassWord", user.PassWord);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Salt", user.Salt);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    connection.Open();
                    cmd.ExecuteNonQuery();


                    return Convert.ToInt32(cmd.LastInsertedId);
                }
            }
        }
        catch (MySqlException ex)
        {
            throw new Exception("Database error: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred: " + ex.Message, ex);
        }
    }

    public void UpdateUserRole(int User_id, string role)
    {
        Debug.WriteLine("User_id: " + User_id);
        Debug.WriteLine("Role: " + role);
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"UPDATE users 
                        SET role = @role 
                        WHERE id = @id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", User_id);
                    command.Parameters.AddWithValue("@role", role);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Employee information updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("No employee found with the given ID.");
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"MySQL error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error: {ex.Message}");
        }
    }
    public void DeleteUserById(int id)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                var sql = @"DELETE FROM users WHERE id=@id;";
                var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);
              
                command.ExecuteNonQuery();
            }
        }
        catch (MySqlException ex)
        {
            throw new Exception("Database error: " + ex.Message, ex);
        }
        catch (Exception ex)
        {
            throw new Exception("An unexpected error occurred: " + ex.Message, ex);
        }
    }

    private static string GetConnectionString()
    {
        var connectionString = "" +
            "Server=localhost;" +
            "Database=pos_manager;" +
            "User=root;" +
            "Password=1234;";

        return connectionString;
    }

}
