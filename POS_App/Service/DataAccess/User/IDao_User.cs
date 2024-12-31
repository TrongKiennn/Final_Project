using MySql.Data.MySqlClient;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace POS_App.Service.DataAccess;

public interface IDao_User
{
    User FindUserByEmail(string email);
    int CreateUser(User user);
    void DeleteUserById(int id);
}

