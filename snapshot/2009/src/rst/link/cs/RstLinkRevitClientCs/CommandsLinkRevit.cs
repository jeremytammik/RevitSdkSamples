#region Header
// RstLink
//
// Copyright (C) 2007-2008 by Autodesk, Inc.
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
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using W = System.Windows.Forms;
using System.Xml.Serialization;
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Symbols;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using Line = Autodesk.Revit.Geometry.Line;
using RstLink;
#endregion // Namespaces

namespace RstLinkRevitClient
{
  #region Import
  /// <summary>
  /// Import incremental changes from the RstLink intermediate XML file
  /// </summary>
  public class RSLinkImport : IExternalCommand
  {
    public CmdResult Execute( 
      ExternalCommandData commandData, 
      ref string msg, 
      ElementSet els )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Hashtable rstElems = null; // todo: rewrite using Dictionary<>

      // De-serialize the result file 
      W.OpenFileDialog dlg = new W.OpenFileDialog();
      try
      {
        // Select File to Open
        dlg.Filter = "RstLink xml files (*.xml)|*.xml";
        dlg.Title = "RstLink - Revit IMPORT from AutoCAD";
        if( dlg.ShowDialog() == W.DialogResult.OK )
        {
          FileStream fs = new FileStream( dlg.FileName, FileMode.Open );
          SoapFormatter sf = new SoapFormatter();
          sf.AssemblyFormat = FormatterAssemblyStyle.Simple;
          sf.Binder = new RsLinkBinder();
          rstElems = sf.Deserialize( fs ) as Hashtable;
          fs.Close();
        }
        else
        {
          RstLink.Util.InfoMsg("Command cancelled!");
          return CmdResult.Failed;
        }
      }
      catch (Exception ex)
      {
        RstLink.Util.InfoMsg("Error when deserializing: " + ex.Message);
        return CmdResult.Failed;
      }
      if (rstElems.Count == 0)
      {
        RstLink.Util.InfoMsg("No elements found in the result file!");
        return CmdResult.Cancelled;
      }

      try
      {
        StreamWriter log = new StreamWriter(dlg.FileName + ".log");
        int iModified = 0;

        // Itearate all results
        IDictionaryEnumerator iter = rstElems.GetEnumerator();
        while (iter.MoveNext())
        {
          RSMember m = iter.Value as RSMember;

          //ADD NEW elements 
          //================
          if (m.revitId == 0)
          {
            // In 8.1 API there are no ways to create new Families with Location as Curve, but Point only.
            // This has been addressed in 9.0 API - see eg CreateBeamsColumnsBraces sample in the SDK

            // Bonus: add new elements....

            //MODIFY NEW Sizes (Types)
            //=======================
          }
          else
          {
            ElementId id = new ElementId();
            id.Value = m.revitId;
            FamilyInstance fi = doc.get_Element(ref id) as FamilyInstance;

            // Check if the Type has changed (in theory we'd need to check Family as well)
            string newType = m._type;
            if (!(fi.Symbol.Name.Equals(newType)))
            {
              log.WriteLine("Member Id=" + m.revitId + ": Type changed from " + fi.Symbol.Name + " to " + newType);
              
              /*
              // 2008-style code:
              
              string catName = null;
              if( m._usage.Equals( "Column" ) )
              {
                catName = "Structural Columns";
              }
              else
              {
                catName = "Structural Framing";
              }
              FamilySymbol newSymb = GetFamilySymbol( doc, catName, newType );
              */
              BuiltInCategory bic = m._usage.Equals( "Column" )
                ? BuiltInCategory.OST_StructuralColumns
                : BuiltInCategory.OST_StructuralFraming;
              FamilySymbol newSymb = GetFamilySymbol( app, bic, newType );

              if (newSymb == null)
              {
                log.WriteLine("  ERROR: Could not find the new Symbol loaded in RVT!");
              }
              else
              {
                try
                {
                  fi.Symbol = newSymb;
                  log.WriteLine("  Symbol SUSSESSFULLY changed!");
                  iModified += 1;
                }
                catch (Exception ex)
                {
                  log.WriteLine("  ERROR: Could not change to the new Symbol ?!");
                }
              }
            }
          }
        }
        RstLink.Util.InfoMsg("Successfully MODIFIED Types for " + iModified + " structural members!");
        log.Close();
      }
      catch (Exception ex)
      {
        RstLink.Util.InfoMsg("Error when processing: " + ex.Message);
        return CmdResult.Failed;
      }
      finally
      {
      }
      return CmdResult.Succeeded;
    }

    #region 2008
    /// <summary>
    /// Return a FamilySymbol for a given category name and type.
    /// (in theory, we can have non-unique solution, i.e. the same 
    /// type name for more than one family from this category!)
    /// </summary>
    public static FamilySymbol GetFamilySymbol_2008( Document doc, string catName, string typeName )
    {
      ElementIterator iter = doc.Elements;
      while (iter.MoveNext())
      {
        Element elem = iter.Current as Element;

        // We got a Family
        if (elem is Family)
        {
          Family fam = elem as Family;
          // If we have a match on Category name, loop all its types for the other match
          try
          {
            // we CANNOT use this, since Category is not implemented for Family objects:
            // If fam.Category.Name.Equals(catName) Then
            foreach( FamilySymbol sym in fam.Symbols )
            {
              if( sym.Name.Equals( typeName ) )
              {
                if( sym.Category.Name.Equals( catName ) ) // must use it here - slightly more inefficient
                {
                  return sym;
                }
              }
            }
          }
          catch
          {
          }
        }
      }
      // if here - haven't got it!
      return null;
    }
    #endregion // 2008

    /// <summary>
    /// Helper to get specified type for a given category.
    /// Return a FamilySymbol for a given category name and type.
    /// (in theory, we can have non-unique solution, i.e. the same 
    /// type name for more than one family from this category!)
    /// </summary>
    public static FamilySymbol GetFamilySymbol(
      Application app,
      BuiltInCategory bic,
      string typeName )
    {
      Filter filterType = app.Create.Filter.NewTypeFilter( typeof( FamilySymbol ) );
      Filter filterCategory = app.Create.Filter.NewCategoryFilter( bic );
      Filter filter = app.Create.Filter.NewLogicAndFilter( filterType, filterCategory );
      List<Element> a = new List<Element>();
      int n = app.ActiveDocument.get_Elements( filter, a );
      // todo: probably the type name can also be checked using a filter ... 
      // using a parameter filter, and equality ... what parameter is that?
      foreach( Element e in a )
      {
        if( e.Name.Equals( typeName ) )
        {
          return e as FamilySymbol;
        }
      }
      return null;
    }
  }
  #endregion // Import

  #region Export
  /// <summary>
  /// Export all structural elements to the RstLink intermediate file.
  /// Currently, only columns and framing is implemented - skeleton code is in place for others.
  /// </summary>
  public class RSLinkExport : IExternalCommand
  {
    public CmdResult Execute(ExternalCommandData commandData, ref string msg, ElementSet els)
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Categories categories = doc.Settings.Categories;
      // From RS3, we can make sure it works in all locales, so use localized category names:
      Category catStruColums = categories.get_Item( BuiltInCategory.OST_StructuralColumns );
      Category catStruFraming = categories.get_Item( BuiltInCategory.OST_StructuralFraming );
      Category catStruFoundation = categories.get_Item( BuiltInCategory.OST_StructuralFoundation );
      
      // No Dictionary was available in .NET 2003, so used untyped collection. If doing again in 2005 - better use Dictionary
      Hashtable rstElems = new Hashtable(); // todo: rewrite using Dictionary<>

      // todo: remove unneccessary try-catch handlers
      // todo: rewrite using 2009 filtering

      // LOOP all elements and add to the collection
      ElementIterator iter = doc.Elements;
      while (iter.MoveNext())
      {
        Element elem = iter.Current as Element;

        if( elem is Wall ) // Strucural WALL
        {
          Wall w = elem as Wall;
          try
          {
            AnalyticalModelWall anaWall = w.AnalyticalModel as AnalyticalModelWall;
            if (anaWall != null)
            {
              if (anaWall.Curves.Size > 0)
              {
                //ToDo WALL
              }
            }
          }
          catch
          {
          }
        }
        else if( elem is Floor ) // Strucural FLOOR
        {
          Floor f = elem as Floor;
          try
          {
            AnalyticalModelFloor anaFloor = f.AnalyticalModel as AnalyticalModelFloor;
            if (anaFloor != null)
            {
              if (anaFloor.Curves.Size > 0)
              {
                //ToDo FLOOR
              }
            }
          }
          catch
          {
          }
        }
        else if( elem is ContFooting ) // Strucural CONTINUOUS FOOTING
        {
          ContFooting cf = elem as ContFooting;
          try
          {
            AnalyticalModel3D ana3D = cf.AnalyticalModel as AnalyticalModel3D;
            if (ana3D != null)
            {
              if (ana3D.Curves.Size > 0)
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
            //INSTANT C# NOTE: The following VB 'Select Case' included range-type or non-constant 'Case' expressions and was converted to C# 'if-else' logic:
            //			  Select Case fi.Category.Name

            // From RS3, better use local-independent design
            // Case "Structural Columns", "Structural Framing"
            //ORIGINAL LINE: Case catStruColums.Name, catStruFraming.Name
            if ((fi.Category.Name == catStruColums.Name) || (fi.Category.Name == catStruFraming.Name))
            {
              try
              {
                AnalyticalModelFrame anaFrame = fi.AnalyticalModel as AnalyticalModelFrame;
                if (anaFrame != null)
                {
                  // Create MEMBER in neutral format and add it to the collection
                  RSMember member = CreateRSMember(fi, anaFrame);
                  if (member != null)
                  {
                    rstElems.Add(member, member);
                  }
                }
              }
              catch
              {
              }
            }
              // todo: do not compary category name, use category id instead
            else if( fi.Category.Name == catStruFoundation.Name ) // Case "Structural Foundations"
            {
              try
              {
                AnalyticalModelLocation anaLoc = fi.AnalyticalModel as AnalyticalModelLocation;
                if (anaLoc != null)
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
        else if (elem is LineLoad)
        {
          //...
        }
        else if (elem is AreaLoad)
        {
          //...
        }
      }

      // Serialize the members to a file
      if( 0 < rstElems.Count )
      {
        // Select File to Save
        W.SaveFileDialog dlg = new W.SaveFileDialog();
        dlg.Filter = "RstLink xml files (*.xml)|*.xml";
        dlg.Title = "RstLink - Revit EXPORT to AutoCAD";
        dlg.FileName = "RstLinkRevitToAcad.xml";
        if (dlg.ShowDialog() == W.DialogResult.OK)
        {
          //SOAP (would be faster if BINARY, but just to make it readable)
          FileStream fs = new FileStream(dlg.FileName, FileMode.Create);
          SoapFormatter sf = new SoapFormatter();
          sf.AssemblyFormat = FormatterAssemblyStyle.Simple;
          sf.Binder = new RstLink.RsLinkBinder();
          sf.Serialize(fs, rstElems);
          fs.Close();
          RstLink.Util.InfoMsg("Successfully exported " + rstElems.Count + " structural elements!");
        }
        else
        {
          RstLink.Util.InfoMsg("Command cancelled!");
          return CmdResult.Cancelled;
        }

        // NOTE: - (de)serialization works fine but all assemblies MUST be in the same folder as revit.EXE!
        // The same is true later when deserializing in AutoCAD - must put them in same folder with acad.EXE.
        // jeremy - i fixed this, no longer true
        //           SEE: Serialize the Collection to a file
        //           http://www.codeproject.com/soap/Serialization_Samples.asp
        //           Serialization(Headaches)
        //           http://www.dotnet4all.com/dotnet-code/2004/12/serialization-headaches.html
        //Try
        //    Dim fsTest As New FileStream("c:\temp\_RSLinkExport.xml", FileMode.Open)
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
        RstLink.Util.InfoMsg("No Structural Elements found in this model!");
      }
      return CmdResult.Succeeded;
    }

    public RSMember CreateRSMember( FamilyInstance fi, AnalyticalModelFrame anaFrame )
    {
      try
      {
        int id = fi.Id.Value;
        Line line = anaFrame.Curve as Line;
        string type = fi.Symbol.Name;
        string usage = null;
        if (fi.Category.Name == "Structural Columns")
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
            switch (fi.get_Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_PARAM).AsInteger())
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
          catch (Exception ex)
          {
            usage = "Parameter Fails";
          }
          //Dim p1 As Parameter = fi.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_TEXT_PARAM)
          //If Not p1 Is Nothing Then
          //    MsgBox(p1.StorageType.ToString)
          //End If

          //Dim p2 As Parameter = fi.Parameter(BuiltInParameter.INSTANCE_STRUCT_USAGE_PARAM)
          //If Not p2 Is Nothing Then
          //    MsgBox(p2.StorageType.ToString)
          //End If
        }
        RSPoint p = new RSPoint( line.get_EndPoint( 0 ).X, line.get_EndPoint( 0 ).Y, line.get_EndPoint( 0 ).Z );
        RSPoint q = new RSPoint( line.get_EndPoint( 1 ).X, line.get_EndPoint( 1 ).Y, line.get_EndPoint( 1 ).Z );
        RSLine l = new RSLine( p, q );
        RSMember m = new RSMember( id, usage, type, l );
        return m;
      }
      catch( Exception ex )
      {
        return null;
      }
    }
  }
  #endregion // Export
}
