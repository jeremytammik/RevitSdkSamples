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


using System.ComponentModel;
using Autodesk.Revit.DB;
using System;    

namespace Revit.SDK.Samples.InPlaceMembers.CS
{
    /// <summary>
    /// This class is used as PropertyGrid.SelectedObject.   
    /// </summary>
    public class Properties
    {
        private int m_ID;
        private string m_Name;
        private string m_Family;
        private string m_Type;
        private string m_StructuralType;
        private string m_StructuralUsage;
        private string m_Material;

        /// <summary>
        /// the value of the element id as an integer
        /// </summary>
        [CategoryAttribute("Identity")]
        public int ID
        {
            get
            {
                return m_ID;
            }
        }

        /// <summary>
        /// a human readable name for the Element.
        /// </summary>
        [CategoryAttribute("Identity")]
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// a human readable name for the family name.
        /// </summary>
        [CategoryAttribute("Category")]
        public string Family
        {
            get
            {
                return m_Family;
            }
        }

        /// <summary>
        /// a human readable name for the family type name.
        /// </summary>
        [CategoryAttribute("Category")]
        public string Type
        {
            get
            {
                return m_Type;
            }
        }

        /// <summary>
        /// the primary structural type of the instance, such as beam or column etc.
        /// </summary>
        [CategoryAttribute("Structural")]
        public string StructuralType
        {
            get
            {
                return m_StructuralType;
            }
        }

        /// <summary>
        /// the primary structural usage of the instance, such as brace, girder etc.
        /// </summary>
        [CategoryAttribute("Structural")]
        public string StructuralUsage
        {
            get
            {
                return m_StructuralUsage;
            }
        }

        /// <summary>
        /// the physical material from which the instance is made.
        /// </summary>
        [CategoryAttribute("Structural")]
        public string Material
        {
            get
            {
                return m_Material;
            }
        }

        /// <summary>
        /// get this family instance's properties to display.
        /// </summary>
        /// <param name="f">a In-Place family instance</param>
        public Properties(FamilyInstance f)
        {
            m_ID = f.Id.IntegerValue;
            m_Name = f.Name;
            m_Family = f.Symbol.Family.Name;
            m_Type = f.Symbol.Name;
            m_StructuralType = f.StructuralType.ToString();
            try
            {
                m_StructuralUsage = f.StructuralUsage.ToString();
            }
            catch(Exception)
            {
                m_StructuralUsage = null;
            }            
            m_Material = f.StructuralMaterialType.ToString();
        }

    }
}
