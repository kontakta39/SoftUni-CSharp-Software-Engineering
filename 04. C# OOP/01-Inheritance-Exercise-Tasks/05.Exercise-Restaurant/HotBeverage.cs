namespace Restaurant;

public abstract class HotBeverage : Beverage
{
    public HotBeverage(string name, decimal price, double mililiters) : base(name, price, mililiters)
    {
    }
}