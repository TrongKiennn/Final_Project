using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class Rank : INotifyPropertyChanged
{
    private string _rank;
    private int _point;
    private decimal _discount;
    private decimal _pointPerDollar;

    public string RankName
    {
        get => _rank;
        set
        {
            if (_rank != value)
            {
                _rank = value;
                OnPropertyChanged(nameof(RankName));
            }
        }
    }

    public int Point
    {
        get => _point;
        set
        {
            if (_point != value)
            {
                _point = value;
                OnPropertyChanged(nameof(Point));
            }
        }
    }
    public decimal Discount
    {
        get => _discount;
        set
        {
            if (_discount != value)
            {
                _discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }
    }
    public decimal PointPerDollar
    {
        get => _pointPerDollar;
        set
        {
            if (_pointPerDollar != value)
            {
                _pointPerDollar = value;
                OnPropertyChanged(nameof(PointPerDollar));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
