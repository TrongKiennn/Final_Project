using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_App.Service.DataAccess.IDao_Events;
namespace POS_App.Service.DataAccess;

public class Dao_Events : IDao_Events
{
    public async Task<List<Event>> GetEventsWithinTimeFrameAsync(DateTime from, DateTime to)
    {
        var result = new List<Event>();
        var connectionString = GetConnectionString();
        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();


        var sql = @"
            SELECT * 
            FROM events 
            WHERE status ='upcoming' 
            AND date BETWEEN @from AND @to";

        var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@from", from);
        command.Parameters.AddWithValue("@to", to);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                
                DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("date"));
                Event Event = new Model.Event
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString("customer_name"),
                    PhoneNumber = reader.GetString("phone_number"),
                    Email = reader.GetString("email"),
                    TableNumber = reader.GetInt32("table_number"),
                    Note = reader.GetString("note"),
                    Date = dateTime.ToString("yyyy-MM-dd"),
                    Time = dateTime.ToString("HH:mm:ss")
                };
               
                result.Add(Event);
            }

           
        }

        connection.Close();

        return result;
    }
    public async Task UpdateEventStatus(int eventId, string status)
    {
        using (var connection = new MySqlConnection(GetConnectionString()))
        {
            connection.Open();

            var query = "UPDATE events SET status = @status WHERE id = @eventId";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@eventId", eventId);
               
                command.ExecuteNonQuery();
            }
        }
    }

    //public async Task UpdateTableStatusAsync(Table table)
    //{
    //    using (var connection = new MySqlConnection(GetConnectionString()))
    //    {
    //        await connection.OpenAsync();
    //        string query = "UPDATE tables SET status = @status WHERE id = @id";

    //        using (var command = new MySqlCommand(query, connection))
    //        {
    //            command.Parameters.AddWithValue("@status", table.status);
    //            command.Parameters.AddWithValue("@id", table.id);
    //            await command.ExecuteNonQueryAsync();
    //        }
    //    }
    //}

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