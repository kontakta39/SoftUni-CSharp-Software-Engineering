namespace Zoo;

public class Lizard : Reptile
{
    public Lizard(string name) : base(name)
    {
    }

    public override string Name { get; set; }
}