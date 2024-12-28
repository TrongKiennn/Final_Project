using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_App.Service.DataAccess;

public interface IDao_Ingredients
{

    List<Ingredient> GetIngredients();

    Ingredient GetIngredientById(int ingredientId);

    Ingredient GetIngredientByName(string ingredientName);
    public void DeleteIngredient(int ingredientId);
    public void UpdateIngredientStockById(int ingredientId, int stock);
    public void CreateIngredient(Ingredient ingredient);
    //public void UpdateTableStatus(int tableId, string status, int orderId);

    //public Task UpdateTableStatusAsync(Table table);

}
