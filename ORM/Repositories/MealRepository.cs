using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseProject;
using DatabaseProject.Models;

public static class MealRepository
{
    public static List<Meal> GetMealsByIds(Database pdb, List<int> mealIds)
    {
        Database db = Database.Connect(pdb);

        string query = @"SELECT meal_id, meal_name, meal_description, meal_calories, meal_protein, meal_carbs, meal_fat, id_trainer
                         FROM meals
                         WHERE meal_id IN (@mealIds)";

        SqlCommand command = db.CreateCommand(query);
        command.Parameters.AddWithValue("@mealIds", string.Join(",", mealIds));

        List<Meal> meals = new List<Meal>();
        using (SqlDataReader reader = db.Select(command))
        {
            while (reader.Read())
            {
                meals.Add(new Meal
                {
                    MealId = reader.GetInt32(reader.GetOrdinal("meal_id")),
                    MealName = reader["meal_name"].ToString(),
                    MealDescription = reader["meal_description"].ToString(),
                    MealCalories = reader.GetInt32(reader.GetOrdinal("meal_calories")),
                    MealProtein = reader.GetFloat(reader.GetOrdinal("meal_protein")),
                    MealCarbs = reader.GetFloat(reader.GetOrdinal("meal_carbs")),
                    MealFat = reader.GetFloat(reader.GetOrdinal("meal_fat")),
                    TrainerId = reader.GetInt32(reader.GetOrdinal("id_trainer"))
                });
            }
        }
        Database.Close(pdb, db);

        return meals;
    }
}
