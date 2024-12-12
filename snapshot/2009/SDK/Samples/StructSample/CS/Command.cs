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
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Parameters;


namespace Revit.SDK.Samples.StructSample.CS
{
    /// <summary>
    ///   This command places a set of coulmns in the selected wall.
    ///   Note that Revit uses Feet as an internal length unit. 
    ///   
    ///   To run this sample, 
    ///   (1) load the column family type of "M_Wood Timber Column", "191 x 292mm" 
    ///       (It is hard-coded in the program.) 
    ///   (2) Draw some walls, and constrain their top and bottom to the levels in the properties dialog. 
    ///   (3) Run the command. 
    ///       It will place columns along each wall with the interval of 5 feet. (The interval is also hard coded.) 
    /// </summary>
    public class Command : IExternalCommand
    {
        #region Interface implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref String message, ElementSet elements)
        {
            try
            {
                Autodesk.Revit.Application rvtApp = commandData.Application;
                Autodesk.Revit.Document rvtDoc = rvtApp.ActiveDocument;
                Autodesk.Revit.ElementSet ss = rvtDoc.Selection.Elements;

                Autodesk.Revit.ElementSet walls = rvtApp.Create.NewElementSet();

                //  iterate through a selection set, and collect walls which are constrained at the top and the bottom.
                foreach (Autodesk.Revit.Element elem in ss)
                {
                    if (elem.GetType() == typeof(Autodesk.Revit.Elements.Wall))
                    {
                        if (elem.get_Parameter(Autodesk.Revit.Parameters.BuiltInParameter.WALL_HEIGHT_TYPE) != null
                            && elem.get_Parameter(Autodesk.Revit.Parameters.BuiltInParameter.WALL_BASE_CONSTRAINT) != null)
                        {
                            walls.Insert(elem);
                        }
                    }
                }

                //  how many did we get? 
                MessageBox.Show("# of constrained walls in the selection set is " + walls.Size);
                if (walls.Size == 0)
                {
                    message = "You must select some walls that are constrained top or bottom";
                    return IExternalCommand.Result.Failed;
                }

                //  next, we need a column symbol. For simplicity, the symbol name is hard-coded here. 
                Autodesk.Revit.Symbols.FamilySymbol colType = FindFamilySymbol(rvtDoc, "M_Wood Timber Column", "191 x 292mm");
                if (colType == null)
                {
                    MessageBox.Show("failed to got a symbol. Please load the M_Wood Timber Column : 191 x 292mm family");
                    message = "Please load the M_Wood Timber Column : 191 x 292mm family";

                    return IExternalCommand.Result.Failed;
                }

                //  place columns.
                double spacing = 5;  //  Spacing in feet hard coded. Note: Revit's internal length unit is feet. 
                foreach (Autodesk.Revit.Elements.Wall wall in walls)
                {
                    FrameWall(rvtApp, wall, spacing, colType);
                }

                //  return succeeded info. 
                return IExternalCommand.Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.ToString();
                return IExternalCommand.Result.Failed;
            }
        }
        #endregion


        /// <summary>
        /// find Column which will be used to placed to Wall
        /// </summary>
        /// <param name="rvtDoc">Revit document</param>
        /// <param name="familyName">Family name of Column</param>
        /// <param name="symbolName">Symbol of Collumn</param>
        /// <returns></returns>
        private FamilySymbol FindFamilySymbol(Document rvtDoc, string familyName, string symbolName)
        {
            Autodesk.Revit.ElementIterator itr = rvtDoc.Elements;

            while (itr.MoveNext())
            {
                Autodesk.Revit.Element elem = (Autodesk.Revit.Element)itr.Current;
                if (elem.GetType() == typeof(Autodesk.Revit.Elements.Family))
                {
                    if (elem.Name == familyName)
                    {
                        Autodesk.Revit.Elements.Family family = (Autodesk.Revit.Elements.Family)elem;
                        foreach (Autodesk.Revit.Symbols.FamilySymbol symbol in family.Symbols)
                        {
                            if (symbol.Name == symbolName)
                            {
                                return symbol;
                            }
                        }
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Frame a Wall
        /// </summary>
        /// <param name="rvtApp">Revit application></param>
        /// <param name="wall">Wall as host to place column objects</param>
        /// <param name="spacing">spacing between two columns</param>
        /// <param name="columnType">column type</param>
        private void FrameWall(Autodesk.Revit.Application rvtApp, Autodesk.Revit.Elements.Wall wall,
            double spacing, Autodesk.Revit.Symbols.FamilySymbol columnType)
        {
            Autodesk.Revit.Document rvtDoc = wall.Document;

            // get wall location
            Autodesk.Revit.LocationCurve loc = (Autodesk.Revit.LocationCurve)wall.Location;
            Autodesk.Revit.Geometry.XYZ startPt = loc.Curve.get_EndPoint(0);
            Autodesk.Revit.Geometry.XYZ endPt = loc.Curve.get_EndPoint(1);

            // get wall's vector
            Autodesk.Revit.Geometry.UV wallVec = new Autodesk.Revit.Geometry.UV();
            wallVec.U = endPt.X - startPt.X;
            wallVec.V = endPt.Y - startPt.Y;

            // get the axis vector
            Autodesk.Revit.Geometry.UV axis = new Autodesk.Revit.Geometry.UV(1.0, 0.0);

            Autodesk.Revit.ElementId baseLevelId = wall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT).AsElementId();
            Autodesk.Revit.ElementId topLevelId = wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).AsElementId();

            // get wall length and vector
            double wallLength = wallVec.Length;
            wallVec = wallVec.Normalized;

            // get # of column
            int nmax = (int)(wallLength / spacing);

            MessageBox.Show("wallLength = " + wallLength + "\r\nspacing = " + spacing.ToString() + "\r\nnmax = " + nmax.ToString());

            // get angle of wall and axis
            double angle = wallVec.Angle(axis);

            // place all column
            Autodesk.Revit.Geometry.XYZ loc2 = startPt;
            double dx = wallVec.U * spacing;
            double dy = wallVec.V * spacing;
            for (int i = 0; i < nmax; i++)
            {
                PlaceColumn(rvtApp, rvtDoc, loc2, angle, columnType, baseLevelId, topLevelId);
                loc2.X += dx;
                loc2.Y += dy;
            }
            
            // place column at end point of wall
            PlaceColumn(rvtApp, rvtDoc, endPt, angle, columnType, baseLevelId, topLevelId);
        }


        /// <summary>
        /// create a column instance and place it on the wall line.
        /// </summary>
        /// <param name="rvtApp">revit application</param>
        /// <param name="rvtDoc">revit document</param>
        /// <param name="point2">location for placing column</param>
        /// <param name="angle">column angle</param>
        /// <param name="columnType">column type placed in Wall</param>
        /// <param name="baseLevelId">level id for base level where column is placed</param>
        /// <param name="topLevelId">level id for top level where column is placed</param>
        private void PlaceColumn(Autodesk.Revit.Application rvtApp, Document rvtDoc, Autodesk.Revit.Geometry.XYZ point2,
            double angle, FamilySymbol columnType, ElementId baseLevelId, ElementId topLevelId)
        {
            Autodesk.Revit.Geometry.XYZ point = point2;

            // Note: Must use level-hosted NewFamilyInstance!
            Level instLevel = (Level)rvtDoc.get_Element(ref baseLevelId);
            Autodesk.Revit.Elements.FamilyInstance column = rvtDoc.Create.NewFamilyInstance(point, columnType,
                instLevel, Autodesk.Revit.Structural.Enums.StructuralType.Column);
            if (column == null)
            {
                MessageBox.Show("failed to create an instance of a column.");
                return;
            }

            // rotate column to place it to right location
            Autodesk.Revit.Geometry.XYZ zVec = new Autodesk.Revit.Geometry.XYZ(0, 0, 1);
            Autodesk.Revit.Geometry.Line axis = rvtApp.Create.NewLineUnbound(point, zVec);
            column.Location.Rotate(axis, angle);

            // Set the level Ids
            Autodesk.Revit.Parameter baseLevelParameter = column.get_Parameter(Autodesk.Revit.Parameters.BuiltInParameter.FAMILY_BASE_LEVEL_PARAM);
            Autodesk.Revit.Parameter topLevelParameter = column.get_Parameter(Autodesk.Revit.Parameters.BuiltInParameter.FAMILY_TOP_LEVEL_PARAM); ;
            baseLevelParameter.Set(ref baseLevelId);
            topLevelParameter.Set(ref topLevelId);
        }
    }
}