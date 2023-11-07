//6 Exercise - Jagged Array Manipulator
int rows = int.Parse(Console.ReadLine());
int[][] jagged = new int[rows][];

for (int row = 0; row < jagged.Length; row++)
{
    int[] rowElements = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    jagged[row] = rowElements;
}

for (int row = 0; row < jagged.Length - 1; row++)
{
    if (jagged[row].Length == jagged[row + 1].Length)
    {
        int[] currentRow = jagged[row].ToArray();
        int[] rowPlusOne = jagged[row + 1].ToArray();

        for (int i = 0; i <= currentRow.Length - 1; i++)
        {
            currentRow[i] *= 2;
            rowPlusOne[i] *= 2;
        }

        jagged[row] = currentRow;
        jagged[row + 1] = rowPlusOne;
    }

    else if (jagged[row].Length != jagged[row + 1].Length)
    {
        int[] currentRow = jagged[row].ToArray();
        int[] rowPlusOne = jagged[row + 1].ToArray();

        for (int i = 0; i <= currentRow.Length - 1; i++)
        {
            currentRow[i] /= 2;
        }

        for (int i = 0; i <= rowPlusOne.Length - 1; i++)
        {
            rowPlusOne[i] /= 2;
        }

        jagged[row] = currentRow;
        jagged[row + 1] = rowPlusOne;
    }
}

string input = Console.ReadLine();

while (input != "End")
{
    string[] operationInfo = input
        .Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string operation = operationInfo[0];
    int currentRow = int.Parse(operationInfo[1]);
    int currentColumn = int.Parse(operationInfo[2]);
    int value = int.Parse(operationInfo[3]);

    if (operation == "Add")
    {
        if (currentRow >= 0 && currentRow < jagged.Length &&
            currentColumn >= 0 && currentColumn < jagged[currentRow].Length)
        {
            jagged[currentRow][currentColumn] += value;
        }
    }

    else if (operation == "Subtract")
    {
        if (currentRow >= 0 && currentRow < jagged.Length &&
               currentColumn >= 0 && currentColumn < jagged[currentRow].Length)
        {
            jagged[currentRow][currentColumn] -= value;
        }
    }

    input = Console.ReadLine();
}

foreach (int[] row in jagged)
{
    Console.WriteLine(string.Join(" ", row));
}
