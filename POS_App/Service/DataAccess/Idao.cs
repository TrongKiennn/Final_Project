﻿using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao
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

}

