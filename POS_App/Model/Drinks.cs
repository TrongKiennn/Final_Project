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
    public string drinkType { get; set; }
    private string _status { get; set; }

    public string status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged(nameof(status));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

