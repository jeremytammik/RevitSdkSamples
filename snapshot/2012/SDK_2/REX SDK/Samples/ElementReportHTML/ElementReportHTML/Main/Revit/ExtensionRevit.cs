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

namespace REX.ElementReportHTML.Main.Revit
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

        public override void OnCreate()
        {
            ThisMainExtension.Data.MainNode = new Node() { Level = Node.levelRoot };

            //The module is only interested in elements which aren't types, have geometry and aren't of a DetailComponents category.
            FilteredElementCollector collector = new FilteredElementCollector(ThisExtension.Revit.ActiveDocument);
            List<Element> elemList = new List<Element>(collector.WhereElementIsNotElementType().ToElements());

            foreach (Element el in elemList)
            {
                if (el.Category == null || !el.CanHaveTypeAssigned())
                    continue;

                Autodesk.Revit.DB.GeometryElement geomElement = el.get_Geometry(new Options());
                BuiltInCategory bic = (BuiltInCategory)el.Category.Id.IntegerValue;
                if ((geomElement == null) || bic == BuiltInCategory.OST_DetailComponents
                    || bic == BuiltInCategory.OST_Views || bic == BuiltInCategory.OST_Cameras)
                    continue;

                Node nodeCategory = null;
                try
                {
                    nodeCategory = ThisMainExtension.Data.MainNode.GetNode(el.Category.Name);
                }
                catch
                {
                    continue;
                }

                if (nodeCategory == null)
                {
                    nodeCategory = ThisMainExtension.Data.MainNode.AddNode(el.Category.Name, el.Category.Name, Node.levelCategory, "");
                    ThisMainExtension.Data.SelectedCategories.Add(nodeCategory);
                }

                Node nodeElement = nodeCategory.AddNode(el.Name, el.Id.ToString(), Node.levelElement, "");

                foreach (Parameter param in el.Parameters)
                {
                    if (param.Definition.Name.Contains("Extensions."))
                        continue;

                    string valueString = "";

                    if (param.StorageType == StorageType.Double)
                        valueString = param.AsValueString();
                    else if (param.StorageType == StorageType.String)
                        valueString = param.AsString();
                    else if (param.StorageType == StorageType.ElementId)
                    {
                        ElementId id = param.AsElementId();
                        Element elem = ThisMainExtension.Revit.ActiveDocument.get_Element(id);
                        if (elem != null)
                            valueString = elem.Name;
                    }

                    if (!string.IsNullOrEmpty(valueString))
                    {
                        nodeElement.AddNode(param.Definition.Name, param.Definition.Name, Node.levelParameter, valueString);
                    }
                }
            }
        }      

        #endregion
    }
}
