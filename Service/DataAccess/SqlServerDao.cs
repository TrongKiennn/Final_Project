using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using static DemoListBinding1610.IDao;

namespace DemoListBinding1610;
public class SqlServerDao : IDao
{
    public Tuple<List<Employee>, int> GetEmployees(
        int page, int rowsPerPage,
        string keyword,
        Dictionary<string, SortType> sortOptions
    ) {
        var result = new List<Employee>();
        var connectionString = GetConnectionString();
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();

        string sortString = "ORDER BY ";
        bool useDefault = true;
        foreach (var item in sortOptions) {
            useDefault = false;
            if (item.Key == "Name") {
                if (item.Value == SortType.Ascending) {
                    sortString += "Name asc ";
                } else {
                    sortString += "Name desc ";
                }
            }
        }
        if (useDefault) {
            sortString += "ID ";
        }

        var sql = $"""
            SELECT count(*) over() as Total, ID, Name, Avatar, Tel
            FROM Employee
            WHERE Name like @Keyword
            {sortString} 
            OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;        
        """;

        var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@Skip", SqlDbType.Int)
            .Value = (page - 1) * rowsPerPage;
        command.Parameters.Add("@Take", SqlDbType.Int)
            .Value = rowsPerPage;
        command.Parameters.Add("@Keyword", SqlDbType.NText)
            .Value = $"%{keyword}%";
        var reader = command.ExecuteReader();
        int count = -1;

        while (reader.Read()) {
            if (count == -1) {
                count = (int)reader["Total"];
            }

            var employee = new Employee(); // Relation -> Objects

            employee.ID = (int)reader["ID"];
            employee.Name = (string)reader["Name"];
            employee.Avatar = reader.IsDBNull("Avatar")
                ? "/Assets/avatar01.jpg"
                :(string) reader["Avatar"];
            employee.Telephone = (string)reader["Tel"];
            result.Add(employee);
        }

        connection.Close();
        return new Tuple<List<Employee>, int>(
            result, count
        );
    }

    private static string GetConnectionString() {
        var connectionString = """
            Server = localhost;
            Database = supershop;
            User Id = sa;
            Password = SqlServer@123;
            TrustServerCertificate = True;
        """;
        
        return connectionString;
    }

    public bool DeleteEmployee(int id) {
        var connectionString = GetConnectionString();
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        var sql = "delete from Employee where id=@ID";
        var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@ID", SqlDbType.Int)
            .Value = id;

        int count = command.ExecuteNonQuery();
        bool success = count == 1;

        connection.Close();
        return success;
    }

    public bool AddEmployee(Employee info) {
        var connectionString = GetConnectionString();
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        var sql = "insert into Employee(name, tel) values(@name, @tel)";
        var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@name", SqlDbType.NText)
            .Value = info.Name;
        command.Parameters.Add("@tel", SqlDbType.NVarChar)
            .Value = info.Telephone;

        int count = command.ExecuteNonQuery();
        bool success = count == 1;

        connection.Close();
        return success;
    }

    public bool UpdateEmployee(Employee info) {
        var connectionString = GetConnectionString();
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        var sql = "update Employee set name=@name, tel=@tel where id=@id";
        var command = new SqlCommand(sql, connection);
        command.Parameters.Add("@name", SqlDbType.NText)
            .Value = info.Name;
        command.Parameters.Add("@tel", SqlDbType.NVarChar)
            .Value = info.Telephone;
        command.Parameters.Add("@id", SqlDbType.Int)
            .Value = info.ID;

        int count = command.ExecuteNonQuery();
        bool success = count == 1;

        connection.Close();
        return success;
    }
}
