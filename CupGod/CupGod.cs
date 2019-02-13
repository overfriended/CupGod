using Smod2;
using Smod2.API;
using Smod2.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupGod
{
    [PluginDetails(
        author = "overfriended",
        name = "Cup God",
        description = "CupGod is a plugin that when you drop a cup, you become god. You cannot do damage.",
        id = "overfriended.cup.god",
        version = "1.1.0",
        SmodMajor = 3,
        SmodMinor = 0,
        SmodRevision = 0
        )]
    public class CupGod : Plugin
    {
        public static CupGod plugin;

        public static int CupGods = 0;

        public static int CountRoles(Role role)
        {
            int count = 0;
            foreach (Player pl in PluginManager.Manager.Server.GetPlayers())
                if (pl.TeamRole.Role == role)
                    count++;
            return count;
        }

        public static int CountRoles(Smod2.API.Team team)
        {
            int count = 0;
            foreach (Player pl in PluginManager.Manager.Server.GetPlayers())
                if (pl.TeamRole.Team == team)
                    count++;
            return count;
        }

        public class PlayerInfo
        {
            public PlayerInfo(string badgename, string badgecolor)
            {
                BadgeColor = badgecolor;
                BadgeName = badgename;
            }

            public string BadgeName;
            public string BadgeColor;
        }
        public override void OnDisable()
        {
            this.Info(this.Details.name + " disabled");
        }

        public override void OnEnable()
        {
            this.Info(this.Details.name + " enabled");
        }

        public override void Register()
        {
            this.AddEventHandlers(new EventHandler(this));
        }
    }
}
