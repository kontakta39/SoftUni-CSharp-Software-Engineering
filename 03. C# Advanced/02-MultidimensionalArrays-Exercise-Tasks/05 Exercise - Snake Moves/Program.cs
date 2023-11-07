//5 Exercise - Snake Moves
int[] dimensionsInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
string input = Console.ReadLine();
int rowsNumber = dimensionsInfo[0];
int columnsNumber = dimensionsInfo[1];
char[,] matrix = new char[rowsNumber, columnsNumber];
List<char> chars = new();
int capacity = rowsNumber * columnsNumber;
int currentCharIndex = 0;

while (currentCharIndex <= capacity)
{
    for (int i = 0; i < input.Length; i++)
    {
        chars.Add(input[i]);
        currentCharIndex++;
    }
}

for (int row = 0; row < matrix.GetLength(0); row++)
{
    int currentColumn = 0;

    if (row % 2 == 0)
    {
        for (int col = 0; col < matrix.GetLength(1); col++)
        {
            matrix[row, currentColumn] = chars[col];
            chars.RemoveAt(col);
            col = -1;
            currentColumn++;

            if (currentColumn == columnsNumber)
            {
                currentColumn = 0;
                break;
            }
        }
    }

    else if (row % 2 != 0)
    {
        int firstElement = 0;
        int ElementPosition = 0;

        for (int col = matrix.GetLength(1) - 1; col >= 0; col--)
        {
            matrix[row, col] = chars[firstElement];
            chars.RemoveAt(firstElement);
            ElementPosition++;

            if (ElementPosition == columnsNumber)
            {
                break;
            }
        }
    }
}

for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        Console.Write(matrix[row, col]);
    }

    Console.WriteLine();
}