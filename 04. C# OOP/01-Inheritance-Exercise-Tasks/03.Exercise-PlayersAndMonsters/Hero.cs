namespace PlayersAndMonsters;

public abstract class Hero
{
    public Hero(string username, int level)
    {
        Username = username;
        Level = level;
    }

    public virtual string Username { get; set; }
    public virtual int Level { get; set; }

    public override string ToString()
    {
        return $"Type: {GetType().Name} Username: {Username} Level: {Level}".ToString();
    }
}