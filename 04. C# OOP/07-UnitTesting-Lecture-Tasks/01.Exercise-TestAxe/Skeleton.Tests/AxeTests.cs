using NUnit.Framework;
using System;

namespace Skeleton.Tests
{
    [TestFixture]
    public class AxeTests
    {
        [Test]
        public void AxeConstructorCheck()
        {
            Axe axe = new(100, 100);
            int expectedAxeAttackPoints = 100;
            int expectedAxeDurabilityPoints = 100;

            Assert.AreEqual(expectedAxeAttackPoints, axe.AttackPoints);
            Assert.AreEqual(expectedAxeDurabilityPoints, axe.DurabilityPoints);
        }

        [Test]
        public void AxeLoosesDurabilityAfterAttack()
        {
            Axe axe = new(50, 10);
            Dummy dummy = new(100, 100);

            axe.Attack(dummy);
            int expectedDurability = 9;
            Assert.That(axe.DurabilityPoints, Is.EqualTo(expectedDurability), "Axe loses durability after attack.");
        }

        [Test]
        public void BrokenAxeThrowsAnException()
        {
            Axe axe = new Axe(10, 1);
            Dummy dummy = new(100, 100);

            axe.Attack(dummy);

            Assert.Throws<InvalidOperationException>
                (() => axe.Attack(dummy), "Axe is broken.");
        }
    }
}