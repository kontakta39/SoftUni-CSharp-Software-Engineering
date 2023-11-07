namespace IncreaseMinionAge;

public class SQLQueries
{
    public const string updateMinions = @"UPDATE Minions
                                        SET Name = LOWER(LEFT(Name, 1)) + SUBSTRING(Name, 2, LEN(Name)), Age += 1
                                        WHERE Id = @Id;";

    public const string selectMinions = @"SELECT [Name], Age FROM Minions;";
}