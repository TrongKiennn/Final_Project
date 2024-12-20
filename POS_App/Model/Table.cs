using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class Table : INotifyPropertyChanged
{
    private int _id { get; set; }
    private int _order_id { get; set; }
    private string _status { get; set; }

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

    public int order_id
    {
        get => _order_id;
        set
        {
            if (_order_id != value)
            {
                _order_id = value;
                OnPropertyChanged(nameof(order_id));
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

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
