namespace Animals;

public class Cat : Animal
{
    public Cat(string name, string favouriteFood) : base(name, favouriteFood)
    {
    }

    public override string ExplainSelf()
    {
        return $"I am {Name} and my favourite food is {FavouriteFood}{Environment.NewLine}MEEOW";
    }
}