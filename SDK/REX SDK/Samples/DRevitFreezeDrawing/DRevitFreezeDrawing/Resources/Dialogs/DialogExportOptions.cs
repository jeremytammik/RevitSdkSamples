//
// (C) Copyright 2016 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.

using System;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using REX.Common;
using Autodesk.REX.Framework;

namespace REX.DRevitFreezeDrawing.Resources.Dialogs
{
   /// <summary>
   /// Export option dialog class
   /// </summary>
   public partial class DialogExportOptions : REXExtensionForm
   {
      int m_layMap;
      int m_lineScale;
      int m_units;
      int m_layAndProp;
      int m_coord;
      bool m_exportRooms;

      public DialogExportOptions(REXExtension argExt) : base(argExt)
      {
         InitializeComponent();
         init();
      }

      public void init()
      {
         combo_layMap.Items.Add(Resources.Strings.Texts.Default);
         combo_layMap.Items.Add(Resources.Strings.Texts.AIA);
         combo_layMap.Items.Add(Resources.Strings.Texts.CP38);
         combo_layMap.Items.Add(Resources.Strings.Texts.BS1192);
         combo_layMap.Items.Add(Resources.Strings.Texts.ISO13567);
         combo_layMap.SelectedIndex = 0;

         combo_lineScale.Items.Add(Resources.Strings.Texts.ModelSpace);
         combo_lineScale.Items.Add(Resources.Strings.Texts.PaperSpace);
         combo_lineScale.Items.Add(Resources.Strings.Texts.ViewScale);
         combo_lineScale.SelectedIndex = 0;

         combo_units.Items.Add(Resources.Strings.Texts.Milimiter);
         combo_units.Items.Add(Resources.Strings.Texts.Centimeter);
         combo_units.Items.Add(Resources.Strings.Texts.Meter);
         combo_units.Items.Add(Resources.Strings.Texts.Inch);
         combo_units.Items.Add(Resources.Strings.Texts.Foot);

         if (ThisExtension.Revit.CommandData().Application.ActiveUIDocument.Document.DisplayUnitSystem == Autodesk.Revit.DB.DisplayUnit.METRIC)
            combo_units.SelectedIndex = 2;
         else
            combo_units.SelectedIndex = 4;

         combo_layAndProp.Items.Add(Resources.Strings.Texts.ByEntity);
         combo_layAndProp.Items.Add(Resources.Strings.Texts.ByLayer);
         combo_layAndProp.Items.Add(Resources.Strings.Texts.NewLayer);
         combo_layAndProp.SelectedIndex = 0;

         combo_Coord.Items.Add(Resources.Strings.Texts.ProjectInternal);
         combo_Coord.Items.Add(Resources.Strings.Texts.Shared);
         combo_Coord.SelectedIndex = 0;

         check_exportRooms.Checked = false;

         m_coord = combo_Coord.SelectedIndex;
         m_layAndProp = combo_layAndProp.SelectedIndex;
         m_layMap = combo_layMap.SelectedIndex;
         m_lineScale = combo_lineScale.SelectedIndex;
         m_units = combo_units.SelectedIndex;

         m_exportRooms = check_exportRooms.Checked;

         this.Icon = ThisExtension.GetIcon();
      }

      private void button_OK_Click(object sender, EventArgs e)
      {
         m_coord = combo_Coord.SelectedIndex;
         m_layAndProp = combo_layAndProp.SelectedIndex;
         m_layMap = combo_layMap.SelectedIndex;
         m_lineScale = combo_lineScale.SelectedIndex;
         m_units = combo_units.SelectedIndex;

         m_exportRooms = check_exportRooms.Checked;

         this.Hide();
      }

      private void button_Cancel_Click(object sender, EventArgs e)
      {
         this.Hide();
      }

      private void DialogExportOptions_Load(object sender, EventArgs e)
      {
         combo_Coord.SelectedIndex = m_coord;
         combo_layAndProp.SelectedIndex = m_layAndProp;
         combo_layMap.SelectedIndex = m_layMap;
         combo_lineScale.SelectedIndex = m_lineScale;
         combo_units.SelectedIndex = m_units;

         check_exportRooms.Checked = m_exportRooms;
      }

      public DWGExportOptions GetExportOptions(bool argCopy)
      {
         DWGExportOptions setDwg = new DWGExportOptions();
         switch (m_units)
         {
            case 0:
               setDwg.TargetUnit = Autodesk.Revit.DB.ExportUnit.Millimeter;
               break;
            case 1:
               setDwg.TargetUnit = Autodesk.Revit.DB.ExportUnit.Centimeter;
               break;
            case 2:
               setDwg.TargetUnit = Autodesk.Revit.DB.ExportUnit.Meter;
               break;
            case 3:
               setDwg.TargetUnit = Autodesk.Revit.DB.ExportUnit.Inch;
               break;
            case 4:
               setDwg.TargetUnit = Autodesk.Revit.DB.ExportUnit.Foot;
               break;
            default:
               setDwg.TargetUnit = Autodesk.Revit.DB.ExportUnit.Foot;
               break;
         }

         switch (m_lineScale)
         {
            case 0:
               setDwg.LineScaling = Autodesk.Revit.DB.LineScaling.ModelSpace;
               break;
            case 1:
               setDwg.LineScaling = Autodesk.Revit.DB.LineScaling.PaperSpace;
               break;
            case 2:
               setDwg.LineScaling = Autodesk.Revit.DB.LineScaling.ViewScale;
               break;
         }

         setDwg.LayerMapping = combo_layMap.SelectedItem.ToString();

         switch (m_layAndProp)
         {
            case 0:
               setDwg.PropOverrides = Autodesk.Revit.DB.PropOverrideMode.ByEntity;
               break;
            case 1:
               setDwg.PropOverrides = Autodesk.Revit.DB.PropOverrideMode.ByLayer;
               break;
            case 2:
               setDwg.PropOverrides = Autodesk.Revit.DB.PropOverrideMode.NewLayer;
               break;
         }

         setDwg.ExportingAreas = m_exportRooms;

         if (argCopy)
         {
            switch (m_coord)
            {
               case 0:
                  setDwg.SharedCoords = false;
                  break;
               case 1:
                  setDwg.SharedCoords = true;
                  break;
            }
         }

         setDwg.ExportOfSolids = Autodesk.Revit.DB.SolidGeometry.Polymesh;

         return setDwg;
      }

      public Autodesk.Revit.DB.ImportUnit GetExportUnits(bool argCopy)
      {
         switch (m_units)
         {
            case 0:
               return Autodesk.Revit.DB.ImportUnit.Millimeter;
            case 1:
               return Autodesk.Revit.DB.ImportUnit.Centimeter;
            case 2:
               return Autodesk.Revit.DB.ImportUnit.Meter;
            case 3:
               return Autodesk.Revit.DB.ImportUnit.Inch;
            case 4:
               return Autodesk.Revit.DB.ImportUnit.Foot;
            default:
               return Autodesk.Revit.DB.ImportUnit.Foot;
         }
      }

      private void Help_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
      {
         REXCommand Cmd = new REXCommand(REXCommandType.Help);
         ThisExtension.Command(ref Cmd);
      }
   }
}