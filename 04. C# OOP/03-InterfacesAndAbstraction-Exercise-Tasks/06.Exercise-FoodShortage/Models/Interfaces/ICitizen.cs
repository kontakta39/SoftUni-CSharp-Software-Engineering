using System.Numerics;

namespace FoodShortage.Models.Interfaces
{
    public interface ICitizen
    {
        int Age { get; }
        BigInteger Id { get; }
        string Birthdate { get; }
    }
}