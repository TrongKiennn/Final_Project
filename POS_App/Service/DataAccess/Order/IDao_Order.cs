using MySql.Data.MySqlClient;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace POS_App.Service.DataAccess;

public interface IDao_Order
{
    Tuple<decimal, decimal, decimal, int, int> GetTotalAmountAndOrdersByDate(DateTimeOffset selectedDay);
    public int CreateOrder(Order info, MySqlTransaction transaction){
        return -1;
    }
    public int GetTableId()
    {
        return -1;
    }
}

