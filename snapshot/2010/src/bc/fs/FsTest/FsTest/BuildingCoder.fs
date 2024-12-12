#light

open System.Windows.Forms
open Autodesk.Revit

type public Command6() =
  interface IExternalCommand with
    member x.Execute( commandData, message, elements ) =
      MessageBox.Show( 
        "Kilroy was here", 
        "Building Coder F# Test" ) 
        |> ignore
      IExternalCommand.Result.Succeeded
  member public x.Execute( commandData, message : string byref, elements ) =
    (x :> IExternalCommand).Execute( commandData, ref message, elements )

type public Command() =
  member public x.Execute( cmdData : ExternalCommandData, message : string byref, elements : ElementSet ) =
    MessageBox.Show( 
      "Kilroy was here", 
      "Building Coder F# Test" ) 
      |> ignore
    IExternalCommand.Result.Succeeded
