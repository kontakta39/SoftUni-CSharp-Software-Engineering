//8 Exercise - Miner
int size = int.Parse(Console.ReadLine());
int[,] matrix = new int[size, size];

for (int row = 0; row < matrix.GetLength(0); row++)
{
    int[] cellsInfo = Console.ReadLine()
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        matrix[row, col] = cellsInfo[col];
    }
}

string[] bombCells = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries);

int currentBombRow = 0;
int currentBombCol = 0;

for (int i = 0; i < bombCells.Length; i++)
{
    int[] currentBombCellsInfo = bombCells[i]
        .Split(",", StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .ToArray();

    currentBombRow = currentBombCellsInfo[0];
    currentBombCol = currentBombCellsInfo[1];

    if (matrix[currentBombRow, currentBombCol] <= 0)
    {
        continue;
    }

    //left
    int leftCellRow = currentBombRow;
    int leftCellCol = currentBombCol - 1;
    bool isInTheMatrix = MatrixCheck(leftCellRow, leftCellCol);

    if (isInTheMatrix && matrix[leftCellRow, leftCellCol] > 0)
    {
        matrix[leftCellRow, leftCellCol] -= matrix[currentBombRow, currentBombCol];
    }

    //right
    int rightCellRow = currentBombRow;
    int rightCellCol = currentBombCol + 1;
    isInTheMatrix = MatrixCheck(rightCellRow, rightCellCol);

    if (isInTheMatrix && matrix[rightCellRow, rightCellCol] > 0)
    {
        matrix[rightCellRow, rightCellCol] -= matrix[currentBombRow, currentBombCol];
    }

    //up
    int upCellRow = currentBombRow - 1;
    int upCellCol = currentBombCol;
    isInTheMatrix = MatrixCheck(upCellRow, upCellCol);

    if (isInTheMatrix && matrix[upCellRow, upCellCol] > 0)
    {
        matrix[upCellRow, upCellCol] -= matrix[currentBombRow, currentBombCol];
    }

    //down
    int downCellRow = currentBombRow + 1;
    int downCellCol = currentBombCol;
    isInTheMatrix = MatrixCheck(downCellRow, downCellCol);

    if (isInTheMatrix && matrix[downCellRow, downCellCol] > 0)
    {
        matrix[downCellRow, downCellCol] -= matrix[currentBombRow, currentBombCol];
    }

    //upper left
    int upperLeftCellRow = currentBombRow - 1;
    int upperLeftCellCol = currentBombCol - 1;
    isInTheMatrix = MatrixCheck(upperLeftCellRow, upperLeftCellCol);

    if (isInTheMatrix && matrix[upperLeftCellRow, upperLeftCellCol] > 0)
    {
        matrix[upperLeftCellRow, upperLeftCellCol] -= matrix[currentBombRow, currentBombCol];
    }

    //upper right
    int upperRightCellRow = currentBombRow - 1;
    int upperRightCellCol = currentBombCol + 1;
    isInTheMatrix = MatrixCheck(upperRightCellRow, upperRightCellCol);

    if (isInTheMatrix && matrix[upperRightCellRow, upperRightCellCol] > 0)
    {
        matrix[upperRightCellRow, upperRightCellCol] -= matrix[currentBombRow, currentBombCol];
    }

    //lower left
    int lowerLeftCellRow = currentBombRow + 1;
    int lowerLeftCellCol = currentBombCol - 1;
    isInTheMatrix = MatrixCheck(lowerLeftCellRow, lowerLeftCellCol);

    if (isInTheMatrix && matrix[lowerLeftCellRow, lowerLeftCellCol] > 0)
    {
        matrix[lowerLeftCellRow, lowerLeftCellCol] -= matrix[currentBombRow, currentBombCol];
    }

    //lower right
    int lowerRightCellRow = currentBombRow + 1;
    int lowerRightCellCol = currentBombCol + 1;
    isInTheMatrix = MatrixCheck(lowerRightCellRow, lowerRightCellCol);

    if (isInTheMatrix && matrix[lowerRightCellRow, lowerRightCellCol] > 0)
    {
        matrix[lowerRightCellRow, lowerRightCellCol] -= matrix[currentBombRow, currentBombCol];
    }

    matrix[currentBombRow, currentBombCol] = 0;
}

int aliveCells = 0;
int sum = 0;

for (int row = 0; row < matrix.GetLength(0); row++)
{
    for (int col = 0; col < matrix.GetLength(1); col++)
    {
        if (matrix[row, col] > 0)
        { 
           aliveCells++;
           sum += matrix[row, col];
        }
    }
}

Console.WriteLine($"Alive cells: {aliveCells}");
Console.WriteLine($"Sum: {sum}");
PrintMatrix();

bool MatrixCheck(int currentRow, int currentCol)
{
    if (currentRow >= 0 && currentRow < matrix.GetLength(0) &&
            currentCol >= 0 && currentCol < matrix.GetLength(1))
    {
        return true;
    }

    return false;
}

void PrintMatrix()
{
    for (int row = 0; row < matrix.GetLength(0); row++)
    {
        for (int col = 0; col < matrix.GetLength(1); col++)
        {
            Console.Write(matrix[row, col] + " ");
        }

        Console.WriteLine();
    }
}