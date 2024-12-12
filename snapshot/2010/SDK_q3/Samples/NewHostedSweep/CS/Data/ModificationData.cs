//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using Autodesk.Revit.Elements;
using Autodesk.Revit;
using System.Drawing.Design;
using Autodesk.Revit.Geometry;
using System.ComponentModel;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This class contains the data for hosted sweep modification.
    /// </summary>
    public class ModificationData
    {
        /// <summary>
        /// Element to modify.
        /// </summary>
        private HostedSweep m_elemToModify;

        /// <summary>
        /// Creation data can be modified.
        /// </summary>
        private CreationData m_creationData;

        /// <summary>
        /// Revit active document.
        /// </summary>
        private Document m_rvtDoc;      

        /// <summary>
        /// Constructor with HostedSweep and CreationData as parameters.
        /// </summary>
        /// <param name="elem">Element to modify</param>
        /// <param name="creationData">CreationData</param>
        public ModificationData(HostedSweep elem, CreationData creationData)
        {
            m_rvtDoc = creationData.Creator.RvtDocument;
            m_elemToModify = elem;
            m_creationData = creationData;

            m_creationData.EdgeAdded +=
                new CreationData.EdgeEventHandler(m_creationData_EdgeAdded);
            m_creationData.EdgeRemoved += 
                new CreationData.EdgeEventHandler(m_creationData_EdgeRemoved);
            m_creationData.SymbolChanged += 
                new CreationData.SymbolChangedEventHandler(m_creationData_SymbolChanged);
        }

        /// <summary>
        /// Name of the Creator.
        /// </summary>
        public string CreatorName
        {
            get
            {
                return m_creationData.Creator.Name;
            }
        }

        /// <summary>
        /// Change the symbol of the HostedSweep.
        /// </summary>
        /// <param name="sym"></param>
        private void m_creationData_SymbolChanged(Symbol sym)
        {
            try
            {
                m_rvtDoc.BeginTransaction();
                m_elemToModify.ObjectType = sym;
                m_rvtDoc.EndTransaction();
            }
            catch
            {
                m_rvtDoc.AbortTransaction();
            }

        }

        /// <summary>
        /// Remove the edge from the HostedSweep.
        /// </summary>
        /// <param name="edge"></param>
        private void m_creationData_EdgeRemoved(Edge edge)
        {
            try
            {
                m_rvtDoc.BeginTransaction();
                m_elemToModify.RemoveSegment(edge.Reference);
                m_rvtDoc.EndTransaction();
            }
            catch
            {
                m_rvtDoc.AbortTransaction();
            }
        }

        /// <summary>
        /// Add the edge to the HostedSweep.
        /// </summary>
        /// <param name="edge"></param>
        private void m_creationData_EdgeAdded(Edge edge)
        {
            try
            {    
                m_rvtDoc.BeginTransaction();
                if (m_elemToModify is Fascia)
                {
                    (m_elemToModify as Fascia).AddSegment(edge.Reference);
                }
                else if (m_elemToModify is Gutter)
                {
                    (m_elemToModify as Gutter).AddSegment(edge.Reference);
                }
                else if (m_elemToModify is SlabEdge)
                {
                    (m_elemToModify as SlabEdge).AddSegment(edge.Reference);
                }
                m_rvtDoc.EndTransaction();
            }
            catch
            {
                m_rvtDoc.AbortTransaction();
            }
        }

        /// <summary>
        /// Show the element in a good view.
        /// </summary>
        public void ShowElement()
        {
            try
            {
                m_rvtDoc.BeginTransaction();
                m_rvtDoc.ShowElements(m_elemToModify);
                m_rvtDoc.EndTransaction();
            }
            catch
            {
                m_rvtDoc.AbortTransaction();
            }
        }

        /// <summary>
        /// Name will be displayed in property grid.
        /// </summary>
        [Category("Identity Data")]
        public String Name
        {
            get
            {
                String result = "[Id:" + m_elemToModify.Id.Value + "] ";
                return result + m_elemToModify.Name;
            }
        }

        /// <summary>
        /// HostedSweep Angle property.
        /// </summary>
        [Category("Profile")]
        public String Angle
        {
            get
            {
                Parameter angle = GetParameter("Angle");
                if (angle != null)
                    return angle.AsValueString();
                else
                    return m_elemToModify.Angle.ToString();
            }
            set
            {
                try
                {
                    m_rvtDoc.BeginTransaction();
                    Parameter angle = GetParameter("Angle");
                    if (angle != null)
                        angle.SetValueString(value);
                    else
                        m_elemToModify.Angle = double.Parse(value);
                    m_rvtDoc.EndTransaction();
                }
                catch
                {
                    m_rvtDoc.AbortTransaction();
                }
            }
        }

        /// <summary>
        /// HostedSweep profiles edges, the edges can be removed or added in the 
        /// pop up dialog.
        /// </summary>
        [TypeConverter(typeof(CreationDataTypeConverter)),
        Editor(typeof(EdgeFormUITypeEditor), typeof(UITypeEditor)),
        Category("Profile"), DisplayName("Profile Edges")]
        public CreationData AddOrRemoveSegments
        {
            get
            {
                return m_creationData;
            }
        }

        /// <summary>
        /// HostedSweep Length property.
        /// </summary>
        [Category("Dimensions")]
        public string Length
        {            
            get
            {
                Parameter length = GetParameter("Length");
                if (length != null)
                    return length.AsValueString();
                else
                    return m_elemToModify.Length.ToString();
            }
        }

        /// <summary>
        /// HostedSweep HorizontalFlipped property.
        /// </summary>
        [Category("Constraints"), DisplayName("Horizontal Profile Flipped")]
        public bool HorizontalFlipped
        {
            get { return m_elemToModify.HorizontalFlipped; }
            set
            {
                if (value != m_elemToModify.HorizontalFlipped)
                {
                    try
                    {
                        m_rvtDoc.BeginTransaction();
                        m_elemToModify.HorizontalFlip();
                        m_rvtDoc.EndTransaction();
                    }
                    catch
                    {
                        m_rvtDoc.AbortTransaction();
                    }
                }
            }
        }

        /// <summary>
        /// HostedSweep HorizontalOffset property.
        /// </summary>
        [Category("Constraints"), DisplayName("Horizontal Profile Offset")]
        public String HorizontalOffset
        {
            get
            {
                Parameter horiOff = GetParameter("Horizontal Profile Offset");
                if (horiOff != null)
                    return horiOff.AsValueString();
                else
                    return m_elemToModify.HorizontalOffset.ToString(); 
            }
            set
            {
                try
                {
                    m_rvtDoc.BeginTransaction();
                    Parameter horiOff = GetParameter("Horizontal Profile Offset");
                    if (horiOff != null)
                        horiOff.SetValueString(value);
                    else
                        m_elemToModify.HorizontalOffset = double.Parse(value); 
                    m_rvtDoc.EndTransaction();
                }
                catch
                {
                    m_rvtDoc.AbortTransaction();
                }
            }
        }

        /// <summary>
        /// HostedSweep VerticalFlipped property.
        /// </summary>
        [Category("Constraints"), DisplayName("Vertical Profile Flipped")]
        public bool VerticalFlipped
        {
            get
            { 
                return m_elemToModify.VerticalFlipped; 
            }
            set
            {
                if (value != m_elemToModify.VerticalFlipped)
                {
                    try
                    {
                        m_rvtDoc.BeginTransaction();
                        m_elemToModify.VerticalFlip();
                        m_rvtDoc.EndTransaction();
                    }
                    catch
                    {
                        m_rvtDoc.AbortTransaction();
                    }
                }
            }
        }

        /// <summary>
        /// HostedSweep VerticalOffset property.
        /// </summary>
        [Category("Constraints"), DisplayName("Vertical Profile Offset")]
        public String VerticalOffset
        {
            get
            {
                Parameter vertOff = GetParameter("Vertical Profile Offset");
                if (vertOff != null)
                    return vertOff.AsValueString();
                else
                    return m_elemToModify.VerticalOffset.ToString(); 
            }
            set 
            {
                try
                {
                    m_rvtDoc.BeginTransaction();
                    Parameter vertOff = GetParameter("Vertical Profile Offset");
                    if (vertOff != null)
                        vertOff.SetValueString(value);
                    else 
                        m_elemToModify.VerticalOffset = double.Parse(value); 
                    m_rvtDoc.EndTransaction();
                }
                catch
                {
                    m_rvtDoc.AbortTransaction();
                }
            }
        }

        /// <summary>
        /// Get parameter by given name.
        /// </summary>
        /// <param name="name">name of parameter</param>
        /// <returns>parameter whose definition name is the given name.</returns>
        protected Parameter GetParameter(String name)
        {
            return m_elemToModify.get_Parameter(name);
        }
    }
}
