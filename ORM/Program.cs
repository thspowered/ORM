using System;
using System.Collections.Generic;
using DatabaseProject;
using DatabaseProject.Models;

namespace DatabaseProjectTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Database db = new Database();
                db.Connect();

                // Testovanie UserRepository
                Console.WriteLine("---- Testovanie UserRepository ----");
                User user = UserRepository.GetUserById(db, 1);
                Console.WriteLine($"User Name: {user.Name}, Surname: {user.Surname}");

                // Testovanie TrainerRepository
                Console.WriteLine("\n---- Testovanie TrainerRepository ----");
                Trainer trainer = TrainerRepository.GetTrainerById(db, 1);
                Console.WriteLine($"Trainer Name: {trainer.Name}, Surname: {trainer.Surname}");


                List<Meal> meals = MealsRepository.GetAllMeals(db, user.Id);

                Console.WriteLine($"Zoznam jedál pre používateľa s ID {user.Id}:");
                foreach (Meal meal in meals)
                {
                    Console.WriteLine($"ID: {meal.MealId}, Názov: {meal.MealName}, Popis: {meal.MealDescription}, Kalórie: {meal.MealCalories}, Proteíny: {meal.MealProtein}, Sacharidy: {meal.MealCarbs}, Tuky: {meal.MealFat}, ID trénera: {meal.TrainerId}");
                }

                MealsRepository.UpdateMeal(db, 1, "kasa", "asd", 20, 20, 10, 4);

                MealsRepository.CalculateTotalNutrition(db, 1);

               
                // Test vytvorenia nového jedálnička
                Console.WriteLine("\n---- Testovanie vytvorenia nového jedálnička ----");
                int result = DietPlanTransaction.CreateDietPlanTransaction(db, 10, 23, DateTime.Now, DateTime.Now.AddDays(7), "Lose weight", "D", new List<int> { 1, 2, 3 });
                if (result == 1)
                {
                    Console.WriteLine("Jedálniček bol úspešne vytvorený.");
                }
                else if (result == -1)
                {
                    Console.WriteLine("Chyba: Klient nepatrí k danému trénerovi.");
                }
                else if (result == -2)
                {
                    Console.WriteLine("Chyba: Transakcia zlyhala.");
                }

                db.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Chyba: {ex.Message}");
            }
        }
    }
}
