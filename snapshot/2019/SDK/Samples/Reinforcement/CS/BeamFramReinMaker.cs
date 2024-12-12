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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.Reinforcement.CS
{

   /// <summary>
   /// The class derived from FramReinMaker shows how to create the rebars for a beam
   /// </summary>
   public class BeamFramReinMaker : FramReinMaker
   {
      #region Private Members

      BeamGeometrySupport m_geometry;   // The geometry support for beam rebar creation

      // The rebar type, hook type and spacing information
      RebarBarType m_topEndType = null;       //type of the end rebar in the top of beam  
      RebarBarType m_topCenterType = null;    //type of the center rebar in the center of beam
      RebarBarType m_bottomType = null; //type of the rebar on bottom of the beam
      RebarBarType m_transverseType = null;   //type of the transverse rebar

      RebarHookType m_topHookType = null;     //type of the hook in the top end rebar
      RebarHookType m_transverseHookType = null;  // type of the hook in the transverse rebar

      double m_transverseEndSpacing = 0;      //the spacing value of end transverse rebar
      double m_transverseCenterSpacing = 0;   //the spacing value of center transverse rebar

      #endregion

      #region Properties
      /// <summary>
      /// get and set the type of the end rebar in the top of beam
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
      /// get and set the type of the center rebar in the top of beam
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
      /// get and set the type of the rebar in the bottom of beam
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
      /// get and set the type of the transverse rebar
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
      /// get and set the spacing value of end transverse rebar
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
      /// get and set the spacing value of center transverse rebar
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
      /// get and set the hook type of top end rebar
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
      /// get and set the hook type of transverse rebar
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
         // create the top rebars
         bool flag = FillTopBars();

         // create the bottom rebars
         flag = flag && FillBottomBars();

         // create the transverse rebars
         flag = flag && FillTransverseBars();

         return base.FillWithBars();
      }

      #endregion



      /// <summary>
      /// Create the rebar at the bottom of beam
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      public bool FillBottomBars()
      {
         // get the geometry information of the bottom rebar
         RebarGeometry geomInfo = m_geometry.GetBottomRebar();

         // create the rebar
         Rebar rebar = PlaceRebars(m_bottomType, null, null, geomInfo,
                                 RebarHookOrientation.Left, RebarHookOrientation.Left);
         return (null != rebar);
      }

      /// <summary>
      /// Create the transverse rebars
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      public bool FillTransverseBars()
      {
         // create all kinds of transverse rebars according to the TransverseRebarLocation
         foreach (TransverseRebarLocation location in Enum.GetValues(
                                                     typeof(TransverseRebarLocation)))
         {
            Rebar createdRebar = FillTransverseBar(location);
            //judge whether the transverse rebar creation is successful
            if (null == createdRebar)
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Create the transverse rebars, according to the location of transverse rebars
      /// </summary>
      /// <param name="location">location of rebar which need to be created</param>
      /// <returns>the created rebar, return null if the creation is unsuccessful</returns>
      public Rebar FillTransverseBar(TransverseRebarLocation location)
      {
         // Get the geometry information which support rebar creation
         RebarGeometry geomInfo = new RebarGeometry();
         switch (location)
         {
            case TransverseRebarLocation.Start: // start transverse rebar
            case TransverseRebarLocation.End:   // end transverse rebar
               geomInfo = m_geometry.GetTransverseRebar(location, m_transverseEndSpacing);
               break;
            case TransverseRebarLocation.Center:// center transverse rebar
               geomInfo = m_geometry.GetTransverseRebar(location, m_transverseCenterSpacing);
               break;
         }

         RebarHookOrientation startHook = RebarHookOrientation.Right;
         RebarHookOrientation endHook = RebarHookOrientation.Left;
         if (!GeomUtil.IsInRightDir(geomInfo.Normal))
         {
            startHook = RebarHookOrientation.Left;
            endHook = RebarHookOrientation.Right;
         }

         // create the rebar
         return PlaceRebars(m_transverseType, m_transverseHookType, m_transverseHookType,
                                         geomInfo, startHook, endHook);
      }

      /// <summary>
      /// Get the hook orient of the top rebar
      /// </summary>
      /// <param name="geomInfo">the rebar geometry support information</param>
      /// <param name="location">the location of top rebar</param>
      /// <returns>the hook orient of the top hook</returns>
      private RebarHookOrientation GetTopHookOrient(RebarGeometry geomInfo, TopRebarLocation location)
      {
         // Top center rebar doesn't need hook.
         if (TopRebarLocation.Center == location)
         {
            throw new Exception("Center top rebar doesn't have any hook.");
         }

         // Get the hook direction, rebar normal and rebar line
         Autodesk.Revit.DB.XYZ hookVec = m_geometry.GetDownDirection();
         Autodesk.Revit.DB.XYZ normal = geomInfo.Normal;
         Line rebarLine = geomInfo.Curves[0] as Line;

         // get the top start hook orient
         if (TopRebarLocation.Start == location)
         {
            Autodesk.Revit.DB.XYZ curveVec = GeomUtil.SubXYZ(rebarLine.GetEndPoint(1), rebarLine.GetEndPoint(0));
            return GeomUtil.GetHookOrient(curveVec, normal, hookVec);
         }
         else    // get the top end hook orient
         {
            Autodesk.Revit.DB.XYZ curveVec = GeomUtil.SubXYZ(rebarLine.GetEndPoint(0), rebarLine.GetEndPoint(1));
            return GeomUtil.GetHookOrient(curveVec, normal, hookVec);
         }
      }


      /// <summary>
      /// Create the rebar at the top of beam
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      private bool FillTopBars()
      {
         // create all kinds of top rebars according to the TopRebarLocation
         foreach (TopRebarLocation location in Enum.GetValues(typeof(TopRebarLocation)))
         {
            Rebar createdRebar = FillTopBar(location);
            //judge whether the top rebar creation is successful
            if (null == createdRebar)
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// Create the rebar at the top of beam, according to the top rebar location
      /// </summary>
      /// <param name="location">location of rebar which need to be created</param>
      /// <returns>the created rebar, return null if the creation is unsuccessful</returns>
      private Rebar FillTopBar(TopRebarLocation location)
      {
         //get the geometry information of the rebar
         RebarGeometry geomInfo = m_geometry.GetTopRebar(location);

         RebarHookType startHookType = null; //the start hook type of the rebar
         RebarHookType endHookType = null;   // the end hook type of the rebar
         RebarBarType rebarType = null;      // the rebar type 
         RebarHookOrientation startOrient = RebarHookOrientation.Right;// the start hook orient
         RebarHookOrientation endOrient = RebarHookOrientation.Left;  // the end hook orient

         // decide the rebar type, hook type and hook orient according to location
         switch (location)
         {
            case TopRebarLocation.Start:
               startHookType = m_topHookType;  // start hook type
               rebarType = m_topEndType;       // rebar type
               startOrient = GetTopHookOrient(geomInfo, location); // start hook orient
               break;
            case TopRebarLocation.Center:
               rebarType = m_topCenterType;    // rebar type
               break;
            case TopRebarLocation.End:
               endHookType = m_topHookType;    // end hook type
               rebarType = m_topEndType;       // rebar type
               endOrient = GetTopHookOrient(geomInfo, location);   // end hook orient
               break;
         }

         // create the rebar
         return PlaceRebars(rebarType, startHookType, endHookType,
                                         geomInfo, startOrient, endOrient);
      }

   }
}
