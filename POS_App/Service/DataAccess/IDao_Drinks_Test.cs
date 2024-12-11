using POS_App.Model;
using System;
using System.Collections.Generic;
using static POS_App.Service.DataAccess.IDao;

namespace POS_App.Service.DataAccess
{
    public interface IDao_Drinks_Test
    {
        Tuple<List<Drinks>, int> GetDrink(
            int page, int rowsPerPage,
            string keyword,
            Dictionary<string, SortType> sortOptions,
            string typeName = null);
    }
}
