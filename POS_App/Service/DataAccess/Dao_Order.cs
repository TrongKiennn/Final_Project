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
}
