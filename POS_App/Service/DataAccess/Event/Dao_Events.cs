using MySql.Data.MySqlClient;
using Mysqlx.Session;
using MySqlX.XDevAPI.Common;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

    public List<Event> GetAllEvent()
    {
        var result = new List<Event>();
        var connectionString = GetConnectionString();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var updateSql = "UPDATE events SET status = 'cancel' WHERE DATE(date) < CURDATE();";
            using (var updateCommand = new MySqlCommand(updateSql, connection))
            {
                updateCommand.ExecuteNonQuery();
            }

            var selectSql = "SELECT * FROM events WHERE status NOT IN ('cancel', 'complete') ORDER BY date ASC;";
            using (var selectCommand = new MySqlCommand(selectSql, connection))
            using (var reader = selectCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("date"));
                    Event Event = new Event
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
        }

        return result;
    }

    public void AddEvent(Event Event)
    {
        try
        {
            string dateTimeString = $"{Event.Date} {Event.Time}";
            DateTime eventDateTime = DateTime.ParseExact(dateTimeString, "yyyy-M-d H:mm:ss", CultureInfo.InvariantCulture);
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO events (event_name,customer_name,phone_number ,email ,date,table_number,note ) VALUES (@eventName,@customerName, @phoneNumber, @Email, @Date, @tableNumber,@Note)";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@eventName", Event.Type);
                    cmd.Parameters.AddWithValue("@customerName", Event.Name);
                    cmd.Parameters.AddWithValue("@phoneNumber", Event.PhoneNumber);
                    cmd.Parameters.AddWithValue("@Email", Event.Email);
                    cmd.Parameters.AddWithValue("@Date", eventDateTime);
                    cmd.Parameters.AddWithValue("@tableNumber", Event.TableNumber);
                    cmd.Parameters.AddWithValue("@Note", Event.Note);
                    cmd.ExecuteNonQuery();
                }
            }
        }catch(Exception ex)
        {
       
            Console.WriteLine($"Error adding event: {ex.Message}");
            throw;
        }
    }

    public Event GetEventById(int id)
    {
        Event Event = null;
        var connectionString = GetConnectionString();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            var query = "SELECT * FROM events WHERE id = @id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime dateTime = reader.GetDateTime(reader.GetOrdinal("date"));
                        Event = new Event
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
                    }
                }
            }
        }

        return Event;
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