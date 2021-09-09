using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace EventBot.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Replies to the message with pong!")]
        public async Task PingAsync()
        {
            await ReplyAsync("pong!");
        }
    }
}
