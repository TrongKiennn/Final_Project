using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Model;

public class employeeInfo : INotifyPropertyChanged
{
    private int _id;
    private int _user_id;
    private string _fullname;
    private DateTimeOffset _dateOfBirth;
    private string _phoneNumber;
    private string _gender;
    private string _position;

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

    public int User_Id
    {
        get => _user_id;
        set
        {
            if (_user_id != value)
            {
                _user_id = value;
                OnPropertyChanged(nameof(User_Id));
            }
        }
    }
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
    public DateTimeOffset DateOfBirth
    {
        get => _dateOfBirth;
        set
        {
            if (_dateOfBirth != value)
            {
                _dateOfBirth = value;
                OnPropertyChanged(nameof(DateOfBirth));
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
    
    public string Gender
    {
        get => _gender;
        set
        {
            if (_gender != value)
            {
                _gender = value;
                OnPropertyChanged(nameof(Gender));
            }
        }
    }
    public string Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
