//9 Exercise - Increase Age Stored Procedure 
using System.Data.SqlClient;

namespace IncreaseAgeStoredProcedure;

public class Program
{
    const string connectionString = @"Server=ACER\SQLEXPRESS;Database=MinionsDB;Integrated Security=True";

    static async Task Main()
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();
        int currentId = int.Parse(Console.ReadLine());
        using SqlCommand sqlCommand = new(SQLQueries.cmdText, connection);
        sqlCommand.Parameters.AddWithValue("@id", currentId);
        await sqlCommand.ExecuteNonQueryAsync();

        using SqlCommand selectMinions = new(SQLQueries.selectMinions, connection);
        selectMinions.Parameters.AddWithValue("@id", currentId);
        using SqlDataReader dataReader = await selectMinions.ExecuteReaderAsync();

        while (await dataReader.ReadAsync())
        {
            Console.WriteLine($"{dataReader["Name"]} - {dataReader["Age"]} years old");
        }
    }
}