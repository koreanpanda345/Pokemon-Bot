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

    public class PokemonBot
    {
            private bool BeingTested = false;
    private DiscordSocketClient client;
    private CommandService command;
        private IServiceProvider service;
        public PokemonBot()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });

            command = new CommandService(new CommandServiceConfig
            {
                CaseSensitiveCommands = true,
                DefaultRunMode = RunMode.Async,
                LogLevel = LogSeverity.Critical
            });
        }
        public async Task MainAsync()
        {

            var cmdHandler = new CommandHandler(client, command);
            await cmdHandler.InitializeAsync();

            client.Ready += Client_Ready;
            client.Log += Client_Log;
            if (Config.bot.token == "" || Config.bot.token == null) return;

            //using (var Stream = new FileStream(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location).Replace(@"bin\Debug\netcoreapp2.1", @"Data\Token.text"), FileMode.Open, FileAccess.Read))
            //using (var ReadToken = new StreamReader(Stream))
            //{
            //    Token = ReadToken.ReadToEnd();
            //}
            await client.LoginAsync(TokenType.Bot, Config.bot.token);
            await client.StartAsync();
            await ConsoleInput();
            await Task.Delay(-1);
        }
        private async Task ConsoleInput()
        {
            string input = string.Empty;
            while (input.Trim().ToLower() != "block")
            {
                input = Console.ReadLine();
                if (input.Trim().ToLower() == "message")
                {
                    ConsoleSendMessage();
                }
                if (input.Trim().ToLower() == "spawn")
                {
                    SpawnPokemon();
                }
                if (input.Trim().ToLower() == "shinyspawn")
                {
                    ShinySpawnPokemon();
                }
                if (input.Trim().ToLower() == "update")
                {
                    ConsoleUpdate();
                }
                if (input.Trim().ToLower() == "sql")
                {
                    SQLConsole();
                }
                if (input.Trim().ToLower() == "close")
                {
                    CloseDatabase();
                }
                if (input.Trim().ToLower() == "connection")
                {
                    Connection();
                }
                if (input.Trim().ToLower() == "status")
                {
                    ClientStatus();
                }
            }
        }
        private async void ClientStatus()
        {

            Console.WriteLine($"Bot is {client.ConnectionState}\nUsername: {client.CurrentUser}\nBot Status: {client.Status}\nActivity set to: {client.Activity.Name}\nGuilds Count: {client.Guilds.Count()}\nGuild Name");
            var socketGuilds = client.Guilds.ToList();
            var maxIndex = client.Guilds.Count();
            for (var i = 0; i < maxIndex; ++i)
            {
                Console.WriteLine($"{i + 1} - {socketGuilds[i]}");
            }
        }
        private async void Connection()
        {
            SqliteDbContext database = new SqliteDbContext();
            Console.WriteLine($"Database Connection is {database.myConnection.State}");
            Console.WriteLine($"Database is {database.myConnection.BusyTimeout}");
        }
        private async void CloseDatabase()
        {
            SqliteDbContext database = new SqliteDbContext();
            if (database.myConnection.State == System.Data.ConnectionState.Open)
            {
                database.CloseConnection();
                Console.WriteLine("Closed database");
            }
            else if (database.myConnection.State == System.Data.ConnectionState.Closed)
            {
                Console.WriteLine("database is already closed");
            }
        }
        private async void SQLConsole()
        {
            var command = string.Empty;
            while (command.Trim() == string.Empty)
            {
                Console.WriteLine("SQL COMMAND");
                command = Console.ReadLine();
                SqliteDbContext database = new SqliteDbContext();
                string query = $"{command}";
                SQLiteCommand myCommand = new SQLiteCommand(query, database.myConnection);
                database.OpenConnection();
                SQLiteDataReader result = myCommand.ExecuteReader();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        if (command.Contains("*"))
                        {
                            Console.WriteLine();
                        }
                        else
                        {
                            Console.WriteLine(result);
                        }

                    }
                }
                result.Close();
                database.CloseConnection();
            }
        }
        private async void ConsoleUpdate()
        {
            var msg = string.Empty;
            while (msg.Trim() == string.Empty)
            {
                Console.WriteLine("Your Message: ");
                msg = Console.ReadLine();
            }
            await client.SetGameAsync($"{msg}", "", ActivityType.Watching);
            await client.SetStatusAsync(UserStatus.DoNotDisturb);
        }
        private async void ConsoleSendMessage()
        {
            Console.WriteLine("Select the guild:");
            var guild = GetSelectedGuild(client.Guilds);
            var textChannel = GetSelectedTextChannel(guild.TextChannels);
            var msg = string.Empty;
            while (msg.Trim() == string.Empty)
            {
                Console.WriteLine("Your Message: ");
                msg = Console.ReadLine();
            }
            await textChannel.SendMessageAsync(msg);
        }
        private void SpawnPokemon()
        {
            Console.WriteLine("Select the guild: ");
            var guild = GetSelectedGuild(client.Guilds);
            var textChannel = GetSelectedTextChannel(guild.TextChannels);
            var msg = string.Empty;
            while (msg.Trim() == string.Empty)
            {
                Console.WriteLine("Pokemon: ");
                msg = Console.ReadLine();
            }
            Spawn.SpawnPokemon(guild, textChannel, msg);
        }
        private void ShinySpawnPokemon()
        {
            Console.WriteLine("Select the guild: ");
            var guild = GetSelectedGuild(client.Guilds);
            var textChannel = GetSelectedTextChannel(guild.TextChannels);
            var msg = string.Empty;
            while (msg.Trim() == string.Empty)
            {
                Console.WriteLine("Pokemon: ");
                msg = Console.ReadLine();
            }
            Spawn.SpawnPokemon(guild, textChannel, msg);
        }
        private SocketTextChannel GetSelectedTextChannel(IEnumerable<SocketTextChannel> channels)
        {
            var textChannels = channels.ToList();
            var maxIndex = channels.Count() - 1;
            for (var i = 0; i <= maxIndex; ++i)
            {
                Console.WriteLine($"{i} - {textChannels[i].Name}");
            }
            var selectedIndex = -1;
            while (selectedIndex < 0 || selectedIndex > maxIndex)
            {
                var success = int.TryParse(Console.ReadLine().Trim(), out selectedIndex);
                if (!success)
                {
                    Console.WriteLine("That was an Invalid Index, try again.");
                    selectedIndex = -1;
                }
                if (selectedIndex < 0 || selectedIndex > maxIndex) Console.WriteLine("This is a valid index");
            }

            return textChannels[selectedIndex];
        }

        private SocketGuild GetSelectedGuild(IEnumerable<SocketGuild> guilds)
        {
            var socketGuilds = guilds.ToList();
            var maxIndex = guilds.Count() - 1;
            for (var i = 0; i <= maxIndex; ++i)
            {
                Console.WriteLine($"{i} - {socketGuilds[i].Name}");
            }
            var selectedIndex = -1;
            while (selectedIndex < 0 || selectedIndex > maxIndex)
            {
                var success = int.TryParse(Console.ReadLine().Trim(), out selectedIndex);
                if (!success)
                {
                    Console.WriteLine("That was an Invalid Index, try again.");
                    selectedIndex = -1;
                }

                if (selectedIndex < 0 || selectedIndex > maxIndex) Console.WriteLine("This is a valid index");
            }

            return socketGuilds[selectedIndex];
        }


        SqliteDbContext database = new SqliteDbContext();

        private async Task Client_Log(LogMessage Message)
        {
            Console.WriteLine($"{DateTime.Now} at {Message.Source} {Message.Message}");

        }

        private async Task Client_Ready()
        {
            if (BeingTested)
            {
                await client.SetGameAsync("Pokemon bot is being tested", "", ActivityType.Watching);
                await client.SetStatusAsync(UserStatus.DoNotDisturb);
            }
            else
            {
                await client.SetGameAsync("Prefix `p.`", "", ActivityType.Watching);
            }

        }
    }
}
