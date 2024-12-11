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


}

