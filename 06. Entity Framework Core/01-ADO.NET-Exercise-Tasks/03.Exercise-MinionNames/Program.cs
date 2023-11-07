//3 Exercise - Minion Names
using System.Data.SqlClient;

namespace MinionNames;
public class Program
{
    const string connectionString = @"Server=ACER\SQLEXPRESS;Database=MinionsDB;Integrated Security=True";

    static async Task Main()
    {
        using SqlConnection sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();
        int currentId = int.Parse(Console.ReadLine());
        using SqlCommand sqlCommand = new SqlCommand(SQLQueries.IdCheck, sqlConnection);
        sqlCommand.Parameters.AddWithValue("@Id", currentId);
        string result = Convert.ToString(await sqlCommand.ExecuteScalarAsync());

        if (!string.IsNullOrWhiteSpace(result))
        {
            Console.WriteLine($"Villain: {result}");

            using SqlCommand getMinionsNames = new SqlCommand(SQLQueries.AllMinionsNames, sqlConnection);
            getMinionsNames.Parameters.AddWithValue("Id", currentId);
            using SqlDataReader dataReader = await getMinionsNames.ExecuteReaderAsync();

            if (dataReader.HasRows)
            {
                while (await dataReader.ReadAsync())
                {
                    Console.WriteLine($"{dataReader["RowNum"]}. {dataReader["Name"]} {dataReader["Age"]}");
                }
            }

            else
            {
                Console.WriteLine("(no minions)");
            }
        }

        else
        {
            Console.WriteLine($"No villain with ID {currentId} exists in the database."); ;
        }
    }
}