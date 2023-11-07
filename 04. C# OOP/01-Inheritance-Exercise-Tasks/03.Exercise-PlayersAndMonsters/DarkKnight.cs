namespace PlayersAndMonsters;

public abstract class DarkKnight : Knight
{
    public DarkKnight(string username, int level) : base(username, level)
    {
    }

    public override string Username { get; set; }
    public override int Level { get; set; }
}