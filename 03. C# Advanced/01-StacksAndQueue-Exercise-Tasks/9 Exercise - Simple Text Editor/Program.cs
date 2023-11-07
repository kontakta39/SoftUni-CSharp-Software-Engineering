//9 Exercise - Simple Text Editor
int operationsCount = int.Parse(Console.ReadLine());
string text = string.Empty;
Stack<string> madeOperations = new();

for (int i = 0; i < operationsCount; i++)
{
    string[] operationInfo = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries);
    int operationNumber = int.Parse(operationInfo[0]);

    if (operationNumber == 1)
    {
        string currentString = operationInfo[1];
        madeOperations.Push(text);
        text += currentString;
    }

    else if (operationNumber == 2)
    {
        int count = int.Parse(operationInfo[1]);
        madeOperations.Push(text);
        int textLength = text.Length;
        int lengthWithRemovedSymbols = textLength - count;
        text = text.Remove(lengthWithRemovedSymbols, count);
    }

    else if (operationNumber == 3)
    {
        int index = int.Parse(operationInfo[1]) - 1;

        for (int j = 0; j < text.Length; j++)
        {
            if (j == index)
            {
                Console.WriteLine(text[j]);
                break;
            }
        }
    }

    else if (operationNumber == 4)
    {
        string lastMadeOperation = madeOperations.Pop();
        text = lastMadeOperation;
    }
}