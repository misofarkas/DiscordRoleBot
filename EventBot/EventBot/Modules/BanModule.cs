using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Discord.Commands;
using Discord;
using EventBot;


namespace EventBot.Modules
{

    public class BanModule : ModuleBase<SocketCommandContext>
    {

        [Command("ban")]
        [Summary("Adds a bot to blacklist")]
        public async Task BanAsync(params string[] mentions)
        {
            foreach (var mention in mentions)
            {
                var botId = GetStringIdFromMention(mention);
                var parsedCorrectly = ulong.TryParse(botId, out var botId2);
                IUserMessage errorMessage = null;

                // Checks wheter the ID was parsed correctly and wheter the user exists
                if (!parsedCorrectly || Context.Guild.GetUser(botId2) is null)
                {
                    errorMessage = await ReplyAsync($"invalid mention {mention}");
                }
                // Checks wheter the user is a bot
                else if (!Context.Guild.GetUser(botId2).IsBot)
                {
                    errorMessage = await ReplyAsync($"{mention} is not a bot");
                }
                // Checks wheter the bot is present in the blacklist file
                else if (File.ReadAllText(GlobalConstants.blacklistPath).Contains(botId))
                {
                    errorMessage = await ReplyAsync($"{mention} is already banned");
                }
                else
                {
                    AddToBlacklist(botId);
                    await ReplyAsync($"{mention} added to blacklist");
                }

                if (errorMessage is not null)
                {
                    // Delete the error message after a specified delay
                    await Task.Delay(GlobalConstants.deletionDelay);
                    await errorMessage.DeleteAsync();
                }
            }
        }


        [Command("unban")]
        [Summary("Removes a bot from blacklist")]
        public async Task UnbanAsync(params string[] mentions)
        {
            foreach (var mention in mentions)
            {
                var botId = GetStringIdFromMention(mention);
                IUserMessage errorMessage = null;

                // Checks wheter the ID was exctacted properly
                if (botId is null)
                {
                    errorMessage = await ReplyAsync($"invalid mention {mention}");
                }
                // Checks whether the bot's ID is present in blacklist file
                else if (!File.ReadAllText(GlobalConstants.blacklistPath).Contains(botId))
                {
                    errorMessage = await ReplyAsync($"{mention} is not banned");
                }
                else
                {
                    RemoveFromBlacklist(botId);
                    await ReplyAsync($"{mention} removed from blacklist");
                }

                if (errorMessage is not null)
                {
                    // Delete the error message after a specified delay
                    await Task.Delay(GlobalConstants.deletionDelay);
                    await errorMessage.DeleteAsync();
                }
            }
        }

        [Command("clearBlacklist")]
        [Summary("Clears the blacklist")]
        // Overwrites the blacklist file with an empty string
        public async Task ClearBlacklist()
        {
            using StreamWriter file = new(GlobalConstants.blacklistPath);
            await file.WriteLineAsync("");
            await ReplyAsync("Blacklist has been cleared");
        }


        // Exctacts the ID from a mention
        // Returns null if unsuccessful
        private string GetStringIdFromMention(string mention)
        {
            if (mention.StartsWith("<@!") && mention.EndsWith(">"))
            {
                var splitMention = mention.Split("!")[1];
                return splitMention.Remove(splitMention.Length - 1);
            }

            return null;
        }
        
        // Writes the banned bot's ID to the blacklist file
        private async void AddToBlacklist(string Id)
        {
            using StreamWriter file = new(GlobalConstants.blacklistPath, append: true);
            await file.WriteLineAsync(Id);
        }

        // Removes the banned bot's ID from the blacklist file
        private async void RemoveFromBlacklist(string Id)
        {
            var blacklist = File.ReadAllLines(GlobalConstants.blacklistPath).ToList();
            blacklist.Remove(Id);
            await File.WriteAllLinesAsync(GlobalConstants.blacklistPath, blacklist.ToArray());
        }
    }
}
