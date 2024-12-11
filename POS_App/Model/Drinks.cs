using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class Drinks : INotifyPropertyChanged
{
    public int id { get; set; }
    public string name { get; set; }
    public decimal price { get; set; }
    public string imageUrl { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

}

