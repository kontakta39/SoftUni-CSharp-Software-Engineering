//7 Exercise - Pascal Triangle
int rowSize = int.Parse(Console.ReadLine());
long[][] jagged = new long[rowSize][];

for (int row = 0; row < jagged.Length; row++)
{
    if (row == 0)
    {
        jagged[row] = new long[1] { 1 };
    }

    else if (row > 0)
    {
        List<long> currentRow = jagged[row - 1].ToList();
        int elementsNumber = currentRow.Count + 1;
        List<long> nextRow = new();

        if (elementsNumber == 2)
        {
            for (int i = 0; i <= currentRow.Count; i++)
            {
                if (i == currentRow.Count)
                {
                    nextRow.Add(currentRow[i - 1]);
                    break;
                }

                nextRow.Add(currentRow[i]);
            }
        }

        else
        {
            nextRow.Add(currentRow[0]);

            for (int i = 0; i < currentRow.Count; i++)
            {
                if (i == currentRow.Count - 1)
                {
                    nextRow.Add(currentRow[currentRow.Count - 1]);
                }

                else
                {
                    nextRow.Add(currentRow[i] + currentRow[i + 1]);
                }
            }
        }

        long[] jaggedRowArray = nextRow.ToArray();
        jagged[row] = new long[jaggedRowArray.Length];

        for (int col = 0; col < jagged[row].Length; col++)
        {
            jagged[row][col] = jaggedRowArray[col];
        }
    }
}

foreach (long[] row in jagged)
{
    Console.WriteLine(string.Join(" ", row));
}

