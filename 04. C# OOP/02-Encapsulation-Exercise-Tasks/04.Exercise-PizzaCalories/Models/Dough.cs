namespace PizzaCalories.Models;

public class Dough
{
    private static readonly Dictionary<string, double> flourTypes = new()
    {
      {"White", 1.5}, {"Wholegrain", 1.0}
    };

    private static readonly Dictionary<string, double> bakingTechniques = new()
    {
      {"Crispy", 0.9}, {"Chewy", 1.1}, {"Homemade", 1.0}
    };

    private string flourType;
    private string bakingTechnique;
    private double grams;

    public Dough(string flourType, string bakingTechnique, double grams)
    {
        FlourType = flourType;
        BakingTechnique = bakingTechnique;
        Grams = grams;
    }

    public string FlourType
    {
        get => flourType;
        private set
        {
            if (!flourTypes.ContainsKey(value))
            {
                throw new ArgumentException("Invalid type of dough.");
            }

            flourType = value;
        }
    }
    public string BakingTechnique
    {
        get => bakingTechnique;
        private set
        {
            if (!bakingTechniques.ContainsKey(value))
            {
                throw new ArgumentException("Invalid type of dough.");
            }

            bakingTechnique = value;
        }
    }
    public double Grams
    {
        get => grams;
        private set
        {
            if (value < 1 || value > 200)
            {
                throw new ArgumentException("Dough weight should be in the range [1..200].");
            }

            grams = value;
        }
    }

    public double DoughTotalCalories()
    {
        double flourTypeModifier = 0;

        foreach (var item in flourTypes)
        {
            if (item.Key == flourType)
            {
                flourTypeModifier = item.Value;
                break;
            }
        }

        double bakingTechniqueModifier = 0;

        foreach (var item in bakingTechniques)
        {
            if (item.Key == bakingTechnique)
            {
                bakingTechniqueModifier = item.Value;
                break;
            }
        }

        return 2 * grams * flourTypeModifier * bakingTechniqueModifier;
    }
}