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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public class Dao_Employee_Attendance : IDao_Employee_Attendance
{

    public Tuple<int, decimal> GetEmployeeAttendence(int user_id)
    {
        int total_working_days = 0;
        decimal total_working_hours = 0;
        string connectionString = GetConnectionString();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string sql = @"
                SELECT 
                    COUNT(DISTINCT date) AS total_working_days,
                    COALESCE(SUM(total_hours), 0) AS total_working_hours
                FROM 
                    employeeAttendances
                WHERE 
                    user_id = @UserId
                    AND MONTH(date) = MONTH(CURRENT_DATE) 
                    AND YEAR(date) = YEAR(CURRENT_DATE);";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@UserId", user_id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            total_working_days = reader.GetInt32("total_working_days");
                            total_working_hours = reader.GetDecimal("total_working_hours");
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

        return new Tuple<int, decimal>(total_working_days, total_working_hours);
    }

    public employeeAttendance FindEmployeeAttendanceByUser_IdToday(int user_id)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "SELECT * FROM employeeAttendances WHERE DATE(date) = CURDATE() AND user_id = @user_id";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", user_id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new employeeAttendance
                            {
                                Id =  reader.GetInt32("id"),
                                User_id = reader.GetInt32("user_id"),
                                CheckInTime = reader.IsDBNull(reader.GetOrdinal("check_in_time")) ? (TimeSpan?)null : reader.GetTimeSpan("check_in_time"),
                                CheckOutTime = reader.IsDBNull(reader.GetOrdinal("check_out_time")) ? (TimeSpan?)null : reader.GetTimeSpan("check_out_time")
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
    public void CreateEmployeeAttendance(int user_id)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "INSERT INTO employeeAttendances (date, user_id) VALUES (NOW(), @user_id)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@user_id", user_id);

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

    public void UpdateCheckInTime(int user_id)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();
                var currentTime = DateTime.Now.TimeOfDay;

                var query = "UPDATE employeeAttendances SET check_in_time = @check_in_time WHERE user_id = @user_id and DATE(date)=CURRENT_DATE";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@check_in_time", currentTime);

                    command.Parameters.AddWithValue("@user_id", user_id);

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



    public void UpdateCheckOutTime(int user_id)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();
                var currentTime = DateTime.Now.TimeOfDay;
                var query = @"
                UPDATE employeeAttendances 
                SET 
                    check_out_time = @check_out_time, 
                    total_hours = TIMESTAMPDIFF(SECOND, check_in_time, check_out_time) / 3600.0
                WHERE user_id = @user_id and DATE(date)=CURRENT_DATE";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@check_out_time", currentTime);

                    command.Parameters.AddWithValue("@user_id", user_id);

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


