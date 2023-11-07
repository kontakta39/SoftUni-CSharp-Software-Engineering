namespace Animals;

public class Kitten : Cat
{
    private const string GenderCat = "Female";

    public Kitten(string name, int age) : base(name, age, GenderCat)
    {
    }

    public override string ProduceSound() => "Meow";
}