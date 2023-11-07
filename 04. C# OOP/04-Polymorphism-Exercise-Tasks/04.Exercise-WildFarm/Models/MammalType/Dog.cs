using WildFarm.Models.FoodType;

namespace WildFarm.Models.MammalType
{
    public class Dog : Mammal
    {
        private const double IncreasedWeight = 0.40;

        public Dog(string name, double weight, string livingRegion)
            : base(name, weight, livingRegion)
        {
        }

        public override HashSet<Type> PreferredFoods
            => new() { typeof(Meat) };

        public override string ProduceSound()
        => "Woof!";

        public override double GetWeightMultiplier()
        => IncreasedWeight;

        public override string ToString()
        => $"{GetType().Name} [{Name}, {Weight}, {LivingRegion}, {FoodEaten}]";
    }
}