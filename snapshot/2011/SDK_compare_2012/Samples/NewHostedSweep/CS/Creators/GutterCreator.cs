//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit;
using System.Windows.Forms;
using System.Collections;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// Provides functions to create Gutter.
    /// </summary>
    public class GutterCreator : HostedSweepCreator
    {
        /// <summary>
        /// Constructor with Revit.Document as parameter.
        /// </summary>
        /// <param name="rvtDoc">Revit document</param>
        public GutterCreator(Autodesk.Revit.UI.UIDocument rvtDoc)
            : base(rvtDoc)
        {
        }

        /// <summary>
        /// Edges which gutter can be created on.
        /// </summary>
        private Dictionary<Autodesk.Revit.DB.Element, List<Edge>> m_roofGutterEdges;

        /// <summary>
        /// Filter all the edges from the element which gutter can be created on.
        /// </summary>
        /// <param name="elem"></param>
        private void FilterEdgesForGutter(Autodesk.Revit.DB.Element elem)
        {
            Transaction transaction = new Transaction(this.RvtDocument, "FilterEdgesForGutter");
            transaction.Start();

            // Note: This method will create a Gutter with no reference.
            // In the future, API may not allow to create such Gutter with 
            // no references, invoke this methods like this may throw exception.
            // 
            Gutter gutter = m_rvtDoc.Create.NewGutter(null, new ReferenceArray());

            List<Edge> roofEdges = m_roofGutterEdges[elem];
            foreach (Edge edge in m_elemGeom[elem].EdgeBindingDic.Keys)
            {
                if (edge.Reference == null) continue;
                try
                {
                    gutter.AddSegment(edge.Reference);
                    // AddSegment successfully, so this edge can be used to crate Gutter.
                    roofEdges.Add(edge);
                }
                catch (Autodesk.Revit.Exceptions.ArgumentOutOfRangeException)
                {
                    // Exception, this edge will be discard.
                }
            }
            // Delete this element, because we just use it to filter the edges.
            m_rvtDoc.Delete(gutter);

            transaction.RollBack();
        }

        /// <summary>
        /// A string indicates this creator just for Roof Gutter creation.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Roof Gutter";
            }
        }

        /// <summary>
        /// All Gutter types in Revit active document.
        /// </summary>
        public override IEnumerable AllTypes
        {
            get { return m_rvtDoc.GutterTypes; }
        }

        /// <summary>
        /// Dictionary to store all the Roof=>Edges which Gutter can be created on.
        /// </summary>
        public override Dictionary<Autodesk.Revit.DB.Element, List<Edge>> SupportEdges
        {
            get
            {
                if (m_roofGutterEdges == null)
                {
                    m_roofGutterEdges = new Dictionary<Autodesk.Revit.DB.Element, List<Edge>>();

                    FilteredElementIterator iter = (new FilteredElementCollector(m_rvtDoc)).OfClass(typeof(FootPrintRoof)).GetElementIterator();                    
                    int i = 0;
                    do
                    {
                        while (iter.MoveNext())
                        {
                            Autodesk.Revit.DB.Element elem = iter.Current as Autodesk.Revit.DB.Element;
                            if (elem is RoofBase)
                            {
                                ElementGeometry solid = ExtractGeom(elem);
                                if (solid != null)
                                {
                                    m_roofGutterEdges.Add(elem, new List<Edge>());
                                    m_elemGeom.Add(elem, solid);                                    
                                    FilterEdgesForGutter(elem);
                                }
                            }
                        }
                        iter = (new FilteredElementCollector(m_rvtDoc)).OfClass(typeof(ExtrusionRoof)).GetElementIterator(); 
                        ++i;
                    }
                    while (i < 2);
                }
                return m_roofGutterEdges;
            }
        }

        /// <summary>
        /// Create a Gutter.
        /// </summary>
        /// <param name="symbol">Gutter type</param>
        /// <param name="refArr">Gutter Reference array</param>
        /// <returns>Created Gutter</returns>
        protected override HostedSweep CreateHostedSweep(ElementType symbol, ReferenceArray refArr)
        {
            Gutter gutter = m_rvtDoc.Create.NewGutter(symbol as GutterType, refArr);
            return gutter;
        }
    }
}
