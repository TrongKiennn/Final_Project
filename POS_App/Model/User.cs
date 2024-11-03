using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;
public class User : INotifyPropertyChanged
{
    public int Id { get; set; }

    public string Email { get; set; }
    public string PassWord { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Salt {  get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

}