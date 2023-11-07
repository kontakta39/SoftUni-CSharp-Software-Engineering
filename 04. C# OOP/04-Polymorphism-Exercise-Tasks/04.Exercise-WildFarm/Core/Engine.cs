using WildFarm.Core.Interfaces;
using WildFarm.Models.BirdType;
using WildFarm.Models.FoodType;
using WildFarm.Models.Interfaces;
using WildFarm.Models.MammalType;
using WildFarm.Models.MammalType.FelineTypes;

namespace WildFarm.Core;

public class Engine : IEngine
{
    public void Run()
    {
        string input = Console.ReadLine();
        int count = 0;
        IAnimal animal = null;
        IFood food = null;
        List<IAnimal> animals = new();

        while (input != "End")
        {
            string[] currentInfo = input
             .Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (count % 2 == 0)
            {
                switch (currentInfo[0])
                {
                    case "Hen":
                        animal = new Hen(currentInfo[1], double.Parse(currentInfo[2]), double.Parse(currentInfo[3]));
                        animals.Add(animal);
                        break;
                    case "Owl":
                        animal = new Owl(currentInfo[1], double.Parse(currentInfo[2]), double.Parse(currentInfo[3]));
                        animals.Add(animal);
                        break;
                    case "Mouse":
                        animal = new Mouse(currentInfo[1], double.Parse(currentInfo[2]), currentInfo[3]);
                        animals.Add(animal);
                        break;
                    case "Dog":
                        animal = new Dog(currentInfo[1], double.Parse(currentInfo[2]), currentInfo[3]);
                        animals.Add(animal);
                        break;
                    case "Cat":
                        animal = new Cat(currentInfo[1], double.Parse(currentInfo[2]), currentInfo[3], currentInfo[4]);
                        animals.Add(animal);
                        break;
                    case "Tiger":
                        animal = new Tiger(currentInfo[1], double.Parse(currentInfo[2]), currentInfo[3], currentInfo[4]);
                        animals.Add(animal);
                        break;
                }
            }

            else
            {
                switch (currentInfo[0])
                {
                    case "Vegetable":
                        food = new Vegetable(int.Parse(currentInfo[1]));
                        break;
                    case "Fruit":
                        food = new Fruit(int.Parse(currentInfo[1]));
                        break;
                    case "Meat":
                        food = new Meat(int.Parse(currentInfo[1]));
                        break;
                    case "Seeds":
                        food = new Seeds(int.Parse(currentInfo[1]));
                        break;
                }

                try
                {
                    Console.WriteLine(animal.ProduceSound());
                    animal.FoodCheck(food);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            count++;
            input = Console.ReadLine();
        }

        foreach (var item in animals)
        {
            Console.WriteLine(item.ToString());
        }
    }
}