//4 Exercise - Product Shop
string input = Console.ReadLine();
Dictionary<string, Dictionary<string, double>> shops = new();

while (input != "Revision")
{
    string[] shopInfo = input.Split(", ");
    string shopName = shopInfo[0];
    string productName = shopInfo[1];
    double price = double.Parse(shopInfo[2]);

    if (!shops.ContainsKey(shopName))
    {
        shops.Add(shopName, new Dictionary<string, double>());
        shops[shopName].Add(productName, price);
    }

    else
    {
        shops[shopName].Add(productName, price);
    }

    input = Console.ReadLine(); 
}

shops = shops.OrderBy(s => s.Key).ToDictionary(x => x.Key, x => x.Value);

foreach (var (shopName, productName)  in shops)
{
    Console.WriteLine($"{shopName}->");

    foreach (var (product, price) in productName)
    {
        Console.WriteLine($"Product: {product}, Price: {price}");
    }
}

