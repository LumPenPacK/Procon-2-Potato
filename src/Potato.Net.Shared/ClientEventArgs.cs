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
using System;

namespace Potato.Net.Shared {
    /// <summary>
    /// An event originating from the networking side of the protocol implementation.
    /// </summary>
    [Serializable]
    public class ClientEventArgs : EventArgs, IClientEventArgs {
        public ClientEventType EventType { get; set; }

        public ConnectionState ConnectionState { get; set; }

        public DateTime Stamp { get; set; }

        public IClientEventData Then { get; set; }

        public IClientEventData Now { get; set; }

        /// <summary>
        /// Initializes the event with the default values.
        /// </summary>
        public ClientEventArgs() {
            this.Stamp = DateTime.Now;

            this.Then = new ClientEventData();
            this.Now = new ClientEventData();
        }
    }
}
