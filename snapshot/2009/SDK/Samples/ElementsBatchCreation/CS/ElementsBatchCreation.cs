//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using Autodesk.Revit;
using Autodesk.Revit.Creation;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Enums;
using System.Windows.Forms;
using System.Diagnostics;

namespace Revit.SDK.Samples.ElementsBatchCreation.CS
{
    using View = Autodesk.Revit.Elements.View;

    /// <summary>
    /// This class will demonstrate how to create many elements via batch creation methods
    /// </summary>
    public class ElementsBatchCreation
    {        
        /// <summary>
        /// A reference to the external application
        /// </summary>
        private ExternalCommandData m_cmdData;

        /// <summary>
        /// A reference to active document
        /// </summary>
        private Autodesk.Revit.Document m_doc;

        /// <summary>
        /// A reference to Level 1
        /// </summary>
        private Level m_level;

        /// <summary>
        /// A reference to ViewPlan named "Level 1;
        /// </summary>
        private ViewPlan m_viewPlan;
        
        /// <summary>
        /// Constructor of ElementsBatchCreation
        /// </summary>
        /// <param name="cmdData">A reference to the external application</param>
        public ElementsBatchCreation(ExternalCommandData cmdData)
        {
            m_cmdData = cmdData;
            m_doc = cmdData.Application.ActiveDocument;
        }

        /// <summary>
        /// Batch Creations of several elements, it will call separate methods for each element
        /// </summary>
        /// <returns>If all batch creations succeed, return true; otherwise, return false</returns>
        public bool CreateElements()
        {    
            //Get common informations for batch creation
            PreCreate();

            //Prepare messages to notify user of succeed and failed operations      
            String failedMethods = "";
            String succeedMethods = "";
            if (CreateAreas())
            {
                succeedMethods += " Area";
            }
            else
            {
                failedMethods += " Area";
            }

            if (CreateColumns())
            {
                succeedMethods += " Column";
            }
            else
            {
                failedMethods += " Column";
            }

            if (CreateRooms())
            {
                succeedMethods += " Room";
            }
            else
            {
                failedMethods += " Room";
            }

            if (CreateTextNotes())
            {
                succeedMethods += " TextNote";
            }
            else
            {
                failedMethods += " TextNote";
            }

            if (CreateWalls())
            {
                succeedMethods += " Wall";
            }
            else
            {
                failedMethods += " Wall";
            }

            bool result = true;
            if(String.IsNullOrEmpty(succeedMethods))
            {
                MessageBox.Show("Batch creations of" + failedMethods + " failed", "ElementsBatchCreation");
                result = false;
            }
            else if (String.IsNullOrEmpty(failedMethods))
            {
                MessageBox.Show("Batch creations of" + succeedMethods + " succeed", "ElementsBatchCreation");
            }
            else
            {
                MessageBox.Show("Batch creations of" + succeedMethods + " succeed," + " Batch creations of" + failedMethods + " failed", "ElementsBatchCreation");
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Get common informations for batch creation
        /// </summary>
        private void PreCreate()
        {
            try
            {
                Autodesk.Revit.Creation.Application appCreation = m_cmdData.Application.Create;

                //Try to get Level named "Level 1" which will be used in most creations
                ElementIterator iterator = m_doc.get_Elements(typeof(Level));
                iterator.Reset();
                while (iterator.MoveNext())
                {
                    Level temp = iterator.Current as Level;
                    if (null != temp && "Level 1" == temp.Name)
                    {
                        m_level = temp;
                        break;
                    }
                }

                //Try to get ViewPlan named "Level 1"
                iterator = m_doc.get_Elements(typeof(ViewPlan));
                iterator.Reset();
                while (iterator.MoveNext())
                {
                    ViewPlan temp = iterator.Current as ViewPlan;
                    if (null != temp && ViewType.AreaPlan == temp.ViewType && "Level 1" == temp.ViewName)
                    {
                        m_viewPlan = temp;
                        break;
                    }
                }

                //If ViewPlan "Level 1" does not exist, try to create one.
                if (null == m_viewPlan && null != m_level)
                {
                    try
                    {
                        m_viewPlan = m_doc.Create.NewAreaViewPlan("Level 1", m_level,AreaElemType.BOMAArea);
                    }
                    catch
                    {                        
                    }                                      
                }
                
                if (null == m_level)
                {
                    return;
                }

                //Create enclosed region using Walls for Area 

                //Create RectangularWallCreationData for Walls' batch creation 
                List<RectangularWallCreationData> rectangularWallCreationDatas = new List<RectangularWallCreationData>();
                
                //List of Curve is used to store Area's boundary lines
                List<Curve> curves = new List<Curve>();

                XYZ pt1 = new XYZ(-5, 95, 0);
                XYZ pt2 = new XYZ(-105, 95, 0);
                Line line = appCreation.NewLine(pt1, pt2, true);
                RectangularWallCreationData rectangularWallCreationData = null;
                if (null != line)
                {
                    rectangularWallCreationData = new RectangularWallCreationData(line, m_level, true);
                    curves.Add(line);
                }
                if (null != rectangularWallCreationData)
                {
                    rectangularWallCreationDatas.Add(rectangularWallCreationData);
                }

                pt1 = new XYZ(-5, 105, 0);
                pt2 = new XYZ(-105, 105, 0);
                line = appCreation.NewLine(pt1, pt2, true);
                if (null != line)
                {
                    rectangularWallCreationData = new RectangularWallCreationData(line, m_level, true);
                    curves.Add(line);
                }
                if (null != rectangularWallCreationData)
                {
                    rectangularWallCreationDatas.Add(rectangularWallCreationData);
                }

                for (int i = 0; i < 11; i++)
                {
                    pt1 = new XYZ(-5 - i * 10, 95, 0);
                    pt2 = new XYZ(-5 - i * 10, 105, 0);
                    line = appCreation.NewLine(pt1, pt2, true);
                    if (null != line)
                    {
                        rectangularWallCreationData = new RectangularWallCreationData(line, m_level, true);
                        curves.Add(line);
                    }
                    if (null != rectangularWallCreationData)
                    {
                        rectangularWallCreationDatas.Add(rectangularWallCreationData);
                    }
                }

                // Create Area Boundary Line for Area
                // It is necessary if need to create closed region for Area
                // But for Room, it is not necessary.
                XYZ origin = new XYZ(0, 0, 0);
                XYZ norm = new XYZ(0, 0, 1);
                Plane plane = appCreation.NewPlane(norm, origin);
                if(null != plane)
                {
                    SketchPlane sketchPlane = m_doc.Create.NewSketchPlane(plane);
                    if(null != sketchPlane)
                    {
                        foreach (Curve curve in curves)
                        {
                            m_doc.Create.NewAreaBoundaryLine(sketchPlane, curve, m_viewPlan);
                        }
                    }
                }


                //Create enclosed region using Walls for Room                
                pt1 = new XYZ(5, -5, 0);
                pt2 = new XYZ(55, -5, 0);
                line = appCreation.NewLine(pt1, pt2, true);
                rectangularWallCreationData = null;
                if (null != line)
                {
                    rectangularWallCreationData = new RectangularWallCreationData(line, m_level, true);
                }
                if (null != rectangularWallCreationData)
                {
                    rectangularWallCreationDatas.Add(rectangularWallCreationData);
                }

                pt1 = new XYZ(5, 5, 0);
                pt2 = new XYZ(55, 5, 0);
                line = appCreation.NewLine(pt1, pt2, true);
                if (null != line)
                {
                    rectangularWallCreationData = new RectangularWallCreationData(line, m_level, true);
                }
                if (null != rectangularWallCreationData)
                {
                    rectangularWallCreationDatas.Add(rectangularWallCreationData);
                }

                for (int i = 0; i < 6; i++)
                {
                    pt1 = new XYZ(5 + i * 10, -5, 0);
                    pt2 = new XYZ(5 + i * 10, 5, 0);
                    line = appCreation.NewLine(pt1, pt2, true);
                    if (null != line)
                    {
                        rectangularWallCreationData = new RectangularWallCreationData(line, m_level, true);
                    }
                    if (null != rectangularWallCreationData)
                    {
                        rectangularWallCreationDatas.Add(rectangularWallCreationData);
                    }
                }   

                // Call Wall batch creation method to create Walls
                m_doc.Create.NewWalls(rectangularWallCreationDatas);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Batch creation of Areas
        /// </summary>
        /// <returns>If batch creation succeeds, return true; otherwise, return false</returns>
        private bool CreateAreas()
        {
            try
            {
                if (null == m_viewPlan)
                {
                    return false;
                }

                List<AreaCreationData> areaCreationDatas = new List<AreaCreationData>();
                //Create AreaCreateDatas for Areas' batch creation 
                for (int i = 1; i < 11; i++)
                {
                    UV point = new UV(i * -10, 100);
                    UV tagPoint = new UV(i * -10, 100);
                    AreaCreationData areaCreationData = new AreaCreationData(m_viewPlan, point);
                    if (null != areaCreationData)
                    {
                        areaCreationData.TagPoint = tagPoint;
                        areaCreationDatas.Add(areaCreationData);
                    }
                }

                // Create Areas
                if (0 == areaCreationDatas.Count)
                {
                    return false;
                }
                m_doc.Create.NewAreas(areaCreationDatas);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Batch creation of Columns
        /// </summary>
        /// <returns>If batch creation succeeds, return true; otherwise, return false</returns>
        private bool CreateColumns()
        {
            try
            {
                List<FamilyInstanceCreationData> fiCreationDatas = new List<FamilyInstanceCreationData>();

                if (null == m_level)
                {
                    return false;
                }

                //Try to get a FamilySymbol
                FamilySymbol familySymbol = null;
                ElementIterator iter = m_doc.get_Elements(typeof(FamilySymbol));
                iter.Reset();
                while (iter.MoveNext())
                {
                    familySymbol = iter.Current as FamilySymbol;
                    if (null != familySymbol && null != familySymbol.Category && "Structural Columns" == familySymbol.Category.Name)
                    {
                        break;
                    }
                }

                if (null == familySymbol)
                {
                    return false;
                }

                //Create FamilyInstanceCreationData for FamilyInstances' batch creation 
                for (int i = 1; i < 11; i++)
                {
                    XYZ location = new XYZ(i * 10, 100,0);
                    FamilyInstanceCreationData fiCreationData =
                        new FamilyInstanceCreationData(location, familySymbol, m_level, Autodesk.Revit.Structural.Enums.StructuralType.Column);
                    if (null != fiCreationData)
                    {
                        fiCreationDatas.Add(fiCreationData);
                    }
                }

                if(0 == fiCreationDatas.Count)
                {
                    return false;
                }

                // Create Columns
                m_doc.Create.NewFamilyInstances(fiCreationDatas);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
       
        /// <summary>
        /// Batch creation of Rooms
        /// </summary>
        /// <returns>If batch creation succeeds, return true; otherwise, return false</returns>
        private bool CreateRooms()
        {
            try
            {
                if (null == m_level)
                {
                    return false;
                }

                //Try to get Phase used to create Rooms
                Phase phase = null;
                ElementIterator iterator = m_doc.get_Elements(typeof(Phase));
                iterator.Reset();
                while (iterator.MoveNext())
                {
                    Phase temp = iterator.Current as Phase;
                    if (null != temp)
                    {
                        phase = temp;
                        break;
                    }
                }

                if (null == phase)
                {
                    return false;
                }

                // Create AreaCreateDatas for Rooms' batch creation 
                List<RoomCreationData> roomCreationDatas = new List<RoomCreationData>();
                for (int i = 1; i < 6; i++)
                {
                    UV point = new UV(i * 10, 0);
                    UV tagPoint = new UV(i * 10, 0);
                    RoomCreationData roomCreationData = new RoomCreationData(m_level, point);
                    if (null != roomCreationData)
                    {
                        roomCreationData.TagPoint = tagPoint;
                        roomCreationDatas.Add(roomCreationData);
                    }
                }

                // Create Rooms
                if (0 == roomCreationDatas.Count)
                {
                    return false;
                }
                m_doc.Create.NewRooms(roomCreationDatas);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Batch creation of TextNotes
        /// </summary>
        /// <returns>If batch creation succeeds, return true; otherwise, return false</returns>
        private bool CreateTextNotes()
        {
            try
            {
                List<TextNoteCreationData> textNoteCreationDatas = new List<TextNoteCreationData>();

                //Try to get View named "Level 1" where the TextNotes are
                View view = null;
                ElementIterator iter = m_doc.get_Elements(typeof(ViewPlan));
                iter.Reset();
                while (iter.MoveNext())
                {
                    View temp = iter.Current as View;
                    if (null != temp && null != temp.Name && "Level 1" == temp.Name && ViewType.FloorPlan == temp.ViewType)
                    {
                        view = temp;
                        break;
                    }
                }

                if (null == view)
                {
                    return false;
                }

                //Create TextNoteCreationData for TextNotes' batch creation 
                for (int i = 1; i < 6; i++)
                {
                    XYZ origin = new XYZ(i * -20, -100 , 0);
                    XYZ baseVec = new XYZ(1, 0, 0);
                    XYZ upVec = new XYZ(0, 0, 1);
                    TextNoteCreationData textNoteCreationData = new TextNoteCreationData(view,origin,baseVec,upVec,10,TextAlignFlags.TEF_ALIGN_CENTER,"TextNote");
                    if (null != textNoteCreationData)
                    {
                        textNoteCreationDatas.Add(textNoteCreationData);
                    }
                }

                // Create TextNotes
                m_doc.Create.NewTextNotes(textNoteCreationDatas);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Batch creation of Walls
        /// </summary>
        /// <returns>If batch creation succeeds, return true; otherwise, return false</returns>
        private bool CreateWalls()
        {
            try
            {
                if (null == m_level)
                {
                    return false;
                }

                //Create ProfiledWallCreationData for Walls' batch creation 
                List<ProfiledWallCreationData> profiledWallCreationDatas = new List<ProfiledWallCreationData>();
                Autodesk.Revit.Creation.Application appCreation = m_cmdData.Application.Create;
                for (int i = 1; i < 11; i++)
                {
                    // Create wall's profile which is a combination of rectangle and arc
                    CurveArray curveArray = appCreation.NewCurveArray();

                    // Create three lines for rectangle part of profile.
                    XYZ pt1 = new XYZ(i * 10, -80, 15);
                    XYZ pt2 = new XYZ(i * 10, -80, 0);
                    XYZ pt3 = new XYZ(i * 10, -90, 0);
                    XYZ pt4 = new XYZ(i * 10, -90, 15);
                    Line line1 = appCreation.NewLine(pt1, pt2, true);
                    Line line2 = appCreation.NewLine(pt2, pt3, true);
                    Line line3 = appCreation.NewLine(pt3, pt4, true);

                    // Create arc part of profile.
                    XYZ pointInCurve = new XYZ(i * 10, -85, 20);
                    Arc arc = appCreation.NewArc(pt4, pt1, pointInCurve);

                    if(null == line1 || null == line2 || null == line3 || null == arc)
                    {
                        continue;
                    }
                    curveArray.Append(line1);
                    curveArray.Append(line2);
                    curveArray.Append(line3);
                    curveArray.Append(arc);
                    
                    ProfiledWallCreationData profiledWallCreationData = new ProfiledWallCreationData(curveArray,true);
                    if (null != profiledWallCreationData)
                    {
                        profiledWallCreationDatas.Add(profiledWallCreationData);
                    }
                }

                //Create RectangularWallCreationData for Walls' batch creation 
                List<RectangularWallCreationData> rectangularWallCreationDatas = new List<RectangularWallCreationData>();
                for (int i = 1; i < 11; i++)
                {
                    XYZ pt1 = new XYZ(i*10,-110,0);
                    XYZ pt2 = new XYZ(i*10,-120,0);
                    Line line = appCreation.NewLine(pt1,pt2,true);
                    RectangularWallCreationData rectangularWallCreationData = null;
                    if(null != line)
                    {
                        rectangularWallCreationData = new RectangularWallCreationData(line, m_level, true);
                    }                    
                    if (null != rectangularWallCreationData)
                    {
                        rectangularWallCreationDatas.Add(rectangularWallCreationData);
                    }
                }

                // Create Walls
                if(0 == profiledWallCreationDatas.Count || 0 == rectangularWallCreationDatas.Count)
                {
                    return false;
                }
                m_doc.Create.NewWalls(profiledWallCreationDatas);
                m_doc.Create.NewWalls(rectangularWallCreationDatas);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}
