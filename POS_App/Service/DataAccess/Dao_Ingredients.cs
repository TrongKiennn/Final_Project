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

public class Dao_Ingredients : IDao_Ingredients
{
    public List<Ingredient> GetIngredients()
    {
        var result = new List<Ingredient>();
        var connectionString = GetConnectionString();
        MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();


        var sql = @"SELECT ingredient_id, name, unit, status, stock FROM ingredients ORDER BY unit DESC;";

        var command = new MySqlCommand(sql, connection);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                var ingredient = new Ingredient
                {
                    ingredient_id = reader.GetInt32("ingredient_id"),
                    name = reader.GetString("name"),
                    unit = reader.GetString("unit"),
                    status = reader.GetString("status"),
                    stock = reader.GetInt32("stock")
                };

                
                result.Add(ingredient);
            }

           
        }

        connection.Close();

        return result;
    }

    public void DeleteIngredient(int ingredientId)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"DELETE FROM ingredients WHERE ingredient_id = @ingredientId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ingredientId", ingredientId);
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

    public void UpdateIngredientStockById(int ingredientId, int stock)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"Update ingredients SET stock=stock+@stock WHERE ingredient_id = @ingredientId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ingredientId", ingredientId);
                    command.Parameters.AddWithValue("@stock", stock);
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

    public void CreateIngredient(Ingredient ingredient)
    {
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"INSERT INTO ingredients (name, stock, unit) VALUES ( @name, @stock, @unit)";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@unit", ingredient.unit);
                    command.Parameters.AddWithValue("@name", ingredient.name);
                    command.Parameters.AddWithValue("@stock", ingredient.stock);
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

   public Ingredient GetIngredientById(int ingredientId)
    {
        Ingredient ingredient = new Ingredient();
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"SELECT ingredient_id, name, unit, status, stock FROM ingredients WHERE ingredient_id = @ingredientId";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ingredientId", ingredientId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ingredient.ingredient_id = reader.GetInt32("ingredient_id");
                            ingredient.name = reader.GetString("name");
                            ingredient.unit = reader.GetString("unit");
                            ingredient.status = reader.GetString("status");
                            ingredient.stock = reader.GetInt32("stock");
                        }
                    }
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

        return ingredient;
    }

    public Ingredient GetIngredientByName(string ingredientName)
    {
        Ingredient ingredient = null;
        try
        {
            var connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                var sql = @"SELECT ingredient_id, name, unit, status, stock FROM ingredients WHERE name = @ingredientName";
                using (var command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@ingredientName", ingredientName);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ingredient = new Ingredient()
                            {
                                ingredient_id = reader.GetInt32("ingredient_id"),
                                name = reader.GetString("name"),
                                unit = reader.GetString("unit"),
                                status = reader.GetString("status"),
                                stock = reader.GetInt32("stock")
                            };
                           
                        }
                    }
                }
                connection.Close();
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine($"Ingredients: MySQL error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ingredients: General error: {ex.Message}");
        }

        return ingredient;
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