using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBot
{
    public static class GlobalConstants
    {
        public const string blacklistPath = "C:\\Users\\BlueH\\Desktop\\cvicenia\\PV178cv\\EventBot\\Blacklist.txt";
        public const ulong authorID = 182489831367114753;
        public const ulong botID = 826442489798983690;
        public const ulong setupChannelID = 811290622001676318;
        public const ulong eventChannelID = 811290895768748053;
        public const ulong announcementsChannelID = 811291007583911997;
        public const int deletionDelay = 5000;

        public static string[] eventRoleMentions = new string[] {
            "<@&811294051713155123>",
            "<@&811293298320736267>",
            "<@&811295585285963837>",
            "<@&811370096278503455>",
            "<@&811294043480129587>",
            "<@&811369976187453501>",
            "<@&811420523312447508>",
            "<@&811293876069203998>",
        };

    }
}
