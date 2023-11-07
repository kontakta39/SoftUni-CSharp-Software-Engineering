//8 Exercise - Balanced Parentheses
string input = Console.ReadLine();
Stack<char> openingScopes = new();
bool isBalanced = false;

for (int i = 0; i < input.Length; i++)
{
    if (openingScopes.Count == 0)
    {
        openingScopes.Push(input[i]);
    }

    else if (input[i] == '(' || input[i] == '{' || input[i] == '[')
    {
        openingScopes.Push(input[i]);
    }

    else if (input[i] == ')' || input[i] == '}' || input[i] == ']')
    {
        char currentStackScope = openingScopes.Pop();

        if (currentStackScope == '(' && input[i] == ')')
        {
            isBalanced = true;
        }

        else if (currentStackScope == '[' && input[i] == ']')
        {
            isBalanced = true;
        }

        else if (currentStackScope == '{' && input[i] == '}')
        {
            isBalanced = true;
        }

        else
        {
            isBalanced = false;
            break;
        }
    }
}

if (openingScopes.Count == 0 && isBalanced)
{
    Console.WriteLine("YES");
}

else
{
    Console.WriteLine("NO");
}