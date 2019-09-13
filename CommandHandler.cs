using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using Discord.Commands;
using System.Reflection;
using System.Linq;
using PokemonBot.Core.Spawning;
using PokemonBot.Core.Xp;
using PokemonBot.Core.Data;

namespace PokemonBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient Client;
        private readonly CommandService Command;
        public CommandHandler(DiscordSocketClient Client, CommandService Command)
        {
            this.Client = Client;
            this.Command = Command;
        }

        public async Task InitializeAsync()
        {
            await Command.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            Command.Log += Command_Log;
            Client.MessageReceived += Client_MessageReceived;
        }

        private async Task Client_MessageReceived(SocketMessage MessageParam)
        {
            var Message = MessageParam as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);

            if (Context.Message == null || Context.Message.Content == "") return;
            if (Context.User.IsBot) return;
            Spawn.UserSentMessage((SocketGuildUser)Context.User, (SocketTextChannel)Context.Channel);

            //Xp.UserSentMessage((SocketGuildUser)Context.User, (SocketTextChannel)Context.Channel);
            int ArgsPos = 0;
            if (!(Message.HasStringPrefix("p.", ref ArgsPos) || Message.HasMentionPrefix(Client.CurrentUser, ref ArgsPos)))
            {

                if (PokemonData.HasStarter(Context.Message.Author.Id))
                {
                    Xp.UserSentMessage((SocketGuildUser)Context.User, (SocketTextChannel)Context.Channel);
                }
                return;
            }
            var Result = await Command.ExecuteAsync(Context, ArgsPos, null);
            if (!Result.IsSuccess && Result.Error != CommandError.UnknownCommand)
            {

                Console.WriteLine($"{DateTime.Now} at Command: {Command.Search(Context, ArgsPos).Commands[0].Command.Name} in {Command.Search(Context, ArgsPos).Commands[0].Command.Module.Name}] {Result.ErrorReason}");
                var embed = new EmbedBuilder();

                if (Result.ErrorReason == "The input text has too few parameters.")
                {
                    if (Command.Search(Context, ArgsPos).Commands[0].Command.Name == "pick")
                    {
                        embed.WithTitle("INVALID CHOICE");
                        embed.WithDescription("please pick a pokemon");
                    }
                    else
                    {
                        embed.WithTitle("***ERROR***");
                        embed.WithDescription("This command requires something, check help command to see what it needs.");
                    }

                }
                else
                {
                    embed.WithTitle("***ERROR***");
                    embed.WithDescription(Result.ErrorReason);

                }
                await Context.Channel.SendMessageAsync("", embed: embed.Build());
            }
        }

        private Task Command_Log(LogMessage Message)
        {
           Console.WriteLine(Message.Message);
           return Task.CompletedTask;
        }
    }
}
