//1 Exercise - Initial Setup
//Creating database in SQL Management Studio by executing the script

// 2 Exercise - Villain Names
using System.Data.SqlClient;

namespace VillainNames;

public class Program
{
    const string connectionString = @"Server=ACER\SQLEXPRESS;Database=MinionsDB;Integrated Security=True";

    static async Task Main()
    {
        using SqlConnection sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new SqlCommand(SQLQueries.getVillainsNamesWithMinionsNumber, sqlConnection);
        using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

        while (await reader.ReadAsync()) 
        {
            Console.WriteLine($"{reader["Name"]} - {reader["MinionsCount"]}");
        }
    }
}