namespace Animals;

public class Tomcat : Cat
{
    private const string GenderCat = "Male";

    public Tomcat(string name, int age) : base(name, age, GenderCat)
    {
    }

    public override string ProduceSound() => "MEOW";
}