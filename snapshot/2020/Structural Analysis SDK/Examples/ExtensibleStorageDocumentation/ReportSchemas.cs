//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Collections;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage.Framework;
using Autodesk.Revit.UI.ExtensibleStorage.Framework;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Attributes;
using Document = Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Document;

namespace ExtensibleStorageDocumentation
{

    /// <summary>
    /// This class is an helper class to provide an overview of the documentation features using schemas
    /// </summary>
    internal class ReportSchemas : IServerUI
    {

        private readonly Autodesk.Revit.DB.Document docRevit;
        private readonly Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Document docReport;
  
        /// <summary>
        ///  /// <summary>
        /// Creates a ReportSchemas class instance.
        /// </summary>
        /// <param name="activeDocRevit"></param>
        /// <param name="activeDocReport"></param>
        public ReportSchemas(Autodesk.Revit.DB.Document activeDocRevit, Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation.Document  activeDocReport)
        {
            this.docRevit = activeDocRevit;
            this.docReport = activeDocReport;
        }

        #region IServerUI Members

        public IList GetDataSource(string key, Autodesk.Revit.DB.Document document, DisplayUnitType unitType)
        {
            switch (key)
            {
             
                default:
                    return null;
            }
        }

        public void LayoutInitialized(object sender, LayoutInitializedEventArgs e)
        {
        }

        public void ValueChanged(object sender, ValueChangedEventArgs e)
        {
        }

        public string GetResource(string key, string contex)
        {
            return key;
        }

        public Uri GetResourceImage(string key, string contex)
        {
            return null;
        }

        #endregion

        public void CreateReportSchema()
        {
            var server = new ServerUI();
            // new schema base on default constructor
            var schema = new Schema();
            // Add schema to  the main report 
            DocumentBody.FillBody(schema, this.docReport.Main, server, this.docRevit);
        }

        /// <summary>
        /// Method which returns a list of all elements of a specific type from the active document
        /// </summary>
        /// <param name="type"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        private IList GetAllElementsOfType(Type type, Autodesk.Revit.DB.Document document)
        {
            return new FilteredElementCollector(document).OfClass(type).ToElements().ToList();
        }
        /// <summary>
        /// Method which returns a list of all elements of a specific category from the active document 
        /// </summary>
        /// <param name="category"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        private IList GetAllElementsOfCategory(BuiltInCategory category, Autodesk.Revit.DB.Document document)
        {
            return new FilteredElementCollector(document).OfCategory(category).ToElementIds().ToList();
        }

       
    }
}