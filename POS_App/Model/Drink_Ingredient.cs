using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class Drink_Ingredient : INotifyPropertyChanged
{
    private int _drink_id { get; set; }
    private int _ingredient_id { get; set; }
    private int _quantity { get; set; }

    public int drink_id
    {
        get => _drink_id;
        set
        {
            if (_drink_id != value)
            {
                _drink_id = value;
                OnPropertyChanged(nameof(drink_id));
            }
        }
    }

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

    public int quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged(nameof(quantity));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
