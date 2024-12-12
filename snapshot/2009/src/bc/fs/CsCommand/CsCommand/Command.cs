using System;
using Autodesk.Revit;

namespace CsCommand
{
  public class Command : IExternalCommand
  {
    public IExternalCommand.Result Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      BuildingCoder.Command6 fsCommand = new BuildingCoder.Command6();
      return fsCommand.Execute( commandData, ref message, elements );
    }
  }
}
