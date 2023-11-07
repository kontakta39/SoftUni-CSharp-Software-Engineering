using WildFarm.Models.FoodType;

namespace WildFarm.Models.BirdType
{
    public class Hen : Bird
    {
        private const double IncreasedWeight = 0.35;

        public Hen(string name, double weight, double wingSize)
            : base(name, weight, wingSize)
        {
        }

        public override HashSet<Type> PreferredFoods
            => new() { typeof(Vegetable), typeof(Fruit), typeof(Meat), typeof(Seeds) };

        public override string ProduceSound()
        => "Cluck";

        public override double GetWeightMultiplier()
        => IncreasedWeight;

        public override string ToString()
        => $"{GetType().Name} [{Name}, {WingSize}, {Weight}, {FoodEaten}]";
    }
}