//9 Exercise - Pokemon Trainer
using PokemonTrainer;

public class StartUp
{
    static void Main()
    {
        List<Trainer> trainers = new();
        string input = Console.ReadLine();

        while (input != "Tournament")
        {
            string[] trainerInfo = input
                .Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string trainerName = trainerInfo[0];
            string pokemonName = trainerInfo[1];
            string pokemonElement = trainerInfo[2];
            int pokemonHealth = int.Parse(trainerInfo[3]);

            bool isContained = false;

            foreach (var item in trainers)
            {
                if (item.Name == trainerName)
                {
                    Pokemon addPokemon = new(pokemonName, pokemonElement, pokemonHealth);
                    item.Pokemon.Add(addPokemon);
                    isContained = true;
                }
            }

            if (isContained == false)
            {
                List<Pokemon> pokemons = new List<Pokemon>();
                Pokemon pokemon = new(pokemonName, pokemonElement, pokemonHealth);
                pokemons.Add(pokemon);

                Trainer trainer = new(trainerName, pokemons);

                trainers.Add(trainer);
            }

            input = Console.ReadLine();
        }

        string command = Console.ReadLine();

        while (command != "End")
        {
            foreach (var trainer in trainers)
            {
                trainer.CheckElement(command);
            }

            command = Console.ReadLine();
        }

        foreach (var trainer in trainers.OrderByDescending(x => x.Badges))
        {
            Console.WriteLine($"{trainer.Name} {trainer.Badges} {trainer.Pokemon.Count}");
        }
    }
}

