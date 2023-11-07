namespace IncreaseAgeStoredProcedure;

public class SQLQueries
{
    public const string getOlderProcedure = @"CREATE PROC dbo.usp_GetOlder @id INT
                                            AS
                                            UPDATE Minions
                                                SET Age += 1
                                                WHERE Id = @id;";

    public const string cmdText = @"EXEC dbo.usp_GetOlder @id";

    public const string selectMinions = @"SELECT [Name], Age 
                                        FROM Minions 
                                        WHERE Id = @Id;";
}