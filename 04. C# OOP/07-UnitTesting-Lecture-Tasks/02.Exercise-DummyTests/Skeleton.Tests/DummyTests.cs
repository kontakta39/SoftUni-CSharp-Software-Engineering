using NUnit.Framework;
using System;

namespace Skeleton.Tests
{
    [TestFixture]
    public class DummyTests
    {
        [Test]
        public void DummyLosesHealthIfAttacked()
        {
            Dummy dummy = new(100, 10);
            Axe axe = new(50, 10);

            dummy.TakeAttack(axe.AttackPoints);

            int expectedHealth = 50;
            Assert.AreEqual(expectedHealth, dummy.Health);
        }

        [Test]
        public void DeadDummyThrowsAnExceptionIfAttacked()
        {
            Dummy dummy = new(50, 10);
            Axe axe = new(50, 10);

            dummy.TakeAttack(axe.AttackPoints);

            Assert.Throws<InvalidOperationException>
                (() => dummy.TakeAttack(axe.AttackPoints), "Dummy is dead.");
        }

        [Test]
        public void DeadDummyCanGiveXP()
        {
            Dummy dummy = new(0, 10);

            int expectedExperience = 10;
            Assert.AreEqual(expectedExperience, dummy.GiveExperience());
        }

        [Test]
        public void AliveDummyCannotGiveXP()
        {
            Dummy dummy = new(50, 10);

            Assert.Throws<InvalidOperationException>
                (() => dummy.GiveExperience(), "Target is not dead.");
        }
    }
}