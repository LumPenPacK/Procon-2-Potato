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

namespace Procon.Core {

    /// <summary>
    /// These commands can originate from a config file or from a connected layer client
    /// </summary>
    [Serializable]
    public enum CommandName {
        None,

        LanguageSetDefaultLanguage,

        InstanceLayerSetup,
        InstanceAddRemoteInterface,
        InstanceRemoveRemoteInterface,

        ConnectionsAddConnection,
        ConnectionsRemoveConnection,

        SecurityGroupsAddGroup,
        SecurityGroupsRemoveGroup,
        SecurityGroupsSetPermission,
        SecurityGroupsAssignAccount,
        SecurityGroupsRevokeAccount,

        SecurityAccountsAddAccount,
        SecurityAccountsRemoveAccount,
        SecurityAccountsAssign,
        SecurityAccountsRevoke,
        SecurityAccountsSetPassword,
        SecurityAccountsSetPreferredLanguageCode,

        PluginSetVariable,

        TextCommandsParse,
        TextCommandsPreview,

        /// Packages
        ///
        PackagesInstallPackage,
        PackagesAddRemoteRepository,
        PackagesRemoveRemoteRepository,
        PackagesAddAutomaticUpdatePackage,
        PackagesRemoveAutomaticUpdatePackage,

        Set,
        Get,
        SetA,

        /// Layer commands
        /// 
        Login,
        Salt,
        Hashed,
        Authenticated,
        Action,

        /// Game commands
        /// 
        Kick,
        Say,
        PermanentBan,
        UnBan,
        Nextmap,
        PlayerStatus
    }
}
