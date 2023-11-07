namespace PizzaCalories.Models;

public class Topping
{
    private static readonly Dictionary<string, double> toppingTypes = new()
    {
      {"Meat", 1.2}, {"Veggies", 0.8},
      {"Cheese", 1.1 }, {"Sauce", 0.9}
    };

    private string topping;
    private double grams;

    public Topping(string topping, double grams)
    {
        ToppingType = topping;
        Grams = grams;
    }

    public string ToppingType
    {
        get => topping;
        private set
        {
            if (!toppingTypes.ContainsKey(value))
            {
                throw new ArgumentException($"Cannot place {value} on top of your pizza.");
            }

            topping = value;
        }
    }
    public double Grams
    {
        get => grams;
        private set
        {
            if (value < 1 || value > 50)
            {
                throw new ArgumentException($"{ToppingType} weight should be in the range [1..50].");
            }

            grams = value;
        }
    }

    public double ToppingTotalCalories()
    {
        double toppingTypeModifier = 0;

        foreach (var item in toppingTypes)
        {
            if (item.Key == topping)
            {
                toppingTypeModifier = item.Value;
                break;
            }
        }

        return 2 * toppingTypeModifier * grams;
    }
}