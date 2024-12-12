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

namespace RvtSamples
{
    /// <summary>
    /// The class contains information of a sample item to be added into samples menu
    /// </summary>
    public class SampleItem
    {
        #region Variables
        /// <summary>
        /// category
        /// </summary>
        private string m_category;
        /// <summary>
        /// display name
        /// </summary>
        private string m_displayName;
        /// <summary>
        /// path of large image
        /// </summary>
        private string m_largeImage;
        /// <summary>
        /// path of image
        /// </summary>
        private string m_image;
        /// <summary>
        /// description
        /// </summary>
        private string m_description;
        /// <summary>
        /// path of assembly
        /// </summary>
        private string m_assembly;
        /// <summary>
        /// class name
        /// </summary>
        private string m_className;
        #endregion

        #region Properties
        /// <summary>
        /// category
        /// </summary>
        public string Category
        {
            get 
            { 
                return m_category; 
            }
        }
        /// <summary>
        /// display name
        /// </summary>
        public string DisplayName
        {
            get 
            { 
                return m_displayName; 
            }
        }
        /// <summary>
        /// path of large image
        /// </summary>
        public string LargeImage
        {
            get 
            { 
                return m_largeImage; 
            }
        }
        /// <summary>
        /// path of image
        /// </summary>
        public string Image
        {
            get 
            { 
                return m_image; 
            }
        }
        /// <summary>
        /// description
        /// </summary>
        public string Description
        {
            get 
            { 
                return m_description; 
            }
        }
        /// <summary>
        /// path of assembly
        /// </summary>
        public string Assembly
        {
            get 
            { 
                return m_assembly; 
            }
        }
        /// <summary>
        /// class name
        /// </summary>
        public string ClassName
        {
            get 
            { 
                return m_className; 
            }
        }
        #endregion

        # region Methods
        /// <summary>
        /// Constructor
        /// </summary>
        public SampleItem()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="category">category</param>
        /// <param name="displayName">display name</param>
        /// <param name="description">description</param>
        /// <param name="largeImage">path of large image</param>
        /// <param name="image">path of image</param>
        /// <param name="assembly">path of assembly</param>
        /// <param name="className">class name</param>
        public SampleItem(string category, string displayName, string description, string largeImage, string image, string assembly, string className)
        {
            m_category = category;
            m_displayName = displayName;
            m_description = description;
            m_largeImage = largeImage;
            m_image = image;
            m_assembly = assembly;
            m_className = className;
        }
        #endregion
    }
}
