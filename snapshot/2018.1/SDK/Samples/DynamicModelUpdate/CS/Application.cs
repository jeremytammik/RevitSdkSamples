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
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace Revit.SDK.Samples.DynamicModelUpdate.CS
{

    ///////////////////////////////////////////////////////////////////////////////////////////////
    //
    // Command to setup the updater, register the triggers (on execute), and unregister it (on close the document)
    //

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class AssociativeSectionUpdater : Autodesk.Revit.UI.IExternalCommand
    {
        Document m_document;
        UIDocument m_documentUI;

        // application's private data
        private static SectionUpdater m_sectionUpdater = null;
        private AddInId m_thisAppId;

        private static List<ElementId> idsToWatch = new List<ElementId>();
        private static ElementId m_oldSectionId = ElementId.InvalidElementId;

        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                m_document = commandData.Application.ActiveUIDocument.Document;
                m_documentUI = commandData.Application.ActiveUIDocument;
                m_thisAppId = commandData.Application.ActiveAddInId;


                // creating and registering the updater for the document.
                if (m_sectionUpdater == null)
                {
                    using (Transaction tran = new Transaction(m_document, "Register Section Updater"))
                    {
                        tran.Start();

                        m_sectionUpdater = new SectionUpdater(m_thisAppId);
                        m_sectionUpdater.Register(m_document);

                        tran.Commit();
                    }
                }

                TaskDialog.Show("Message", "Please select a section view, then select a window.");

                ElementId modelId = null;
                Element sectionElement = null;
                ElementId sectionId = null;
                try
                {
                    Reference referSection = m_documentUI.Selection.PickObject(ObjectType.Element, "Please select a section view.");
                    if (referSection != null)
                    {
                        Element sectionElem = m_document.GetElement(referSection);
                        if (sectionElem != null)
                        {
                            sectionElement = sectionElem;
                        }
                    }
                    Reference referModel = m_documentUI.Selection.PickObject(ObjectType.Element, "Please select a window to associated with the section view.");
                    if (referModel != null)
                    {
                        Element model = m_document.GetElement(referModel);
                        if (model != null)
                        {
                            if (model is FamilyInstance)
                                modelId = model.Id;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    TaskDialog.Show("Message", "The selection has been canceled.");
                    return Result.Cancelled;
                }

                if (modelId == null)
                {
                    TaskDialog.Show("Error", "The model is supposed to be a window.\n The operation will be canceled.");
                    return Result.Cancelled;
                }

                // Find the real ViewSection for the selected section element.
                string name = sectionElement.Name;
                FilteredElementCollector collector = new FilteredElementCollector(m_document);
                collector.WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Views));
                var viewElements = from element in collector
                                   where element.Name == name
                                   select element;

                List<Autodesk.Revit.DB.Element> sectionViews = viewElements.ToList<Autodesk.Revit.DB.Element>();
                if (sectionViews.Count == 0)
                {
                    TaskDialog.Show("Message", "Cannot find the view name " + name + "\n The operation will be canceled.");
                    return Result.Failed;
                }
                sectionId = sectionViews[0].Id;

                // Associated the section view to the window, and add a trigger for it.
                if (!idsToWatch.Contains(modelId) || m_oldSectionId != sectionId)
                {
                    idsToWatch.Clear();
                    idsToWatch.Add(modelId);
                    m_oldSectionId = sectionId;
                    UpdaterRegistry.RemoveAllTriggers(m_sectionUpdater.GetUpdaterId());
                    m_sectionUpdater.AddTriggerForUpdater(m_document, idsToWatch, sectionId, sectionElement);
                    TaskDialog.Show("Message", "The ViewSection id: " + sectionId + " has been associated to the window id: " + modelId + "\n You can try to move or modify the window to see how the updater works.");
                }
                else
                {
                    TaskDialog.Show("Message", "The model has been already associated to the ViewSection.");
                }

                m_document.DocumentClosing += UnregisterSectionUpdaterOnClose;

                return Result.Succeeded;
            }
            catch (System.Exception ex)
            {
                message = ex.ToString();
                return Result.Failed;
            }
        }

        /// <summary>
        /// Unregister the updater on Revit document close.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="args">The DocumentClosing event args.</param>
        private void UnregisterSectionUpdaterOnClose(object source, DocumentClosingEventArgs args)
        {
            idsToWatch.Clear();
            m_oldSectionId = ElementId.InvalidElementId;

            if (m_sectionUpdater != null)
            {
                UpdaterRegistry.UnregisterUpdater(m_sectionUpdater.GetUpdaterId());
                m_sectionUpdater = null;
            }
        }

    }

}
