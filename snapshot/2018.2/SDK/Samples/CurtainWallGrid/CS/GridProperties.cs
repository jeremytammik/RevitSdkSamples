//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.ComponentModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.CurtainWallGrid.CS
{
   /// <summary>
   /// all the supported Align type for the curtain grid
   /// </summary>
   public enum CurtainGridAlign
   {
      Beginning,
      Center,
      End
   }

   /// <summary>
   /// stores all the properties of the curtain grid and manages the behaviors and operations of the curtain grid
   /// </summary>
   public class GridProperties
   {
      #region Fields
      // stores the data of vertical justification
      private CurtainGridAlign m_verticalJustification;

      // stores the data of vertical angle
      private double m_verticalAngle;

      // stores the data of vertical offset
      private double m_verticalOffset;

      // stores how many vertical lines there are in the grid
      private int m_verticalLinesNumber;

      // stores the data of horizontal justification
      private CurtainGridAlign m_horizontalJustification;

      // stores the data of horizontal angle
      private double m_horizontalAngle;

      // stores the data of horizontal offset
      private double m_horizontalOffset;

      // stores how many horizontal lines there are in the grid
      private int m_horizontalLinesNumber;

      // stores how many panels there are in the grid
      private int m_panelNumber;

      // stores how many curtain cells there are in the grid
      private int m_cellNumber;

      // stores how many unlocked panels there are in the grid
      private int m_unlockedPanelsNumber;

      // stores how many mullions there are in the grid
      private int m_mullionsNumber;

      // stores how many unlocked mullions there are in the grid
      private int m_unlockedmullionsNumber;
      #endregion

      #region Properties
      /// <summary>
      /// stores the data of vertical justification
      /// </summary>
      [CategoryAttribute("Vertical Grid Pattern"), DefaultValueAttribute(CurtainGridAlign.Beginning), ReadOnlyAttribute(true)]
      public CurtainGridAlign VerticalJustification
      {
         get
         {
            return m_verticalJustification;
         }
         set
         {
            m_verticalJustification = value;
         }
      }

      /// <summary>
      /// stores the data of vertical angle
      /// </summary>
      [CategoryAttribute("Vertical Grid Pattern"), DefaultValueAttribute(0.0), ReadOnlyAttribute(true)]
      public double VerticalAngle
      {
         get
         {
            return m_verticalAngle;
         }
         set
         {
            m_verticalAngle = value;
         }
      }

      /// <summary>
      /// stores the data of vertical offset
      /// </summary>
      [CategoryAttribute("Vertical Grid Pattern"), DefaultValueAttribute(0.0), ReadOnlyAttribute(true)]
      public double VerticalOffset
      {
         get
         {
            return m_verticalOffset;
         }
         set
         {
            m_verticalOffset = value;
         }
      }

      /// <summary>
      /// stores how many U lines there are in the grid
      /// </summary>
      [CategoryAttribute("Vertical Grid Pattern"), DefaultValueAttribute(0), ReadOnlyAttribute(true)]
      public int VerticalLinesNumber
      {
         get
         {
            return m_verticalLinesNumber;
         }
         set
         {
            m_verticalLinesNumber = value;
         }
      }

      /// <summary>
      /// stores the data of horizontal justification
      /// </summary>
      [CategoryAttribute("Horizontal Grid Pattern"), DefaultValueAttribute(CurtainGridAlign.Beginning), ReadOnlyAttribute(true)]
      public CurtainGridAlign HorizontalJustification
      {
         get
         {
            return m_horizontalJustification;
         }
         set
         {
            m_horizontalJustification = value;
         }
      }

      /// <summary>
      /// stores the data of horizontal angle
      /// </summary>
      [CategoryAttribute("Horizontal Grid Pattern"), DefaultValueAttribute(0.0), ReadOnlyAttribute(true)]
      public double HorizontalAngle
      {
         get
         {
            return m_horizontalAngle;
         }
         set
         {
            m_horizontalAngle = value;
         }
      }

      /// <summary>
      /// stores the data of horizontal offset
      /// </summary>
      [CategoryAttribute("Horizontal Grid Pattern"), DefaultValueAttribute(0.0), ReadOnlyAttribute(true)]
      public double HorizontalOffset
      {
         get
         {
            return m_horizontalOffset;
         }
         set
         {
            m_horizontalOffset = value;
         }
      }

      /// <summary>
      /// stores how many V lines there are in the grid
      /// </summary>
      [CategoryAttribute("Horizontal Grid Pattern"), DefaultValueAttribute(0), ReadOnlyAttribute(true)]
      public int HorizontalLinesNumber
      {
         get
         {
            return m_horizontalLinesNumber;
         }
         set
         {
            m_horizontalLinesNumber = value;
         }
      }

      /// <summary>
      /// stores how many panels there are in the grid
      /// </summary>
      [CategoryAttribute("Other Data"), DefaultValueAttribute(0), ReadOnlyAttribute(true)]
      public int PanelNumber
      {
         get
         {
            return m_panelNumber;
         }
         set
         {
            m_panelNumber = value;
         }
      }

      /// <summary>
      /// stores how many curtain cells there are in the grid
      /// </summary>
      [CategoryAttribute("Other Data"), DefaultValueAttribute(0), ReadOnlyAttribute(true)]
      public int CellNumber
      {
         get
         {
            return m_cellNumber;
         }
         set
         {
            m_cellNumber = value;
         }
      }

      /// <summary>
      /// stores how many unlocked panels there are in the grid
      /// </summary>
      [CategoryAttribute("Other Data"), DefaultValueAttribute(0), ReadOnlyAttribute(true)]
      public int UnlockedPanelsNumber
      {
         get
         {
            return m_unlockedPanelsNumber;
         }
         set
         {
            m_unlockedPanelsNumber = value;
         }
      }

      /// <summary>
      /// stores how many mullions there are in the grid
      /// </summary>
      [CategoryAttribute("Other Data"), DefaultValueAttribute(0), ReadOnlyAttribute(true)]
      public int MullionsNumber
      {
         get
         {
            return m_mullionsNumber;
         }
         set
         {
            m_mullionsNumber = value;
         }
      }

      /// <summary>
      /// stores how many unlocked mullions there are in the grid
      /// </summary>
      [CategoryAttribute("Other Data"), DefaultValueAttribute(0), ReadOnlyAttribute(true)]
      public int UnlockedmullionsNumber
      {
         get
         {
            return m_unlockedmullionsNumber;
         }
         set
         {
            m_unlockedmullionsNumber = value;
         }
      }
      #endregion
   } // end of class
}
