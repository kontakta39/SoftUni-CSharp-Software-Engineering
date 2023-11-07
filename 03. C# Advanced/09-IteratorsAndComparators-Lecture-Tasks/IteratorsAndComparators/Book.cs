namespace IteratorsAndComparators;

public class Book
{
    public Book(string title, int year, params string[] names)
    {
        Title = title;
        Year = year;
        Authors = names.ToList();
    }
    public string Title { get; set; }
    public int Year { get; set; }
    public List<string> Authors { get; set; }
}