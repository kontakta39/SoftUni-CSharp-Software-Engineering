namespace Cars.Models.Interfaces;

public interface IElectricCar : ICar
{
    int Battery { get; }
}