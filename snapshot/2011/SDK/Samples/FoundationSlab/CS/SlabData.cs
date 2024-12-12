//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.FoundationSlab.CS
{
    /// <summary>
    /// A class collecting all useful datas from revit API for UI.
    /// </summary>
    public class SlabData
    {
        const double PlanarPrecision = 0.00033;

        // For finding elements and creating foundations slabs.
        Autodesk.Revit.UI.UIApplication m_revit;
        public static Autodesk.Revit.Creation.Application CreApp;
        // Foundation slab type for creating foundation slabs.
        FloorType m_foundationSlabType;
        // A set of levels to find out the lowest level of the building.
        SortedList<double, Level> m_levelList = new SortedList<double,Level>();
        // A set of views to find out the regular slab's bounding box.
        List<View> m_viewList = new List<View>();
        // A set of floors to find out all the regular slabs at the base of the building.
        List<Floor> m_floorList = new List<Floor>();

        // A set of regular slabs at the base of the building.
        // This set supplies all the regular slabs' datas for UI.
        List<RegularSlab> m_allBaseSlabList = new List<RegularSlab>();

        // A set of  the types of foundation slab.
        // This set supplies all the types of foundation slab for UI.
        List<FloorType> m_slabTypeList = new List<FloorType>();

        /// <summary>
        /// BaseSlabList property.
        /// This property is for UI. It can be edited by user.
        /// </summary>
        public Collection<RegularSlab> BaseSlabList
        {
            get { return new Collection< RegularSlab >(m_allBaseSlabList); }
        }
        
        /// <summary>
        /// FoundationSlabTypeList property.
        /// This property is for UI. It can not be edited by user.
        /// </summary>
        public ReadOnlyCollection<FloorType> FoundationSlabTypeList
        {
            get { return new ReadOnlyCollection<FloorType>(m_slabTypeList); }
        }

        /// <summary>
        /// FoundationSlabType property.
        /// This property gets value from UI to create foundation slabs.
        /// </summary>
        public object FoundationSlabType
        {
            set { m_foundationSlabType = value as FloorType; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="revit">An application object that contains data related to revit command.</param>
        public SlabData(UIApplication revit)
        {
            m_revit = revit;
            CreApp = m_revit.Application.Create;
            // Find out all useful elements.
            FindElements();
            // Get all base slabs. If no slab be found, throw an exception and return cancel.
            if (!GetAllBaseSlabs())
                throw new NullReferenceException("No planar slabs at the base of the building.");
        }

        /// <summary>
        /// Check whether a regular slab is selected.
        /// </summary>
        /// <returns>The bool value suggest being selected or not.</returns>
        public bool CheckHaveSelected()
        {
            foreach (RegularSlab slab in m_allBaseSlabList)
            {
                if (slab.Selected)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Change the Selected property for all regular slabs.
        /// </summary>
        /// <param name="value">The value for Selected property</param>
        public void ChangeAllSelected(bool value)
        {
            foreach (RegularSlab slab in m_allBaseSlabList)
            {
                slab.Selected = value;
            }
        }

        /// <summary>
        /// Create foundation slabs.
        /// </summary>
        /// <returns>The bool value suggest successful or not.</returns>
        public bool CreateFoundationSlabs()
        {
            // Create a foundation slab for each selected regular slab.
            foreach (RegularSlab slab in m_allBaseSlabList)
            {
                if (!slab.Selected)
                {
                    continue;
                }

                // Create a new slab.
                Autodesk.Revit.DB.XYZ normal = new Autodesk.Revit.DB.XYZ (0,0,1);

                Floor foundationSlab = m_revit.ActiveUIDocument.Document.Create.NewFoundationSlab(
                    slab.OctagonalProfile, m_foundationSlabType, m_levelList.Values[0], 
                    true, normal);

                if (null == foundationSlab)
                {
                    return false;
                }

                // Delete the regular slab.
                Autodesk.Revit.DB.ElementId deleteSlabId = slab.Id;
                m_revit.ActiveUIDocument.Document.Delete(deleteSlabId);
            }
            return true;
        }

        /// <summary>
        /// Find out all useful elements.
        /// </summary>
        private void FindElements()
        {
           IList<ElementFilter> filters = new List<ElementFilter>(4);
           filters.Add(new ElementClassFilter(typeof(Level)));
           filters.Add(new ElementClassFilter(typeof(View)));
           filters.Add(new ElementClassFilter(typeof(Floor)));
           filters.Add(new ElementClassFilter(typeof(FloorType)));

           LogicalOrFilter orFilter = new LogicalOrFilter(filters);
           FilteredElementCollector collector = new FilteredElementCollector(m_revit.ActiveUIDocument.Document);
           FilteredElementIterator iterator = collector.WherePasses(orFilter).GetElementIterator();
           while (iterator.MoveNext())
           {
                // Find out all levels.
                Level level = (iterator.Current) as Level;
                if (null != level)
                {
                    m_levelList.Add(level.Elevation, level);
                    continue;
                }

                // Find out all views.
                View view = (iterator.Current) as View;
                if (null != view && !view.IsTemplate)
                {
                    m_viewList.Add(view);
                    continue;
                }

                // Find out all floors.
                Floor floor = (iterator.Current) as Floor;
                if (null != floor)
                {
                    m_floorList.Add(floor);
                    continue;
                }

                // Find out all foundation slab types.
                FloorType floorType = (iterator.Current) as FloorType;
                if (null == floorType)
                {
                    continue;
                }
                if ("Structural Foundations" == floorType.Category.Name)
                {
                    m_slabTypeList.Add(floorType);
                }
            }
        }

        /// <summary>
        /// Get all base slabs.
        /// </summary>
        /// <returns>A bool value suggests successful or not.</returns>
        private bool GetAllBaseSlabs()
        {
            // No level, no slabs.
            if (0 == m_levelList.Count)
                return false;

            // Find out the lowest level's view for finding the bounding box of slab.
            View baseView = null;
            foreach (View view in m_viewList)
            {
                if (view.ViewName == m_levelList.Values[0].Name)
                {
                    baseView = view;
                }
            }
            if (null == baseView)
                return false;

            // Get all slabs at the base of the building.
            foreach (Floor floor in m_floorList)
            {
                if (floor.Level.Id.IntegerValue == m_levelList.Values[0].Id.IntegerValue)
                {
                    BoundingBoxXYZ bbXYZ = floor.get_BoundingBox(baseView);   // Get the slab's bounding box.

                    // Check the floor. If the floor is planar, deal with it, otherwise, leap it.
                    if (!IsPlanarFloor(bbXYZ, floor))
                        continue;

                    CurveArray floorProfile = GetFloorProfile(floor);    // Get the slab's profile.
                    RegularSlab regularSlab = new RegularSlab(floor, floorProfile, bbXYZ);   // Get a regular slab.
                    m_allBaseSlabList.Add(regularSlab);  // Add regular slab to the set.
                }
            }

            // Getting regular slabs.
            if (0 != m_allBaseSlabList.Count)
                return true;
            else return false;
        }

        /// <summary>
        /// Check whether the floor is planar.
        /// </summary>
        /// <param name="bbXYZ">The floor's bounding box.</param>
        /// <param name="floor">The floor object.</param>
        /// <returns>A bool value suggests the floor is planar or not.</returns>
        private static bool IsPlanarFloor(BoundingBoxXYZ bbXYZ, Floor floor)
        {
            // Get floor thickness.
            double floorThickness = 0.0;
            Parameter attribute = floor.ObjectType.get_Parameter(BuiltInParameter.FLOOR_ATTR_DEFAULT_THICKNESS_PARAM);
            if (null != attribute)
            {
                floorThickness = attribute.AsDouble();
            }

            // Get bounding box thickness.
            double boundThickness = Math.Abs(bbXYZ.Max.Z - bbXYZ.Min.Z);

            // Planar or not.
            if (Math.Abs(boundThickness - floorThickness) < PlanarPrecision)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Get a floor's profile.
        /// </summary>
        /// <param name="floor">The floor whose profile you want to get.</param>
        /// <returns>The profile of the floor.</returns>
        private CurveArray GetFloorProfile(Floor floor)
        {
            CurveArray floorProfile = new CurveArray();
            // Structural slab's profile can be found in it's AnalyticalModel.
            if (null != floor.GetAnalyticalModel())
            {
                AnalyticalModel analyticalModel = floor.GetAnalyticalModel();
                IList<Curve> curveList= analyticalModel.GetCurves(AnalyticalCurveType.ActiveCurves);
                for (int i = 0; i < curveList.Count; i++)
                {
                   floorProfile.Append(curveList[i]);
                }

                return floorProfile;
            }

            // Nonstructural floor's profile can be formed through it's Geometry.
            Options aOptions = m_revit.Application.Create.NewGeometryOptions();
            Autodesk.Revit.DB.GeometryElement aElementOfGeometry = floor.get_Geometry(aOptions);
            GeometryObjectArray geometryObjects = aElementOfGeometry.Objects;
            foreach (GeometryObject o in geometryObjects)
            {
                Solid solid = o as Solid;
                if (null == solid)
                    continue;
                
                // Form the floor's profile through solid's edges.
                EdgeArray edges = solid.Edges;
                for (int i = 0; i < (edges.Size) / 3; i++)
                {
                    Edge edge = edges.get_Item(i);
                    List<XYZ> xyzArray = edge.Tessellate() as List<XYZ>;  // A set of points.
                    for (int j = 0; j < (xyzArray.Count - 1); j++)
                    {
                        Autodesk.Revit.DB.XYZ startPoint = xyzArray[j];
                        Autodesk.Revit.DB.XYZ endPoint = xyzArray[j + 1];
                        Line line = CreApp.NewLine(startPoint, endPoint,true);
                       
                        floorProfile.Append(line);
                    }
                }
            }
            return floorProfile;
        }

    }
}
