using System.Numerics;

namespace BirthdayCelebrations.Models.Interfaces
{
    public interface ICitizen
    {
        string Name { get; }
        int Age { get; }
        BigInteger Id { get; }
        string Birthdate { get; }
    }
}