using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokemonBot.Resources.Database;

namespace PokemonBot.Core.Xp
{
    internal static class Xp
    {
        internal static async void UserSentMessage(SocketGuildUser user, SocketTextChannel channel)
        {
           int Current = Data.PokemonData.GetXp(user.Id, Data.PokemonData.GetSelected(user.Id));
            double NextLevel = Math.Round((4 * (Math.Pow(Data.PokemonData.GetLevel(user.Id, Data.PokemonData.GetSelected(user.Id)), 3)) / 5 * 2));
            if (Current >= NextLevel)
            {
                Data.PokemonData.LevelUp(user.Id, Data.PokemonData.GetSelected(user.Id));
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithTitle($"Congratulations {user.Id}");
                embed.WithDescription($"Your {Data.PokemonData.GetPokemon(user.Id, Data.PokemonData.GetSelected(user.Id))}");

                await channel.SendMessageAsync("", embed: embed.Build());
                Data.PokemonData.ResetXp(user.Id, Data.PokemonData.GetSelected(user.Id));
            }
            else
            { 
                Data.PokemonData.AddXp(user.Id, Data.PokemonData.GetSelected(user.Id), 200);
            }
        }
    }
}
