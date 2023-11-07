//2 Exercise - Squares in Matrix
int[] dimensionsInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
int rowsNumber = dimensionsInfo[0];
int columnsNumber = dimensionsInfo[1];
char[,] matrix = new char[rowsNumber, columnsNumber];
int squaresCount = 0;

for (int row = 0; row < matrix.GetLength(0); row++)
{
    char[] colNumbers = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(char.Parse)
        .ToArray();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = colNumbers[col];
    }
}

for (int row = 0; row < matrix.GetLength(0) - 1; row++)
{
    for (int col = 0; col < matrix.GetLength(1) - 1; col++)
    {
        if (matrix[row, col] == matrix[row, col + 1]
            && matrix[row, col] == matrix[row + 1, col + 1]
            && matrix[row, col] == matrix[row + 1, col])
        {
            squaresCount++;
        }
    }
}

Console.WriteLine(squaresCount);
