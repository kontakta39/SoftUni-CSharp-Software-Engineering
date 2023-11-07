//3 Exercise - Maximal Sum
int[] dimensionsInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
int rowsNumber = dimensionsInfo[0];
int columnsNumber = dimensionsInfo[1];
int[,] matrix = new int[rowsNumber, columnsNumber];
int currentSum = 0;
int maxSum = int.MinValue;
int startingRow = 0, startingColumn = 0;

for (int row = 0; row < matrix.GetLength(0); row++)
{
    int[] colNumbers = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = colNumbers[col];
    }
}

for (int row = 0; row < matrix.GetLength(0) - 2; row++)
{
    for (int col = 0; col < matrix.GetLength(1) - 2; col++)
    {
        currentSum = (matrix[row, col] + matrix[row, col + 1] + matrix[row, col + 2]) +
            (matrix[row + 1, col] + matrix[row + 1, col + 1] + matrix[row + 1, col + 2]) +
            (matrix[row + 2, col] + matrix[row + 2, col + 1] + matrix[row + 2, col + 2]);

        if (currentSum > maxSum)
        {
            maxSum = currentSum;
            startingRow = row;
            startingColumn= col;
        }
    }
}

Console.WriteLine($"Sum = {maxSum}");

for (int row = startingRow; row < startingRow + 3; row++)
{
    for (int col = startingColumn; col < startingColumn + 3; col++)
    {
        Console.Write($"{matrix[row, col]} ");
    }

    Console.WriteLine();
}