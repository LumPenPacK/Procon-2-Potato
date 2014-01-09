﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Procon.Core.Packages;
using Procon.Core.Shared;
using Procon.Core.Shared.Models;
using Procon.Core.Variables;

namespace Procon.Core.Test.Packages {
    /// <summary>
    /// Tests the packages controller's basic functionality, setting up remote repositories, polling etc.
    /// </summary>
    /// <remarks>
    ///     <para>While we use Nuget packages for testing we do not validate any of Nuget's processes</para>
    /// </remarks>
    [TestFixture]
    public class TestPackagesController {

        /// <summary>
        /// Tests the grouped repository setting can be setup via variables.
        /// </summary>
        [Test]
        public void TestPackageGroupSetup() {
            var variables = new VariableController();

            var @namespace = "";

            var packages = new PackagesController() {
                Shared = {
                    Variables = variables
                }
            }.Execute() as PackagesController;

            variables.Set(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.VariablesSet
            }, VariableModel.NamespaceVariableName(@namespace, CommonVariableNames.PackagesRepositoryUri), "path-to-repository");

            variables.Set(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.VariablesSet
            }, CommonVariableNames.PackagesConfigGroups, new List<String>() {
                @namespace
            });

            Assert.IsNotEmpty(packages.Cache.Repositories);
            Assert.AreEqual("path-to-repository", packages.Cache.Repositories.FirstOrDefault().Uri);
        }

        /// <summary>
        /// Tests the grouped repository setting can be setup via variables, with the group being
        /// set first then the group name added to the known group spaces
        /// </summary>
        [Test]
        public void TestPackageGroupSetupOrderReversed() {
            var variables = new VariableController();

            var @namespace = "";

            var packages = new PackagesController() {
                Shared = {
                    Variables = variables
                }
            }.Execute() as PackagesController;

            variables.Set(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.VariablesSet
            }, CommonVariableNames.PackagesConfigGroups, new List<String>() {
                @namespace
            });

            variables.Set(new Command() {
                Origin = CommandOrigin.Local,
                CommandType = CommandType.VariablesSet
            }, VariableModel.NamespaceVariableName(@namespace, CommonVariableNames.PackagesRepositoryUri), "path-to-repository");

            Assert.IsNotEmpty(packages.Cache.Repositories);
            Assert.AreEqual("path-to-repository", packages.Cache.Repositories.FirstOrDefault().Uri);
        }

        /// <summary>
        /// Tests variables are nulled during a dispose.
        /// </summary>
        [Test]
        public void TestDispose() {
            PackagesController packages = new PackagesController();

            packages.Dispose();

            Assert.IsNull(packages.GroupedVariableListener);
            Assert.IsNull(packages.LocalRepository);
            Assert.IsNull(packages.Cache);
        }
    }
}