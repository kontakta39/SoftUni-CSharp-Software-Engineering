namespace AddMinion;

public class SQLQueries
{
    public const string selectVillianId = @"SELECT Id 
                                            FROM Villains 
                                            WHERE [Name] = @Name;";

    public const string selectMinionId = @"SELECT Id 
                                        FROM Minions 
                                        WHERE [Name] = @Name;";

    public const string insertIntoMinionsVillians = @"INSERT INTO 
                                                    MinionsVillains (MinionId, VillainId) 
                                                    VALUES (@minionId, @villainId)";

    public const string insertIntoVillians = @"INSERT INTO 
                                                Villains ([Name], EvilnessFactorId) 
                                                VALUES (@villainName, 4)";

    public const string insertIntoMinions = @"INSERT INTO
                                            Minions ([Name], Age, TownId) 
                                            VALUES (@name, @age, @townId)";

    public const string insertIntoTowns = @"INSERT INTO 
                                            Towns ([Name]) VALUES (@townName)";

    public const string selectIdFromTowns = @"SELECT Id 
                                            FROM Towns 
                                            WHERE [Name] = @townName;";
}