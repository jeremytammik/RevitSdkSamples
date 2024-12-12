#region Header
// RstLink
//
// Copyright (C) 2007-2013 by Autodesk, Inc.
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
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Soap;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using A = Autodesk.AutoCAD.ApplicationServices;
using RstLink;
#endregion // Namespaces

namespace RstLinkAcadClient
{
  public class AcadCommands
  {
    #region RSMakeMember
    [CommandMethod("RsLink", "RSMakeMember", CommandFlags.Modal | CommandFlags.UsePickSet)]
    public void MakeMember()
    {
      A.Document doc = A.Application.DocumentManager.MdiActiveDocument;
      Editor ed = doc.Editor;

      // Get Selection Set of Line entities
      //-----------------------------------
      // Message (optional)
      PromptSelectionOptions optSS = new PromptSelectionOptions();
      optSS.MessageForAdding = "Select Lines to stamp as RST Members ";
      // Selection Filter (optional)
      TypedValue[] selRB = new TypedValue[1];
      selRB[0] = new TypedValue(0, "LINE");
      SelectionFilter filterSS = new SelectionFilter(selRB);
      //  Select
      PromptSelectionResult resSS = ed.GetSelection(optSS, filterSS);
      if (resSS.Status != PromptStatus.OK)
      {
        ed.WriteMessage("Selection error - aborting command");
        return;
      }

      // Loop them and add RSMember Xdata for each (skip if already exist)
      //-----------
      Transaction tr = doc.TransactionManager.StartTransaction();

      try
      {
        // Loop selected and change
        //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#
        //				SelectedObject selObj = null;
        foreach (SelectedObject selObj in resSS.Value)
        {
          Line ln = (Line)(tr.GetObject(selObj.ObjectId, OpenMode.ForWrite));
          if (ln.GetXDataForApplication("RSMember") != null)
          {
            ed.WriteMessage("Line " + ln.Handle.ToString() + " is ALREADY an RST Member!" + "\n");
          }
          else
          {
            // Set RSMember XData
            ln.XData = new ResultBuffer(new TypedValue(1001, "RSMember"), new TypedValue(1071, 0), new TypedValue(1070, 0), new TypedValue(1000, "NEW"));
            ed.WriteMessage("Line " + ln.Handle.ToString() + " successfully stamped as new RST Member!" + "\n");
          }

        }

        tr.Commit();
      }
      catch (System.Exception ex)
      {
        RstLink.Util.InfoMsg( "Error? :" + ex.Message );
      }
      finally
      {
        tr.Dispose();
      }
    }
    #endregion // RSMakeMember

    #region RSExport
    [CommandMethod( "RsLink", "RSExport", CommandFlags.Modal )]
    public void Export()
    {
      // Collection of intermediate RS objects
      Hashtable _RSelems = new Hashtable();

      // Loop ModelSpace 
      Database db = HostApplicationServices.WorkingDatabase;
      Transaction trans = db.TransactionManager.StartTransaction();
      try
      {
        BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
        BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead);
        IEnumerator btrIter = btr.GetEnumerator();
        while (btrIter.MoveNext())
        {
          // Check if Line with RSMember Xdata
          Entity ent = (Entity)trans.GetObject((ObjectId)btrIter.Current, OpenMode.ForRead);
          if (ent is Line)
          {
            Line l = (Line)ent;
            ResultBuffer xd = l.GetXDataForApplication("RSMember");
            if (xd != null)
            {
              //better put this in a util, but KIS
              int revitId = 0;
              short iUsage = 0;
              string sType = "UNKNOWN";
              // Loop result buffer for the values
              IEnumerator rbIter = xd.GetEnumerator();
              while (rbIter.MoveNext())
              {
                TypedValue tmpVal = (TypedValue)rbIter.Current;
                switch (tmpVal.TypeCode)
                {
                  case 1071:
                    revitId = (int)tmpVal.Value;
                    break;
                  case 1070:
                    iUsage = (short)tmpVal.Value;
                    break;
                  case 1000:
                    sType = (string)tmpVal.Value;
                    break;
                }
              }
              // Convert usage code to string
              string sUsage = "UNKNOWN";
              switch (iUsage)
              {
                case 0:
                  sUsage = "Other";
                  break;
                case 1:
                  sUsage = "Column";
                  break;
                case 2:
                  sUsage = "Girder";
                  break;
                case 3:
                  sUsage = "Joist";
                  break;
                case 4:
                  sUsage = "Vertical Bracing";
                  break;
                case 5:
                  sUsage = "Horizontal Bracing";
                  break;
              }

              RSMember rsm = new RSMember(revitId, sUsage, sType, new RSLine(new RSPoint(l.StartPoint.X, l.StartPoint.Y, l.StartPoint.Z), new RSPoint(l.EndPoint.X, l.EndPoint.Y, l.EndPoint.Z)));
              _RSelems.Add(rsm, rsm);
            }
          }
        }
        trans.Commit();

        // Serialize the RS objects
        if (_RSelems.Count > 0)
        {
          // Select File to Save
          SaveFileDialog dlg = new SaveFileDialog();
          dlg.Filter = "RstLink xml files (*.xml)|*.xml";
          dlg.Title = "RstLink - AutoCAD EXPORT to Revit";
          dlg.FileName = "RstLinkAcadToRevit.xml";
          if (dlg.ShowDialog() == DialogResult.OK)
          {

            FileStream fs = new FileStream(dlg.FileName, FileMode.Create);
            SoapFormatter sf = new SoapFormatter();
            sf.Serialize(fs, _RSelems);
            fs.Close();
            RstLink.Util.InfoMsg("Successfully Exported " + _RSelems.Count + " RST Members!");

          }
          else
          {
            RstLink.Util.InfoMsg("Command cancelled!");
            return;
          }
        }
        else
        {
          RstLink.Util.InfoMsg("No RST Members found in ModelSpace!");
        }

      }
      catch (System.Exception ex)
      {
        RstLink.Util.InfoMsg("Error in RBExport: " + ex.Message);
      }
      finally
      {
        trans.Dispose();
      }
    }
    #endregion // RSExport

    #region RSImport
    [CommandMethod( "RsLink", "RSImport", CommandFlags.Modal )]
    public void Import()
    {
      // Deserialize RSElements
      Hashtable _RSelems = null;
      try
      {
        // Select File to Open
        OpenFileDialog dlg = new OpenFileDialog();
        dlg.Filter = "RstLink xml files (*.xml)|*.xml";
        dlg.Title = "RstLink - AutoCAD IMPORT from Revit";
        if (dlg.ShowDialog() == DialogResult.OK)
        {
          FileStream fs = new FileStream(dlg.FileName, FileMode.Open);
          SoapFormatter sf = new SoapFormatter();
          sf.Binder = new RsLinkBinder();
          _RSelems = (Hashtable)sf.Deserialize(fs);
          fs.Close();
        }
        else
        {
          RstLink.Util.InfoMsg("Command cancelled!");
          return;
        }
        if (_RSelems.Count <= 0)
        {
          RstLink.Util.InfoMsg("No elements found!");
          return;
        }
        else
        {
          //MsgBox(_RSelems.Count)
        }
      }
      catch (System.Exception ex)
      {
        RstLink.Util.InfoMsg("Error " + ex.Message);
        return;
      }

      //Create Acad Entities...loop all intermediate objects and act based on the type
      //INSTANT C# NOTE: Commented this declaration since looping variables in 'foreach' loops are declared in the 'foreach' header in C#
      //			RsLinkElement elem = null;
      foreach (RsLinkElement elem in _RSelems.Values)
      {
        // MEMBER
        if (elem.GetType() == typeof(RSMember))
        {
          RSMember member = elem as RSMember;

          Database db = HostApplicationServices.WorkingDatabase;
          Transaction trans = db.TransactionManager.StartTransaction();
          try
          {
            // Crete new Line and set end points
            Line line = new Line(new Point3d(member._geom._StartPt.X, member._geom._StartPt.Y, member._geom._StartPt.Z), new Point3d(member._geom._EndPt.X, member._geom._EndPt.Y, member._geom._EndPt.Z));
            // Set specific layer
            string sLayerName = "RS " + member._usage;
            short iCol = 1;
            short iUsage = 1;
            switch (member._usage)
            {
              case "Column":
                iUsage = 1;
                iCol = 1;
                break;
              case "Girder":
                iUsage = 2;
                iCol = 2;
                break;
              case "Joist":
                iUsage = 3;
                iCol = 3;
                break;
              case "Vertical Bracing":
                iUsage = 4;
                iCol = 4;
                break;
              case "Horizontal Bracing":
                iUsage = 5;
                iCol = 5;
                break;
              case "Purlin":
                iUsage = 0;
                iCol = 6;
                break;
              case "Kicker Bracing":
                iUsage = 0;
                iCol = 7;
                break;
              case "Other":
                iUsage = 0;
                iCol = 8;
                break;
              default:
                iUsage = 0;
                iCol = 9;
                break;
            }
            line.LayerId = GetOrCreateLayer(sLayerName, iCol);

            // Add to BTR and commit the transaction
            BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);
            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
            btr.AppendEntity(line);
            trans.AddNewlyCreatedDBObject(line, true);

            // Make sure Xdata App is registered (not efficient here - KIS!)
            if (!(RegXdataApp("RSMember")))
            {
              RstLink.Util.InfoMsg("Cannot find or register Xdata App");
              return;
            }
            // Set RSMember XData
            line.XData = new ResultBuffer(new TypedValue(1001, "RSMember"), new TypedValue(1071, member.revitId), new TypedValue(1070, iUsage), new TypedValue(1000, member._type));
            trans.Commit();
          }
          catch (System.Exception ex)
          {
            RstLink.Util.InfoMsg("Error in Creating MEMBER: " + ex.Message);
          }
          finally
          {
            trans.Dispose();
          }

        }
      }
      RstLink.Util.InfoMsg("Successfully Imported " + _RSelems.Count + " RST Members!");

      // This works, but throws an exception which can be ignored?
      try
      {
        //AcadApp.DocumentManager.MdiActiveDocument.SendStringToExecute("_ZOOM _E ", True, True, False)
        A.Document doc = A.Application.DocumentManager.MdiActiveDocument;
        doc.SendStringToExecute("-view _SWISO ", true, true, false);
      }
      catch (System.Exception ex)
      {
      }
    }
    #endregion // RSImport

    #region Utility methods
    public bool RegXdataApp( string appName )
    {
      Database db = HostApplicationServices.WorkingDatabase;
      Transaction trans = db.TransactionManager.StartTransaction();
      try
      {
        RegAppTable regTable = (RegAppTable)trans.GetObject(db.RegAppTableId, OpenMode.ForWrite);
        if (regTable.Has(appName))
        {
          return true;
        }
        else
        {
          RegAppTableRecord regTableRec = new RegAppTableRecord();
          regTableRec.Name = appName;
          regTable.Add(regTableRec);
          trans.AddNewlyCreatedDBObject(regTableRec, true);
        }
        trans.Commit();
      }
      catch (System.Exception ex)
      {
        return false;
      }
      finally
      {
        trans.Dispose();
      }
      return true;
    }

    public ObjectId GetOrCreateLayer(string layerName, short iCol)
    {
      ObjectId layerId = new ObjectId(); //the return value for this function
      Database db = HostApplicationServices.WorkingDatabase;
      Transaction trans = db.TransactionManager.StartTransaction();

      try
      {
        //Get the layer table first...
        LayerTable lt = (LayerTable)trans.GetObject(db.LayerTableId, OpenMode.ForWrite);

        if (lt.Has(layerName)) //Check if it exists...
        {
          layerId = lt[layerName];
        }
        else //If not, create the layer here.
        {
          LayerTableRecord ltr = new LayerTableRecord();
          ltr.Name = layerName;
          ltr.Color = Color.FromColorIndex(ColorMethod.ByAci, iCol);
          layerId = lt.Add(ltr);
          trans.AddNewlyCreatedDBObject(ltr, true);
        }
        trans.Commit();
      }
      catch (System.Exception ex)
      {
        RstLink.Util.InfoMsg("Error in GetOrCreateLayer:" + ex.Message);
      }
      finally
      {
        trans.Dispose();
      }
      return layerId;
    }
    #endregion // Utility methods
  }
}
