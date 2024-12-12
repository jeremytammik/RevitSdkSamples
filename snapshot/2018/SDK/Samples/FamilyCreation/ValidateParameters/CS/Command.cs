//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ValidateParameters.CS
{  
    /// <summary>
    /// A class inherits IExternalCommand interface.
    /// this class controls the class which subscribes handle events and the events' information UI.
    /// like a bridge between them.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command:IExternalCommand
    {
        #region Class Memeber Variables
        /// <summary>
        /// store the family manager
        /// </summary>
        FamilyManager m_familyManager;         
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
                                             ref string message,
                                             ElementSet elements)
        {
            Document document;            
            document = commandData.Application.ActiveUIDocument.Document;           
            // only a family document can retrieve family manager
            if (document.IsFamilyDocument)
            {                
                m_familyManager = document.FamilyManager;
                List<string> errorMessages = ValidateParameters(m_familyManager);
                using(MessageForm msgForm = new MessageForm(errorMessages.ToArray()))
                {
                    msgForm.StartPosition = FormStartPosition.CenterParent;
                    msgForm.ShowDialog();                    
                    return Autodesk.Revit.UI.Result.Succeeded;
                }
            }
            else
            {
                message = "please make sure you have opened a family document!";
                return Autodesk.Revit.UI.Result.Failed;
            }
        }
        #endregion

        #region Class Implementation
        /// <summary>
        /// implementation of validate parameters, get all family types and parameters, 
        /// use the function FamilyType.HasValue() to make sure if the parameter needs to
        /// validate. Then along to the storage type to validate the parameters.
        /// </summary>
        /// <returns>error information list</returns>
        public static List<string> ValidateParameters(FamilyManager familyManager)
        {
            List<string> errorInfo = new List<string>();
            // go though all parameters
            foreach (FamilyType type in familyManager.Types)
            {
                bool right = true;
                foreach (FamilyParameter para in familyManager.Parameters)
                {
                    try
                    { 
                        if (type.HasValue(para))
                        {
                            switch (para.StorageType)
                            {
                                case StorageType.Double:
                                    if (!(type.AsDouble(para) is double))
                                        right = false;
                                    break;
                                case StorageType.ElementId:
                                    try
                                    {
                                        Autodesk.Revit.DB.ElementId elemId=type.AsElementId(para);
                                    }
                                    catch
                                    {
                                        right = false;
                                    }                                    
                                    break;
                                case StorageType.Integer:
                                    if (!(type.AsInteger(para) is int))
                                        right = false;
                                    break;
                                case StorageType.String:
                                    if (!(type.AsString(para) is string))
                                        right = false;
                                    break;
                                default:
                                    break;
                            }
                        }
                    } 
                    // output the parameters which failed during validating.
                    catch
                    {
                        errorInfo.Add("Family Type:" + type.Name + "   Family Parameter:" 
                            + para.Definition.Name + "   validating failed!");                       
                    }
                    if (!right)
                    {
                        errorInfo.Add("Family Type:" + type.Name + "   Family Parameter:"
                            + para.Definition.Name + "   validating failed!");                       
                    }                  
                } 
            }           
            return errorInfo;
        }       
        #endregion   
    }   
}
