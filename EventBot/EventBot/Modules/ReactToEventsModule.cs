using Discord.Commands;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EventBot.Modules
{
    public static class ReactToEventsModule
    {
        public static async Task ReactToEventsAsync(SocketCommandContext context)
        {
            // Check if the event is from a banned bot
            if (File.ReadAllText(GlobalConstants.blacklistPath).Contains(context.User.Id.ToString()))
            {
                return;
            }

            var thumbsUp = new Emoji("👍");
            var userRoles = context.Guild.GetUser(GlobalConstants.authorID).Roles;
            var embed = context.Message.Embeds.ElementAtOrDefault(0);
            var eventType = "";

            
            if (embed is not null)
            {
                // The message is in an embeded format
                // Try to get the type of the event from the first field,
                // if it fails, message is incorrectly formated
                try { eventType = embed.Fields[0].Value; }
                catch(IndexOutOfRangeException) { }
            }
            else
            {
                // The message is not an embed
                // Extract the first line, which should be the event type if it's correctly formated
                eventType = context.Message.Content.Split('\n').First();
            }


            foreach (var role in userRoles)
            {
                // If the event type is equal to one of author's roles, add a thumbsup reaction
                if (role.Mention == eventType || role.Name == eventType)
                {
                    await context.Message.AddReactionAsync(thumbsUp);
                    break;
                }
            }
        }
    }
}
