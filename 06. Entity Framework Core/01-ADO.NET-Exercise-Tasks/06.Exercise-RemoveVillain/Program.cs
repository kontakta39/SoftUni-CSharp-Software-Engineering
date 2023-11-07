//6 Exercise - Remove Villain 
using System.Data.SqlClient;

namespace RemoveVillain;

public class Program
{
    const string connectionString = @"Server=ACER\SQLEXPRESS;Database=MinionsDB;Integrated Security=True";

    static async Task Main()
    {
        using SqlConnection sqlConnection = new(connectionString);
        sqlConnection.Open();
        int currentVillainId = int.Parse(Console.ReadLine());
        using SqlCommand sqlCommand = new(SQLQueries.selectVillains, sqlConnection);
        sqlCommand.Parameters.AddWithValue("@villainId", currentVillainId);

        string result = Convert.ToString(await sqlCommand.ExecuteScalarAsync());

        if (!string.IsNullOrEmpty(result))
        {
            using SqlTransaction transaction = sqlConnection.BeginTransaction();

            try
            {
                using SqlCommand deleteFromMinionsVillains = new(SQLQueries.deleteFromMinionsVillains, sqlConnection, transaction);
                deleteFromMinionsVillains.Parameters.AddWithValue("@villainId", currentVillainId);
                int deletedMinionsCount = await deleteFromMinionsVillains.ExecuteNonQueryAsync();

                using SqlCommand deleteFromVillains = new(SQLQueries.deleteFromVillains, sqlConnection, transaction);
                deleteFromVillains.Parameters.AddWithValue("@villainId", currentVillainId);
                int deletedVillainCount = await deleteFromVillains.ExecuteNonQueryAsync();

                transaction.Commit();

                Console.WriteLine($"{result} was deleted.");
                Console.WriteLine($"{deletedMinionsCount} minions were released.");
            }
            catch (Exception)
            {
                transaction.Rollback();
                Console.WriteLine("An error occurred. No changes were made to the database.");
            }
        }

        else
        {
            Console.WriteLine("No such villain was found.");
        }
    }
}