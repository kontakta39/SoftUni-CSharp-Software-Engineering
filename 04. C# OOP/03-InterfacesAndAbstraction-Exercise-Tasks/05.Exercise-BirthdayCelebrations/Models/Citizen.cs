using BirthdayCelebrations.Models.Interfaces;
using System.Numerics;

namespace BirthdayCelebrations.Models
{
    public class Citizen : ICitizen
    {
        public Citizen(string name, int age, BigInteger id, string birthdate)
        {
            Name = name;
            Age = age;
            Id = id;
            Birthdate = birthdate;
        }

        public string Name { get; private set; }

        public int Age { get; private set; }

        public BigInteger Id { get; private set; }

        public string Birthdate { get; private set; }
    }
}