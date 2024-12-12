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
using System.Linq;
using System.Text;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using System.Diagnostics;
using System.IO;
using System.Collections;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.Ribbon.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand, create a wall
    /// all the properties for new wall comes from user selection in Ribbon
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class CreateWall : IExternalCommand
    {
        public static ElementSet CreatedWalls = new ElementSet(); //restore all the walls created by API.

        #region IExternalCommand Members Implementation
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            Autodesk.Revit.UI.UIApplication app = revit.Application;

            WallType newWallType = GetNewWallType(app); //get WallType from RadioButtonGroup - WallTypeSelector
            Level newWallLevel = GetNewWallLevel(app); //get Level from Combobox - LevelsSelector
            CurveArray newWallShape = GetNewWallShape(app); //get wall Curve from Combobox - WallShapeComboBox
            String newWallMark = GetNewWallMark(app); //get mark of new wall from Text box - WallMark

            Wall newWall = null;
            if ("CreateStructureWall" == this.GetType().Name) //decided by SplitButton
            { newWall = app.ActiveUIDocument.Document.Create.NewWall(newWallShape, newWallType, newWallLevel, true); }
            else { newWall = app.ActiveUIDocument.Document.Create.NewWall(newWallShape, newWallType, newWallLevel, false); }
            if (null != newWall)
            {
                newWall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).Set(newWallMark); //set new wall's mark
                CreatedWalls.Insert(newWall);
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        #endregion IExternalCommand Members Implementation

        #region protected methods
        protected WallType GetNewWallType(Autodesk.Revit.UI.UIApplication app)
        {
            RibbonPanel myPanel = app.GetRibbonPanels()[0];
            RadioButtonGroup radioGroupTypeSelector =
                GetRibbonItemByName(myPanel, "WallTypeSelector") as RadioButtonGroup;
             if (null == radioGroupTypeSelector) { throw new InvalidCastException("Cannot get Wall Type selector!"); }
             String wallTypeName = radioGroupTypeSelector.Current.ItemText;
             WallType newWallType = null;
             FilteredElementCollector collector = new FilteredElementCollector(app.ActiveUIDocument.Document);
             ICollection<Element> founds = collector.OfClass(typeof(WallType)).ToElements();
             foreach (Element elem in founds)
             {
                WallType wallType = elem as WallType;
                if (wallType.Name.StartsWith(wallTypeName))
                {
                   newWallType = wallType; break;
                }
             }

             return newWallType;
        }

        protected Level GetNewWallLevel(Autodesk.Revit.UI.UIApplication app)
        {
            RibbonPanel myPanel = app.GetRibbonPanels()[0];
            Autodesk.Revit.UI.ComboBox comboboxLevel =
                GetRibbonItemByName(myPanel, "LevelsSelector") as Autodesk.Revit.UI.ComboBox;
            if (null == comboboxLevel) { throw new InvalidCastException("Cannot get Level selector!"); }
            String wallLevel = comboboxLevel.Current.ItemText;
            //find wall type in document
            Level newWallLevel = null;
            FilteredElementCollector collector = new FilteredElementCollector(app.ActiveUIDocument.Document);
            ICollection<Element> founds = collector.OfClass(typeof(Level)).ToElements();
            foreach (Element elem in founds)
            {
               Level level = elem as Level;
               if (level.Name.StartsWith(wallLevel))
               {
                  newWallLevel = level; break;
               }
            }

            return newWallLevel;
        }

        protected CurveArray GetNewWallShape(Autodesk.Revit.UI.UIApplication app)
        {
            RibbonPanel myPanel = app.GetRibbonPanels()[0];
            Autodesk.Revit.UI.ComboBox comboboxWallShape =
                GetRibbonItemByName(myPanel, "WallShapeComboBox") as Autodesk.Revit.UI.ComboBox;
            if (null == comboboxWallShape) { throw new InvalidCastException("Cannot get Wall Shape Gallery!"); }
            String wallShape = comboboxWallShape.Current.ItemText;
            if ("SquareWall" == wallShape) { return GetSquareWallShape(app.Application.Create); }
            else if ("CircleWall" == wallShape) { return GetCircleWallShape(app.Application.Create); }
            else if ("TriangleWall" == wallShape) { return GetTriangleWallShape(app.Application.Create); }
            else { return GetRectangleWallShape(app.Application.Create); }
        }

        protected String GetNewWallMark(Autodesk.Revit.UI.UIApplication app)
        {
            RibbonPanel myPanel = app.GetRibbonPanels()[0];
            Autodesk.Revit.UI.TextBox textBox =
                GetRibbonItemByName(myPanel, "WallMark") as Autodesk.Revit.UI.TextBox;
            if (null == textBox) { throw new InvalidCastException("Cannot get Wall Mark TextBox!"); }
            String newWallMark;
            int newWallIndex = 0;
            FilteredElementCollector collector = new FilteredElementCollector(app.ActiveUIDocument.Document);
            ICollection<Element> founds = collector.OfClass(typeof(Wall)).ToElements();
            foreach (Element elem in founds)
            {
                Wall wall = elem as Wall;
                string wallMark = wall.get_Parameter(BuiltInParameter.ALL_MODEL_MARK).AsString();
                if (wallMark.StartsWith(textBox.Value.ToString()) && wallMark.Contains('_'))
                {
                    //get the index for new wall (wall_1, wall_2...)
                    char[] chars = { '_' };
                    string[] strings = wallMark.Split(chars);
                    if (strings.Length >= 2)
                    {
                        try
                        {
                            int index = Convert.ToInt32(strings[strings.Length - 1]);
                            if (index > newWallIndex) { newWallIndex = index; }
                        }
                        catch (System.Exception)
                        {
                            continue;
                        }
                    }
                }
            }
            newWallMark = textBox.Value.ToString() + '_' + (newWallIndex + 1);
            return newWallMark;
        }

        protected CurveArray GetRectangleWallShape(Autodesk.Revit.Creation.Application creApp)
        {
            //calculate size of Structural and NonStructural walls
            int WallsSize = CreateStructureWall.CreatedWalls.Size + CreatedWalls.Size;
            CurveArray curves = creApp.NewCurveArray();
            //15: distance from each wall, 60: wall length , 60: wall width 
            Line line1 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 0), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 60, 0), true);
            Line line2 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 60, 0), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 60, 40), true);
            Line line3 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 60, 40), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 40), true);
            Line line4 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 40), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 0), true);
            curves.Append(line1);
            curves.Append(line2);
            curves.Append(line3);
            curves.Append(line4);
            return curves;
        }

        protected CurveArray GetSquareWallShape(Autodesk.Revit.Creation.Application creApp)
        {
            //calculate size of Structural and NonStructural walls
            int WallsSize = CreateStructureWall.CreatedWalls.Size + CreatedWalls.Size;
            CurveArray curves = creApp.NewCurveArray();
            //15: distance from each wall, 40: wall length  
            Line line1 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 0), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 40, 0), true);
            Line line2 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 40, 0), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 40, 40), true);
            Line line3 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 40, 40), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 40), true);
            Line line4 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 40), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 0), true);
            curves.Append(line1);
            curves.Append(line2);
            curves.Append(line3);
            curves.Append(line4);
            return curves;
        }

        protected CurveArray GetCircleWallShape(Autodesk.Revit.Creation.Application creApp)
        {
            //calculate size of Structural and NonStructural walls
            int WallsSize = CreateStructureWall.CreatedWalls.Size + CreatedWalls.Size;
            CurveArray curves = creApp.NewCurveArray();
            //15: distance from each wall, 40: diameter of circle  
            Arc arc = creApp.NewArc(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 20, 0), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 20, 40), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 40, 20));
            Arc arc2 = creApp.NewArc(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 20, 0), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 20, 40), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 20));
            curves.Append(arc);
            curves.Append(arc2);
            return curves;
        }

        protected CurveArray GetTriangleWallShape(Autodesk.Revit.Creation.Application creApp)
        {
            //calculate size of Structural and NonStructural walls
            int WallsSize = CreateStructureWall.CreatedWalls.Size + CreatedWalls.Size;
            CurveArray curves = creApp.NewCurveArray();
            //15: distance from each wall, 40: height of triangle  
            Line line1 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 0), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 40, 0), true);
            Line line2 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 40, 0), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 20, 40), true);
            Line line3 = creApp.NewLine(new Autodesk.Revit.DB.XYZ (WallsSize * 15, 20, 40), new Autodesk.Revit.DB.XYZ (WallsSize * 15, 0, 0), true);
            curves.Append(line1);
            curves.Append(line2);
            curves.Append(line3);
            return curves;
        }
        #endregion

        /// <summary>
        /// return the RibbonItem by the input name in a specific panel
        /// </summary>
        /// <param name="panelRibbon">RibbonPanel which contains the RibbonItem </param>
        /// <param name="itemName">name of RibbonItem</param>
        /// <return>RibbonItem whose name is same with input string</param>
        public RibbonItem GetRibbonItemByName(RibbonPanel panelRibbon, String itemName)
        {
            foreach (RibbonItem item in panelRibbon.GetItems())
            {
                if (itemName == item.Name)
                {
                    return item;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,create a structural wall
    /// all the properties for new wall comes from user selection in Ribbon
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CreateStructureWall : CreateWall
    {
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand, 
    /// delete all the walls which create by Ribbon sample
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class DeleteWalls : IExternalCommand
    {
        #region IExternalCommand Members Implementation
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            // delete all the walls which create by RibbonSample
            revit.Application.ActiveUIDocument.Document.Delete(CreateWall.CreatedWalls);
            CreateWall.CreatedWalls.Clear();

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,Move walls, X direction
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class XMoveWalls : IExternalCommand
    {
        #region IExternalCommand Members Implementation

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            IEnumerator iter = CreateWall.CreatedWalls.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                Wall wall = iter.Current as Wall;
                if (null != wall)
                {
                    wall.Location.Move(new Autodesk.Revit.DB.XYZ (12, 0, 0));
                }
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,Move walls, Y direction
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class YMoveWalls : IExternalCommand
    {
        #region IExternalCommand Members Implementation

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            IEnumerator iter = CreateWall.CreatedWalls.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                Wall wall = iter.Current as Wall;
                if (null != wall)
                {
                    wall.Location.Move(new Autodesk.Revit.DB.XYZ (0, 12, 0));
                }
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,
    /// Reset all the Ribbon options to default, such as level, wall type...
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ResetSetting : IExternalCommand
    {
        #region IExternalCommand Members Implementation

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            RibbonPanel myPanel = revit.Application.GetRibbonPanels()[0];
            //reset wall type
            RadioButtonGroup radioGroupTypeSelector =
                GetRibbonItemByName(myPanel, "WallTypeSelector") as RadioButtonGroup;
            if (null == radioGroupTypeSelector) { throw new InvalidCastException("Cannot get Wall Type selector!"); }
            radioGroupTypeSelector.Current = radioGroupTypeSelector.GetItems()[0];

            //reset level
            Autodesk.Revit.UI.ComboBox comboboxLevel =
                GetRibbonItemByName(myPanel, "LevelsSelector") as Autodesk.Revit.UI.ComboBox;
            if (null == comboboxLevel) { throw new InvalidCastException("Cannot get Level selector!"); }
            comboboxLevel.Current = comboboxLevel.GetItems()[0];

            //reset wall shape
            Autodesk.Revit.UI.ComboBox comboboxWallShape =
                GetRibbonItemByName(myPanel, "WallShapeComboBox") as Autodesk.Revit.UI.ComboBox;
            if (null == comboboxLevel) { throw new InvalidCastException("Cannot get wall shape combo box!"); }
            comboboxWallShape.Current = comboboxWallShape.GetItems()[0];

            //get wall mark
            Autodesk.Revit.UI.TextBox textBox = 
                GetRibbonItemByName(myPanel, "WallMark") as Autodesk.Revit.UI.TextBox;
            if (null == textBox) { throw new InvalidCastException("Cannot get Wall Mark TextBox!"); }
            textBox.Value = "new wall";

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        /// <summary>
        /// return the RibbonItem by the input name in a specific panel
        /// </summary>
        /// <param name="panelRibbon">RibbonPanel which contains the RibbonItem </param>
        /// <param name="itemName">name of RibbonItem</param>
        /// <return>RibbonItem whose name is same with input string</param>
        public RibbonItem GetRibbonItemByName(RibbonPanel panelRibbon, String itemName)
        {
            foreach (RibbonItem item in panelRibbon.GetItems())
            {
                if (itemName == item.Name)
                {
                    return item;
                }
            }

            return null;
        }

        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Do Nothing, 
    /// Create this just because ToggleButton have to bind to a ExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Dummy : IExternalCommand
    {
        #region IExternalCommand Members Implementation

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            return Autodesk.Revit.UI.Result.Succeeded;
        }

        #endregion IExternalCommand Members Implementation
    }
}
