using System.Collections;

namespace IteratorsAndComparators;

public class Library : IEnumerable<Book>
{
    private List<Book> library;

    public Library(params Book[] books)
    {
        library = books.ToList();
    }

    public IEnumerator<Book> GetEnumerator()
    {
       return library.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
       return GetEnumerator();
    }
}