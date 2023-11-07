using System.Text;

namespace ShoppingSpree;

public class Person
{
    private string name;
    private int money;
    private List<Product> productsBag;

    public Person(string name, int money)
    {
        Name = name;
        Money = money;
        productsBag = new();
    }

    public string Name
    {
        get => name;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{nameof(Name)} cannot be empty");
            }

            name = value;
        }
    }
    public int Money
    {
        get => money;
        private set
        {
            if (value < 0)
            {
                throw new ArgumentException($"{nameof(Money)} cannot be negative");
            }

            money = value;
        }
    }

    public void AddProduct(Person currentPerson, Product currentProduct)
    {
        if (currentPerson.Money >= currentProduct.Cost)
        {
            currentPerson.Money -= currentProduct.Cost;
            productsBag.Add(currentProduct);
            Console.WriteLine($"{currentPerson.Name} bought {currentProduct.Name}");
        }

        else
        {
            throw new ArgumentException($"{currentPerson.Name} can't afford {currentProduct.Name}");
        }
    }

    public override string ToString()
    {
        if (productsBag.Count > 0)
        {
            StringBuilder sb = new();
            productsBag = productsBag.OrderBy(p => p.Name).ToList();
            sb.Append($"{Name} - ");
            for (int i = 0; i < productsBag.Count; i++)
            {
                if (i == productsBag.Count - 1)
                {
                    sb.Append($"{productsBag[i].Name}");
                }

                else
                {
                    sb.Append($"{productsBag[i].Name}, ");
                }
            }

            return sb.ToString();
        }

        else
        {
            return $"{Name} - Nothing bought".ToString();
        }
    }
}