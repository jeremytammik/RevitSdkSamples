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
   /// The class derived from FramReinMaker shows how to create the reinforcement for a beam
   /// </summary>
   public class BeamFramReinMaker : FramReinMaker
   {
      #region Private Members

      BeamGeometrySupport m_geometry;   // The geometry support for beam reinforcement creation

      // The reinforcement type, hook type and spacing information
      RebarBarType m_topEndType = null;       //type of the end reinforcement in the top of beam  
      RebarBarType m_topCenterType = null;    //type of the center reinforcement in the center of beam
      RebarBarType m_bottomType = null;       //type of the reinforcement on bottom of the beam
      RebarBarType m_transverseType = null;   //type of the transverse reinforcement

      RebarHookType m_topHookType = null;     //type of the hook in the top end reinforcement
      RebarHookType m_transverseHookType = null;  // type of the hook in the transverse reinforcement

      double m_transverseEndSpacing = 0;      //the spacing value of end transverse reinforcement
      double m_transverseCenterSpacing = 0;   //the spacing value of center transverse reinforcement

      #endregion

      #region Properties
      /// <summary>
      /// get and set the type of the end reinforcement in the top of beam
      /// </summary>
      public RebarBarType TopEndRebarType
      {
         get
         {
            return m_topEndType;
         }
         set
         {
            m_topEndType = value;
         }
      }

      /// <summary>
      /// get and set the type of the center reinforcement in the top of beam
      /// </summary>
      public RebarBarType TopCenterRebarType
      {
         get
         {
            return m_topCenterType;
         }
         set
         {
            m_topCenterType = value;
         }
      }

      /// <summary>
      /// get and set the type of the reinforcement in the bottom of beam
      /// </summary>
      public RebarBarType BottomRebarType
      {
         get
         {
            return m_bottomType;
         }
         set
         {
            m_bottomType = value;
         }
      }

      /// <summary>
      /// get and set the type of the transverse reinforcement
      /// </summary>
      public RebarBarType TransverseRebarType
      {
         get
         {
            return m_transverseType;
         }
         set
         {
            m_transverseType = value;
         }
      }

      /// <summary>
      /// get and set the spacing value of end transverse reinforcement
      /// </summary>
      public double TransverseEndSpacing
      {
         get
         {
            return m_transverseEndSpacing;
         }
         set
         {
            if (0 > value)
            {
               throw new Exception("Transverse end spacing should be above zero");
            }
            m_transverseEndSpacing = value;
         }
      }

      /// <summary>
      /// get and set the spacing value of center transverse reinforcement
      /// </summary>
      public double TransverseCenterSpacing
      {
         get
         {
            return m_transverseCenterSpacing;
         }
         set
         {
            if (0 > value)
            {
               throw new Exception("Transverse center spacing should be above zero");
            }
            m_transverseCenterSpacing = value;
         }
      }

      /// <summary>
      /// get and set the hook type of top end reinforcement
      /// </summary>
      public RebarHookType TopHookType
      {
         get
         {
            return m_topHookType;
         }
         set
         {
            m_topHookType = value;
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
      /// Constructor of the BeamFramReinMaker
      /// </summary>
      /// <param name="commandData">the ExternalCommandData reference</param>
      /// <param name="hostObject">the host beam</param>
      public BeamFramReinMaker(ExternalCommandData commandData, FamilyInstance hostObject)
         : base(commandData, hostObject)
      {
         //create new options for current project
         Options geoOptions = commandData.Application.Application.Create.NewGeometryOptions();
         geoOptions.ComputeReferences = true;

         //create a BeamGeometrySupport instance.
         m_geometry = new BeamGeometrySupport(hostObject, geoOptions);
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
      /// Display a form to collect the information for beam reinforcement creation
      /// </summary>
      /// <returns>true if the information collection is successful, otherwise false</returns>
      protected override bool DisplayForm()
      {
         // Display BeamFramReinMakerForm for the user to input information 
         using (BeamFramReinMakerForm displayForm = new BeamFramReinMakerForm(this))
         {
            if (DialogResult.OK != displayForm.ShowDialog())
            {
               return false;
            }
         }
         return base.DisplayForm();
      }

      /// <summary>
      /// Override method to create rebar on the selected beam
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      protected override bool FillWithBars()
      {
         //create Rebar Container
         ElementId conTypeId = RebarContainerType.CreateDefaultRebarContainerType(m_revitDoc);
         RebarContainer cont = RebarContainer.Create(m_revitDoc, m_hostObject, conTypeId);

         // create the top items
         bool flag = FillTopItems(cont);

         // create the bottom items
         flag = flag && FillBottomItems(cont);

         // create the transverse items
         flag = flag && FillTransverseItems(cont);

         return base.FillWithBars();
      }

      #endregion



      /// <summary>
      /// Create the reinforcement at the bottom of beam
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      public bool FillBottomItems(RebarContainer cont)
      {
         // get the geometry information of the bottom reinforcement
         RebarGeometry geomInfo = m_geometry.GetBottomRebar();

         // create the container item
         RebarContainerItem item = PlaceContainerItem(cont, m_bottomType, null, null, geomInfo, RebarHookOrientation.Left, RebarHookOrientation.Left);
         return (null != item);
      }

      /// <summary>
      /// Create the transverse reinforcement
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      public bool FillTransverseItems(RebarContainer cont)
      {
         // create all kinds of transverse reinforcement according to the TransverseRebarLocation
         foreach (TransverseRebarLocation location in Enum.GetValues(
                                                     typeof(TransverseRebarLocation)))
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
      /// Create the transverse reinforcement, according to the location of transverse bars
      /// </summary>
      /// <param name="location">location of rebar which need to be created</param>
      /// <returns>the created container item, return null if the creation is unsuccessful</returns>
      public RebarContainerItem FillTransverseItem(RebarContainer cont, TransverseRebarLocation location)
      {
         // Get the geometry information which support reinforcement creation
         RebarGeometry geomInfo = new RebarGeometry();
         switch (location)
         {
            case TransverseRebarLocation.Start: // start transverse reinforcement
            case TransverseRebarLocation.End:   // end transverse reinforcement
               geomInfo = m_geometry.GetTransverseRebar(location, m_transverseEndSpacing);
               break;
            case TransverseRebarLocation.Center:// center transverse reinforcement
               geomInfo = m_geometry.GetTransverseRebar(location, m_transverseCenterSpacing);
               break;
         }

         RebarHookOrientation startHook = RebarHookOrientation.Left;
         RebarHookOrientation endHook = RebarHookOrientation.Left;
         if (!GeomUtil.IsInRightDir(geomInfo.Normal))
         {
            startHook = RebarHookOrientation.Right;
            endHook = RebarHookOrientation.Right;
         }

         // create the container item
         return PlaceContainerItem(cont, m_transverseType, m_transverseHookType, m_transverseHookType, geomInfo, startHook, endHook);
      }

      /// <summary>
      /// Get the hook orient of the top reinforcement
      /// </summary>
      /// <param name="geomInfo">the rebar geometry support information</param>
      /// <param name="location">the location of top rebar</param>
      /// <returns>the hook orient of the top hook</returns>
      private RebarHookOrientation GetTopHookOrient(RebarGeometry geomInfo, TopRebarLocation location)
      {
         // Top center rebar doesn't need hook.
         if (TopRebarLocation.Center == location)
         {
            throw new Exception("Center top reinforcement doesn't have any hook.");
         }

         // Get the hook direction, reinforcement normal and reinforcement line
         Autodesk.Revit.DB.XYZ hookVec = m_geometry.GetDownDirection();
         Autodesk.Revit.DB.XYZ normal = geomInfo.Normal;
         Line rebarLine = geomInfo.Curves[0] as Line;

         // get the top start hook orient
         if (TopRebarLocation.Start == location)
         {
            Autodesk.Revit.DB.XYZ curveVec = GeomUtil.SubXYZ(rebarLine.GetEndPoint(0), rebarLine.GetEndPoint(1));
            return GeomUtil.GetHookOrient(curveVec, normal, hookVec);
         }
         else    // get the top end hook orient
         {
            Autodesk.Revit.DB.XYZ curveVec = GeomUtil.SubXYZ(rebarLine.GetEndPoint(0), rebarLine.GetEndPoint(1));
            return GeomUtil.GetHookOrient(curveVec, normal, hookVec);
         }
      }


      /// <summary>
      /// Create the reinforcement at the top of beam
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      private bool FillTopItems(RebarContainer cont)
      {
         // create all kinds of top reinforcement according to the TopRebarLocation
         foreach (TopRebarLocation location in Enum.GetValues(typeof(TopRebarLocation)))
         {
            RebarContainerItem item = FillTopItem(cont, location);
            //judge whether the top reinforcement creation is successful
            if (null == item)
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Create the reinforcement at the top of beam, according to the top reinforcement location
      /// </summary>
      /// <param name="location">location of rebar which need to be created</param>
      /// <returns>the created reinforcement, return null if the creation is unsuccessful</returns>
      private RebarContainerItem FillTopItem(RebarContainer cont, TopRebarLocation location)
      {
         //get the geometry information of the reinforcement
         RebarGeometry geomInfo = m_geometry.GetTopRebar(location);

         RebarHookType startHookType = null; //the start hook type of the reinforcement
         RebarHookType endHookType = null;   // the end hook type of the reinforcement
         RebarBarType rebarType = null;      // the reinforcement type 
         RebarHookOrientation startOrient = RebarHookOrientation.Right;// the start hook orient
         RebarHookOrientation endOrient = RebarHookOrientation.Left;  // the end hook orient

         // decide the reinforcement type, hook type and hook orient according to location
         switch (location)
         {
            case TopRebarLocation.Start:
               startHookType = m_topHookType;  // start hook type
               rebarType = m_topEndType;       // reinforcement type
               startOrient = GetTopHookOrient(geomInfo, location); // start hook orient
               break;
            case TopRebarLocation.Center:
               rebarType = m_topCenterType;    // reinforcement type
               break;
            case TopRebarLocation.End:
               endHookType = m_topHookType;    // end hook type
               rebarType = m_topEndType;       // reinforcement type
               endOrient = GetTopHookOrient(geomInfo, location);   // end hook orient
               break;
         }

         // create the container item
         return PlaceContainerItem(cont, rebarType, startHookType, endHookType, geomInfo, startOrient, endOrient);
      }

   }
}
