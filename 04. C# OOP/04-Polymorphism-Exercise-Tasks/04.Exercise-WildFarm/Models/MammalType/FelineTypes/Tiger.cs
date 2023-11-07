using WildFarm.Models.FoodType;

namespace WildFarm.Models.MammalType.FelineTypes
{
    public class Tiger : Feline
    {
        private const double IncreasedWeight = 1.00;

        public Tiger(string name, double weight, string livingRegion, string breed)
            : base(name, weight, livingRegion, breed)
        {
        }

        public override HashSet<Type> PreferredFoods
            => new() { typeof(Meat) };

        public override string ProduceSound()
        => "ROAR!!!";

        public override double GetWeightMultiplier()
        => IncreasedWeight;

        public override string ToString()
        => $"{GetType().Name} [{Name}, {Breed}, {Weight}, {LivingRegion}, {FoodEaten}]";
    }
}