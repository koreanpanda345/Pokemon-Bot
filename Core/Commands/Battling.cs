using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using System.Linq;
using NReco.ImageGenerator;
using PokeApiNet.Data;
using PokeApiNet.Models;
using PokemonBot.Core.Data;
namespace PokemonBot.Core.Commands
{
    public class Battling : ModuleBase<SocketCommandContext>
    {
        private IGuildUser Challenger;
        private IGuildUser Challenged;
        private int CurrentHp1;
        private int CurrentHp2;
        private int duelid;
        [Command("duel")]
        public async Task duel(IGuildUser user)
        {
            Challenger = (IGuildUser)Context.Message.Author;
            await Data.BattleData.CreateBattle(Context.Message.Author.Id, user.Id);
            await Context.Channel.SendMessageAsync($"{user.Username}, {Context.Message.Author.Username} Has challenged you.");
        }
        [Command("accept")]
        public async Task acceptChallenge()
        {
            Challenged = (IGuildUser)Context.Message.Author;
            await Data.BattleData.StartBattle(Context.Message.Author.Id);
            await StartDuel(Context.Message.Author.Id);
        }
        public async Task StartDuel(ulong id)
        {
            int Duelid = Data.BattleData.GetDuelId(id);
            ulong id1 = Data.BattleData.GetChallenger(Duelid);
            PokeApiClient pokeClient = new PokeApiClient();

            int num1 = Data.PokemonData.GetSelected(id1);
            int num2 = Data.PokemonData.GetSelected(id);


            Pokemon poke1 = await pokeClient.GetResourceAsync<Pokemon>(Data.PokemonData.GetPokemon(id1, num1));
            Pokemon poke2 = await pokeClient.GetResourceAsync<Pokemon>(Data.PokemonData.GetPokemon(id, num1));
            int hp1 = ((2 * poke1.Stats[5].BaseStat + Data.PokemonData.GetIvs(id1, num1, 0) + 100) * Data.PokemonData.GetLevel(id1, num1)) / 100 + 10;
            int hp2 = ((2 * poke2.Stats[5].BaseStat + Data.PokemonData.GetIvs(id, num2, 0) + 100) * Data.PokemonData.GetLevel(id, num2)) / 100 + 10;
            string pokeSprite1 = poke1.Sprites.FrontDefault.ToString();
            string pokeSprite2 = poke2.Sprites.FrontDefault.ToString();
            int CurrentHp1 = hp1;
            int CurrentHp2 = hp2;
            EmbedBuilder embed1 = new EmbedBuilder();
            EmbedBuilder embed2 = new EmbedBuilder();

            embed1.WithTitle($"{Challenged.Username}'s {Data.PokemonData.GetPokemon(Challenged.Id, num1)}");
            embed1.WithDescription($"{hp1}/{hp1}");
            embed1.WithImageUrl(pokeSprite1);

            embed2.WithTitle($"{Challenger.Username}'s {Data.PokemonData.GetPokemon(Challenger.Id, num2)}");
            embed2.WithDescription($"{hp2}/{hp2}");
            embed2.WithImageUrl(pokeSprite2);

            await Context.Channel.SendMessageAsync("", embed: embed1.Build());

            await Context.Channel.SendMessageAsync("V.S.");

            await Context.Channel.SendMessageAsync("", embed: embed2.Build());
            IGuildUser user1 = Challenged;
            Move move11 = await pokeClient.GetResourceAsync<Move>(Data.PokemonData.GetMoves(user1.Id, 1));
            EmbedBuilder embed3 = new EmbedBuilder();
            embed3.WithTitle($"Your {Data.PokemonData.GetPokemon(user1.Id, Data.PokemonData.GetSelected(user1.Id))} Moves");
            embed3.AddField($"{Data.PokemonData.GetMoves(user1.Id, 1)}", $"Power: {move11.Power}");
            embed3.AddField($"{Data.PokemonData.GetMoves(user1.Id, 2)}", "1");
            embed3.AddField($"{Data.PokemonData.GetMoves(user1.Id, 3)}", "1");
            embed3.AddField($"{Data.PokemonData.GetMoves(user1.Id, 4)}", "1");
            await user1.SendMessageAsync("", embed: embed3.Build());
            IGuildUser user2 = Challenged;
            Move move21 = await pokeClient.GetResourceAsync<Move>(Data.PokemonData.GetMoves(user1.Id, 1));
            EmbedBuilder embed4 = new EmbedBuilder();
            embed4.WithTitle($"Your {Data.PokemonData.GetPokemon(user2.Id, Data.PokemonData.GetSelected(user2.Id))} Moves");
            embed4.AddField($"{Data.PokemonData.GetMoves(user2.Id, 1)}", $"Power: {move21.Power}");
            embed4.AddField($"{Data.PokemonData.GetMoves(user2.Id, 2)}", "1");
            embed4.AddField($"{Data.PokemonData.GetMoves(user2.Id, 3)}", "1");
            embed4.AddField($"{Data.PokemonData.GetMoves(user2.Id, 4)}", "1");
            await user2.SendMessageAsync("", embed: embed4.Build());
        }
        [Command("testbattle")]
        public async Task TestBattle()
        {
            PokeApiClient pokeClient = new PokeApiClient();
            
            int num1 = Data.PokemonData.GetSelected(Context.Message.Author.Id);
            int num2 = Data.PokemonData.GetSelected(Context.Message.Author.Id);


            Pokemon poke1 = await pokeClient.GetResourceAsync<Pokemon>(Data.PokemonData.GetPokemon(Context.Message.Author.Id,num1));
            Pokemon poke2 = await pokeClient.GetResourceAsync<Pokemon>(Data.PokemonData.GetPokemon(Context.Message.Author.Id, num1));
            int hp1 = ((2 * poke1.Stats[5].BaseStat + Data.PokemonData.GetIvs(Context.Message.Author.Id, num1, 0) + 100) * Data.PokemonData.GetLevel(Context.Message.Author.Id, num1)) / 100 + 10;
            int hp2 = ((2 * poke2.Stats[5].BaseStat + Data.PokemonData.GetIvs(Context.Message.Author.Id, num2, 0) + 100) * Data.PokemonData.GetLevel(Context.Message.Author.Id, num2)) / 100 + 10;
            string pokeSprite1 = poke1.Sprites.FrontDefault.ToString();
            string pokeSprite2 = poke2.Sprites.FrontDefault.ToString();

            int CurrentHp1 = hp1;
            int CurrentHp2 = hp2;

            EmbedBuilder embed1 = new EmbedBuilder();
            EmbedBuilder embed2 = new EmbedBuilder();

            embed1.WithTitle($"{Context.Message.Author.Username}'s {Data.PokemonData.GetPokemon(Context.Message.Author.Id, num1)}");
            embed1.WithDescription($"{CurrentHp1}/{hp1}");
            embed1.WithImageUrl(pokeSprite1);

            embed2.WithTitle($"{Context.Message.Author.Username}'s {Data.PokemonData.GetPokemon(Context.Message.Author.Id, num2)}");
            embed2.WithDescription($"{CurrentHp2}/{hp2}");
            embed2.WithImageUrl(pokeSprite2);

            await Context.Channel.SendMessageAsync("", embed: embed1.Build());

            await Context.Channel.SendMessageAsync("V.S.");

            await Context.Channel.SendMessageAsync("", embed: embed2.Build());
            IGuildUser user1 = (IGuildUser)Context.Message.Author;
            Move move11 = await pokeClient.GetResourceAsync<Move>(Data.PokemonData.GetMoves(user1.Id, 1));
            EmbedBuilder embed3 = new EmbedBuilder();
            embed3.WithTitle($"Your {Data.PokemonData.GetPokemon(user1.Id, Data.PokemonData.GetSelected(user1.Id))} Moves");
            embed3.AddField($"{Data.PokemonData.GetMoves(user1.Id, 1)}", $"Power: {move11.Power}");
            embed3.AddField($"{Data.PokemonData.GetMoves(user1.Id, 2)}", "1");
            embed3.AddField($"{Data.PokemonData.GetMoves(user1.Id, 3)}", "1");
            embed3.AddField($"{Data.PokemonData.GetMoves(user1.Id, 4)}", "1");
            await user1.SendMessageAsync("", embed: embed3.Build());
        }
        [Command("use")]
        public async Task UseMove(string move)
        {
            PokeApiClient pokeClient = new PokeApiClient();
            Move move11 = await pokeClient.GetResourceAsync<Move>(move);
            int damage1 =DamageDealt(move11);
            int damage2 = DamageDealt(move11);
            await DuringBattle((IGuildUser) Context.Message.Author, damage1, damage2);
        }
        public int DamageDealt(Move move)
        {
            return (int)move.Power;
        }
        public async Task DuringBattle(IGuildUser User, int damageToChallenger, int damageToChallenged)
        {
            PokeApiClient pokeClient = new PokeApiClient();

            int num1 = Data.PokemonData.GetSelected(User.Id);
            int num2 = Data.PokemonData.GetSelected(User.Id);


            Pokemon poke1 = await pokeClient.GetResourceAsync<Pokemon>(Data.PokemonData.GetPokemon(User.Id, num1));
            Pokemon poke2 = await pokeClient.GetResourceAsync<Pokemon>(Data.PokemonData.GetPokemon(User.Id, num1));
            int hp1 = ((2 * poke1.Stats[5].BaseStat + Data.PokemonData.GetIvs(User.Id, num1, 0) + 100) * Data.PokemonData.GetLevel(User.Id, num1)) / 100 + 10;
            int hp2 = ((2 * poke2.Stats[5].BaseStat + Data.PokemonData.GetIvs(User.Id, num2, 0) + 100) * Data.PokemonData.GetLevel(User.Id, num2)) / 100 + 10;
            string pokeSprite1 = poke1.Sprites.FrontDefault.ToString();
            string pokeSprite2 = poke2.Sprites.FrontDefault.ToString();

            EmbedBuilder embed1 = new EmbedBuilder();
            EmbedBuilder embed2 = new EmbedBuilder();
            CurrentHp1 -= damageToChallenger;
            CurrentHp2 -= damageToChallenged;
            embed1.WithTitle($"{User.Username}'s {Data.PokemonData.GetPokemon(User.Id, num1)}");
            embed1.WithDescription($"{CurrentHp1}/{hp1}");
            embed1.WithImageUrl(pokeSprite1);

            embed2.WithTitle($"{User.Username}'s {Data.PokemonData.GetPokemon(User.Id, num2)}");
            embed2.WithDescription($"{CurrentHp2}/{hp2}");
            embed2.WithImageUrl(pokeSprite2);

            await Context.Channel.SendMessageAsync("", embed: embed1.Build());

            await Context.Channel.SendMessageAsync("V.S.");

            await Context.Channel.SendMessageAsync("", embed: embed2.Build());
        }
    }
}
