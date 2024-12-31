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
using static POS_App.Service.DataAccess.IDao_Order_Item;

namespace POS_App.Service.DataAccess;

public class Dao_Order_Item : IDao_Order_Item
{
    public bool CreateOrderItem(int orderId, orderItem info, MySqlTransaction transaction)
    {
        try
        {
            string query = @"
                INSERT INTO orderItems (order_id, drink_id, quantity, total)
                VALUES (@order_id, @drink_id, @quantity, @total);";

            using (MySqlCommand cmd = new MySqlCommand(query, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@order_id", orderId);
                cmd.Parameters.AddWithValue("@drink_id", info.Drinks.id);
                cmd.Parameters.AddWithValue("@quantity", info.OrderDetail.Quantity);
                cmd.Parameters.AddWithValue("@total", info.TotalPerProduct);

                cmd.ExecuteNonQuery();
                return true;
            }
        }
        catch
        {
            throw;
        }
    }

    public List<orderItem> GetOrderItemByOrderId(int orderId)
    {
        List<orderItem> orderItems = new List<orderItem>();
        Drinks drinkItem = new Drinks();
        int Quantity = 0;
        var connectionString = GetConnectionString();
        MySqlConnection connection = new MySqlConnection(connectionString);
        connection.Open();
        string query = @"
                SELECT d.id AS drink_id, d.drink_name, d.price, d.drink_img_url AS imageUrl,oi.quantity
                FROM orderItems oi
                JOIN drinks d ON oi.drink_id = d.id
                WHERE oi.order_id = @orderId";

        using (var cmd = new MySqlCommand(query, connection))
        {
            cmd.Parameters.AddWithValue("@orderId", orderId);

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    drinkItem= new Drinks
                    {
                        id = reader.GetInt32("drink_id"),
                        name = reader.GetString("drink_name"),
                        price = reader.GetDecimal("price"),
                        imageUrl = reader.GetString("imageUrl"),
                    };
                    Quantity = reader.GetInt32("quantity");
                    orderItems.Add(new orderItem
                    {
                        Drinks = drinkItem,
                        OrderDetail = new OrderDetail
                        {
                            Quantity = Quantity
                        }
                    });
                }
            }
        }

        return orderItems;
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

