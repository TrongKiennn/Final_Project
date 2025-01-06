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


namespace POS_App;

public partial class DrinkIngredientViewModel : INotifyPropertyChanged
{
    IDao_Drinks _dao;
    IDao_Drink_Ingredient _Dao_Drink_Ingredient;
    IDao_Ingredients _Dao_Ingredients;
    public ICommand DrinkClickCommand { get; set; }
    public ICommand IngredientClickCommand { get; set; }
    public ICommand ConfirmDeleteCommand { get; set; }
    public ICommand ContinueToUpdateCommand { get; set; }
    public ICommand ContinueToCreateCommand { get; set; }

    public ObservableCollection<Drinks> Drinks { get; set; }
    public ObservableCollection<Drink_Ingredient> Drink_Ingredients { get; set; }
    public ObservableCollection<Ingredient> Ingredients { get; set; }

    public ObservableCollection<IngredientOfDrinkRecipe> ingredientOfDrinkRecipes { get; set; }

    private Drinks _selectedDrink;
    public Drinks SelectedDrink
    {
        get => _selectedDrink;
        set => SetProperty(ref _selectedDrink, value);
    }
    private ErrorHandling _errorCreate;
    public ErrorHandling ErrorCreate
    {
        get => _errorCreate;
        set => SetProperty(ref _errorCreate, value);
    }
    private ErrorHandling _errorUpdateOrDelete;
    public ErrorHandling ErrorUpdateOrDelete
    {
        get => _errorUpdateOrDelete;
        set => SetProperty(ref _errorUpdateOrDelete, value);
    }

    private IngredientOfDrinkRecipe _newIngredientOdDrinkRecipe;
    public IngredientOfDrinkRecipe NewIngredientOfDrinkRecipe
    {
        get => _newIngredientOdDrinkRecipe;
        set => SetProperty(ref _newIngredientOdDrinkRecipe, value);
    }

    private IngredientOfDrinkRecipe _selectedIngredientOfDrinkRecipe;
    public IngredientOfDrinkRecipe SelectedIngredientOfDrinkRecipe
    {
        get => _selectedIngredientOfDrinkRecipe;
        set => SetProperty(ref _selectedIngredientOfDrinkRecipe, value);
    }

    private string _userRole;
    public string UserRole
    {
        get => _userRole;
        set => SetProperty(ref _userRole, value);
    }

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
    public DrinkIngredientViewModel()
    { 
        DrinkClickCommand = new RelayCommand<Drinks>(OnDrinkClick);
        _dao = ServiceFactory.GetChildOf(typeof(IDao_Drinks)) as IDao_Drinks;
        _Dao_Ingredients = ServiceFactory.GetChildOf(typeof(IDao_Ingredients)) as IDao_Ingredients;
        _Dao_Drink_Ingredient = ServiceFactory.GetChildOf(typeof(IDao_Drink_Ingredient)) as IDao_Drink_Ingredient;

        IngredientClickCommand = new RelayCommand<IngredientOfDrinkRecipe>(OnIngredientClick);
        ConfirmDeleteCommand = new RelayCommand(_ => OnConfirmDelete());
        ContinueToUpdateCommand = new RelayCommand(_ => OnContinueToUpdate());
        ContinueToCreateCommand = new RelayCommand(_ => OnContinueToCreate());

        NewIngredientOfDrinkRecipe = new IngredientOfDrinkRecipe();
        ErrorUpdateOrDelete = new ErrorHandling();
        ErrorCreate = new ErrorHandling();
        var localSettings = ApplicationData.Current.LocalSettings;

        if (localSettings.Values.ContainsKey("Role"))
        {
            UserRole = localSettings.Values["Role"] as string;
        }

        LoadData();
    }

    private void OnDrinkClick(Drinks selectedDrink)
    {
        ingredientOfDrinkRecipes = new ObservableCollection<IngredientOfDrinkRecipe>();
        Debug.WriteLine(selectedDrink.name);
        SelectedDrink = selectedDrink;
        var drinkIngreItem = _Dao_Drink_Ingredient.GetDrinkIngredientsByDrinkId(SelectedDrink.id);
        Drink_Ingredients = new ObservableCollection<Drink_Ingredient>(drinkIngreItem);
        foreach (var item in Drink_Ingredients)
        {
           var Ingredient=_Dao_Ingredients.GetIngredientById(item.ingredient_id);
            ingredientOfDrinkRecipes.Add(
                new IngredientOfDrinkRecipe 
                { 
                    DrinkId= SelectedDrink.id,
                    IngredientId=Ingredient.ingredient_id ,
                    Name = Ingredient.name,
                    Quantity = item.quantity,
                    Unit = Ingredient.unit
                }
            );
        }
        NewIngredientOfDrinkRecipe.DrinkId = SelectedDrink.id;
    }

    private void LoadDrinkRecipe()
    {
        ingredientOfDrinkRecipes = new ObservableCollection<IngredientOfDrinkRecipe>();
        var drinkIngreItem = _Dao_Drink_Ingredient.GetDrinkIngredientsByDrinkId(SelectedDrink.id);
        Drink_Ingredients = new ObservableCollection<Drink_Ingredient>(drinkIngreItem);
        foreach (var item in Drink_Ingredients)
        {
            var Ingredient = _Dao_Ingredients.GetIngredientById(item.ingredient_id);
            ingredientOfDrinkRecipes.Add(
                new IngredientOfDrinkRecipe
                {
                    DrinkId = SelectedDrink.id,
                    IngredientId = Ingredient.ingredient_id,
                    Name = Ingredient.name,
                    Quantity = item.quantity,
                    Unit = Ingredient.unit
                }
            );
        }
        ErrorCreate.ErrorMessage = "";
        ErrorUpdateOrDelete.ErrorMessage = "";
    }

    public void LoadData()
    {
      
            var items= _dao.GetDrinkWithFilter("");

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

    private void OnIngredientClick(IngredientOfDrinkRecipe clickedIngredientOfDrinkRecipe)
    {
        SelectedIngredientOfDrinkRecipe = clickedIngredientOfDrinkRecipe;
        Debug.WriteLine(SelectedIngredientOfDrinkRecipe.Name);
    }
    private void OnConfirmDelete()
    {
        if (UserRole == "manager")
        {
            if (SelectedIngredientOfDrinkRecipe != null)
            {
                _Dao_Drink_Ingredient.DeleteDrinkIngredient(SelectedIngredientOfDrinkRecipe.DrinkId, SelectedIngredientOfDrinkRecipe.IngredientId);
                LoadDrinkRecipe();
                ErrorUpdateOrDelete.ErrorMessage = "";
                SelectedDrink = null;
                SelectedIngredientOfDrinkRecipe = null;
            }
            else
            {
                ErrorUpdateOrDelete.ErrorMessage = "You haven't chosen any ingredient yet.";
            }
        }
        else
        {
            ErrorUpdateOrDelete.ErrorMessage = "You don't have permission to delete.";
        }
    }

    private void OnContinueToUpdate()
    {
        if (UserRole == "manager")
        {
            if(SelectedDrink == null)
            {
                ErrorUpdateOrDelete.ErrorMessage = "You haven't chosen a drink yet.";
                return;
            }
            if (SelectedIngredientOfDrinkRecipe != null)
            {
                if (SelectedIngredientOfDrinkRecipe.Quantity < 0)
                {
                    ErrorUpdateOrDelete.ErrorMessage = "Quantity must be greater than 0.";
                    return;
                }
                _Dao_Drink_Ingredient.UpdateQuantityOfDrinkIngredient(SelectedIngredientOfDrinkRecipe.DrinkId, SelectedIngredientOfDrinkRecipe.IngredientId, SelectedIngredientOfDrinkRecipe.Quantity);
                SelectedIngredientOfDrinkRecipe = null;
                SelectedDrink = null;
                ErrorUpdateOrDelete.ErrorMessage = "";
                LoadDrinkRecipe();
            }
        }
        else
        {

            ErrorUpdateOrDelete.ErrorMessage = "You don't have permission to update.";
        }
    }

    private void OnContinueToCreate()
    {
        if (UserRole == "manager")
        {
            if(SelectedDrink == null)
            {
                ErrorCreate.ErrorMessage = "You haven't chosen a drink yet.";
                return;
            }
            var findIngredient = _Dao_Ingredients.GetIngredientByName(NewIngredientOfDrinkRecipe.Name);
            if (findIngredient == null)
            {
                ErrorCreate.ErrorMessage = $"Cannot find any ingredient named {NewIngredientOfDrinkRecipe.Name}";
            }
            else
            {
                if (NewIngredientOfDrinkRecipe.DrinkId == 0)
                {
                    ErrorCreate.ErrorMessage = "You haven't chosen a drink yet.";
                    return;
                }
                if (NewIngredientOfDrinkRecipe.Quantity < 0)
                {
                    ErrorCreate.ErrorMessage = "Quantity must be greater than 0.";
                    return;
                }
                if (NewIngredientOfDrinkRecipe != null)
                {
                    
                    _Dao_Drink_Ingredient.CreateDrinkIngredient(NewIngredientOfDrinkRecipe.DrinkId, findIngredient.ingredient_id, NewIngredientOfDrinkRecipe.Quantity);
                    LoadDrinkRecipe();
                }
                NewIngredientOfDrinkRecipe =null;
                SelectedDrink = null;
                ErrorCreate.ErrorMessage = "";
            }

            
        }
        else
        {
            ErrorCreate.ErrorMessage = "You don't have permission to create.";
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