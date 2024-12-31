using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Customer
{

    Tuple<List<Model.Customer>,int, int, int> GetAllCustomer();
    List<Model.Customer> GetCustomerByFilter(string searchText);
    public void CreateCustomer(Model.Customer customer);
    public void UpdateCustomerPoint(string phoneNumber, int point);
    public void UpdateCustomerInfo(Model.Customer customer, string prePhoneNumber);

    public Model.Customer FindCustomerByPhone(string phoneNumber);

}


