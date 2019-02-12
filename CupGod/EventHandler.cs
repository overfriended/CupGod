using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using AdminToolbox.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CupGod
{
    class EventHandler : IEventHandlerPlayerDropItem, IEventHandlerPlayerHurt
    {
        private readonly CupGod plugin;

        List<string> playerSteamId = new List<string>();


        public EventHandler(CupGod plugin)
        {
            this.plugin = plugin;
        }

        public String getSteamIdFromName(String name)
        {
            Player player = GetPlayerFromString.GetPlayer(name);
            return player.SteamId;
        }

        public void OnPlayerDropItem(PlayerDropItemEvent ev)
        {
            if (ev.Item.ItemType == Smod2.API.ItemType.CUP)
            {
                PlayerSettings playerSettings = new PlayerSettings(ev.Player.SteamId);
                playerSettings.godMode = true;

                playerSteamId.Add(ev.Player.SteamId);
            }
        }

        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            if (playerSteamId.Contains(ev.Attacker.SteamId))
            {
                ev.Damage = 0;
            }
        }

        public void OnRoundEnd(IEventHandlerRoundEnd ev)
        {
            playerSteamId.Clear();
        }

    }
}
