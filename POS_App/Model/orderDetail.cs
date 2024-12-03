using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;
public class OrderDetail : INotifyPropertyChanged
{
    private bool _sugar100;
    private bool _sugar50;
    private bool _noIce;
    private bool _separateIce;
    private bool _shareIce;
    private bool _littleIce;
    private bool _takeAway;
    private bool _stayHere;
    private string _note;

    public bool Sugar100
    {
        get => _sugar100;
        set
        {
            _sugar100 = value;
            OnPropertyChanged(nameof(Sugar100));
        }
    }

    public bool Sugar50
    {
        get => _sugar50;
        set
        {
            _sugar50 = value;
            OnPropertyChanged(nameof(Sugar50));
        }
    }

    public bool NoIce
    {
        get => _noIce;
        set
        {
            _noIce = value;
            OnPropertyChanged(nameof(NoIce));
        }
    }

    public bool SeparateIce
    {
        get => _separateIce;
        set
        {
            _separateIce = value;
            OnPropertyChanged(nameof(SeparateIce));
        }
    }

    public bool ShareIce
    {
        get => _shareIce;
        set
        {
            _shareIce = value;
            OnPropertyChanged(nameof(ShareIce));
        }
    }

    public bool LittleIce
    {
        get => _littleIce;
        set
        {
            _littleIce = value;
            OnPropertyChanged(nameof(LittleIce));
        }
    }

    public bool TakeAway
    {
        get => _takeAway;
        set
        {
            _takeAway = value;
            OnPropertyChanged(nameof(TakeAway));
        }
    }

    public bool StayHere
    {
        get => _stayHere;
        set
        {
            _stayHere = value;
            OnPropertyChanged(nameof(StayHere));
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

