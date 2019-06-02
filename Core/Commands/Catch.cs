using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Discord.Commands;
using System.Linq;
using PokeApiNet.Data;
using PokeApiNet.Models;
using PokemonBot.Core.Spawning;
using System.Data.SQLite;
using PokemonBot.Core.Data;
using Newtonsoft.Json;
using System.Data.SQLite;
using PokemonBot.Resources.Database;
namespace PokemonBot.Core.Commands
{
    public class Catch : ModuleBase<SocketCommandContext>
    {
        private List<Id> _ID;
        [Command("catch")]
        public async Task _Catch(string name)
        {
            //if (!Data.Data.HasStarter(Context.Message.Author.Username))
            //{
              //  await Context.Channel.SendMessageAsync("You do not have a starter yet.");
                //return;
            //}
            string result = File.ReadAllText(@"Resources/Spawned.json");
            Spawned spawn = JsonConvert.DeserializeObject<Spawned>(result);
            Console.WriteLine(spawn.name);
            int shiny;
            if (spawn.name == "Taken") return;
            if (name == $"{spawn.name}")
            {
                if (spawn.shiny)
                {
                    shiny = 1;
                }
                else
                {
                    shiny = 0;
                }
                string[] NatureChance = {
                "Hardy",
                "Lonely",
                "Brave",
                "Adamant",
                "Naughty",
                "Bold",
                "Docile",
                "Relaxed",
                "Impish",
                "Lax",
                "Timid",
                "Hasty",
                "Serious",
                "Jolly",
                "Naive",
                "Modest",
                "Mild",
                "Quiet",
                "Bashful",
                "Rash",
                "Calm",
                "Gentle",
                "Sassy",
                "Careful",
                "Quirky"
            };
                int lvl = RandomNumber(1, 50);
                int spawnHp = RandomNumber(1, 31);
                int spawnAtk = RandomNumber(1, 31);
                int spawnDef = RandomNumber(1, 31);
                int spawnSpAtk = RandomNumber(1, 31);
                int spawnSpDef = RandomNumber(1, 31);
                int spawnSpe = RandomNumber(1, 31);
                double ivTotal = (((spawnHp + spawnAtk + spawnDef + spawnSpAtk + spawnSpDef + spawnSpe) / 186) * 100);
                double totalIv = Math.Round(ivTotal);
                int spawnNature = RandomNumber(1, 24);
                string _nature = NatureChance[spawnNature];
                int[] IV = { spawnHp, spawnAtk, spawnDef, spawnSpAtk, spawnSpDef, spawnSpe };
               

                await Data.Data.SavePokemon(Context.Message.Author.Id, spawn.name, lvl, IV, _nature, shiny);
                await Context.Channel.SendMessageAsync($"Congratulations <@{Context.User.Id}>! You caught a **level {lvl} {spawn.name}**! ");
              
                Spawned _spawn = new Spawned()
                {
                    channel = Context.Message.Channel.Id,
                    name = "taken",
                    shiny = false
                };
                string _result = JsonConvert.SerializeObject(_spawn);
                Console.WriteLine(_result);
                File.WriteAllText(@"Resources/Spawned.json", _result);
                Console.WriteLine("Stored!");
            }
        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        }
}
