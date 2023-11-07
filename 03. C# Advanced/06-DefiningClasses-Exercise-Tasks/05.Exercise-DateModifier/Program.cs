//5 Exercise - Date Modifier
namespace DateModifier;

public class StartUp
{
    static void Main()
    {
        string startDate = Console.ReadLine();
        string endDate = Console.ReadLine();

        int differnceInDays = DateModifier.DaysDifference(startDate, endDate);
        Console.WriteLine(Math.Abs(differnceInDays));
    }
}