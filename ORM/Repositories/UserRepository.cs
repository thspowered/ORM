using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseProject;
using DatabaseProject.Models;

public static class UserRepository
{
    public static User GetUserById(Database pdb, int id)
    {
        try
        {
            string query = @"SELECT id_user, name, surname, email, phone, age, height, weight, sex, role, allergies, id_trainer
                         FROM [User]
                         WHERE id_user = @id";

            SqlCommand command = pdb.CreateCommand(query);
            command.Parameters.AddWithValue("@id", id);

            User user = null;
            using (SqlDataReader reader = pdb.Select(command))
            {
                if (reader.Read())
                {
                    user = new User
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id_user")),
                        Name = reader["name"].ToString(),
                        Surname = reader["surname"].ToString(),
                        Email = reader["email"].ToString(),
                        Phone = reader["phone"].ToString(),
                        Age = reader.GetInt32(reader.GetOrdinal("age")),
                        Height = reader.GetDouble(reader.GetOrdinal("height")),
                        Weight = reader.GetDouble(reader.GetOrdinal("weight")),
                        Sex = reader["sex"].ToString(),
                        Role = reader["role"].ToString(),
                        Allergies = reader["allergies"].ToString(),
                        TrainerId = reader.GetInt32(reader.GetOrdinal("id_trainer"))
                    };
                }
            }

            return user;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Chyba pri získavaní používateľa: " + ex.Message);
            return null;
        }
    }
}
