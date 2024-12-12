//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using Autodesk.Revit;
using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

namespace Revit.SDK.Samples.CreateAirHandler.CS
{
    /// <summary>
    /// Create one air handler and add connectors.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// The revit application
        /// </summary>
        private static Autodesk.Revit.ApplicationServices.Application m_application;

        /// <summary>
        /// The current document of the application
        /// </summary>
        private static Autodesk.Revit.DB.Document m_document;

        /// <summary>
        /// the factory to creaate extrusions and connectors
        /// </summary>
        private FamilyItemFactory f;

        /// <summary>
        /// the extrusion array
        /// </summary>
        private Extrusion[] extrusions;

        /// <summary>
        /// The list of all the elements to be combined in the air handler system
        /// </summary>
        private CombinableElementArray m_combineElements;

        #region Data used to create extrusions and connectors
        /// <summary>
        /// Data to create extrusions and connectors
        /// </summary>
        private Autodesk.Revit.DB.XYZ [,] profileData = new Autodesk.Revit.DB.XYZ [5, 4]
        {
            // In Array 0 to 2, the data is the points that defines the edges of the profile
            {
                new Autodesk.Revit.DB.XYZ (-17.28, -0.53, 0.9),
                new Autodesk.Revit.DB.XYZ (-17.28, 11, 0.9),
                new Autodesk.Revit.DB.XYZ (-0.57, 11, 0.9),
                new Autodesk.Revit.DB.XYZ (-0.57, -0.53, 0.9)
            },
            {
                new Autodesk.Revit.DB.XYZ (-0.57, 7, 6.58),
                new Autodesk.Revit.DB.XYZ (-0.57, 7, 3),
                new Autodesk.Revit.DB.XYZ (-0.57, 3.6, 3),
                new Autodesk.Revit.DB.XYZ (-0.57, 3.6, 6.58)
            },
            {
                new Autodesk.Revit.DB.XYZ (-17.28, -0.073, 7.17),
                new Autodesk.Revit.DB.XYZ (-17.28, 10.76, 7.17),
                new Autodesk.Revit.DB.XYZ (-17.28, 10.76, 3.58),
                new Autodesk.Revit.DB.XYZ (-17.28, -0.073, 3.58)
            },
            // In Array 3 and 4, the data is the normal and origin of the plane of the arc profile
            {
                new Autodesk.Revit.DB.XYZ (0, -1, 0),
                new Autodesk.Revit.DB.XYZ (-9, 0.53, 7.17),
                null,
                null
            },
            {
                new Autodesk.Revit.DB.XYZ (0, -1, 0),
                new Autodesk.Revit.DB.XYZ (-8.24, 0.53, 0.67),
                null,
                null
            }
        };

        /// <summary>
        /// the normal and origin of the sketch plane
        /// </summary>
        private Autodesk.Revit.DB.XYZ [,] sketchPlaneData = new Autodesk.Revit.DB.XYZ [5, 2]
        {
            {new Autodesk.Revit.DB.XYZ (0, 0, 1), new Autodesk.Revit.DB.XYZ (0, 0, 0.9)},
            {new Autodesk.Revit.DB.XYZ (1, 0, 0), new Autodesk.Revit.DB.XYZ (-0.57, 0, 0)},
            {new Autodesk.Revit.DB.XYZ (-1, 0, 0), new Autodesk.Revit.DB.XYZ (-17.28, 0, 0)},
            {new Autodesk.Revit.DB.XYZ (0, -1, 0), new Autodesk.Revit.DB.XYZ (0, 0.53, 0)},
            {new Autodesk.Revit.DB.XYZ (0, -1, 0), new Autodesk.Revit.DB.XYZ (0, 0.53, 0)}
        };

        /// <summary>
        /// the start and end offsets of the extrusion
        /// </summary>
        private double[,] extrusionOffsets = new double[5, 2]
        {
            {-0.9, 6.77},
            {0, -0.18},
            {0, -0.08},
            {1, 1.15},
            {1, 1.15}
        };

        /// <summary>
        /// whether the extrusion is solid
        /// </summary>
        private bool[] isSolid = new bool[5] { true, false, false, true, true };

        /// <summary>
        /// the radius of the arc profile
        /// </summary>
        private double arcRadius = 0.17;

        /// <summary>
        /// the height and width of the connector
        /// </summary>
        private double[,] connectorDimensions = new double[2, 2]
        {
            {3.58, 3.4},
            {3.59, 10.833}
        };

        /// <summary>
        /// the flow of the connector
        /// </summary>
        private double flow = 547;

        /// <summary>
        /// Transaction of ExternalCommand
        /// </summary>
        private Transaction m_transaction;
        #endregion

        #region IExternalCommand Members
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
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message,
            ElementSet elements)
        {
            // set out default result to failure.
            Autodesk.Revit.UI.Result retRes = Autodesk.Revit.UI.Result.Failed;
            m_application = commandData.Application.Application;
            m_document = commandData.Application.ActiveUIDocument.Document;
            f = m_document.FamilyCreate;
            extrusions = new Extrusion[5];
            m_combineElements = new CombinableElementArray();

            m_transaction = new Transaction(m_document, "External Tool");
            m_transaction.Start();

            if (m_document.OwnerFamily.FamilyCategory.Name != "Mechanical Equipment")
            {
                message = "Please make sure you opened a template of Mechanical Equipment.";
                return retRes;
            }

            try
            {
                CreateExtrusions();
                m_document.Regenerate();
                CreateConnectors();
                m_document.Regenerate();
                m_document.CombineElements(m_combineElements);
                m_document.Regenerate();
            }
            catch(Exception x)
            {
                m_transaction.RollBack();
                message = x.Message;
                return retRes;
            }

            m_transaction.Commit();

            retRes = Autodesk.Revit.UI.Result.Succeeded;
            return retRes;
        }
        #endregion

        /// <summary>
        /// get all planar faces of an extrusion
        /// </summary>
        /// <param name="extrusion">the extrusion to read</param>
        /// <returns>a list of all planar faces of the extrusion</returns>
        public List<PlanarFace> GetPlanarFaces(Extrusion extrusion)
        {
            // the option to get geometry elements
            Options m_geoOptions = m_application.Create.NewGeometryOptions();
            m_geoOptions.View = m_document.ActiveView;
            m_geoOptions.ComputeReferences = true;

            // get the planar faces
            List<PlanarFace> m_planarFaces = new List<PlanarFace>();
            Autodesk.Revit.DB.GeometryElement geoElement = extrusion.get_Geometry(m_geoOptions);
            foreach (GeometryObject geoObject in geoElement.Objects)
            {
                Solid geoSolid = geoObject as Solid;
                if (null == geoSolid)
                {
                    continue;
                }
                foreach (Face geoFace in geoSolid.Faces)
                {
                    if (geoFace is PlanarFace)
                    {
                        m_planarFaces.Add(geoFace as PlanarFace);
                    }
                }
            }
            return m_planarFaces;
        }

        /// <summary>
        /// create the extrusions of the air handler system
        /// </summary>
        private void CreateExtrusions()
        {
            Autodesk.Revit.Creation.Application app = m_application.Create;
            CurveArray curves = null;
            CurveArrArray profile = null;
            Plane plane = null;
            SketchPlane sketchPlane = null;

            #region Create the cuboid extrusions

            for (int i = 0; i <= 2; ++i)
            {
                // create the profile
                curves = app.NewCurveArray();
                curves.Append(app.NewLine(profileData[i, 0], profileData[i, 1], true));
                curves.Append(app.NewLine(profileData[i, 1], profileData[i, 2], true));
                curves.Append(app.NewLine(profileData[i, 2], profileData[i, 3], true));
                curves.Append(app.NewLine(profileData[i, 3], profileData[i, 0], true));
                profile = app.NewCurveArrArray();
                profile.Append(curves);

                // create the sketch plane
                plane = app.NewPlane(sketchPlaneData[i, 0], sketchPlaneData[i, 1]);
                sketchPlane = f.NewSketchPlane(plane);

                // create the extrusion
                extrusions[i] = f.NewExtrusion(isSolid[i], profile, sketchPlane,
                    extrusionOffsets[i, 1]);
                extrusions[i].StartOffset = extrusionOffsets[i, 0];
                m_combineElements.Append(extrusions[i]);
            }

            #endregion

            #region Create the round extrusions

            for (int i = 3; i <= 4; ++i)
            {
                // create the profile
                profile = app.NewCurveArrArray();

                curves = app.NewCurveArray();
                plane = new Plane(profileData[i, 0], profileData[i, 1]);
                curves.Append(app.NewArc(plane, arcRadius, 0, Math.PI * 2));
                profile.Append(curves);

                // create the sketch plane
                plane = app.NewPlane(sketchPlaneData[i, 0], sketchPlaneData[i, 1]);
                sketchPlane = f.NewSketchPlane(plane);

                // create the extrusion
                extrusions[i] = f.NewExtrusion(isSolid[i], profile, sketchPlane,
                    extrusionOffsets[i, 1]);
                extrusions[i].StartOffset = extrusionOffsets[i, 0];
                m_combineElements.Append(extrusions[i]);
            }

            #endregion
        }

        /// <summary>
        /// create the connectors on the extrusions
        /// </summary>
        private void CreateConnectors()
        {
            List<PlanarFace> m_planarFaces = null;
            Parameter param = null;

            #region Create the Supply Air duct connector

            // get the planar faces of extrusion1
            m_planarFaces = GetPlanarFaces(extrusions[1]);

            // create the Supply Air duct connector
            DuctConnector connSupplyAir = f.NewDuctConnector(m_planarFaces[0].Reference,
                DuctSystemType.SupplyAir);
            param = connSupplyAir.get_Parameter(BuiltInParameter.CONNECTOR_HEIGHT);
            param.Set(connectorDimensions[0, 0]);
            param = connSupplyAir.get_Parameter(BuiltInParameter.CONNECTOR_WIDTH);
            param.Set(connectorDimensions[0, 1]);
            param = connSupplyAir.get_Parameter(BuiltInParameter.RBS_DUCT_FLOW_DIRECTION_PARAM);
            param.Set(2);
            param = connSupplyAir.get_Parameter(BuiltInParameter.RBS_DUCT_FLOW_CONFIGURATION_PARAM);
            param.Set(1);
            param = connSupplyAir.get_Parameter(BuiltInParameter.RBS_DUCT_FLOW_PARAM);
            param.Set(flow);

            #endregion

            #region Create the Return Air duct connector

            // get the planar faces of extrusion2
            m_planarFaces = GetPlanarFaces(extrusions[2]);

            // create the Return Air duct connector
            DuctConnector connReturnAir = f.NewDuctConnector(m_planarFaces[0].Reference,
                DuctSystemType.ReturnAir);
            param = connReturnAir.get_Parameter(BuiltInParameter.CONNECTOR_HEIGHT);
            param.Set(connectorDimensions[1, 0]);
            param = connReturnAir.get_Parameter(BuiltInParameter.CONNECTOR_WIDTH);
            param.Set(connectorDimensions[1, 1]);
            param = connReturnAir.get_Parameter(BuiltInParameter.RBS_DUCT_FLOW_DIRECTION_PARAM);
            param.Set(1);
            param =
                connReturnAir.get_Parameter(BuiltInParameter.RBS_DUCT_FLOW_CONFIGURATION_PARAM);
            param.Set(1);
            param = connReturnAir.get_Parameter(BuiltInParameter.RBS_DUCT_FLOW_PARAM);
            param.Set(flow);

            #endregion

            #region Create the Supply Hydronic pipe connector

            // get the planar faces of extrusion3
            m_planarFaces = GetPlanarFaces(extrusions[3]);

            // create the Hydronic Supply pipe connector
            PipeConnector connSupplyHydronic = f.NewPipeConnector(m_planarFaces[0].Reference,
                PipeSystemType.SupplyHydronic);
            param = connSupplyHydronic.get_Parameter(BuiltInParameter.CONNECTOR_RADIUS);
            param.Set(arcRadius);
            param =
                connSupplyHydronic.get_Parameter(BuiltInParameter.RBS_PIPE_FLOW_DIRECTION_PARAM);
            param.Set(2);

            #endregion

            #region Create the Return Hydronic pipe connector

            // get the planar faces of extrusion4
            m_planarFaces = GetPlanarFaces(extrusions[4]);

            // create the Hydronic Return pipe connector
            PipeConnector connReturnHydronic = f.NewPipeConnector(m_planarFaces[0].Reference,
                PipeSystemType.ReturnHydronic);
            param = connReturnHydronic.get_Parameter(BuiltInParameter.CONNECTOR_RADIUS);
            param.Set(arcRadius);
            param =
                connReturnHydronic.get_Parameter(BuiltInParameter.RBS_PIPE_FLOW_DIRECTION_PARAM);
            param.Set(1);

            #endregion

        }
    }
}
