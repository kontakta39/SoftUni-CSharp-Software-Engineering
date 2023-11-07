//4 Exercise - Add Minion
using System.Data.SqlClient;

namespace AddMinion;

public class Program
{
    const string connectionString = @"Server=ACER\SQLEXPRESS;Database=MinionsDB;Integrated Security=True";

    static async Task Main()
    {
        using SqlConnection sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();

        try
        {
            string[] delimiters = new string[] { ": ", " " };
            string[] minionInfo = Console.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string minionName = minionInfo[1];
            int minionAge = int.Parse(minionInfo[2]);
            string minionTown = minionInfo[3];
            string[] villainInfo = Console.ReadLine().Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            string villainName = villainInfo[1];

            int townId = await GetOrCreateTownId(sqlConnection, minionTown);
            int villainId = await GetOrCreateVillainId(sqlConnection, villainName);
            int minionId = await AddMinionToDatabase(sqlConnection, minionName, minionAge, townId);
            await MakingMinionServantToVillain(sqlConnection, minionId, villainId);

            Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    static async Task<int> GetOrCreateTownId(SqlConnection connection, string townName)
    {
        using SqlCommand command = new SqlCommand(SQLQueries.selectIdFromTowns, connection);
        command.Parameters.AddWithValue("@townName", townName);
        object result = await command.ExecuteScalarAsync();
        int townId = result == null ? await CreateTown(connection, townName) : (int)result;

        return townId;
    }

    static async Task<int> CreateTown(SqlConnection connection, string townName)
    {
        using SqlCommand command = new SqlCommand(SQLQueries.insertIntoTowns + "; SELECT SCOPE_IDENTITY();", connection);
        command.Parameters.AddWithValue("@townName", townName);
        int townId = Convert.ToInt32(await command.ExecuteScalarAsync());
        Console.WriteLine($"Town {townName} was added to the database.");

        return Convert.ToInt32(townId);
    }

    static async Task<int> GetOrCreateVillainId(SqlConnection connection, string villainName)
    {
        using SqlCommand command = new SqlCommand(SQLQueries.selectVillianId, connection);
        command.Parameters.AddWithValue("@Name", villainName);
        object result = await command.ExecuteScalarAsync();
        int villainId = result == null ? await CreateVillain(connection, villainName) : (int)result;
        return villainId;
    }

    static async Task<int> CreateVillain(SqlConnection connection, string villainName)
    {
        using SqlCommand command = new SqlCommand(SQLQueries.insertIntoVillians + "; SELECT SCOPE_IDENTITY();", connection);
        command.Parameters.AddWithValue("@villainName", villainName);
        int villainId = Convert.ToInt32(await command.ExecuteScalarAsync());
        Console.WriteLine($"Villain {villainName} was added to the database.");

        return villainId;
    }

    static async Task<int> AddMinionToDatabase(SqlConnection connection, string minionName, int minionAge, int townId)
    {
        using SqlCommand command = new SqlCommand(SQLQueries.insertIntoMinions + "; SELECT SCOPE_IDENTITY();", connection);
        command.Parameters.AddWithValue("@name", minionName);
        command.Parameters.AddWithValue("@age", minionAge);
        command.Parameters.AddWithValue("@townId", townId);
        int minionId = Convert.ToInt32(await command.ExecuteScalarAsync());

        return minionId;
    }

    static async Task MakingMinionServantToVillain(SqlConnection connection, int minionId, int villainId)
    {
        using SqlCommand minionVillainCommand = new SqlCommand(SQLQueries.insertIntoMinionsVillians, connection);
        minionVillainCommand.Parameters.AddWithValue("@minionId", minionId);
        minionVillainCommand.Parameters.AddWithValue("@villainId", villainId);
        await minionVillainCommand.ExecuteNonQueryAsync();
    }
}