//
// (C) Copyright 2003-2013 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.DoorSwing.CS
{
   /// <summary>
   /// A class inherit from Form is used to list all the door family exist in current project and 
   /// initialize each door type's Left/Right feature.
   /// </summary>
   public partial class InitializeForm : System.Windows.Forms.Form
   {
      DoorSwingData m_dataBuffer; 
      DoorGeometry m_currentGraphic;

      /// <summary>
      /// constructor without any argument.
      /// </summary>
      private InitializeForm()
      {
         InitializeComponent();
      }

      /// <summary>
      /// constructor overload.
      /// </summary>
      /// <param name="dataBuffer"> one reference of DoorSwingData.</param>
      public InitializeForm(DoorSwingData dataBuffer) : this()
      {
         m_dataBuffer = dataBuffer;

         // set data source of customizeDoorOpeningDataGridView.
         customizeDoorOpeningDataGridView.AutoGenerateColumns = false;
         customizeDoorOpeningDataGridView.DataSource          = m_dataBuffer.DoorFamilies;
         familyNameColumn.DataPropertyName                    = "FamilyName";
         OpeningColumn.DataPropertyName                       = "BasalOpeningValue";
         OpeningColumn.DataSource                             = DoorSwingData.OpeningTypes;

         customizeDoorOpeningDataGridView.Focus();
         if (customizeDoorOpeningDataGridView.Rows.Count != 0)
         {
            customizeDoorOpeningDataGridView.Rows[0].Selected = true;
         }        
      }

      /// <summary>
      /// Preview door's geometry when user select one door family in customizeDoorOpeningDataGridView.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void customizeDoorOpeningDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
      {
        DoorFamily selectedDoorFamily = customizeDoorOpeningDataGridView.Rows[e.RowIndex].DataBoundItem as DoorFamily;
        m_currentGraphic              = selectedDoorFamily.Geometry;

         // update the dialog box's display.
        previewPictureBox.Refresh();
      }

      /// <summary>
      /// PreviewBox redraw.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void previewPictureBox_Paint(object sender, PaintEventArgs e)
      {
         // do nothing.
         if (null == m_currentGraphic)
         {
            return;
         }

         // The object of Graphics to draw sketch.
         Graphics graphics            = e.Graphics;
         // Get the element bounding box's rectangle area.
         RectangleF doorGeoRectangleF = m_currentGraphic.BBOX2D;
         // Get the display rectangle area of PreviewBox.
         RectangleF displayRectangleF = previewPictureBox.DisplayRectangle;

         // Calculate the draw area according to the size of the sketch: Adjust the shrink to change borders
         if ((doorGeoRectangleF.Width * displayRectangleF.Height) > (doorGeoRectangleF.Height * displayRectangleF.Width))
         {
            displayRectangleF.Inflate((float)(-0.1 * displayRectangleF.Width), (float)(-1 + (doorGeoRectangleF.Height * 0.8 * displayRectangleF.Width) / (doorGeoRectangleF.Width * displayRectangleF.Height)));
         }
         else
         {
            displayRectangleF.Inflate((float)(-1 + (doorGeoRectangleF.Width * 0.8 * displayRectangleF.Height) / (doorGeoRectangleF.Height * displayRectangleF.Width)), (float)(-0.1 * displayRectangleF.Height));
         }

         // Mapping the point in sketch to point in draw area.
         PointF[] plgpts = new PointF[3];
         plgpts[0].X = displayRectangleF.Left;
         plgpts[0].Y = displayRectangleF.Bottom;
         plgpts[1].X = displayRectangleF.Right;
         plgpts[1].Y = displayRectangleF.Bottom;
         plgpts[2].X = displayRectangleF.Left;
         plgpts[2].Y = displayRectangleF.Top;
       
         // Get the transform matrix.
         System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix(doorGeoRectangleF, plgpts);

         // Clear the object of graphics.
         graphics.Clear(previewPictureBox.BackColor); 
         // Transform the object of graphics.
         graphics.Transform = matrix; 
         // The pen for drawing profiles
         Pen drawPen        = new Pen(System.Drawing.Color.Red, (float)0.05); 

         // Draw profiles.
         m_currentGraphic.DrawGraphics(graphics, drawPen);
      }
   }
}