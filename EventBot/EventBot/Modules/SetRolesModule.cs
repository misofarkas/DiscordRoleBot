using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace EventBot.Modules
{
    public class SetRolesModule : ModuleBase<SocketCommandContext>
    {
        [Command("set")]
        [Summary("Sets the user's interest roles")]
        public async Task SetAsync(params string[] desiredRoleNames)
        {
            var guildRoles = Context.Guild.Roles;
            var user = Context.User;
            var rolesSet = "";
            var invalidRoles = "";

            // If atleast one selected role is valid, delete all interest roles
            if (GlobalConstants.eventRoleMentions.Intersect(desiredRoleNames).Count() > 0)
            {
                foreach (var roleMention in GlobalConstants.eventRoleMentions)
                {
                    await (user as IGuildUser).RemoveRoleAsync(guildRoles.FirstOrDefault(x => x.Mention == roleMention));
                }
            }
            
            
            if (desiredRoleNames.Length == 0)
            {
                // No role was selected -> select all roles instead
                desiredRoleNames = GlobalConstants.eventRoleMentions;
            }
            
            foreach (var desiredRoleName in desiredRoleNames)
            {
                // Checks wheter the role is valid
                if (GlobalConstants.eventRoleMentions.Contains(desiredRoleName))
                {
                    // Find the role object and add it to user
                    await (user as IGuildUser).AddRoleAsync(guildRoles.FirstOrDefault(x => x.Mention == desiredRoleName));
                    rolesSet += $"{desiredRoleName} ";
                }
                else
                {
                    invalidRoles += $"{desiredRoleName} ";
                }
            }

            if (rolesSet != "")
            {
                // Print all roles that ever successfuly set
                await ReplyAsync($"successfuly set roles : {rolesSet}");
            }
            if (invalidRoles != "")
            {
                // Print all invalid roles and delete the error message after a delay
                var errorMessage = await ReplyAsync($"Roles {invalidRoles} do not exist or can't be set");
                await Task.Delay(GlobalConstants.deletionDelay);
                await errorMessage.DeleteAsync();
            }
        }
    }
}
