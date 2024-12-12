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
using System.Collections.ObjectModel;
using System.Text;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.Reinforcement.CS
{
    /// <summary>
    /// The interface for the family instance reinforcement creation.
    /// The main method is Run(), which used to create the reinforcement
    /// </summary>
    public interface IFrameReinMaker
    {
        /// <summary>
        /// Main function of Maker interface
        /// </summary>
        /// <returns>indicate the result of run</returns>
        bool Run();
    }


    /// <summary>
    /// The base class for family instance reinforcement creation.
    /// It only implement the Run() method. which give the flow process for creation.
    /// </summary>
    public class FramReinMaker : IFrameReinMaker
    {
        /// <summary>
        /// the API create handle
        /// </summary>
        protected Autodesk.Revit.DB.Document m_revitDoc;

        /// <summary>
        /// the family instance to places rebar on
        /// </summary>
        protected FamilyInstance m_hostObject;

        /// <summary>
        /// a set to store all the rebar types
        /// </summary>
        protected List<RebarBarType> m_rebarTypes = new List<RebarBarType>();

        /// <summary>
        /// a list to store all the hook types
        /// </summary>
        protected List<RebarHookType> m_hookTypes = new List<RebarHookType>();

        /// <summary>
        /// Show all the rebar types in revit
        /// </summary>
        public IList<RebarBarType> RebarTypes
        {
            get
            {
                return m_rebarTypes;
            }
        }

        /// <summary>
        /// Show all the rebar hook types in revit
        /// </summary>
        public IList<RebarHookType> HookTypes
        {
            get
            {
                return m_hookTypes;
            }
        }

        /// <summary>
        /// Implement the Run() method of IFrameReinMaker interface.
        /// Give the flew process of the reinforcement creation.
        /// </summary>
        /// <returns></returns>
        bool IFrameReinMaker.Run()
        {
            // First, check the data whether is right and enough.
            if (!AssertData())
            {
                return false;
            }

            // Second, show a form to the user to collect creation information
            if (!DisplayForm())
            {
                return false;
            }

            // At last, begin to create the reinforcement rebars
            if (!FillWithBars())
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// This is a virtual method which used to check the data whether is right and enough.
        /// </summary>
        /// <returns>true if the the data is right and enough, otherwise false.</returns>
        protected virtual bool AssertData()
        {
            return true;    // only return true
        }

        /// <summary>
        /// This is a virtual method which used to collect creation information
        /// </summary>
        /// <returns>true if the informatin collection is successful, otherwise false</returns>
        protected virtual bool DisplayForm()
        {
            return true;    // only return true
        }


        /// <summary>
        /// This is a virtual method which used to create reinforcement.
        /// </summary>
        /// <returns>true if the creation is successful, otherwise false</returns>
        protected virtual bool FillWithBars()
        {
            return true;    // only return true
        }

        /// <summary>
        /// The constructor of FramReinMaker
        /// </summary>
        /// <param name="commandData">the ExternalCommandData reference</param>
        /// <param name="hostObject">the host family instance</param>
        protected FramReinMaker(ExternalCommandData commandData, FamilyInstance hostObject)
        {
            // Get and store reinforcement create handle and host family instance
            m_revitDoc = commandData.Application.ActiveUIDocument.Document;
            m_hostObject = hostObject;

            // Get all the rebar types in revit
            if (!GetRebarTypes(commandData))
            {
                throw new Exception("Can't get any rebar type from revit.");
            }

            // Get all the rebar hook types in revit
            if (!GetHookTypes(commandData))
            {
                throw new Exception("Can't get any rebar hook type from revit.");
            }
        }


        /// <summary>
        /// The helper function to changed rebar number and spacing properties
        /// </summary>
        /// <param name="bar">The rebar instance which need to modify</param>
        /// <param name="number">The rebar number want to set</param>
        /// <param name="spacing">The spacing want to set</param>
        protected static void SetRebarSpaceAndNumber(Rebar bar, int number, double spacing)
        {
            // Asset the parameter is valid
            if (null == bar || 2 > number || 0 > spacing)
            {
                return;
            }

            // Change the rebar number and spacing properties
            bar.SetLayoutAsNumberWithSpacing(number, spacing, true, true, true);
        }

        /// <summary>
        /// A wrap fuction which used to create the reinforcement.
        /// </summary>
        /// <param name="rebarType">The element of RebarBarType</param>
        /// <param name="startHook">The element of start RebarHookType</param>
        /// <param name="endHook">The element of end RebarHookType</param>
        /// <param name="geomInfo">The goemetry information of the rebar</param>
        /// <param name="startOrient">An Integer defines the orientation of the start hook</param>
        /// <param name="endOrient">An Integer defines the orientation of the end hook</param>
        /// <returns></returns>
        protected Rebar PlaceRebars(RebarBarType rebarType, RebarHookType startHook,
                                            RebarHookType endHook, RebarGeometry geomInfo,
                                            RebarHookOrientation startOrient, RebarHookOrientation endOrient)
        {
            Autodesk.Revit.DB.XYZ normal = geomInfo.Normal; // the direction of rebar distribution
            IList<Curve> curves = geomInfo.Curves;    // the shape of the rebar curves

            // Invoke the NewRebar() method to create rebar 
            Rebar createdRebar = Rebar.CreateFromCurves(m_revitDoc, Autodesk.Revit.DB.Structure.RebarStyle.Standard, rebarType, startHook, endHook,
                                         m_hostObject, normal, curves,
                                                startOrient, endOrient, false, true);

            if (null == createdRebar)   // Assert the creation is successful
            {
                return null;
            }

            // Change the rebar number and spacing properties to the user wanted
            SetRebarSpaceAndNumber(createdRebar, geomInfo.RebarNumber, geomInfo.RebarSpacing);
            return createdRebar;
        }

        /// <summary>
        /// get all the hook types in current project, and store in m_hookTypes data
        /// </summary>
        /// <param name="commandData">the ExternalCommandData reference</param>
        /// <returns>true if some hook types can be gotton, otherwise false</returns>
        private bool GetHookTypes(ExternalCommandData commandData)
        {
            // Initialize the m_hookTypes which used to store all hook types.
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document);
            filteredElementCollector.OfClass(typeof(RebarHookType));
            m_hookTypes = filteredElementCollector.Cast<RebarHookType>().ToList<RebarHookType>();

            // If no hook types in revit return false, otherwise true
            return (0 == m_hookTypes.Count) ? false : true;
        }

        /// <summary>
        /// get all the rebar types in current project, and store in m_rebarTypes data
        /// </summary>
        /// <param name="commandData">the ExternalCommandData reference</param>
        /// <returns>true if some rebar types can be gotton, otherwise false</returns>
        private bool GetRebarTypes(ExternalCommandData commandData)
        {
            // Initialize the m_rebarTypes which used to store all rebar types.
            // Get all rebar types in revit and add them in m_rebarTypes
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document);
            filteredElementCollector.OfClass(typeof(RebarBarType));
            m_rebarTypes = filteredElementCollector.Cast<RebarBarType>().ToList<RebarBarType>();

            // If no rebar types in revit return false, otherwise true
            return (0 == m_rebarTypes.Count) ? false : true;
        }
    }
}
