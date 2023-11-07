int dimensions = int.Parse(Console.ReadLine());
int sumCells = 0;
int[,] matrix = new int[dimensions, dimensions];

for (int row = 0; row < matrix.GetLength(0); row++)
{
    int[] colElements = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = colElements[col];
    }
}

for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col <matrix.GetLength(1); col++)
    {
        if (row == col)
        {
            sumCells += matrix[row, col];
        }
    }
}

Console.WriteLine(sumCells);