#light

module RevitAddinModule

type RevitAddin() =
  interface Autodesk.Revit.IExternalCommand with
    member public this.Execute( cData, strMessage : string byref, aElements ) =
      strMessage <- "Hello world from RevitAddinModule.RevitAddin"
      Autodesk.Revit.IExternalCommand.Result.Failed
