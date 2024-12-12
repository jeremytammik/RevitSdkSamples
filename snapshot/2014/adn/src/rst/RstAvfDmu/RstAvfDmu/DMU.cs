#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace RstAvfDmu
{
  [Transaction( TransactionMode.Automatic )]
  public class AlignRebar : IExternalApplication
  {
    /// <summary>
    /// Starting at the given directory, search upwards for 
    /// a subdirectory with the given target name located
    /// in some parent directory. 
    /// </summary>
    /// <param name="path">Starting directory, e.g. GetDirectoryName( GetExecutingAssembly().Location ).</param>
    /// <param name="target">Target subdirectory name, e.g. "Images".</param>
    /// <returns>The full path of the target directory if found, else null.</returns>
    string FindFolderInParents( string path, string target )
    {
      Debug.Assert( Directory.Exists( path ),
        "expected an existing directory to start search in" );

      string s;

      do
      {
        s = Path.Combine( path, target );
        if( Directory.Exists( s ) )
        {
          return s;
        }
        path = Path.GetDirectoryName( path );
      } while( null != path );

      return null;
    }

    /// <summary>
    /// On shutdown, unregister the updater
    /// </summary>
    public Result OnShutdown( UIControlledApplication a )
    {
      RebarUpdater updater
        = new RebarUpdater( a.ActiveAddInId );

      UpdaterRegistry.UnregisterUpdater(
        updater.GetUpdaterId() );

      return Result.Succeeded;
    }

    /// <summary>
    /// On start up, add UI buttons, register 
    /// the updater and add triggers
    /// </summary>
    public Result OnStartup( UIControlledApplication a )
    {
      // Add the UI buttons on start up
      AddButtons( a );

      RebarUpdater updater = new RebarUpdater(
        a.ActiveAddInId );

      // Register the updater in the singleton 
      // UpdateRegistry class
      UpdaterRegistry.RegisterUpdater( updater );

      // Set the filter; in this case we 
      // shall work with beams specifically
      ElementCategoryFilter filter
        = new ElementCategoryFilter(
          BuiltInCategory.OST_StructuralFraming );

      // Add trigger 
      UpdaterRegistry.AddTrigger(
        updater.GetUpdaterId(), filter,
        Element.GetChangeTypeGeometry() );

      return Result.Succeeded;
    }

    /// <summary>
    /// Add UI buttons
    /// </summary>
    public void AddButtons( UIControlledApplication a )
    {
      // create a ribbon panel on the Analyze tab
      RibbonPanel panel = a.CreateRibbonPanel(
        Tab.Analyze, "RST Labs" );

      AddDmuCommandButtons( panel );
    }


    /// <summary>
    /// Control buttons for the Dynamic Model Update 
    /// </summary>
    public void AddDmuCommandButtons(
      RibbonPanel panel )
    {
      string path = GetType().Assembly.Location;

      string imagePath = FindFolderInParents( 
        Path.GetDirectoryName( path ), "Images" );

      // create toggle buttons for radio button group 

      ToggleButtonData toggleButtonData3
        = new ToggleButtonData(
          "RSTLabsDMUOff", "Align Off", path,
          "RstAvfDmu.UIDynamicModelUpdateOff" );

      toggleButtonData3.LargeImage = new BitmapImage( 
        new Uri( Path.Combine( imagePath, "Families.ico" ) ) );

      ToggleButtonData toggleButtonData4
        = new ToggleButtonData(
          "RSTLabsDMUOn", "Align On", path,
          "RstAvfDmu.UIDynamicModelUpdateOn" );

      toggleButtonData4.LargeImage = new BitmapImage(
        new Uri( Path.Combine( imagePath, "Families.ico" ) ) );

      // make dyn update on/off radio button group 

      RadioButtonGroupData radioBtnGroupData2 =
        new RadioButtonGroupData( "RebarAlign" );

      RadioButtonGroup radioBtnGroup2
        = panel.AddItem( radioBtnGroupData2 )
          as RadioButtonGroup;

      radioBtnGroup2.AddItem( toggleButtonData3 );
      radioBtnGroup2.AddItem( toggleButtonData4 );
    }
  }

  public class RebarUpdater : IUpdater
  {
    public static bool m_updateActive = false;
    AddInId addinID = null;
    UpdaterId updaterID = null;

    public RebarUpdater( AddInId id )
    {
      addinID = id;
      // UpdaterId that is used to register and 
      // unregister updaters and triggers
      updaterID = new UpdaterId( addinID, new Guid(
        "63CDBB88-5CC4-4ac3-AD24-52DD435AAB25" ) );
    }

    /// <summary>
    /// Align rebar to updated beam 
    /// </summary>
    public void Execute( UpdaterData data )
    {
      if( m_updateActive == false ) { return; }

      // Get access to document object
      Document doc = data.GetDocument();

      try
      {
        // Loop through all the modified elements
        foreach( ElementId id in
          data.GetModifiedElementIds() )
        {
          FamilyInstance beam = doc.GetElement( id )
            as FamilyInstance;

          // Create a filter to retrieve all rebars
          FilteredElementCollector rebars
            = new FilteredElementCollector( doc );

          rebars.OfCategory( BuiltInCategory.OST_Rebar );
          rebars.OfClass( typeof( Rebar ) );

          foreach( Rebar rebar in rebars )
          {
            // Calculate the beam line
            XYZ beamStartPoint = new XYZ();
            XYZ beamEndPoint = new XYZ();
            Line line = CalculateBeamLine( beam );

            // Get the start and end point 
            if( null != line )
            {
              beamStartPoint = line.GetEndPoint( 0 );
              beamEndPoint = line.GetEndPoint( 1 );
            }

            // To align the rebar to the new beam's 
            // length, we split the tasks in two stages
            // Step 1: Move the rebar to align with one 
            // of the end of the beam

            // For this we first access 
            // the rebar line geometry

            //Line rebarLine = rebar.Curves.get_Item( 
            //  0 ) as Line; // 2011

            //Line rebarLine = rebar.GetCenterlineCurves( 
            //  false )[0] as Line; // 2012

            Line rebarLine = rebar.GetCenterlineCurves(
              false, true, true )[0] as Line; // 2013

            // Calculate the translation vector 
            // (the extent of the move)
            XYZ transVec = new XYZ( beamStartPoint.X
              - rebarLine.GetEndPoint( 0 ).X, 0.0, 0.0 );

            // Perform the move 
            //doc.Move( rebar, transVec ); //2011
            ElementTransformUtils.MoveElement(doc, rebar.Id, transVec);  //2012

            // This move causes the beam line to change 
            // and so recalculating the beam line
            line = CalculateBeamLine( beam );

            // Step 2: Set the new length of the rebar 
            // based on new beam length. For this, we 
            // can set the relevant parameter after 
            // checking at UI
            rebar.get_Parameter( "B" ).Set(
              GetLength( line ) );
          }
        }
      }
      catch( Exception ex )
      {
        TaskDialog.Show( "Exception", ex.Message );
      }
    }

    // Calculate the beam line 
    private Line CalculateBeamLine(
      FamilyInstance beam )
    {
      GeometryElement geoElement
        = beam.get_Geometry( new Options() );

      if( null == geoElement )
      {
        throw new Exception(
          "Can't get the geometry of selected element." );
      }

      Line beamLine = null;

      foreach( GeometryObject geoObject in geoElement )
      {
        // get the driving path and vector of the beam 

        beamLine = geoObject as Line;

        if( null != beamLine )
        {
          return beamLine;
        }
      }
      return null;
    }

    /// <summary>
    /// Return the length of the given line
    /// </summary>
    public static double GetLength( Line line )
    {
      XYZ v = line.GetEndPoint( 1 ) - line.GetEndPoint( 0 );
      return v.GetLength();
    }

    /// <summary>
    /// Return the auxiliary string
    /// </summary>
    public string GetAdditionalInformation()
    {
      return "Automatically align rebar to match beam";
    }

    /// <summary>
    /// Set the priority
    /// </summary>
    public ChangePriority GetChangePriority()
    {
      return ChangePriority.Rebar;
    }

    /// <summary>
    /// Return the updater Id
    /// </summary>
    public UpdaterId GetUpdaterId()
    {
      return updaterID;
    }

    /// <summary>
    /// Return the updater name
    /// </summary>
    public string GetUpdaterName()
    {
      return "Rebar alignment updater";
    }
  }

  [Transaction( TransactionMode.ReadOnly )]
  public class UIDynamicModelUpdateOff : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      RebarUpdater.m_updateActive = false;
      return Result.Succeeded;
    }
  }

  [Transaction( TransactionMode.ReadOnly )]
  public class UIDynamicModelUpdateOn : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      RebarUpdater.m_updateActive = true;
      return Result.Succeeded;
    }
  }
}
