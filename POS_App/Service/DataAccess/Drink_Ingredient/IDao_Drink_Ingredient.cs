using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Drink_Ingredient
{

    List<Drink_Ingredient> GetDrinkIngredientsByDrinkId(int drink_id);
    public void DeleteDrinkIngredient(int drinkId, int ingredientId);
    public void CreateDrinkIngredient(int drinkId, int ingredientId, int quantity);
    public void UpdateQuantityOfDrinkIngredient(int drinkId, int ingredientId,int quantity);
}
