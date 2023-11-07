using WildFarm.Models.FoodType;

namespace WildFarm.Models.MammalType
{
    public class Mouse : Mammal
    {
        private const double IncreasedWeight = 0.10;

        public Mouse(string name, double weight, string livingRegion) 
            : base(name, weight, livingRegion)
        {
        }

        public override HashSet<Type> PreferredFoods
            => new() { typeof(Vegetable), typeof(Fruit) };

        public override string ProduceSound()
        => "Squeak";

        public override double GetWeightMultiplier()
        => IncreasedWeight;

        public override string ToString()
        => $"{GetType().Name} [{Name}, {Weight}, {LivingRegion}, {FoodEaten}]";
    }
}