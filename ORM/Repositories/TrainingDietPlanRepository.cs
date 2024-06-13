using System;
using System.Data.SqlClient;
using DatabaseProject;
using DatabaseProject.Models;

public static class TrainingDietPlanRepository
{
    public static TrainingDietPlan GetDietPlanByUserIdAndTrainerId(Database pdb, int userId, int trainerId)
    {
        Database db = Database.Connect(pdb);

        string query = @"SELECT plan_id, starting_date, end_time, goal_description, type, id_user, id_trainer
                         FROM trainingdietplan
                         WHERE id_user = @userId AND id_trainer = @trainerId";

        SqlCommand command = db.CreateCommand(query);
        command.Parameters.AddWithValue("@userId", userId);
        command.Parameters.AddWithValue("@trainerId", trainerId);

        TrainingDietPlan dietPlan = null;
        using (SqlDataReader reader = db.Select(command))
        {
            if (reader.Read())
            {
                dietPlan = new TrainingDietPlan
                {
                    PlanId = reader.GetInt32(reader.GetOrdinal("plan_id")),
                    StartingDate = reader.GetDateTime(reader.GetOrdinal("starting_date")),
                    EndTime = reader.GetDateTime(reader.GetOrdinal("end_time")),
                    GoalDescription = reader["goal_description"].ToString(),
                    Type = reader["type"].ToString(),
                    UserId = reader.GetInt32(reader.GetOrdinal("id_user")),
                    TrainerId = reader.GetInt32(reader.GetOrdinal("id_trainer"))
                };
            }
        }
        Database.Close(pdb, db);

        return dietPlan;
    }

    public static void CreateDietPlan(Database pdb, TrainingDietPlan dietPlan)
    {
        Database db = Database.Connect(pdb);

        string query = @"INSERT INTO trainingdietplan (starting_date, end_time, goal_description, type, id_user, id_trainer)
                         VALUES (@startingDate, @endTime, @goalDescription, @type, @userId, @trainerId)";

        SqlCommand command = db.CreateCommand(query);
        command.Parameters.AddWithValue("@startingDate", dietPlan.StartingDate);
        command.Parameters.AddWithValue("@endTime", dietPlan.EndTime);
        command.Parameters.AddWithValue("@goalDescription", dietPlan.GoalDescription);
        command.Parameters.AddWithValue("@type", dietPlan.Type);
        command.Parameters.AddWithValue("@userId", dietPlan.UserId);
        command.Parameters.AddWithValue("@trainerId", dietPlan.TrainerId);

        db.ExecuteNonQuery(command);
        Database.Close(pdb, db);
    }


    public static void DeleteDietPlan(Database pdb, int planId)
    {
        try
        {
            Database db = Database.Connect(pdb);

            string query = "DELETE FROM trainingdietplan WHERE plan_id = @planId";

            SqlCommand command = db.CreateCommand(query);
            command.Parameters.AddWithValue("@planId", planId);

            db.ExecuteNonQuery(command);
            Database.Close(pdb, db);

            Console.WriteLine($"Jedálniček s ID {planId} bol úspešne odstránený.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba pri odstraňovaní jedálnička: " + ex.Message);
        }
    }

    public static int GetNewIdPlan(Database pdb)
    {
        Database db = Database.Connect(pdb);

        string query = "SELECT IDENT_CURRENT('trainingdietplan') AS LastId";
        SqlCommand command = db.CreateCommand(query);

        int lastId = -1;
        using (SqlDataReader reader = db.Select(command))
        {
            if (reader.Read())
            {
                lastId = Convert.ToInt32(reader["LastId"]);
            }
        }
        Database.Close(pdb, db);

        return lastId;
    }


}
