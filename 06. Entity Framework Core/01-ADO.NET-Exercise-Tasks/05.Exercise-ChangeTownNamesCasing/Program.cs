//5 Exercise - Change Town Names Casing
using System.Data.SqlClient;

namespace ChangeTownNamesCasing;

public class Program
{
    const string connectionString = @"Server=ACER\SQLEXPRESS;Database=MinionsDB;Integrated Security=True";

    static async Task Main()
    {
        using SqlConnection sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();
        string currentCountry = Console.ReadLine();
        using SqlCommand sqlCommand = new SqlCommand(SQLQueries.ChangeTownNames, sqlConnection);
        sqlCommand.Parameters.AddWithValue("@countryName", currentCountry);

        int result = Convert.ToInt32(await sqlCommand.ExecuteNonQueryAsync());

        if (result != 0)
        {
            Console.WriteLine($"{result} town names were affected.");

            using SqlCommand getTownNames = new SqlCommand(SQLQueries.GetTownNames, sqlConnection);
            getTownNames.Parameters.AddWithValue("@countryName", currentCountry);
            using SqlDataReader dataReader = await getTownNames.ExecuteReaderAsync();

            List<string> townNames = new List<string>();

            while (await dataReader.ReadAsync())
            {
                townNames.Add(dataReader["Name"].ToString());
            }

            Console.WriteLine($"[{string.Join(", ", townNames)}]");
        }

        else
        {
            Console.WriteLine("No town names were affected.");
        }
    }
}