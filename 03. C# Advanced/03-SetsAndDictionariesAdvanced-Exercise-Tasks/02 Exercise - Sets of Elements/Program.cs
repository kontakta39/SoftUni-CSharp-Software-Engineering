//2 Exercise - Sets of Elements
int[] elementsCountInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .ToArray();
int nCountElements = elementsCountInfo[0];
int mCountElements = elementsCountInfo[1];
HashSet<int> nSet = new HashSet<int>();
HashSet<int> mSet = new HashSet<int>();

for (int i = 0; i < nCountElements; i++)
{
    int currentElement = int.Parse(Console.ReadLine());
    nSet.Add(currentElement);
}

for (int i = 0; i < mCountElements; i++)
{
    int currentElement = int.Parse(Console.ReadLine());
    mSet.Add(currentElement);
}

for (int i = 0; i < nSet.Count; i++)
{
    int currentElement = nSet.ElementAt(i);

    if (mSet.Contains(currentElement))
    {
        Console.Write(currentElement + " ");
    }
}
