﻿// Copyright 2011 Geoffrey 'Phogue' Green
// 
// Altered by Cameron 'Imisnew2' Gunnin
// 
// http://www.phogue.net
// 
// This file is part of Procon 2.
// 
// Procon 2 is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Procon 2 is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Procon 2.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ComponentModel;
using System.Linq;
using Procon.Net.Utils;
using Procon.Net.Utils.Maxmind;

namespace Procon.Net.Protocols.Objects
{
    [Serializable]
    public class Player : ProtocolObject
    {
        #region Default (Normalized) keys used to access common player information.

        protected static readonly string C_UID           = "procon.UID";
        protected static readonly string C_SLOT_ID       = "procon.SlotID";
        protected static readonly string C_CLAN_TAG      = "procon.ClanTag";
        protected static readonly string C_NAME          = "procon.Name";
        protected static readonly string C_NAME_STRIPPED = "procon.NameStripped";
        protected static readonly string C_GUID          = "procon.GUID";
        protected static readonly string C_TEAM          = "procon.Team";
        protected static readonly string C_SQUAD         = "procon.Squad";
        protected static readonly string C_SCORE         = "procon.Score";
        protected static readonly string C_KILLS         = "procon.Kills";
        protected static readonly string C_DEATHS        = "procon.Deaths";
        protected static readonly string C_ROLE          = "procon.Role";
        protected static readonly string C_INVENTORY     = "procon.Inventory";
        protected static readonly string C_PING          = "procon.Ping";
        protected static readonly string C_COUNTRY_NAME  = "procon.CountryName";
        protected static readonly string C_COUNTRY_CODE  = "procon.CountryCode";
        protected static readonly string C_IP            = "procon.IP";
        protected static readonly string C_PORT          = "procon.Port";

        #endregion

        public Player() {
            this[C_UID]           = new DataVariable(C_UID,           null, false, "Player.UID",           "Player.UID_Description");
            this[C_GUID]          = new DataVariable(C_GUID,          null, false, "Player.GUID",          "Player.GUID_Description");
            this[C_SLOT_ID]       = new DataVariable(C_SLOT_ID,       null, false, "Player.SLOT_ID",       "Player.SLOT_ID_Description");
            this[C_CLAN_TAG]      = new DataVariable(C_CLAN_TAG,      null, false, "Player.CLAN_TAG",      "Player.CLAN_TAG_Description");
            this[C_NAME]          = new DataVariable(C_NAME,          null, false, "Player.NAME",          "Player.NAME_Description");
            this[C_NAME_STRIPPED] = new DataVariable(C_NAME_STRIPPED, null, false, "Player.NAME_STRIPPED", "Player.NAME_STRIPPED_Description");
            this[C_TEAM]          = new DataVariable(C_TEAM,          null, false, "Player.TEAM",          "Player.TEAM_Description");
            this[C_SQUAD]         = new DataVariable(C_SQUAD,         null, false, "Player.SQUAD",         "Player.SQUAD_Description");
            this[C_KILLS]         = new DataVariable(C_KILLS,         null, false, "Player.KILLS",         "Player.KILLS_Description");
            this[C_DEATHS]        = new DataVariable(C_DEATHS,        null, false, "Player.DEATHS",        "Player.DEATHS_Description");
            this[C_ROLE]          = new DataVariable(C_ROLE,          null, false, "Player.ROLE",          "Player.ROLE_Description");
            this[C_INVENTORY]     = new DataVariable(C_INVENTORY,     null, false, "Player.INVENTORY",     "Player.INVENTORY_Description");
            this[C_PING]          = new DataVariable(C_PING,          null, false, "Player.PING",          "Player.PING_Description");
            this[C_COUNTRY_NAME]  = new DataVariable(C_COUNTRY_NAME,  null, false, "Player.COUNTRY_NAME",  "Player.COUNTRY_NAME_Description");
            this[C_COUNTRY_CODE]  = new DataVariable(C_COUNTRY_CODE,  null, false, "Player.COUNTRY_CODE",  "Player.COUNTRY_CODE_Description");
            this[C_IP]            = new DataVariable(C_IP,            null, false, "Player.IP",            "Player.IP_Description");
            this[C_PORT]          = new DataVariable(C_PORT,          null, false, "Player.PORT",          "Player.PORT_Description");

            UID     = String.Empty;
            GUID    = String.Empty;
            ClanTag = String.Empty;
            Name    = String.Empty;
        }

        /// <summary>A Unique Identifier.</summary>
        public virtual string UID
        {
            get { return TryGetVariable<string>(C_UID, null); }
            set
            {
                DataAddSet(C_UID, value);
                OnPropertyChanged("UID");
            }
        }
        /// <summary>A Game-specific Unique Identifier.</summary>
        public virtual string GUID
        {
            get { return TryGetVariable<string>(C_GUID, null); }
            set
            {
                DataAddSet(C_GUID, value);
                OnPropertyChanged("GUID");
            }
        }
        /// <summary>A Player Number assigned by the server to this player.</summary>
        public virtual uint SlotID
        {
            get { return TryGetVariable<uint>(C_SLOT_ID, 0); }
            set
            {
                DataAddSet(C_SLOT_ID, value);
                OnPropertyChanged("SlotID");
            }
        }

        /// <summary>A string of characters that prefixes this player's name.</summary>
        public virtual string ClanTag
        {
            get { return TryGetVariable<string>(C_CLAN_TAG, null); }
            set
            {
                DataAddSet(C_CLAN_TAG, value);
                OnPropertyChanged("ClanTag");
            }
        }
        /// <summary>This player's Name.</summary>
        public virtual string Name
        {
            get { return TryGetVariable<string>(C_NAME, null); }
            set
            {
                DataAddSet(C_NAME, value);
                DataAddSet(C_NAME_STRIPPED, value.Strip());
                OnPropertyChanged("Name");
                OnPropertyChanged("NameStripped");
            }
        }
        /// <summary>This player's Name, with diacratics/l33t/etc replaced with ANSI equivalents.</summary>
        public virtual string NameStripped
        {
            get { return TryGetVariable<string>(C_NAME_STRIPPED, null); }
        }

        /// <summary>This player's Team (e.g, Team1).</summary>
        public virtual Team Team
        {
            get { return TryGetVariable<Team>(C_TEAM, Team.None); }
            set
            {
                DataAddSet(C_TEAM, value);
                OnPropertyChanged("Team");
            }
        }
        /// <summary>This player's Squad (e.g, Alpha).</summary>
        public virtual Squad Squad
        {
            get { return TryGetVariable<Squad>(C_SQUAD, Squad.None); }
            set
            {
                DataAddSet(C_SQUAD, value);
                OnPropertyChanged("Squad");
            }
        }
        /// <summary>This player's Score.</summary>
        public virtual int Score
        {
            get { return TryGetVariable<int>(C_SCORE, 0); }
            set
            {
                DataAddSet(C_SCORE, value);
                OnPropertyChanged("Score");
            }
        }
        /// <summary>This player's Kill count.</summary>
        public virtual int Kills
        {
            get { return TryGetVariable<int>(C_KILLS, 0); }
            set
            {
                DataAddSet(C_KILLS, value);
                OnPropertyChanged("Kills");
                OnPropertyChanged("Kdr");
            }
        }
        /// <summary>This player's Death count.</summary>
        public virtual int Deaths
        {
            get { return TryGetVariable<int>(C_DEATHS, 0); }
            set
            {
                DataAddSet(C_DEATHS, value);
                OnPropertyChanged("Deaths");
                OnPropertyChanged("Kdr");
            }
        }
        /// <summary>This player's Kill to Death ratio.</summary>
        public virtual float Kdr
        {
            get { return (Deaths <= 0) ? Kills : Kills / (float)Deaths; }
        }

        /// <summary>A Game-specific job/class the player assumes (e.g, Sniper, Medic).</summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual Role Role
        {
            get { return TryGetVariable<Role>(C_ROLE, null); }
            set
            {
                DataAddSet(C_ROLE, value);
                OnPropertyChanged("Role");
            }
        }
        /// <summary>A Game-specific collection of items the player has (e.g, Armor, AK-47, HE Grenade).</summary>
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public virtual Inventory Inventory
        {
            get { return TryGetVariable<Inventory>(C_INVENTORY, null); }
            set
            {
                DataAddSet(C_INVENTORY, value);
                OnPropertyChanged("Inventory");
            }
        }

        /// <summary>This player's latency to the game server.</summary>
        public virtual uint Ping
        {
            get { return TryGetVariable<uint>(C_PING, 0); }
            set
            {
                DataAddSet(C_PING, value);
                OnPropertyChanged("Ping");
            }
        }
        /// <summary>This player's Full Country Name he/she is playing from.</summary>
        public virtual string CountryName
        {
            get { return TryGetVariable<string>(C_COUNTRY_NAME, null); }
        }
        /// <summary>This player's Abbreviated Country Name he/she is playing from.</summary>
        public virtual string CountryCode
        {
            get { return TryGetVariable<string>(C_COUNTRY_CODE, null); }
        }
        /// <summary>This player's IP Address.</summary>
        public virtual string IP
        {
            get { return TryGetVariable<string>(C_IP, null); }
            set
            {
                string mIP   = null;
                string mPort = null;
                // Validate Ip has colon before trying to split.
                if (value != null && value.Contains(":")) {
                    mIP   = value.Split(':').FirstOrDefault();
                    mPort = value.Split(':').LastOrDefault();
                }

                // Validate Ip string was valid before continuing.
                if (mIP != null && mIP != IP && mIP.Length > 0 && mIP.Contains('.'))
                {
                    DataAddSet(C_IP, mIP);
                    OnPropertyChanged("IP");

                    // Try: In-case GeoIP.dat is not loaded properly
                    try
                    {
                        DataAddSet(C_COUNTRY_NAME, mCountryLookup.lookupCountryName(mIP));
                        DataAddSet(C_COUNTRY_CODE, mCountryLookup.lookupCountryCode(mIP));
                        OnPropertyChanged("CountryName");
                        OnPropertyChanged("CountryCode");
                    }
                    catch (Exception) { }
                }
                // Validate Port string was valid before continuing.
                if (mPort != null && mPort != Port && mPort.Length > 0 && !mPort.Contains('.'))
                {
                    DataAddSet(C_PORT, mPort);
                    OnPropertyChanged("Port");
                }
            }
        }
        /// <summary>The player's Port Address.</summary>
        public virtual string Port
        {
            get { return TryGetVariable<string>(C_PORT, null); }
        }

        // Used when determining a player's Country Name and Code.
        public static readonly CountryLookup mCountryLookup = new CountryLookup("GeoIP.dat");
    }
}