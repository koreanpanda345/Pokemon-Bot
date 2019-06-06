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

        [Command("select")]
        public async Task SelectPokemon(int num)
        {
            SqliteDbContext database = new SqliteDbContext();
            string query = $"UPDATE player SET selected = @selected WHERE Id = '{Context.Message.Author.Id}'";
            SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
            database.OpenConnection();
            myCommand.Parameters.AddWithValue("@selected", num);
            myCommand.ExecuteNonQuery();
            database.CloseConnection();
            Console.WriteLine(database.myConnection.State);
            await Context.Channel.SendMessageAsync($"You have selected your **Level {Data.PokemonData.GetLevel(Context.Message.Author.Id, num)} {Data.PokemonData.GetPokemon(Context.Message.Author.Id, num)}**.");
        }
        [Command("pokemon")]
        public async Task AllPokemon()
        {
            int num = 1;
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
            while(i < num)
            {
                Console.WriteLine(i);
                pokemon += $"**{Data.PokemonData.GetPokemon(id, i)}** | Level: {Data.PokemonData.GetLevel(id, i)}\n";
                ++i;
            }
            embed.WithDescription(pokemon);
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
        }

        [Command("info")]
        public async Task InfoPokemon(string i = "0")
        {
            int num;
            if(i == "latest")
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
            if(Data.PokemonData.IsShiny(Context.Message.Author.Id, num) == true)
            {
                embed.WithTitle($"Level {Data.PokemonData.GetLevel(Context.Message.Author.Id, num)} {Data.PokemonData.GetPokemon(Context.Message.Author.Id, num)}🌟");
            }
            else
            {
                embed.WithTitle($"Level {Data.PokemonData.GetLevel(Context.Message.Author.Id, num)} {Data.PokemonData.GetPokemon(Context.Message.Author.Id, num)}");
            }


            int hp = ((2 * poke.Stats[5].BaseStat + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 0)+ 100) * Data.PokemonData.GetLevel(Context.Message.Author.Id, num)) / 100 + 10;
            float TotalIv = ((Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 0) + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 1)+ Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 2) + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 3)+ Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 4) + Data.PokemonData.GetIvs(Context.Message.Author.Id, num, 5)) / 186) * 100;
            if(poke.Types.Count == 2)
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

            if(Data.PokemonData.IsShiny(Context.Message.Author.Id, num) == true)
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

        public float Natures(ulong Id, int _num, Pokemon poke)
        {
            List<float> Iv = new List<float>();
            ulong user = Id;
            int num = Data.PokemonData.GetId(user);
            int level = Data.PokemonData.GetLevel(user, num);
            string nature = Data.PokemonData.GetNature(user, num);
            int ivAtk = Data.PokemonData.GetIvs(user, num, 1);
            int ivDef = Data.PokemonData.GetIvs(user, num, 2);
            int ivSpatk = Data.PokemonData.GetIvs(user, num, 3);
            int ivSpdef = Data.PokemonData.GetIvs(user, num, 4);
            int ivSpe = Data.PokemonData.GetIvs(user, num, 5);
            float atk = (((2 * poke.Stats[1].BaseStat + ivAtk) * level) / 100);
            float def = (((2 * poke.Stats[2].BaseStat + ivDef) * level) / 100);
            float spatk = (((2 * poke.Stats[3].BaseStat + ivSpatk) * level) / 100);
            float spdef = (((2 * poke.Stats[4].BaseStat + ivSpdef) * level) / 100);
            float spe = (((2 * poke.Stats[5].BaseStat + ivSpe) * level) / 100);
            /**
             * Natures
              */
            //Attacks
            Iv.Add(atk);
            Iv.Add(def);
            Iv.Add(spatk);
            Iv.Add(spdef);
            Iv.Add(spe);
                return Iv[_num];
        }
    }
}
/**
 * if(player[message.author.id].pokemon.id[_id].nature === 'Hardy'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Lonely'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100) * 1.10;
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100) / 1.10;
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Brave'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100) * 1.10;
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100) / 1.10;
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Adamant'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100) * 1.10;
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100) / 1.10;
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Naughty'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100) * 1.10;
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100) / 1.10;
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Bold'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100) / 1.10;
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100) * 1.10;
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Docile'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Relaxed'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100) * 1.10;
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100) / 1.10;
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Impish'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100) * 1.10;
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100) / 1.10;
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Lax'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100) * 1.10;
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100) / 1.10;
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Timid'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100) / 1.10;
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100) * 1.10;
        }        
        if(player[message.author.id].pokemon.id[_id].nature === 'Hasty'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100) / 1.10;
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100) * 1.10;
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Serious'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Jolly'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100) / 1.10;
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100) * 1.10;
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Naive'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100) / 1.10;
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100) * 1.10;
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Modest'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100) / 1.10;
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100) / 1.10;
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Mild'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100) / 1.10;
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100) * 1.10;
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Quiet'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100) * 1.10;
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100) / 1.10;
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Bashful'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Rash'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100) * 1.10;
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100) / 1.10;
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Calm'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100) / 1.10;
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100) * 1.10;
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Gentle'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100) / 1.10;
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100) * 1.10;
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Sassy'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100) * 1.10;
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100) / 1.10;
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Careful'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100) / 1.10;
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100) * 1.10;
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
        if(player[message.author.id].pokemon.id[_id].nature === 'Quirky'){
            atk = (((2 * infoPokemon.base_stats.atk + _atk) * _lvl) / 100 + 100);
            def = (((2 * infoPokemon.base_stats.def + _def) * _lvl) / 100 + 100);
            spatk = (((2 * infoPokemon.base_stats.sp_atk + _spatk) * _lvl) / 100 + 100);
            spdef = (((2 * infoPokemon.base_stats.sp_def + _spdef) * _lvl) / 100 + 100);
            spe = (((2 * infoPokemon.base_stats.speed + _spe) * _lvl) / 100 + 100);
        }
    */
