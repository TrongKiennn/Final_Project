using Microsoft.UI.Text;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using POS_App.Command;
using POS_App.Model;
using POS_App.Models;
using POS_App.Service;
using POS_App.Service.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Services.Maps;
using static POS_App.Service.DataAccess.IDao_Drinks;
using static POS_App.Service.DataAccess.IDao_Order;
using static POS_App.Service.DataAccess.IDao_Order_Item;
using MySql.Data.MySqlClient;
using System.Data;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using Windows.Storage;
using System.IO;
using Windows.System;
using System.Diagnostics.Eventing.Reader;

using Windows.Storage.Pickers;

namespace POS_App;

public partial class UpdateMenuViewModel : INotifyPropertyChanged
{
    IDao_Drinks _dao;
  
    public ICommand DrinkClickCommand { get; set; }
    public ICommand ChangImageCommand { get; set; }
    public ICommand UpdateDrinkCommand { get; set; }
    public ICommand SaveDrinkCommand { get; set; }
    public ICommand ConfirmDeleteCommand { get; set; }
    public ICommand AddImageToNewDrink { get; set; }
    public ObservableCollection<Drinks> Drinks { get; set; }
   
    private UploadImageHelper uploadImageHelper = new UploadImageHelper();
    private Drinks _selectedDrink;
    public Drinks SelectedDrink
    {
        get => _selectedDrink;
        set => SetProperty(ref _selectedDrink, value);
    }
   
    private Drinks _newDrink = new Drinks();
    public Drinks NewDrink
    {
        get => _newDrink;
        set => SetProperty(ref _newDrink, value);
    }

    private ErrorHandling _checkDrinkInfo = new ErrorHandling();
    public ErrorHandling CheckDrinkInfo
    {
        get => _checkDrinkInfo;
        set => SetProperty(ref _checkDrinkInfo, value);
    }

    private string _userRole;
    public string UserRole
    {
        get => _userRole;
        set => SetProperty(ref _userRole, value);
    }

    public bool IsAddSuccess = false;

    private string _searchText;
    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
                OnPropertyChanged();
                PerformSearch();
            }
        }
    }
    public UpdateMenuViewModel()
    {
        DrinkClickCommand = new RelayCommand<Drinks>(OnDrinkClick);
        ChangImageCommand = new RelayCommand(_=>OnChangeImage());
        UpdateDrinkCommand = new RelayCommand(_ => UpdateDrink());
        SaveDrinkCommand = new RelayCommand(_ => SaveDrink());
        ConfirmDeleteCommand = new RelayCommand(_ => ConfirmDelete());
        AddImageToNewDrink = new RelayCommand(_ => OnAddImage());

        _dao = ServiceFactory.GetChildOf(typeof(IDao_Drinks)) as IDao_Drinks;
        var localSettings = ApplicationData.Current.LocalSettings;

        if (localSettings.Values.ContainsKey("Role"))
        {
            UserRole = localSettings.Values["Role"] as string;
        }

        LoadData();
    }

    private void OnDrinkClick(Drinks selectedDrink)
    {
        CheckDrinkInfo = new ErrorHandling();
        SelectedDrink = selectedDrink;
    }

    private async void OnAddImage()
    {

        var result = await uploadImageHelper.GetImageUrlFromSupaBase();
        if (result.ErrorMessage != "")
        {
            CheckDrinkInfo.ErrorMessage = result.ErrorMessage;
            return;
        }
        else
        {
            NewDrink.imageUrl = result.ImageUrl;
        }
    }
    private async void OnChangeImage()
    {
        if(SelectedDrink == null)
        {
            CheckDrinkInfo.ErrorMessage = "Please select a drink to change image";
            return;
        }
        var result =await uploadImageHelper.GetImageUrlFromSupaBase();
        if (result.ErrorMessage != "")
        {
            CheckDrinkInfo.ErrorMessage = result.ErrorMessage;
            return;
        }
        else
        {
            SelectedDrink.imageUrl = result.ImageUrl;
        }
    }

    private void UpdateDrink()
    {
        if (SelectedDrink == null)
        {
            CheckDrinkInfo.ErrorMessage = "Please select a drink to update";
            return;
        }

        var validations = new List<(string FieldValue, string ErrorMessage)>
        {
                (SelectedDrink.name, "Drink name cannot be blank."),
            (SelectedDrink.drinkType, "Type cannot be blank."),
            (SelectedDrink.imageUrl, "Please upload image.")
        };


        foreach (var (fieldValue, errorMessage) in validations)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                CheckDrinkInfo.ErrorMessage = errorMessage;
                return;
            }
        }



        if (SelectedDrink.price <= 0)
        {
            CheckDrinkInfo.ErrorMessage = "Price must be greater than 0";
            return;
        }

        var validDrinkTypes = new List<string> { "Coffee", "MilkTea", "Croissant", "Smoothie", "Juice", "Tea" };

        if (string.IsNullOrWhiteSpace(SelectedDrink.drinkType) || !validDrinkTypes.Contains(SelectedDrink.drinkType))
        {
            CheckDrinkInfo.ErrorMessage = "Drink type must be one of the following: Coffee, MilkTea, Croissant, Smoothie, Juice, Tea.";
            return;
        }
       

        _dao.UpdateDrink(SelectedDrink);
        SelectedDrink = null;
        CheckDrinkInfo = new ErrorHandling();
        LoadData();
    }

    private void SaveDrink()
    {
        Debug.WriteLine(NewDrink.drinkType);
        var validations = new List<(string FieldValue, string ErrorMessage)>
        {
                (NewDrink.name, "Drink name cannot be blank."),
            (NewDrink.drinkType, "Type cannot be blank."),
            (NewDrink.imageUrl, "Please upload image.")
        };


        foreach (var (fieldValue, errorMessage) in validations)
        {
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                CheckDrinkInfo.ErrorMessage = errorMessage;
                return;
            }
        }

    

        if(NewDrink.price <= 0)
        {
            CheckDrinkInfo.ErrorMessage = "Price must be greater than 0";
            return;
        }

        var validDrinkTypes = new List<string> { "Coffee", "MilkTea", "Croissant", "Smoothie", "Juice", "Tea" };

        if (string.IsNullOrWhiteSpace(NewDrink.drinkType) || !validDrinkTypes.Contains(NewDrink.drinkType))
        {
            CheckDrinkInfo.ErrorMessage = "Drink type must be one of the following: Coffee, MilkTea, Croissant, Smoothie, Juice, Tea.";
            return;
        }

        if (NewDrink == null)
        {
            CheckDrinkInfo.ErrorMessage = "Please input drink information";
            return;
        }

        var findDrink=_dao.GetDrinkByName(NewDrink.name);
        if (findDrink != null)
        {
            CheckDrinkInfo.ErrorMessage = "This item already exists";
            return;
        }
        _dao.SaveDrink(NewDrink);
        IsAddSuccess = true;
        NewDrink = new Drinks();
        LoadData();
        CheckDrinkInfo = new ErrorHandling();
    }

    private void ConfirmDelete()
    {
        if (SelectedDrink == null)
        {
            CheckDrinkInfo.ErrorMessage = "Please select a drink to delete";
            return;
        }

        _dao.DeleteDrink(SelectedDrink.id);
        SelectedDrink= null;
        LoadData();
    }

    public void LoadData()
    {

        var items = _dao.GetDrinkWithFilter("");

        if (items == null || !items.Any())
        {
            Drinks = new ObservableCollection<Drinks>();
        }
        else
        {
            Drinks = new ObservableCollection<Drinks>(items);
        }
    }

    private void PerformSearch()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {

            LoadData();
        }
        else
        {
            var filteredDrink = _dao.GetDrinkWithFilter(SearchText);
            Drinks = new ObservableCollection<Drinks>(filteredDrink);
        }
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
    {
        if (Equals(storage, value))
            return false;

        storage = value;
        OnPropertyChanged(propertyName);
        return true;
    }


}