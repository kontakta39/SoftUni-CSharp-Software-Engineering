using FoodShortage.Models.Interfaces;
using System.Numerics;

namespace FoodShortage.Models
{
    public class Robot : IRobot
    {
        public Robot(string model, BigInteger id)
        {
            Model = model;
            Id = id;
        }

        public string Model { get; private set; }

        public BigInteger Id { get; private set; }
    }
}