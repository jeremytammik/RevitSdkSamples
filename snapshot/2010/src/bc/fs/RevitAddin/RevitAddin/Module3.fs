#light

namespace RevitAddin

module RevitAddinModule

type RevitAddin() =
  interface Autodesk.Revit.IExternalCommand with
    member public this.Execute( cData, strMessage : string byref, aElements ) =
      strMessage <- "Hello world from RevitAddin.RevitAddinModule.RevitAddin"
      Autodesk.Revit.IExternalCommand.Result.Failed

