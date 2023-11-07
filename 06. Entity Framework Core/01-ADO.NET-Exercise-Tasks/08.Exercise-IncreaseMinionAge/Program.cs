//8 Exercise - Increase Minion Age
using System.Data.SqlClient;

namespace IncreaseMinionAge;

public class Program
{
    const string connectionString = @"Server=ACER\SQLEXPRESS;Database=MinionsDB;Integrated Security=True";

    static async Task Main()
    {
        using SqlConnection connection = new(connectionString);
        connection.Open();

        int[] currentIds = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToArray();

        foreach (var item in currentIds)
        {
            using SqlCommand sqlCommand = new(SQLQueries.updateMinions, connection);
            sqlCommand.Parameters.AddWithValue("@id", item);
            int result = await sqlCommand.ExecuteNonQueryAsync();
        }

        using SqlCommand selectMinions = new(SQLQueries.selectMinions, connection);
        using SqlDataReader reader = await selectMinions.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Console.WriteLine($"{reader["Name"]} {reader["Age"]}");
        }
    }
}