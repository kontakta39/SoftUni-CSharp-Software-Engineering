using NUnit.Framework;
using System.Diagnostics.Contracts;

namespace VendingRetail.Tests;

public class Tests
{
    [Test]
    public void ConstructorCheck()
    {
        CoffeeMat coffee = new(50, 10);
        int expectedWaterCapacity = 50;
        int expectedButtonsCount = 10;

        Assert.AreEqual(expectedWaterCapacity, coffee.WaterCapacity);
        Assert.AreEqual(expectedButtonsCount, coffee.ButtonsCount);
        Assert.AreEqual(0, coffee.Income);
    }

    [Test]
    public void CheckWaterCapacitySetter()
    {
        CoffeeMat coffee = new(50, 10);
        int expectedWaterCapacity = 50;

        Assert.AreEqual(expectedWaterCapacity, coffee.WaterCapacity);
    }

    [Test]
    public void CheckButtonsCountSetter()
    {
        CoffeeMat coffee = new(50, 10);
        int expectedButtonsCount = 10;

        Assert.AreEqual(expectedButtonsCount, coffee.ButtonsCount);
    }

    [Test]
    public void CheckIncomeSetter()
    {
        CoffeeMat coffee = new(50, 10);

        Assert.AreEqual(0, coffee.Income);
    }

    [Test]
    public void FillWaterTankCorrectly()
    {
        CoffeeMat coffee = new(50, 10);
        string expectedMessage = $"Water tank is filled with {50}ml";

        string currentMessage = coffee.FillWaterTank();
        Assert.AreEqual(expectedMessage, currentMessage);
    }

    [Test]
    public void FillWaterTankButWatterTankIsFull()
    {
        CoffeeMat coffee = new(50, 10);
        string expectedMessage = $"Water tank is already full!";

        _ = coffee.FillWaterTank();
        string currentMessage = coffee.FillWaterTank();

        Assert.AreEqual(expectedMessage, currentMessage);
    }

    [Test]
    public void AddDrinkCorrectly()
    {
        CoffeeMat coffeeMat = new(50, 10);

        bool isAdded = coffeeMat.AddDrink("Cola", 1.50);

        Assert.IsTrue(isAdded);
    }

    [Test]
    public void CannotAddDrinkBecauseTheVendingHasNoMoreButtons()
    {
        CoffeeMat coffeeMat = new(50, 1);

        _ = coffeeMat.AddDrink("Cola", 1.50);
        bool isAdded = coffeeMat.AddDrink("Sprite", 1.70);

        Assert.IsFalse(isAdded);
    }

    [Test]
    public void CannotAddDrinkBecauseItHasAlreadyBeenAdded()
    {
        CoffeeMat coffeeMat = new(50, 10);

        _ = coffeeMat.AddDrink("Cola", 1.50);
        bool isAdded = coffeeMat.AddDrink("Cola", 1.50);

        Assert.IsFalse(isAdded);
    }

    [Test]
    public void BuyDrinkCorrectly()
    {
        CoffeeMat coffee = new(100, 10);
        double expectedIncome = 1.5;
        string expectedMessage = $"Your bill is {expectedIncome:f2}$";

        _ = coffee.FillWaterTank();
        _ = coffee.AddDrink("Cola", 1.50);
        string currentMessage = coffee.BuyDrink("Cola");

        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(expectedIncome, coffee.Income);
    }

    [Test]
    public void BuyDrinkButDoesNotExistsInTheVending()
    {
        CoffeeMat coffee = new(100, 10);
        string expectedName = "Cola";
        double expectedIncome = 0;
        string expectedMessage = $"{expectedName} is not available!";

        _ = coffee.FillWaterTank();
        _ = coffee.AddDrink("Sprite", 1.50);
        string currentMessage = coffee.BuyDrink("Cola");

        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(expectedIncome, coffee.Income);
    }

    [Test]
    public void BuyDrinkButWaterTankLevelIsNotEnough()
    {
        CoffeeMat coffee = new(50, 10);
        string expectedMessage = $"CoffeeMat is out of water!";

        _ = coffee.FillWaterTank();
        string currentMessage = coffee.BuyDrink("Cola");

        Assert.AreEqual(expectedMessage, currentMessage);
    }

    [Test]
    public void CollectIncomeCorrectly()
    {
        CoffeeMat coffee = new(100, 10);
        double expectedIncome = 1.5;

        _ = coffee.FillWaterTank();
        _ = coffee.AddDrink("Cola", 1.50);
        _ = coffee.BuyDrink("Cola");

        double collectedIncome = coffee.CollectIncome();

        Assert.AreEqual(expectedIncome, collectedIncome);
    }

    [Test]
    public void CollectIncomeButThereIsNoIncome()
    {
        CoffeeMat coffee = new(100, 10);
        double expectedIncome = 0;

        _ = coffee.FillWaterTank();
        _ = coffee.AddDrink("Cola", 1.50);

        double collectedIncome = coffee.CollectIncome();

        Assert.AreEqual(expectedIncome, collectedIncome);
    }
}