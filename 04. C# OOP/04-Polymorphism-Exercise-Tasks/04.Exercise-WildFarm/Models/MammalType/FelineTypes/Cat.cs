using WildFarm.Models.FoodType;

namespace WildFarm.Models.MammalType.FelineTypes
{
    public class Cat : Feline
    {
        private const double IncreasedWeight = 0.30;

        public Cat(string name, double weight, string livingRegion, string breed)
            : base(name, weight, livingRegion, breed)
        {
        }

        public override HashSet<Type> PreferredFoods
            => new() { typeof(Vegetable), typeof(Meat) };

        public override string ProduceSound()
        => "Meow";

        public override double GetWeightMultiplier()
        => IncreasedWeight;

        public override string ToString()
        => $"{GetType().Name} [{Name}, {Breed}, {Weight}, {LivingRegion}, {FoodEaten}]";
    }
}