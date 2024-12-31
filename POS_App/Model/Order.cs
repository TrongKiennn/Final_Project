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
    private int user_id { get; set; }
    private int customerId { get; set; }

    private decimal _subtotal;
    public decimal TotalSalesTax => Subtotal * 0.1m;

    public decimal Total => Subtotal + TotalSalesTax- DiscountSalesTax;

    private decimal _discountSalesTax=0;
    public decimal DiscountSalesTax
    {
        get => _discountSalesTax;
        set
        {
            if (_discountSalesTax != value)
            {
                _discountSalesTax = value;
                OnPropertyChanged(nameof(DiscountSalesTax));
            }
        }
    }


    public decimal CusPayment
    {
        get { return Total; }
    }

    private string _locationOptions = "Take Away";
    private int _table_id { get; set; }
   

    public int table_id
    {
        get => _table_id;
        set
        {
            if (_table_id != value)
            {
                _table_id = value;
                OnPropertyChanged(nameof(table_id));
            }
        }
    }
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
    public decimal Subtotal
    {
        get => _subtotal;
        set
        {
            if (_subtotal != value)
            {
                _subtotal = value;
                OnPropertyChanged(nameof(Subtotal));
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
    public string LocationOptions
    {
        get => _locationOptions;
        set
        {
            _locationOptions = value;
            OnPropertyChanged(nameof(LocationOptions));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


