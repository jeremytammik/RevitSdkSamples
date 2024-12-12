#region Header
// Revit API .NET Labs
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
using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Parameters;
using Autodesk.Revit.Structural;
using Autodesk.Revit.Utility;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace RstLabs
{
  // Full analytical model listed for *selected* structural elements
  public class Lab3 : IExternalCommand
  {
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Document doc = commandData.Application.ActiveDocument;
      Category CatStruCols = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralColumns);
      Category CatStruFrmg = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFraming);
      Category CatStruFndt = doc.Settings.Categories.get_Item(BuiltInCategory.OST_StructuralFoundation);
      ElementSetIterator iter = doc.Selection.Elements.ForwardIterator();
      string sMsg = null;

      while (iter.MoveNext())
      {
        Autodesk.Revit.Element elem = (Autodesk.Revit.Element)iter.Current;
        // Structural WALL
        if (elem is Wall)
        {
          Wall w = (Wall)elem;
          try
          {
            AnalyticalModelWall anaWall = (AnalyticalModelWall)w.AnalyticalModel;
            if ((anaWall != null) && (anaWall.Curves.Size > 0))
            {
              sMsg = "Analytical Model for Wall " + w.Id.Value.ToString() + "\r\n";
              this.GetAnalyticalModelWall(ref anaWall, ref sMsg);
              RacUtils.InfoMsg(sMsg);
            }
          }
          catch
          {

          }
        }
        // Strucural FLOOR
        else if (elem is Floor)
        {
          Floor f = (Floor)elem;
          try
          {
            AnalyticalModelFloor anaFloor = (AnalyticalModelFloor)f.AnalyticalModel;
            if ((anaFloor != null) && (anaFloor.Curves.Size > 0))
            {
              sMsg = "Analytical Model for Floor " + f.Id.Value.ToString() + "\r\n";
              this.GetAnalyticalModelFloor(ref anaFloor, ref sMsg);
              RacUtils.InfoMsg(sMsg);
            }
          }
          catch
          {

          }
        }
        // Strucural CONTINUOUS FOOTING
        else if (elem is ContFooting)
        {
          ContFooting cf = (ContFooting)elem;
          try
          {
            AnalyticalModel3D ana3D = (AnalyticalModel3D)cf.AnalyticalModel;
            if ((ana3D != null) && (ana3D.Curves.Size > 0))
            {
              sMsg = "Analytical Model for Continuous Footing " + cf.Id.Value.ToString() + "\r\n";
              this.GetAnalyticalModelContFooting(ref ana3D, ref sMsg);
              RacUtils.InfoMsg(sMsg);
            }
          }
          catch
          {

          }
        }
        else if (elem is FamilyInstance)
        {
          try
          {
            FamilyInstance fi = (FamilyInstance)elem;
            string strCatName = fi.Category.Name;

            if (strCatName == CatStruCols.Name) // "Structural Columns" for EN locale
            {
              try
              {
                AnalyticalModelFrame anaFrame = (AnalyticalModelFrame)fi.AnalyticalModel;
                if (anaFrame != null)
                {
                  sMsg = "Analytical Model for Structural Column " + fi.Id.Value.ToString() + "\r\n";
                  Curve cur = anaFrame.Curve;
                  this.ListCurve(ref cur, ref sMsg);
                  this.ListRigidLinks(ref anaFrame, ref sMsg);
                  AnalyticalSupportData supportData = anaFrame.SupportData;
                  this.ListSupportInfo(ref supportData, ref sMsg);
                  RacUtils.InfoMsg(sMsg);
                }
              }
              catch
              {

              }
            }
            if (strCatName == CatStruFrmg.Name) // "Structural Framing" for EN locale
            {
              try
              {
                AnalyticalModelFrame anaFrame = (AnalyticalModelFrame)fi.AnalyticalModel;
                if (anaFrame != null)
                {
                  sMsg = "Analytical Model for Structural Framing " + fi.StructuralType.ToString()
                    + " " + fi.Id.Value.ToString() + "\r\n";
                  Curve cur = anaFrame.Curve;
                  this.ListCurve(ref cur, ref sMsg);
                  this.ListRigidLinks(ref anaFrame, ref sMsg);
                  AnalyticalSupportData supportData = anaFrame.SupportData;
                  this.ListSupportInfo(ref supportData, ref sMsg);
                  RacUtils.InfoMsg(sMsg);
                }
              }
              catch
              {

              }
            }
            if (strCatName == CatStruFndt.Name)  // "Structural Foundations" for EN locale
            {
              try
              {
                AnalyticalModelLocation anaLoc = (AnalyticalModelLocation)fi.AnalyticalModel;
                if (anaLoc != null)
                {
                  XYZ pt = anaLoc.Point;
                  sMsg = "Analytical Model for Foundation " + fi.Id.Value.ToString()
                    + "\r\n  LOCATION = " + pt.X.ToString() + ", " + pt.Y.ToString() + ", " + pt.Z.ToString()
                    + "\r\n";
                  AnalyticalSupportData supportData = anaLoc.SupportData;

                  this.ListSupportInfo(ref supportData, ref sMsg);
                  RacUtils.InfoMsg(sMsg);
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
      }
      return CmdResult.Succeeded;
    }

    public void ListCurve(ref Curve crv, ref string s)
    {
      if ((crv is Autodesk.Revit.Geometry.Line))
      {
        // LINE
        Line line = crv as Autodesk.Revit.Geometry.Line;
        XYZ ptS = line.get_EndPoint(0);
        XYZ ptE = line.get_EndPoint(1);
        s = s + "  LINE:"
              + ptS.X + ", " + ptS.Y + ", " + ptS.Z + " ; "
              + ptE.X + ", " + ptE.Y + ", " + ptE.Z + "\r\n";
      }
      else if ((crv is Arc))
      {
        // ARC
        Arc arc = crv as Arc;
        XYZ ptS = arc.get_EndPoint(0);
        XYZ ptE = arc.get_EndPoint(1);
        double r = arc.Radius;
        s = s + "  ARC:"
              + ptS.X + ", " + ptS.Y + ", " + ptS.Z + " ; "
              + ptE.X + ", " + ptE.Y + ", " + ptE.Z
              + " ; R=" + r
              + "\r\n";
      }
      else
      {
        // GENERIC PARAMETRIC CURVE
        if (crv.IsBound)
        {
          s = s + "  BOUND CURVE " + crv.GetType().Name + " - Tessellated result:" + "\r\n";
          XYZArray pts = crv.Tessellate();

          foreach (XYZ pt in pts)
          {
            s = s + "  PT:"
                  + pt.X + ", " + pt.Y + ", "  + pt.Z + " ; "
                 + "\r\n";
          }
        }
        else
        {
          s = s + "  UNBOUND CURVE ??? - shouldn\'t ever be in an Analytical Model!" + "\r\n";
        }
      }
    }


    public void GetAnalyticalModelWall(ref AnalyticalModelWall anaWall, ref string s)
    {
      Curve crvTemp = null;
      foreach (Curve crv in anaWall.Curves)
      {
        crvTemp = crv;
        ListCurve(ref crvTemp, ref s);
      }
      AnalyticalSupportData supportData = anaWall.SupportData;
      ListSupportInfo(ref supportData, ref s);
    }

    public void GetAnalyticalModelFloor(ref AnalyticalModelFloor anaFloor, ref string s)
    {
      Curve crvTemp = null;
      foreach (Curve crv in anaFloor.Curves)
      {
        crvTemp = crv;
        ListCurve(ref crvTemp, ref s);
      }
      AnalyticalSupportData supportData = anaFloor.SupportData;
      ListSupportInfo(ref supportData, ref s);
    }

    public void GetAnalyticalModelContFooting(ref AnalyticalModel3D ana3d, ref string s)
    {
      Curve crvTemp = null;
      foreach (Curve crv in ana3d.Curves)
      {
        crvTemp = crv;
        ListCurve(ref crvTemp, ref s);
      }
      AnalyticalSupportData supportData = ana3d.SupportData;
      ListSupportInfo(ref supportData, ref s);
    }

    // New for RB3
    public void ListSupportInfo(ref AnalyticalSupportData supData, ref string s)
    {
      // If supData include valid support data.
      if (supData == null)
      {
        s = s + "There is no Support Data with this Element." + "\r\n";
      }
      else
      {
        // Supported or not?
        if (supData.Supported)
        {
          s = s + "\r\n" + "Supported = YES, by:" + "\r\n";
        }
        else
        {
          s = s + "\r\n" + "Supported = NO" + "\r\n";
          return;
        }
        // List all supports
        foreach (AnalyticalSupportInfo supInfo in supData.InfoArray)
        {
          Autodesk.Revit.Element supEl = supInfo.Element;
          s = s + "  " + supInfo.SupportType.ToString()
                 + " from elem.id=" + supEl.Id.Value.ToString()
                 + " cat=" + supEl.Category.Name
                 + "\r\n";
        }
      }
    }

    // New for RB3
    public void ListRigidLinks(ref AnalyticalModelFrame anaFrm, ref string s)
    {
      s = s + "\r\n" + "Rigid Link START = ";
      Curve rigidLinkStart = anaFrm.get_RigidLink(0);
      if (rigidLinkStart == null)
      {
        s = s + "None" + "\r\n";
      }
      else
      {
        ListCurve(ref rigidLinkStart, ref s);
      }
      s += "Rigid Link END   = ";
      Curve rigidLinkEnd = anaFrm.get_RigidLink(1);
      if ((rigidLinkEnd == null))
      {
        s = s + "None" + "\r\n";
      }
      else
      {
        ListCurve(ref rigidLinkEnd, ref s);
      }
    }
  }
}
