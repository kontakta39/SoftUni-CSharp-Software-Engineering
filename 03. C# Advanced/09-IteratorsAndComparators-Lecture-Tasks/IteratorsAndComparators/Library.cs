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
        return new BooksEnumerator(library);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class BooksEnumerator : IEnumerator<Book>
    {
        private List<Book> books;
        private int index = -1;

        public BooksEnumerator(List<Book> books)
        {
            this.books = books;
        }

        public Book Current => books[index];

        object IEnumerator.Current => Current;

        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            index++;

            return index < books.Count;
        }

        public void Reset()
        {
            index = -1;
        }
    }
}