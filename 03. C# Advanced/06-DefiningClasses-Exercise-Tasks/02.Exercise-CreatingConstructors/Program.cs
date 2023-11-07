//2 Exercise - Creating Constructors
using DefiningClasses;

public class StartUp
{
    static void Main()
    {
        Person firstPerson = new();

        Person secondPerson = new(20);
        Person thirdPerson = new("Raiko", 35);      
    }
}
