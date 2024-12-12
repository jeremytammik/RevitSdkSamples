//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Fabrication;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class RenumberingPart : IExternalCommand
   {
      #region IExternalCommand Members Implementation
      /// <summary>
      /// This class inherits from IExternalCommand interface, and implements the Execute method to renumber the straight/coupling 
      /// fabrication parts by a selected ductwork or pipework run.
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
      /// Cancelled can be used to signify that the user canceled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         Document doc = commandData.Application.ActiveUIDocument.Document;
         UIDocument uidoc = commandData.Application.ActiveUIDocument;

         FabricationPart fabPart = null;
         try
         {
            Reference refObj = uidoc.Selection.PickObject(ObjectType.Element, "Pick a ductwork or pipework straight or coupling to start.");
            fabPart = doc.GetElement(refObj) as FabricationPart;
         }
         catch (Exception ex)
         {
            // canceled the pick.
            message = ex.Message;
            return Result.Succeeded;
         }

         if (fabPart == null || (!fabPart.IsAStraight() && !IsACoupling(fabPart)))
         {
            message = "Pick a ductwork or pipework straight or coupling to start.";
            return Result.Failed;
         }
         else
         {
            List<FabricationPart> straightParts = new List<FabricationPart>();
            straightParts = GetStraightRunFromPart(fabPart);

            var partIds = straightParts.Select(p => p.Id).ToList();
            // Highlight the parts in the run.
            uidoc.Selection.SetElementIds(partIds);

            using (Transaction ts = new Transaction(doc, "Renumbering Parts"))
            {
               try
               {
                  ts.Start();
                  foreach (FabricationPart part in straightParts)
                  {
                     if (IsAReducer(part))
                     {
                        continue;
                     }

                     if (IsADuct(part))
                     {
                        if (IsACoupling(part))
                        {
                           part.ItemNumber = "DUCT-COUPLING" + m_ductCouplingNumber.ToString();
                           m_ductCouplingNumber++;
                        }
                        else
                        {
                           part.ItemNumber = "DUCT" + m_ductNumber.ToString();
                           m_ductNumber++;
                        }
                     }
                     else if (IsAPipe(part))
                     {
                        if (IsACoupling(part))
                        {
                           part.ItemNumber = "PIPE-COUPLING" + m_pipeCouplingNumber.ToString();
                           m_pipeCouplingNumber++;
                        }
                        else
                        {
                           part.ItemNumber = "PIPE" + m_pipeNumber.ToString();
                           m_pipeNumber++;
                        }
                     }
                  }
                  ts.Commit();
               }
               catch (Exception ex)
               {
                  message = ex.Message;
                  return Result.Failed;
               };
            }
         }
         return Result.Succeeded;
      }
      #endregion IExternalCommand Members Implementation

      #region Private Methods
      /// <summary>
      /// Gets the ductwork or pipework run from the given part. The run includes the straight parts and couplings (if any) 
      /// and ends with any fitting part.
      /// </summary>
      /// <param name="fabPart">The given part.</param>
      /// <returns>The list of the straight parts being sorted.</returns>
      private List<FabricationPart> GetStraightRunFromPart(FabricationPart fabPart)
      {
         List<FabricationPart> straightParts = new List<FabricationPart>();
         if (fabPart != null)
         {
            List<FabricationPart> downstreamParts = new List<FabricationPart>();
            List<FabricationPart> upstreamParts = new List<FabricationPart>();

            var connectorManager = fabPart.ConnectorManager;
            var connectors = connectorManager.Connectors.Cast<Connector>();
            var connectedConnectors = connectors.Where(IsConnected).ToList();
            for (int ii = 0; ii < connectedConnectors.Count; ii++)
            {
               Connector connector = connectedConnectors[ii];
               if (ii == 0)
               {
                  downstreamParts = GetConnectedStraightRun(connector, fabPart);
               }
               else
               {
                  upstreamParts = GetConnectedStraightRun(connector, fabPart);
                  upstreamParts.Reverse();
               }
            }

            if (upstreamParts.Count > 0)
            {
               straightParts = upstreamParts;
            }
            straightParts.Add(fabPart);
            straightParts.AddRange(downstreamParts);
         }
         return straightParts;
      }

      /// <summary>
      /// Gets the straight part run from the given connector.
      /// </summary>
      /// <param name="fromConnector">The connector that associates with the part.</param>
      /// <param name="fromPart">The given part.</param>
      /// <returns>The list of the straight parts.</returns>
      private List<FabricationPart> GetConnectedStraightRun(Connector fromConnector, FabricationPart fromPart)
      {
         List<FabricationPart> parts = new List<FabricationPart>();

         FabricationPart part = fromPart;
         while (part != null)
         {
            var connectorsConnectedtoOtherPart = GetRefConnectors(fromConnector).Where(c => IsConnectedStraightPart(c, part)).ToList();
            if (connectorsConnectedtoOtherPart.Count == 0)
            {
               break;
            }
            else
            {
               foreach (Connector conn in connectorsConnectedtoOtherPart)
               {
                  // should only be one:
                  part = conn.Owner as FabricationPart;
                  parts.Add(part);
                  fromConnector = GetNextConnector(conn, part);
                  break;
               }
            }
         }

         return parts;
      }

      /// <summary>
      /// Gets the connector from the part that is different from the given connector.
      /// </summary>
      /// <param name="fromConnector">The given connector that associates with the part.</param>
      /// <param name="fabPart">The fabrication part.</param>
      /// <returns>The connector that is different from given connector.</returns>
      private Connector GetNextConnector(Connector fromConnector, FabricationPart fabPart)
      {
         if (fabPart != null)
         {
            var connectorManger = fabPart.ConnectorManager;
            var connectors = connectorManger.Connectors.Cast<Connector>();
            foreach (Connector conn in connectors)
            {
               if (conn.Id != fromConnector.Id)
               {
                  return conn;
               }
            }
         }
         return null;
      }

      /// <summary>
      /// Gets a connector that connects to the straight part.
      /// </summary>
      /// <param name="fromConnector">The connector associates with the part.</param>
      /// <param name="fromPart">The straight part.</param>
      /// <returns>The connector that connects to the part.</returns>
      private FabricationPart GetConnectedStraightPart(Connector fromConnector, FabricationPart fromPart)
      {
         FabricationPart connectedPart = null;
         var connectorsConnectedtoOtherPart = GetRefConnectors(fromConnector).Where(c => IsConnectedStraightPart(c, fromPart)).ToList();
         foreach (Connector conn in connectorsConnectedtoOtherPart)
         {
            connectedPart = conn.Owner as FabricationPart;
            if (connectedPart != null)
            {
               break;
            }
         }

         return connectedPart;
      }

      /// <summary>
      /// Gets all references of the connector.
      /// </summary>
      /// <param name="connector">The given connector.</param>
      /// <returns>The list of the connectors that are connected to the given connector.</returns>
      private IEnumerable<Connector> GetRefConnectors(Connector connector)
      {
         return connector.AllRefs.Cast<Connector>();
      }

      /// <summary>
      /// True if connector is not Reference connector and is connected
      /// </summary>
      Func<Connector, bool> IsConnected = c => c.ConnectorType != ConnectorType.Reference && c.IsConnected;

      /// <summary>
      /// Checks if the connector associates with a connected straight part.
      /// </summary>
      /// <param name="c">The connector to check.</param>
      /// <param name="basePart">The part to check.</param>
      /// <returns>True if connector is not a Reference connector and is connected to a straight part which is different from the given part.</returns>
      public bool IsConnectedStraightPart(Connector c, FabricationPart basePart)
      {
         if (c != null)
         {
            FabricationPart part = c.Owner as FabricationPart;
            if (part != null)
            {
               return (c.ConnectorType != ConnectorType.Reference && c.IsConnected && c.Owner.Id != basePart.Id && (part.IsAStraight() || IsACoupling(part) || IsAReducer(part)));
            }
         }
         return false;
      }

      /// <summary>
      /// Checks if the given part is fabrication ductwork.
      /// </summary>
      /// <param name="fabPart">The part to check.</param>
      /// <returns>True if the part is fabrication ductwork.</returns>
      private bool IsADuct(FabricationPart fabPart)
      {
         return (fabPart != null && (fabPart.Category.Id.IntegerValue == (int)BuiltInCategory.OST_FabricationDuctwork));
      }

      /// <summary>
      /// Checks if the part is fabrication pipework.
      /// </summary>
      /// <param name="fabPart">The part to check.</param>
      /// <returns>True if the part is fabrication pipework.</returns>
      private bool IsAPipe(FabricationPart fabPart)
      {
         return (fabPart != null && (fabPart.Category.Id.IntegerValue == (int)BuiltInCategory.OST_FabricationPipework));
      }

      /// <summary>
      /// Checks if the part is a coupling. 
      /// The CID's (the fabrication part item customer Id) that are recognized internally as couplings are:
      ///   CID 522, 1112 - Round Ductwork
      ///   CID 1522 - Oval Ductwork
      ///   CID 4522 - Rectangular Ductwork
      ///   CID 2522 - Pipe Work
      ///   CID 3522 - Electrical
      /// </summary>
      /// <param name="fabPart">The part to check.</param>
      /// <returns>True if the part is a coupling.</returns>
      private bool IsACoupling(FabricationPart fabPart)
      {
         if (fabPart != null)
         {
            int CID = fabPart.ItemCustomId;
            if (CID == 522 || CID == 1522 || CID == 2522 || CID == 3522 || CID == 1112)
            {
               return true;
            }
         }
         return false;
      }

      /// <summary>
      /// Checks if the part is a reducer. 
      /// </summary>
      /// <param name="fabPart">The part to check.</param>
      /// <returns>True if the part is a reducer.</returns>
      private bool IsAReducer(FabricationPart fabPart)
      {
         if (fabPart != null)
         {
            int CID = fabPart.ItemCustomId;
            if (CID == 2051)
            {
               return true;
            }
         }
         return false;
      }
      #endregion

      #region Member Variables
      private static int m_ductNumber = 1;
      private static int m_ductCouplingNumber = 1;
      private static int m_pipeNumber = 1;
      private static int m_pipeCouplingNumber = 1;
      #endregion
   }
}