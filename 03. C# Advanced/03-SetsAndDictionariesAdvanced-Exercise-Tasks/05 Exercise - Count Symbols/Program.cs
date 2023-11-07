//5 Exercise - Count Symbols
string text = Console.ReadLine();
char[] textSymbols = text.ToCharArray();
SortedDictionary<char, int> symbols = new();

for (int i = 0; i < textSymbols.Length; i++)
{
    char currentSymbol = textSymbols[i];

    if (!symbols.ContainsKey(currentSymbol))
    {
        symbols.Add(currentSymbol, 1);
    }

    else
    {
        symbols[currentSymbol]++;
    }
}

foreach (var item in symbols)
{
    Console.WriteLine($"{item.Key}: {item.Value} time/s");
}

