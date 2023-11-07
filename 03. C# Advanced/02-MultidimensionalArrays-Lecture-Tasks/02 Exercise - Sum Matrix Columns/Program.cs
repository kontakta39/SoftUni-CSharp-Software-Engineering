//2 Exercise - Sum Matrix Columns
int[] dimensionsInfo = Console.ReadLine()
    .Split(", ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
int sumAllCells = 0;
int rowsNumber = dimensionsInfo[0];
int colsNumber = dimensionsInfo[1];
int[,] matrix = new int[rowsNumber, colsNumber];

for (int row = 0; row < rowsNumber; row++)
{
    int[] colElements = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    for (int col = 0; col < colsNumber; col++)
    {
        matrix[row, col] = colElements[col];
    }
}

for (int col = 0; col < colsNumber; col++)
{
    int sum = 0;

    for (int row = 0; row < rowsNumber; row++)
    {
        //Console.Write(matrix[row, col] + " ");
        sum += matrix[row, col];
    }

    Console.WriteLine(sum);
}

