namespace Zoo;

public abstract class Mammal : Animal
{
    public Mammal(string name) : base(name)
    {
    }

    public override string Name { get; set; }
}