using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class Order : INotifyPropertyChanged
{
    private int _id { get; set; }
    public int id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged(nameof(id));
            }
        }
    }
    private int user_id { get; set; }
    private int customerId { get; set; }

    private decimal _subtotal;
    public decimal Subtotal
    {
        get => _subtotal;
        set
        {
            if (_subtotal != value)
            {
                _subtotal = value;
                OnPropertyChanged(nameof(Subtotal));
                OnPropertyChanged(nameof(TotalSalesTax));
                OnPropertyChanged(nameof(Total));
            }
        }
    }

    public int _user_id
    {
        get => user_id;
        set
        {
            if (user_id != value)
            {
                user_id = value;
                OnPropertyChanged(nameof(_user_id)); 
                }

        }
    }

    public decimal TotalSalesTax => Subtotal * 0.1m;

    public decimal Total => Subtotal + TotalSalesTax;

    public decimal CusPayment => Total;

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


