namespace PlayersAndMonsters;

public abstract class DarkWizard : Wizard
{
    public DarkWizard(string username, int level) : base(username, level)
    {
    }

    public override string Username { get; set; }
    public override int Level { get; set; }
}