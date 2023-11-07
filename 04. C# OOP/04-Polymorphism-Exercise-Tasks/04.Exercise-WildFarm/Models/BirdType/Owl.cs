using WildFarm.Models.FoodType;

namespace WildFarm.Models.BirdType
{
    public class Owl : Bird
    {
        private const double IncreasedWeight = 0.25;

        public Owl(string name, double weight, double wingSize)
            : base(name, weight, wingSize)
        {
        }

        public override HashSet<Type> PreferredFoods
            => new() { typeof(Meat) };

        public override string ProduceSound()
        => "Hoot Hoot";

        public override double GetWeightMultiplier()
        => IncreasedWeight;

        public override string ToString()
        => $"{GetType().Name} [{Name}, {WingSize}, {Weight}, {FoodEaten}]";
    }
}