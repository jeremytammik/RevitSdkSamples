using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;
using System.Reflection;

namespace ExtensibleStorageManager
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class Command : IExternalCommand 
   {
   
     
      public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {

         UICommand dialog = new UICommand(commandData.Application.ActiveUIDocument.Document, commandData.Application.ActiveAddInId.GetGUID().ToString());
          dialog.ShowDialog();
          return Result.Succeeded;
      }




 
   }




}
