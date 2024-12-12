//
// (C) Copyright 2003-2020 by Autodesk, Inc.
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

using Autodesk.AdvanceSteel.ApplicabilityRanges;
using Autodesk.AdvanceSteel.ConstructionTypes;
using Autodesk.AdvanceSteel.DotNetRoots.Units;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Structure.StructuralSections;
using Autodesk.Revit.UI;
using Autodesk.SteelConnectionsDB;
using System.Collections.Generic;
using System.Linq;


namespace Revit.SDK.Samples.SampleCommandsSteelElements.AddRangesToConnectionType.CS
{

   
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class Command : IExternalCommand
    {
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

         // Get the document from external command data.
         UIDocument activeDoc = commandData.Application.ActiveUIDocument;
         Autodesk.Revit.DB.Document doc = activeDoc.Document;

         if (null == doc)
         {
            return Result.Failed;
         }
         // The transaction and its status. We use Revit's Transaction class for this purpose
         Autodesk.Revit.DB.Transaction trans = new Autodesk.Revit.DB.Transaction(doc, "Add ranges of applicability");
         TransactionStatus ts = TransactionStatus.Uninitialized;

         try
         {
            // Select the connection to add ranges, using Revit's StructuralConnectionHandler class
            StructuralConnectionHandler conn = Utilities.Functions.SelectConnection(activeDoc);

            if (null == conn)
            {
               return Result.Failed;
            }

            StructuralConnectionHandlerType connectionType = doc.GetElement(conn.GetTypeId()) as StructuralConnectionHandlerType;

            if (null == connectionType)
            {
               return Result.Failed;
            }

            RuleApplicabilityRangeTable rangeTable = ApplicabilityRangesAccess.GetRanges(connectionType);

            // Create the rows and add the conditions to them
            RuleApplicabilityRangeRow rangeRow1 = new RuleApplicabilityRangeRow();
            rangeRow1.Key = "My new range 1";
            rangeRow1.Ranges = CreateConditionsForRow1();

            RuleApplicabilityRangeRow rangeRow2 = new RuleApplicabilityRangeRow();
            rangeRow2.Key = "My new range 2";
            rangeRow2.Ranges = CreateConditionsForRow2();

            // get existing rows
            RuleApplicabilityRangeRow[] rows = rangeTable.Rows;
            RuleApplicabilityRangeRow[] newRows = new RuleApplicabilityRangeRow[] { rangeRow1, rangeRow2 };

            // set back the rows
            rangeTable.Rows = rows.Concat(newRows).ToArray();

            // we can also verify if the conditions added in the ranges are met. If the result is false it means that the input elements are out of the defined conditions
            bool validate = ApplicabilityRangeValidator.Validate(conn, rangeTable, "Revit", "");

            // Start the transaction
            trans.Start();

            // Save the ranges
            ApplicabilityRangesAccess.SaveRanges(connectionType, rangeTable);

            // Commit the transaction
            ts = trans.Commit();

            if (ts != TransactionStatus.Committed)
            {
               message = "Failed to commit the current transaction !";
               trans.RollBack();
               return Result.Failed;
            }
         }

         catch (Autodesk.Revit.Exceptions.OperationCanceledException)
         {
            if (ts != TransactionStatus.Uninitialized)
            {
               trans.RollBack();
            }
            trans.Dispose();
            return Result.Cancelled;
         }
         catch (Autodesk.Revit.Exceptions.ArgumentException)
         {
            if (ts != TransactionStatus.Uninitialized)
            {
               trans.RollBack();
            }
            trans.Dispose();
            message = "Failed to add ranges";
            return Result.Failed;
         }
         return Result.Succeeded;
      }

      private static RuleApplicabilityCondition[] CreateConditionsForRow1()
      {
         // the conditions are for the 1st element from the connection
         int connectionObjectIdx = 0;

         // create a condition for FX parameter
         RuleApplicabilityConditionRange condition1 = new RuleApplicabilityConditionRange()
         {
            Key = "Cond1",
            MinVal = new RuleApplicabilityData(0.0), // 0 KN
            MaxVal = new RuleApplicabilityData(5.0), // 5 KN
            ObjectId = connectionObjectIdx,
            Unit = Unit.eUnitType.kForce,
            PropertyId = RuleApplicabilityPropertyId.kFx
         };


         // create a condition for material parameter
         RuleApplicabilityConditionList condition2 = new RuleApplicabilityConditionList()
         {
            Key = "Cond2",
            Items = new RuleApplicabilityData[2] { new RuleApplicabilityData("some material name 1"), new RuleApplicabilityData("some material name 2") },
            ObjectId = connectionObjectIdx,
            PropertyId = RuleApplicabilityPropertyId.kMaterial_Name
         };

         // create a condition for family name parameter
         RuleApplicabilityConditionList condition3 = new RuleApplicabilityConditionList()
         {
            Key = "Cond3",
            Items = new RuleApplicabilityData[1] { new RuleApplicabilityData("W Shapes") },
            ObjectId = connectionObjectIdx,
            PropertyId = RuleApplicabilityPropertyId.kSection_Class
         };

         return new RuleApplicabilityCondition[]{condition1, condition2, condition3 };
      }

      private static RuleApplicabilityCondition[] CreateConditionsForRow2()
      {
         // the conditions are for the 1st element from the connection
         int connectionObjectIdx = 0;

         // create a condition for MX parameter
         RuleApplicabilityConditionRange condition1 = new RuleApplicabilityConditionRange()
         {
            Key = "Cond1",
            MinVal = new RuleApplicabilityData(0.0), // 0 KN-M
            MaxVal = new RuleApplicabilityData(5.0), // 5 KN-M
            ObjectId = connectionObjectIdx,
            Unit = Unit.eUnitType.kMoment,
            PropertyId = RuleApplicabilityPropertyId.kMx
         };

         // create a condition for section shape
         RuleApplicabilityConditionList condition2 = new RuleApplicabilityConditionList()
         {
            Key = "Cond2",
            Items = new RuleApplicabilityData[1] { new RuleApplicabilityData((int)StructuralSectionShape.IParallelFlange)},
            ObjectId = connectionObjectIdx,
            PropertyId = RuleApplicabilityPropertyId.kSection_Shape
         };

         // create a condition for flange thickness 
         RuleApplicabilityConditionRange condition3 = new RuleApplicabilityConditionRange()
         {
            Key = "Cond3",
            MinVal = new RuleApplicabilityData(10.0), // 10 mm
            MaxVal = new RuleApplicabilityData(20.0), // 20 mm
            ObjectId = connectionObjectIdx,
            Unit = Unit.eUnitType.kDistance,
            PropertyId = RuleApplicabilityPropertyId.kSection_FlangeThickness
         };

         return new RuleApplicabilityCondition[] { condition1, condition2, condition3 };
      }
   }
}