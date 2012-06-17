﻿// Copyright 2011 Geoffrey 'Phogue' Green
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Procon.Net.Protocols.Frostbite.BF {
    using Procon.Net.Attributes;
    using Procon.Net.Protocols.Objects;
    using Procon.Net.Protocols.Frostbite.Objects;

    public abstract class BFGame : FrostbiteGame {

        public BFGame(string hostName, ushort port)
            : base(hostName, port) {

        }

        protected override void Action(Move move) {

            // admin.movePlayer <name: player name> <teamId: Team ID> <squadId: Squad ID> <forceKill: boolean>
            bool forceMove = (move.MoveActionType == MoveActionType.ForceMove || move.MoveActionType == MoveActionType.ForceRotate);

            Map selectedMap = this.State.MapPool.Find(x => String.Compare(x.Name, this.State.Variables.MapName, true) == 0);

            if (selectedMap != null) {
                // If they are just looking to rotate the player through the teams
                if (move.MoveActionType == MoveActionType.Rotate || move.MoveActionType == MoveActionType.ForceRotate) {
                    int currentTeamId = FrostbiteConverter.TeamToTeamId(move.Target.Team);

                    // Avoid divide by 0 error - shouldn't ever be encountered though.
                    if (selectedMap.GameMode.TeamCount > 0) {
                        int newTeamId = (currentTeamId + 1) % (selectedMap.GameMode.TeamCount + 1);

                        move.Destination.Team = FrostbiteConverter.TeamIdToTeam(newTeamId == 0 ? 1 : newTeamId);
                    }
                }
            }

            // Now check if the destination squad is supported.
            if (selectedMap.GameMode.Name == "SQDM" || selectedMap.GameMode.Name == "SQRUSH") {
                move.Destination.Squad = (selectedMap.GameMode as FrostbiteGameMode).DefaultSquad;
            }

            this.Send(
                this.Create(
                    "admin.movePlayer \"{0}\" {1} {2} {3}",
                    move.Target.Name,
                    FrostbiteConverter.TeamToTeamId(move.Destination.Team),
                    FrostbiteConverter.SquadToSquadId(move.Destination.Squad),
                    FrostbiteConverter.BoolToString(forceMove)
                )
            );
        }

    }
}