using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Data.SQLite;
using PokeApiNet.Data;
using PokeApiNet.Models;
using PokemonBot.Resources.Database;
using PokemonBot.Core.Data;
using Discord.Rest;
namespace PokemonBot.Core.Commands
{
    public class Information : ModuleBase<SocketCommandContext>
    {
        private int pages = 1;

        [Command("select"), Summary("Lets you select which pokemon you want to have set as your partner.")]
        public async Task SelectPokemon(int num)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"UPDATE player SET selected = @selected WHERE Id = '{Context.Message.Author.Id}'";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@selected", num);
            myCommand.Prepare();
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
            Console.WriteLine(database.myConnection.State);
            await Context.Channel.SendMessageAsync($"You have selected your **Level {Data.PokemonData.GetLevel(Context.Message.Author.Id, num)} {Data.PokemonData.GetPokemon(Context.Message.Author.Id, num)}**.");
        }
        [Command("pokemon"), Summary("All the pokemon you caught")]
        public async Task AllPokemon()
        {
            int num = 1;
            int page = 1;
            int pokeNum = Data.PokemonData.GetId(Context.Message.Author.Id);
            string pokemon = "";
            ulong id = Context.Message.Author.Id;
            EmbedBuilder embed = new EmbedBuilder();
            if (Data.PokemonData.GetId(Context.Message.Author.Id) > 15)
            {
                num = 16;
            }
            else
            {
                num = Data.PokemonData.GetId(Context.Message.Author.Id);
            }
            Console.WriteLine(num);
            int i = 1;
            while (i < num)
            {
                Console.WriteLine(i);
                pokemon += $"**{Data.PokemonData.GetPokemon(id, i)}** | Level: {Data.PokemonData.GetLevel(id, i)}\n";
                ++i;
                
            }
            embed.WithDescription(pokemon);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("info"), Summary("Lets you info your pokemon.")]
        public async Task InfoPokemon(string i = "0")
        {
            int num;
            if (i == "latest" || i == "latests")
            {
                num = Data.PokemonData.GetId(Context.Message.Author.Id);
            }
            else
            {
                num = System.Convert.ToInt32(i);
                if (num == 0)
                {
                    num = Data.PokemonData.GetSelected(Context.Message.Author.Id);
                }
            }
            PokeApiClient pokeClient = new PokeApiClient();
            Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(Data.PokemonData.GetPokemon(Context.Message.Author.Id, num));
            EmbedBuilder embed = new EmbedBuilder();
            if (Data.PokemonData.IsShiny(Context.Message.Author.Id, num) == true)
            {
                embed.WithTitle($"Level {Data.PokemonData.GetLevel(Context.Message.Author.Id, num)} {Data.PokemonData.GetPokemon(Context.Message.Author.Id, num)}🌟");
            }
            else
            {
                embed.WithTitle($"Level {Data.PokemonData.GetLevel(Context.Message.Author.Id, num)} {Data.PokemonData.GetPokemon(Context.Message.Author.Id, num)}");
            }


            int hp = ((2 * poke.Stats[5].BaseStat + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 0) + 100) * Data.PokemonData.GetLevel(Context.Message.Author.Id, num)) / 100 + 10;
            float TotalIv = ((Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 0) + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 1) + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 2) + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 3) + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 4) + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 5)) / 186) * 100;
            if (poke.Types.Count == 2)
            {
                embed.WithDescription($"Type: {poke.Types[0].Type.Name} | {poke.Types[1].Type.Name}\n" +
                    $"**Nature:** {Data.PokemonData.GetNature(Context.Message.Author.Id, num)}\n" +
                    $"**HP:** {hp} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 0)}/31\n" +
                    $"**Attack:** {Natures(Context.Message.Author.Id, 0, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 1)}/31\n" +
                    $"**Defense:** {Natures(Context.Message.Author.Id, 1, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 2)}/31\n" +
                    $"**Sp.Atk:** {Natures(Context.Message.Author.Id, 2, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 3)}/31\n" +
                    $"**Sp.Def:** {Natures(Context.Message.Author.Id, 3, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 4)}/31\n" +
                    $"**Speed:** {Natures(Context.Message.Author.Id, 4, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 5)}/31\n" +
                    $"**Total IV %:** {TotalIv}");
            }
            else
            {
                embed.WithDescription($"Type: {poke.Types[0].Type.Name}\n" +
                    $"**Nature:** {Data.PokemonData.GetNature(Context.Message.Author.Id, num)}\n" +
                    $"**HP:** {hp} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 0)}/31\n" +
                    $"**Attack:** {Natures(Context.Message.Author.Id, 0, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 1)}/31\n" +
                    $"**Defense:** {Natures(Context.Message.Author.Id, 1, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 2)}/31\n" +
                    $"**Sp.Atk:** {Natures(Context.Message.Author.Id, 2, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 3)}/31\n" +
                    $"**Sp.Def:** {Natures(Context.Message.Author.Id, 3, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 4)}/31\n" +
                    $"**Speed:** {Natures(Context.Message.Author.Id, 4, poke)} - IV: {Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 5)}/31\n" +
                    $"**Total IV %:** {TotalIv}");

            }

            if (Data.PokemonData.IsShiny(Context.Message.Author.Id, num) == true)
            {
                embed.WithImageUrl(poke.Sprites.FrontShiny);
            }
            else
            {
                embed.WithImageUrl(poke.Sprites.FrontDefault);
            }
            embed.WithFooter($"{num} of {Data.PokemonData.GetId(Context.Message.Author.Id)} Pokemon");

            await Context.Channel.SendMessageAsync("", embed: embed.Build());

        }
        [Command("dex"), Summary("Lets you look at pokemon you don't own or do own.")]
        public async Task PokeDex([Remainder] string name)
        {

            PokeApiClient pokeClient = new PokeApiClient();
                Pokemon poke = await pokeClient.GetResourceAsync<Pokemon>(name);
            

            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle($"Data On {name}");
            if (poke.Types.Count == 2)
            {
                embed.WithDescription($"Type: {poke.Types[0].Type.Name} | {poke.Types[1].Type.Name}\n" +
                    $"**HP:** {poke.Stats[0].BaseStat}\n" +
                    $"**Attack:** {poke.Stats[1].BaseStat}\n" +
                    $"**Defense:** {poke.Stats[2].BaseStat}\n" +
                    $"**Sp.Atk:** {poke.Stats[3].BaseStat}\n" +
                    $"**Sp.Def:** {poke.Stats[4].BaseStat}\n" +
                    $"**Speed:** {poke.Stats[5].BaseStat}\n");
            }
            else
            {
                embed.WithDescription($"Type: {poke.Types[0].Type.Name}\n" +
                    $"**HP:** {poke.Stats[0].BaseStat}\n" +
                    $"**Attack:** {poke.Stats[1].BaseStat}\n" +
                    $"**Defense:** {poke.Stats[2].BaseStat}\n" +
                    $"**Sp.Atk:** {poke.Stats[3].BaseStat}\n" +
                    $"**Sp.Def:** {poke.Stats[4].BaseStat}\n" +
                    $"**Speed:** {poke.Stats[5].BaseStat}\n");

            }
            embed.WithImageUrl(poke.Sprites.FrontDefault);
            await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        public double Natures(ulong Id, int _num, Pokemon poke)
        {
            List<double> Iv = new List<double>();
            ulong user = Id;
            int num = Data.PokemonData.GetId(user);
            int level = Data.PokemonData.GetLevel(user, num);
            string nature = Data.PokemonData.GetNature(user, num);
            int ivAtk = Data.PokemonData.GetIvs(user, num, 1);
            int ivDef = Data.PokemonData.GetIvs(user, num, 2);
            int ivSpatk = Data.PokemonData.GetIvs(user, num, 3);
            int ivSpdef = Data.PokemonData.GetIvs(user, num, 4);
            int ivSpe = Data.PokemonData.GetIvs(user, num, 5);
            double atk = (((2 * poke.Stats[1].BaseStat + ivAtk) * level) / 100);
            double def = (((2 * poke.Stats[2].BaseStat + ivDef) * level) / 100);
            double spatk = (((2 * poke.Stats[3].BaseStat + ivSpatk) * level) / 100);
            double spdef = (((2 * poke.Stats[4].BaseStat + ivSpdef) * level) / 100);
            double spe = (((2 * poke.Stats[5].BaseStat + ivSpe) * level) / 100);
            /**
             * Natures
              */
            //Attacks
            if(nature == "Lonely")
            {
                atk *= 1.10;
                def /= 1.10;
            }
            else if (nature == "Brave")
            {
                atk *= 1.10;
                spe /= 1.10;
            }
            else if (nature == "Adamant")
            {
                atk *= 1.10;
                spatk /= 1.10;
            }
            else if (nature == "Naughty")
            {
                atk *= 1.10;
                spdef /= 1.10;
            }
            else if (nature == "Bold")
            {
                def *= 1.10;
                atk /= 1.10;
            }
            else if (nature == "Relaxed")
            {
                def *= 1.10;
                spe /= 1.10;
            }
            else if(nature == "Impish")
            {
                def *= 1.10;
                spatk /= 1.10;
            }
            else if (nature == "Lax")
            {
                def *= 1.10;
                spdef /= 1.10;
            }
            else if (nature == "Timid")
            {
                spe *= 1.10;
                atk /= 1.10;
            }
            else if (nature == "Hasty")
            {
                spe *= 1.10;
                def /= 1.10;
            }
            else if (nature == "Jolly")
            {
                spe *= 1.10;
                spatk /= 1.10;
            }
            else if (nature == "Naive")
            {
                spe *= 1.10;
                spdef /= 1.10;
            }
            else if (nature == "Modest")
            {
                spatk *= 1.10;
                atk /= 1.10;
            }
            else if (nature == "Mild")
            {
                spatk *= 1.10;
                def /= 1.10;
            }
            else if (nature == "Quiet")
            {
                spatk *= 1.10;
                spe /= 1.10;
            }
            else if (nature == "Rash")
            {
                spatk *= 1.10;
                spdef /= 1.10;
            }
            else if (nature == "Calm")
            {
                spdef *= 1.10;
                atk /= 1.10;
            }
            else if (nature == "Gentle")
            {
                spdef *= 1.10;
                def /= 1.10;
            }
            else if (nature == "Sassy")
            {
                spdef *= 1.10;
                spe /= 1.10;
            }
            else if (nature == "Careful")
            {
                spdef *= 1.10;
                spatk /= 1.10;
            }



            Iv.Add(Math.Floor(Math.Round(atk)));
            Iv.Add(Math.Floor(Math.Round(def)));
            Iv.Add(Math.Floor(Math.Round(spatk)));
            Iv.Add(Math.Floor(Math.Round(spdef)));
            Iv.Add(Math.Floor(Math.Round(spe)));
                return Iv[_num];
        }
    }
}
/**
 *
        if(player[message.author.id].pokemon.id[_id].nature === 'Quirky'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
    */
