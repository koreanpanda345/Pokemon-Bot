using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;


using Discord.Commands;
using System.Reflection;
using PokemonBot.Core.Spawning;
using PokemonBot.Core.Xp;
using System.Data.SQLite;
using PokemonBot.Resources.Database;
using PokemonBot.Core.Data;
using System.Collections.Generic;
using System.Linq;


namespace PokemonBot
{
    class Program
    {
        static void Main(string[] args)
       => new PokemonBot().MainAsync().GetAwaiter().GetResult();

        
    }
}
