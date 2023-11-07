using System.Reflection;

namespace AuthorProblem;

public class Tracker
{
    public void PrintMethodsByAuthor()
    {
        Type type = typeof(StartUp);
        MethodInfo[] allMethods = type.GetMethods(
        BindingFlags.Static |
        BindingFlags.Public |
        BindingFlags.NonPublic);

        foreach (MethodInfo method in allMethods)
        {
            AuthorAttribute[] attributes = method.GetCustomAttributes<AuthorAttribute>().ToArray();

            foreach (AuthorAttribute attribute in attributes)
            {
                Console.WriteLine($"{method.Name} is written by {attribute.Name}");
            }
        }
    }
}