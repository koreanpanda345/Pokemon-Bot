using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using PokemonBot.Core.Data;
using PokeApiNet.Data;
using PokeApiNet.Models;
using Discord;
using Discord.WebSocket;

namespace PokemonBot.Core.Xp
{
    internal static class Evolve
    {
        internal static async void Evolving(SocketGuildUser user, SocketTextChannel channel)
        {
            int selected = Data.PokemonData.GetSelected(user.Id);
            string pokeName = Data.PokemonData.GetPokemon(user.Id, selected);
            int level = Data.PokemonData.GetLevel(user.Id, selected);
            PokeApiClient pokeClient = new PokeApiClient();
            Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(pokeName);
            EvolutionChain pokeEvolution = await pokeClient.GetResourceAsync<EvolutionChain>(pokeName);
            if (pokeEvolution.Chain.EvolvesTo.Count >= 1)
            {
                if (level >= pokeEvolution.Chain.EvolvesTo[0].EvolutionDetails[0].MinLevel)
                {
                    await Data.PokemonData.EvolvePokemon(user.Id, selected, pokeEvolution.Chain.EvolvesTo[0].Species.Name);
                    await channel.SendMessageAsync($"Your {Data.PokemonData.GetPokemon(user.Id, selected)} has evolved into {pokeEvolution.Chain.EvolvesTo[0].Species.Name}");
                }
            }
        }
    }
}
