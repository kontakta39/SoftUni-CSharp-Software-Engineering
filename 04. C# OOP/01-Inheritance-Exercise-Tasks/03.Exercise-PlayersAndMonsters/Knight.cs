namespace PlayersAndMonsters;

public abstract class Knight : Hero
{
    public Knight(string username, int level) : base(username, level)
    {
    }

    public override string Username { get; set; }
    public override int Level { get; set; }
}