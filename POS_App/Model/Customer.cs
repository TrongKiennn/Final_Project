using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class Customer : INotifyPropertyChanged
{
    private string _fullname;
    private string _phoneNumber;
    private string _email;
    private int _point=0;
    private string _rank;
    private decimal _rankDiscount;
    private decimal _pointPerDollar;

    public string FullName
    {
        get => _fullname;
        set
        {
            if (_fullname != value)
            {
                _fullname = value;
                OnPropertyChanged(nameof(FullName));
            }
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            if (_phoneNumber != value)
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            if (_email != value)
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
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
    public string Rank
    {
        get => _rank;
        set
        {
            if (_rank != value)
            {
                _rank = value;
                OnPropertyChanged(nameof(Rank));
            }
        }
    }

    public decimal RankDiscount
    {
        get => _rankDiscount;
        set
        {
            if (_rankDiscount != value)
            {
                _rankDiscount = value;
                OnPropertyChanged(nameof(RankDiscount));
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
