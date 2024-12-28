using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_App.Service.DataAccess.IDao_Tables;
namespace POS_App.Service.DataAccess;

public class Dao_Tables : IDao_Tables
{
    public Tuple<List<Table>, int> GetTable()
    {
        var result = new List<Table>();
        var connectionString = GetConnectionString();
        MySqlConnection connection = new MySqlConnection(connectionString);
       
        connection.Open();


        var sql = @"SELECT id, status, order_id FROM tableManager;";

        var command = new MySqlCommand(sql, connection);

        int count = -1;

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var table = new Table
                {
                    id = reader.GetInt32("id"),
                    status = reader.GetString("status")
                };

                if (table.status == "reserved")
                {
                    table.order_id = reader.GetInt32("order_id");
                }
                result.Add(table);
            }

            if (reader.NextResult() && reader.Read())
            {
                count = reader.GetInt32("Total");
            }
        }

        connection.Close();

        return new Tuple<List<Table>, int>(result, count);
    }
    public void UpdateTableStatus(int tableId, string status,int orderId)
    {
        using (var connection = new MySqlConnection(GetConnectionString()))
        {
            connection.Open();

            var query = "UPDATE tableManager SET status = @status, order_id=@order_id WHERE id = @tableId";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@tableId", tableId);
                command.Parameters.AddWithValue("@order_id", orderId);
                command.ExecuteNonQuery();
            }
        }
    }

    public async Task UpdateTableStatusAsync(Table table)
    {
        using (var connection = new MySqlConnection(GetConnectionString()))
        {
            await connection.OpenAsync();
            string query = "UPDATE tableManager SET status = @status, order_id=@order_id WHERE id = @id";

            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@status", table.status);
                command.Parameters.AddWithValue("@id", table.id);
                if (table.order_id == 0)
                    command.Parameters.AddWithValue("@order_id", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@order_id", table.order_id);
                await command.ExecuteNonQueryAsync();
            }
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