using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace PokemonBot.Core
{
    public class Trade : ModuleBase<SocketCommandContext>
    {
        [Command("trade")]
        [Summary("Allows you to trade with someone.")]
        public async Task AskTrade(IGuildUser user)
        {

        }
    }
}
