using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class IngredientOfDrinkRecipe : INotifyPropertyChanged
{
    private int _ingredientId;
    private int _drinkId=0;
    private int _quantity;
    private string _name;
    private string _unit;

    public int IngredientId
    {
        get => _ingredientId;
        set
        {
            _ingredientId = value;
            OnPropertyChanged(nameof(IngredientId));
        }
    }
    public int DrinkId
    {
        get => _drinkId;
        set
        {
            _drinkId = value;
            OnPropertyChanged(nameof(DrinkId));
        }
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            _quantity = value;
            OnPropertyChanged(nameof(Quantity));
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }

    public string Unit
    {
        get => _unit;
        set
        {
            _unit = value;
            OnPropertyChanged(nameof(Unit));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

