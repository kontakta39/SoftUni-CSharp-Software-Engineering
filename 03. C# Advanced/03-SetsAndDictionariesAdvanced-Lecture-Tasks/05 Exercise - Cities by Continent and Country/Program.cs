//5 Exercise - Cities by Continent and Country
int countriesCount = int.Parse(Console.ReadLine());
Dictionary<string, Dictionary<string, List<string>>> countries = new();

for (int i = 0; i < countriesCount; i++)
{
    string[] continentInfo = Console.ReadLine().Split(" ");
    string continentName = continentInfo[0];
    string countryName = continentInfo[1];
    string cityName = continentInfo[2];

    if (!countries.ContainsKey(continentName))
    {
        countries.Add(continentName, new Dictionary<string, List<string>>());
        countries[continentName].Add(countryName, new List<string>());
        countries[continentName][countryName].Add(cityName);
    }

    else if (!countries[continentName].ContainsKey(countryName))
    {
        countries[continentName].Add(countryName, new List<string>());
        countries[continentName][countryName].Add(cityName);
    }

    else
    {
        countries[continentName][countryName].Add(cityName);
    }
}

foreach (var (continentName, countryName) in countries)
{
    Console.WriteLine($"{continentName}:");

    foreach (var (country, city) in countryName)
    {
        Console.WriteLine($"{country} -> {string.Join(", ", city)}");
    }
}
