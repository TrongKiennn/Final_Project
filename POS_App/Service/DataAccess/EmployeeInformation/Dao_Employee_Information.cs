using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace POS_App.Service.DataAccess;

public class Dao_Employee_Information : IDao_Employee_Information    
{
    public Tuple<List<employeeInfo>, int> GetEmployees()
    {
        var result = new List<employeeInfo>();
        int count = -1;

        var connectionString = GetConnectionString();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var sql = @"
            SELECT id, user_id, fullname, dateOfBirth, phoneNumber, gender, position 
            FROM employeeInformations 
            ORDER BY id DESC;
            
            SELECT COUNT(*) AS Total 
            FROM employeeInformations;
        ";

            using (var command = new MySqlCommand(sql, connection))
            using (var reader = command.ExecuteReader())
            {
                // Đọc danh sách nhân viên
                while (reader.Read())
                {
                    var employeeInformation = new employeeInfo
                    {
                        Id = reader.GetInt32("id"),
                        User_Id = reader.GetInt32("user_id"),
                        FullName = reader.GetString("fullname"),
                        DateOfBirth = reader.GetDateTime("dateOfBirth"),
                        PhoneNumber = reader.GetString("phoneNumber"),
                        Gender = reader.GetString("gender"),
                        Position = reader.GetString("position")
                    };
                    result.Add(employeeInformation);
                }

              
                if (reader.NextResult() && reader.Read())
                {
                    count = reader.GetInt32("Total");
                }
            }
        }

        return new Tuple<List<employeeInfo>, int>(result, count);
    }

    public List<employeeInfo> GetEmployeeByFilter(string SearchText)
    {
        var result = new List<employeeInfo>();


        var connectionString = GetConnectionString();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var sql = @"
            SELECT id, user_id, fullname, dateOfBirth, phoneNumber, gender, position 
            FROM employeeInformations 
            WHERE fullname LIKE @Keyword
            ORDER BY id DESC;";

            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Keyword", "%" + SearchText + "%");

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employeeInformation = new employeeInfo
                        {
                            Id = reader.GetInt32("id"),
                            User_Id = reader.GetInt32("user_id"),
                            FullName = reader.GetString("fullname"),
                            DateOfBirth = reader.GetDateTime("dateOfBirth"),
                            PhoneNumber = reader.GetString("phoneNumber"),
                            Gender = reader.GetString("gender"),
                            Position = reader.GetString("position")
                        };
                        result.Add(employeeInformation);
                    }
                }
            }

            return result;
        }
    }


    public void CreateEmployeeInfo(employeeInfo employeeInfo)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"INSERT INTO employeeInformations (user_id, fullname, dateOfBirth, phoneNumber, gender, position) VALUES ( @user_id, @fullname, @dateofbirth, @phonenumber, @gender, @position)";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@user_id", employeeInfo.User_Id);
                    command.Parameters.AddWithValue("@fullname", employeeInfo.FullName);
                    command.Parameters.AddWithValue("@dateOfBirth", employeeInfo.DateOfBirth);
                    command.Parameters.AddWithValue("@phoneNumber", employeeInfo.PhoneNumber);
                    command.Parameters.AddWithValue("@gender", employeeInfo.Gender);
                    command.Parameters.AddWithValue("@position", employeeInfo.Position);
                    command.ExecuteNonQuery();
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

    public void UpdateEmployeeInfo(employeeInfo employeeInfo)
    {
       
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"UPDATE employeeInformations 
                        SET fullname = @FullName, 
                            dateOfBirth = @DateOfBirth, 
                            phoneNumber = @PhoneNumber, 
                            gender = @Gender, 
                            position = @Position 
                        WHERE user_id = @User_Id";

                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@FullName", employeeInfo.FullName);
                    command.Parameters.AddWithValue("@DateOfBirth", employeeInfo.DateOfBirth);
                    command.Parameters.AddWithValue("@PhoneNumber", employeeInfo.PhoneNumber);
                    command.Parameters.AddWithValue("@Gender", employeeInfo.Gender);
                    command.Parameters.AddWithValue("@Position", employeeInfo.Position);
                    command.Parameters.AddWithValue("@User_Id", employeeInfo.User_Id);

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

    public employeeInfo GetEmployeeById(int employeeId)
    {
        employeeInfo employeeInfo = new employeeInfo();
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"SELECT user_id, fullname, dateOfBirth, phoneNumber, gender, position FROM employeeInformations WHERE id = @employeeId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeInfo.User_Id = reader.GetInt32("user_id");
                            employeeInfo.FullName = reader.GetString("fullname");
                            employeeInfo.DateOfBirth = reader.GetDateTime("dateOfBirth");
                            employeeInfo.PhoneNumber = reader.GetString("phoneNumber");
                            employeeInfo.Gender = reader.GetString("gender");
                            employeeInfo.Position = reader.GetString("position");
                        }
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

        return employeeInfo;
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