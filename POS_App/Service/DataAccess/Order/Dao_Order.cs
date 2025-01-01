using Castle.Components.DictionaryAdapter.Xml;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using MySqlX.XDevAPI.Common;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_App.Service.DataAccess.IDao_Order;

namespace POS_App.Service.DataAccess;

public class Dao_Order : IDao_Order
{
    public Tuple<decimal, decimal, decimal, int, int> GetTotalAmountAndOrdersByDate(DateTimeOffset selectedDay)
    {
        decimal totalByDay = 0;
        decimal totalByMonth = 0;
        decimal totalByYear = 0;
        int totalOrdersByDay = 0;
        int totalOrdersByMonth = 0;
        

        string connectionString = GetConnectionString();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = @"
            SELECT 
                (SELECT IFNULL(SUM(total_amount), 0) FROM orders WHERE DATE(created_at) = @SelectedDate) AS TotalByDay,
                (SELECT IFNULL(SUM(total_amount), 0) FROM orders WHERE YEAR(created_at) = @SelectedYear AND MONTH(created_at) = @SelectedMonth) AS TotalByMonth,
                (SELECT IFNULL(SUM(total_amount), 0) FROM orders WHERE YEAR(created_at) = @SelectedYear) AS TotalByYear,
                (SELECT IFNULL(COUNT(*), 0) FROM orders WHERE DATE(created_at) = @SelectedDate) AS TotalOrdersByDay,
                (SELECT IFNULL(COUNT(*), 0) FROM orders WHERE YEAR(created_at) = @SelectedYear AND MONTH(created_at) = @SelectedMonth) AS TotalOrdersByMonth
            ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SelectedDate", selectedDay.Date);
                    command.Parameters.AddWithValue("@SelectedYear", selectedDay.Year);
                    command.Parameters.AddWithValue("@SelectedMonth", selectedDay.Month);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalByDay = reader.GetDecimal(0);
                            totalByMonth = reader.GetDecimal(1);
                            totalByYear = reader.GetDecimal(2);
                            totalOrdersByDay = reader.GetInt32(3);
                            totalOrdersByMonth = reader.GetInt32(4);
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

        return new Tuple<decimal, decimal, decimal, int, int>(totalByDay, totalByMonth, totalByYear, totalOrdersByDay, totalOrdersByMonth);
    }


    public int CreateOrder(Order info, MySqlTransaction transaction)
    {
        try
        {
            string query = @"
                INSERT INTO orders (total_amount, status, user_id) 
                VALUES (@total_amount, @status, @user_id);
                SELECT LAST_INSERT_ID();";

            using (MySqlCommand cmd = new MySqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@total_amount", info.Total);
                cmd.Parameters.AddWithValue("@status", "completed");
                cmd.Parameters.AddWithValue("@user_id", info._user_id);

                object result = cmd.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int orderId))
                {
                    
                    return orderId;
                }
            }
        }
        catch
        {
            throw;
        }

        return -1;
    }
    public int GetTableId()
    {
        try
        {
            var connectionString = GetConnectionString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"SELECT id FROM tableManager WHERE status = 'available' LIMIT 1;";
                MySqlCommand command = new MySqlCommand(query, connection);

                connection.Open();  
                var result = command.ExecuteScalar();  

                if (result != null)
                {
                    return Convert.ToInt32(result);  
                }
            }
        }
        catch
        {
            throw;
        }

        return -1;  
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
