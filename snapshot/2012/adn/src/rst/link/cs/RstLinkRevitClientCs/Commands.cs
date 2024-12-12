#region Header
// RstLink
//
// Copyright (C) 2007-2010 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using W = System.Windows.Forms;
using System.Xml.Serialization;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using RstLink;
#endregion // Namespaces

namespace RstLinkRevitClient
{
  #region Integer built-in category constants
  class IntBic
  {
    public const int Columns = ( int ) BuiltInCategory.OST_StructuralColumns;
    public const int Framing = ( int ) BuiltInCategory.OST_StructuralFraming;
    public const int Foundation = ( int ) BuiltInCategory.OST_StructuralFoundation;
  }
  #endregion // Integer built-in category constants

  #region RsLinkExport command
  /// <summary>
  /// Export all structural elements to the RstLink intermediate file.
  /// Currently, only columns and framing is implemented - skeleton code is in place for others.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly)]
  public class RsLinkExport : IExternalCommand
  {
    #region GetStructuralElements
    /// <summary>
    /// Retrieve all structural elements in the given document.
    /// 
    /// In addition to the explicit filtering demonstrated here, 
    /// the StructuralInstanceUsageFilter might also retrieve
    /// exactly the desired elements with no further ado. 
    /// 
    /// The StructuralMaterialTypeFilter might also be useful.
    /// </summary>
    static 
    FilteredElementCollector GetStructuralElements(
      Document doc )
    {
      // what categories of family instances
      // are we interested in?

      BuiltInCategory[] bics = new BuiltInCategory[] {
        BuiltInCategory.OST_StructuralColumns,
        BuiltInCategory.OST_StructuralFraming,
        BuiltInCategory.OST_StructuralFoundation
      };

      IList<ElementFilter> a
        = new List<ElementFilter>( bics.Count() );

      foreach( BuiltInCategory bic in bics )
      {
        a.Add( new ElementCategoryFilter( bic ) );
      }

      LogicalOrFilter categoryFilter
        = new LogicalOrFilter( a );

      LogicalAndFilter familyInstanceFilter
        = new LogicalAndFilter( categoryFilter,
          new ElementClassFilter(
            typeof( FamilyInstance ) ) );

      IList<ElementFilter> b
        = new List<ElementFilter>( 6 );

      b.Add( new ElementClassFilter( 
        typeof( Wall ) ) );

      b.Add( new ElementClassFilter( 
        typeof( Floor ) ) );

      b.Add( new ElementClassFilter( 
        typeof( ContFooting ) ) );

      b.Add( new ElementClassFilter( 
        typeof( FamilyInstance ) ) );

      b.Add( new ElementClassFilter( 
        typeof( PointLoad ) ) );

      b.Add( new ElementClassFilter( 
        typeof( LineLoad ) ) );

      b.Add( new ElementClassFilter( 
        typeof( AreaLoad ) ) );

      b.Add( familyInstanceFilter );

      LogicalOrFilter classFilter
        = new LogicalOrFilter( b );

      FilteredElementCollector collector
        = new FilteredElementCollector( doc );

      collector.WherePasses( classFilter );

      return collector;
    }
    #endregion // GetStructuralElements

    /// <summary>
    /// Export helper to create an RSMember instance from a 
    /// family instance representing a structural member.
    /// </summary>
    static RSMember CreateRSMember(
      FamilyInstance fi,
      AnalyticalModel anaFrame )
    {
      try
      {
        int id = fi.Id.IntegerValue;
        Line line = anaFrame.GetCurve() as Line;
        string type = fi.Symbol.Name;
        string usage = null;
        if( fi.Category.Name == "Structural Columns" )
        {
          usage = "Column";
        }
        else
        {
          // This doesn't work any longer in structure 3! (was OK in 2?)
          //usage = fi.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_TEXT_PARAM).AsString
          // Now must get the integer enumeration and map the name
          //124334  6	Other
          //124340  3	Girder
          //124349  5	Purlin
          //134865  6	Other
          //129463  7	Vertical Bracing
          //124337  3	Girder
          //124331  6	Other
          //124346  8	Horizontal Bracing
          //124343  4	Joist
          //129409  9	Kicker Bracing
          try
          {
            switch( fi.get_Parameter( BuiltInParameter.INSTANCE_STRUCT_USAGE_PARAM ).AsInteger() )
            {
              case 3:
                usage = "Girder";
                break;
              case 4:
                usage = "Joist";
                break;
              case 5:
                usage = "Purlin";
                break;
              case 6:
                usage = "Other";
                break;
              case 7:
                usage = "Vertical Bracing";
                break;
              case 8:
                usage = "Horizontal Bracing";
                break;
              case 9:
                usage = "Kicker Bracing";
                break;
              default:
                usage = "Unknown";
                break;
            }
          }
          catch( Exception ex )
          {
            usage = "Parameter Fails";
          }
        }
        XYZ p1 = line.get_EndPoint( 0 );
        XYZ q1 = line.get_EndPoint( 1 );
        RSPoint p = new RSPoint( p1.X, p1.Y, p1.Z );
        RSPoint q = new RSPoint( q1.X, q1.Y, q1.Z );
        RSLine l = new RSLine( p, q );
        RSMember m = new RSMember( id, usage, type, l );
        return m;
      }
      catch( Exception ex )
      {
        return null;
      }
    }

    /// <summary>
    /// Export all structural members in the iven document 
    /// to an external file and return the user selected filename.
    /// </summary>
    public static string ExportMembers( Document doc, string filename )
    {
      // use an untyped collection to manage the elements:

      Hashtable rstElems = new Hashtable();

      // todo: rewrite using Dictionary instead, but how to serialize it?
      //HashSet<RSMember> rstElems = new HashSet<RSMember>();

      // todo: remove unneccessary try-catch handlers

      // select all applicable elements and add to the collection

      /*
      IList<ElementFilter> filters = new List<ElementFilter>();
      filters.Add( new ElementClassFilter( typeof( Wall ) ) );
      filters.Add( new ElementClassFilter( typeof( Floor ) ) );
      filters.Add( new ElementClassFilter( typeof( ContFooting ) ) );
      filters.Add( new ElementClassFilter( typeof( FamilyInstance ) ) );
      filters.Add( new ElementClassFilter( typeof( PointLoad ) ) );
      filters.Add( new ElementClassFilter( typeof( LineLoad ) ) );
      filters.Add( new ElementClassFilter( typeof( AreaLoad ) ) );
      LogicalOrFilter filter = new LogicalOrFilter( filters );

      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.WherePasses( filter );
      */

      FilteredElementCollector collector = GetStructuralElements( doc );

      foreach( Element elem in collector )
      {
        if( elem is Wall ) // Structural WALL
        {
          Wall w = elem as Wall;
          try
          {
            AnalyticalModel anaWall = w.GetAnalyticalModel();
            if( anaWall != null )
            {
              if( anaWall.GetCurves( AnalyticalCurveType.RawCurves ).Count > 0 )
              {
                //ToDo WALL
              }
            }
          }
          catch
          {
          }
        }
        else if( elem is Floor ) // Structural FLOOR
        {
          Floor f = elem as Floor;
          try
          {
            AnalyticalModel anaFloor = f.GetAnalyticalModel();

            if( anaFloor != null )
            {
              if( anaFloor.GetCurves( AnalyticalCurveType.RawCurves ).Count > 0 )
              {
                //ToDo FLOOR
              }
            }
          }
          catch
          {
          }
        }
        else if( elem is ContFooting ) // Structural CONTINUOUS FOOTING
        {
          ContFooting cf = elem as ContFooting;
          try
          {
            AnalyticalModel ana3D = cf.GetAnalyticalModel();

            if( ana3D != null )
            {
              if( ana3D.GetCurves( AnalyticalCurveType.RawCurves ).Count > 0 )
              {
                //ToDo CONT.FOOTING
              }
            }
          }
          catch
          {
          }
        }
        else if( elem is FamilyInstance ) // one of strucural standard families
        {
          try
          {
            FamilyInstance fi = elem as FamilyInstance;

            int iBic = fi.Category.Id.IntegerValue;

            if( ( iBic == IntBic.Columns ) || ( iBic == IntBic.Framing ) )
            {
              try
              {
                AnalyticalModel anaFrame = fi.GetAnalyticalModel();

                if( anaFrame != null )
                {
                  // Create MEMBER in neutral format and add it to the collection
                  RSMember member = CreateRSMember( fi, anaFrame );
                  if( member != null )
                  {
                    rstElems.Add( member, member );
                  }
                }
              }
              catch( Exception ex )
              {
                Debug.Print( ex.Message );
              }
            }
            else if( iBic == IntBic.Foundation ) // Case "Structural Foundations"
            {
              try
              {
                AnalyticalModel anaLoc = fi.GetAnalyticalModel();

                if( anaLoc != null )
                {
                  //ToDo FOUNDATION...also change hard-coded category name
                }
              }
              catch
              {
              }
            }
          }
          catch
          {
          }
        }
        else if( elem is PointLoad ) // ToDo: all LOADS!
        {
          //...
        }
        else if( elem is LineLoad )
        {
          //...
        }
        else if( elem is AreaLoad )
        {
          //...
        }
      }

      // serialize the members to a file

      if( 0 < rstElems.Count )
      {
        // select file to save

        W.SaveFileDialog dlg = new W.SaveFileDialog();
        dlg.Filter = "RstLink xml files (*.xml)|*.xml";
        dlg.Title = "RstLink - Revit EXPORT to AutoCAD";
        dlg.FileName = filename;
        if( dlg.ShowDialog() == W.DialogResult.OK )
        {
          // SOAP serialisation (would be faster if binary, but just to make it readable):

          FileStream fs = new FileStream( dlg.FileName, FileMode.Create );
          SoapFormatter sf = new SoapFormatter();
          sf.AssemblyFormat = FormatterAssemblyStyle.Simple;
          sf.Binder = new RstLink.RsLinkBinder();
          sf.Serialize( fs, rstElems );
          fs.Close();
          RstLink.Util.InfoMsg( string.Format(
            "Successfully exported {0} structural elements to '{1}'!",
            rstElems.Count, dlg.FileName ) );
          return dlg.FileName;
        }
        else
        {
          RstLink.Util.InfoMsg( "Command cancelled!" );
        }

        // NOTE: - (de)serialization works fine but all assemblies MUST be in the same folder as revit.EXE!
        // The same is true later when deserializing in AutoCAD - must put them in same folder with acad.EXE.
        // jeremy - i fixed this, no longer true
        //           SEE: Serialize the Collection to a file
        //           http://www.codeproject.com/soap/Serialization_Samples.asp
        //           Serialization(Headaches)
        //           http://www.dotnet4all.com/dotnet-code/2004/12/serialization-headaches.html
        //Try
        //    Dim fsTest As New FileStream("c:\temp\_RsLinkExport.xml", FileMode.Open)
        //    Dim sfTest As New SoapFormatter
        //    Dim elemsTest As Hashtable = sfTest.Deserialize(fsTest)
        //    fsTest.Close()
        //    MsgBox("Num.of DeSer = " & elemsTest.Count)
        //Catch ex As Exception
        //    MsgBox("Error in DeSer: " & ex.Message)
        //End Try
      }
      else
      {
        RstLink.Util.InfoMsg( "No Structural Elements found in this model!" );
      }
      return null;
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string msg,
      ElementSet els )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;
      string filename = "RstLinkRevitToAcad.xml";

      return null == ExportMembers( doc, filename )
        ? Result.Failed
        : Result.Succeeded;
    }
  }
  #endregion // Export

  #region RsLinkImport command
  /// <summary>
  /// Import incremental changes from the RstLink intermediate XML file
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  public class RsLinkImport : IExternalCommand
  {
    /// <summary>
    /// Helper to get specified type for a given category.
    /// Return a family symbol for a given category name and type.
    /// (in theory, we can have non-unique solution, i.e. the same 
    /// type name for more than one family from this category!)
    /// </summary>
    static FamilySymbol GetFamilySymbol(
      Document doc,
      BuiltInCategory bic,
      string typeName )
    {
      FilteredElementCollector collector = new FilteredElementCollector( doc );
      collector.OfClass( typeof( FamilySymbol ) );
      collector.OfCategory( bic );

      // todo: probably the type name can also be checked using a filter ... 
      // using a parameter filter, and equality ... what parameter is that?
      // or rewrite using a LINQ query? benchmark different approaches?

      foreach( Element e in collector )
      {
        if( e.Name.Equals( typeName ) )
        {
          return e as FamilySymbol;
        }
      }
      return null;
    }

    /// <summary>
    /// Import all structural members from a given 
    /// external file into the document.
    /// </summary>
    public static int ImportMembers(
      string filename,
      Document doc,
      bool showMessage )
    {
      FileStream fs = new FileStream( filename, FileMode.Open );
      SoapFormatter sf = new SoapFormatter();
      sf.AssemblyFormat = FormatterAssemblyStyle.Simple;
      sf.Binder = new RsLinkBinder();
      Hashtable rstElems = sf.Deserialize( fs ) as Hashtable; // HashSet<RSMember>;
      fs.Close();

      using( StreamWriter log = new StreamWriter( filename + ".log" ) )
      {
        int iModified = 0;

        foreach( RSMember m in rstElems.Values )
        {
          if( 0 == m.revitId )
          {
            // ADD NEW elements...

            // MODIFY NEW Sizes (Types)...
          }
          else
          {
            ElementId id = new ElementId( m.revitId );
            FamilyInstance fi = doc.get_Element( id ) as FamilyInstance;

            if( null == fi )
            {
              log.WriteLine( "Member Id=" + m.revitId + " not found." );
            }
            else
            {
              // Check if the Type has changed (in theory we'd need to check Family as well)
              string newType = m._type;

              if( !fi.Symbol.Name.Equals( newType ) )
              {
                log.WriteLine( "Member Id=" + m.revitId + ": Type changed from " + fi.Symbol.Name + " to " + newType );

                BuiltInCategory bic = m._usage.Equals( "Column" )
                  ? BuiltInCategory.OST_StructuralColumns
                  : BuiltInCategory.OST_StructuralFraming;

                FamilySymbol newSymb = GetFamilySymbol( doc, bic, newType );

                if( newSymb == null )
                {
                  log.WriteLine( "  ERROR: Could not find the new Symbol loaded in Revit!" );
                }
                else
                {
                  try
                  {
                    fi.Symbol = newSymb;
                    log.WriteLine( "  Symbol SUCCESSFULLY changed!" );
                    iModified += 1;
                  }
                  catch( Exception ex )
                  {
                    log.WriteLine( "  ERROR: Could not change to the new Symbol!" );
                  }
                }
              }
            }
          }
        }
        if( showMessage )
        {
          RstLink.Util.InfoMsg( 
            "Successfully MODIFIED Types for " 
            + iModified + " structural members!" );
        }
        log.Close();
      }
      return rstElems.Count;
    }

    public Result Execute(
      ExternalCommandData commandData,
      ref string msg,
      ElementSet els )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      W.OpenFileDialog dlg = new W.OpenFileDialog();

      try
      {
        // select file to open:

        dlg.Filter = "RstLink xml files (*.xml)|*.xml";
        dlg.Title = "RstLink - Revit IMPORT from AutoCAD";

        if( dlg.ShowDialog() == W.DialogResult.OK )
        {
          // de-serialize the result file:

          int n = ImportMembers( dlg.FileName, doc, true );

          if( 0 == n )
          {
            RstLink.Util.InfoMsg( "No elements found in the result file!" );
            return Result.Cancelled;
          }
          return Result.Succeeded;
        }
        else
        {
          RstLink.Util.InfoMsg( "Command cancelled!" );
          return Result.Failed;
        }
      }
      catch( Exception ex )
      {
        RstLink.Util.InfoMsg( "Error while deserializing: " + ex.Message );
        return Result.Failed;
      }
    }
  }
  #endregion // Import
}
