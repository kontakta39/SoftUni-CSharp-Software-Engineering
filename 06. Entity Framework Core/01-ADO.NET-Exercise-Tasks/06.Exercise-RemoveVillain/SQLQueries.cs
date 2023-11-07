namespace RemoveVillain;

public class SQLQueries
{
    public const string selectVillains= @"SELECT [Name] 
                                        FROM Villains 
                                        WHERE Id = @villainId;";

    public const string deleteFromMinionsVillains = @"DELETE FROM MinionsVillains 
                                                     WHERE VillainId = @villainId;";

    public const string deleteFromVillains = @"DELETE FROM Villains
                                               WHERE Id = @villainId;";
}