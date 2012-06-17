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
using System.Xml.Linq;
using Procon.Net.Utils;

namespace Procon.Net.Protocols.Objects
{
    [Serializable]
    public class GameMode : ProtocolObject
    {
        // This is a list of keys used to access common game mode information.  The values
        // for these keys should be set by children classes, because only they know which
        // pieces of information correspond to the common keys.  This is called normalization.
        //
        // See "Player.cs" for an example.
        #region Default (Normalized) Keys used to access common game mode information.

        protected static readonly string C_NAME         = "procon.Name";
        protected static readonly string C_FRIENDY_NAME = "procon.FriendlyName";
        protected static readonly string C_TEAM_COUNT   = "procon.TeamCount";

        #endregion



        /// <summary>This game mode's name as it is used via Rcon.</summary>
        public string Name
        {
            get { return TryGetVariable<string>(C_NAME, null); }
            set
            {
                DataAddSet(C_NAME, value);
                OnPropertyChanged("Name");
            }
        }
        /// <summary>This game mode's human-readable name.</summary>
        public string FriendlyName
        {
            get { return TryGetVariable<string>(C_FRIENDY_NAME, null); }
            set
            {
                DataAddSet(C_FRIENDY_NAME, value);
                OnPropertyChanged("FriendlyName");
            }
        }
        /// <summary>This game mode's number of teams, not including spectator/neutral.</summary>
        public int TeamCount
        {
            get { return TryGetVariable<int>(C_TEAM_COUNT, 0); }
            set
            {
                DataAddSet(C_TEAM_COUNT, value);
                OnPropertyChanged("TeamCount");
            }
        }



        /// <summary>Initializes the game mode with some default values.</summary>
        public GameMode() {
            DataAdd(C_NAME,         new DataVariable(C_NAME,         String.Empty));
            DataAdd(C_FRIENDY_NAME, new DataVariable(C_FRIENDY_NAME, String.Empty));
        }

        /// <summary>Deserializes game mode information received via a network.</summary>
        public virtual GameMode Deserialize(XElement element) {
            Name         = element.ElementValue("name");
            FriendlyName = element.ElementValue("friendlyname");
            TeamCount    = int.Parse(element.ElementValue("teamcount"));
            return this;
        }
    }
}