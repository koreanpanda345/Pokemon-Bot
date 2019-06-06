using System;
using System.Collections.Generic;
using System.Text;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Linq;

namespace PokemonBot.Core.Data
{
    internal static class Global
    {
        internal static ulong MessageIdToTrack { get; set; }
    }
}
