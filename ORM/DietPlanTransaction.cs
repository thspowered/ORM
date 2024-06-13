using DatabaseProject.Models;



namespace DatabaseProject
{
    public static class DietPlanTransaction
    {
        public static int CreateDietPlanTransaction(Database pdb, int clientId, int trainerId, DateTime start, DateTime end, string goalDescription, string type, List<int> mealIds)
        {
            
            try
            {
                
                // Začiatok transakcie
                pdb.BeginTransaction();

                // Získanie používateľa a trénera
                User user = UserRepository.GetUserById(pdb, clientId);
                Trainer trainer = TrainerRepository.GetTrainerById(pdb, trainerId);
                

               

                // Kontrola, či klient patrí k danému trénerovi
                if (user == null || trainer == null || user.TrainerId != trainerId)
                {
                    // Ak nie, rollback transakcie
                    pdb.Rollback();
                    return -1; // Návratová hodnota pre klienta, ktorý nepatrí k trénerovi
                }

                // Kontrola, či klient už má existujúci jedálniček
                TrainingDietPlan existingDietPlan = TrainingDietPlanRepository.GetDietPlanByUserIdAndTrainerId(pdb, clientId, trainerId);
                if (existingDietPlan != null)
                {
                    // Ak áno, odstráňte ho
                    MealsRepository.DeleteDietPlanMeal(pdb, existingDietPlan.PlanId);
                    TrainingDietPlanRepository.DeleteDietPlan(pdb, existingDietPlan.PlanId);
                }

                // Vytvorenie nového jedálnička
                TrainingDietPlan newDietPlan = new TrainingDietPlan
                {
                    StartingDate = start,
                    EndTime = end,
                    GoalDescription = goalDescription,
                    Type = type,
                    UserId = clientId,
                    TrainerId = trainerId
                };
                TrainingDietPlanRepository.CreateDietPlan(pdb, newDietPlan);

                // Získanie ID nového jedálnička
                int newDietPlanId = TrainingDietPlanRepository.GetNewIdPlan(pdb);

                // Pridanie jedál do tabuľky dietPlan_Meals
                MealsRepository.AddMealsToDietPlan(pdb, newDietPlanId, mealIds);

                // Ak všetko prebehlo úspešne, potvrdenie transakcie
                pdb.EndTransaction();

                return 1; // Návratová hodnota pre úspešné vytvorenie jedálnička
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transakcia zlyhala: " + ex.Message);
                pdb.Rollback();
                return -2; // Návratová hodnota pre chybu transakcie
            }
        }
    }
}