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
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.GenericStructuralConnection.CS
{
    /// <summary>
    /// Utility class to select connections or connection input elements. 
    /// </summary>
    class StructuralConnectionSelectionUtils
    {
        /// <summary>
        /// Static method to select structural connections.
        /// </summary>
        /// <returns>Returns the id of the connection.</returns>
        public static StructuralConnectionHandler SelectConnection(UIDocument document)
        {
            StructuralConnectionHandler conn = null;
            // Create a filter for structural connections.
            LogicalOrFilter types = new LogicalOrFilter(new List<ElementFilter> { new ElementCategoryFilter(BuiltInCategory.OST_StructConnections) });
            StructuralConnectionSelectionFilter filter = new StructuralConnectionSelectionFilter(types);
            Reference target = document.Selection.PickObject(ObjectType.Element, filter, "Select connection element :");
            if (target != null)
            {
                Element targetElement = document.Document.GetElement(target);
                if (targetElement != null)
                {
                    conn = targetElement as StructuralConnectionHandler;
                }
            }

            return conn;
        }
        /// <summary>
        /// Static method o select valid input element for the structural connection.
        /// </summary>
        /// <param name="document"> Current document. </param>
        /// <returns>Returns a list of element ids.</returns>
        public static List<ElementId> SelectConnectionElements(UIDocument document)
        {
            List<ElementId> elemIds = new List<ElementId>();

            // Create a filter for the allowed structural connection inputs.
            LogicalOrFilter connElemTypes = new LogicalOrFilter(new List<ElementFilter>{
            new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming),
            new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns),
            new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation),
            new ElementCategoryFilter(BuiltInCategory.OST_Floors),
            new ElementCategoryFilter(BuiltInCategory.OST_Walls)});
            StructuralConnectionSelectionFilter elemFilter = new StructuralConnectionSelectionFilter(connElemTypes);
            
            List<Reference> refs = document.Selection.PickObjects(ObjectType.Element, elemFilter, "Select elements to add to connection :").ToList();
            elemIds = refs.Select(e => e.ElementId).ToList();

            return elemIds;
        }
    }
}
