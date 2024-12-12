//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// This class will deal with all parameters related to window creation
    /// </summary>
    public class WindowParameter
    {
        /// <summary>
        ///store the family type name
        /// </summary>
        String m_type = String.Empty;
        
        /// <summary>
        /// store the height of opening
        /// </summary>
        double m_height = 0.0;

        /// <summary>
        /// store the width of opening
        /// </summary>
        double m_width = 0.0;       

        #region Properties
        /// <summary>
        /// get/set the Type property
        /// </summary>
        public String Type
        {
            set
            {
                m_type = value;
            }
            get
            {
                return m_type;
            }
        }

        /// <summary>
        /// get/set the Height property
        /// </summary>
        public double Height
        {
            set
            {
                m_height = value;
            }
            get
            {
                return m_height;
            }
        }

        /// <summary>
        /// get/set the Width property
        /// </summary>
        public double Width
        {
            set
            {
                m_width = value;
            }
            get
            {
                return m_width;
            }
        }
        #endregion 

        /// <summary>
        /// constructor of WindowParameter
        /// </summary>
        /// <param name="isMetric">indicate whether the template is metric or imperial</param>
        public WindowParameter(bool isMetric)
        {
            if (isMetric)
            {
                m_type = "NewType";
                m_height = 1000;
                m_width = 500;
            }
            else
            {
                m_type = "NewType";
                m_height = 4.0;
                m_width = 2.0;
            }
        }

        /// <summary>
        /// construcion of WindowParameter
        /// </summary>
        /// <param name="para">the WindowParameter</param>
        public WindowParameter(WindowParameter para)
        {
            if (String.IsNullOrEmpty(para.m_type))
            {
                m_type = "NewType";
            }
            m_type = para.Type + "1";
            m_height = para.Height;
            m_width = para.Width;
        }
    }

    /// <summary>
    /// This class is used to deal with wizard parameters
    /// </summary>
    public class WizardParameter 
    {
        // ToDo add properties for them
        
        /// <summary>
        /// store the template name
        /// </summary>
        public String m_template = String.Empty;

        /// <summary>
        /// store the current WindowParameter
        /// </summary>
        private WindowParameter m_curPara = new WindowParameter(true);        
        
        /// <summary>
        /// store the windowparameter hashtable
        /// </summary>
        Hashtable m_winParas = new Hashtable();
        
        /// <summary>
        /// store the frame material list
        /// </summary>
        private List<String> m_frameMats = new List<string>();
        
        /// <summary>
        /// store the glass material list
        /// </summary>
        private List<String> m_GlassMats = new List<string>();
        
        /// <summary>
        /// store the glass material
        /// </summary>
        String m_glassMat = String.Empty;

        /// <summary>
        /// store the sash material
        /// </summary>
        String m_sashMat = String.Empty;

        /// <summary>
        /// store the ValidateWindowParameter
        /// </summary>
        private ValidateWindowParameter m_validator = new ValidateWindowParameter(10, 10);
        
        /// <summary>
        /// store the temp path
        /// </summary>
        private String m_pathName = System.IO.Path.GetTempPath();
      
        #region
        /// <summary>
        /// get/set Validator property
        /// </summary>
        public ValidateWindowParameter Validator
        {
            get
            {
                return m_validator;
            }
            set
            {
                m_validator = value;
            }
        }

        /// <summary>
        /// get/set FrameMaterials property
        /// </summary>
        public List<String> FrameMaterials
        {
            set
            {
                m_frameMats = value;
            }
            get
            {
                return m_frameMats;
            }
        }

        /// <summary>
        /// get/set GlassMaterials property
        /// </summary>
        public List<String> GlassMaterials
        {
            set
            {
                m_GlassMats = value;
            }
            get
            {
                return m_GlassMats;
            }
        }

        /// <summary>
        /// get/set GlassMat property
        /// </summary>
        public String GlassMat
        {
            set
            {
                m_glassMat = value;
            }
            get
            {
                return m_glassMat;
            }
        }

        /// <summary>
        /// get/set SashMat property
        /// </summary>
        public String SashMat
        {
            set
            {
                m_sashMat = value;
            }
            get
            {
                return m_sashMat;
            }
        }

        /// <summary>
        /// get/set WinParaTab property
        /// </summary>
        public Hashtable WinParaTab
        {
            get
            {
                return m_winParas;
            }
            set
            {
                m_winParas = value;
            }
        }

        /// <summary>
        /// get/set CurrentPara property
        /// </summary>
        public WindowParameter CurrentPara
        {
            get
            {
                return m_curPara;
            }
            set
            {
                m_curPara = value;
            }
        }

        /// <summary>
        /// get/set PathName property
        /// </summary>
        public String PathName
        {
            get
            {
                return m_pathName;
            }
            set
            {
                m_pathName = value;
            }
        }       
        #endregion    
    }

    /// <summary>
    /// This class inherits from WindowParameter
    /// </summary>
    public class DoubleHungWinPara : WindowParameter
    {
        /// <summary>
        /// store the m_inset
        /// </summary>
        double m_inset = 0.0;

        /// <summary>
        /// store the m_sillHeight
        /// </summary>
        double m_sillHeight = 0.0; 

        #region
        /// <summary>
        /// set/get Inset property
        /// </summary>
        public double Inset
        {
            set
            {
                m_inset = value;
            }
            get
            {
                return m_inset;
            }
        }

        /// <summary>
        /// set/get SillHeight property
        /// </summary>
        public double SillHeight
        {
            set
            {
                m_sillHeight = value;
            }
            get
            {
                return m_sillHeight;
            }
        }
        #endregion

        /// <summary>
        /// constructor of DoubleHungWinPara
        /// </summary>
        /// <param name="isMetric">indicate whether the template is metric of imperial</param>
        public DoubleHungWinPara(bool isMetric)
            : base(isMetric)
        {
            if (isMetric)
            {
                m_inset = 20;
                m_sillHeight = 800;
            }
            else
            {
                m_inset = 0.05;
                m_sillHeight = 3;
            }            
        }

        /// <summary>
        /// constructor of DoubleHungWinPara
        /// </summary>
        /// <param name="dbhungPara">DoubleHungWinPara</param>
        public DoubleHungWinPara(DoubleHungWinPara dbhungPara)
            : base(dbhungPara)
        {
            m_inset = dbhungPara.Inset;
            m_sillHeight = dbhungPara.SillHeight;
        }
    }
}
