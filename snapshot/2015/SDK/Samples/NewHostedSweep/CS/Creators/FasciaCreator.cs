//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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
    /// Provides functions to create Fascia.
    /// </summary>
    public class FasciaCreator : HostedSweepCreator
    {
        /// <summary>
        /// Constructor which take Revit.Document as parameter.
        /// </summary>
        /// <param name="rvtDoc">Revit document</param>
        public FasciaCreator(Autodesk.Revit.UI.UIDocument rvtDoc)
            : base(rvtDoc)
        {
        }

        /// <summary>
        /// Dictionary to store the roof=>edges for fascia creation.
        /// </summary>
        private Dictionary<Autodesk.Revit.DB.Element, List<Edge>> m_roofFasciaEdges;

        /// <summary>
        /// Filter all the edges of the given element for fascia creation.
        /// </summary>
        /// <param name="elem">Element used to filter edges which fascia can be created on</param>
        private void FilterEdgesForFascia(Autodesk.Revit.DB.Element elem)
        {
            Transaction transaction = new Transaction(this.RvtDocument, "FilterEdgesForFascia");
            transaction.Start();

            // Note: This method will create a Fascia with no references.
            // In the future, API may not allow to create such Fascia with 
            // no references, invoke this methods like this may throw exception.
            // 
            Fascia fascia = m_rvtDoc.Create.NewFascia(null, new ReferenceArray());

            List<Edge> roofEdges = m_roofFasciaEdges[elem];
            foreach (Edge edge in m_elemGeom[elem].EdgeBindingDic.Keys)
            {
                if (edge.Reference == null) continue;
                try
                {
                    fascia.AddSegment(edge.Reference);
                    // AddSegment successfully, so this edge can be used to crate Fascia.
                    roofEdges.Add(edge);
                }
                catch (Autodesk.Revit.Exceptions.ArgumentOutOfRangeException)
                {
                    // Exception, this edge will be discard.
                }
            }
            // Delete this element, because we just use it to filter the edges.
            m_rvtDoc.Delete(fascia.Id);

            transaction.RollBack();
        }

        /// <summary>
        /// A string indicates this creator just for Roof Fascia creation.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Roof Fascia";
            }
        }

        /// <summary>
        /// All fascia types in Revit active document.
        /// </summary>
        public override IEnumerable AllTypes
        {
            get
            {
                FilteredElementCollector filteredElementCollector = new FilteredElementCollector(m_rvtDoc);
                filteredElementCollector.OfClass(typeof(FasciaType));
                return filteredElementCollector;
            }
        }

        /// <summary>
        /// Dictionary to store all the Roof=>Edges which Fascia can be created on.
        /// </summary>
        public override Dictionary<Autodesk.Revit.DB.Element, List<Edge>> SupportEdges
        {
            get
            {
                if (m_roofFasciaEdges == null)
                {
                    m_roofFasciaEdges = new Dictionary<Autodesk.Revit.DB.Element, List<Edge>>();
                    FilteredElementCollector collector = new FilteredElementCollector(m_rvtDoc);
                    collector.OfClass(typeof(FootPrintRoof));
                    IList<Element> elements = collector.ToElements();

                    collector = new FilteredElementCollector(m_rvtDoc);
                    collector.OfClass(typeof(ExtrusionRoof));
                    foreach (Element elem in collector)
                    {
                        elements.Add(elem);
                    }

                    foreach (Element elem in elements)
                    {
                        if (elem is RoofBase)
                        {
                            ElementGeometry solid = ExtractGeom(elem);
                            if (solid != null)
                            {
                                m_roofFasciaEdges.Add(elem, new List<Edge>());
                                m_elemGeom.Add(elem, solid);
                                FilterEdgesForFascia(elem);
                            }
                        }
                    }
                }
                return m_roofFasciaEdges;
            }
        }

        /// <summary>
        /// Create a Fascia.
        /// </summary>
        /// <param name="symbol">Fascia type</param>
        /// <param name="refArr">Fascia reference array</param>
        /// <returns>Created Fascia</returns>
        protected override HostedSweep CreateHostedSweep(ElementType symbol, ReferenceArray refArr)
        {
            Fascia fascia = m_rvtDoc.Create.NewFascia(symbol as FasciaType, refArr);

            return fascia;
        }
    }
}
