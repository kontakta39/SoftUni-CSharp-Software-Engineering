//3 Exercise - Cards
using System.Text;

public class StartUp
{
    static void Main()
    {
        string[] input = Console.ReadLine()
            .Split(", ", StringSplitOptions.RemoveEmptyEntries);
        StringBuilder sb = new StringBuilder();

        foreach (var item in input)
        {
            string[] currentPair = item
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            try
            {
                string currentFace = currentPair[0];
                char currentSuit = char.Parse(currentPair[1]);
                Card card = new(currentFace, currentSuit);

                sb.Append(card.ShowCard(currentFace, currentSuit) + " ");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid card!");
            }
            
        }

        Console.WriteLine(sb.ToString().TrimEnd());
    }
}

class Card
{
    private readonly List<string> faces = new()
    { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"};

    private readonly Dictionary<char, char> suits = new()
    {
        { 'S', '\u2660' },
        { 'H', '\u2665' },
        { 'D', '\u2666' },
        { 'C', '\u2663' },
    };

    public Card(string face, char suit)
    {
        Face = face;
        Suit = suit;
    }

    public string Face { get; private set; }
    public char Suit { get; private set; }

    public string ShowCard(string currentFace, char currentSuit)
    {
        char paint = '\0';

        if (!faces.Contains(currentFace))
        {
            throw new FormatException();
        }

        else
        {
            foreach (var item in suits)
            {
                if (item.Key == currentSuit)
                {
                    paint = item.Value;
                    break;
                }
            }
        }

        return $"[{currentFace}{paint}]";
    }
}