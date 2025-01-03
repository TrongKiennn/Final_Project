﻿using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.UI.Xaml.Controls;
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
using static POS_App.Service.DataAccess.IDao_Drinks;
namespace POS_App.Service.DataAccess;

public class Dao_Drinks : IDao_Drinks
{
   
    public Tuple<List<Drinks>, int> GetDrink(
    int page, int rowsPerPage,
    string keyword,
    Dictionary<string, SortType> sortOptions,
    string typeName = null // Thêm filter theo type_name
)
{
        var result = new List<Drinks>();
        var connectionString = GetConnectionString();
        MySqlConnection connection = new MySqlConnection(connectionString);
        string sortString = "ORDER BY ";

        connection.Open();
        bool useDefault = true;
        foreach (var item in sortOptions)
        {
            useDefault = false;
            if (item.Key == "drink_name")
            {
                sortString += item.Value == SortType.Ascending ? "drink_name ASC " : "drink_name DESC ";
            }
        }
        if (useDefault)
        {
            sortString += "ID ";
        }

        // Cập nhật truy vấn SQL để lọc theo type_name
        var sql = $"""
            SELECT d.id AS ID, d.drink_name AS Name, d.drink_img_url AS ImageUrl, 
                   d.price AS Price,d.status, t.type_name AS TypeName
            FROM drinks d
            INNER JOIN type_of_drink t ON d.typeId = t.id
            WHERE d.drink_name LIKE @Keyword
            {(string.IsNullOrEmpty(typeName) ? "" : "AND t.type_name = @TypeName")}
            {sortString}
            LIMIT @Skip, @Take;

            SELECT COUNT(*) AS Total
            FROM drinks d
            INNER JOIN type_of_drink t ON d.typeId = t.id
            WHERE d.drink_name LIKE @Keyword
            {(string.IsNullOrEmpty(typeName) ? "" : "AND t.type_name = @TypeName")};
        """;

        var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Skip", (page - 1) * rowsPerPage);
        command.Parameters.AddWithValue("@Take", rowsPerPage);
        command.Parameters.AddWithValue("@Keyword", $"%{keyword}%");
        if (!string.IsNullOrEmpty(typeName))
        {
            command.Parameters.AddWithValue("@TypeName", typeName);
        }

        int count = -1;

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var drink = new Drinks
                {
                    id = reader.GetInt32("ID"),
                    name = reader.GetString("Name"),
                    imageUrl = reader.IsDBNull("ImageUrl") ? "/Assets/default_drink.jpg" : reader.GetString("ImageUrl"),
                    price = reader.GetDecimal("Price"),
                    status = reader.GetString("status")
                };

                result.Add(drink);
            }

            // Chuyển sang tập kết quả thứ hai để đọc tổng số bản ghi
            if (reader.NextResult() && reader.Read())
            {
                count = reader.GetInt32("Total");
            }
        }

        connection.Close();

        return new Tuple<List<Drinks>, int>(result, count);
    }
    public void UpdateDrinkStatus(int drinkId, string status)
    {
        using (var connection = new MySqlConnection(GetConnectionString()))
        {
            connection.Open();

            var query = "UPDATE drinks SET status = @status WHERE id = @drinkId";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@drinkId", drinkId);
                command.ExecuteNonQuery();
            }
        }
    }


    public List<Drinks> GetDrinkWithoutFilter()
    {
        var result = new List<Drinks>();
        var connectionString = GetConnectionString();
        MySqlConnection connection = new MySqlConnection(connectionString);
      

        connection.Open();
       

       
        var sql = $"""
        SELECT d.id AS ID, d.drink_name AS Name, d.drink_img_url AS ImageUrl, 
               d.price AS Price,d.status
        FROM drinks d; 
        """;

        var command = new MySqlCommand(sql, connection);
     
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var drink = new Drinks
                {
                    id = reader.GetInt32("ID"),
                    name = reader.GetString("Name"),
                    imageUrl = reader.IsDBNull("ImageUrl") ? "/Assets/default_drink.jpg" : reader.GetString("ImageUrl"),
                    price = reader.GetDecimal("Price"),
                    status = reader.GetString("status")
                };

                result.Add(drink);
            }
        }

        connection.Close();

        return result;
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


