//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using Autodesk.Revit;
using Autodesk.Revit.DB;
using System.Drawing.Design;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This class contains the data for hosted sweep creation.
    /// </summary>
    public class CreationData
    {
        /// <summary>
        /// Represents the method that will handle the EdgeAdded or EdgeRemoved events
        /// of CreationData
        /// </summary>
        /// <param name="edge">Edge</param>
        public delegate void EdgeEventHandler(Edge edge);

        /// <summary>
        /// Represents the method that will handle the SymbolChanged events
        /// of CreationData
        /// </summary>
        /// <param name="sym">Symbol</param>
        public delegate void SymbolChangedEventHandler(ElementType sym);

        /// <summary>
        /// Edge is added to HostedSweep.
        /// </summary>
        public event EdgeEventHandler EdgeAdded;

        /// <summary>
        /// Edge is removed from HostedSweep.
        /// </summary>
        public event EdgeEventHandler EdgeRemoved;

        /// <summary>
        /// HostedSweep symbol is changed.
        /// </summary>
        public event SymbolChangedEventHandler SymbolChanged;

        /// <summary>
        /// Creator contains the necessary data to fetch the edges and get the symbol.
        /// </summary>
        private HostedSweepCreator m_creator;

        /// <summary>
        /// Symbol for HostedSweep creation.
        /// </summary>
        private ElementType m_symbol;

        /// <summary>
        /// Edges which contains references for HostedSweep creation.
        /// </summary>
        private List<Edge> m_edgesForHostedSweep = new List<Edge>();

        /// <summary>
        /// Back up of Symbol.
        /// </summary>
        private ElementType m_backUpSymbol;

        /// <summary>
        /// Back up of Edges.
        /// </summary>
        private List<Edge> m_backUpEdges = new List<Edge>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="creator">HostedSweepCreator</param>
        public CreationData(HostedSweepCreator creator)
        {
            m_creator = creator;
        }

        /// <summary>
        /// Back up the Symbol and Edges.
        /// </summary>
        public void BackUp()
        {
            m_backUpSymbol = m_symbol;
            m_backUpEdges.Clear();
            m_backUpEdges.AddRange(m_edgesForHostedSweep);            
        }

        /// <summary>
        /// Restore the Symbol and Edges.
        /// </summary>
        public void Restore()
        {
            m_symbol = m_backUpSymbol;
            m_edgesForHostedSweep.Clear();
            m_edgesForHostedSweep.AddRange(m_backUpEdges);
        }

        /// <summary>
        /// If CreationData changed, notify its observers.
        /// </summary>
        public void Update()
        {
            if (SymbolChanged != null && m_backUpSymbol != null &&
                m_backUpSymbol.Id.IntegerValue != m_symbol.Id.IntegerValue)
                SymbolChanged(m_symbol);

            if (EdgeRemoved != null)
            {
                foreach (Edge edge in m_backUpEdges)
                {
                    if (m_edgesForHostedSweep.IndexOf(edge) == -1)
                    {
                        EdgeRemoved(edge);
                    }
                }
            }

            if(EdgeAdded != null)
            {
                foreach (Edge edge in m_edgesForHostedSweep)
                {
                    if (m_backUpEdges.IndexOf(edge) == -1)
                    {
                        EdgeAdded(edge);
                    }
                }
            }
        }

        /// <summary>
        /// Creator contains the necessary data to fetch the edges and get the symbol.
        /// </summary>
        public HostedSweepCreator Creator
        {
            get { return m_creator; }
        }

        /// <summary>
        /// Symbol for HostedSweep creation.
        /// </summary>
        public ElementType Symbol
        {
            get { return m_symbol; }
            set { m_symbol = value; }
        }        

        /// <summary>
        /// Edges which contains references for HostedSweep creation.
        /// </summary>
        public List<Edge> EdgesForHostedSweep
        {
            get { return m_edgesForHostedSweep; }
        }
    }
}
