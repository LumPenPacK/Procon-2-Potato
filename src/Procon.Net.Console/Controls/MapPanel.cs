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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Procon.Net.Console.Controls {
    using Procon.Net.Protocols.Objects;

    public partial class MapPanel : UserControl {

        private Game m_activeGame;
        public Game ActiveGame {
            get {
                return this.m_activeGame;
            }
            set {
                this.m_activeGame = value;

                // Assign events.
                if (this.m_activeGame != null) {
                    this.m_activeGame.GameEvent += new Game.GameEventHandler(m_activeGame_GameEvent);
                    this.m_activeGame.ClientEvent += new Game.ClientEventHandler(m_activeGame_ClientEvent);
                }
            }
        }

        public MapPanel() {
            InitializeComponent();

            this.cboActions.Items.AddRange(
                new Object[] {
                    new Map() {
                        MapActionType = MapActionType.NextMap
                    },
                    new Map() {
                        MapActionType = MapActionType.NextRound
                    },
                    new Map() {
                        MapActionType = MapActionType.RestartMap
                    },
                    new Map() {
                        MapActionType = MapActionType.RestartRound
                    }
                }
            );

            this.cboActions.SelectedIndex = 0;
        }


        private void RefreshMapList() {
            this.lsvMapList.BeginUpdate();

            foreach (ListViewItem mapListItem in this.lsvMapList.Items) {

                Map mapObject = mapListItem.Tag as Map;

                if (mapObject != null) {

                    mapListItem.Text = mapObject.Index.ToString();

                    if (mapListItem.SubItems.Count <= 1) {
                        mapListItem.SubItems.AddRange(
                            new ListViewItem.ListViewSubItem[] {
                                new ListViewItem.ListViewSubItem() {
                                    Text = mapObject.Name
                                },
                                new ListViewItem.ListViewSubItem() {
                                    Text = mapObject.GameMode.FriendlyName
                                },
                                new ListViewItem.ListViewSubItem() {
                                    Text = mapObject.Rounds.ToString()
                                }
                            }
                        );
                    }
                    else {
                        mapListItem.SubItems[1].Text = mapObject.Name;
                        mapListItem.SubItems[2].Text = mapObject.GameMode.FriendlyName;
                        mapListItem.SubItems[3].Text = mapObject.Rounds.ToString();
                    }
                }
            }

            foreach (ColumnHeader column in this.lsvMapList.Columns) {
                column.Width = -2;
            }

            this.lsvMapList.EndUpdate();
        }

        private void m_activeGame_ClientEvent(Game sender, ClientEventArgs e) {
            if (this.InvokeRequired == true) {
                this.Invoke(new Action<Game, ClientEventArgs>(this.m_activeGame_ClientEvent), sender, e);
                return;
            }

            if (e.ConnectionState == ConnectionState.Connected) {
                this.lsvMapList.Items.Clear();
            }
        }

        private void m_activeGame_GameEvent(Game sender, GameEventArgs e) {
            if (this.InvokeRequired == true) {
                this.Invoke(new Action<Game, GameEventArgs>(this.m_activeGame_GameEvent), sender, e);
                return;
            }

            if (e.EventType == GameEventType.MaplistUpdated) {
                this.lsvMapList.Items.Clear();

                foreach (Map map in this.ActiveGame.State.MapList) {
                    this.lsvMapList.Items.Add(
                        new ListViewItem() {
                            Tag = map
                        }
                    );
                }

                this.RefreshMapList();
            }
        }

        private void RefreshQuickAction() {
            if (this.cboActions.SelectedIndex >= 0) {

                if (this.cboActions.SelectedItem is Map) {
                    this.quickActionsPropertyGrid.SelectedObject = this.cboActions.SelectedItem;
                }

                this.quickActionsPropertyGrid.Refresh();
            }
        }

        private void cboActions_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.cboActions.SelectedIndex >= 0) {
                this.quickActionsPropertyGrid.SelectedObject = this.cboActions.SelectedItem;

                this.RefreshQuickAction();
            }
        }

        private void btnAction_Click(object sender, EventArgs e) {
            if (this.ActiveGame != null && this.cboActions.SelectedIndex >= 0) {
                this.ActiveGame.Action((ProtocolObject)this.cboActions.SelectedItem);

                this.RefreshQuickAction();
            }
        }

        private void cboActions_DrawItem(object sender, DrawItemEventArgs e) {
            e.DrawBackground();
            e.DrawFocusRectangle();

            if (e.Index >= 0) {
                if (this.cboActions.Items[e.Index] is Map) {
                    e.Graphics.DrawString(
                        String.Format("Map.{0}", (this.cboActions.Items[e.Index] as Map).MapActionType),
                        this.cboActions.Font,
                        SystemBrushes.ControlText,
                        e.Bounds.Left + 5,
                        e.Bounds.Top
                    );
                }
            }
        }
    }
}