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
using RvtDwgAddon;
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
