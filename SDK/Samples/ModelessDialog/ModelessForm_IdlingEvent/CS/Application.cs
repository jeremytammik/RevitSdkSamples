//
// (C) Copyright 2003-2019 by Autodesk, Inc. All rights reserved.
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
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace Revit.SDK.Samples.ModelessForm_IdlingEvent.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    public class Application : IExternalApplication
    {
        // class instance
        internal static Application thisApp = null;
        // ModelessForm instance
        private ModelessForm m_MyForm;
        
        #region IExternalApplication Members
        /// <summary>
        /// Implements the OnShutdown event
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            if (m_MyForm != null && !m_MyForm.IsDisposed)
            {
                m_MyForm.Dispose();
                m_MyForm = null;

                // if we've had a dialog, we had subscribed
                application.Idling -= IdlingHandler;
            }

            return Result.Succeeded;
        }

        /// <summary>
        /// Implements the OnStartup event
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication application)
        {
            m_MyForm = null;   // no dialog needed yet; the command will bring it
            thisApp = this;  // static access to this application instance

            return Result.Succeeded;
        }

        /// <summary>
        ///   This method creates and shows a modeless dialog, unless it already exists.
        /// </summary>
        /// <remarks>
        ///   The external command invokes this on the end-user's request
        /// </remarks>
        /// 
        public void ShowForm(UIApplication uiapp)
        {
            // If we do not have a dialog yet, create and show it
            if (m_MyForm == null || m_MyForm.IsDisposed)
            {
                m_MyForm = new ModelessForm();
                m_MyForm.Show();

                // if we have a dialog, we need Idling too
                uiapp.Idling += IdlingHandler;
            }
        }

        /// <summary>
        ///   A handler for the Idling event.
        /// </summary>
        /// <remarks>
        ///   We keep the handler very simple. First we check
        ///   if we still have the dialog. If not, we unsubscribe from Idling,
        ///   for we no longer need it and it makes Revit speedier.
        ///   If we do have the dialog around, we check if it has a request ready
        ///   and process it accordingly.
        /// </remarks>
        /// 
        public void IdlingHandler(object sender, IdlingEventArgs args)
        {
            UIApplication uiapp = sender as UIApplication;

            if (m_MyForm.IsDisposed)
            {
                uiapp.Idling -= IdlingHandler;
                return;
            }
            else   // dialog still exists
            {
                // fetch the request from the dialog

                RequestId request = m_MyForm.Request.Take();

                if (request != RequestId.None)
                {
                    try
                    {
                        // we take the request, if any was made,
                        // and pass it on to the request executor

                        RequestHandler.Execute(uiapp, request);
                    }
                    finally
                    {
                        // The dialog may be in its waiting state;
                        // make sure we wake it up even if we get an exception.

                        m_MyForm.WakeUp();
                    }
                }
            }

            return;
        }

        #endregion
    }
}
