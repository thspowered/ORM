using DatabaseProject;
using DatabaseProject.Models;
using System;
using System.Data.SqlClient;

public static class TrainerRepository
{
    public static Trainer GetTrainerById(Database db, int id)
    {
        SqlCommand command = db.CreateCommand(@"
            SELECT id_trainer, name, surname, email, phone, qualifications
            FROM trainer
            WHERE id_trainer = @id");
        command.Parameters.AddWithValue("@id", id);

        Trainer trainer = null;
        using (SqlDataReader reader = db.Select(command))
        {
            if (reader.Read())
            {
                trainer = new Trainer
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id_trainer")),
                    Name = reader["name"].ToString(),
                    Surname = reader["surname"].ToString(),
                    Email = reader["email"].ToString(),
                    Phone = reader["phone"].ToString(),
                    Qualifications = reader["qualifications"].ToString()
                };
            }
        }

        return trainer;
    }
}
