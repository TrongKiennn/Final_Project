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


namespace POS_App.ViewModel
{
   

    public class MaterialManagementViewModel : INotifyPropertyChanged
    {
        IDao_Ingredients _Dao_Ingredients;

        public ICommand IngredientClickCommand { get; set; }
        public ICommand ConfirmDeleteCommand { get; set; }
        public ICommand ContinueToUpdateCommand { get; set; }
        public ICommand ContinueToCreateCommand { get; set; }
        public ObservableCollection<Ingredient> Ingredients { get; set; }

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

        private Ingredient _newIngredient;
        public Ingredient NewIngredient
        {
            get => _newIngredient;
            set => SetProperty(ref _newIngredient, value);
        }

        private Ingredient _selectedIngredient;
        public Ingredient SelectedIngredient
        {
            get => _selectedIngredient;
            set => SetProperty(ref _selectedIngredient, value);
        }

        private Ingredient _updateIngredient;
        public Ingredient UpdateIngredient
        {
            get => _updateIngredient;
            set => SetProperty(ref _updateIngredient, value);
        }


        private string _userRole;
        public string UserRole
        {
            get => _userRole;
            set => SetProperty(ref _userRole, value);
        }
        public MaterialManagementViewModel()
        {
            _Dao_Ingredients = ServiceFactory.GetChildOf(typeof(IDao_Ingredients)) as IDao_Ingredients;
            IngredientClickCommand = new RelayCommand<Ingredient>(OnIngredientClick);
            ConfirmDeleteCommand = new RelayCommand(_=>OnConfirmDelete());
            ContinueToUpdateCommand = new RelayCommand(_ => OnContinueToUpdate());
            ContinueToCreateCommand = new RelayCommand(_ => OnContinueToCreate());

            NewIngredient= new Ingredient();
            ErrorUpdateOrDelete = new ErrorHandling();
            ErrorCreate = new ErrorHandling();
            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values.ContainsKey("Role"))
            {
                UserRole = localSettings.Values["Role"] as string;
            }
            LoadData();
        }


        public void LoadData()
        {
            var items = _Dao_Ingredients.GetIngredients();
            Ingredients = new ObservableCollection<Ingredient>(items);
        }

        private void OnIngredientClick(Ingredient selectedIngredient)
        {
            SelectedIngredient = selectedIngredient;
            UpdateIngredient = SelectedIngredient;
            Debug.WriteLine(SelectedIngredient.name);
        }

        private void OnConfirmDelete()
        {
            if (UserRole == "manager"  || UserRole == "admin")
            {
                if (SelectedIngredient != null)
                {
                    _Dao_Ingredients.DeleteIngredient(SelectedIngredient.ingredient_id);
                    LoadData();
                    ErrorUpdateOrDelete.ErrorMessage = "";
                }
            }
            else
            {
                ErrorUpdateOrDelete.ErrorMessage = "You don't have permission to delete.";
            }
        }

        private void OnContinueToUpdate()
        {
            if (UserRole == "manager"  || UserRole == "admin")
            {
               
                if (SelectedIngredient != null)
                {
                    if(SelectedIngredient.stock<0)
                    {
                        ErrorUpdateOrDelete.ErrorMessage = "Quantity must be greater than 0.";
                        return;
                    }
                    _Dao_Ingredients.UpdateIngredientStockById(UpdateIngredient.ingredient_id, UpdateIngredient.stock);
                   
                    ErrorUpdateOrDelete.ErrorMessage = "";
                }
            }
            else
            {
               
                ErrorUpdateOrDelete.ErrorMessage = "You don't have permission to update.";
               
                
            }
            SelectedIngredient = new Ingredient();
            UpdateIngredient = new Ingredient();
            LoadData();
        }

        private void OnContinueToCreate()
        {
            if (UserRole == "manager"  || UserRole == "admin")
            {
                var findIngredient=_Dao_Ingredients.GetIngredientByName(NewIngredient.name);
                if (findIngredient != null)
                {
                    ErrorCreate.ErrorMessage = "Ingredient already exists.";
                    return;
                }
                if (NewIngredient.stock < 0)
                {
                    ErrorUpdateOrDelete.ErrorMessage = "Quantity must be greater than 0.";
                    return;
                }
                if(NewIngredient.name == null || NewIngredient.name == "")
                {
                    ErrorCreate.ErrorMessage = "Ingredient name cannot be blank.";
                    return;
                }
                if(NewIngredient.unit == null || NewIngredient.unit == "")
                {
                    ErrorCreate.ErrorMessage = "Unit cannot be blank.";
                    return;
                }

                if (NewIngredient != null)
                {
                    _Dao_Ingredients.CreateIngredient(NewIngredient);
                    LoadData();
                    ErrorCreate.ErrorMessage= "";
                }
                NewIngredient = new Ingredient();
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
}
