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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Fabrication;
using System.Linq;

namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class FabricationPartLayout : IExternalCommand
   {
      Document m_doc { get; set; }
      IList<FabricationService> m_services { get; set; }

      // kept in sync
      IList<int> m_materialIds { get; set; }
      IList<string> m_materialGroups { get; set; }
      IList<string> m_materialNames { get; set; }

      // kept in sync
      IList<int> m_specIds { get; set; }
      IList<string> m_specGroups { get; set; }
      IList<string> m_specNames { get; set; }

      // kept in sync
      IList<int> m_insSpecIds { get; set; }
      IList<string> m_insSpecGroups { get; set; }
      IList<string> m_insSpecNames { get; set; }

      // kept in sync
      IList<int> m_connIds { get; set; }
      IList<string> m_connGroups { get; set; }
      IList<string> m_connNames { get; set; }

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
      public virtual Result Execute(ExternalCommandData commandData
          , ref string message, ElementSet elements)
      {
         try
         {
            m_doc = commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector cl = new FilteredElementCollector(m_doc);
            cl.OfClass(typeof(Level));
            IList<Element> levels = cl.ToElements();
            Level levelOne = null;
            foreach (Level level in levels)
            {
               if (level != null && level.Name.Equals("Level 1"))
               {
                  levelOne = level;
                  break;
               }
            }

            if (levelOne == null)
               return Result.Failed;

            // locate the AHU in the model - should only be one instance in the model.
            FilteredElementCollector c2 = new FilteredElementCollector(m_doc);
            c2.OfClass(typeof(FamilyInstance));
            IList<Element> families = c2.ToElements();
            if (families.Count != 1)
               return Result.Failed;

            FamilyInstance fam_ahu = families[0] as FamilyInstance;
            if (fam_ahu == null)
               return Result.Failed;

            // locate the proper connector - rectangular 40"x40" outlet
            Connector conn_ahu = null;
            ConnectorSet conns_ahu = fam_ahu.MEPModel.ConnectorManager.UnusedConnectors;
            double lengthInFeet = 40.0 / 12.0;
            foreach (Connector conn in conns_ahu)
            {
               // Revit units measured in feet, so dividing the width and height by 12

               if (conn.Shape == ConnectorProfileType.Rectangular && conn.Width == lengthInFeet && conn.Height == lengthInFeet)
                  conn_ahu = conn;
            }

            if (conn_ahu == null)
               return Result.Failed;

            // get the current fabrication configuration
            FabricationConfiguration config = FabricationConfiguration.GetFabricationConfiguration(m_doc);
            if (config == null)
               return Result.Failed;

            // create materials look-up tables
            GetMaterials(config);

            // create specs look-up tables
            GetSpecs(config);

            // create insulation specs look-up tables
            GetInsulationSpecs(config);

            // create fabrication configuration look-up tables
            GetFabricationConnectors(config);

            // get all the loaded services
            m_services = config.GetAllLoadedServices();
            if (m_services.Count == 0)
               return Result.Failed;

            FabricationService havcService = m_services.FirstOrDefault(x => x.Name.Contains("HVAC"));
            FabricationService pipeService = m_services.FirstOrDefault(x => x.Name.Contains("Plumbing"));

            FabricationServiceButton bt_transition = locateButton(havcService, 0, "Transition");
            FabricationServiceButton bt_sqBend = locateButton(havcService, 0, "Square Bend");
            FabricationServiceButton bt_tap = locateButton(havcService, 0, "Tap");
            FabricationServiceButton bt_rectStraight = locateButton(havcService, 0, "Straight");
            FabricationServiceButton bt_radBend = locateButton(havcService, 0, "Radius Bend");
            FabricationServiceButton bt_flatShoe = locateButton(havcService, 1, "Flat Shoe");
            FabricationServiceButton bt_tube = locateButton(havcService, 1, "Tube");
            FabricationServiceButton bt_90bend = locateButton(havcService, 1, "Bend - 90");
            FabricationServiceButton bt_45bend = locateButton(havcService, 1, "Bend - 45");
            FabricationServiceButton bt_rectTee = locateButton(havcService, 0, "Tee");
            FabricationServiceButton bt_sqToRound = locateButton(havcService, 0, "Square to Round");
            FabricationServiceButton bt_reducer = locateButton(havcService, 1, "Reducer - C");
            FabricationServiceButton bt_curvedBoot = locateButton(havcService, 1, "Curved Boot");
            FabricationServiceButton bt_hangerBearer = locateButton(havcService, 4, "Rectangular Bearer");
            FabricationServiceButton bt_hangerRound = locateButton(havcService, 4, "Round Duct Hanger");

            FabricationServiceButton bt_valve = locateButton(pipeService, 2, "Globe Valve");
            FabricationServiceButton bt_groovedPipe = locateButton(pipeService, 1, "Type L Hard Copper");
            FabricationServiceButton bt_90elbow = locateButton(pipeService, 1, "No610 - 90 Elbow");

            using (Transaction tr = new Transaction(m_doc, "Create Layout"))
            {
               tr.Start();

               // connect a square bend to the ahu
               FabricationPart pt_sqBend1 = FabricationPart.Create(m_doc, bt_sqBend, 0, levelOne.Id);
               Connector conn1_sqBend1 = GetPrimaryConnector(pt_sqBend1.ConnectorManager);
               Connector conn2_sqBend1 = GetSecondaryConnector(pt_sqBend1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_sqBend1, conn_ahu, 3.0 * Math.PI / 2.0);

               // add a 15' straight to the square bend
               FabricationPart pt_rectStraight1 = CreateStraightPart(bt_rectStraight, 0, levelOne.Id, 15.0);
               Connector conn1_straight1 = GetPrimaryConnector(pt_rectStraight1.ConnectorManager);
               Connector conn2_straight1 = GetSecondaryConnector(pt_rectStraight1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_straight1, conn2_sqBend1, 0);

               // add two Rectangular Bearer hangers at 5' to each end of the 15' straight
               FabricationPart.CreateHanger(m_doc, bt_hangerBearer, pt_rectStraight1.Id, conn1_straight1, 5.0, true);
               FabricationPart.CreateHanger(m_doc, bt_hangerBearer, pt_rectStraight1.Id, conn2_straight1, 5.0, true);

               // connect a tap to the straight half way along
               FabricationPart pt_tap1 = FabricationPart.Create(m_doc, bt_tap, 0, levelOne.Id);
               Connector conn1_tap1 = GetPrimaryConnector(pt_tap1.ConnectorManager);
               Connector conn2_tap1 = GetSecondaryConnector(pt_tap1.ConnectorManager);
               FabricationPart.PlaceAsTap(m_doc, conn1_tap1, conn1_straight1, 7.5, 3.0 * Math.PI / 2.0, 0);

               // connect a square to round to the tap, with an outlet of 10"
               FabricationPart pt_sqToRound1 = FabricationPart.Create(m_doc, bt_sqToRound, 0, levelOne.Id);
               Connector conn1_sqToRound1 = GetPrimaryConnector(pt_sqToRound1.ConnectorManager);
               Connector conn2_sqToRound1 = GetSecondaryConnector(pt_sqToRound1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_sqToRound1, conn2_tap1, 0);
               conn2_sqToRound1.Radius = 5.0 / 12.0; // convert to feet

               // connect a bend 90, based on the condition (converting 10" into feet)
               FabricationPart pt_90bend1 = FabricationPart.Create(m_doc, bt_90bend, 10.0 / 12.0, 10.0 / 12.0, levelOne.Id);
               Connector conn1_90bend1 = GetPrimaryConnector(pt_90bend1.ConnectorManager);
               Connector conn2_90bend1 = GetSecondaryConnector(pt_90bend1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_90bend1, conn2_sqToRound1, 0);

               FabricationPart pt_90bend2 = FabricationPart.Create(m_doc, bt_90bend, 0, levelOne.Id);
               Connector conn1_90bend2 = GetPrimaryConnector(pt_90bend2.ConnectorManager);
               Connector conn2_90bend2 = GetSecondaryConnector(pt_90bend2.ConnectorManager);
               SizeAlignCoupleConnect(conn1_90bend2, conn2_90bend1, 0);

               // now let's add a tube in
               FabricationPart pt_tube1 = CreateStraightPart(bt_tube, 0, levelOne.Id, 5.0);
               Connector conn1_tube1 = GetPrimaryConnector(pt_tube1.ConnectorManager);
               Connector conn2_tube1 = GetSecondaryConnector(pt_tube1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube1, conn2_90bend2, 0);

               // and now add a square to round, connecting by round end
               // change the spec to undefined, change material, add insulation, change a connector
               FabricationPart pt_sqToRound2 = FabricationPart.Create(m_doc, bt_sqToRound, 0, levelOne.Id);
               Connector conn1_sqToRound2 = GetPrimaryConnector(pt_sqToRound2.ConnectorManager);
               Connector conn2_sqToRound2 = GetSecondaryConnector(pt_sqToRound2.ConnectorManager); // round end
               SizeAlignCoupleConnect(conn2_sqToRound2, conn2_tube1, 0);

               // set the spec to none           
               pt_sqToRound2.Specification = 0; // none         

               // now locate specific material, insulation spec and fabrication connector
               //int specId = config.LocateSpecification("Ductwork", "+6 WG");
               int materialId = config.LocateMaterial("Ductwork", "Mild Steel");
               int insSpecId = config.LocateInsulationSpecification("Ductwork", "Acoustic Liner 1''");
               int connId = config.LocateFabricationConnector("Duct - S&D", "S&D", ConnectorDomainType.Undefined, ConnectorProfileType.Rectangular);

               // now set the material, insulation spec and one of the connectors
               if (materialId >= 0)
                  pt_sqToRound2.Material = materialId;
               if (insSpecId >= 0)
                  pt_sqToRound2.InsulationSpecification = insSpecId;
               if (connId >= 0)
                  conn1_sqToRound2.GetFabricationConnectorInfo().BodyConnectorId = connId;

               // connect a 2' 6" transition to the square bend
               FabricationPart pt_transition1 = FabricationPart.Create(m_doc, bt_transition, 0, levelOne.Id);
               SetDimValue(pt_transition1, "Length", 2.5); // set length of transition to 2' 6"
               Connector conn1_transition1 = GetPrimaryConnector(pt_transition1.ConnectorManager);
               Connector conn2_transition1 = GetSecondaryConnector(pt_transition1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_transition1, conn2_straight1, 0);
               conn2_transition1.Width = 2.0;
               conn2_transition1.Height = 2.0;

               // connect a rising square bend to the transition
               FabricationPart pt_sqBend2 = FabricationPart.Create(m_doc, bt_sqBend, 0, levelOne.Id);
               Connector conn1_sqBend2 = GetPrimaryConnector(pt_sqBend2.ConnectorManager);
               Connector conn2_sqBend2 = GetSecondaryConnector(pt_sqBend2.ConnectorManager);
               SizeAlignCoupleConnect(conn1_sqBend2, conn2_transition1, 0);

               // connect a 4' 5" straight to the square bend
               FabricationPart pt_rectStraight2 = CreateStraightPart(bt_rectStraight, 0, levelOne.Id, (4.0 + (5.0 / 12.0)));
               Connector conn1_straight2 = GetPrimaryConnector(pt_rectStraight2.ConnectorManager);
               Connector conn2_straight2 = GetSecondaryConnector(pt_rectStraight2.ConnectorManager);
               SizeAlignCoupleConnect(conn1_straight2, conn2_sqBend2, 0);

               // connect a square bend to the straight
               FabricationPart pt_sqBend3 = FabricationPart.Create(m_doc, bt_sqBend, 0, levelOne.Id);
               Connector conn1_sqBend3 = GetPrimaryConnector(pt_sqBend3.ConnectorManager);
               Connector conn2_sqBend3 = GetSecondaryConnector(pt_sqBend3.ConnectorManager);
               SizeAlignCoupleConnect(conn1_sqBend3, conn2_straight2, Math.PI);

               // add a 5' straight 
               FabricationPart pt_rectStraight3 = CreateStraightPart(bt_rectStraight, 0, levelOne.Id, 5.0);
               Connector conn1_straight3 = GetPrimaryConnector(pt_rectStraight3.ConnectorManager);
               Connector conn2_straight3 = GetSecondaryConnector(pt_rectStraight3.ConnectorManager);
               SizeAlignCoupleConnect(conn1_straight3, conn2_sqBend3, 0);

               //add a Bearer hanger in middle of straight3 
               FabricationPart.CreateHanger(m_doc, bt_hangerBearer, pt_rectStraight3.Id, conn1_straight3, 2.5, true);


               // add a 45 degree radius bend 
               FabricationPart pt_radBend1 = FabricationPart.Create(m_doc, bt_radBend, 0, levelOne.Id);
               Connector conn1_radBend1 = GetPrimaryConnector(pt_radBend1.ConnectorManager);
               Connector conn2_radBend1 = GetSecondaryConnector(pt_radBend1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_radBend1, conn2_straight3, 3.0 * Math.PI / 2.0);
               SetDimValue(pt_radBend1, "Angle", Math.PI / 4.0);

               // add a 1' 8" straight 
               FabricationPart pt_rectStraight4 = CreateStraightPart(bt_rectStraight, 0, levelOne.Id, 1.0 + (8.0 / 12.0));
               Connector conn1_straight4 = GetPrimaryConnector(pt_rectStraight4.ConnectorManager);
               Connector conn2_straight4 = GetSecondaryConnector(pt_rectStraight4.ConnectorManager);
               SizeAlignCoupleConnect(conn1_straight4, conn2_radBend1, 0);

               // add a 45 degree radius bend 
               FabricationPart pt_radBend2 = FabricationPart.Create(m_doc, bt_radBend, 0, levelOne.Id);
               Connector conn1_radBend2 = GetPrimaryConnector(pt_radBend2.ConnectorManager);
               Connector conn2_radBend2 = GetSecondaryConnector(pt_radBend2.ConnectorManager);
               SizeAlignCoupleConnect(conn1_radBend2, conn2_straight4, Math.PI);
               SetDimValue(pt_radBend2, "Angle", Math.PI / 4.0);

               // add a 5' straight 
               FabricationPart pt_rectStraight5 = CreateStraightPart(bt_rectStraight, 0, levelOne.Id, 5.0);
               Connector conn1_straight5 = GetPrimaryConnector(pt_rectStraight5.ConnectorManager);
               Connector conn2_straight5 = GetSecondaryConnector(pt_rectStraight5.ConnectorManager);
               SizeAlignCoupleConnect(conn1_straight5, conn2_radBend2, 0);

               //add a Bearer hanger in middle of straight5 
               FabricationPart.CreateHanger(m_doc, bt_hangerBearer, pt_rectStraight5.Id, conn1_straight5, 2.5, true);

               // add a 2' 6" straight 
               FabricationPart pt_rectStraight6 = CreateStraightPart(bt_rectStraight, 0, levelOne.Id, 2.5);
               Connector conn1_straight6 = GetPrimaryConnector(pt_rectStraight6.ConnectorManager);
               Connector conn2_straight6 = GetSecondaryConnector(pt_rectStraight6.ConnectorManager);
               SizeAlignCoupleConnect(conn1_straight6, conn2_straight5, 0);

               // add an 8" tap to the last straight - half way along the straight - using parameter to set the product entry
               // could also set the radius directly.
               FabricationPart pt_flatShoe1 = FabricationPart.Create(m_doc, bt_flatShoe, 0, levelOne.Id);
               Parameter prodEntry_flatShoe1 = pt_flatShoe1.get_Parameter(BuiltInParameter.FABRICATION_PRODUCT_ENTRY);
               prodEntry_flatShoe1.Set("8''");
               Connector conn1_flatShoe1 = GetPrimaryConnector(pt_flatShoe1.ConnectorManager);
               Connector conn2_flatShoe1 = GetSecondaryConnector(pt_flatShoe1.ConnectorManager);
               FabricationPart.PlaceAsTap(m_doc, conn1_flatShoe1, conn1_straight6, 1.25, Math.PI, 0);

               // add a 16' 8 long tube
               double length_tube2 = 16.0 + (8.0 / 12.0);
               FabricationPart pt_tube2 = CreateStraightPart(bt_tube, 0, levelOne.Id, length_tube2);
               Connector conn1_tube2 = GetPrimaryConnector(pt_tube2.ConnectorManager);
               Connector conn2_tube2 = GetSecondaryConnector(pt_tube2.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube2, conn2_flatShoe1, 0);
               //add 3 hangers for tube2 , with specified button condition
               for (int i = 0; i < 3; i++)
               {
                  FabricationPart.CreateHanger(m_doc, bt_hangerRound, 1, pt_tube2.Id, conn1_tube2, (i + 1) * length_tube2 / 4, true);
               }

               // add a 90 degree bend
               FabricationPart pt_90bend3 = FabricationPart.Create(m_doc, bt_90bend, 0, levelOne.Id);
               Connector conn1_90bend3 = GetPrimaryConnector(pt_90bend3.ConnectorManager);
               Connector conn2_90bend3 = GetSecondaryConnector(pt_90bend3.ConnectorManager);
               SizeAlignCoupleConnect(conn1_90bend3, conn2_tube2, Math.PI);

               // add a 10' long tube
               FabricationPart pt_tube3 = CreateStraightPart(bt_tube, 0, levelOne.Id, 10.0);
               Connector conn1_tube3 = GetPrimaryConnector(pt_tube3.ConnectorManager);
               Connector conn2_tube3 = GetSecondaryConnector(pt_tube3.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube3, conn2_90bend3, 0);

               //add one hangers in middle of tube3, by default button condition 
               FabricationPart.CreateHanger(m_doc, bt_hangerRound, pt_tube3.Id, conn1_tube3, 5.0, true);

               // add a 45 degree bend 
               FabricationPart pt_45Bend1 = FabricationPart.Create(m_doc, bt_45bend, 0, levelOne.Id);
               Connector conn1_45Bend1 = GetPrimaryConnector(pt_45Bend1.ConnectorManager);
               Connector conn2_45Bend1 = GetSecondaryConnector(pt_45Bend1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_45Bend1, conn2_tube3, Math.PI);

               // add a 2' long tube
               FabricationPart pt_tube4 = CreateStraightPart(bt_tube, 0, levelOne.Id, 2.0);
               Connector conn1_tube4 = GetPrimaryConnector(pt_tube4.ConnectorManager);
               Connector conn2_tube4 = GetSecondaryConnector(pt_tube4.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube4, conn2_45Bend1, 0);

               // add a 45 degree bend 
               FabricationPart pt_45Bend2 = FabricationPart.Create(m_doc, bt_45bend, 0, levelOne.Id);
               Connector conn1_45Bend2 = GetPrimaryConnector(pt_45Bend2.ConnectorManager);
               Connector conn2_45Bend2 = GetSecondaryConnector(pt_45Bend2.ConnectorManager);
               SizeAlignCoupleConnect(conn1_45Bend2, conn2_tube4, Math.PI);

               // add a 10' long tube
               FabricationPart pt_tube5 = CreateStraightPart(bt_tube, 0, levelOne.Id, 10.0);
               Connector conn1_tube5 = GetPrimaryConnector(pt_tube5.ConnectorManager);
               Connector conn2_tube5 = GetSecondaryConnector(pt_tube5.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube5, conn2_45Bend2, 0);

               //add one hangers in middle of tube5, by default button condition 
               FabricationPart.CreateHanger(m_doc, bt_hangerRound, pt_tube5.Id, conn1_tube5, 5.0, true);

               // now go back to the straight (pt_rectStraight5) with the tap and add a square bend
               FabricationPart pt_sqBend4 = FabricationPart.Create(m_doc, bt_sqBend, 0, levelOne.Id);
               Connector conn1_sqBend4 = GetPrimaryConnector(pt_sqBend4.ConnectorManager);
               Connector conn2_sqBend4 = GetSecondaryConnector(pt_sqBend4.ConnectorManager);
               SizeAlignCoupleConnect(conn1_sqBend4, conn2_straight6, Math.PI);

               // add a 5' straight
               FabricationPart pt_rectStraight7 = CreateStraightPart(bt_rectStraight, 0, levelOne.Id, 5.0);
               Connector conn1_straight7 = GetPrimaryConnector(pt_rectStraight7.ConnectorManager);
               Connector conn2_straight7 = GetSecondaryConnector(pt_rectStraight7.ConnectorManager);
               SizeAlignCoupleConnect(conn1_straight7, conn2_sqBend4, 0);

               //add a Bearer hanger in middle of straight7, by default condition
               FabricationPart.CreateHanger(m_doc, bt_hangerBearer, pt_rectStraight7.Id, conn1_straight7, 2.5, true);

               // add a modified tee
               FabricationPart pt_rectTee1 = FabricationPart.Create(m_doc, bt_rectTee, 0, levelOne.Id);

               // Set the size prior to connecting the part. Template parts with more than 2 connectors will disable editing of sizes once one of the connectors is connected to something.
               SetDimValue(pt_rectTee1, "Right Width", 16.0 / 12.0); // set right width dimension to 16" (converted to feet)
               SetDimValue(pt_rectTee1, "Btm Width", 20.0 / 12.0);   // set bottom width dimension to 20" (converted to feet)

               Connector conn1_rectTee1 = GetPrimaryConnector(pt_rectTee1.ConnectorManager);
               Connector conn2_rectTee1 = GetSecondaryConnector(pt_rectTee1.ConnectorManager);
               Connector conn3_rectTee1 = GetFirstNonPrimaryOrSecondaryConnector(pt_rectTee1.ConnectorManager);
               SizeAlignCoupleConnect(conn3_rectTee1, conn2_straight7, Math.PI);

               // add a square to round to the tee (conn2)
               FabricationPart pt_sqToRound3 = FabricationPart.Create(m_doc, bt_sqToRound, 0, levelOne.Id);
               Connector conn1_sqToRound3 = GetPrimaryConnector(pt_sqToRound3.ConnectorManager);
               Connector conn2_sqToRound3 = GetSecondaryConnector(pt_sqToRound3.ConnectorManager);
               SizeAlignCoupleConnect(conn1_sqToRound3, conn2_rectTee1, 0);
               SetDimValue(pt_sqToRound3, "Length", 1.0 + (8.5 / 12.0));   // set length dimension to 1' 8 1/2" (converted to feet)
               SetDimValue(pt_sqToRound3, "Diameter", 1.0);                // set diameter dimension to 1'

               // add a 22' 4" long tube
               double length_tube6 = 22.0 + (4.0 / 12.0);
               FabricationPart pt_tube6 = CreateStraightPart(bt_tube, 0, levelOne.Id, length_tube6);
               Connector conn1_tube6 = GetPrimaryConnector(pt_tube6.ConnectorManager);
               Connector conn2_tube6 = GetSecondaryConnector(pt_tube6.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube6, conn2_sqToRound3, 0);

               //add 3 hangers for tube6, by default condition
               for (int i = 0; i < 3; i++)
               {
                  FabricationPart.CreateHanger(m_doc, bt_hangerRound, 1, pt_tube6.Id, conn1_tube6, (i + 1) * length_tube6 / 4, true);
               }

               // add a reducer, reducing to 8"
               FabricationPart pt_reducer1 = FabricationPart.Create(m_doc, bt_reducer, 0, levelOne.Id);
               Connector conn1_reducer1 = GetPrimaryConnector(pt_reducer1.ConnectorManager);
               Connector conn2_reducer1 = GetSecondaryConnector(pt_reducer1.ConnectorManager);
               SizeAlignCoupleConnect(conn1_reducer1, conn2_tube6, 0);
               Parameter prodEntry_reducer1 = pt_reducer1.get_Parameter(BuiltInParameter.FABRICATION_PRODUCT_ENTRY);
               prodEntry_reducer1.Set("12''x8''");

               // add a 10' long tube
               FabricationPart pt_tube7 = CreateStraightPart(bt_tube, 0, levelOne.Id, 10.0);
               Connector conn1_tube7 = GetPrimaryConnector(pt_tube7.ConnectorManager);
               Connector conn2_tube7 = GetSecondaryConnector(pt_tube7.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube7, conn2_reducer1, 0);

               //add one hangers in middle of tube7, by default button condition 
               FabricationPart.CreateHanger(m_doc, bt_hangerRound, pt_tube7.Id, conn1_tube7, 5.0, true);

               // add a curved boot tap to the 22' 4" tube (pt_tube6) 1' 2" from the end, reducing to 8"
               FabricationPart pt_curvedBoot1 = FabricationPart.Create(m_doc, bt_curvedBoot, 0, levelOne.Id);
               Parameter prodEntry_curvedBoot1 = pt_curvedBoot1.get_Parameter(BuiltInParameter.FABRICATION_PRODUCT_ENTRY);
               prodEntry_curvedBoot1.Set("12''x8''");
               Connector conn1_curvedBoot1 = GetPrimaryConnector(pt_curvedBoot1.ConnectorManager);
               Connector conn2_curvedBoot1 = GetSecondaryConnector(pt_curvedBoot1.ConnectorManager);
               FabricationPart.PlaceAsTap(m_doc, conn1_curvedBoot1, conn2_tube6, 1.0 + (2.0 / 12.0), Math.PI, Math.PI);

               // add a 16' 8" long tube to the curved boot
               double length_tube8 = 16.0 + (8.0 / 12.0);
               FabricationPart pt_tube8 = CreateStraightPart(bt_tube, 0, levelOne.Id, length_tube8);
               Connector conn1_tube8 = GetPrimaryConnector(pt_tube8.ConnectorManager);
               Connector conn2_tube8 = GetSecondaryConnector(pt_tube8.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube8, conn2_curvedBoot1, 0);
               //add 3 hangers for tube8, with specified condition 
               for (int i = 0; i < 3; i++)
               {
                  FabricationPart.CreateHanger(m_doc, bt_hangerRound, 0, pt_tube8.Id, conn1_tube8, (i + 1) * length_tube8 / 4, true);
               }

               // going back to the modified tee
               // add a square to round to the tee (conn2)
               FabricationPart pt_sqToRound4 = FabricationPart.Create(m_doc, bt_sqToRound, 0, levelOne.Id);
               Connector conn1_sqToRound4 = GetPrimaryConnector(pt_sqToRound4.ConnectorManager);
               Connector conn2_sqToRound4 = GetSecondaryConnector(pt_sqToRound4.ConnectorManager);
               SizeAlignCoupleConnect(conn1_sqToRound4, conn1_rectTee1, 0);
               SetDimValue(pt_sqToRound4, "Length", 1.0 + (8.5 / 12.0));  // set length dimension to 1' 8 1/2" (converted to feet)
               SetDimValue(pt_sqToRound4, "Diameter", 1.0);               // set diameter dimension to 1'

               // add a 10' long tube
               FabricationPart pt_tube9 = CreateStraightPart(bt_tube, 0, levelOne.Id, 10.0);
               Connector conn1_tube9 = GetPrimaryConnector(pt_tube9.ConnectorManager);
               Connector conn2_tube9 = GetSecondaryConnector(pt_tube9.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube9, conn2_sqToRound4, 0);

               //add one hangers in middle of tube9, by default button condition 
               FabricationPart.CreateHanger(m_doc, bt_hangerRound, pt_tube9.Id, conn1_tube9, 5.0, true);

               // add a curved boot to the tube 3/4 way along, reducing to 10"
               FabricationPart pt_curvedBoot2 = FabricationPart.Create(m_doc, bt_curvedBoot, 0, levelOne.Id);
               Parameter prodEntry_curvedBoot2 = pt_curvedBoot2.get_Parameter(BuiltInParameter.FABRICATION_PRODUCT_ENTRY);
               prodEntry_curvedBoot2.Set("12''x8''");
               Connector conn1_curvedBoot2 = GetPrimaryConnector(pt_curvedBoot2.ConnectorManager);
               Connector conn2_curvedBoot2 = GetSecondaryConnector(pt_curvedBoot2.ConnectorManager);
               FabricationPart.PlaceAsTap(m_doc, conn1_curvedBoot2, conn1_tube9, 7.5, Math.PI, 0);

               // add 8' 1" long tube to the curved boot
               double length_tube10 = 8.0 + (1.0 / 12.0);
               FabricationPart pt_tube10 = CreateStraightPart(bt_tube, 0, levelOne.Id, length_tube10);
               Connector conn1_tube10 = GetPrimaryConnector(pt_tube10.ConnectorManager);
               Connector conn2_tube10 = GetSecondaryConnector(pt_tube10.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube10, conn2_curvedBoot2, 0);

               //add one hangers in middle of tube10, by default button condition 
               FabricationPart.CreateHanger(m_doc, bt_hangerRound, pt_tube10.Id, conn1_tube10, length_tube10 / 2, true);

               // add a 45 degree bend 
               FabricationPart pt_45Bend3 = FabricationPart.Create(m_doc, bt_45bend, 0, levelOne.Id);
               Connector conn1_45Bend3 = GetPrimaryConnector(pt_45Bend3.ConnectorManager);
               Connector conn2_45Bend3 = GetSecondaryConnector(pt_45Bend3.ConnectorManager);
               SizeAlignCoupleConnect(conn1_45Bend3, conn2_tube10, 0);

               // add 20' long tube                
               FabricationPart pt_tube11 = CreateStraightPart(bt_tube, 0, levelOne.Id, 20.0);
               Connector conn1_tube11 = GetPrimaryConnector(pt_tube11.ConnectorManager);
               Connector conn2_tube11 = GetSecondaryConnector(pt_tube11.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube11, conn2_45Bend3, 0);

               // add a 45 degree bend 
               FabricationPart pt_45Bend4 = FabricationPart.Create(m_doc, bt_45bend, 0, levelOne.Id);
               Connector conn1_45Bend4 = GetPrimaryConnector(pt_45Bend4.ConnectorManager);
               Connector conn2_45Bend4 = GetSecondaryConnector(pt_45Bend4.ConnectorManager);
               SizeAlignCoupleConnect(conn1_45Bend4, conn2_tube11, 0);

               // add 1' 8" long tube 
               FabricationPart pt_tube12 = CreateStraightPart(bt_tube, 0, levelOne.Id, 1.0 + (8.0 / 12.0));
               Connector conn1_tube12 = GetPrimaryConnector(pt_tube12.ConnectorManager);
               Connector conn2_tube12 = GetSecondaryConnector(pt_tube12.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube12, conn2_45Bend4, 0);

               // add a reducer (to 10") on the tube with the curved boot (pt_tube12)
               FabricationPart pt_reducer2 = FabricationPart.Create(m_doc, bt_reducer, 0, levelOne.Id);
               Connector conn1_reducer2 = GetPrimaryConnector(pt_reducer2.ConnectorManager);
               Connector conn2_reducer2 = GetSecondaryConnector(pt_reducer2.ConnectorManager);
               SizeAlignCoupleConnect(conn1_reducer2, conn2_tube9, 0);
               Parameter prodEntry_reducer2 = pt_reducer2.get_Parameter(BuiltInParameter.FABRICATION_PRODUCT_ENTRY);
               prodEntry_reducer2.Set("12''x10''");

               // add a 10' long tube
               FabricationPart pt_tube13 = CreateStraightPart(bt_tube, 0, levelOne.Id, 10.0);
               Connector conn1_tube13 = GetPrimaryConnector(pt_tube13.ConnectorManager);
               Connector conn2_tube13 = GetSecondaryConnector(pt_tube13.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube13, conn2_reducer2, 0);

               //add one hangers for tube13, by default button condition 
               FabricationPart.CreateHanger(m_doc, bt_hangerRound, pt_tube13.Id, conn1_tube13, 5.0, true);


               // add a 90 bend, going 45 degrees down
               FabricationPart pt_90bend4 = FabricationPart.Create(m_doc, bt_90bend, 0, levelOne.Id);
               Connector conn1_90bend4 = GetPrimaryConnector(pt_90bend4.ConnectorManager);
               Connector conn2_90bend4 = GetSecondaryConnector(pt_90bend4.ConnectorManager);
               SizeAlignCoupleConnect(conn1_90bend4, conn2_tube13, 3.0 * Math.PI / 4.0);

               // add a 1' 2.5" long tube
               FabricationPart pt_tube14 = CreateStraightPart(bt_tube, 0, levelOne.Id, 1.0 + (2.5 / 12.0));
               Connector conn1_tube14 = GetPrimaryConnector(pt_tube14.ConnectorManager);
               Connector conn2_tube14 = GetSecondaryConnector(pt_tube14.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube14, conn2_90bend4, 0);

               // add a 45 bend
               FabricationPart pt_45bend5 = FabricationPart.Create(m_doc, bt_45bend, 0, levelOne.Id);
               Connector conn1_45bend5 = GetPrimaryConnector(pt_45bend5.ConnectorManager);
               Connector conn2_45bend5 = GetSecondaryConnector(pt_45bend5.ConnectorManager);
               SizeAlignCoupleConnect(conn1_45bend5, conn2_tube14, Math.PI / 2.0);

               // add a 20' long tube
               FabricationPart pt_tube15 = CreateStraightPart(bt_tube, 0, levelOne.Id, 20.0);
               Connector conn1_tube15 = GetPrimaryConnector(pt_tube15.ConnectorManager);
               Connector conn2_tube15 = GetSecondaryConnector(pt_tube15.ConnectorManager);
               SizeAlignCoupleConnect(conn1_tube15, conn2_45bend5, 0);
               //add 4 hangers for tube15, by default button condition 
               for (int i = 0; i < 4; i++)
               {
                  FabricationPart.CreateHanger(m_doc, bt_hangerRound, pt_tube15.Id, conn1_tube15, (i + 1) * (20.0 / 5), true);
               }

               // now let's place a 6" valve by its insertion point in free space
               FabricationPart pt_valve1 = FabricationPart.Create(m_doc, bt_valve, 1, levelOne.Id);
               Parameter prodEntry_pt_valve1 = pt_valve1.get_Parameter(BuiltInParameter.FABRICATION_PRODUCT_ENTRY);
               prodEntry_pt_valve1.Set("6''");
               FabricationPart.AlignPartByInsertionPoint(m_doc, pt_valve1.Id, new XYZ(16, -10, 0), 0, 0, 0, FabricationPartJustification.Middle, null);
               m_doc.Regenerate();
               Connector conn2_valve1 = GetSecondaryConnector(pt_valve1.ConnectorManager);

               // add 10' copper pipe to the valve
               FabricationPart pt_pipe1 = CreateStraightPart(bt_groovedPipe, 0, levelOne.Id, 10.0);
               Connector conn1_pipe1 = GetPrimaryConnector(pt_pipe1.ConnectorManager);
               Connector conn2_pipe1 = GetSecondaryConnector(pt_pipe1.ConnectorManager);
               SizeAlignSlopeJustifyCoupleConnect(conn1_pipe1, conn2_valve1, 0, 0, FabricationPartJustification.Middle);

               // insert a valve into the middle of the copper pipe - it will size automatically
               XYZ pipe1_pos = (conn1_pipe1.Origin + conn2_pipe1.Origin) / 2.0;
               FabricationPart pt_valve2 = FabricationPart.Create(m_doc, bt_valve, 1, levelOne.Id);
               FabricationPart.AlignPartByInsertionPointAndCutInToStraight(m_doc, pt_pipe1.Id, pt_valve2.Id, pipe1_pos, Math.PI / 2.0, 0, false);
               m_doc.Regenerate();

               // add a 90 elbow and slope it
               FabricationPart pt_90elbow1 = FabricationPart.Create(m_doc, bt_90elbow, 0, levelOne.Id);
               Connector conn1_90elbow1 = GetPrimaryConnector(pt_90elbow1.ConnectorManager);
               Connector conn2_90elbow1 = GetSecondaryConnector(pt_90elbow1.ConnectorManager);
               SizeAlignSlopeJustifyCoupleConnect(conn1_90elbow1, conn2_pipe1, Math.PI, 0.02, FabricationPartJustification.Middle);

               // add a copper pipe
               FabricationPart pt_pipe2 = CreateStraightPart(bt_groovedPipe, 0, levelOne.Id, 10.0);
               Connector conn1_pipe2 = GetPrimaryConnector(pt_pipe2.ConnectorManager);
               Connector conn2_pipe2 = GetSecondaryConnector(pt_pipe2.ConnectorManager);
               SizeAlignSlopeJustifyCoupleConnect(conn1_pipe2, conn2_90elbow1, 0, 0, FabricationPartJustification.Middle);

               tr.Commit();
            }
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }

         return Result.Succeeded;
      }

      /// <summary>
      /// Convenience method to get fabrication part's dimension value, specified by the dimension name.
      /// </summary>
      /// <param name="part">
      /// The fabrication part to be queried.
      /// </param>
      /// <param name="dimName">
      /// The name of the fabrication dimension.
      /// </param>
      /// <returns>
      /// Returns the fabrication dimension value for the fabrication part, as specified by the dimension name.
      /// </returns>
      double GetDimValue(FabricationPart part, string dimName)
      {
         double value = 0;
         if (part != null)
         {
            IList<FabricationDimensionDefinition> dims = part.GetDimensions();
            foreach (FabricationDimensionDefinition def in dims)
            {
               if (def.Name.Equals(dimName))
               {
                  value = part.GetDimensionValue(def);
                  break;
               }
            }
         }

         return value;
      }

      /// <summary>
      /// Convenience method to set fabrication part's dimension value, specified by the dimension name.
      /// </summary>
      /// <param name="part">
      /// The fabrication part.
      /// </param>
      /// <param name="dimName">
      /// The name of the fabrication dimension.
      /// </param>
      /// <param name="dimValue">
      /// The value of the fabrication dimension to set to.
      /// </param>
      /// <returns>
      /// Returns the fabrication dimension value for the fabrication part, as specified by the dimension name.
      /// </returns>
      bool SetDimValue(FabricationPart part, string dimName, double dimValue)
      {
         IList<FabricationDimensionDefinition> dims = part.GetDimensions();
         FabricationDimensionDefinition dim = null;
         foreach (FabricationDimensionDefinition def in dims)
         {
            if (def.Name.Equals(dimName))
            {
               dim = def;
               break;
            }
         }

         if (dim == null)
            return false;

         part.SetDimensionValue(dim, dimValue);
         m_doc.Regenerate();
         return true;
      }

      /// <summary>
      /// Convenience method to automatically size, align, couple (if needed) and connect two fabrication part
      /// by the specified connectors.
      /// </summary>
      /// <param name="conn_from">
      /// The connector to align by of the fabrication part to move.
      /// </param>
      /// <param name="conn_to">
      /// The connector to align to.
      /// </param>
      /// <param name="rotation">
      /// Rotation around the direction of connection - angle between width vectors in radians. 
      /// </param>
      /// <returns>
      /// Returns the fabrication dimension value for the fabrication part, as specified by the dimension name.
      /// </returns>
      void SizeAlignCoupleConnect(Connector conn_from, Connector conn_to, double rotation)
      {
         if (conn_from.Shape == ConnectorProfileType.Rectangular || conn_from.Shape == ConnectorProfileType.Oval)
         {
            conn_from.Height = conn_to.Height;
            conn_from.Width = conn_to.Width;
         }
         else
         {
            conn_from.Radius = conn_to.Radius;
         }

         m_doc.Regenerate();

         FabricationPart.AlignPartByConnectors(m_doc, conn_from, conn_to, rotation);
         m_doc.Regenerate();

         FabricationPart.ConnectAndCouple(m_doc, conn_from, conn_to);
         m_doc.Regenerate();
      }

      /// <summary>
      /// Convenience method to automatically size, align, slope, justification couple (if needed) and connect two fabrication parts
      /// by the specified connectors.
      /// </summary>
      /// <param name="conn_from">
      /// The connector to align by of the fabrication part to move.
      /// </param>
      /// <param name="conn_to">
      /// The connector to align to.
      /// </param>
      /// <param name="rotation">
      /// Rotation around the direction of connection - angle between width vectors in radians. 
      /// </param>
      /// <param name="slope">
      /// The slope value to flex to match if possible in fractional units (eg.1/50). Positive values are up, negative are down. Slopes can only be applied
      /// to fittings, whilst straights will inherit the slope from the piece it is connecting to.
      /// </param>
      /// <param name="justification">
      /// The justification to align eccentric parts.
      /// </param>
      /// <returns>
      /// Returns the fabrication dimension value for the fabrication part, as specified by the dimension name.
      /// </returns>
      void SizeAlignSlopeJustifyCoupleConnect(Connector conn_from, Connector conn_to, double rotation, double slope, FabricationPartJustification justification)
      {
         if (conn_from.Shape == ConnectorProfileType.Rectangular || conn_from.Shape == ConnectorProfileType.Oval)
         {
            conn_from.Height = conn_to.Height;
            conn_from.Width = conn_to.Width;
         }
         else
         {
            conn_from.Radius = conn_to.Radius;
         }

         m_doc.Regenerate();

         FabricationPart.AlignPartByConnectorToConnector(m_doc, conn_from, conn_to, rotation, slope, justification);
         m_doc.Regenerate();

         FabricationPart.ConnectAndCouple(m_doc, conn_from, conn_to);
         m_doc.Regenerate();
      }

      /// <summary>
      /// Convenience method to locate a fabrication service button specified by group and name.
      /// </summary>
      /// <param name="service">
      /// The fabrication service.
      /// </param>
      /// <param name="group">
      /// The fabrication service group index.
      /// </param>
      /// <param name="name">
      /// The fabrication service button name.
      /// </param>
      /// <returns>
      /// Returns the fabrication service button as specified by the fabrication service, group and name.
      /// </returns>
      FabricationServiceButton locateButton(FabricationService service, int group, string name)
      {
         FabricationServiceButton button = null;
         if (service != null && group >= 0 && group < service.GroupCount)
         {
            int buttonCount = service.GetButtonCount(group);
            for (int i = 0; button == null && i < buttonCount; i++)
            {
               FabricationServiceButton bt = service.GetButton(group, i);
               if (bt != null && bt.Name.Equals(name))
                  button = bt;
            }
         }

         return button;
      }

      /// <summary>
      /// Convenience method to create a straight fabrication part.
      /// </summary>
      /// <param name="fsb">
      /// The FabricationServiceButton used to create the fabrication part from.
      /// </param>
      /// <param name="condition">
      /// The condition index of the fabrication service button.
      /// </param>
      /// <param name="levelId">
      /// The element identifier belonging to the level on which to create this fabrication part.
      /// </param>
      /// <param name="length">
      /// The length, in feet, of the fabrication part to be created. 
      /// </param>
      /// <returns>
      /// Returns a straight fabrication part, as specified by the fabrication service button, condition, level id and length.
      /// </returns>
		FabricationPart CreateStraightPart(FabricationServiceButton fsb, int condition, ElementId levelId, double length)
      {
         FabricationPart straight = FabricationPart.Create(m_doc, fsb, condition, levelId);

         Parameter length_option = straight.LookupParameter("Length Option");
         length_option.Set("Value");

         Parameter lengthParam = straight.LookupParameter("Length");
         lengthParam.Set(length);

         m_doc.Regenerate();

         return straight;
      }

      /// <summary>
      /// Convenience method to get the primary connector from the specified connector manager.
      /// </summary>
      /// <param name="cm">
      /// The connector manager.
      /// </param>
      /// <returns>
      /// Returns the primary connector from the connector manager.
      /// </returns>
		Connector GetPrimaryConnector(ConnectorManager cm)
      {
         foreach (Connector cn in cm.Connectors)
         {
            MEPConnectorInfo info = cn.GetMEPConnectorInfo();
            if (info.IsPrimary)
               return cn;
         }
         return null;
      }

      /// <summary>
      /// Convenience method to get the secondary connector from the specified connector manager.
      /// </summary>
      /// <param name="cm">
      /// The connector manager.
      /// </param>
      /// <returns>
      /// Returns the secondary connector from the connector manager.
      /// </returns>
		Connector GetSecondaryConnector(ConnectorManager cm)
      {
         foreach (Connector cn in cm.Connectors)
         {
            MEPConnectorInfo info = cn.GetMEPConnectorInfo();
            if (info.IsSecondary)
               return cn;
         }
         return null;
      }

      /// <summary>
      /// Convenience method to get the first non-primary and non-secondary connector from the specified connector manager.
      /// </summary>
      /// <param name="cm">
      /// The connector manager.
      /// </param>
      /// <returns>
      /// Returns the first non-primary and non-secondary connector from the connector manager.
      /// </returns>
      Connector GetFirstNonPrimaryOrSecondaryConnector(ConnectorManager cm)
      {
         foreach (Connector cn in cm.Connectors)
         {
            MEPConnectorInfo info = cn.GetMEPConnectorInfo();
            if (!info.IsPrimary && !info.IsSecondary)
               return cn;
         }
         return null;
      }

      /// <summary>
      /// Convenience method to get all fabrication material identifiers from the
      /// specified fabrication configuration.
      /// </summary>
      /// <param name="config">
      /// The fabrication configuration.
      /// </param>
      /// <returns>
      /// Returns a list of all the fabrication material identifiers for this
      /// fabrication configuration.
      /// </returns>
      void GetMaterials(FabricationConfiguration config)
      {
         m_materialIds = config.GetAllMaterials(null);

         m_materialGroups = new List<string>();
         m_materialNames = new List<string>();

         for (int i = 0; i < m_materialIds.Count; i++)
         {
            m_materialGroups.Add(config.GetMaterialGroup(m_materialIds[i]));
            m_materialNames.Add(config.GetMaterialName(m_materialIds[i]));
         }
      }

      /// <summary>
      /// Convenience method to get all fabrication specification identifiers from the
      /// specified fabrication configuration.
      /// </summary>
      /// <param name="config">
      /// The fabrication configuration.
      /// </param>
      /// <returns>
      /// Returns a list of all the fabrication specification identifiers for this
      /// fabrication configuration.
      /// </returns>
      void GetSpecs(FabricationConfiguration config)
      {
         m_specIds = config.GetAllSpecifications(null);

         m_specGroups = new List<string>();
         m_specNames = new List<string>();

         for (int i = 0; i < m_specIds.Count; i++)
         {
            m_specGroups.Add(config.GetSpecificationGroup(m_specIds[i]));
            m_specNames.Add(config.GetSpecificationName(m_specIds[i]));
         }
      }

      /// <summary>
      /// Convenience method to get all fabrication insulation specification identifiers from the
      /// specified fabrication configuration.
      /// </summary>
      /// <param name="config">
      /// The fabrication configuration.
      /// </param>
      /// <returns>
      /// Returns a list of all the fabrication insulation specification identifiers for this
      /// fabrication configuration.
      /// </returns>
      void GetInsulationSpecs(FabricationConfiguration config)
      {
         m_insSpecIds = config.GetAllInsulationSpecifications(null);

         m_insSpecGroups = new List<string>();
         m_insSpecNames = new List<string>();

         for (int i = 0; i < m_insSpecIds.Count; i++)
         {
            m_insSpecGroups.Add(config.GetInsulationSpecificationGroup(m_insSpecIds[i]));
            m_insSpecNames.Add(config.GetInsulationSpecificationName(m_insSpecIds[i]));
         }
      }

      /// <summary>
      /// Convenience method to get all fabrication connector identifiers from the
      /// specified fabrication configuration.
      /// </summary>
      /// <param name="config">
      /// The fabrication configuration.
      /// </param>
      /// <returns>
      /// Returns a list of all the fabrication connector identifiers for this
      /// fabrication configuration.
      /// </returns>
      void GetFabricationConnectors(FabricationConfiguration config)
      {
         m_connIds = config.GetAllFabricationConnectorDefinitions(ConnectorDomainType.Undefined, ConnectorProfileType.Invalid);

         m_connGroups = new List<string>();
         m_connNames = new List<string>();

         for (int i = 0; i < m_connIds.Count; i++)
         {
            m_connGroups.Add(config.GetFabricationConnectorGroup(m_connIds[i]));
            m_connNames.Add(config.GetFabricationConnectorName(m_connIds[i]));
         }
      }
   }
}

