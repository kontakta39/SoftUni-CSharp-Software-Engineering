namespace FightingArena.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class ArenaTests
    {
        [Test]
        public void ConstructorCheck()
        {
            List<Warrior> expectedWarriors = new();
            Arena arena = new();
            CollectionAssert.AreEqual(expectedWarriors, arena.Warriors);
        }

        [Test]
        public void WarriorsListCountCheck()
        {
            List<Warrior> expectedWarriors = new();
            int expectedCount = expectedWarriors.Count;
            Arena arena = new();

            Assert.AreEqual(expectedCount, arena.Count);
        }

        [Test]
        public void EnrollAWarrior()
        {
            Warrior expectedWarrior = new("Savev", 40, 60);
            Arena arena = new();
            arena.Enroll(expectedWarrior);

            List<Warrior> expectedWarriors = new();
            expectedWarriors.Add(expectedWarrior);

            CollectionAssert.AreEqual(expectedWarriors, arena.Warriors);
        }

        [Test]
        public void EnrollAWarriorThatExistsInAListThrowsAnException()
        {
            Warrior expectedWarrior = new("Savev", 40, 60);
            string expectedWarriorName = expectedWarrior.Name;
            Arena arena = new();
            arena.Enroll(expectedWarrior);

            Assert.Throws<InvalidOperationException>
                (() => arena.Enroll(expectedWarrior), "Warrior is already enrolled for the fights!");
        }

        [Test]
        public void FightBetweenTwoWarriors()
        {
            Warrior firstWarrior = new("Savev", 100, 100);
            Warrior secondWarrior = new("Valchev", 40, 60);

            Arena arena = new();
            arena.Enroll(firstWarrior);
            arena.Enroll(secondWarrior);
            string firstWarriorName = firstWarrior.Name;
            string secondWarriorName = secondWarrior.Name;
            arena.Fight(firstWarriorName, secondWarriorName);

            int expectedDefenderWarriorHP = 0;
            
            Assert.AreEqual(expectedDefenderWarriorHP, secondWarrior.HP);
        }

        [Test]
        public void IfWarriorAttackerNameIsNullThrowsAnException()
        {
            Warrior defenderWarrior = new("Valchev", 40, 60);
            Arena arena = new();
            arena.Enroll(defenderWarrior);

            string attackerWarriorName = "Savev";
            string defenderWarriorName = defenderWarrior.Name;

            Assert.Throws<InvalidOperationException>
                (() => arena.Fight(attackerWarriorName, defenderWarriorName), 
                $"There is no fighter with name {attackerWarriorName} enrolled for the fights!");
        }

        [Test]
        public void IfWarriorDefenderNameIsNullThrowsAnException()
        {
            Warrior attackerWarrior = new("Savev", 100, 100);
            Arena arena = new();
            arena.Enroll(attackerWarrior);

            string attackerWarriorName = attackerWarrior.Name;
            string defenderWarriorName = "Valchev";

            Assert.Throws<InvalidOperationException>
                (() => arena.Fight(attackerWarriorName, defenderWarriorName),
                $"There is no fighter with name {defenderWarriorName} enrolled for the fights!");
        }
    }
}