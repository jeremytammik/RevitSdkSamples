//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using System.Text;
using System.Drawing;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit;

namespace Revit.SDK.Samples.CurtainWallGrid.CS
{
   /// <summary>
   /// the class manages the creation operation for the curtain wall
   /// </summary>
   public class WallGeometry
   {
      #region Fields
      // the document of this sample
      MyDocument m_myDocument;

      // the refferred drawing class for the curtain wall
      private WallDrawing m_drawing;

      // the selected ViewPlan used for curtain wall creation
      Autodesk.Revit.DB.ViewPlan m_selectedView;

      // the selected wall type
      WallType m_selectedWallType;

      // store the start point of baseline (in PointD format)
      PointD m_startPointD;

      //store the start point of baseline (in Autodesk.Revit.DB.XYZ format)
      Autodesk.Revit.DB.XYZ m_startXYZ;

      // store the end point of baseline (in PointD format)
      PointD m_endPointD;

      //store the end point of baseline (in Autodesk.Revit.DB.XYZ format)
      Autodesk.Revit.DB.XYZ m_endXYZ;
      #endregion

      #region Properties
      /// <summary>
      /// the document of this sample
      /// </summary>
      public MyDocument MyDocument
      {
         get
         {
            return m_myDocument;
         }
      }

      /// <summary>
      /// the refferred drawing class for the curtain wall
      /// </summary>
      public WallDrawing Drawing
      {
         get
         {
            return m_drawing;
         }
      }

      /// <summary>
      /// the selected ViewPlan used for curtain wall creation
      /// </summary>
      public Autodesk.Revit.DB.ViewPlan SelectedView
      {
         get
         {
            return m_selectedView;
         }
         set
         {
            m_selectedView = value;
         }
      }

      /// <summary>
      /// the selected wall type
      /// </summary>
      public WallType SelectedWallType
      {
         get
         {
            return m_selectedWallType;
         }
         set
         {
            m_selectedWallType = value;
         }
      }

      /// <summary>
      /// store the start point of baseline (in PointD format)
      /// </summary>
      public PointD StartPointD
      {
         get
         {
            return m_startPointD;
         }
         set
         {
            m_startPointD = value;
         }
      }

      /// <summary>
      /// Get start point of baseline
      /// </summary>
      public Autodesk.Revit.DB.XYZ StartXYZ
      {
         get
         {
            return m_startXYZ;
         }
         set
         {
            m_startXYZ = value;
         }
      }

      /// <summary>
      /// store the end point of baseline (in PointD format)
      /// </summary>
      public PointD EndPointD
      {
         get
         {
            return m_endPointD;
         }
         set
         {
            m_endPointD = value;
         }
      }

      /// <summary>
      /// Get end point of baseline
      /// </summary>
      public Autodesk.Revit.DB.XYZ EndXYZ
      {
         get
         {
            return m_endXYZ;
         }
         set
         {
            m_endXYZ = value;
         }
      }
      #endregion

      #region Constructors
      /// <summary>
      /// default constructor
      /// </summary>
      /// <param name="myDoc">
      /// the document of the sample
      /// </param>
      public WallGeometry(MyDocument myDoc)
      {
         m_myDocument = myDoc;
         m_drawing = new WallDrawing(this);
      }
      #endregion

      #region Public methods
      /// <summary>
      /// create the curtain wall to the active document of Revit
      /// </summary>
      /// <returns>
      /// the created curtain wall
      /// </returns>
      public Wall CreateCurtainWall()
      {
         if (null == m_selectedWallType || null == m_selectedView)
         {
            return null;
         }

         Autodesk.Revit.Creation.Document createDoc = m_myDocument.Document.Create;
         Autodesk.Revit.Creation.Application createApp = m_myDocument.CommandData.Application.Application.Create;
         //baseline
         System.Drawing.Point point0 = m_drawing.WallLine2D.StartPoint;
         System.Drawing.Point point1 = m_drawing.WallLine2D.EndPoint;
         //new baseline and transform coordinate on windows UI to Revit UI
         m_startXYZ = new Autodesk.Revit.DB.XYZ(m_startPointD.X, m_startPointD.Y, 0);
         m_endXYZ = new Autodesk.Revit.DB.XYZ(m_endPointD.X, m_endPointD.Y, 0);
         Autodesk.Revit.DB.Line baseline = null;
         try
         {
            baseline = createApp.NewLineBound(m_startXYZ, m_endXYZ);
         }
         catch (System.ArgumentException)
         {
            MessageBox.Show("The start point and the end point of the line are too close, please re-draw it.");
         }
         Transaction act = new Transaction(m_myDocument.Document);
         act.Start(Guid.NewGuid().GetHashCode().ToString());
         Wall wall = createDoc.NewWall(baseline, m_selectedWallType,
             m_selectedView.GenLevel, 20, 0, false, false);
         act.Commit();
         Transaction act2 = new Transaction(m_myDocument.Document);
         act2.Start(Guid.NewGuid().GetHashCode().ToString());
         m_myDocument.UIDocument.ShowElements(wall);
         act2.Commit();
         return wall;
      }
      #endregion
   }// end of class
}
