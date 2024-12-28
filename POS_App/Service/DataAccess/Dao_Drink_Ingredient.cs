using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using POS_App.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static POS_App.Service.DataAccess.IDao_Tables;
namespace POS_App.Service.DataAccess;

public class Dao_Drink_Ingredient : IDao_Drink_Ingredient
{
    public List<Drink_Ingredient> GetDrinkIngredientsByDrinkId(int drink_id)
    {
        var result = new List<Drink_Ingredient>();
        var connectionString = GetConnectionString();
        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();


        var sql = @"SELECT drink_id, ingredient_id, quantity FROM drink_ingredients Where drink_id=@drink_id;";

       
        var command = new MySqlCommand(sql, connection);
        command.Parameters.AddWithValue("@drink_id", drink_id);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var drinkIngredient = new Drink_Ingredient
                {
                    drink_id = reader.GetInt32("drink_id"),
                    ingredient_id = reader.GetInt32("ingredient_id"),
                    quantity = reader.GetInt32("quantity")
                };

                result.Add(drinkIngredient);
            }


        }

        connection.Close();

        return result;
    }

    public void DeleteDrinkIngredient(int drinkId, int ingredientId)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"DELETE FROM drink_ingredients WHERE drink_id=@drink_id AND ingredient_id=@ingredient_id;";
                var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@drink_id", drinkId);
                command.Parameters.AddWithValue("@ingredient_id", ingredientId);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        catch (MySqlException ex)
        {

            Console.WriteLine($"MySQL error: {ex.Message}");
        }
        catch (Exception ex)
        {

            Console.WriteLine($"General error: {ex.Message}");
        }
    }

    public void CreateDrinkIngredient(int drinkId, int ingredientId, int quantity)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"INSERT INTO drink_ingredients (drink_id, ingredient_id, quantity) 
                VALUES (@drink_id, @ingredient_id, @quantity)
                ON DUPLICATE KEY UPDATE quantity = VALUES(quantity);";

                var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@drink_id", drinkId);
                command.Parameters.AddWithValue("@ingredient_id", ingredientId);
                command.Parameters.AddWithValue("@quantity", quantity);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
        catch (MySqlException ex)
        {

            Console.WriteLine($"Drink_Ingredient: MySQL error: {ex.Message}");
        }
        catch (Exception ex)
        {

            Console.WriteLine($"Drink_Ingredient: General error: {ex.Message}");
        }
    }

    public void UpdateQuantityOfDrinkIngredient(int deinkId, int ingredientId, int quantity)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"Update drink_ingredients SET quantity=@quantity WHERE ingredient_id = @ingredientId and drink_id=@drinkId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@drinkId", deinkId);
                    command.Parameters.AddWithValue("@ingredientId", ingredientId);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        catch (MySqlException ex)
        {

            Console.WriteLine($"MySQL error: {ex.Message}");
        }
        catch (Exception ex)
        {

            Console.WriteLine($"General error: {ex.Message}");
        }
    }
    private static string GetConnectionString()
    {
        var connectionString = "" +
            "Server=localhost;" +
            "Database=pos_manager;" +
            "User=root;" +
            "Password=1234;";

        return connectionString;
    }

}