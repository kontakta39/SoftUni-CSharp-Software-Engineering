//4 Exercise - Pizza Calories
using PizzaCalories.Models;

public class StartUp
{
    static void Main()
    {
        Pizza pizza = null;
        double currentDoughCalories = 0;
        double currentToppingCalories = 0;

        string[] pizzaInfo = Console.ReadLine().Split();
        string pizzaName = pizzaInfo[1];

        string[] doughInfo = Console.ReadLine()
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToArray();

        string flourType = doughInfo[0];
        string currentFlourType = char.ToUpper(flourType[0]) + flourType.Substring(1).ToLower();
        string bakingTechnique = doughInfo[1];
        string currentBakingTechnique = char.ToUpper(bakingTechnique[0]) + bakingTechnique.Substring(1).ToLower();
        double doughGrams = double.Parse(doughInfo[2]);

        try
        {
            Dough dough = new(currentFlourType, currentBakingTechnique, doughGrams);
            currentDoughCalories = dough.DoughTotalCalories();

            try
            {
                pizza = new(pizzaName, dough);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(0);
        }

        if (pizza != null)
        {
            pizza.AddDough(currentDoughCalories);
        }

        string toppingInput = Console.ReadLine();

        while (toppingInput != "END")
        {
            string[] toppingInfo = toppingInput
            .Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToArray();
            string toppingType = toppingInfo[0];
            string currentToppingType = char.ToUpper(toppingType[0]) + toppingType.Substring(1).ToLower();
            double toppingGrams = double.Parse(toppingInfo[1]);

            try
            {
                Topping topping = new(currentToppingType, toppingGrams);
                currentToppingCalories = topping.ToppingTotalCalories();

                if (pizza != null)
                {
                    try
                    {
                        pizza.AddTopping(topping, currentToppingCalories);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Environment.Exit(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }

            toppingInput = Console.ReadLine();
        }

        Console.WriteLine(pizza);
    }
}