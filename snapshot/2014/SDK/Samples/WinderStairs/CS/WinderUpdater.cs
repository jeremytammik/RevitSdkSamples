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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;

namespace Revit.SDK.Samples.WinderStairs.CS
{
    /// <summary>
    /// This class implements IUpdater and IExternalEventHandler interfaces. It's mainly
    /// for winder stairs creation and regeneration when its dependencies are being changed.
    /// </summary>
    class WinderUpdater : IUpdater, IExternalEventHandler
    {
        private UpdaterId m_updaterId = null;
        private IList<ElementId> m_curveElements = null;
        private ElementId m_winderRunId = ElementId.InvalidElementId;
        private Winder m_winder = null;
        private bool m_removeTheUpdater = false;
        private bool m_drawSketch = false;

        /// <summary>
        /// Constructor to initialize the fields and create the winder stairs.
        /// It also registers the updater if addinId is not null.
        /// </summary>
        /// <param name="winder">Winder to create winder stairs</param>
        /// <param name="crvElements">Curve Elements to control the winder shape</param>
        /// <param name="rvtDoc">Revit Document</param>
        /// <param name="addinId">Active AddInId</param>
        /// <param name="drawSketch">Switch to control the winder sketch drawing</param>
        public WinderUpdater(Winder winder, IList<ElementId> crvElements,
            Document rvtDoc, AddInId addinId, bool drawSketch)
        {
            m_curveElements = crvElements;
            m_winder = winder;
            m_drawSketch = drawSketch;

            // Create the winder stairs
            GenerateWinderStairs(rvtDoc);

            // Register the updater if addinId is not null.
            if (addinId != null)
            {
                // Register the updater
                m_updaterId = new UpdaterId(addinId, Guid.NewGuid());
                UpdaterRegistry.RegisterUpdater(this, rvtDoc);

                // Add modification triggers
                UpdaterRegistry.AddTrigger(m_updaterId, rvtDoc, 
                    m_curveElements, Element.GetChangeTypeAny());
                
                // Add deletion trigger
                List<ElementId> deleteParents = new List<ElementId>();
                deleteParents.AddRange(crvElements);
                var stairRun = rvtDoc.GetElement(m_winderRunId) as StairsRun;
                deleteParents.Add(stairRun.GetStairs().Id);
                UpdaterRegistry.AddTrigger(m_updaterId, rvtDoc,
                    deleteParents, Element.GetChangeTypeElementDeletion());
            }
        }

        /// <summary>
        /// Create the winder stairs in the Document.
        /// </summary>
        /// <param name="rvtDoc">Revit Document.</param>
        private void GenerateWinderStairs(Document rvtDoc)
        {
            if (m_removeTheUpdater)
            {
                // unregister this updater
                UpdaterRegistry.UnregisterUpdater(m_updaterId, rvtDoc);
                return;
            }

            // Create the winder
            m_winder.ControlPoints = WinderUtil.CalculateControlPoints(rvtDoc, m_curveElements);
            m_winder.Build(rvtDoc, m_drawSketch, ref m_winderRunId);
        }

        /// <summary>
        /// Implementation of IUpdater.Execute method.
        /// </summary>
        void IUpdater.Execute(UpdaterData data)
        {
            // If there is any deleted elements, this updater need to be unregistered.
            if (data.GetDeletedElementIds().Count != 0)
            {
                // set the unregistered flag, 
                // because it's not allowed to remove the executing updater.
                m_removeTheUpdater = true;
            }
            // Raise the External Event
            ExternalEvent exEvent = ExternalEvent.Create(this);
            exEvent.Raise();
        }

        /// <summary>
        /// Implementation of IUpdater.GetAdditionalInformation method.
        /// </summary>
        string IUpdater.GetAdditionalInformation()
        {
            return "API SKETCHED WINDER";
        }

        /// <summary>
        /// Implementation of IUpdater.GetChangePriority method.
        /// </summary>
        ChangePriority IUpdater.GetChangePriority()
        {
            return ChangePriority.GridsLevelsReferencePlanes;
        }

        /// <summary>
        /// Implementation of IUpdater.GetUpdaterId method.
        /// </summary>
        UpdaterId IUpdater.GetUpdaterId()
        {
            return m_updaterId;
        }

        /// <summary>
        /// Implementation of IUpdater.GetUpdaterName method.
        /// </summary>
        string IUpdater.GetUpdaterName()
        {
            return "Revit.SDK.Samples.WinderStairs";
        }

        /// <summary>
        /// Implementation of IExternalEventHandler.Execute method.
        /// </summary>
        void IExternalEventHandler.Execute(UIApplication app)
        {
            try
            {
                GenerateWinderStairs(app.ActiveUIDocument.Document);
            }
            catch (System.Exception ex)
            {
                TaskDialog.Show("WinderStairs", 
                    "Can't generate the winder layout because " + ex.Message);
            }
        }

        /// <summary>
        /// Implementation of IExternalEventHandler.GetName method.
        /// </summary>
        string IExternalEventHandler.GetName()
        {
            return "Revit.SDK.Samples.WinderStairs";
        }
    }
}
