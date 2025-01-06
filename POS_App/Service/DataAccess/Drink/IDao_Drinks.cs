using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Drinks
{
    public enum SortType
    {
        Ascending,
        Descending
    }
    Tuple<List<Drinks>, int> GetDrink(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions,
         string typeName = null
    );

    List<Drinks> GetDrinkWithFilter(string searchTest);

    Drinks GetDrinkByName(string drinkName);
    public void UpdateDrinkStatus(int drinkId, string status);
    public void SaveDrink(Drinks drink);
    public void UpdateDrink(Drinks drink);
    public void DeleteDrink(int drinkId);
}


