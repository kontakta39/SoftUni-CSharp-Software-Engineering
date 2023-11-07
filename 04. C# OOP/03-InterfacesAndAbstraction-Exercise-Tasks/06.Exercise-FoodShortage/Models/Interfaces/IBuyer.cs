namespace FoodShortage.Models.Interfaces
{
    public interface IBuyer : INamable
    {
        int Food { get; }
        void BuyFood();
    }
}