using NUnit.Framework;
using System.Linq;

namespace RobotFactory.Tests;

public class FactoryTests
{
    [Test]
    public void ConstructorCheck()
    {
        string expectedName = "Savevi";
        int expectedCapacity = 100;

        Factory factory = new("Savevi", 100);

        Assert.AreEqual(expectedName, factory.Name);
        Assert.AreEqual(expectedCapacity, factory.Capacity);
        Assert.NotNull(factory.Robots);
        Assert.NotNull(factory.Supplements);
    }

    [Test]
    public void CheckFactoryNameSetter()
    {
        string expectedName = "Savevi";

        Factory factory = new("Savevi", 50);

        Assert.AreEqual(expectedName, factory.Name);
    }

    [Test]
    public void CheckFactoryCapacitySetter()
    {
        int expectedCapacity = 50;

        Factory factory = new("Savevi", 50);

        Assert.AreEqual(expectedCapacity, factory.Capacity);
    }

    [Test]
    public void RobotShouldBeAddedCorrectlyIfFactoryHasEnoughCapacity()
    {
        string model = "Savev";
        double price = 100.50;
        int interfaceStandard = 75;

        Robot expectedRobot = new(model, price, interfaceStandard);
        string expectedMessage = $"Produced --> {expectedRobot}";

        Factory factory = new("Savevi", 10);
        string currentMessage = factory.ProduceRobot(model, price, interfaceStandard);
        Robot currentRobot = factory.Robots.Single();

        Assert.AreEqual(expectedRobot.Model, currentRobot.Model);
        Assert.AreEqual(expectedRobot.Price, currentRobot.Price);
        Assert.AreEqual(expectedRobot.InterfaceStandard, currentRobot.InterfaceStandard);
        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(1, factory.Robots.Count);
    }

    [Test]
    public void RobotShouldNotBeAddedCorrectlyBecauseFactoryDoesNotHaveEnoughCapacity()
    {
        string model = "Savev";
        double price = 100.50;
        int interfaceStandard = 75;

        string expectedMessage = "The factory is unable to produce more robots for this production day!";

        Factory factory = new("Savevi", 1);
        _ = factory.ProduceRobot(model, price, interfaceStandard);

        string modelOne = "Valchev";
        double priceOne = 70.50;
        int interfaceStandardOne = 45;

        string currentMessage = factory.ProduceRobot(model, price, interfaceStandard);

        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(1, factory.Robots.Count);
    }

    [Test]
    public void CheckProduceSupplemnt()
    {
        string name = "Lakova";
        int interfaceStandard = 25;
        Supplement expectedSupplement = new(name, interfaceStandard);
        string expectedMessage = expectedSupplement.ToString();

        Factory factory = new("Savevi", 10);
        string currentMessage = factory.ProduceSupplement(name, interfaceStandard);

        Assert.AreEqual(expectedMessage, currentMessage);
        Assert.AreEqual(1, factory.Supplements.Count);
    }

    [Test]
    public void UpgradeRobotShouldWorkCorrectly()
    {
        Factory factory = new("Savevi", 10);
        Robot robot = new("Savev", 100.50, 75);

        string name = "Lakova";
        int interfaceStandard = 25;
        robot.Supplements.Add(new(name, interfaceStandard));

        Supplement supplement = new("Martinov", 75);
        bool ifAdded = factory.UpgradeRobot(robot, supplement);

        Assert.IsTrue(ifAdded);
        Assert.AreEqual(2, robot.Supplements.Count);
    }

    [Test]
    public void UpgradeRobotShouldNotAddSupplementBecauseItIsAlreadyAdded()
    {
        Factory factory = new("Savevi", 10);
        Robot robot = new("Savev", 100.50, 75);

        Supplement supplement = new("Lakova", 75);
        robot.Supplements.Add(supplement);

        bool ifAdded = factory.UpgradeRobot(robot, supplement);

        Assert.IsFalse(ifAdded);
        Assert.AreEqual(1, robot.Supplements.Count);
    }

    [Test]
    public void UpgradeRobotShouldNotAddSupplementBecauseRobotInterfaceStandardDiffersFromTheSupplementStandard()
    {
        Factory factory = new("Savevi", 10);
        Robot robot = new("Savev", 100.50, 75);

        Supplement supplement = new("Lakova", 75);
        robot.Supplements.Add(supplement);
        Supplement supplementOne = new("Martinov", 74);

        bool ifAdded = factory.UpgradeRobot(robot, supplementOne);

        Assert.IsFalse(ifAdded);
        Assert.AreEqual(1, robot.Supplements.Count);
    }

    [Test]
    public void SellRobotCorrectly()
    {
        Factory factory = new("Savevi", 10);
        Robot robot = new("Savev", 100, 75);
        Robot robotOne = new("Valchev", 70, 65);
        Robot robotTwo = new("Galev", 80, 55);
        Robot robotThree = new("Hristov", 35, 30);
        factory.Robots.Add(robot);
        factory.Robots.Add(robotOne);
        factory.Robots.Add(robotTwo);
        factory.Robots.Add(robotThree);

        Robot expectedRobot = new("Valchev", 70, 65);
        Robot currentRobot = factory.SellRobot(75);

        Assert.AreEqual(expectedRobot.Model, currentRobot.Model);
        Assert.AreEqual(expectedRobot.Price, currentRobot.Price);
        Assert.AreEqual(expectedRobot.InterfaceStandard, currentRobot.InterfaceStandard);
    }

    [Test]
    public void CannotSellRobotBecauseThePriceIsLowerThanTheSoldRobots()
    {
        Factory factory = new("Savevi", 10);
        Robot robot = new("Savev", 100, 75);
        Robot robotOne = new("Valchev", 70, 65);
        Robot robotTwo = new("Galev", 80, 55);
        Robot robotThree = new("Hristov", 35, 30);
        factory.Robots.Add(robot);
        factory.Robots.Add(robotOne);
        factory.Robots.Add(robotTwo);
        factory.Robots.Add(robotThree);

        Robot currentRobot = factory.SellRobot(20);

        Assert.IsNull(currentRobot);
    }
}