using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;


namespace EventBot.Modules
{
    
    public class NewEventModule : ModuleBase<SocketCommandContext>
    {
        [Command("new")]
        [Summary("Creates a new event")]
        public async Task NewAsync(params string[] Args)
        {
            IUserMessage errorMessage = null;

            if (Args.Length != 6)
            {
                errorMessage = await ReplyAsync("command requires 6 parameters");

                // Delete the error message after a specified delay
                await Task.Delay(GlobalConstants.deletionDelay);
                await errorMessage.DeleteAsync();
                return;
            }

            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == Args[0]);
            
            // Checks if the role exists
            if (role is null)
            {
                errorMessage = await ReplyAsync("invalid event role/type");
            }
            // Checks if date is in correct format
            else if (!DateTime.TryParse(Args[3], out _))
            {
                errorMessage = await ReplyAsync("invalid date format");
            }
            // Checks if time is in correct format
            else if (!TimeSpan.TryParse(Args[4], out _))
            {
                errorMessage = await ReplyAsync("invalid time format");
            }
            // Checks if number of required members is in correct format
            else if (!Int32.TryParse(Args[5], out var numOfMembers) || numOfMembers < 1)
            {
                errorMessage = await ReplyAsync("invalid number of members");
            }
            
            if (errorMessage is not null)
            {
                // Delete the error message after a specified delay
                await Task.Delay(GlobalConstants.deletionDelay);
                await errorMessage.DeleteAsync();
                return;
            }

            var embed = new EmbedBuilder { Title = $"NEW EVENT" };

            // Fill the embed with fields
            embed.AddField(field => { field.Name = "Event type"; field.Value = $"<@&{role.Id}>"; field.IsInline = false; });
            embed.AddField(field => { field.Name = "Event name"; field.Value = Args[1]; field.IsInline = false; });
            embed.AddField(field => { field.Name = "Location"; field.Value = Args[2]; field.IsInline = true; });
            embed.AddField(field => { field.Name = "Date"; field.Value = Args[3]; field.IsInline = true; });
            embed.AddField(field => { field.Name = "Time"; field.Value = Args[4]; field.IsInline = true; });
            embed.AddField(field => { field.Name = "Minimum members"; field.Value = Args[5]; field.IsInline = false; });

            await ReplyAsync(embed: embed.Build());
        }
    }
}
