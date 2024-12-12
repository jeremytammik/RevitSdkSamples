//
// RSLinkRevitApp.RsApp - Revit Structure Link external application class
//
// Jeremy Tammik, Autodesk Inc., 2007-03-28
//
using Autodesk.Revit;

namespace RSLinkRevitApp
{
  /// <summary>
  /// Revit Structure Link external application class.
  /// Defines a minimal UI for the export and import commands.
  /// </summary>
  public class RsApp : IExternalApplication
  {
    public const string Title = "Revit Structure Link";

    #region IExternalApplication Members

    IExternalApplication.Result IExternalApplication.OnShutdown( ControlledApplication application )
    {
      return IExternalApplication.Result.Succeeded;
    }

    IExternalApplication.Result IExternalApplication.OnStartup( ControlledApplication application )
    {
      try
      {
        string appAssemblyName = "RSLinkRevitApp";
        string appDllName = appAssemblyName + ".dll";
        string cmdNamespace = "RSLinkRevitClient";
        string appPath = this.GetType().Assembly.Location;
        string cmdPath = appPath.Replace( appDllName, "..\\..\\..\\RSLinkRevitClient\\bin\\RSLinkRevitClient.dll" );
        string imgPath = appPath.Replace( appDllName, "..\\..\\toolbar.bmp" );
        //
        // create a custom tool bar and set its image path:
        //
        Autodesk.Revit.Toolbar toolBar = application.CreateToolbar();
        toolBar.Image = imgPath;
        toolBar.Name = Title;
        //
        // add export button:
        //
        ToolbarItem item = toolBar.AddItem( cmdPath, cmdNamespace + ".RSLinkExport" );
        item.ItemType = ToolbarItem.ToolbarItemType.BtnRText;
        item.ItemText = "Export";
        item.StatusbarTip = item.ToolTip = "Export analytical data from Revit to external analysis";
        //
        // add import button:
        //
        item = toolBar.AddItem( cmdPath, cmdNamespace + ".RSLinkImport" );
        item.ItemType = ToolbarItem.ToolbarItemType.BtnRText;
        item.ItemText = "Import";
        item.StatusbarTip = item.ToolTip = "Import analytical data from external analysis to Revit";
        //
        // add help button:
        //
        item = toolBar.AddItem( appPath, "RSLinkRevitApp.CommandAbout" );
        item.ItemType = ToolbarItem.ToolbarItemType.BtnStd;
        item.StatusbarTip = item.ToolTip = "About " + Title;
      }
      catch( System.Exception ex )
      {
        System.Windows.Forms.MessageBox.Show( ex.Message, Title );
        return IExternalApplication.Result.Failed;
      }
      return IExternalApplication.Result.Succeeded;
    }
    #endregion // IExternalApplication Members
  }

  /// <summary>
  /// Revit external command class to display "About..." message.
  /// </summary>
  public class CommandAbout : IExternalCommand
  {
    #region IExternalCommand Members
    public IExternalCommand.Result Execute( ExternalCommandData revit, ref string message, ElementSet elements )
    {
      string a
        = "Sample application to demonstrate linking Revit Structure with an external stress analysis application."
        + "\n\nCopyright (C) 2007 by Miro Schonauer and Jeremy Tammik, Autodesk Inc.";
      System.Windows.Forms.MessageBox.Show( a, RsApp.Title );
      return IExternalCommand.Result.Succeeded;
    }
    #endregion // IExternalCommand Members
  }
}
