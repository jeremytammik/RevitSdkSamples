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
using Autodesk.Revit;
using Autodesk.Revit.Creation;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Elements;
using Autodesk.Revit.MEP;
using Autodesk.Revit.MEP.Enums;
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.CreateAirHandler.CS
{
    /// <summary>
    /// Create one air handler and add connectors.
    /// </summary>
    public class Command : IExternalCommand
    {
        /// <summary>
        /// The revit application
        /// </summary>
        private static Autodesk.Revit.Application m_oApplication;

        /// <summary>
        /// The current document of the application
        /// </summary>
        private static Autodesk.Revit.Document m_oDocument;

        /// <summary>
        /// the factory to creaate extrusions and connectors
        /// </summary>
        private FamilyItemFactory m_oFamilyItemFactory;

        /// <summary>
        /// the extrusion array
        /// </summary>
        private Extrusion[] m_oExtrusions;

        /// <summary>
        /// The list of all the elements to be combined in the air handler system
        /// </summary>
        private CombinableElementArray m_oCombineElements;

        #region Data used to create extrusions and connectors
        /// <summary>
        /// Data to create extrusions and connectors
        /// </summary>
        private XYZ[,] profileData = new XYZ[5, 4]
        {
            // In Array 0 to 2, the data is the points that defines the edges of the profile
            {
                new XYZ(-17.28, -0.53, 0.9),
                new XYZ(-17.28, 11, 0.9),
                new XYZ(-0.57, 11, 0.9),
                new XYZ(-0.57, -0.53, 0.9)
            },
            {
                new XYZ(-0.57, 7, 6.58),
                new XYZ(-0.57, 7, 3),
                new XYZ(-0.57, 3.6, 3),
                new XYZ(-0.57, 3.6, 6.58)
            },
            {
                new XYZ(-17.28, -0.073, 7.17),
                new XYZ(-17.28, 10.76, 7.17),
                new XYZ(-17.28, 10.76, 3.58),
                new XYZ(-17.28, -0.073, 3.58)
            },
            // In Array 3 and 4, the data is the normal and origin of the plane of the arc profile
            {
                new XYZ(0, -1, 0),
                new XYZ(-9, 0.53, 7.17),
                null,
                null
            },
            {
                new XYZ(0, -1, 0),
                new XYZ(-8.24, 0.53, 0.67),
                null,
                null
            }
        };

        /// <summary>
        /// the normal and origin of the sketch plane
        /// </summary>
        private XYZ[,] sketchPlaneData = new XYZ[5, 2]
        {
            {new XYZ(0, 0, -1), new XYZ(0, 0, 0.9)},
            {new XYZ(1, 0, 0), new XYZ(-0.57, 0, 0)},
            {new XYZ(1, 0, 0), new XYZ(-17.28, 0, 0)},
            {new XYZ(0, -1, 0), new XYZ(0, 0.53, 0)},
            {new XYZ(0, -1, 0), new XYZ(0, 0.53, 0)}
        };

        /// <summary>
        /// the start and end offsets of the extrusion
        /// </summary>
        private double[,] extrusionOffsets = new double[5, 2]
        {
            {-0.9, 6.77},
            {0, 0.18},
            {0, 0.08},
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
            {3.5, 3.5},
            {3.58, 10.83}
        };

        /// <summary>
        /// the flow of the connector
        /// </summary>
        private double flow = 547;
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
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message,
            ElementSet elements)
        {
            // set out default result to failure.
            IExternalCommand.Result retRes = IExternalCommand.Result.Failed;
            m_oApplication = commandData.Application;
            m_oDocument = m_oApplication.ActiveDocument;
            m_oFamilyItemFactory = m_oDocument.FamilyCreate;
            m_oExtrusions = new Extrusion[5];
            m_oCombineElements = new CombinableElementArray();

            m_oDocument.BeginTransaction();

            if (m_oDocument.OwnerFamily.FamilyCategory.Name != "Mechanical Equipment")
            {
                message = "Please make sure you opened a template of Mechanical Equipment.";
                return retRes;
            }

            try
            {
                CreateExtrusions();
                CreateConnectors();
                m_oDocument.CombineElements(m_oCombineElements);
            }
            catch(Exception x)
            {
                m_oDocument.AbortTransaction();
                message = x.Message;
                return retRes;
            }

            m_oDocument.EndTransaction();

            retRes = IExternalCommand.Result.Succeeded;
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
            Options m_geoOptions = m_oApplication.Create.NewGeometryOptions();
            m_geoOptions.View = m_oDocument.ActiveView;
            m_geoOptions.ComputeReferences = true;

            // get the planar faces
            List<PlanarFace> m_planarFaces = new List<PlanarFace>();
            Autodesk.Revit.Geometry.Element geoElement = extrusion.get_Geometry(m_geoOptions);
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
            Autodesk.Revit.Creation.Application app = new Autodesk.Revit.Creation.Application();
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
                sketchPlane = m_oFamilyItemFactory.NewSketchPlane(plane);

                // create the extrusion
                m_oExtrusions[i] = m_oFamilyItemFactory.NewExtrusion(isSolid[i], profile, sketchPlane,
                    extrusionOffsets[i, 1]);
                m_oExtrusions[i].StartOffset = extrusionOffsets[i, 0];
                m_oCombineElements.Append(m_oExtrusions[i]);
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
                sketchPlane = m_oFamilyItemFactory.NewSketchPlane(plane);

                // create the extrusion
                m_oExtrusions[i] = m_oFamilyItemFactory.NewExtrusion(isSolid[i], profile, sketchPlane,
                    extrusionOffsets[i, 1]);
                m_oExtrusions[i].StartOffset = extrusionOffsets[i, 0];
                m_oCombineElements.Append(m_oExtrusions[i]);
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
            m_planarFaces = GetPlanarFaces(m_oExtrusions[1]);

            // create the Supply Air duct connector
            DuctConnector connSupplyAir = m_oFamilyItemFactory.NewDuctConnector(m_planarFaces[5].Reference,
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
            m_planarFaces = GetPlanarFaces(m_oExtrusions[2]);

            // create the Return Air duct connector
            DuctConnector connReturnAir 
              = m_oFamilyItemFactory.NewDuctConnector(
                m_planarFaces[0].Reference, DuctSystemType.ReturnAir );

            param = connReturnAir.get_Parameter(
              BuiltInParameter.CONNECTOR_HEIGHT );

            param.Set(connectorDimensions[1, 0]);

            // Must create a type definition for setting parameter values.
            m_oDocument.FamilyManager.NewType("Test Type");

            // Create family parameters for connector width (Common / Length) and height (HVAC / Duct Size)
            FamilyParameter oConnectorHeightAsLength 
              = m_oDocument.FamilyManager.AddParameter(
                "Return Air Connector Height As Length", 
                BuiltInParameterGroup.PG_GEOMETRY, 
                ParameterType.Length, false );

            //m_oDocument.FamilyManager.SetValueString( oConnectorHeightAsLength, "1'" );
            m_oDocument.FamilyManager.Set( oConnectorHeightAsLength, 1.0 / 12.0 );

            // This works just fine
            m_oDocument.FamilyManager.AssociateElementParameterToFamilyParameter(
              param, oConnectorHeightAsLength );

            //param = connReturnAir.get_Parameter( // returns null
            //  BuiltInParameter.CONNECTOR_SIZE );

            FamilyParameter oConnectorHeightAsDuctSize 
              = m_oDocument.FamilyManager.AddParameter(
                "Return Air Connector Height As DuctSize", 
                BuiltInParameterGroup.PG_GEOMETRY, 
                ParameterType.HVACDuctSize, false );

            //m_oDocument.FamilyManager.SetValueString( oConnectorHeightAsDuctSize, "1'" );
            m_oDocument.FamilyManager.Set( oConnectorHeightAsDuctSize, 1.0 / 12.0 );

            // This throws an exception "Cannot bind these two parameter due to mismatch parameter type."
            m_oDocument.FamilyManager.AssociateElementParameterToFamilyParameter(
              param, oConnectorHeightAsDuctSize );
            
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
            m_planarFaces = GetPlanarFaces(m_oExtrusions[3]);

            // create the Hydronic Supply pipe connector
            PipeConnector connSupplyHydronic = m_oFamilyItemFactory.NewPipeConnector(m_planarFaces[0].Reference,
                PipeSystemType.SupplyHydronic);
            param = connSupplyHydronic.get_Parameter(BuiltInParameter.CONNECTOR_RADIUS);
            param.Set(arcRadius);
            param =
                connSupplyHydronic.get_Parameter(BuiltInParameter.RBS_PIPE_FLOW_DIRECTION_PARAM);
            param.Set(2);

            #endregion

            #region Create the Return Hydronic pipe connector

            // get the planar faces of extrusion4
            m_planarFaces = GetPlanarFaces(m_oExtrusions[4]);

            // create the Hydronic Return pipe connector
            PipeConnector connReturnHydronic = m_oFamilyItemFactory.NewPipeConnector(m_planarFaces[0].Reference,
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
