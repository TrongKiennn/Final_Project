using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public interface IDao
{
    List<Employee> GetEmployees();
    List<User> GetUsers();
}

