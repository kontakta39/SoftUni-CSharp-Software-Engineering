namespace Zoo;

public class Bear : Mammal
{
    public Bear(string name) : base(name)
    {
    }

    public override string Name { get; set; }
}