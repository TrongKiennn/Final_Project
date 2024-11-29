using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoListBinding1610;
public class Employee : INotifyPropertyChanged
{
    public int ID { get; set;}
    public string Name { get; set; }
    public string Avatar {get; set;}
    public string Telephone { get; set;}

    public event PropertyChangedEventHandler PropertyChanged;

}
