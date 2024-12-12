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
using System.Reflection;
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ImportExport.CS
{
    /// <summary>
    /// Base data class which stores the basic information for import
    /// </summary>
    public class ImportData
    {
        #region Class Member Variables
        /// <summary>
        /// Revit command data
        /// </summary>
        protected ExternalCommandData m_commandData;
        /// <summary>
        /// Active document
        /// </summary>
        protected Document m_activeDoc;

        /// <summary>
        /// Directory where to import the file
        /// </summary>
        protected String m_importFolder;
        /// <summary>
        /// File Name or Prefix to be used
        /// </summary>
        protected String m_importFileFullName;

        /// <summary>
        /// The format to be exported
        /// </summary>
        protected ImportFormat m_importFormat;

        /// <summary>
        /// The filter which will be used in file saving dialog
        /// </summary>
        protected String m_filter;

        /// <summary>
        /// The title of importing dialog
        /// </summary>
        protected String m_title;
        #endregion

        #region Class Properties
        /// <summary>
        /// Revit command data
        /// </summary>
        public ExternalCommandData CommandData
        {
            get 
            { 
                return m_commandData; 
            }
        }

        /// <summary>
        /// File Name or Prefix to be used
        /// </summary>
        public String ImportFileFullName
        {
            get 
            {
                return m_importFileFullName; 
            }
            set 
            {
                m_importFileFullName = value; 
            }
        }  

        /// <summary>
        /// The format to be imported
        /// </summary>
        public ImportFormat ImportFormat
        {
            get 
            { 
                return m_importFormat; 
            }
            set 
            { 
                m_importFormat = value; 
            }
        }

        /// <summary>
        /// The filter which will be used in file saving dialog
        /// </summary>
        public String Filter
        {
            get 
            { 
                return m_filter; 
            }
        }

        /// <summary>
        /// Directory where to import the file
        /// </summary>
        public String ImportFolder
        {
            get
            {
                return m_importFolder;
            }
            set
            {
                m_importFolder = value;
            }
        }

        /// <summary>
        /// The title of importing dialog
        /// </summary>
        public String Title
        {
            get
            {
                return m_title;
            }
        }
        #endregion

        #region Class Member Methods
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData">Revit command data</param>
        /// <param name="importFormat">Format to import</param>
        public ImportData(ExternalCommandData commandData, ImportFormat importFormat)
        {
            m_commandData = commandData;
            m_activeDoc = commandData.Application.ActiveUIDocument.Document;
            m_importFormat = importFormat;
            m_filter = String.Empty;
            Initialize();
        }

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Initialize()
        {
            //The directory into which the file will be imported
            String dllFilePath = Assembly.GetExecutingAssembly().Location;
            m_importFolder = Path.GetDirectoryName(dllFilePath);
            m_importFileFullName = String.Empty;
        }

        /// <summary>
        /// Collect the parameters and import
        /// </summary>
        /// <returns></returns>
        public virtual bool Import()
        {
            if (m_importFileFullName == null)
            {
                throw new NullReferenceException();
            }

            return true;
        }
        #endregion
    }
}
