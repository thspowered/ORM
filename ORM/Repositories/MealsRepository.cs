using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseProject;
using DatabaseProject.Models;

public static class MealsRepository
{
    public static List<Meal> GetMealsByIds(Database pdb, List<int> mealIds)
    {
        

        string query = @"SELECT meal_id, meal_name, meal_description, meal_calories, meal_protein, meal_carbs, meal_fat, id_trainer
                         FROM meals
                         WHERE meal_id IN (@mealIds)";

        SqlCommand command =pdb.CreateCommand(query);
        command.Parameters.AddWithValue("@mealIds", string.Join(",", mealIds));

        List<Meal> meals = new List<Meal>();
        using (SqlDataReader reader = pdb.Select(command))
        {
            while (reader.Read())
            {
                meals.Add(new Meal
                {
                    MealId = reader.GetInt32(reader.GetOrdinal("meal_id")),
                    MealName = reader["meal_name"].ToString(),
                    MealDescription = reader["meal_description"].ToString(),
                    MealCalories = reader.GetInt32(reader.GetOrdinal("meal_calories")),
                    MealProtein = reader.GetDouble(reader.GetOrdinal("meal_protein")),
                    MealCarbs = reader.GetDouble(reader.GetOrdinal("meal_carbs")),
                    MealFat = reader.GetDouble(reader.GetOrdinal("meal_fat")),
                    TrainerId = reader.GetInt32(reader.GetOrdinal("id_trainer"))
                });
            }
        }
       

        return meals;
    }

    public static void AddMealsToDietPlan(Database pdb, int planId, List<int> mealIds)
    {
        try
        {
            

            foreach (int mealId in mealIds)
            {
                string query = @"INSERT INTO dietplan_meals (plan_id, meal_id) VALUES (@planId, @mealId)";
                SqlCommand command = pdb.CreateCommand(query);
                command.Parameters.AddWithValue("@planId", planId);
                command.Parameters.AddWithValue("@mealId", mealId);

                pdb.ExecuteNonQuery(command);
            }

           
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba pri pridávaní jedál do jedálnička: " + ex.Message);
        }
    }

    public static void DeleteDietPlanMeal(Database pdb, int planId)
    {
        try
        {
          

            string query = "DELETE FROM dietplan_meals WHERE plan_id = @planId";

            SqlCommand command = pdb.CreateCommand(query);
            command.Parameters.AddWithValue("@planId", planId);

            pdb.ExecuteNonQuery(command);
            

            Console.WriteLine($"Záznamy o jedlách pre jedálniček s ID {planId} boli úspešne odstránené.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba pri odstraňovaní záznamov o jedlách: " + ex.Message);
        }
    }


    public static List<Meal> GetAllMeals(Database pdb, int userId)
    {
        

        string query = @"SELECT meals.meal_id, meals.meal_name, meals.meal_description, meals.meal_calories, meals.meal_protein, meals.meal_carbs, meals.meal_fat, meals.id_trainer
                     FROM meals
                     INNER JOIN dietplan_meals ON meals.meal_id = dietplan_meals.meal_id
                     INNER JOIN trainingdietplan ON dietplan_meals.plan_id = trainingdietplan.plan_id
                     WHERE trainingdietplan.id_user = @userId";

        SqlCommand command = pdb.CreateCommand(query);
        command.Parameters.AddWithValue("@userId", userId);

        List<Meal> meals = new List<Meal>();
        using (SqlDataReader reader = pdb.Select(command))
        {
            while (reader.Read())
            {
                meals.Add(new Meal
                {
                    MealId = reader.GetInt32(reader.GetOrdinal("meal_id")),
                    MealName = reader["meal_name"].ToString(),
                    MealDescription = reader["meal_description"].ToString(),
                    MealCalories = reader.GetInt32(reader.GetOrdinal("meal_calories")),
                    MealProtein = reader.GetDouble(reader.GetOrdinal("meal_protein")),
                    MealCarbs = reader.GetDouble(reader.GetOrdinal("meal_carbs")),
                    MealFat = reader.GetDouble(reader.GetOrdinal("meal_fat")),
                    TrainerId = reader.GetInt32(reader.GetOrdinal("id_trainer"))
                });
            }
        }

        return meals;
    }


    public static void CalculateTotalNutrition(Database pdb, int planId)
    {
        

        string query = @"SELECT meals.meal_calories, meals.meal_protein, meals.meal_carbs, meals.meal_fat
                     FROM dietplan_meals
                     INNER JOIN meals ON dietplan_meals.meal_id = meals.meal_id
                     WHERE dietplan_meals.plan_id = @planId";

        SqlCommand command = pdb.CreateCommand(query);
        command.Parameters.AddWithValue("@planId", planId);

        int totalCalories = 0;
        double totalProtein = 0;
        double totalCarbs = 0;
        double totalFat = 0;

        using (SqlDataReader reader = pdb.Select(command))
        {
            while (reader.Read())
            {
                totalCalories += reader.GetInt32(reader.GetOrdinal("meal_calories"));
                totalProtein += reader.GetDouble(reader.GetOrdinal("meal_protein"));
                totalCarbs += reader.GetDouble(reader.GetOrdinal("meal_carbs"));
                totalFat += reader.GetDouble(reader.GetOrdinal("meal_fat"));
            }
        }

        Console.WriteLine($"Celkové nutričné hodnoty pre jedálniček s ID {planId}:");
        Console.WriteLine($"Celkové kalórie: {totalCalories}");
        Console.WriteLine($"Celkové bielkoviny: {totalProtein} g");
        Console.WriteLine($"Celkové sacharidy: {totalCarbs} g");
        Console.WriteLine($"Celkové tuky: {totalFat} g");

       
    }


    public static void UpdateMeal(Database pdb, int mealId, string newName, string newDescription, int newCalories, float newProtein, float newCarbs, float newFat)
    {
        try
        {
            

            string query = @"UPDATE meals 
                         SET meal_name = @newName, meal_description = @newDescription, meal_calories = @newCalories, 
                             meal_protein = @newProtein, meal_carbs = @newCarbs, meal_fat = @newFat
                         WHERE meal_id = @mealId";

            SqlCommand command = pdb.CreateCommand(query);
            command.Parameters.AddWithValue("@newName", newName);
            command.Parameters.AddWithValue("@newDescription", newDescription);
            command.Parameters.AddWithValue("@newCalories", newCalories);
            command.Parameters.AddWithValue("@newProtein", newProtein);
            command.Parameters.AddWithValue("@newCarbs", newCarbs);
            command.Parameters.AddWithValue("@newFat", newFat);
            command.Parameters.AddWithValue("@mealId", mealId);

            int rowsAffected = pdb.ExecuteNonQuery(command);

            if (rowsAffected > 0)
            {
                Console.WriteLine($"Jedlo s ID {mealId} bolo úspešne aktualizované.");
            }
            else
            {
                Console.WriteLine($"Jedlo s ID {mealId} sa nepodarilo aktualizovať.");
            }

            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba pri aktualizovaní jedla: " + ex.Message);
        }
    }


    public static void DeleteDietPlanMeal(Database pdb, int planId, int mealId)
    {
        try
        {
            string query = "DELETE FROM dietplan_meals WHERE plan_id = @planId AND meal_id = @mealId";

            SqlCommand command = pdb.CreateCommand(query);
            command.Parameters.AddWithValue("@planId", planId);
            command.Parameters.AddWithValue("@mealId", mealId);

            pdb.ExecuteNonQuery(command);

            Console.WriteLine($"Záznam o jedle s ID {mealId} pre jedálniček s ID {planId} bol úspešne odstránený.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba pri odstraňovaní záznamu o jedle: " + ex.Message);
        }
    }


}
