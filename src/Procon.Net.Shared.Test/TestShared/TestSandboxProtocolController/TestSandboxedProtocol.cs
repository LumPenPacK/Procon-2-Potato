﻿using System.Collections.Generic;
using Myrcon.Protocols.Test;
using NUnit.Framework;
using Procon.Net.Shared.Sandbox;

namespace Procon.Net.Shared.Test.TestShared.TestSandboxProtocolController {
    /// <summary>
    /// Tests all functionality are passed through to the underlying protocol object
    /// </summary>
    [TestFixture]
    public class TestSandboxedProtocol {

        [Test]
        public void TestClientSandboxedNotNull() {
            IClient client = new MockClient();

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    WaitingClient = client
                }
            };

            Assert.AreEqual(client, controller.Client);
        }

        [Test]
        public void TestClientSandboxedNull() {
            var controller = new SandboxProtocolController();

            Assert.Null(controller.Client);
        }

        [Test]
        public void TestStateSandboxedNotNull() {
            IProtocolState state = new ProtocolState();

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    WaitingState = state
                }
            };

            Assert.AreEqual(state, controller.State);
        }

        [Test]
        public void TestStateSandboxedNull() {
            var controller = new SandboxProtocolController();

            Assert.Null(controller.State);
        }

        [Test]
        public void TestOptionsSandboxedNotNull() {
            IProtocolSetup options = new ProtocolSetup();

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    WaitingOptions = options
                }
            };

            Assert.AreEqual(options, controller.Options);
        }

        [Test]
        public void TestOptionsSandboxedNull() {
            var controller = new SandboxProtocolController();

            Assert.Null(controller.Options);
        }

        [Test]
        public void TestProtocolTypeSandboxedNotNull() {
            IProtocolType protocolType = new ProtocolType();

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    WaitingProtocolType = protocolType
                }
            };

            Assert.AreEqual(protocolType, controller.ProtocolType);
        }

        [Test]
        public void TestProtocolTypeSandboxedNull() {
            var controller = new SandboxProtocolController();

            Assert.Null(controller.ProtocolType);
        }

        [Test]
        public void TestSetupSandboxedNotNull() {
            var called = false;

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    OnSetupHandler = setup => {
                        called = true;

                        return new ProtocolSetupResult();
                    }
                }
            };

            controller.Setup(null);

            Assert.IsTrue(called);
        }

        [Test]
        public void TestSetupSandboxedNull() {
            var controller = new SandboxProtocolController();

            Assert.Null(controller.Setup(null));
        }

        [Test]
        public void TestActionSandboxedNotNull() {
            var called = false;

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    OnActionHandler = action => {
                        called = true;

                        return new List<IPacket>();
                    }
                }
            };

            controller.Action(null);

            Assert.IsTrue(called);
        }

        [Test]
        public void TestActionSandboxedNull() {
            var controller = new SandboxProtocolController();

            Assert.Null(controller.Action(null));
        }

        [Test]
        public void TestSendSandboxedNotNull() {
            var called = false;

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    OnSendHandler = packet => {
                        called = true;

                        return new Packet();
                    }
                }
            };

            controller.Send(null);

            Assert.IsTrue(called);
        }

        [Test]
        public void TestSendSandboxedNull() {
            var controller = new SandboxProtocolController();

            Assert.Null(controller.Send(null));
        }

        [Test]
        public void TestAttemptConnectionSandboxedNotNull() {
            var called = false;

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    OnAttemptConnectionHandler = () => {
                        called = true;
                    }
                }
            };

            controller.AttemptConnection();

            Assert.IsTrue(called);
        }

        [Test]
        public void TestShutdownSandboxedNotNull() {
            var called = false;

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    OnShutdownHandler = () => {
                        called = true;
                    }
                }
            };

            controller.Shutdown();

            Assert.IsTrue(called);
        }

        [Test]
        public void TestSynchronizeSandboxedNotNull() {
            var called = false;

            var controller = new SandboxProtocolController() {
                SandboxedProtocol = new MockIntegrationTestProtocol() {
                    OnSynchronizeHandler = () => {
                        called = true;
                    }
                }
            };

            controller.Synchronize();

            Assert.IsTrue(called);
        }
    }
}