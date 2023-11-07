//3 Exercise - Shopping Spree
using ShoppingSpree;

public class StartUp
{
    static void Main()
    {
        string[] delimeters = { "=", ";" };
        List<Person> people = new();
        string[] personInfo = Console.ReadLine()
            .Split(delimeters, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < personInfo.Length; i += 2)
        {
            string personName = personInfo[i];
            int money = int.Parse(personInfo[i + 1]);

            try
            {
                Person person = new(personName, money);
                people.Add(person);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        string[] productInfo = Console.ReadLine()
    .Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
        List<Product> products = new();

        for (int i = 0; i < productInfo.Length; i += 2)
        {
            string productName = productInfo[i];
            int cost = int.Parse(productInfo[i + 1]);

            try
            {
                Product product = new(productName, cost);
                products.Add(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Environment.Exit(0);
            }
        }

        string operation = Console.ReadLine();

        while (operation != "END")
        {
            string[] operationInfo = operation.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string currentPersonName = operationInfo[0];
            string currentProductName = operationInfo[1];

            Person currentPerson = people.Where(x => x.Name == currentPersonName).FirstOrDefault();
            Product currentProduct = products.Where(x => x.Name == currentProductName).FirstOrDefault();

            try
            {
                currentPerson.AddProduct(currentPerson, currentProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            operation = Console.ReadLine();
        }

        foreach (Person person in people)
        {
            Console.WriteLine(person.ToString());
        }
    }
}