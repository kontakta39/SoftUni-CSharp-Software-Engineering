using Raiding.Models.Interfaces;

namespace Raiding.Models
{
    public abstract class BaseHero : IBaseHero
    {
        public BaseHero(string name, int power)
        {
            Name = name;
            Power = power;
        }

        public string Name { get; private set; }

        public virtual int Power { get; private set; }

        public abstract string CastAbility();
    }
}