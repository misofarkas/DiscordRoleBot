using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;
using System.Threading.Tasks;
using EventBot.Modules;
using EventBot;

namespace bot.Handlers
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient client;
        private readonly CommandService commandService;
        

        // Retrieve client and CommandService instance via constructor
        public CommandHandler(DiscordSocketClient client, CommandService commandService)
        {
            this.client = client;
            this.commandService = commandService;
        }

        public async Task SetupAsync()
        {
            // Hook the MessageReceived event into our command handler
            client.MessageReceived += HandleCommandAsync;
            client.ReactionAdded += OnReactModule.OnReactSendToAnnouncentsAsync;

            // Here we discover all of the command modules in the entry assembly and load them
            await commandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // React to messages sent in Events channel
            if (message.Channel.Id == GlobalConstants.eventChannelID &&
                message.Author.IsBot &&
                message.Author.Id != GlobalConstants.botID)
            {
                await ReactToEventsModule.ReactToEventsAsync(new SocketCommandContext(client, message));
                return;
            }


            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!message.HasCharPrefix('.', ref argPos) ||
                message.Author.IsBot ||
                message.Author.Id != GlobalConstants.authorID)
                return;

            // If the command was not sent in the right channel, delete it and return
            if (!message.HasStringPrefix(".new", ref argPos) &&
                message.Channel.Id != GlobalConstants.setupChannelID ||
                message.HasStringPrefix(".new", ref argPos) &&
                message.Channel.Id != GlobalConstants.eventChannelID)
            {
                var errorMessage = await message.Channel.SendMessageAsync("Use ``events`` channel for creating new events," +
                    " use ``setup`` channel for all other commands");
                await message.DeleteAsync();
                await Task.Delay(GlobalConstants.deletionDelay);
                await errorMessage.DeleteAsync();
                return;
            }

            // reset argPos to correct position if the command was .new
            if (message.HasStringPrefix(".new", ref argPos)) argPos = 1;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(client, message);

            // Delete the author's message
            await message.DeleteAsync();

            // Execute the command with the command context we just created
            await commandService.ExecuteAsync(context: context, argPos: argPos, services: null);
            
        }
    }
}
