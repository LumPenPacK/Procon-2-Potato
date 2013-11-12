﻿using System;
using System.Linq;
using Procon.Net.Geolocation;

namespace Procon.Net.Protocols.Objects {
    using Procon.Net.Utils;

    [Serializable]
    public sealed class Player : NetworkObject {

        // Used when determining a player's Country Name and Code.
        public static readonly IGeolocate Geolocation = new GeolocateIp();

        public Player() {
            this.Uid = String.Empty;
            this.ClanTag = String.Empty;
            this.Name = String.Empty;
            this.Groups = new GroupingList();
            this.Location = new Location();
        }

        /// <summary>
        /// A Unique Identifier.
        /// </summary>
        public String Uid { get; set; }

        /// <summary>
        /// A Player Number assigned by the server to this player.
        /// </summary>
        public uint SlotId { get; set; }

        /// <summary>
        /// A String of characters that prefixes this player's name.
        /// </summary>
        public String ClanTag { get; set; }

        /// <summary>
        /// This player's Name.
        /// </summary>
        public String Name {
            get { return this._name; }
            set {
                if (this._name != value) {
                    this._name = value;
                    this.NameStripped = this._name.Strip();
                }
            }
        }
        private String _name;

        /// <summary>
        /// This player's Name, with diacritics/l33t/etc replaced with ANSI equivalents.
        /// </summary>
        public String NameStripped { get; set; }

        /// <summary>
        /// This players grouping on this server
        /// </summary>
        public GroupingList Groups { get; set; }

        /// <summary>
        /// This player's Score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// This player's Kill count.
        /// </summary>
        public int Kills { get; set; }

        /// <summary>
        /// This player's Death count.
        /// </summary>
        public int Deaths { get; set; }

        /// <summary>
        /// This player's Kill to Death ratio.
        /// </summary>
        public float Kdr {
            get { return (this.Deaths <= 0) ? this.Kills : this.Kills / (float)this.Deaths; }
        }

        /// <summary>
        /// A Game-specific job/class the player assumes (e.g, Sniper, Medic).
        /// </summary>
        public Role Role { get; set; }

        /// <summary>
        /// A Game-specific collection of items the player has (e.g, Armor, AK-47, HE Grenade).
        /// </summary>
        public Inventory Inventory { get; set; }

        /// <summary>
        /// This player's latency to the game server.
        /// </summary>
        public uint Ping { get; set; }

        /// <summary>
        /// The players location, found from their ip address if available.
        /// </summary>
        public Location Location { get; set; }

        [Obsolete]
        public String CountryName { get { return this.Location.CountryName; } }

        [Obsolete]
        public String CountryCode { get { return this.Location.CountryCode; } }

        /// <summary>
        /// This player's IP Address.
        /// </summary>
        public String IP {
            get { return this._ip; }
            set {
                this._ip = value;

                // Validate Ip has colon before trying to split.
                if (value != null && value.Contains(":")) {
                    this._ip = value.Split(':').FirstOrDefault();
                    this.Port = value.Split(':').LastOrDefault();
                }

                Location location = Player.Geolocation.Locate(this._ip);

                if (location != null) {
                    this.Location = location;
                }
            }
        }
        private String _ip;

        /// <summary>
        /// The player's Port Address.
        /// </summary>
        public String Port { get; set; }

        /// <summary>
        /// Adds or updates an existing group with the same type.
        /// </summary>
        /// <param name="updateGroup">The group to add or modify the existing group with the same type</param>
        public void ModifyGroup(Grouping updateGroup) {
            if (updateGroup != null) {
                Grouping existingGroup = this.Groups.FirstOrDefault(group => group.Type == updateGroup.Type);

                if (existingGroup == null) {
                    this.Groups.Add(updateGroup.Clone() as Grouping);
                }
                else {
                    existingGroup.Uid = updateGroup.Uid;
                }
            }
        }
    }
}