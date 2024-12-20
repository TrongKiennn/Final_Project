using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;
public class OrderDetail : INotifyPropertyChanged
{
    private string _sugarOptions="100% sugar";
    private string _iceOptions="Normal Ice";
    private int _quantity = 1;
    private string _note;

    public string SugarOptions
    {
        get => _sugarOptions;
        set
        {
            _sugarOptions = value;
            OnPropertyChanged(nameof(SugarOptions));
        }
    }

    public string IceOptions
    {
        get => _iceOptions;
        set
        {
            _iceOptions = value;
            OnPropertyChanged(nameof(IceOptions));
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

    public string Note
    {
        get => _note;
        set
        {
            _note = value;
            OnPropertyChanged(nameof(Note));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

