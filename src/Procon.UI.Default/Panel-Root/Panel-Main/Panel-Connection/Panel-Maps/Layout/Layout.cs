﻿using System;
using System.Windows;
using System.Windows.Controls;

using Procon.UI.API;

namespace Procon.UI.Default.Root.Main.Connection.Maps.Layout
{
    [Extension(
        Alters    = new String[] { "MainConnectionLayout" },
        Replaces  = new String[] { },
        DependsOn = new String[] { "Main Connection Layout" })]
    public class Layout : IExtension
    {
        #region IExtension Properties

        public string Author
        { get { return "Imisnew2"; } }

        public string Link
        { get { return "www.TeamPlayerGaming.com/members/Imisnew2.html"; } }

        public string LinkText
        { get { return "Team Player Gaming"; } }

        public string Name
        { get { return "Main Connection Maps Layout"; } }

        public string Version
        { get { return "1.0.0.0"; } }

        public string Description
        { get { return ""; } }

        #endregion IExtension Properties

        [STAThread]
        public bool Entry(Window root)
        {
            // Find the controls I want to use and check for issues.
            Grid layout = ExtensionApi.FindControl<Grid>(root, "MainConnectionLayout");
            if  (layout == null) return false;

            // Do what I need to setup my control.
            LayoutView view = new LayoutView();
            Grid.SetRow(view, 1);
            layout.Children.Add(view);

            // Exit with good status.
            return true;
        }
    }
}
