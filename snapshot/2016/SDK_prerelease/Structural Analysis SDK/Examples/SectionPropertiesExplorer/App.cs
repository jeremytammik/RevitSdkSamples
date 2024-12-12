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
using System.Collections.Generic;
using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace SectionPropertiesExplorer
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    public class Command : Autodesk.Revit.UI.IExternalCommand
    {
        #region IExternalCommand implementation
        //--------------------------------------------------------------

        public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            this.commandData = commandData;
            List<Autodesk.Revit.DB.Element> revitElements = new List<Autodesk.Revit.DB.Element>();
            ICollection<Autodesk.Revit.DB.ElementId> revitElementIds = commandData.Application.ActiveUIDocument.Selection.GetElementIds();

            foreach (Autodesk.Revit.DB.ElementId elId in revitElementIds)
            {
                Autodesk.Revit.DB.Element el = commandData.Application.ActiveUIDocument.Document.GetElement(elId);

                Autodesk.Revit.DB.BuiltInCategory cat = (Autodesk.Revit.DB.BuiltInCategory)el.Category.Id.IntegerValue;

                switch (cat)
                {
                    case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralColumns:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFraming:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Floors:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFoundation:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_Walls:
                    case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                        revitElements.Add(el);
                        break;
                    default:
                        break;

                }
            }

            if (revitElements.Count < 1)
            {
                MessageBox.Show("No structural columns, structural framings, slabs or no walls are selected.");
                return Autodesk.Revit.UI.Result.Cancelled;
            }

            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            uiDoc.Selection.GetElementIds().Clear();

            MainWindow mainWindow = new MainWindow(revitElements, On_ElementSelected);
            mainWindow.ShowDialog();

            List<ElementId> elIds = new List<ElementId>();
            foreach (Autodesk.Revit.DB.Element el in revitElements)
            {
                elIds.Add(el.Id);
            }
            uiDoc.Selection.SetElementIds(elIds);
            uiDoc.RefreshActiveView();

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        //--------------------------------------------------------------
        #endregion

        #region event handlers
        //--------------------------------------------------------------

        private void On_ElementSelected(object sender, MainWindow.ElementSelectedEventArgs e)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            uiDoc.Selection.GetElementIds().Clear();
            List<ElementId> elIds = new List<ElementId>();
            elIds.Add(e.SelectedElement.Id);
            uiDoc.Selection.SetElementIds(elIds);
        
            uiDoc.RefreshActiveView();
        }

        //--------------------------------------------------------------
        #endregion

        #region private members
        //--------------------------------------------------------------

        private Autodesk.Revit.UI.ExternalCommandData commandData;

        //--------------------------------------------------------------
        #endregion
    }
}
