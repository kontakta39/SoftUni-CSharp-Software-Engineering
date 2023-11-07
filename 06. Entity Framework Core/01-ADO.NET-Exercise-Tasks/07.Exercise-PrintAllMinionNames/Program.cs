//7 Exercise - Print All Minion Names
using System.Data.SqlClient;

namespace PrintAllMinionNames;
public class Program
{
    const string connectionString = @"Server=ACER\SQLEXPRESS;Database=MinionsDB;Integrated Security=True";

    static async Task Main()
    {
        using SqlConnection sqlConnection = new(connectionString);
        sqlConnection.Open();
        using SqlCommand sqlCommand = new(@"SELECT [Name] FROM Minions;", sqlConnection);
        using SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

        List<string> allMinionsNames = new();

        while (await reader.ReadAsync())
        {
            allMinionsNames.Add(reader["Name"].ToString());
        }

        for (int i = 0; i < allMinionsNames.Count; i++)
        {
            Console.WriteLine(allMinionsNames[i]);

            if (allMinionsNames.Count > 1)
            {
                Console.WriteLine(allMinionsNames[allMinionsNames.Count - 1]);
                allMinionsNames.RemoveAt(i);
                allMinionsNames.RemoveAt(allMinionsNames.Count - 1);
            }

            else
            {
                allMinionsNames.RemoveAt(i);
            }

            i = -1;
        }
    }
}