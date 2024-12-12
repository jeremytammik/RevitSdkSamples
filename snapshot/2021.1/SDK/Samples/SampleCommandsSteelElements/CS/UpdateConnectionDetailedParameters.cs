using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Autodesk.Revit.UI.Selection;
using Autodesk.AdvanceSteel.CADAccess;
using Autodesk.AdvanceSteel.Geometry;
using Autodesk.AdvanceSteel.Modelling;
using Autodesk.SteelConnectionsDB;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.AdvanceSteel.ConstructionTypes;

namespace Revit.SDK.Samples.SampleCommandsSteelElements.UpdateConnectionDetailedParameters.CS
{
   /// <summary>
   /// This command shows how you can modify detailed connection parameters. I choose a base plate connection for this test.
   /// In order to run this command, you need a model with a base plate connection.
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
      public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         UIDocument activeDoc = commandData.Application.ActiveUIDocument;
         Autodesk.Revit.DB.Document doc = activeDoc.Document;
         if (null == doc)
            return Result.Failed;

         try
         {
            // Start detailed steel modeling transaction
            using (FabricationTransaction trans = new FabricationTransaction(doc, false, "Update connection parameters"))
            {
               // for more details, please consult http://www.autodesk.com/adv-steel-api-walkthroughs-2019-enu
               Reference eRef = activeDoc.Selection.PickObject(ObjectType.Element, "Pick a base plate connection");

               Element elem = doc.GetElement(eRef.ElementId);
               if (null == elem || !(elem is StructuralConnectionHandler))
                  return Result.Failed;

               StructuralConnectionHandler rvtConnection = (StructuralConnectionHandler)elem;

               FilerObject filerObj = Utilities.Functions.GetFilerObject(doc, eRef);

               if (null == filerObj || !(filerObj is UserAutoConstructionObject))
                  return Result.Failed;

               UserAutoConstructionObject asConnection = (UserAutoConstructionObject)filerObj;
               //
               //read connection parameters
               IFiler connectionFiler = asConnection.Save();

               if(connectionFiler != null)
               {
                  //I choose to modify thickess of the base plate
                  connectionFiler.WriteItem(Convert.ToDouble(50.0), "BaseThickness"); //units must be milimmeters; 
                  asConnection.Load(connectionFiler); //update connection parameters
                  asConnection.Update();
                  //
                  //if the connection parameters are modified, than we have to set this flag to true,
                  //meaning that this connection has different parameters than it's connection type.
                  rvtConnection.OverrideTypeParams = true;
               }
               trans.Commit();
            }
         }
         catch (Autodesk.Revit.Exceptions.OperationCanceledException)
         {
            return Result.Cancelled;
         }
         return Result.Succeeded;
      }
   }
}
