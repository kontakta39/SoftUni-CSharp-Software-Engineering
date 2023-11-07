using Raiding.Core.Interfaces;
using Raiding.Models;
using Raiding.Models.Interfaces;

namespace Raiding.Core;

public class Engine : IEngine
{
    public void Run()
    {
        List<IBaseHero> heroes = new();
        int counter = int.Parse(Console.ReadLine());
        int heroesCount = 0;

        while (heroesCount != counter)
        {
            string heroName = Console.ReadLine();
            string heroType = Console.ReadLine();

            try
            {
                if (heroType == "Druid")
                {
                    IBaseHero hero = new Druid(heroName);
                    heroes.Add(hero);
                    heroesCount++;
                }

                else if (heroType == "Paladin")
                {
                    IBaseHero hero = new Paladin(heroName);
                    heroes.Add(hero);
                    heroesCount++;
                }

                else if (heroType == "Rogue")
                {
                    IBaseHero hero = new Rogue(heroName);
                    heroes.Add(hero);
                    heroesCount++;
                }

                else if (heroType == "Warrior")
                {
                    IBaseHero hero = new Warrior(heroName);
                    heroes.Add(hero);
                    heroesCount++;
                }

                else
                {
                    throw new ArgumentException("Invalid hero!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        int bossPower = int.Parse(Console.ReadLine());
        int heroesPower = 0;

        foreach (var item in heroes)
        {
            heroesPower += item.Power;
            Console.WriteLine(item.CastAbility());
        }

        if (heroesPower >= bossPower)
        {
            Console.WriteLine("Victory!");
        }

        else
        {
            Console.WriteLine("Defeat...");
        }
    }
}