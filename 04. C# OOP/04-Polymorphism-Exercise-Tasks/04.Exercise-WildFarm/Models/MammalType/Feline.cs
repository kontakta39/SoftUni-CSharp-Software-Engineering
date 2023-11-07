using WildFarm.Models.Interfaces;

namespace WildFarm.Models.MammalType
{
    public abstract class Feline : Mammal, IFeline
    {
        protected Feline(string name, double weight, string livingRegion, string breed) 
            : base(name, weight, livingRegion)
        {
            Breed = breed;
        }

        public string Breed { get; private set; }
    }
}
