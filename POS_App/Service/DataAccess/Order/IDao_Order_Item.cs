using MySql.Data.MySqlClient;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Order_Item
{
    public bool CreateOrderItem(int orderId, orderItem info, MySqlTransaction transaction)
    {
        return true;
    }
    List<Drinks> GetTopSellingDrinks();
    List<orderItem> GetOrderItemByOrderId(int orderId);
}

