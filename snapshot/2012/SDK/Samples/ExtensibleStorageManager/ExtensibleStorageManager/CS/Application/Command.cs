using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;

namespace ExtensibleStorageManager
{
  [Transaction( TransactionMode.Manual )]
  [Journaling( JournalingMode.NoCommandData )]
  public class Command : IExternalCommand
  {
    public Result Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      UICommand dialog = new UICommand( 
        commandData.Application.ActiveUIDocument.Document, 
        commandData.Application.ActiveAddInId.GetGUID().ToString() );

      dialog.ShowDialog();

      return Result.Succeeded;
    }
  }
}
