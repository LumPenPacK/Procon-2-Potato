﻿using System.Linq;
using NUnit.Framework;
using Procon.Core.Connections.Plugins;
using Procon.Core.Database;
using Procon.Core.Shared;
using Procon.Core.Shared.Models;
using Procon.Core.Variables;
using Procon.Database.Shared.Builders.Methods.Data;

namespace Procon.Examples.Plugins.Database.Test {
    [TestFixture]
    public class TestDatabase {

        /// <summary>
        /// Opens a database driver to a SQLite in-memory database
        /// </summary>
        /// <returns>this</returns>
        public static DatabaseController OpenDatabaseDriver() {
            VariableController variables = new VariableController();

            DatabaseController database = new DatabaseController() {
                Shared = {
                    Variables = variables
                }
            }.Execute() as DatabaseController;
            
            variables.Set(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.VariablesSet
            }, CommonVariableNames.DatabaseDriverName, "SQLite");

            variables.Set(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.VariablesSet
            }, CommonVariableNames.DatabaseMemory, true);
            
            return database;
        }

        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.Database.SaveOneUser
        /// </summary>
        [Test]
        public void TestSavingSingleRowFromModel() {
            DatabaseController database = TestDatabase.OpenDatabaseDriver();

            // Create a new plugin controller to load up the test plugin
            CorePluginController plugins = new CorePluginController() {
                BubbleObjects = {
                    database
                }
            };

            plugins.Execute();

            // Enabling the plugin should then load up the migrations and execute them.
            // See Procon.Examples.Database.GenericEvent
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                ScopeModel = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            ICommandResult result = plugins.Tunnel(new Command() {
                Name = "SaveOneUser",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local,
                ScopeModel = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            // Test the command was successful
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(CommandResultType.Success, result.CommandResultType);

            // Test that data was inserted.
            result = database.Tunnel(
                CommandBuilder.DatabaseQuery(
                new Find()
                .Collection("Procon_Example_Database_Users")
                .Limit(1)
                ).SetOrigin(CommandOrigin.Local)
            );

            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(CommandResultType.Success, result.CommandResultType);

            // UserModel is found in Procon.Examples.Database.UserModel. We reuse it here in the test to keep everything consistent.
            UserModel model = UserModel.FirstFromQuery(result.Now.Queries.FirstOrDefault());

            Assert.AreEqual("Phogue", model.Name);
        }


        /// <summary>
        /// Test for you to debug.
        /// Set a breakpoint within Procon.Examples.Database.SaveOneUser
        /// Set a breakpoint within Procon.Examples.Database.FindOneUser
        /// </summary>
        [Test]
        public void TestSavingAndFindingSingleRowFromModel() {
            DatabaseController database = TestDatabase.OpenDatabaseDriver();

            // Create a new plugin controller to load up the test plugin
            CorePluginController plugins = new CorePluginController() {
                BubbleObjects = {
                    database
                }
            };

            plugins.Execute();

            // Enabling the plugin should then load up the migrations and execute them.
            // See Procon.Examples.Database.GenericEvent
            plugins.Tunnel(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.PluginsEnable,
                ScopeModel = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            plugins.Tunnel(new Command() {
                Name = "SaveOneUser",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local,
                ScopeModel = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            ICommandResult result = plugins.Tunnel(new Command() {
                Name = "FindOneUser",
                // We're cheating a little bit here and just saying the command came from
                // "local" as in it was generated by Procon itself.
                Origin = CommandOrigin.Local,
                ScopeModel = {
                    PluginGuid = plugins.LoadedPlugins.First().PluginGuid
                }
            });

            // Test the command was successful
            Assert.AreEqual(true, result.Success);
            Assert.AreEqual(CommandResultType.Success, result.CommandResultType);
            Assert.AreEqual("Phogue", result.Message);
        }
    }
}
