//
// (C) Copyright 2003-2008 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

 
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

namespace Revit.SDK.Samples.ApplicationEvents.CS
{
    /// <summary>
    /// A class inherits IExternalCommand interface.
    /// this class controls the class which subscribes handle events and the events' information UI.
    /// like a bridge between them.
    /// </summary>
    public class Command : IExternalCommand
    {
        #region Class Memeber Variables
        // a window used to show events' information
        private static ApplicationEventsInfoWindows InfWindows;

        // subscribe to relevant application event and prepare information when event is notified. 
        private static RevitApplicationEvents appEvents; 

        // application of Revit
        private Autodesk.Revit.Application m_revit;
        #endregion


        #region Class Static Property
        /// <summary>
        /// Property to get and set private member variables of InfoWindows
        /// </summary>
        public static ApplicationEventsInfoWindows InfoWindows
        {
            get
            {
                if (null == InfWindows)
                {
                    InfWindows = new ApplicationEventsInfoWindows();
                }

                return InfWindows;
            }
            set 
            { 
                InfWindows = value; 
            }
        }
        #endregion


        #region Class Interface Implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(ExternalCommandData commandData,
                                               ref string message,
                                               ElementSet elements)
        {
            try
            {
                // Set Revit application to private variable m_revit
                m_revit = commandData.Application;

                // Show or hide the modeless window
                ControlWindowsShowed();

                return IExternalCommand.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return IExternalCommand.Result.Failed;
            }
        }
        #endregion


        #region Class Implementation
        /// <summary>
        /// Control the visibility of the modeless window which shows the events information
        /// </summary>
        private void ControlWindowsShowed()
        {
            // if the window wasn't shown in desktop before, 
            // show this window modeless once users run this external command again.
            if (null == InfWindows)
            {
                InfWindows = new ApplicationEventsInfoWindows(GetApplicationEventsInstance());
                InfWindows.Show();
            }
            else  // else close this modeless window when users run this external command again.
            {
                InfWindows.Close();
            }
        }

        /// <summary>
        /// Get the instance which subscribes events and prepares relevant events' information
        /// the life cycle of this instance is from call start of this method to shut down of Revit.
        /// </summary>
        /// <returns>The instance which subscribes events and prepares relevant events' information</returns>
        private RevitApplicationEvents GetApplicationEventsInstance()
        {
            if (null == appEvents)
            {
                appEvents = new RevitApplicationEvents(m_revit);
            }

            return appEvents;
        }
        #endregion
    }
}
