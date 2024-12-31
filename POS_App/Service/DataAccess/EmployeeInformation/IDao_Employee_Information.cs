using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Employee_Information
{

    Tuple<List<employeeInfo>,int> GetEmployees();

    List<employeeInfo> GetEmployeeByFilter(string SearchText);
    employeeInfo GetEmployeeById(int employeeId);

    //employeeInfo GetEmployeeByName(string EmployeeName);

    public void CreateEmployeeInfo(employeeInfo employeeInfo);

}
