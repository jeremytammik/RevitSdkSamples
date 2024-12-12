//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using Autodesk.REX.Framework;
using REX.Common;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace REX.FrameGenerator.Main.Revit
{
    internal class ExtensionRevit : REXExtensionProduct
    {
        public ExtensionRevit(REX.Common.REXExtension Ext)
            : base(Ext)
        {
        }

        /// <summary>
        /// Get the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }

        /// <summary>
        /// Get the Data structure.
        /// </summary>
        /// <value>The main Data.</value>
       internal Data ThisMainData
        {
            get
            {
                return ThisMainExtension.Data;
            }
        }
        #region REXExtensionProduct Members

        public override bool OnInitialize()
        {
            // insert code here.

            Beams = new List<FamilySymbol>();
            Columns = new List<FamilySymbol>();
            Footings = new List<FamilySymbol>();
            Levels = new List<Level>();
            return true;
        }

        public override void OnCreate()
        {
            // insert code here.
        }

        public override void OnCreateDialogs()
        {
            // insert code here.
        }

        public override void OnCreateLayout()
        {
            // insert code here.
        }

        public override void OnSetDialogs()
        {
            // insert code here.
        }

        public override void OnSetData()
        {
            // insert code here.
        }

        public override void OnActivateLayout(REX.Common.REXLayoutItem LItem, bool Activate)
        {
            // insert code here.
        }

        public override bool OnCanRun()
        {
            // insert code here.

            return true;
        }

        public override void OnRun()
        {

            if (FindTwoLevels() && FindFamilySymbol())
            {
                FamilyInstance Column = null;
                FamilyInstance Foundation = null;
                FamilyInstance Beam = null;
                Curve beamCurve = null;
                Level baseLevel = Levels[0] as Level;
                Level topLevel = Levels[1] as Level;
                Autodesk.Revit.DB.XYZ startPoint = new XYZ(0, 0, 0);
                Autodesk.Revit.DB.XYZ endPoint = new XYZ(0, 0, 0);

                // generate footing and beam 
                for (int i = 0; i < ThisMainExtension.Data.NbX; i++)
                {
                    for (int j = 0; j < ThisMainExtension.Data.NbY; j++)
                    {
                        startPoint = new XYZ(i * ThisMainExtension.Data.SpacingX,
                                        j * ThisMainExtension.Data.SpacingY, 0);
                        Foundation = ThisExtension.Revit.ActiveDocument.Create.NewFamilyInstance(
                            startPoint, Footings[0], baseLevel, Autodesk.Revit.DB.Structure.StructuralType.Footing);
                        Column = ThisExtension.Revit.ActiveDocument.Create.NewFamilyInstance(
                            startPoint, Columns[0], baseLevel, Autodesk.Revit.DB.Structure.StructuralType.Column);
                    }

                }

                // generate beams on X direction
                for (int i = 0; i < ThisMainExtension.Data.NbX; i++)
                {
                    for (int j = 0; j < ThisMainExtension.Data.NbY - 1; j++)
                    {
                        startPoint = new XYZ(i * ThisMainExtension.Data.SpacingX,
                                        j * ThisMainExtension.Data.SpacingY,
                                        topLevel.Elevation);
                        endPoint = new XYZ(i * ThisMainExtension.Data.SpacingX,
                                        (j + 1) * ThisMainExtension.Data.SpacingY,
                                        topLevel.Elevation);
                        beamCurve = ThisExtension.Revit.Application().Create.NewLineBound(startPoint, endPoint);
                        Beam = ThisExtension.Revit.ActiveDocument.Create.NewFamilyInstance(beamCurve,
                             Beams[0], topLevel, Autodesk.Revit.DB.Structure.StructuralType.Beam);
                    }
                }
                // generate beams on Y direction
                for (int j = 0; j < ThisMainExtension.Data.NbY; j++)
                {
                    for (int i = 0; i < ThisMainExtension.Data.NbX - 1; i++)
                    {
                        startPoint = new XYZ(i * ThisMainExtension.Data.SpacingX,
                                        j * ThisMainExtension.Data.SpacingY,
                                        topLevel.Elevation);
                        endPoint = new XYZ((i + 1) * ThisMainExtension.Data.SpacingX,
                                    j * ThisMainExtension.Data.SpacingY,
                                    topLevel.Elevation);
                        beamCurve = ThisExtension.Revit.Application().Create.NewLineBound(startPoint, endPoint);
                        Beam = ThisExtension.Revit.ActiveDocument.Create.NewFamilyInstance(beamCurve,
                             Beams[0], topLevel, Autodesk.Revit.DB.Structure.StructuralType.Beam);

                    }
                }

            }
        }
        public bool FindTwoLevels()
        {
            FilteredElementIterator fei = new FilteredElementCollector(ThisExtension.Revit.ActiveUIDocument.Document).OfClass(typeof(Level)).GetElementIterator();
            fei.Reset();
            int nblevel = 0;
            while (fei.MoveNext())
            {
                Level level = fei.Current as Level;
                if (null != level)
                {
                    Levels.Add(level);
                    nblevel++;
                    if (nblevel > 1)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool FindFamilySymbol()
        {
            // Categories names
            string strBeamCat = ThisExtension.Revit.ActiveDocument.Settings.Categories.get_Item(Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFraming).Name;
            string strColCat = ThisExtension.Revit.ActiveDocument.Settings.Categories.get_Item(Autodesk.Revit.DB.BuiltInCategory.OST_StructuralColumns).Name;
            string strFootCat = ThisExtension.Revit.ActiveDocument.Settings.Categories.get_Item(Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFoundation).Name;
            bool mboolBeam = false;
            bool mboolColumn = false;
            bool mboolFoot = false;

            FilteredElementIterator fei = new FilteredElementCollector(ThisExtension.Revit.ActiveUIDocument.Document).OfClass(typeof(Family)).GetElementIterator();
            while (fei.MoveNext())
            {
                Family f = fei.Current as Family;
                if (f != null)
                {
                    foreach (object symbol in f.Symbols)
                    {
                        FamilySymbol familyType = symbol as FamilySymbol;
                        if (null == familyType)
                        {
                            continue;
                        }
                        if (null == familyType.Category)
                        {
                            continue;
                        }
                        if (strBeamCat == familyType.Category.Name)
                        {
                            Beams.Add(familyType);
                            mboolBeam = true;
                        }
                        else if (strColCat == familyType.Category.Name)
                        {
                            Columns.Add(familyType);
                            mboolColumn = true;
                        }
                        if (strFootCat == familyType.Category.Name)
                        {
                            Footings.Add(familyType);
                            mboolFoot = true;
                        }
                    }
                }
            }
            if (mboolBeam && mboolColumn && mboolFoot)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public override bool OnCanClose()
        {
            // insert code here.

            return true;
        }

        public override bool OnCanClose(REX.Common.REXCanCloseType Type)
        {
            // insert code here.

            return true;
        }

        public override void OnClose()
        {
            // insert code here.
        }

        public override void OnPreferencesChanged()
        {
            // insert code here.
        }

        public override string OnGetText(string Name)
        {
            // insert code here.

            return "";
        }

        public override object OnCommand(ref REXCommand CommandStruct)
        {
            // insert code here.

            return null;
        }

        public override object OnEvent(ref REXEvent EventStruct)
        {
            // insert code here.

            return null;
        }

        public override void OnUpdateDialogs()
        {
            // insert code here.
        }

        private List<FamilySymbol> Beams;
        private List<FamilySymbol> Columns;
        private List<FamilySymbol> Footings;
        private List<Level> Levels;

        #endregion
    }
}
