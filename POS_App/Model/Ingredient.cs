using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class Ingredient: INotifyPropertyChanged
{
    private int _ingredient_id { get; set; }
    private string _name { get; set; }
    private int _stock { get; set; }
    private string _status { get; set; }
    private string _unit { get; set; }
    public int ingredient_id
    {
        get => _ingredient_id;
        set
        {
            if (_ingredient_id != value)
            {
                _ingredient_id = value;
                OnPropertyChanged(nameof(ingredient_id));
            }
        }
    }

    public string name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(name));
            }
        }
    }

    public int stock
    {
        get => _stock;
        set
        {
            if (_stock != value)
            {
                _stock = value;
                OnPropertyChanged(nameof(stock));
            }
        }
    }

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

    public string unit
    {
        get => _unit;
        set
        {
            if (_unit != value)
            {
                _unit = value;
                OnPropertyChanged(nameof(unit));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
