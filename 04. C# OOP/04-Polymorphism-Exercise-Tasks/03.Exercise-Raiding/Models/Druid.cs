namespace Raiding.Models
{
    public class Druid : BaseHero
    {
        private const int Power = 80;
        public Druid(string name) : base(name, Power)
        {
        }

        public override string CastAbility()
         => $"{GetType().Name} - {Name} healed for {Power}";
    }
}