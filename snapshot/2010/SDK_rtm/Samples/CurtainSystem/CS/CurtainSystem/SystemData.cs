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

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.CurtainSystem.CS.CurtainSystem
{
    /// <summary>
    /// the class to maintain the data and operations of the curtain system
    /// </summary>
    public class SystemData
    {
        // the data of the sample
        Data.Document m_document;
        
        // the count of the created curtain systems
        static int m_csIndex = -1;
        
        // all the created curtain systems and their data
        List<SystemInfo> m_curtainSystemInfos;
        /// <summary>
        /// all the created curtain systems and their data
        /// </summary>
        public List<SystemInfo> CurtainSystemInfos
        {
            get
            {
                return m_curtainSystemInfos;
            }
        }

        /// <summary>
        /// occurs only when new curtain system added/removed
        /// the delegate method to handle the curtain system added/removed events
        /// </summary>
        public delegate void CurtainSystemChangedHandler();
        /// <summary>
        /// the event triggered when curtain system added/removed
        /// </summary>
        public event CurtainSystemChangedHandler CurtainSystemChanged;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="doc">
        /// the document of the sample
        /// </param>
        public SystemData(Data.Document doc)
        {
            m_document = doc;
            m_curtainSystemInfos = new List<SystemInfo>();
        }

        /// <summary>
        /// create a new curtain system
        /// </summary>
        /// <param name="faceIndices">
        /// the faces to be covered with new curtain system
        /// </param>
        /// <param name="byFaceArray">
        /// indicates whether the curtain system will be created by face array
        /// </param>
        public void CreateCurtainSystem(List<int> faceIndices, bool byFaceArray)
        {
            // just refresh the main UI
            if (null == faceIndices ||
                0 == faceIndices.Count)
            {
                if (null != CurtainSystemChanged)
                {
                    CurtainSystemChanged();
                }
                return;
            }
            SystemInfo resultInfo = new SystemInfo(m_document);
            resultInfo.ByFaceArray = byFaceArray;
            resultInfo.GridFacesIndices = faceIndices;
            resultInfo.Index = ++m_csIndex;

            //
            // step 1: create the curtain system
            //
            // create the curtain system by face array
            if (true == byFaceArray)
            {
                FaceArray faceArray = new FaceArray();
                foreach (int index in faceIndices)
                {
                    faceArray.Append(m_document.MassFaceArray.get_Item(index));
                }

                Autodesk.Revit.Elements.CurtainSystem curtainSystem = null;
                try
                {
                    m_document.ActiveDocument.BeginTransaction();
                    curtainSystem = m_document.ActiveDocument.Create.NewCurtainSystem(faceArray, m_document.CurtainSystemType);
                    m_document.ActiveDocument.EndTransaction();
                }
                catch (System.Exception)
                {
                    m_document.FatalErrorMsg = Properties.Resources.MSG_CreateCSFailed;
                    return;
                }

                resultInfo.CurtainForm = curtainSystem;
            }
            // create the curtain system by reference array
            else
            {
                ReferenceArray refArray = new ReferenceArray();
                foreach (int index in faceIndices)
                {
                    refArray.Append(m_document.MassFaceArray.get_Item(index).Reference);
                }

                ElementSet curtainSystems = null;
                try
                {
                    m_document.ActiveDocument.BeginTransaction();
                    curtainSystems = m_document.ActiveDocument.Create.NewCurtainSystem(refArray, m_document.CurtainSystemType);
                    m_document.ActiveDocument.EndTransaction();
                }
                catch (System.Exception)
                {
                    m_document.FatalErrorMsg = Properties.Resources.MSG_CreateCSFailed;
                    return;
                }

                // internal fatal error, quit the sample
                if (null == curtainSystems ||
                    1 != curtainSystems.Size)
                {
                    m_document.FatalErrorMsg = Properties.Resources.MSG_MoreThan1CSCreated;
                    return;
                }

                // store the curtain system
                foreach (Autodesk.Revit.Elements.CurtainSystem cs in curtainSystems)
                {
                    resultInfo.CurtainForm = cs;
                    break;
                }
            }
            //
            // step 2: update the curtain system list in the main UI
            //
            m_curtainSystemInfos.Add(resultInfo);
            if (null != CurtainSystemChanged)
            {
                CurtainSystemChanged();
            }
        }

        /// <summary>
        /// delete the curtain systems
        /// </summary>
        /// <param name="checkedIndices">
        /// the curtain systems to be deleted
        /// </param>
        public void DeleteCurtainSystem(List<int> checkedIndices)
        {
            m_document.ActiveDocument.BeginTransaction();
            foreach (int index in checkedIndices)
            {
                SystemInfo info = m_curtainSystemInfos[index];
                if (null != info.CurtainForm)
                {
                    m_document.ActiveDocument.Delete(info.CurtainForm);
                    info.CurtainForm = null;
                }
            }
            m_document.ActiveDocument.EndTransaction();

            // update the list of created curtain systems
            // remove the "deleted" curtain systems out
            List<SystemInfo> infos = m_curtainSystemInfos;
            m_curtainSystemInfos = new List<SystemInfo>();

            foreach (SystemInfo info in infos)
            {
                if (null != info.CurtainForm)
                {
                    m_curtainSystemInfos.Add(info);
                }
            }

            if (null != CurtainSystemChanged)
            {
                CurtainSystemChanged();
            }
        }

       
    }// end of class

    /// <summary>
    /// the information of a curtain system
    /// </summary>
    public class SystemInfo
    {
        // the data of the sample
        Data.Document m_document;

        // the curtain system
        Autodesk.Revit.Elements.CurtainSystem m_curtainSystem;
        /// <summary>
        /// the curtain system
        /// </summary>
        public Autodesk.Revit.Elements.CurtainSystem CurtainForm
        {
            get
            {
                return m_curtainSystem;
            }
            set
            {
                 m_curtainSystem = value;
            }
        }
       
        // indicates which faces the curtain system covers
        List<int> m_gridFacesIndices;
        /// <summary>
        /// indicates which faces the curtain system covers
        /// </summary>
        public List<int> GridFacesIndices
        {
            get
            {
                return m_gridFacesIndices;
            }
            set
            {
                m_gridFacesIndices = value;

                 // the faces which don't be included will be added to the m_uncoverFacesIndices collection
                 for (int i = 0; i < 6; i++ )
                 {
                     if (false == m_gridFacesIndices.Contains(i))
                     {
                         m_uncoverFacesIndices.Add(i);
                     }
                 }
            }
        }

        // the uncovered faces
        List<int> m_uncoverFacesIndices;
        /// <summary>
        /// the uncovered faces
        /// </summary>
        public List<int> UncoverFacesIndices
        {
            get
            {
                return m_uncoverFacesIndices;
            }
        }

        // indicates whether the curtain system is created by face array
        private bool m_byFaceArray;
        /// <summary>
        /// indicates whether the curtain system is created by face array
        /// </summary>
        public bool ByFaceArray
        {
            get 
            { 
                return m_byFaceArray; 
            }
            set 
            {
                m_byFaceArray = value; 
            }
        }

        // the name of the curtain system, identified by its index
        private string m_name;
        /// <summary>
        /// the name of the curtain system, identified by its index
        /// </summary>
        public string Name
        {
            get
            {
                return m_name;
            }
        }

        // the index of the curtain systems
        private int m_index;
        /// <summary>
        /// the index of the curtain systems
        /// </summary>
        public int Index
        {
            get
            {
                return m_index;
            }
            set
            {
                 m_index = value;
                 m_name = "Curtain System " + m_index;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="doc">
        /// the document of the sample
        /// </param>
        public SystemInfo(Data.Document doc)
        {
            m_document = doc;
            m_gridFacesIndices = new List<int>();
            m_uncoverFacesIndices = new List<int>();
            m_byFaceArray = false;
            m_index = 0;
        }

        /// <summary>
        /// add some curtain grids to the curtain system
        /// </summary>
        /// <param name="faceIndices">
        /// the faces to be covered
        /// </param>
        public void AddCurtainGrids(List<int> faceIndices)
        {
            // step 1: find out the faces to be covered
            List<Reference> refFaces = new List<Reference>();
            foreach (int index in faceIndices)
            {
                refFaces.Add(m_document.MassFaceArray.get_Item(index).Reference);
            }
            // step 2: cover the selected faces with curtain grids
            try
            {
                m_document.ActiveDocument.BeginTransaction();
                foreach (Reference refFace in refFaces)
                {
                    m_curtainSystem.AddCurtainGrid(refFace);
                }
                m_document.ActiveDocument.EndTransaction();
            }
            catch (System.Exception)
            {
                m_document.FatalErrorMsg = Properties.Resources.MSG_AddCGFailed;
                return;
            }
            // step 3: update the uncovered faces and curtain grid faces data
            foreach (int i in faceIndices)
            {
                m_uncoverFacesIndices.Remove(i);
                m_gridFacesIndices.Add(i);
            }
        }

        /// <summary>
        /// remove the selected curtain grids
        /// </summary>
        /// <param name="faceIndices">
        /// the curtain grids to be removed
        /// </param>
        public void RemoveCurtainGrids(List<int> faceIndices)
        {
            // step 1: find out the faces to be covered
            List<Reference> refFaces = new List<Reference>();
            foreach (int index in faceIndices)
            {
                refFaces.Add(m_document.MassFaceArray.get_Item(index).Reference);
            }
            // step 2: remove the selected curtain grids
            try
            {
                m_document.ActiveDocument.BeginTransaction();
                foreach (Reference refFace in refFaces)
                {
                    m_curtainSystem.RemoveCurtainGrid(refFace);
                }
                m_document.ActiveDocument.EndTransaction();
            }
            catch (System.Exception)
            {
                m_document.FatalErrorMsg = Properties.Resources.MSG_RemoveCGFailed;
                return;
            }
            // step 3: update the uncovered faces and curtain grid faces data
            foreach (int i in faceIndices)
            {
                m_gridFacesIndices.Remove(i);
                m_uncoverFacesIndices.Add(i);
            }
        }

        /// <summary>
        /// override ToString method
        /// </summary>
        /// <returns>
        /// the string value of the class
        /// </returns>
        public override string ToString()
        {
            return m_name;
        }
    }

    /// <summary>
    /// the information for the curtain grid (which face does it lay on)
    /// </summary>
    public class GridFaceInfo
    {
        // the host face of the curtain grid
        private int m_faceIndex;
        /// <summary>
        /// the host face of the curtain grid
        /// </summary>
        public int FaceIndex
        {
            get
            {
                return m_faceIndex;
            }
            set
            {
                 m_faceIndex = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="index">
        /// the index of the host face
        /// </param>
        public GridFaceInfo(int index)
        {
            m_faceIndex = index;
        }

        /// <summary>
        /// the string value for the class
        /// </summary>
        /// <returns>
        /// the string value for the class
        /// </returns>
        public override string ToString()
        {
            return "Grid on Face " + m_faceIndex;
        }
    }

    /// <summary>
    /// the information for the faces of the mass
    /// </summary>
    public class UncoverFaceInfo
    {
        // indicates the index for the face
        private int m_faceIndex;
        /// <summary>
        /// indicates the index for the face
        /// </summary>
        public int Index
        {
            get
            {
                return m_faceIndex;
            }
            set
            {
                m_faceIndex = value;
            }
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="index">
        /// the index of the face
        /// </param>
        public UncoverFaceInfo(int index)
        {
            m_faceIndex = index;
        }

        /// <summary>
        /// the string value for the class
        /// </summary>
        /// <returns>
        /// the string value for the class
        /// </returns>
        public override string ToString()
        {
            return "Face " + m_faceIndex;
        }
    }
}
