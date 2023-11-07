namespace PokemonTrainer;

public class Trainer
{
    public Trainer(string trainerName, List<Pokemon> currentPokemon)
    {
        Name = trainerName;
        Badges = 0;
        Pokemon = currentPokemon;
    }

    public string Name { get; set; }
    public int Badges { get; set; }
    public List<Pokemon> Pokemon { get; set; }

    public void CheckElement(string command)
    {
        if (Pokemon.Any(x => x.Element == command))
        {
            Badges++;
        }

        else
        {
            for (int i = 0; i < Pokemon.Count; i++)
            {
                Pokemon[i].Health -= 10;

                if (Pokemon[i].Health <= 0)
                {
                    Pokemon.Remove(Pokemon[i]);
                }
            }
        }
    }
}

