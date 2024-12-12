//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.ValidateParameters.CS
{
    /// <summary>
    /// A class inherits IExternalApplication interface to add an event to the document saving,
    /// which will be called when the document is being saved.
    /// </summary>
    class Application: IExternalApplication
    {
        #region IExternalApplication Members    
        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit starts before a file or default template is actually loaded.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully started. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then Revit should inform the user that the external application 
        /// failed to load and the release the internal reference.</returns>
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            try
            {               
                application.DocumentSaving += new EventHandler<Autodesk.Revit.Events.DocumentSavingEventArgs>(application_DocumentSaving);
                application.DocumentSavingAs += new EventHandler<Autodesk.Revit.Events.DocumentSavingAsEventArgs>(application_DocumentSavingAs);
            }
            catch (Exception)
            {
                return IExternalApplication.Result.Failed;
            }
            return IExternalApplication.Result.Succeeded;
        }

        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit is about to exit,Any documents must have been closed before this method is called.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully shutdown. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then the Revit user should be warned of the failure of the external 
        /// application to shut down correctly.</returns>
        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            try
            {
                application.DocumentSaving -= new EventHandler<Autodesk.Revit.Events.DocumentSavingEventArgs>(application_DocumentSaving);
                application.DocumentSavingAs -= new EventHandler<Autodesk.Revit.Events.DocumentSavingAsEventArgs>(application_DocumentSavingAs);
            }
            catch (Exception)
            {
                return IExternalApplication.Result.Failed;
            }
            return IExternalApplication.Result.Succeeded;
        }

        /// <summary>
        /// Subscribe to the DocumentSaving event to be notified when Revit is just about to save the document. 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">The event argument used by DocumentSaving event. </param>
        private void application_DocumentSaving(Object sender, Autodesk.Revit.Events.DocumentSavingEventArgs e)
        {  
            validateParameters(e.Document);
        }

        /// <summary>
        /// Subscribe to the DocumentSavingAs event to be notified when Revit is just about to save the document with a new file name. 
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">The event argument used by DocumentSavingAs event.</param>
        private void application_DocumentSavingAs(Object sender, Autodesk.Revit.Events.DocumentSavingAsEventArgs e)
        {   
            validateParameters(e.Document);
        }

        /// <summary>
        /// The method is to validate parameters via FamilyParameter and FamilyType
        /// </summary>
        /// <param name="doc">the document which need to validate parameters</param>
        private void validateParameters(Document doc)
        {
            List<string> errorInfo = new List<string>();
            FamilyManager familyManager;
            if (doc.IsFamilyDocument)
            {
                familyManager = doc.FamilyManager;
                errorInfo = Command.ValidateParameters(familyManager);
            }
            else
            {
                errorInfo.Add("The current document isn't a family document, so the validation doesn't work correctly!");
            }
            using (MessageForm msgForm = new MessageForm(errorInfo.ToArray()))
            {
                msgForm.StartPosition = FormStartPosition.CenterParent;
                msgForm.ShowDialog();
            }
        }
        #endregion
    }
}
