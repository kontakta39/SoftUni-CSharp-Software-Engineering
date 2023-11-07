using System.Numerics;

namespace FoodShortage.Models.Interfaces
{
    public interface IRobot
    {
        string Model { get; }
        BigInteger Id { get; }
    }
}