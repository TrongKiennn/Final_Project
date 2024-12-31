using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class employeeAttendance : INotifyPropertyChanged
{
    private int _id;
    private int _user_id;
    private int _monthlyAttendance;
    private decimal _totalHours;
    private TimeSpan? _checkInTime;
    private TimeSpan? _checkOutTime;
    public int Id
    {
        get => _id;
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    public int User_id
    {
        get => _user_id;
        set
        {
            if (_user_id != value)
            {
                _user_id = value;
                OnPropertyChanged(nameof(User_id));
            }
        }
    }

    public int MonthlyAttendance
    {
        get => _monthlyAttendance;
        set
        {
            if (_monthlyAttendance != value)
            {
                _monthlyAttendance = value;
                OnPropertyChanged(nameof(MonthlyAttendance));
            }
        }
    }

    public decimal TotalHours
    {
        get => _totalHours;
        set
        {
            if (_totalHours != value)
            {
                _totalHours = value;
                OnPropertyChanged(nameof(TotalHours));
            }
        }
    }

    public TimeSpan? CheckInTime
    {
        get => _checkInTime;
        set
        {
            if (_checkInTime != value)
            {
                _checkInTime = value;
                OnPropertyChanged(nameof(CheckInTime));
            }
        }
    }
    public TimeSpan? CheckOutTime
    {
        get => _checkOutTime;
        set
        {
            if (_checkOutTime != value)
            {
                _checkOutTime = value;
                OnPropertyChanged(nameof(CheckOutTime));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
