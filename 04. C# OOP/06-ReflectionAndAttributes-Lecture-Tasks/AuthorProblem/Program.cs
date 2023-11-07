//5 Exercise - Create Attribute
namespace AuthorProblem;

[Author("Victor")][Author("Niki")]
public class StartUp
{
    [Author("George")]
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
    }
}