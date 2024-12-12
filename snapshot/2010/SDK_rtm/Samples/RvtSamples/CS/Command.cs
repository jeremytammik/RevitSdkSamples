using System;
using Autodesk.Revit;

namespace RvtSamples
{
  class Command : IExternalCommand
  {
    public IExternalCommand.Result Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      string filename = Application.Filename;

    }
  }
}
