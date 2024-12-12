#region Header
// Revit Structure API Labs
//
// Copyright (C) 2010-2013 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System.Diagnostics;
#endregion // Namespaces

namespace NewRstLab
{
  /// <summary>
  /// This command achieves:
  /// Display the selected beam/brace’s analytical curve, support information
  /// </summary>
  #region RstLab3_1_GetAnalyticalModel
  [Transaction( TransactionMode.Manual )]
  public class RstLab3_1_GetAnalyticalModel : IExternalCommand
  {
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      Document doc = uiApp.ActiveUIDocument.Document;

      Transaction trans = new Transaction( doc, "RstLab3_1" );
      trans.Start();

      //Get a beam from the current selection
      FamilyInstance beam = null;
      BuiltInCategory builtCat = BuiltInCategory.OST_StructuralFraming;
      beam = RstUtils.GetFamilyInstanceBySelection( doc, uiApp, builtCat,
          "Please select a structural beam/brace" );
      if( null == beam )
      {
        messages = "Please select a beam before start this command";
        return Result.Failed;
      }

      string sMsg = null;
      //Get the analytical model of the beam.
      AnalyticalModel model = beam.GetAnalyticalModel();

      //Get the analytical curve for column.
      //
      if( model.IsSingleCurve() )
      {
        Curve colCurve = model.GetCurve();
        sMsg += "\n The structural beam's analytical model curve "
            + RstUtils.ListCurve( ref colCurve );
      }

      //get the approximated property
      if( model.IsApproximated() )
      {
        sMsg += "\n The analytical model of this beam is approximated";
      }
      else
      {
        sMsg += "\n The analytical model of this beam is non-approximated";
      }

      RstUtils.InfoMsg( sMsg );


      //Get the support data
      IList<AnalyticalModelSupport> supports = model.GetAnalyticalModelSupports();
      sMsg = string.Format( "\nThe beam has {0} support(s)", supports.Count );
      int nCounter = 0;
      foreach( AnalyticalModelSupport support in supports )
      {
        nCounter++;
        XYZ supportPt = support.GetPoint();
        sMsg += string.Format( "\n  support#{0}  point:{1}; support type: {2}",
            nCounter, RstUtils.PointString( supportPt ),
            support.GetSupportType().ToString() );
        ElementId idSupporter = support.GetSupportingElement();
        Element elemSupporter = doc.GetElement( idSupporter );
        sMsg += string.Format( "\n  support#{0} is provided by a(n) {1}",
            nCounter, elemSupporter.Category.Name.ToString() );
        sMsg += string.Format( "\n  support#{0}'s support priority is ",
            nCounter, support.GetPriority().ToString() );
      }
      RstUtils.InfoMsg( sMsg );

      trans.Commit();

      return Result.Succeeded;
    }
  }
  #endregion // RstLab3_1_GetColumnAnalyticalModel

  /// <summary>
  /// This command achieves:
  /// Display the rigid link curve of a selected beam.
  /// </summary>
  #region RstLab3_2_GetBeamRigidLink
  [Transaction( TransactionMode.ReadOnly )]
  public class RstLab3_2_GetBeamRigidLink : IExternalCommand
  {
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      Document doc = uiApp.ActiveUIDocument.Document;

      // Access rigid link from selected beam

      FamilyInstance beam = null;
      BuiltInCategory builtCat = BuiltInCategory.OST_StructuralFraming;
      
      beam = RstUtils.GetFamilyInstanceBySelection( doc, uiApp, builtCat,
          "Please select a structural beam to get its rigid link" );
      
      if( null == beam )
      {
        messages = "Please select a structural beam before start this command";
        return Result.Failed;
      }

      //Transaction trans = new Transaction( doc, "RstLab3_2" );
      //trans.Start();

      string sMsg = null;

      AnalyticalModel model = beam.GetAnalyticalModel();

      if( model.CanHaveRigidLinks() )
      {
        sMsg += "\r\nRigid Link START = ";
        AnalyticalModelSelector selector = new AnalyticalModelSelector(
            AnalyticalCurveSelector.StartPoint );
        Curve rigidLinkStart = model.GetRigidLink( selector );
        if( null == rigidLinkStart )
        {
          sMsg += "None\r\n";
        }
        else
        {
          sMsg += RstUtils.ListCurve( ref rigidLinkStart );
        }
        sMsg += "\r\nRigid Link END   = ";

        selector = new AnalyticalModelSelector( AnalyticalCurveSelector.EndPoint );
        Curve rigidLinkEnd = model.GetRigidLink( selector );
        if( null == rigidLinkEnd )
        {
          sMsg += "None\r\n";
        }
        else
        {
          sMsg += RstUtils.ListCurve( ref rigidLinkEnd );
        }
        RstUtils.InfoMsg( sMsg );
      }

      //trans.Commit();

      return Result.Succeeded;
    }
  }
  #endregion // RstLab3_2_GetBeamRigidLink


  /// <summary>
  /// This command achieves:
  /// 1.	Offset a selected column’s analytical model
  /// 2.	Change the vertical projection to top of physical for a selected column.
  /// </summary>
  #region RstLab3_3_EditAnalyticalModel
  [Transaction( TransactionMode.Manual )]
  public class RstLab3_3_EditAnalyticalModel : IExternalCommand
  {
    public Result Execute(
        ExternalCommandData commandData,
        ref string messages,
        ElementSet elements )
    {
      UIApplication uiApp = commandData.Application;
      Application app = uiApp.Application;
      Document doc = uiApp.ActiveUIDocument.Document;

      Transaction trans = new Transaction( doc, "RstLab3_3" );
      trans.Start();

      FamilyInstance column = null;
      AnalyticalModel model = null;

      try
      {
        //Get a column by selection to offset the analytical model
        BuiltInCategory builtCat = BuiltInCategory.OST_StructuralColumns;
        column = RstUtils.GetFamilyInstanceBySelection( doc, uiApp, builtCat,
            "Please select a column to offset its analytical model" );

        if( null == column )
        {
          messages = "No column is selected";
          return Result.Failed;
        }
        model = column.GetAnalyticalModel();

        // Some structural component's analytical model
        // can be offset, for instance column.
        // However some cannot be offset, for instance 
        // beam. So we need to check first if the 
        // analytical model can be offset.

        //if( model.CanSetAnalyticalOffset() ) // 2013 // this needs to be replaced

        try // 2014 // temporary workaround
        {
          // Move the analytical model 
          // one foot along X axis.

          model.SetOffset(
            AnalyticalElementSelector.Whole,
            new XYZ( 1, 0, 0 ) );
        }
        catch( Exception )
        {
          Debug.Print( 
            "Selected element cannot be offset." );
        }

        trans.Commit();
      }
      catch( Exception ex )
      {
        RstUtils.InfoMsg( "Offset analytical model failed: " + ex.Message );
        trans.RollBack();
      }

      //change vertical projection type
      //try
      //{
      //  column = RstUtils.GetFamilyInstanceBySelection(
      //      doc, uiApp, BuiltInCategory.OST_StructuralColumns,
      //      "Please select a column to change vertical projection type " );
      //  if( null == column )
      //  {
      //    messages = "No column is selected";
      //    return Result.Failed;
      //  }
      //  model = column.GetAnalyticalModel();

      //Change the projection type
      //model.SetAnalyticalProjectionType(AnalyticalElementSelector.EndOrTop, AnalyticalDirection.VerticalTop, AnalyticalProjectionType.AutoDetect); 
      //model.SetAnalyticalProjectionType(
      //    AnalyticalDirection.VerticalTop, AnalyticalProjectionType.Top );
      //doc.Regenerate();
      //Get a reference plane.
      //ReferencePlane rp = RstUtils.GetReferencePlaneBySelection(doc, uiApp,
      //    "Please select a reference plane for analytical projection");
      //set the horizontal projection plane.
      //ElementId id = rp.Id;
      //model.SetAnalyticalProjectionDatumPlane(AnalyticalDirection.Horizontal, id);

      //}

      //catch( Exception ex )
      //{
      //  RstUtils.InfoMsg("Failed to change projection type and projection plane\n"
      //      + ex.Message );
      //  trans.RollBack();
      //}

      return Result.Succeeded;
    }
  }
  #endregion // RstLab3_3_EditAnalyticalModel
}
