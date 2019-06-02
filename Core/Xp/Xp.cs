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
           int Current = Data.Data.GetXp(user.Id, Data.Data.GetSelected(user.Id));
            double NextLevel = Math.Round((4 * (Math.Pow(Data.Data.GetLevel(user.Id, Data.Data.GetSelected(user.Id)), 3)) / 5 * 2));
            if (Current >= NextLevel)
            {
                Data.Data.LevelUp(user.Id, Data.Data.GetSelected(user.Id));
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithTitle($"Congratulations {user.Id}");
                embed.WithDescription($"Your {Data.Data.GetPokemon(user.Id, Data.Data.GetSelected(user.Id))}");

                await channel.SendMessageAsync("", embed: embed.Build());
                Data.Data.ResetXp(user.Id, Data.Data.GetSelected(user.Id));
            }
            else
            { 
                Data.Data.AddXp(user.Id, Data.Data.GetSelected(user.Id), 200);
            }
        }
    }
}
