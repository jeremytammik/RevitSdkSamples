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
using System.Text;

using Autodesk.REX.Framework;
using REX.Common;
using Autodesk.Revit.DB;

namespace REX.PyramidGenerator.Main.Revit
{
    //Step 4: Extension class
    internal class ExtensionRevit : REXExtensionProduct
    {
        /// <summary>
        /// The list of read families
        /// </summary>
        Dictionary<string, FamilySymbol> Families;
        /// <summary>
        /// The destination level
        /// </summary>
        Level LevelReference;
        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionRevit"/> class.
        /// </summary>
        /// <param name="Ext">The ext.</param>
        public ExtensionRevit(REX.Common.REXExtension Ext)
            : base(Ext)
        {
            Families = new Dictionary<string, FamilySymbol>();
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

        #region IREXExtension Members implemented
        /// <summary>
        /// Creates needed structures and verify needs.
        /// </summary>
        public override void OnCreate()
        {
            //Step 4.2.: Extension class
            FilteredElementCollector collector = new FilteredElementCollector(ThisExtension.Revit.ActiveDocument);

            //Reading elements which FamilySymbols of the Framing category
            IList<ElementFilter> filterList = new List<ElementFilter>();
            filterList.Add(new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming));
            filterList.Add(new ElementClassFilter(typeof(FamilySymbol)));
            LogicalAndFilter filter = new LogicalAndFilter(filterList);

            IList<Element> elements = collector.WherePasses(filter).ToElements();

            if (elements == null || elements.Count == 0)
            {
                ThisExtension.System.SystemBase.Errors.AddError("Error1", "No families were detected in the project", null);
                return;
            }
            else
            {
                Families = new Dictionary<string, FamilySymbol>();

                foreach (Element el in elements)
                {
                    FamilySymbol familySymbol = el as FamilySymbol;

                    string familyName = familySymbol.Family.Name + ": " + familySymbol.Name;
                    Families.Add(familyName, familySymbol);
                    ThisMainExtension.Data.AvailableFamilySymbols.Add(familyName);
                }
            }

            //Reading the level - the first one is taken
            collector = new FilteredElementCollector(ThisExtension.Revit.ActiveDocument);
            LevelReference = collector.OfClass(typeof(Level)).FirstElement() as Level;
            if (LevelReference == null)
            {
                ThisExtension.System.SystemBase.Errors.AddError("Error2", "No level was detected", null);
                return;
            }

            //Loading data from the selection
            foreach (ElementId id in ThisExtension.Revit.ActiveUIDocument.Selection.GetElementIds())
            {
                Element el = ThisExtension.Revit.ActiveUIDocument.Document.GetElement(id);
                if (el != null)
                    if (ThisExtension.Revit.Parameters.LoadFromHost(el))
                    {
                        break;
                    }
            }
        }
        /// <summary>
        /// Runs process calculations or other operations.
        /// </summary>
        public override void OnRun()
        {
            //Step 4.9.: Extension class
            double b = ThisExtension.Units.Interface(ThisMainExtension.Data.B, EUnitType.Dimensions_StructureDim, 1, REXInterfaceType.Revit);
            double h = ThisExtension.Units.Interface(ThisMainExtension.Data.H, EUnitType.Dimensions_StructureDim, 1, REXInterfaceType.Revit);

            double x1 = 0;
            double y1 = 0;

            double x2 = b;
            double y2 = 0;

            double x3 = b / 2;
            double y3 = b * Math.Sqrt(3) / 2;

            double x4 = b / 2;
            double y4 = y3 / 3;

            FamilySymbol familySymbol = Families[ThisMainExtension.Data.FamilySymbol];

            ThisExtension.Progress.Steps = 6;
            ThisExtension.Progress.Header = "Generation";
            ThisExtension.Progress.Text = "...";
            ThisExtension.Progress.Show(ThisExtension.GetWindowForParent());

            RemoveAllGeneratedBeams();

            ThisMainExtension.Data.ElementIds.Clear();

            GenerateBeam(new XYZ(x1, y1, 0), new XYZ(x2, y2, 0), familySymbol);
            GenerateBeam(new XYZ(x2, y2, 0), new XYZ(x3, y3, 0), familySymbol);
            GenerateBeam(new XYZ(x1, y1, 0), new XYZ(x3, y3, 0), familySymbol);
            GenerateBeam(new XYZ(x1, y1, 0), new XYZ(x4, y4, h), familySymbol);
            GenerateBeam(new XYZ(x2, y2, 0), new XYZ(x4, y4, h), familySymbol);
            GenerateBeam(new XYZ(x3, y3, 0), new XYZ(x4, y4, h), familySymbol);

            SaveDataToAllBeams();

            ThisExtension.Progress.Hide();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Removes all generated beams.
        /// </summary>
        void RemoveAllGeneratedBeams()
        {
            //Step 5.3.2.: Serialize
            foreach (int id in ThisMainExtension.Data.ElementIds)
            {
                Element el = ThisExtension.Revit.ActiveDocument.GetElement(new ElementId(id));
                if (el != null)
                    ThisExtension.Revit.ActiveDocument.Delete(el.Id);
            }
        }
        /// <summary>
        /// Saves the data to all beams.
        /// </summary>
        void SaveDataToAllBeams()
        {
            //Step 5.3.4.: Serialize
            foreach (int id in ThisMainExtension.Data.ElementIds)
            {
                Element el = ThisExtension.Revit.ActiveDocument.GetElement(new ElementId(id));
                if (el != null)
                    ThisExtension.Revit.Parameters.SaveToHost(el);
            }
        }
        /// <summary>
        /// Generates the beam.
        /// </summary>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="familySymbol">The FamilySymbol.</param>
        void GenerateBeam(XYZ start, XYZ end, FamilySymbol familySymbol)
        {
            ThisExtension.Progress.Step();
            Line line = Line.CreateBound(start, end);
            
            if (!familySymbol.IsActive)
                familySymbol.Activate();

            FamilyInstance familyInstance = ThisExtension.Revit.ActiveDocument.Create.NewFamilyInstance(line, familySymbol, LevelReference, Autodesk.Revit.DB.Structure.StructuralType.Brace);
            if (familyInstance != null)
            {
                //Step 5.3.3.: Serialize
                ThisMainExtension.Data.ElementIds.Add(familyInstance.Id.IntegerValue);
            }
        }
        #endregion

    }
}
