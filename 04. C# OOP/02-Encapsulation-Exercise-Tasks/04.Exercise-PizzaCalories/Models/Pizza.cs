namespace PizzaCalories.Models;

public class Pizza
{
    private List<Topping> toppings;
    private string name;
    private double currentDoughCalories;
    private double currentToppingCalories;

    public Pizza(string name, Dough dough)
    {
        Name = name;
        Dough = dough;
        toppings = new();
    }

    public string Name
    {
        get => name;
        private set
        {
            if (value.Length < 1 || value.Length > 15)
            {
                throw new ArgumentException("Pizza name should be between 1 and 15 symbols.");
            }

            name = value;
        }
    }
    public int Count { get => toppings.Count; }

    public double TotalCalories { get => currentDoughCalories + currentToppingCalories; }

    public Dough Dough { get; private set; }

    public void AddDough(double doughCalories)
    {
        currentDoughCalories = doughCalories;
    }

    public void AddTopping(Topping topping, double toppingCalories)
    {
        if (Count <= 10)
        {
            toppings.Add(topping);
            currentToppingCalories += toppingCalories;
        }

        else
        {
            throw new ArgumentException("Number of toppings should be in range [0..10].");
        }
    }

    public override string ToString()

        => $"{Name} - {TotalCalories:f2} Calories.".ToString();

}