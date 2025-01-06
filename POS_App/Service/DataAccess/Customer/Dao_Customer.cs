using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.Cms;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public class Dao_Customer : IDao_Customer
{

    public Tuple<List<Model.Customer>, int, int, int> GetAllCustomer()
    {
        List<Model.Customer> customers = new List<Model.Customer>();
        int totalBronze = 0;
        int totalSilver = 0;
        int totalGold = 0;
        string connectionString = GetConnectionString();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string customersSql = "SELECT fullname, phoneNumber, email, point, customer_rank FROM customers";
                using (MySqlCommand command = new MySqlCommand(customersSql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Model.Customer
                            {
                                FullName = reader.GetString("fullname"),
                                PhoneNumber = reader.GetString("phoneNumber"),
                                Email = reader.GetString("email"),
                                Point = reader.GetInt32("point"),
                                Rank = reader.GetString("customer_rank")
                            });
                        }
                    }
                }

                string countSql = @"
                SELECT COUNT(*) FROM customers WHERE customer_rank='Bronze';
                SELECT COUNT(*) FROM customers WHERE customer_rank='Silver';
                SELECT COUNT(*) FROM customers WHERE customer_rank='Gold';
            ";
                using (MySqlCommand countCommand = new MySqlCommand(countSql, connection))
                {
                    using (var countReader = countCommand.ExecuteReader())
                    {
                        if (countReader.Read()) totalBronze = countReader.GetInt32(0);
                        if (countReader.NextResult() && countReader.Read()) totalSilver = countReader.GetInt32(0);
                        if (countReader.NextResult() && countReader.Read()) totalGold = countReader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        return new Tuple<List<Model.Customer>, int, int, int>(customers, totalBronze, totalSilver, totalGold);
    }

    public List<Model.Customer> GetCustomerByFilter(string searchText)
    {
        List<Model.Customer> customers = new List<Model.Customer>();
     
        string connectionString = GetConnectionString();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string customersSql = "SELECT fullname, phoneNumber, email, point, customer_rank FROM customers WHERE fullname LIKE @Keyword";
                using (MySqlCommand command = new MySqlCommand(customersSql, connection))
                {
                    command.Parameters.AddWithValue("@Keyword", "%" + searchText + "%");
                    using (var reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            customers.Add(new Model.Customer
                            {
                                FullName = reader.GetString("fullname"),
                                PhoneNumber = reader.GetString("phoneNumber"),
                                Email = reader.GetString("email"),
                                Point = reader.GetInt32("point"),
                                Rank = reader.GetString("customer_rank")
                            });
                        }
                    }
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        return customers;
    }


    public Model.Customer FindCustomerByPhone(string phoneNumber)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "SELECT * FROM customers WHERE  phoneNumber = @phoneNumber";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                          

                            return new Model.Customer
                            {
                                FullName = reader.GetString("fullname"),
                                PhoneNumber = reader.GetString("phoneNumber"),
                                Email = reader.GetString("email"),
                                Point = reader.GetInt32("point"),
                                RankDiscount = reader.GetDecimal("rank_discount"),
                                PointPerDollar = reader.GetDecimal("point_per_dollar"),
                                
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }
    public void CreateCustomer(Model.Customer customer)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "INSERT INTO customers (fullname, phoneNumber, email, point) VALUES (@fullname,@phoneNumber,@email ,@point)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fullname", customer.FullName);
                    command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@email", customer.Email);
                    command.Parameters.AddWithValue("@point", customer.Point);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }

    public void UpdateCustomerPoint(string phoneNumber,int point)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "UPDATE customers SET point = point+@point WHERE phoneNumber = @phoneNumber";

                Debug.WriteLine(phoneNumber);
                Debug.WriteLine(point);

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    command.Parameters.AddWithValue("@point", point);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
        }
    }



    public void UpdateCustomerInfo(Model.Customer customer, string prePhoneNumber)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();
                var query = @"
                UPDATE customers
                SET 
                    fullname=@fullname,
                    phoneNumber=@phoneNumber,
                    email=@email,
                    point=@point
                WHERE phoneNumber = @prePhoneNumber;" ;

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fullname", customer.FullName);
                    command.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
                    command.Parameters.AddWithValue("@email", customer.Email);
                    command.Parameters.AddWithValue("@point", customer.Point);
                    command.Parameters.AddWithValue("@prePhoneNumber", prePhoneNumber);
                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            throw;
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


