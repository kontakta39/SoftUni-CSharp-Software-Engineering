namespace FightingArena.Tests
{
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class WarriorTests
    {
        private Warrior warrior;

        [SetUp]
        public void SetUp()
        {
            warrior = new Warrior("Savev", 40, 100);
        }

        [Test]
        public void ConstructorCheck()
        {
            string name = "Savev";
            int damage = 40;
            int hp = 100;

            Assert.AreEqual(name, warrior.Name);
            Assert.AreEqual(damage, warrior.Damage);
            Assert.AreEqual(hp, warrior.HP);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void WarriorNameThrowsAnExceptionIfSetIsNull(string name)
        {
            Assert.Throws<ArgumentException>
                (() => warrior = new Warrior(name, 40, 100), "Name should not be empty or whitespace!");
        }

        [TestCase(0)]
        [TestCase(-10)]
        public void WarriorDamageThrowsAnExceptionIfSetIsNegative(int damage)
        {
            Assert.Throws<ArgumentException>
                (() => warrior = new Warrior("Savev", damage, 100), "Damage value should be positive!");
        }

        [TestCase(-10)]
        public void WarriorHPThrowsAnExceptionIfSetIsNegative(int hp)
        {
            Assert.Throws<ArgumentException>
                (() => warrior = new Warrior("Savev", 40, hp), "HP should not be negative!");
        }

        [Test]
        public void YourHPIsTooLowToAttackThrowsAnException()
        {
            warrior = new("Savev", 90, 10);
            Warrior enemy = new("Valchev", 20, 80);

            Assert.Throws<InvalidOperationException>
                (() => warrior.Attack(enemy), "Your HP is too low in order to attack other warriors!");
        }

        [Test]
        public void EnemyHPIsTooLowToAttackThrowsAnException()
        {
            Warrior enemy = new("Valchev", 80, 20);
            const int MIN_ATTACK_HP = 30;

            Assert.Throws<InvalidOperationException>
                (() => warrior.Attack(enemy), $"Enemy HP must be greater than {MIN_ATTACK_HP} in order to attack him!");
        }

        [Test]
        public void IfYourHPIsLowerThanTheEnemyHPThrowsAnException()
        {
            warrior = new("Savev", 60, 40);
            Warrior enemy = new("Valchev", 50, 50);

            Assert.Throws<InvalidOperationException>
                (() => warrior.Attack(enemy), "You are trying to attack too strong enemy");
        }

        [Test]
        public void YourWarriorAttackEnemyWithMoreDamage()
        {
            warrior = new("Savev", 100, 100);
            Warrior enemy = new("Valchev", 40, 60);

            warrior.Attack(enemy);
            int expectedResult = 0;

            Assert.AreEqual(expectedResult, enemy.HP);
        }

        [Test]
        public void EnemyAttackYourWarriorWithMoreDamage()
        {
            warrior = new("Savev", 40, 60);
            Warrior enemy = new("Valchev", 50, 100);

            int expectedResult = enemy.HP - warrior.Damage;
            warrior.Attack(enemy);

            Assert.AreEqual(expectedResult, enemy.HP);
        }
    }
}