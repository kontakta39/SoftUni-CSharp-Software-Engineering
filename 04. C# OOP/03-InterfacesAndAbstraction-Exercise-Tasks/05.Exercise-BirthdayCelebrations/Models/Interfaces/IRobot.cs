using System.Numerics;

namespace BirthdayCelebrations.Models.Interfaces
{
    public interface IRobot
    {
        string Model { get; }
        BigInteger Id { get; }
    }
}