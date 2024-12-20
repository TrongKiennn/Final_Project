using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Tables
{
   
    Tuple<List<Table>, int> GetTable();
    public void UpdateTableStatus(int tableId, string status,int orderId);

    public Task UpdateTableStatusAsync(Table table);

}
