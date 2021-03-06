#region Copyright
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Potato.Net.Shared.Protocols {
    [Obsolete]
    public static class SupportedGameTypes {

        /// <summary>
        /// List of cached supported game type attributes attached to their actual type.
        /// </summary>
        private static Dictionary<IProtocolType, Type> _supportedGames;

        /// <summary>
        /// Late loads Potato.Net.Protocols.*.dll's 
        /// </summary>
        private static IEnumerable<Assembly> LateBindGames() {
            List<Assembly> assemblies = new List<Assembly>() {
                Assembly.GetAssembly(typeof(IProtocol))
            };

            foreach (String protocol in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.Protocols.*.dll", SearchOption.AllDirectories)) {
                try {
                    assemblies.Add(Assembly.LoadFile(protocol));
                }
                catch { }
            }

            return assemblies;
        }

        public static Dictionary<IProtocolType, Type> GetSupportedGames(List<Assembly> additional = null) {
            Dictionary<IProtocolType, Type> games = SupportedGameTypes._supportedGames;

            if (games == null) {

                // Load the supported games
                IEnumerable<Assembly> assemblies = SupportedGameTypes.LateBindGames().Union(additional ?? new List<Assembly>());

                Regex supportedGamesNamespame = new Regex(@"^.*\.Protocols.*");

                // Cache the results 
                SupportedGameTypes._supportedGames = games = (from gameClassType in assemblies.SelectMany(assembly => assembly.GetTypes())
                                                              let gameType = (gameClassType.GetCustomAttributes(typeof(IProtocolType), false) as IEnumerable<IProtocolType>).FirstOrDefault()
                                                where gameType != null &&
                                                      gameClassType != null &&
                                                      gameClassType.IsClass == true &&
                                                      gameClassType.IsAbstract == false &&
                                                      gameClassType.Namespace != null &&
                                                      supportedGamesNamespame.IsMatch(gameClassType.Namespace) == true &&
                                                      typeof(IProtocol).IsAssignableFrom(gameClassType)
                                                select new {
                                                    Name = gameType,
                                                    Type = gameClassType
                                                }).ToDictionary(w => new ProtocolType(w.Name) as IProtocolType, w => w.Type);
            }

            return games;
        }
    }
}
