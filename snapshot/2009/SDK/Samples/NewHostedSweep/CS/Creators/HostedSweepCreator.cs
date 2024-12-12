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
using Autodesk.Revit.Geometry;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Elements;
using Autodesk.Revit;
using System.Windows.Forms;
using System.Collections;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// Provides functions to create hosted sweep and preserves available edges and type.
    /// It is the base class of FasciaCreator, GutterCreator, and SlabEdgeCreator.
    /// </summary>
    public abstract class HostedSweepCreator
    {
        #region Public Interfaces

        /// <summary>
        /// A string indicates which type this creator can create.
        /// </summary>
        virtual public String Name
        {
            get
            {
                return "Hosted Sweep";
            }
        }

        /// <summary>
        /// A dictionary stores all the element=>edges which hosted-sweep can be created on.
        /// </summary>
        public abstract Dictionary<Autodesk.Revit.Element, List<Edge>> SupportEdges
        {
            get;
        }

        /// <summary>
        /// All type of hosted-sweep.
        /// </summary>
        public abstract IEnumerable AllTypes
        {
            get;
        }

        /// <summary>
        /// A dictionary stores all the element=>geometry which hosted-sweep can be created on.
        /// </summary>
        public Dictionary<Autodesk.Revit.Element, ElementGeometry> ElemGeomDic
        {
            get { return m_elemGeom; }
        }

        /// <summary>
        /// Create a hosted-sweep according to the CreationData parameter.
        /// </summary>
        /// <param name="creationData">CreationData parameter</param>
        /// <returns>ModificationData which contains the created hosted-sweep</returns>
        public ModificationData Create(CreationData creationData)
        {
            ReferenceArray refArr = new ReferenceArray();
            foreach (Edge edge in creationData.EdgesForHostedSweep)
            {
                refArr.Append(edge.Reference);
            }

            ModificationData modificationData = null;
            try
            {
                m_rvtDoc.BeginTransaction();
                HostedSweep createdHostedSweep = CreateHostedSweep(creationData.Symbol, refArr);
                m_rvtDoc.ShowElements(createdHostedSweep);                
                if (m_rvtDoc.EndTransaction())
                {// just only end transaction return true, we will create the hosted sweep.                    
                    modificationData =
                        new ModificationData(createdHostedSweep, creationData);
                    m_createdHostedSweeps.Add(modificationData);
                }         
            }
            catch
            {
                m_rvtDoc.AbortTransaction();
            }
            return modificationData;
        }

        /// <summary>
        /// A list to store all the created hosted-sweep by this creator.
        /// </summary>
        public List<ModificationData> CreatedHostedSweeps
        {
            get { return m_createdHostedSweeps; }
        }

        /// <summary>
        /// Revit active document.
        /// </summary>
        public Autodesk.Revit.Document RvtDocument
        {
            get
            {
                return m_rvtDoc;
            }
        }
        #endregion

        #region Fields and Constructor
        /// <summary>
        /// List of Modification to store all the created hosted-sweep by this.
        /// </summary>
        private List<ModificationData> m_createdHostedSweeps;

        /// <summary>
        /// Revit active document.
        /// </summary>
        protected Document m_rvtDoc;

        /// <summary>
        /// Dictionary to store element's geometry which this creator can be used.
        /// </summary>
        protected Dictionary<Autodesk.Revit.Element, ElementGeometry> m_elemGeom;

        /// <summary>
        /// Constructor which takes a Revit.Document as parameter.
        /// </summary>
        /// <param name="rvtDoc">Revit.Document parameter</param>
        protected HostedSweepCreator(Autodesk.Revit.Document rvtDoc)
        {
            m_rvtDoc = rvtDoc;
            m_elemGeom = new Dictionary<Autodesk.Revit.Element, ElementGeometry>();
            m_createdHostedSweeps = new List<ModificationData>();
        }
        #endregion

        #region Protected Methods

        /// <summary>
        /// Create a hosted-sweep according to the given Symbol and ReferenceArray.
        /// </summary>
        /// <param name="symbol">Hosted-sweep Symbol</param>
        /// <param name="refArr">Hosted-sweep ReferenceArray</param>
        /// <returns>Created hosted-sweep</returns>
        protected abstract HostedSweep CreateHostedSweep(Symbol symbol, ReferenceArray refArr);

        /// <summary>
        /// Extract the geometry of the given Element.
        /// </summary>
        /// <param name="elem">Element parameter</param>
        /// <returns>Element's geometry</returns>
        protected ElementGeometry ExtractGeom(Autodesk.Revit.Element elem)
        {
            Solid result = null;
            Options options = new Options();
            options.ComputeReferences = true;
            Autodesk.Revit.Geometry.Element gElement = elem.get_Geometry(options);
            foreach (GeometryObject gObj in gElement.Objects)
            {
                result = gObj as Solid;
                if (result != null && result.Faces.Size > 0)
                    break;
            }
            BoundingBoxXYZ box = elem.get_BoundingBox(null);
            return new ElementGeometry(result, box);
        }
        #endregion
    }
}
