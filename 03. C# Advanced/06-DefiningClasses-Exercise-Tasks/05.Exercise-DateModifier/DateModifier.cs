namespace DateModifier;

public class DateModifier
{
    public static int DaysDifference(string start, string end)
    {
        DateTime startDate = DateTime.Parse(start);
        DateTime endDate = DateTime.Parse(end);
        int differnce = (startDate - endDate).Days;
        return differnce;
    }
}
