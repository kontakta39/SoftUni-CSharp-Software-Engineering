//6 Exercise - Jagged-Array Modification
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

string input = Console.ReadLine();

while (input != "END")
{
    string[] elementsInfo = input
    .Split(" ", StringSplitOptions.RemoveEmptyEntries);
    string operation = elementsInfo[0];
    int rowNumber = int.Parse(elementsInfo[1]);
    int colNumber = int.Parse(elementsInfo[2]);
    int value = int.Parse(elementsInfo[3]);

    if (operation == "Add")
    {
        if (rowNumber >= 0 && rowNumber < jagged.Length && 
            colNumber >= 0 && colNumber < jagged[rowNumber].Length)
        {
            jagged[rowNumber][colNumber] += value;
        }

        else
        {
            Console.WriteLine("Invalid coordinates");
        }
    }

    else if (operation == "Subtract")
    {
        if (rowNumber >= 0 && rowNumber < jagged.Length &&
             colNumber >= 0 && colNumber < jagged[rowNumber].Length)
        {
            jagged[rowNumber][colNumber] -= value;
        }

        else
        {
            Console.WriteLine("Invalid coordinates");
        }
    }

    input = Console.ReadLine();
}

foreach (int[] row in jagged)
{
    Console.WriteLine(string.Join(" ", row));
}