//6 Exercise - Animals
using Animals;

public class StartUp
{
    static void Main()
    {
        string animalType = Console.ReadLine();

        while (animalType != "Beast!")
        {
            string[] animalInfo = Console.ReadLine()
    .Split(" ", StringSplitOptions.RemoveEmptyEntries);


            switch (animalType)
            {
                case "Dog":
                    try
                    {
                        Dog dog = new(animalInfo[0], int.Parse(animalInfo[1]), animalInfo[2]);
                        Console.WriteLine(animalType);
                        Console.WriteLine(dog.ToString());
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "Frog":
                    try
                    {
                        Frog frog = new(animalInfo[0], int.Parse(animalInfo[1]), animalInfo[2]);
                        Console.WriteLine(animalType);
                        Console.WriteLine(frog.ToString());
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "Cat":
                    try
                    {
                        Cat cat = new(animalInfo[0], int.Parse(animalInfo[1]), animalInfo[2]);
                        Console.WriteLine(animalType);
                        Console.WriteLine(cat.ToString());
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "Tomcat":
                    try
                    {
                        Tomcat tomcat = new(animalInfo[0], int.Parse(animalInfo[1]));
                        Console.WriteLine(animalType);
                        Console.WriteLine(tomcat.ToString());
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                    break;

                case "Kitten":
                    try
                    {
                        Kitten kitten = new(animalInfo[0], int.Parse(animalInfo[1]));
                        Console.WriteLine(animalType);
                        Console.WriteLine(kitten.ToString());
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                    break;
            }

            animalType = Console.ReadLine();
        }
    }
}