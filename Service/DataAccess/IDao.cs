using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoListBinding1610;

public interface IDao
{
    public enum SortType {
        Ascending,
        Descending
    }
    Tuple<List<Employee>, int> GetEmployees(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    );

    bool DeleteEmployee(int id);
    bool AddEmployee(Employee info);

    bool UpdateEmployee(Employee info);
}

