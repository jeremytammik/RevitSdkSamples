//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

/// <summary>
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;

namespace REX.Common
{
    /// <summary>
    /// Foundation class for REX.Foundation usage.
    /// </summary>
    public class REXFoundationApplication : Autodesk.REX.Framework.IREXApplication2
    {
        protected Autodesk.REX.Framework.IREXApplication2 ApplicationRef;

        /// <summary>
        /// Initializes a new instance of the <see cref="REXApplication"/> class.
        /// </summary>
        public REXFoundationApplication()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);

            OnCreateApplication();
        }

        protected virtual void OnCreateApplication()
        {
        }

        /// <summary>
        /// Handles the AssemblyResolve event of the currentDomain control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        System.Reflection.Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Autodesk.REX.Framework.REXConfiguration.Initialize(System.Reflection.Assembly.GetExecutingAssembly());
            return Autodesk.REX.Framework.REXAssemblies.Resolve(sender, args, Autodesk.REX.Framework.REXConfiguration.Control.VersionName, System.Reflection.Assembly.GetExecutingAssembly());
        }

        #region IREXApplication2 Members

        /// <summary>
        /// Create module.
        /// </summary>
        /// <param name="AppContext">The application context.</param>
        /// <returns>Returns true if succeeded.</returns>
        public bool Create(ref Autodesk.REX.Framework.REXContext AppContext)
        {
            if (AppContext.Extension.Name == null || AppContext.Extension.Name == "")
                AppContext.Extension.Name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            return ApplicationRef.Create(ref AppContext);
        }

        /// <summary>
        /// Creates and shows module as modal dialog.
        /// </summary>
        /// <param name="AppContext">The application context.</param>
        /// <returns>Returns true if suceeded.</returns>
        public bool Modal(ref Autodesk.REX.Framework.REXContext AppContext)
        {
            if (AppContext.Extension.Name == null || AppContext.Extension.Name == "")
                AppContext.Extension.Name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            return ApplicationRef.Modal(ref AppContext);
        }

        #endregion

        #region IREXApplication Members

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            ApplicationRef.Close();
        }

        /// <summary>
        /// Commands the specified command struct.
        /// </summary>
        /// <param name="CommandStruct">The command struct.</param>
        /// <returns></returns>
        public object Command(ref Autodesk.REX.Framework.REXCommand CommandStruct)
        {
            return ApplicationRef.Command(ref CommandStruct);
        }

        /// <summary>
        /// Controls this instance.
        /// </summary>
        /// <returns></returns>
        public object Control()
        {
            return ApplicationRef.Control();
        }

        /// <summary>
        /// Creates and shows module as modal dialog.
        /// </summary>
        /// <param name="AppContext">The application context.</param>
        /// <returns>Returns true if suceeded.</returns>
        public bool Create(ref Autodesk.REX.Framework.REXApplicationContext AppContext)
        {
            return ApplicationRef.Create(ref AppContext);
        }

        /// <summary>
        /// Called when event.
        /// </summary>
        /// <param name="EventStruct">The event structure.</param>
        /// <returns>Returns result of event operation.</returns>
        public object Event(ref Autodesk.REX.Framework.REXEvent EventStruct)
        {
            return ApplicationRef.Event(ref EventStruct);
        }

        /// <summary>
        /// Creates and shows module as modal dialog.
        /// </summary>
        /// <param name="AppContext">The application context.</param>
        /// <returns>Returns true if suceeded.</returns>
        public bool Modal(ref Autodesk.REX.Framework.REXApplicationContext AppContext)
        {
            return ApplicationRef.Modal(ref AppContext);
        }

        /// <summary>
        /// Shows this module.
        /// </summary>
        /// <returns>Returns true if succeeded.</returns>
        public bool Show()
        {
            return ApplicationRef.Show();
        }

        #endregion
    }
}
