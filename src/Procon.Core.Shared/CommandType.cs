using System;
using System.ComponentModel;

namespace Procon.Core.Shared {

    /// <summary>
    /// These commands can originate from a config file or from a connected layer client
    /// </summary>
    [Serializable]
    public enum CommandType {
        /// <summary>
        /// No command type specified
        /// </summary>
        None,
        /// <summary>
        /// General query to fetch all of the connections, security group and variables. Anything in
        /// the Instance class pretty much.
        /// </summary>
        [Description("General query to fetch all of the connections, security group and variables. Anything in the Instance class pretty much.")]
        InstanceQuery,
        /// <summary>
        /// Sends a signal to the service controller to restart Procon, applying any updates.
        /// </summary>
        [Description("Sends a signal to the service controller to restart Procon, applying any updates.")]
        InstanceServiceRestart,
        /// <summary>
        /// Sends a signal to the service controller to stop Procon, install/update a package to
        /// the latest version and then start Procon.
        /// </summary>
        [Description("Sends a signal to the service controller to stop Procon, install/update a package to the latest version and then start Procon.")]
        InstanceServiceMergePackage,
        /// <summary>
        /// Sends a signal to the service controller to stop Procon, uninstall a package if it is installed
        /// and then start Procon.
        /// </summary>
        [Description("Sends a signal to the service controller to stop Procon, uninstall a package if it is installed and then start Procon.")]
        InstanceServiceUninstallPackage,
        /// <summary>
        /// Queries information related specificy to a connection. Note that this will essentially query all
        /// other list queries below (NetworkProtocolQuery*) and populate a single request with the results.
        /// </summary>
        [Description("Queries information related specificy to a connection. Note that this will essentially query all other list queries below (NetworkProtocolQuery*) and populate a single request with the results.")]
        ConnectionQuery,

        /// <summary>
        /// Adds a new protocol connection
        /// </summary>
        [Description("Adds a new protocol connection")]
        InstanceAddConnection,
        /// <summary>
        /// Removes a protocol connection
        /// </summary>
        [Description("Removes a protocol connection")]
        InstanceRemoveConnection,

        /// <summary>
        /// Pulls a localized text from the localization controller.
        /// </summary>
        [Description("Pulls a localized text from the localization controller.")]
        LanguageLocalize,

        /// <summary>
        /// Enables a plugin, allowing events/commands to be passed through to the plugin
        /// </summary>
        [Description("Enables a plugin, allowing events/commands to be passed through to the plugin")]
        PluginsEnable,
        /// <summary>
        /// Disables plugin, preventing events/commands being tunneled
        /// </summary>
        [Description("Disables plugin, preventing events/commands being tunneled")]
        PluginsDisable,

        /// <summary>
        /// Queries the permissions of a group
        /// </summary>
        [Description("Queries the permissions of a group")]
        SecurityQueryPermission,

        /// <summary>
        /// Set a group to have streaming permission set, a minimal amount of permissions
        /// to authenticate, get/set variables (flash) and query various sections of Procon.
        /// </summary>
        /// <remarks>All set permissions will be set to 1</remarks>
        [Description("Set a group to have streaming permission set, a minimal amount of permissions to authenticate, get/set variables (flash) and query various sections of Procon.")]
        SecuritySetPredefinedStreamPermissions,

        /// <summary>
        /// Sets a majority of the permissions an administrator would use. There are numerous commands
        /// that would only be useful for stream accounts, but I'm sure admins will want to have maximo
        /// power for no reson.
        /// </summary>
        /// <remarks>All set permissions will be set to 2</remarks>
        [Description("Sets a majority of the permissions an administrator would use.")]
        SecuritySetPredefinedAdministratorsPermissions,

        // Groups

        /// <summary>
        /// Adds a new group to the security controller
        /// </summary>
        [Description("Adds a new group to the security controller")]
        SecurityAddGroup,
        /// <summary>
        /// Removes a group
        /// </summary>
        [Description("Removes a group")]
        SecurityRemoveGroup,
        /// <summary>
        /// Sets a single permission on a group
        /// </summary>
        [Description("Sets a single permission on a group")]
        SecurityGroupSetPermission,
        /// <summary>
        /// Appends a single trait onto a permission
        /// </summary>
        [Description("Appends a single trait onto a permission")]
        SecurityGroupAppendPermissionTrait,
        /// <summary>
        /// Removes a single trait from a permission
        /// </summary>
        [Description("Removes a single trait from a permission")]
        SecurityGroupRemovePermissionTrait,
        /// <summary>
        /// Sets the description of the permission
        /// </summary>
        [Description("Sets the description of the permission")]
        SecurityGroupSetPermissionDescription,
        /// <summary>
        /// Copies the permissions from one group to another group
        /// </summary>
        [Description("Copies the permissions from one group to another group")]
        SecurityGroupCopyPermissions,

        // Accounts

        /// <summary>
        /// Adds a new account to a group
        /// </summary>
        [Description("Adds a new account to a group")]
        SecurityGroupAddAccount,
        /// <summary>
        /// Removes an account
        /// </summary>
        [Description("Removes an account")]
        SecurityRemoveAccount,

        /// <summary>
        /// Checks if an account can execute a given command
        /// </summary>
        [Description("Checks if an account can execute a given command")]
        SecurityAccountAuthenticate,
        /// <summary>
        /// Sets a new password for an account, updating the password hash
        /// with the new hashed password.
        /// </summary>
        [Description("Sets a new password for an account, updating the password hash with the new hashed password.")]
        SecurityAccountSetPassword,
        /// <summary>
        /// Modifies the password hash (literal) for the account
        /// </summary>
        [Description("Modifies the password hash (literal) for the account")]
        SecurityAccountSetPasswordHash,
        /// <summary>
        /// Sets the default language to use on an account
        /// </summary>
        [Description("Sets the default language to use on an account")]
        SecurityAccountSetPreferredLanguageCode,

        // Players

        /// <summary>
        /// Adds a player to an account
        /// </summary>
        [Description("Adds a player to an account")]
        SecurityAccountAddPlayer,
        /// <summary>
        /// Removes a player from an account
        /// </summary>
        [Description("Removes a player from an account")]
        SecurityRemovePlayer,

        /// <summary>
        /// Queries the database controller, dispatching a query to an open driver.
        /// </summary>
        [Description("Queries the database controller, dispatching a query to an open driver.")]
        DatabaseQuery,

        /// <summary>
        /// Finds a match against a registered text command then dispatches the
        /// command for execution.
        /// </summary>
        [Description("Finds a match against a registered text command then dispatches the command for execution.")]
        TextCommandsExecute,
        /// <summary>
        /// Preview a text command match, but do not dispatch for execution.
        /// </summary>
        [Description("Preview a text command match, but do not dispatch for execution.")]
        TextCommandsPreview,
        /// <summary>
        /// Adds a new text command
        /// </summary>
        [Description("Adds a new text command")]
        TextCommandsRegister,
        /// <summary>
        /// Removes a text command
        /// </summary>
        [Description("Removes a text command")]
        TextCommandsUnregister,

        /// <summary>
        /// Installs or updates a package to the latest version available.
        /// </summary>
        [Description("Installs or updates a package to the latest version available.")]
        PackagesMergePackage,
        /// <summary>
        /// Uninstalls a package.
        /// </summary>
        [Description("Uninstalls a package.")]
        PackagesUninstallPackage,
        /// <summary>
        /// Checks for the latest versions of all packages from source repositories. This
        /// command is dispatched asynchronously, so a later even will then need to
        /// be listened for when the packages list finally updates itself.
        /// </summary>
        [Description("Checks for the latest versions of all packages from source repositories. This command is dispatched asynchronously, so a later even will then need to be listened for when the packages list finally updates itself.")]
        PackagesFetchPackages,

        /// <summary>
        /// Fetches the supported protocols from the applicable protocol packages.
        /// </summary>
        [Description("Fetches the supported protocols from the applicable protocol packages.")]
        ProtocolsFetchSupportedProtocols,

        /// <summary>
        /// Sets a volatile variable that will not be saved to the config.
        /// </summary>
        [Description("Sets a volatile variable that will not be saved to the config.")]
        VariablesSet,
        /// <summary>
        /// Sets a variable which will be saved to a config
        /// </summary>
        [Description("Sets a variable which will be saved to a config")]
        VariablesSetA,
        /// <summary>
        /// Sets a variable which will be saved to the config, but only loaded as a volatile variable.
        /// </summary>
        [Description("Sets a variable which will be saved to the config, but only loaded as a volatile variable.")]
        VariablesSetF,
        /// <summary>
        /// Fetches a variable value
        /// </summary>
        [Description("Fetches a variable value")]
        VariablesGet,

        /// <summary>
        /// Fetches all events after an event id, provided the events remain in the stream.
        /// </summary>
        [Description("Fetches all events after an event id, provided the events remain in the stream.")]
        EventsFetchAfterEventId,
        /// <summary>
        /// Log a new event, assigning it an id and setting the stamp to the current date/time. 
        /// </summary>
        [Description("Log a new event, assigning it an id and setting the stamp to the current date/time. ")]
        EventsLog,

        // Game queries

        /// <summary>
        /// Fetches a list of players in the server right now.
        /// </summary>
        [Description("Fetches a list of players in the server right now.")]
        NetworkProtocolQueryPlayers,
        /// <summary>
        /// Fetches a full list of bans on the server, right now.
        /// </summary>
        [Description("Fetches a full list of bans on the server, right now.")]
        NetworkProtocolQueryBans,
        /// <summary>
        /// Fetches a list of maps currently running on the server.
        /// </summary>
        [Description("Fetches a list of maps currently running on the server.")]
        NetworkProtocolQueryMaps,
        /// <summary>
        /// Fetches a list of maps available on the server right now.
        /// </summary>
        [Description("Fetches a list of maps available on the server right now.")]
        NetworkProtocolQueryMapPool,
        /// <summary>
        /// Fetches a list of variables currently set on the server.
        /// </summary>
        [Description("Fetches a list of variables currently set on the server.")]
        NetworkProtocolQuerySettings,

    }
}
