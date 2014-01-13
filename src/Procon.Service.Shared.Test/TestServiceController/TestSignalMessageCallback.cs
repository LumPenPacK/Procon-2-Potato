﻿using System.Collections.Generic;
using NUnit.Framework;
using Procon.Service.Shared.Test.TestServiceController.Mocks;

namespace Procon.Service.Shared.Test.TestServiceController {
    [TestFixture]
    public class TestSignalMessageCallback {
        /// <summary>
        /// Tests that passing in a merge message with the correct parameters will 
        /// call the packages merge
        /// </summary>
        [Test]
        public void TestMergeMessageWithParametersCallsMerge() {
            var merged = false;

            var service = new ServiceController() {
                Settings = {
                    ServiceUpdateCore = false
                },
                Packages = new MockServicePackageManager() {
                    PackageInstalled = (sender, uri, packageId) => { merged = true; }
                }
            };

            service.SignalMessage(new ServiceMessage() {
                Name = "merge",
                Arguments = new Dictionary<string, string>() {
                    { "uri", "localhost" },
                    { "packageid", "id" }
                }
            });

            Assert.IsTrue(merged);

            service.Dispose();
        }

        /// <summary>
        /// Tests that passing in a merge message with the correct parameters will 
        /// call the packages merge
        /// </summary>
        [Test]
        public void TestUninstallMessageWithParametersCallsMerge() {
            var uninstall = false;

            var service = new ServiceController() {
                Settings = {
                    ServiceUpdateCore = false
                },
                Packages = new MockServicePackageManager() {
                    PackageUninstalled = (sender, uri, packageId) => { uninstall = true; }
                }
            };

            service.SignalMessage(new ServiceMessage() {
                Name = "uninstall",
                Arguments = new Dictionary<string, string>() {
                    { "packageid", "id" }
                }
            });

            Assert.IsTrue(uninstall);

            service.Dispose();
        }

        /// <summary>
        /// Tests the signal begin callback is called
        /// </summary>
        [Test]
        public void TestSignalBeginCallback() {
            var signaled = false;

            var service = new ServiceController() {
                Settings = {
                    ServiceUpdateCore = false
                },
                SignalBegin = (controller, message) => signaled = true
            };

            service.SignalMessage(new ServiceMessage() {
                Name = "help"
            });

            Assert.IsTrue(signaled);

            service.Dispose();
        }

        /// <summary>
        /// Tests the signal end callback is called
        /// </summary>
        [Test]
        public void TestSignalEndCallback() {
            var signaled = false;

            var service = new ServiceController() {
                Settings = {
                    ServiceUpdateCore = false
                },
                SignalEnd = (controller, message, seconds) => signaled = true
            };

            service.SignalMessage(new ServiceMessage() {
                Name = "help"
            });

            Assert.IsTrue(signaled);

            service.Dispose();
        }

        /// <summary>
        /// Tests an error callback is called when a parameter is missing from a signal message
        /// that requires parameters
        /// </summary>
        [Test]
        public void TestSignalParameterError() {
            var error = false;

            var service = new ServiceController() {
                Settings = {
                    ServiceUpdateCore = false
                },
                SignalParameterError = (controller, list) => error = true
            };

            service.SignalMessage(new ServiceMessage() {
                Name = "merge",
                Arguments = new Dictionary<string, string>() {
                    { "uri", "localhost" }
                }
            });

            Assert.IsTrue(error);

            service.Dispose();
        }

        /// <summary>
        /// Tests the signal statistics callback is called
        /// </summary>
        [Test]
        public void TestSignalStatisticsCallback() {
            var signaled = false;

            var service = new ServiceController() {
                Settings = {
                    ServiceUpdateCore = false
                },
                SignalStatistics = (controller, message) => signaled = true
            };

            service.SignalMessage(new ServiceMessage() {
                Name = "statistics"
            });

            Assert.IsTrue(signaled);

            service.Dispose();
        }

        /// <summary>
        /// Tests the signal help callback is called
        /// </summary>
        [Test]
        public void TestSignalHelpCallback() {
            var signaled = false;

            var service = new ServiceController() {
                Settings = {
                    ServiceUpdateCore = false
                },
                SignalHelp = controller => signaled = true
            };

            service.SignalMessage(new ServiceMessage() {
                Name = "help"
            });

            Assert.IsTrue(signaled);

            service.Dispose();
        }
    }
}