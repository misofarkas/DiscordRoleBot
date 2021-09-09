using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("info")]
        [Summary("Gives information about all available commands")]
        public async Task InfoAsync()
        {

            var embed = new EmbedBuilder { Title = $"INFO" };

            #region descriptions
            var setDescription = "sets selected valid roles to the user. If no role was selected, sets all roles";
            var newDescription = "creates a new event in the event channel. If it reaches minimum reactions required," +
                " it posts the event in the announcements channel";
            var banDescription = "adds the selected bot to blacklist. Events from blacklisted bots will be ignored";
            #endregion

            // for each command, add a field with it's description
            embed.AddField(field => { field.Name = ".set [@role1] [@role2] ...)";
                                      field.Value = setDescription; field.IsInline = false; });
            embed.AddField(field => { field.Name = ".new (eventType) (name) (location) (date) (time) (minimumMembers)";
                                      field.Value = newDescription; field.IsInline = false; });
            embed.AddField(field => { field.Name = ".ban (@bot1) [@bot2] ...";
                                      field.Value = banDescription; field.IsInline = false; });
            embed.AddField(field => { field.Name = ".unban (@bot1) [@bot2] ...";
                                      field.Value = "removes the selected bot from blacklist"; field.IsInline = false; });
            embed.AddField(field => { field.Name = ".clearBlacklist";
                                      field.Value = "completely erases all banned bots from blacklist"; field.IsInline = false; });
            embed.AddField(field => { field.Name = ".ping";
                                      field.Value = "replies with ``pong!`` to the user"; field.IsInline = false; });
            embed.AddField(field => { field.Name = ".info";
                                      field.Value = "prints this info message"; field.IsInline = false; });

            await ReplyAsync(embed: embed.Build());
        }
    }
}
