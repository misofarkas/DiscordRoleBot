using bot.Handlers;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBot.Modules
{
    public static class OnReactModule
    {
        public static async Task OnReactSendToAnnouncentsAsync(Cacheable<IUserMessage, ulong> cachedMessage,
                                  ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (channel.Id != GlobalConstants.eventChannelID)
            {
                // Reaction not in event channel
                return;
            }

            var message = await cachedMessage.DownloadAsync();
            var thumbsUp = new Emoji("👍");

            if (message is null)
            {
                // Message is already deleted
                return;
            }
            if (message.Author.Id != GlobalConstants.botID)
            {
                // Reaction was not on our bot's message
                return;
            }
            

            // Delete other reactions that are not thumbsUps
            if (!reaction.Emote.Equals(thumbsUp))
            {
               await message.RemoveReactionAsync(reaction.Emote, reaction.User.Value as IUser);
            }

            var numberOfReacts = message.Reactions[thumbsUp].ReactionCount;
            var guildChannel = reaction.Channel as SocketGuildChannel;
            var announcements = guildChannel.Guild.GetChannel(GlobalConstants.announcementsChannelID);

            // Try to parse last(sixth) field in the embeded message
            if (!Int32.TryParse(message.Embeds.First().Fields[5].Value, out int minimumAttendees))
            {
                // minimumAttendees is wrongly formated
                return;
            }

            if (numberOfReacts >= minimumAttendees)
            {
                // Threshold of minimum reactions reached
                // Delete the message from events channel and sent it in announcements channel
                await (announcements as ISocketMessageChannel).SendMessageAsync(embed: (message.Embeds.First() as Embed));
                await message.DeleteAsync();
            }
        }
    }
}
