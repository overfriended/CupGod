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
        String playerRank;
        String playerColor;

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
                if (playerSteamId.Contains(ev.Player.SteamId))
                {
                    playerSteamId.Remove(ev.Player.SteamId);

                    plugin.Info($"Player {ev.Player.Name} (rank {ev.Player.GetRankName()}) has been removed from the blessing of the Cup God.");

                    ev.Player.SendConsoleMessage("Your blessing of the Cup God has been revoked.");

                    CupGod.CupGods = 0;

                    playerRank = ev.Player.GetUserGroup().Name;
                    playerColor = ev.Player.GetUserGroup().Color;

                    ev.Player.SetRank(playerColor, playerRank);
                } else if (CupGod.CupGods == 0 && !playerSteamId.Contains(ev.Player.SteamId))
                {
                    playerSteamId.Add(ev.Player.SteamId);

                    plugin.Info($"Player {ev.Player.Name} (rank {ev.Player.GetRankName()}) has been added to the blessing of the Cup God");

                    ev.Player.SendConsoleMessage("You have been granted the blessing of the Cup God.");

                    CupGod.CupGods++;

                    ev.Player.SetRank("blue_green", "Blessing of the Cup God");
                } else if (CupGod.CupGods > 0 && !playerSteamId.Contains(ev.Player.SteamId))
                {
                    plugin.Info($"Player {ev.Player.Name} (rank {ev.Player.GetRankName()}) tried to obtain the blessing but someone beat them to it.");

                    ev.Player.SendConsoleMessage($"I am sorry, but {playerSteamId.First()} is already Cup God.");
                }
            }
        } 

        public void OnPlayerHurt(PlayerHurtEvent ev)
        {
            if (playerSteamId.Contains(ev.Attacker.SteamId))
            {
                ev.Damage = 0;
            }
            if (playerSteamId.Contains(ev.Player.SteamId))
            {
                ev.Damage = 0;
                if (ev.Attacker is Player)
                {
                    ev.Attacker.Damage(10, DamageType.NUKE);
                }
            }
        }

        public void OnRoundEnd(RoundEndEvent ev)
        {
            playerSteamId.Clear();
            plugin.Info("Round end reason: " + ev.Status);
            CupGod.CupGods = 0;
        }
        public void OnCheckRoundEnd(CheckRoundEndEvent ev)
        {

            bool MTFAlive = CupGod.CountRoles(Smod2.API.Team.NINETAILFOX) > 0;
            bool CiAlive = CupGod.CountRoles(Smod2.API.Team.CHAOS_INSURGENCY) > 0;
            bool ScpAlive = CupGod.CountRoles(Smod2.API.Team.SCP) > 0;
            bool DClassAlive = CupGod.CountRoles(Smod2.API.Team.CLASSD) > 0;
            bool ScientistsAlive = CupGod.CountRoles(Smod2.API.Team.SCIENTIST) > 0;
            int CupGodCount = CupGod.CupGods;
            bool CGAlive = CupGodCount > 0;

            if (MTFAlive && (CiAlive || ScpAlive || DClassAlive || CGAlive))
                ev.Status = ROUND_END_STATUS.ON_GOING;
            else if (CiAlive && (MTFAlive || (DClassAlive && ScpAlive) || ScientistsAlive || CGAlive))
                ev.Status = ROUND_END_STATUS.ON_GOING;
            else if (ScpAlive && (MTFAlive || DClassAlive || ScientistsAlive))
                ev.Status = ROUND_END_STATUS.ON_GOING;
            else if (CGAlive && ScpAlive && !MTFAlive && !CiAlive && !DClassAlive && !ScientistsAlive)
                ev.Status = ROUND_END_STATUS.SCP_VICTORY;
            else if (CiAlive && ScpAlive)
                ev.Status = ROUND_END_STATUS.SCP_CI_VICTORY;
        }
    }
}
