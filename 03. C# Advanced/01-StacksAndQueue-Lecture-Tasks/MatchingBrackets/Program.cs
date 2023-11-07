string input = Console.ReadLine();
Stack<int> openingScopes = new();

for (int i = 0; i < input.Length; i++)
{
    if (input[i] == '(')
    {
        openingScopes.Push(i);
    }

    else if (input[i] == ')')
    {
        int currentOpeningScopeIndex = openingScopes.Pop();
        int currentClosingScopeIndex = i;
        int length = currentClosingScopeIndex - currentOpeningScopeIndex + 1;
        string result = input.Substring(currentOpeningScopeIndex, length);
        Console.WriteLine(result);
    }
}