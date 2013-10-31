﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Procon.Core.Test.Plugins {
    using Procon.Core.Connections.Plugins;
    [TestClass]
    public class TestPluginsSerialization {

        /// <summary>
        /// Makes sure executing a command across the appdomain will serialize
        /// the basic command result across the app domain.
        /// </summary>
        [TestMethod]
        public void TestPluginsSerializationCommandResult() {
            PluginController plugins = new PluginController().Execute() as PluginController;

            CommandResultArgs result = plugins.Execute(new Command() {
                Name = "TestPluginsSerializationCommandResult",
                Username = "Phogue",
                Parameters = TestHelpers.ObjectListToContentList(new List<Object>() {
                    "Return Message"
                })
            });

            Assert.IsTrue(result.Success);
            Assert.AreEqual(CommandResultType.Success, result.Status);
            Assert.AreEqual("Return Message", result.Message);
        }
    }
}