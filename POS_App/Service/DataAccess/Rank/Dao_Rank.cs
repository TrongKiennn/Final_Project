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

public class Dao_Rank : IDao_Rank
{


    public Model.Rank GetRank(string rankName)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();

                var query = "SELECT * FROM ranks WHERE  customer_rank = @rankName";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@rankName", rankName);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        { 
                            return new Rank
                            {
                                RankName = reader.GetString("customer_rank"),
                                Point = reader.GetInt32("rank_point"),
                                Discount = reader.GetDecimal("rank_discount"),
                                PointPerDollar = reader.GetDecimal("point_per_dollar")
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

    public void UpdateRankSetting(Model.Rank rank, decimal pointPerDollar)
    {
        try
        {
            using (var connection = new MySqlConnection(GetConnectionString()))
            {
                connection.Open();
                var query = @"
                UPDATE ranks
                SET 
                    rank_point=@rankPoint,
                    rank_discount=@rankDiscount,
                    point_per_dollar=@pointPerDollar
                WHERE customer_rank = @customerRank;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@rankPoint", rank.Point);
                    command.Parameters.AddWithValue("@rankDiscount", rank.Discount);
                    command.Parameters.AddWithValue("@pointPerDollar", pointPerDollar);
                    command.Parameters.AddWithValue("@customerRank", rank.RankName);
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


