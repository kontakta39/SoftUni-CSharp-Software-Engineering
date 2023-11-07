namespace PlayersAndMonsters;

public abstract class Elf : Hero
{
    public Elf(string username, int level) : base(username, level)
    {
    }

    public override string Username { get; set; }
    public override int Level { get; set; }
}