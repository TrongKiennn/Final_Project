using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DemoListBinding1610.IDao;

namespace DemoListBinding1610;
public class MockDao : IDao
{
    public Tuple<List<Employee>, int> GetEmployees(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    )
    {
        var employees = new List<Employee>() {
                new() {
                    ID = 3,
                    Name = "John Algabi",
                    Avatar = "/Assets/avatar01.jpg"
                },
                new() {
                    ID = 2,
                    Name = "Jackie Bloude",
                    Avatar = "/Assets/avatar02.jpg"
                },
                new() {
                    ID = 1,
                    Name = "Jason Claude",
                    Avatar = "/Assets/avatar03.jpg",
                },
                new() {
                    ID = 4,
                    Name = "Jason Claude  4",
                    Avatar = "/Assets/avatar04.jpg",
                },
                 new() {
                    ID = 6,
                    Name = "Jason Claude 5",
                    Avatar = "/Assets/avatar05.jpg",
                },
                 new() {
                    ID = 5,
                    Name = "John Algabi",
                    Avatar = "/Assets/avatar06.jpg"
                },
                new() {
                    ID = 8,
                    Name = "Jackie Bloude",
                    Avatar = "/Assets/avatar08.jpg"
                },
                new() {
                    ID = 9,
                    Name = "Jason Claude",
                    Avatar = "/Assets/avatar09.jpg",
                },
                new() {
                    ID = 13,
                    Name = "Jason Claude  4",
                    Avatar = "/Assets/avatar10.jpg",
                },
                 new() {
                    ID = 7,
                    Name = "Jason Claude 5",
                    Avatar = "/Assets/avatar07.jpg",
                },
                 new() {
                    ID = 11,
                    Name = "John Algabi 11",
                    Avatar = "/Assets/avatar11.jpg"
                },
                new() {
                    ID = 12,
                    Name = "Jackie Bloude 12",
                    Avatar = "/Assets/avatar12.jpg"
                },
                new() {
                    ID = 10,
                    Name = "Jason Claude 13",
                    Avatar = "/Assets/avatar13.jpg",
                },
                new() {
                    ID = 14,
                    Name = "Jason Claude  14",
                    Avatar = "/Assets/avatar14.jpg",
                },
                 new() {
                    ID = 15,
                    Name = "Jason Claude 15",
                    Avatar = "/Assets/avatar15.jpg",
                },
                 new() {
                    ID = 16,
                    Name = "John Algabi 16",
                    Avatar = "/Assets/avatar16.jpg"
                },
                new() {
                    ID = 18,
                    Name = "Jackie Bloude 18",
                    Avatar = "/Assets/avatar18.jpg"
                },
                new() {
                    ID = 19,
                    Name = "Jason Claude 19",
                    Avatar = "/Assets/avatar19.jpg",
                },
                new() {
                    ID = 20,
                    Name = "Jason Claude  20",
                    Avatar = "/Assets/avatar20.jpg",
                },
                 new() {
                    ID = 17,
                    Name = "Jason Claude 17",
                    Avatar = "/Assets/avatar17.jpg",
                },
                 new() {
                    ID = 21,
                    Name = "Jason Claude  21",
                    Avatar = "/Assets/avatar07.jpg",
                },
                 new() {
                    ID = 22,
                    Name = "Jason Claude 22",
                    Avatar = "/Assets/avatar18.jpg",
                },
            };
        // Search
        var query = from e in employees
                    where e.Name.ToLower().Contains(keyword.ToLower())
                    select e;  

        //// Filter
        //int min = 3;
        //int max = 15;
        //query = query.Where(item => item.ID > min && item.ID < max);

         // Sort
        foreach(var option in sortOptions) {
            if (option.Key == "ID") {
                if (option.Value == SortType.Ascending) {
                    query = query.OrderBy(e => e.ID);
                } else {
                    query = query.OrderByDescending(e => e.ID);
                }
            }
        }

        var result = query
            .Skip((page  - 1 )*rowsPerPage)
            .Take( rowsPerPage );
            
        return new Tuple<List<Employee>, int> (
            result.ToList(),
            query.Count()
        );
    }

    public bool DeleteEmployee(int id) {
        return true;
    }

    public bool AddEmployee(Employee info) {
        return true;
    }

    public bool UpdateEmployee(Employee info) {
        return true;
    }
}

