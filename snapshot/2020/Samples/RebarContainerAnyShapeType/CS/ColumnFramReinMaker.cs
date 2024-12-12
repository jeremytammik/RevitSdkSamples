//
// (C) Copyright 2015 by Autodesk, Inc.
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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.RebarContainerAnyShapeType.CS
{
   /// <summary>
   /// The class derived form FramReinMaker shows how to create the reinforcement for a column
   /// </summary>
   public class ColumnFramReinMaker : FramReinMaker
   {
      #region Private Members

      ColumnGeometrySupport m_geometry; // The geometry support for column reinforcement creation

      RebarBarType m_transverseEndType = null;      //type of the end transverse reinforcement
      RebarBarType m_transverseCenterType = null;   //type of the center transverse reinforcement
      RebarBarType m_verticalType = null;           //type of the vertical reinforcement
      RebarHookType m_transverseHookType = null;    //type of the hook

      double m_transverseEndSpacing = 0;    //the space value of end transverse reinforcement
      double m_transverseCenterSpacing = 0; //the space value of center transverse reinforcement
      int m_verticalRebarNumber = 0;        //the number of the vertical reinforcement

      #endregion

      #region Properties

      /// <summary>
      /// get and set the type of the end transverse reinforcement
      /// </summary>
      public RebarBarType TransverseEndType
      {
         get
         {
            return m_transverseEndType;
         }
         set
         {
            m_transverseEndType = value;
         }
      }

      /// <summary>
      /// get and set the type of the center transverse reinforcement
      /// </summary>
      public RebarBarType TransverseCenterType
      {
         get
         {
            return m_transverseCenterType;
         }
         set
         {
            m_transverseCenterType = value;
         }
      }

      /// <summary>
      /// get and set the type of the vertical reinforcement
      /// </summary>
      public RebarBarType VerticalRebarType
      {
         get
         {
            return m_verticalType;
         }
         set
         {
            m_verticalType = value;
         }
      }

      /// <summary>
      /// get and set the space value of end transverse reinforcement
      /// </summary>
      public double TransverseEndSpacing
      {
         get
         {
            return m_transverseEndSpacing;
         }
         set
         {
            if (0 > value)  // spacing data must be above 0
            {
               throw new Exception("Transverse end spacing should be above zero");
            }
            m_transverseEndSpacing = value;
         }
      }

      /// <summary>
      /// get and set the space value of center transverse reinforcement
      /// </summary>
      public double TransverseCenterSpacing
      {
         get
         {
            return m_transverseCenterSpacing;
         }
         set
         {
            if (0 > value)  // spacing data must be above 0
            {
               throw new Exception("Transverse center spacing should be above zero");
            }
            m_transverseCenterSpacing = value;
         }
      }

      /// <summary>
      /// get and set the number of vertical reinforcement
      /// </summary>
      public int VerticalRebarNumber
      {
         get
         {
            return m_verticalRebarNumber;
         }
         set
         {
            if (4 > value)  // vertical reinforcement number must be above 3
            {
               throw new Exception("The minimum of vertical reinforcement number should be 4.");
            }
            m_verticalRebarNumber = value;
         }
      }

      /// <summary>
      /// get and set the hook type of transverse reinforcement
      /// </summary>
      public RebarHookType TransverseHookType
      {
         get
         {
            return m_transverseHookType;
         }
         set
         {
            m_transverseHookType = value;
         }
      }

      #endregion

      #region Constructor
      /// <summary>
      /// Constructor of the ColumnFramReinMaker
      /// </summary>
      /// <param name="commandData">the ExternalCommandData reference</param>
      /// <param name="hostObject">the host column</param>
      public ColumnFramReinMaker(ExternalCommandData commandData, FamilyInstance hostObject)
         : base(commandData, hostObject)
      {
         //create a new options for current project
         Options geoOptions = commandData.Application.Application.Create.NewGeometryOptions();
         geoOptions.ComputeReferences = true;

         //create a ColumnGeometrySupport instance 
         m_geometry = new ColumnGeometrySupport(hostObject, geoOptions);
      }
      #endregion

      #region Override Methods

      /// <summary>
      /// Override method to do some further checks
      /// </summary>
      /// <returns>true if the the data is right and enough, otherwise false.</returns>
      protected override bool AssertData()
      {
         return base.AssertData();
      }

      /// <summary>
      /// Display a form to collect the information for column reinforcement creation
      /// </summary>
      /// <returns>true if the information collection is successful, otherwise false</returns>
      protected override bool DisplayForm()
      {
         // Display ColumnFramReinMakerForm for the user input information 
         using (ColumnFramReinMakerForm displayForm = new ColumnFramReinMakerForm(this))
         {
            if (DialogResult.OK != displayForm.ShowDialog())
            {
               return false;
            }
         }
         return base.DisplayForm();
      }

      /// <summary>
      /// Override method to create reinforcement on the selected column
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false.</returns>
      protected override bool FillWithBars()
      {
         //create Rebar Container
         ElementId conTypeId = RebarContainerType.CreateDefaultRebarContainerType(m_revitDoc);
         ElementId hostId = m_hostObject.Id;
         Element host = m_revitDoc.GetElement(hostId);
         if (null != host)
         {
            RebarContainer cont = RebarContainer.Create(m_revitDoc, host, conTypeId);
            bool flag = FillTransverseItems(cont);
            flag = flag && FillVerticalItems(cont);
         }

         return base.FillWithBars();
      }


      #endregion

      /// <summary>
      /// create the transverse reinforcement for the column
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      public bool FillTransverseItems(RebarContainer cont)
      {
         // create all kinds of transverse reinforcement according to the TransverseRebarLocation
         foreach (TransverseRebarLocation location in Enum.GetValues(typeof(TransverseRebarLocation)))
         {
            RebarContainerItem item = FillTransverseItem(cont, location);
            //judge whether the transverse reinforcement creation is successful
            if (null == item)
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Create the transverse reinforcement, according to the transverse reinforcement location
      /// </summary>
      /// <param name="location">location of rebar which need to be created</param>
      /// <returns>the created reinforcement, return null if the creation is unsuccessful</returns>
      public RebarContainerItem FillTransverseItem(RebarContainer cont, TransverseRebarLocation location)
      {
         // Get the geometry information which support reinforcement creation
         RebarGeometry geomInfo = new RebarGeometry();
         RebarBarType barType = null;
         switch (location)
         {
            case TransverseRebarLocation.Start: // start transverse reinforcement
            case TransverseRebarLocation.End:   // end transverse reinforcement
               geomInfo = m_geometry.GetTransverseRebar(location, m_transverseEndSpacing);
               barType = m_transverseEndType;
               break;
            case TransverseRebarLocation.Center:// center transverse reinforcement   
               geomInfo = m_geometry.GetTransverseRebar(location, m_transverseCenterSpacing);
               barType = m_transverseCenterType;
               break;
            default:
               break;
         }

         // create the container item
         return PlaceContainerItem(cont, barType, m_transverseHookType, m_transverseHookType, geomInfo, RebarHookOrientation.Left, RebarHookOrientation.Left);
      }

      /// <summary>
      /// Create the vertical reinforcement according the location
      /// </summary>
      /// <param name="location">location of rebar which need to be created</param>
      /// <returns>the created reinforcement, return null if the creation is unsuccessful</returns>
      public RebarContainerItem FillVerticalItem(RebarContainer cont, VerticalRebarLocation location)
      {
         //calculate the reinforcement number in different location
         int rebarNubmer = m_verticalRebarNumber / 4;
         switch (location)
         {
            case VerticalRebarLocation.East:    // the east vertical reinforcement
               if (0 < m_verticalRebarNumber % 4)
               {
                  rebarNubmer++;
               }
               break;
            case VerticalRebarLocation.North:   // the north vertical reinforcement
               if (2 < m_verticalRebarNumber % 4)
               {
                  rebarNubmer++;
               }
               break;
            case VerticalRebarLocation.West:    // the west vertical reinforcement
               if (1 < m_verticalRebarNumber % 4)
               {
                  rebarNubmer++;
               }
               break;
            case VerticalRebarLocation.South:   // the south vertical reinforcement
               break;
         }

         // get the geometry information for reinforcement creation
         RebarGeometry geomInfo = m_geometry.GetVerticalRebar(location, rebarNubmer);

         // create the container item
         return PlaceContainerItem(cont, m_verticalType, null, null, geomInfo, RebarHookOrientation.Right, RebarHookOrientation.Right);
      }

      /// <summary>
      /// create the all the vertical reinforcement
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      private bool FillVerticalItems(RebarContainer cont)
      {
         // create all kinds of vertical reinforcement according to the VerticalRebarLocation
         foreach (VerticalRebarLocation location in Enum.GetValues(typeof(VerticalRebarLocation)))
         {
            RebarContainerItem item = FillVerticalItem(cont, location);
            //judge whether the vertical reinforcement creation is successful
            if (null == item)
            {
               return false;
            }
         }

         return true;
      }
   
   }

}
