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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using MacroSamples_RFA;

namespace Revit.SDK.Samples.ValidateParameters.CS
{  
    /// <summary>
    /// this class controls the class which subscribes handle events and the events' information UI.
    /// like a bridge between them.
    /// </summary>
    public class ValidateParameters
    {
        #region Class Memeber Variables
        /// <summary>
        /// store the family manager
        /// </summary>
        private FamilyManager? m_familyManager;
        private Autodesk.Revit.ApplicationServices.Application? m_revit = null;
        private ThisApplication m_thisApp;
        #endregion

        public ValidateParameters(ThisApplication thisApp)
        {
            m_thisApp = thisApp;
            m_revit = thisApp.ActiveUIDocument.Document.Application;
        }
        
        #region Class Interface Implementation
        
        /// <summary>
        /// Run
        /// </summary>
        public void Run()
        {
            Document document = m_thisApp.ActiveUIDocument.Document;           
            // only a family document can retrieve family manager
            if (document.IsFamilyDocument)
            {                
                m_familyManager = document.FamilyManager;
                List<string> errorMessages = Validate(m_familyManager);
                using (MessageForm msgForm = new MessageForm(errorMessages.ToArray(), m_thisApp))
                {
                    msgForm.StartPosition = FormStartPosition.CenterParent;
                    msgForm.ShowDialog();
                    return;
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Current document is not family document.");
                return;
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
        public static List<string> Validate(FamilyManager familyManager)
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
                                        ElementId elemId=type.AsElementId(para);
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
