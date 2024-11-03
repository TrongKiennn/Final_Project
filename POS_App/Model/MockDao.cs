using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class MockDao : IDao
{
    public List<Employee> GetEmployees()
    {
        var result = new List<Employee>() {
                new() {
                    ID = 1,
                    Name = "John Algabi",

                },
                new() {
                    ID = 2,
                    Name = "Jackie Bloude",

                },
                new() {
                    ID = 3,
                    Name = "Jason Claude",

                },
            };

        return result;
    }

    public List<User> GetUsers()
    {
        var result = new List<User>()
        {
        };
        return result;
    }
}