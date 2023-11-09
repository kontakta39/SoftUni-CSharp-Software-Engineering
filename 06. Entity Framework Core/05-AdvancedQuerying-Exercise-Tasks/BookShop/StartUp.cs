namespace BookShop;

using BookShop.Models;
using BookShop.Models.Enums;
using Data;
using Initializer;
using System.Globalization;
using System.Linq;
using System.Text;

public class StartUp
{
    public static void Main()
    {
        using var db = new BookShopContext();
        DbInitializer.ResetDatabase(db);

        //2 Exercise
        //string command = Console.ReadLine();
        //Console.WriteLine(GetBooksByAgeRestriction(db, command));

        //3 Exercise
        //Console.WriteLine(GetGoldenBooks(db));

        //4 Exercise
        //Console.WriteLine(GetBooksByPrice(db));

        //5 Exercise
        //int year = int.Parse(Console.ReadLine());
        //Console.WriteLine(GetBooksNotReleasedIn(db, year));

        //6 Exercise
        //string input = Console.ReadLine();
        //Console.WriteLine(GetBooksByCategory(db, input));

        //7 Exercise
        //string date = Console.ReadLine();
        //Console.WriteLine(GetBooksReleasedBefore(db, date));

        //8 Exercise
        //string input = Console.ReadLine();
        //Console.WriteLine(GetAuthorNamesEndingIn(db, input));

        //9 Exercise
        //string input = Console.ReadLine();
        //Console.WriteLine(GetBookTitlesContaining(db, input));

        //10 Exercise
        //string input = Console.ReadLine();
        //Console.WriteLine(GetBooksByAuthor(db, input));

        //11 Exercise
        //int lengthCheck = int.Parse(Console.ReadLine());
        //Console.WriteLine(CountBooks(db, lengthCheck));

        //12 Exercise
        //Console.WriteLine(CountCopiesByAuthor(db));

        //13 Exercise
        //Console.WriteLine(GetTotalProfitByCategory(db));

        //14 Exercise
        //Console.WriteLine(GetMostRecentBooks(db));

        //15 Exercise
        //IncreasePrices(db);

        //16 Exercise
        Console.WriteLine(RemoveBooks(db));
    }

    //2 Exercise - Age Restriction
    public static string GetBooksByAgeRestriction(BookShopContext context, string command)
    {
        var ageRestriction = Enum.Parse(typeof(AgeRestriction), command, true);

        var currentBooks = context.Books
                       .Where(b => b.AgeRestriction.Equals(ageRestriction))
                       .OrderBy(b => b.Title)
                       .Select(b => b.Title)
                       .ToList();

        StringBuilder sb = new();

        foreach (var book in currentBooks)
        {
            sb.AppendLine(book);
        }

        return sb.ToString().TrimEnd();
    }

    //3 Exercise - Golden Books
    public static string GetGoldenBooks(BookShopContext context)
    {
        var currentBooks = context.Books
            .Where(b => b.Copies < 5000 && b.EditionType.Equals(EditionType.Gold))
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToList();

        StringBuilder sb = new();

        foreach (var book in currentBooks)
        {
            sb.AppendLine(book);
        }

        return sb.ToString().TrimEnd();
    }

    //4 Exercise - Golden Books
    public static string GetBooksByPrice(BookShopContext context)
    {
        var currentBooks = context.Books
            .Where(b => b.Price > 40)
            .OrderByDescending(b => b.Price)
            .Select(b => new
            {
                b.Title,
                b.Price
            })
            .ToList();

        StringBuilder sb = new();

        foreach (var book in currentBooks)
        {
            sb.AppendLine($"{book.Title} - ${book.Price:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //5 Exercise - Not Released In
    public static string GetBooksNotReleasedIn(BookShopContext context, int year)
    {
        var currentBooks = context.Books
            .Where(b => b.ReleaseDate.Value.Year != year)
            .OrderBy(b => b.BookId)
            .Select(b => b.Title)
            .ToList();

        StringBuilder sb = new();

        foreach (var book in currentBooks)
        {
            sb.AppendLine(book);
        }

        return sb.ToString().TrimEnd();
    }

    //6 Exercise - Book Titles by Category
    public static string GetBooksByCategory(BookShopContext context, string input)
    {
        string[] currentCategories = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(c => c.ToLower())
            .ToArray();

        List<string> books = new();

        foreach (var category in currentCategories)
        {
            var currentBooks = context.Books
               .SelectMany(b => b.BookCategories, (b, c) => new
               {
                   b.Title,
                   Category = c.Category.Name.ToLower()
               })
               .Where(b => b.Category == category)
               .ToList();

            foreach (var item in currentBooks)
            {
                books.Add(item.Title);
            }
        }

        StringBuilder sb = new();

        foreach (var book in books.OrderBy(b => b))
        {
            sb.AppendLine(book);
        }

        return sb.ToString().TrimEnd();
    }

    //7 Exercise - Released Before Date
    public static string GetBooksReleasedBefore(BookShopContext context, string date)
    {
        var currentBooks = context.Books
           .Where(b => b.ReleaseDate < DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture))
           .OrderByDescending(b => b.ReleaseDate)
           .Select(b => new
           {
               b.Title,
               b.EditionType,
               b.Price
           })
           .ToList();

        StringBuilder sb = new();

        foreach (var book in currentBooks)
        {
            sb.AppendLine($"{book.Title} - {book.EditionType} - ${book.Price:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //8 Exercise - Author Search
    public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
    {
        var authors = context.Authors
            .Where(b => b.FirstName.EndsWith(input))
            .Select(b => $"{b.FirstName} {b.LastName}");

        StringBuilder sb = new();

        foreach (var author in authors.OrderBy(b => b))
        {
            sb.AppendLine(author);
        }

        return sb.ToString().TrimEnd();
    }

    //9 Exercise - Book Search
    public static string GetBookTitlesContaining(BookShopContext context, string input)
    {
        var books = context.Books
            .Where(b => b.Title.ToLower().Contains(input.ToLower()))
            .Select(b => b.Title);

        StringBuilder sb = new();

        foreach (var book in books.OrderBy(b => b))
        {
            sb.AppendLine(book);
        }

        return sb.ToString().TrimEnd();
    }

    //10 Exercise - Book Search by Author
    public static string GetBooksByAuthor(BookShopContext context, string input)
    {
        var books = context.Books
            .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
            .OrderBy(b => b.BookId)
            .Select(b => new
            {
                b.Title,
                AuthorFullName = $"{b.Author.FirstName} {b.Author.LastName}"
            });

        StringBuilder sb = new();

        foreach (var book in books)
        {
            sb.AppendLine($"{book.Title} ({book.AuthorFullName})");
        }

        return sb.ToString().TrimEnd();
    }

    //11 Exercise - Count Books
    public static int CountBooks(BookShopContext context, int lengthCheck)
    {
        int booksCount = context.Books
            .Where(b => b.Title.Length > lengthCheck)
            .Count();

        return booksCount;
    }

    //12 Exercise - Total Book Copies
    public static string CountCopiesByAuthor(BookShopContext context)
    {
        var authors = context.Authors
            .Select(a => new
            {
                AuthorFullName = $"{a.FirstName} {a.LastName}",
                CopiesTotalNumber = a.Books.Sum(b => b.Copies)
            })
            .OrderByDescending(a => a.CopiesTotalNumber)
            .ToList();

        StringBuilder sb = new();

        foreach (var author in authors)
        {
            sb.AppendLine($"{author.AuthorFullName} - {author.CopiesTotalNumber}");
        }

        return sb.ToString().TrimEnd();
    }

    //13 Exercise - Profit by Category
    public static string GetTotalProfitByCategory(BookShopContext context)
    {
        var categories = context.Categories
            .Select(c => new
            {
                c.Name,
                TotalProfit = c.CategoryBooks
                 .Select(b => b.Book.Copies * b.Book.Price)
                 .Sum()
            })
            .OrderByDescending(c => c.TotalProfit)
            .ThenBy(c => c.Name)
            .ToList();

        StringBuilder sb = new();

        foreach (var category in categories)
        {
            sb.AppendLine($"{category.Name} ${category.TotalProfit:f2}");
        }

        return sb.ToString().TrimEnd();
    }

    //14 Exercise - Most Recent Books
    public static string GetMostRecentBooks(BookShopContext context)
    {
        var categories = context.Categories
            .Select(c => new
            {
                c.Name,
                Books = c.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Take(3)
                        .Select(b => new
                        {
                            BookName = b.Book.Title,
                            ReleaseDate = b.Book.ReleaseDate.Value.Year
                        })
                        .ToList()
            })
            .OrderBy(c => c.Name)
            .ToList();

        StringBuilder sb = new();

        foreach (var category in categories)
        {
            sb.AppendLine($"--{category.Name}");
            category.Books.ForEach(c => sb.AppendLine($"{c.BookName} ({c.ReleaseDate})"));
        }

        return sb.ToString().TrimEnd();
    }

    //15 Exercise - Increase Prices
    public static void IncreasePrices(BookShopContext context)
    {
        var books = context.Books
           .Where(b => b.ReleaseDate.Value.Year < 2010)
           .ToList();

        foreach (var book in books)
        {
            book.Price += 5;
        }

        context.SaveChanges();
    }

    //16 Exercise - Remove Books
    public static int RemoveBooks(BookShopContext context)
    {
        var books = context.Books
           .Where(b => b.Copies < 4200)
           .ToList();

        context.Books.RemoveRange(books);
        context.SaveChanges();

        return books.Count;
    }
}