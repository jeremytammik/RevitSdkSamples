//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Collections;
using System.Text;
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.CreateWallsUnderBeams.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CreateWallsUnderBeams : IExternalCommand
    {
        // Private Members
        ArrayList m_wallTypeCollection;         // Store all the wall types in current document
        ArrayList m_beamCollection;             // Store the selection of beams in Revit
        WallType m_selectedWallType;            // Store the selected wall type
        Level m_level;                          // Store the level which wall create on
        Boolean m_isStructural;                 // Indicate whether create structural walls
        String m_errorInformation;              // Store the error information
        const Double PRECISION = 0.0000000001;  // Define a precision of double data

        // Properties
        /// <summary>
        /// Inform all the wall types can be created in current document
        /// </summary>
        public ArrayList WallTypes
        {
            get
            {
                return m_wallTypeCollection;
            }
        }

        /// <summary>
        /// Inform the wall type selected by the user
        /// </summary>
        public Object SelectedWallType
        {
            set
            {
                m_selectedWallType = value as WallType;
            }
        }

        /// <summary>
        /// Inform whether the user want to create structual or architecture walls
        /// </summary>
        public Boolean IsSturctual
        {
            get
            {
                return m_isStructural;
            }
            set
            {
                m_isStructural = value;
            }
        }

        // Methods
        /// <summary>
        /// Default constructor of CreateWallsUnderBeams
        /// </summary>
        public CreateWallsUnderBeams()
        {
            m_wallTypeCollection = new ArrayList();
            m_beamCollection = new ArrayList();
            m_isStructural = true;
        }

        #region IExternalCommand Members Implementation
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
                                                    ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Autodesk.Revit.UI.UIApplication revit = commandData.Application;
            UIDocument project = revit.ActiveUIDocument;

            // Find the selection of beams in Revit
            ElementSet selection = project.Selection.Elements;
            foreach (Autodesk.Revit.DB.Element e in selection)
            {
                FamilyInstance m = e as FamilyInstance;
                if (null != m)
                {
                    if (StructuralType.Beam == m.StructuralType)
                    {
                        // Store all the beams the user selected in Revit
                        m_beamCollection.Add(e);
                    }
                }
            }
            if (0 == m_beamCollection.Count)
            {
                message = "Can not find any beams.";
                return Autodesk.Revit.UI.Result.Failed;
            }

            // Make sure all the beams have horizontal analytical line
            if (!CheckBeamHorizontal())
            {
                message = m_errorInformation;
                return Autodesk.Revit.UI.Result.Failed;
            }

            // Search all the wall types in the Revit
            foreach (WallType wallType in project.Document.WallTypes)
            {
                if (null == wallType)
                {
                    continue;
                }
                m_wallTypeCollection.Add(wallType);
            }

            // Show the dialog for the user select the wall style
            using(CreateWallsUnderBeamsForm displayForm = new CreateWallsUnderBeamsForm(this))
            {
                if (DialogResult.OK != displayForm.ShowDialog())
                {
                    return Autodesk.Revit.UI.Result.Failed;
                }
            }

            // Create the walls which along and under the path of the beams.
            if (!BeginCreate(project.Document))
            {
                message = m_errorInformation;
                return Autodesk.Revit.UI.Result.Failed;
            }

            // If everything goes right, return succeeded.
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation

        /// <summary>
        /// Create the walls which along and under the path of the selected beams
        /// </summary>
        /// <param name="project"> A reference of current document</param>
        /// <returns>true if there is no error in process; otherwise, false.</returns>
        Boolean BeginCreate(Autodesk.Revit.DB.Document project)
        {
            // Begin to create walls along and under each beam
            for (int i = 0; i < m_beamCollection.Count; i++)
            {
                // Get each selected beam.
                FamilyInstance m = m_beamCollection[i] as FamilyInstance;
                if (null == m)
                {
                    m_errorInformation = "The program should not go here.";
                    return false;
                }

                // Get the analytical model of the beam, 
                // the wall will be created using this model line as path.            
                AnalyticalModel model = m.GetAnalyticalModel();
                if (null == model)
                {
                    m_errorInformation = "The beam should have analytical model.";
                    return false;
                }

                // Get the level using the beam's reference level
                Autodesk.Revit.DB.ElementId levelId = m.get_Parameter(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM).AsElementId();
                m_level = project.get_Element(levelId) as Level;
                if (null == m_level)
                {
                    m_errorInformation = "The program should not go here.";
                    return false;
                }

                Transaction t = new Transaction(project, Guid.NewGuid().GetHashCode().ToString());
                t.Start();
                Wall createdWall = project.Create.NewWall(model.GetCurve(), m_selectedWallType,
                                                m_level, 10, 0, true, m_isStructural);
                if (null == createdWall)
                {
                    m_errorInformation = "Can not create the walls";
                    return false;
                }

                // Modify some parameters of the created wall to make it look better.
                Double offset = model.GetCurve().get_EndPoint(0).Z - m_level.Elevation;
                createdWall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).Set(levelId);
                createdWall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).Set(offset - 3000 / 304.8);
                createdWall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(levelId);
                t.Commit();
            }
            return true;
        }


        /// <summary>
        /// Check whether all the beams have horizontal analytical line 
        /// </summary>
        /// <returns>true if each beam has horizontal analytical line; otherwise, false.</returns>
        Boolean CheckBeamHorizontal()
        {
            for (int i = 0; i < m_beamCollection.Count; i++)
            {
                // Get the analytical curve of each selected beam.
                // And check if Z coordinate of start point and end point of the curve are same.
                FamilyInstance m = m_beamCollection[i] as FamilyInstance;
                AnalyticalModel model = m.GetAnalyticalModel();
                if (null == model)
                {
                    m_errorInformation = "The beam should have analytical model.";
                    return false;
                }
                else if ((PRECISION <= model.GetCurve().get_EndPoint(0).Z - model.GetCurve().get_EndPoint(1).Z)
                    || (-PRECISION >= model.GetCurve().get_EndPoint(0).Z - model.GetCurve().get_EndPoint(1).Z))
                {
                    m_errorInformation = "Please only select horizontal beams.";
                    return false;
                }
            }
            return true;
        }
    }
}
