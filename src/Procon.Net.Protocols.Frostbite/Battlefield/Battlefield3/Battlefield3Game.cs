﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Procon.Net.Attributes;
using Procon.Net.Protocols.Frostbite.Battlefield.BF3.Objects;
using Procon.Net.Protocols.Objects;
using Procon.Net.Protocols.Frostbite.Objects;

namespace Procon.Net.Protocols.Frostbite.Battlefield.BF3 {
    [GameType(Type = CommonGameType.BF_3, Name = "Battlefield 3", Provider = "Myrcon")]
    public class Battlefield3Game : BattlefieldGame {

        public Battlefield3Game(string hostName, ushort port)
            : base(hostName, port) {

            this.ServerInfoParameters = new List<string>() {
                "ServerName",
                "PlayerCount",
                "MaxPlayerCount",
                "GameMode",
                "Map",
                "CurrentRound",
                "TotalRounds",
                "TeamScores",
                "ConnectionState",
                "Ranked",
                "PunkBuster",
                "Passworded",
                "ServerUpTime",
                "RoundTime"
            };

            this.PacketDispatcher.Append(new Dictionary<PacketDispatch, PacketDispatcher.PacketDispatchHandler>() {
                {
                    new PacketDispatch() {
                        Name = "player.ping", 
                        Origin = PacketOrigin.Client
                    },
                    new PacketDispatcher.PacketDispatchHandler(this.PlayerPingResponseDispatchHandler)
                }
            });
        }

        protected override void AuxiliarySynchronize() {
            base.AuxiliarySynchronize();

            foreach (Player player in this.State.PlayerList) {
                this.SendPlayerPingPacket(player.Name);
            }
        }

        protected override void AdminListPlayersFinalize(FrostbitePlayerList players) {
            // If no limits on the subset we just fetched.
            if (players.Subset.Count == 0) {

                // 1. Remove all names in the state list that are not found in the new list (players that have left)
                this.State.PlayerList.RemoveAll(x => players.Select(y => y.Name).Contains(x.Name) == false);

                // 2. Add or update any new players
                foreach (Player player in players) {
                    Player statePlayer = this.State.PlayerList.Find(x => x.Name == player.Name);

                    if (statePlayer == null) {
                        this.State.PlayerList.Add(player);
                    }
                    else {
                        // Already exists, update with any new information we have.
                        statePlayer.Kills = player.Kills;
                        statePlayer.Score = player.Score;
                        statePlayer.Deaths = player.Deaths;
                        statePlayer.ClanTag = player.ClanTag;
                        statePlayer.Uid = player.Uid;

                        statePlayer.ModifyGroup(player.Groups.FirstOrDefault(group => group.Type == Grouping.Team));
                        statePlayer.ModifyGroup(player.Groups.FirstOrDefault(group => group.Type == Grouping.Squad));
                    }
                }

                this.OnGameEvent(GameEventType.GamePlayerlistUpdated, new GameEventData() {
                    Players = new List<Player>(this.State.PlayerList)
                });
            }
        }

        public override void AdminListPlayersResponseDispatchHandler(IPacketWrapper request, IPacketWrapper response) {
            Battlefield3PlayerList players = new Battlefield3PlayerList() {
                Subset = new FrostbiteGroupingList().Parse(request.Packet.Words.GetRange(1, request.Packet.Words.Count - 1))
            }.Parse(response.Packet.Words.GetRange(1, response.Packet.Words.Count - 1));

            this.AdminListPlayersFinalize(players);
        }

        public override void MapListListDispatchHandler(IPacketWrapper request, IPacketWrapper response) {
            if (request.Packet.Words.Count >= 1) {

                FrostbiteMapList maps = new Battlefield3FrostbiteMapList().Parse(response.Packet.Words.GetRange(1, response.Packet.Words.Count - 1));

                foreach (Map map in maps) {
                    Map mapInfo = this.State.MapPool.Find(x => String.Compare(x.Name, map.Name, StringComparison.OrdinalIgnoreCase) == 0);
                    if (mapInfo != null) {
                        map.FriendlyName = mapInfo.FriendlyName;
                        map.GameMode = mapInfo.GameMode;
                    }
                }
                this.State.MapList = maps;

                this.OnGameEvent(
                    GameEventType.GameMaplistUpdated
                );
            }
        }

        public override void BanListListDispatchHandler(IPacketWrapper request, IPacketWrapper response) {

            if (request.Packet.Words.Count >= 1) {

                int startOffset = 0;

                if (request.Packet.Words.Count >= 2) {
                    if (int.TryParse(request.Packet.Words[1], out startOffset) == false) {
                        startOffset = 0;
                    }
                }

                // We've just started requesting the banlist, clear it.
                if (startOffset == 0) {
                    this.State.BanList.Clear();
                }

                FrostbiteBanList banList = new Battlefield3BanList().Parse(response.Packet.Words.GetRange(1, response.Packet.Words.Count - 1));

                if (banList.Count > 0) {
                    foreach (Ban ban in banList)
                        this.State.BanList.Add(ban);

                    this.Send(this.CreatePacket("banList.list {0}", startOffset + 100));
                }
                else {
                    // We have recieved the whole banlist in 100 ban increments.. throw event.
                    this.OnGameEvent(
                        GameEventType.GameBanlistUpdated
                    );
                }
            }
        }

        public void PlayerPingResponseDispatchHandler(IPacketWrapper request, IPacketWrapper response) {

            if (request.Packet.Words.Count >= 2 && response != null && response.Packet.Words.Count >= 2) {
                Player player = this.State.PlayerList.FirstOrDefault(p => p.Name == request.Packet.Words[1]);
                uint ping = 0;

                if (player != null && uint.TryParse(response.Packet.Words[1], out ping) == true) {
                    // Sometimes the protocol sends through the max value of uint. We ignore everything above 1000.
                    if (ping < 1000) {
                        player.Ping = ping;
                    }
                }
            }
        }

        protected void SendPlayerPingPacket(String playerName) {
            this.Send(this.CreatePacket("player.ping {0}", playerName));
        }

        protected override void SendEventsEnabledPacket() {
            this.Send(this.CreatePacket("admin.eventsEnabled true"));
        }

        public override void PlayerOnAuthenticatedDispatchHandler(IPacketWrapper request, IPacketWrapper response) {
            // Ignore this in bf3? Seems onJoin handles both.
        }

        public override void PlayerOnJoinDispatchHandler(IPacketWrapper request, IPacketWrapper response) {

            if (request.Packet.Words.Count >= 2) {

                Player player = new Player() {
                    Name = request.Packet.Words[1],
                    Uid = request.Packet.Words[2]
                };

                if (this.State.PlayerList.Find(x => x.Name == player.Name) == null) {
                    this.State.PlayerList.Add(player);
                }

                this.OnGameEvent(GameEventType.GamePlayerJoin, new GameEventData() { Players = new List<Player>() { player } });
            }
        }

        public override void PlayerOnKillDispatchHandler(IPacketWrapper request, IPacketWrapper response) {

            if (request.Packet.Words.Count >= 5) {

                bool headshot = false;

                if (bool.TryParse(request.Packet.Words[4], out headshot) == true) {

                    Player killer = this.State.PlayerList.FirstOrDefault(p => p.Name == request.Packet.Words[1]);
                    Player target = this.State.PlayerList.FirstOrDefault(p => p.Name == request.Packet.Words[1]);

                    if (killer != null && target != null) {
                        // If not a suicide.
                        if (killer.Uid != target.Uid) {
                            killer.Kills++;
                        }

                        target.Deaths++;
                    }

                    this.OnGameEvent(GameEventType.GamePlayerKill, new GameEventData() {
                        Kills = new List<Kill>() {
                            new Kill() {
                                HumanHitLocation = headshot == true ? FrostbiteGame.Headshot : FrostbiteGame.Bodyshot,
                                Scope = {
                                    Players = new List<Player>() {
                                        target
                                    },
                                    Items = new List<Item>() {
                                        new Item() {
                                            // Servers sends garbage at the end of the round?
                                            Name = Regex.Replace(request.Packet.Words[3], @"[^\\w\\/_-]+", "")
                                        }
                                    }
                                },
                                Now = {
                                    Players = new List<Player>() {
                                        killer
                                    }
                                }
                            }
                        }
                    });
                }
            }
        }
        
        protected override void Action(Map map) {

            if (map.ActionType == NetworkActionType.NetworkMapAppend) {
                // mapList.add <map: string> <gamemode: string> <rounds: integer> [index: integer]
                this.Send(this.CreatePacket("mapList.add \"{0}\" \"{1}\" {2}", map.Name, map.GameMode.Name, map.Rounds));

                this.Send(this.CreatePacket("mapList.save"));

                this.Send(this.CreatePacket("mapList.list"));
            }
            // Added by Imisnew2 - You should check this phogue!
            else if (map.ActionType == NetworkActionType.NetworkMapChangeMode) {
                if (map.GameMode != null) {
                    this.Send(this.CreatePacket("admin.setPlaylist \"{0}\"", map.GameMode.Name));
                }
            }
            else if (map.ActionType == NetworkActionType.NetworkMapInsert) {
                // mapList.add <map: string> <gamemode: string> <rounds: integer> [index: integer]
                this.Send(this.CreatePacket("mapList.add \"{0}\" \"{1}\" {2} {3}", map.Name, map.GameMode.Name, map.Rounds, map.Index));

                this.Send(this.CreatePacket("mapList.save"));

                this.Send(this.CreatePacket("mapList.list"));
            }
            else if (map.ActionType == NetworkActionType.NetworkMapRemove) {
                var matchingMaps = this.State.MapList.Where(x => x.Name == map.Name).OrderByDescending(x => x.Index);

                foreach (Map match in matchingMaps) {
                    this.Send(this.CreatePacket("mapList.remove {0}", match.Index));
                }
                
                this.Send(this.CreatePacket("mapList.save"));

                this.Send(this.CreatePacket("mapList.list"));
            }
            else if (map.ActionType == NetworkActionType.NetworkMapRemoveIndex) {
                this.Send(this.CreatePacket("mapList.remove {0}", map.Index));

                this.Send(this.CreatePacket("mapList.list"));
            }
            else if (map.ActionType == NetworkActionType.NetworkMapNextIndex) {
                this.Send(this.CreatePacket("mapList.setNextMapIndex {0}", map.Index));
            }
            else if (map.ActionType == NetworkActionType.NetworkMapRestart || map.ActionType == NetworkActionType.NetworkMapRoundRestart) {
                this.Send(this.CreatePacket("mapList.restartRound"));
            }
            else if (map.ActionType == NetworkActionType.NetworkMapNext || map.ActionType == NetworkActionType.NetworkMapRoundNext) {
                this.Send(this.CreatePacket("mapList.runNextRound"));
            }
            else if (map.ActionType == NetworkActionType.NetworkMapClear) {
                this.Send(this.CreatePacket("mapList.clear"));

                this.Send(this.CreatePacket("mapList.save"));
            }
        }
    }
}