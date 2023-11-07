using System.Collections;

namespace Froggy;

public class Lake : IEnumerable<int>
{
    private List<int> froggs = new();

    public void Add(int[] numbers)
    {
        froggs = numbers.ToList();
    }

    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < froggs.Count; i++)
        {
            if (i % 2 == 0)
            {
                yield return froggs[i];
            }
        }

        for (int i = froggs.Count - 1; i >= 0; i--)
        {
            if (i % 2 != 0)
            {
                yield return froggs[i];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}