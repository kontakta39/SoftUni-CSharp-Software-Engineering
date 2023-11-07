namespace Raiding.Models.Interfaces
{
    public interface IBaseHero
    {
        string Name { get; }
        int Power { get; }
        public string CastAbility();
    }
}