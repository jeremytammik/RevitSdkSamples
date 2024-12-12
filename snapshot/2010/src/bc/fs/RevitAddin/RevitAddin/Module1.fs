#light

namespace RevitAddin

open Autodesk.Revit

type RevitAddin() =
  interface Autodesk.Revit.IExternalCommand with
    member public this.Execute( cData, strMessage : string byref, aElements ) =
      strMessage <- "Hello world from RevitAddin.RevitAddin"
      Autodesk.Revit.IExternalCommand.Result.Failed
