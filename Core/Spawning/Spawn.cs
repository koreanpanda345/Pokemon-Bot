using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.WebSocket;
using PokeApiNet.Data;
using PokeApiNet.Models;
using System.Data.SQLite;
using System.IO;
using Newtonsoft.Json;
namespace PokemonBot.Core.Spawning
{
   public class Spawned
    {
        public ulong channel { get; set; }
        public string name { get; set; }
        public bool shiny { get; set; }
        public override string ToString()
        {
            return string.Format("Spawn Information:\n\t Channel ID: {0}, Pokemon: {1}, is shiny: {2}", channel, name, shiny);
        }
    }
    internal static class Spawn
    {
        internal static async void ShinySpawnPokemon(SocketGuild guild, SocketTextChannel channel, string PokemonName)
        {
            PokeApiClient pokeClient = new PokeApiClient();
                Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(PokemonName);
                Spawned spawn = new Spawned()
                {
                    channel = channel.Id,
                    name = poke.Name,
                    shiny = true
                };
                string result = JsonConvert.SerializeObject(spawn);
                Console.WriteLine(result);
                File.WriteAllText(@"Resources/Spawned.json", result);
                Console.WriteLine("Stored!");

                result = String.Empty;
                result = File.ReadAllText(@"Resources/Spawned.json");
                Spawned resultSpawn = JsonConvert.DeserializeObject<Spawned>(result);
                Console.WriteLine(resultSpawn.ToString());

                Console.WriteLine($"{poke.Name}");
                var embed = new EmbedBuilder();
                embed.WithTitle("A wild pokemon has appeared!");
                embed.WithDescription("Guess the pokemon and type p.catch <pokemon> to catch it");
                embed.WithImageUrl($"{poke.Sprites.FrontShiny}");

                await channel.SendMessageAsync("", embed: embed.Build());
            
        }
            internal static async void SpawnPokemon(SocketGuild guild, SocketTextChannel channel, string PokemonName)
        {
            PokeApiClient pokeClient = new PokeApiClient();
                int shinyChance1 = RandomNumber(1, 5);
                int shinyChance2 = RandomNumber(1, 5);
                Console.WriteLine($"{shinyChance1} - {shinyChance2}");
                if (shinyChance1 == shinyChance2)
                {
                    Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(PokemonName);
                    Spawned spawn = new Spawned()
                    {
                        channel = channel.Id,
                        name = poke.Name,
                        shiny = true
                    };
                    string result = JsonConvert.SerializeObject(spawn);
                    Console.WriteLine(result);
                    File.WriteAllText(@"Resources/Spawned.json", result);
                    Console.WriteLine("Stored!");

                    result = String.Empty;
                    result = File.ReadAllText(@"Resources/Spawned.json");
                    Spawned resultSpawn = JsonConvert.DeserializeObject<Spawned>(result);
                    Console.WriteLine(resultSpawn.ToString());

                    Console.WriteLine($"{poke.Name}");
                    var embed = new EmbedBuilder();
                    embed.WithTitle("A wild pokemon has appeared!");
                    embed.WithDescription("Guess the pokemon and type p.catch <pokemon> to catch it");
                    embed.WithImageUrl($"{poke.Sprites.FrontShiny}");

                    await channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {
                    Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(PokemonName);
                    Spawned spawn = new Spawned()
                    {
                        channel = channel.Id,
                        name = poke.Name,
                        shiny = false
                    };
                    string result = JsonConvert.SerializeObject(spawn);
                    Console.WriteLine(result);
                    File.WriteAllText(@"Resources/Spawned.json", result);
                    Console.WriteLine("Stored!");

                    result = String.Empty;
                    result = File.ReadAllText(@"Resources/Spawned.json");
                    Spawned resultSpawn = JsonConvert.DeserializeObject<Spawned>(result);
                    Console.WriteLine(resultSpawn.ToString());

                    Console.WriteLine($"{poke.Name}");
                    var embed = new EmbedBuilder();
                    embed.WithTitle("A wild pokemon has appeared!");
                    embed.WithDescription("Guess the pokemon and type p.catch <pokemon> to catch it");
                    embed.WithImageUrl($"{poke.Sprites.FrontDefault}");

                    await channel.SendMessageAsync("", embed: embed.Build());
                }
        }
        internal static async void UserSentMessage(SocketGuildUser user, SocketTextChannel channel)
        {
            PokeApiClient pokeClient = new PokeApiClient();
            int rand1 = RandomNumber(1, 30);
            int rand2 = RandomNumber(1, 30);
            Console.WriteLine($"{rand1} - {rand2}");
            if (rand1 == rand2)
            {
                int shinyChance1 = RandomNumber(1, 5);
                int shinyChance2 = RandomNumber(1, 5);
                Console.WriteLine($"{shinyChance1} - {shinyChance2}");
                if (shinyChance1 == shinyChance2)
                {
                    int pokeId = RandomNumber(1, 808);
                    Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(pokeId);
                    Spawned spawn = new Spawned()
                    {
                        channel = channel.Id,
                        name = poke.Name,
                        shiny = true
                    };
                    string result = JsonConvert.SerializeObject(spawn);
                    Console.WriteLine(result);
                    File.WriteAllText(@"Resources/Spawned.json", result);
                    Console.WriteLine("Stored!");

                    result = String.Empty;
                    result = File.ReadAllText(@"Resources/Spawned.json");
                    Spawned resultSpawn = JsonConvert.DeserializeObject<Spawned>(result);
                    Console.WriteLine(resultSpawn.ToString());

                    Console.WriteLine($"{poke.Name}");
                    var embed = new EmbedBuilder();
                    embed.WithTitle("A wild pokemon has appeared!");
                    embed.WithDescription("Guess the pokemon and type p.catch <pokemon> to catch it");
                    embed.WithImageUrl($"{poke.Sprites.FrontShiny}");

                    await channel.SendMessageAsync("", embed: embed.Build());
                }
                else
                {


                    int pokeId = RandomNumber(1, 808);
                    Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(pokeId);
                    Spawned spawn = new Spawned()
                    {
                        channel = channel.Id,
                        name = poke.Name,
                        shiny = false
                    };
                    string result = JsonConvert.SerializeObject(spawn);
                    Console.WriteLine(result);
                    File.WriteAllText(@"Resources/Spawned.json", result);
                    Console.WriteLine("Stored!");

                    result = String.Empty;
                    result = File.ReadAllText(@"Resources/Spawned.json");
                    Spawned resultSpawn = JsonConvert.DeserializeObject<Spawned>(result);
                    Console.WriteLine(resultSpawn.ToString());

                    Console.WriteLine($"{poke.Name}");
                    var embed = new EmbedBuilder();
                    embed.WithTitle("A wild pokemon has appeared!");
                    embed.WithDescription("Guess the pokemon and type p.catch <pokemon> to catch it");
                    embed.WithImageUrl($"{poke.Sprites.FrontDefault}");

                    await channel.SendMessageAsync("", embed: embed.Build());
                }
            }
        }
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}
