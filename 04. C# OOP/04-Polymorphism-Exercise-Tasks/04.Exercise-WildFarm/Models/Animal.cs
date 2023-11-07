using WildFarm.Models.Interfaces;

namespace WildFarm.Models
{
    public abstract class Animal : IAnimal
    {
        public Animal(string name, double weight)
        {
            Name = name;
            Weight = weight;
        }

        public string Name { get; private set; }
        public double Weight { get; private set; }
        public int FoodEaten { get; private set; }
        public abstract HashSet<Type> PreferredFoods { get; }

        public abstract string ProduceSound();

        public abstract double GetWeightMultiplier();

        public void FoodCheck(IFood food)
        {
            bool isContained = false;

            foreach (var item in PreferredFoods)
            {
                if (food.GetType().Name == item.Name)
                {
                    isContained = true;
                    FoodEaten = food.Quantity;
                    Weight = Weight + (FoodEaten * GetWeightMultiplier());
                    break;
                }
            }

            if (isContained == false)
            {
                throw new ArgumentException($"{GetType().Name} does not eat {food.GetType().Name}!");
            }
        }
        public abstract override string ToString();
    }
}