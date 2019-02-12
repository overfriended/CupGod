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
        name = "Test",
        description = "CupGod is a plugin that when you drop a cup, you become god. You cannot do damage.",
        id = "overfriended.cup.god",
        version = "1.0",
        SmodMajor = 3,
        SmodMinor = 0,
        SmodRevision = 0
        )]
    public class CupGod : Plugin
    {

        public override void OnDisable()
        {
            this.Info(this.Details.name + "disabled");
        }

        public override void OnEnable()
        {
            this.Info(this.Details.name + "enabled");
        }

        public override void Register()
        {
            this.AddEventHandlers(new EventHandler(this));
        }
    }
}
