namespace WildFarm.Models.Interfaces
{
    public interface IAnimal
    {
        string Name { get; }
        double Weight { get; }
        int FoodEaten { get; }

        public void FoodCheck(IFood food);
        public double GetWeightMultiplier();
        public string ProduceSound();
    }
}