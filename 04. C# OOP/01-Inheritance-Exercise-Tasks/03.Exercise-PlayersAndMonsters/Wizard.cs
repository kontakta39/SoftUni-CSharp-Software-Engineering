namespace PlayersAndMonsters;

public abstract class Wizard : Hero
{
    public Wizard(string username, int level) : base(username, level)
    {
    }

    public override string Username { get; set; }
    public override int Level { get; set; }
}