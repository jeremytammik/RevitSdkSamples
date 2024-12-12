//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.BlendVertexConnectTable.CS
{
    /// <summary>
    /// This class defines the data item of the vertex connection of blend
    /// </summary>
    class VertexConnectData
    {
        #region VertexConnectData member variables
        // vertex index in bottom sketch, the index is actually index in all vertexes.
        // if the curves of sketch are lines, the index will be equal to index in VertexConnectTableData.
        // if the curves of sketch are not line, after tessellating, the index will not be equal to VertexConnectTableData.
        private int m_bottomIndex;

        // vertex index in top sketch, the index is actual index in all top vertexes.
        private int m_topIndex;    
        #endregion


        #region VertexConnectData construction
        /// <summary>
        /// Class construction, create one vertex connect item
        /// </summary>
        /// <param name="_bottomIndex"> bottom vertex index </param>
        /// <param name="_topIndex"> top vertex index </param>
        public VertexConnectData(int _bottomIndex, int _topIndex)
        {
            m_bottomIndex = _bottomIndex;
            m_topIndex = _topIndex;
        }
        #endregion


        #region Properties of VertexConnectData
        /// <summary>
        /// Bottom index property, read only
        /// </summary>
        public int BottomIndex
        {
            get
            {
                return m_bottomIndex;
            }
        }


        /// <summary>
        /// Top index property, read only
        /// </summary>
        public int TopIndex
        {
            get
            {
                return m_topIndex;
            }
        }
        #endregion
    }


    /// <summary>
    /// The class defines the data structure only shown in DataGridView control, we should not use it in other fields.
    /// Assuming the bottom sketch of blend is a circle, there will be only two connect vertexes, 
    /// but there will be more than two vertices after tessellating the curves of sketches.
    /// The index(bottomIndex / topIndex) in VertexConnectData class is the actual position in all these vertices.
    /// We should not mark all these vertices with "T0, ..., Tn" (or B1, ..., Bn), we should only mark those connect vertexes.
    /// The VertexConnectTableData defines the updated index in all connect vertexes(that is, the vertices has the index are connect vertex)
    /// In other words, the indexes in this class are virtual!
    /// </summary>
    class VertexConnectTableData
    {
        #region VertexConnectTableData member variables
        private int m_tableIndex;  // connect edge index

        // vertex index in bottom sketch, the index are actually index in all vertexes
        private int m_bottomIndex;

        // vertex index in top sketch, the index are actual index in all top vertexes.
        private int m_topIndex;    
        #endregion


        #region VertexConnectTableData constructions

        /// <summary>
        /// Create new item for showing in DataGridView control
        /// </summary>
        /// <param name="_tableIndex">Index of data</param>
        /// <param name="_bottomIndex">Virtual bottom vertex index</param>
        /// <param name="_topIndex">Virtual top vertex index</param>
        public VertexConnectTableData(int _tableIndex, int _bottomIndex, int _topIndex)
        {
            m_tableIndex  = _tableIndex;
            m_bottomIndex = _bottomIndex;
            m_topIndex    = _topIndex;
        }
        #endregion 


        #region Properties of VertexConnectTableData
        /// <summary>
        /// Table index property, read only
        /// </summary>
        public int TableIndex
        {
            get
            {
                return m_tableIndex;
            }
        }


        /// <summary>
        /// Bottom index property, read only
        /// </summary>
        public int BottomIndex
        {
            get
            {
                return m_bottomIndex;
            }
        }


        /// <summary>
        /// Top index property, read only
        /// </summary>
        public int TopIndex
        {
            get
            {
                return m_topIndex;
            }
        }
        #endregion
    }
}
