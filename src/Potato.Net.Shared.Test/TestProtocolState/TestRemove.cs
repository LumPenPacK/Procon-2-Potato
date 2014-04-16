﻿#region Copyright
// Copyright 2014 Myrcon Pty. Ltd.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Potato.Net.Shared.Models;

namespace Potato.Net.Shared.Test.TestProtocolState {
    [TestFixture]
    public class TestRemove {

        // Players

        [Test]
        public void TestSinglePlayersRemoved() {
            var state = new ProtocolState() {
                Players = {
                    new PlayerModel() {
                        Uid = "1",
                        Score = 1
                    },
                    new PlayerModel() {
                        Uid = "2",
                        Score = 2
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                Players = new List<PlayerModel>() {
                    new PlayerModel() {
                        Uid = "1"
                    }
                }
            });

            Assert.AreEqual(1, state.Players.Count);
            Assert.AreEqual(2, state.Players.First().Score);
        }

        // Maps

        [Test]
        public void TestSingleMapsRemoved() {
            var state = new ProtocolState() {
                Maps = {
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode1"
                        },
                        FriendlyName = "Boring Map 1"
                    },
                    new MapModel() {
                        Name = "map2",
                        GameMode = new GameModeModel() {
                            Name = "gamemode2"
                        },
                        FriendlyName = "Boring Map 2"
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                Maps = new List<MapModel>() {
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode1"
                        }
                    }
                }
            });

            Assert.AreEqual(1, state.Maps.Count);
            Assert.AreEqual("Boring Map 2", state.Maps.First().FriendlyName);
        }

        [Test]
        public void TestTwoMapsIdenticalNamesDifferentGameModes() {
            var state = new ProtocolState() {
                Maps = {
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode1"
                        },
                        FriendlyName = "First Map"
                    },
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode2"
                        },
                        FriendlyName = "Second Map"
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                Maps = new List<MapModel>() {
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode1"
                        }
                    }
                }
            });

            Assert.AreEqual(1, state.Maps.Count);
            Assert.AreEqual("Second Map", state.Maps.First().FriendlyName);
        }

        // Bans

        [Test]
        public void TestSingleUidBansRemoved() {
            var state = new ProtocolState() {
                Bans = {
                    new BanModel() {
                        Scope = {
                            Times = {
                                new TimeSubsetModel() {
                                    Context = TimeSubsetContext.Permanent
                                }
                            },
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Uid = "1",
                                    Score = 1
                                }
                            }
                        }
                    },
                    new BanModel() {
                        Scope = {
                            Times = {
                                new TimeSubsetModel() {
                                    Context = TimeSubsetContext.Time
                                }
                            },
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Uid = "2",
                                    Score = 2
                                }
                            }
                        }
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                Bans = new List<BanModel>() {
                    new BanModel() {
                        Scope = {
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Uid = "1",
                                    Score = 1
                                }
                            }
                        }
                    }
                }
            });

            Assert.AreEqual(1, state.Bans.Count);
            Assert.AreEqual(TimeSubsetContext.Time, state.Bans.First().Scope.Times.First().Context);
        }

        [Test]
        public void TestSingleIpBansRemoved() {
            var state = new ProtocolState() {
                Bans = {
                    new BanModel() {
                        Scope = {
                            Times = {
                                new TimeSubsetModel() {
                                    Context = TimeSubsetContext.Permanent
                                }
                            },
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Ip = "1",
                                    Score = 1
                                }
                            }
                        }
                    },
                    new BanModel() {
                        Scope = {
                            Times = {
                                new TimeSubsetModel() {
                                    Context = TimeSubsetContext.Round
                                }
                            },
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Ip = "2",
                                    Score = 2
                                }
                            }
                        }
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                Bans = new List<BanModel>() {
                    new BanModel() {
                        Scope = {
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Ip = "1",
                                    Score = 1
                                }
                            }
                        }
                    }
                }
            });

            Assert.AreEqual(1, state.Bans.Count);
            Assert.AreEqual(TimeSubsetContext.Round, state.Bans.First().Scope.Times.First().Context);
        }

        [Test]
        public void TestSingleNameBansRemoved() {
            var state = new ProtocolState() {
                Bans = {
                    new BanModel() {
                        Scope = {
                            Times = {
                                new TimeSubsetModel() {
                                    Context = TimeSubsetContext.Permanent
                                }
                            },
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Name = "1",
                                    Score = 1
                                }
                            }
                        }
                    },
                    new BanModel() {
                        Scope = {
                            Times = {
                                new TimeSubsetModel() {
                                    Context = TimeSubsetContext.Round
                                }
                            },
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Name = "2",
                                    Score = 2
                                }
                            }
                        }
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                Bans = new List<BanModel>() {
                    new BanModel() {
                        Scope = {
                            Players = new List<PlayerModel>() {
                                new PlayerModel() {
                                    Name = "1",
                                    Score = 1
                                }
                            }
                        }
                    }
                }
            });

            Assert.AreEqual(1, state.Bans.Count);
            Assert.AreEqual(TimeSubsetContext.Round, state.Bans.First().Scope.Times.First().Context);
        }

        // MapPool

        [Test]
        public void TestSingleMapPoolRemoved() {
            var state = new ProtocolState() {
                MapPool = {
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode1"
                        },
                        FriendlyName = "Boring Map 1"
                    },
                    new MapModel() {
                        Name = "map2",
                        GameMode = new GameModeModel() {
                            Name = "gamemode2"
                        },
                        FriendlyName = "Boring Map 2"
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                MapPool = new List<MapModel>() {
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode1"
                        }
                    }
                }
            });

            Assert.AreEqual(1, state.MapPool.Count);
            Assert.AreEqual("Boring Map 2", state.MapPool.First().FriendlyName);
        }

        [Test]
        public void TestMapPoolIdenticalNamesDifferentGameModes() {
            var state = new ProtocolState() {
                MapPool = {
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode1"
                        },
                        FriendlyName = "First Map"
                    },
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode2"
                        },
                        FriendlyName = "Second Map"
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                MapPool = new List<MapModel>() {
                    new MapModel() {
                        Name = "map1",
                        GameMode = new GameModeModel() {
                            Name = "gamemode1"
                        }
                    }
                }
            });

            Assert.AreEqual(1, state.MapPool.Count);
            Assert.AreEqual("Second Map", state.MapPool.First().FriendlyName);
        }



        // GameModePool

        [Test]
        public void TestSingleGameModePoolRemoved() {
            var state = new ProtocolState() {
                GameModePool = {
                    new GameModeModel() {
                        Name = "gamemode 1",
                        FriendlyName = "Boring GameMode 1"
                    },
                    new GameModeModel() {
                        Name = "gamemode 2",
                        FriendlyName = "Boring GameMode 2"
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                GameModePool = new List<GameModeModel>() {
                    new GameModeModel() {
                        Name = "gamemode 1"
                    }
                }
            });

            Assert.AreEqual(1, state.GameModePool.Count);
            Assert.AreEqual("Boring GameMode 2", state.GameModePool.First().FriendlyName);
        }

        // Groups

        [Test]
        public void TestSingleGroupPoolRemoved() {
            var state = new ProtocolState() {
                Groups = {
                    new GroupModel() {
                        Uid = "1",
                        Type = GroupModel.Team,
                        FriendlyName = "Boring Group 1"
                    },
                    new GroupModel() {
                        Uid = "2",
                        Type = GroupModel.Team,
                        FriendlyName = "Boring Group 2"
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                Groups = new List<GroupModel>() {
                    new GroupModel() {
                        Uid = "1",
                        Type = GroupModel.Team
                    }
                }
            });

            Assert.AreEqual(1, state.Groups.Count);
            Assert.AreEqual("Boring Group 2", state.Groups.First().FriendlyName);
        }

        // Items

        [Test]
        public void TestSingleItemPoolRemoved() {
            var state = new ProtocolState() {
                Items = {
                    new ItemModel() {
                        Name = "1",
                        FriendlyName = "Boring Item 1"
                    },
                    new ItemModel() {
                        Name = "2",
                        FriendlyName = "Boring Item 2"
                    }
                }
            };

            state.Removed(new ProtocolStateSegment() {
                Items = new List<ItemModel>() {
                    new ItemModel() {
                        Name = "1"
                    }
                }
            });

            Assert.AreEqual(1, state.Items.Count);
            Assert.AreEqual("Boring Item 2", state.Items.First().FriendlyName);
        }
    }
}