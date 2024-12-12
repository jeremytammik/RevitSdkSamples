//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using Autodesk.Revit.UI;
using Autodesk.Revit;
using Autodesk.Revit.DB.Architecture;
using System.Drawing.Design;
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
        /// Revit UI document.
        /// </summary>
        private UIDocument m_rvtUIDoc;

        /// <summary>
        /// Sub transaction
        /// </summary>
        Transaction m_transaction;


        /// <summary>
        /// Constructor with HostedSweep and CreationData as parameters.
        /// </summary>
        /// <param name="elem">Element to modify</param>
        /// <param name="creationData">CreationData</param>
        public ModificationData(HostedSweep elem, CreationData creationData)
        {
            m_rvtDoc = creationData.Creator.RvtDocument;
            m_rvtUIDoc = creationData.Creator.RvtUIDocument;
            m_elemToModify = elem;
            m_creationData = creationData;

            m_transaction = new Transaction(m_rvtDoc, "External Tool");

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
        private void m_creationData_SymbolChanged(ElementType sym)
        {
            try
            {
                StartTransaction();
                m_elemToModify.ChangeTypeId(sym.Id);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
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
                StartTransaction();
                m_elemToModify.RemoveSegment(edge.Reference);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
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
                StartTransaction();
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
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
            }
        }

        /// <summary>
        /// Show the element in a good view.
        /// </summary>
        public void ShowElement()
        {
            try
            {
                StartTransaction();
                m_rvtUIDoc.ShowElements(m_elemToModify);
                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
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
                String result = "[Id:" + m_elemToModify.Id.ToString() + "] ";
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
                    StartTransaction();
                    Parameter angle = GetParameter("Angle");
                    if (angle != null)
                        angle.SetValueString(value);
                    else
                        m_elemToModify.Angle = double.Parse(value);
                    CommitTransaction();
                }
                catch
                {
                    RollbackTransaction();
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
                        StartTransaction();
                        m_elemToModify.HorizontalFlip();
                        CommitTransaction();
                    }
                    catch
                    {
                        RollbackTransaction();
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
                    StartTransaction();
                    Parameter horiOff = GetParameter("Horizontal Profile Offset");
                    if (horiOff != null)
                        horiOff.SetValueString(value);
                    else
                        m_elemToModify.HorizontalOffset = double.Parse(value); 
                    CommitTransaction();
                }
                catch
                {
                    RollbackTransaction();
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
                        StartTransaction();
                        m_elemToModify.VerticalFlip();
                        CommitTransaction();
                    }
                    catch
                    {
                        RollbackTransaction();
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
                    StartTransaction();
                    Parameter vertOff = GetParameter("Vertical Profile Offset");
                    if (vertOff != null)
                        vertOff.SetValueString(value);
                    else 
                        m_elemToModify.VerticalOffset = double.Parse(value); 
                    CommitTransaction();
                }
                catch
                {
                    RollbackTransaction();
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
            return m_elemToModify.LookupParameter(name);
        }


        public TransactionStatus StartTransaction()
        {
            return m_transaction.Start();
        }

        public TransactionStatus CommitTransaction()
        {
            return m_transaction.Commit();
        }

        public TransactionStatus RollbackTransaction()
        {
            return m_transaction.RollBack();
        }
    }
}
