using MySql.Data.MySqlClient;
using POS_App.Model;
using POS_App.Service;
using POS_App.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_App.Service.DataAccess.IDao_Order;
using static POS_App.Service.DataAccess.IDao_Order_Item;
using System.Collections.ObjectModel;
public class OrderService
{
    IDao_Order _Order_Dao;
    IDao_Order_Item _Order_Item_Dao;

    public OrderService(IDao_Order daoOrder, IDao_Order_Item daoOrderItem)
    {
        _Order_Dao = ServiceFactory.GetChildOf(typeof(IDao_Order)) as IDao_Order;
        _Order_Item_Dao = ServiceFactory.GetChildOf(typeof(IDao_Order_Item)) as IDao_Order_Item;
    }

    public int CreateOrderWithItems(Order order, ObservableCollection<orderItem>orderItems)
    {
        string connectionString = GetConnectionString();

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            using (MySqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    // 1. Tạo Order và lấy orderId
                    int orderId = _Order_Dao.CreateOrder(order, transaction);
                    if (orderId <= 0)
                        throw new Exception("Failed to create order.");

                    // 2. Tạo các OrderItems với orderId
                    foreach (var item in orderItems)
                    {
                        if (!_Order_Item_Dao.CreateOrderItem(orderId, item, transaction))
                            throw new Exception("Failed to create order item.");
                    }

                    // 3. Commit transaction
                    transaction.Commit();
                    return orderId;
                }
                catch
                {
                    // Rollback transaction nếu có lỗi
                    transaction.Rollback();
                    throw;
                }
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