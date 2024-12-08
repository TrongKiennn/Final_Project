using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class Order : INotifyPropertyChanged
{
    private Drinks _drinks;
    public Drinks Drinks
    {
        get => _drinks;
        set
        {
            if (_drinks != value)
            {
                _drinks = value;
                OnPropertyChanged(nameof(Drinks));
            }
        }
    }

    private OrderDetail _orderDetail;
    public OrderDetail OrderDetail
    {
        get => _orderDetail;
        set
        {
            if (_orderDetail != value)
            {
                _orderDetail = value;
                OnPropertyChanged(nameof(OrderDetail));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}


