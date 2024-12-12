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

namespace Revit.SDK.Samples.FrameBuilder.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;
    using System.ComponentModel;

    using Autodesk.Revit;
    using Autodesk.Revit.DB;

    /// <summary>
    /// for control PropertyGrid to show and modify parameters of column, beam or brace
    /// </summary>
    public class FrameTypeParameters
    {
        Parameter m_hDimension;        // parameter named h
        Parameter m_bDimension;        // parameter named b

        /// <summary>
        /// parameter h in parameter category Dimension
        /// </summary>
        [CategoryAttribute("Dimensions")]
        public double h
        {
            get
            {
                return m_hDimension.AsDouble();
            }
            set
            {
                m_hDimension.Set(value);
            }
        }

        /// <summary>
        /// parameter b in parameter category Dimension
        /// </summary>
        [CategoryAttribute("Dimensions")]
        public double b
        {
            get
            {
                return m_bDimension.AsDouble();
            }
            set
            {
                m_bDimension.Set(value);
            }
        }

        /// <summary>
        /// constructor without parameter is forbidden
        /// </summary>
        private FrameTypeParameters()
        {
        }

        /// <summary>
        /// constructor used only for object factory
        /// </summary>
        /// <param name="symbol">FamilySymbol object has parameters</param>
        private FrameTypeParameters(FamilySymbol symbol)
        {
            // iterate and initialize parameters
            foreach (Parameter para in symbol.Parameters)
            {
                if (para.Definition.Name == "h")
                {
                    m_hDimension = para;
                    continue;
                }
                if (para.Definition.Name == "b")
                {
                    m_bDimension = para;
                    continue;
                }
            }
        }

        /// <summary>
        /// object factory to create FramingTypeParameters; 
        /// will return null if necessary Parameters can't be found
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static FrameTypeParameters CreateInstance(FamilySymbol symbol)
        {
            FrameTypeParameters result = new FrameTypeParameters(symbol);
            if (null == result.m_bDimension || null == result.m_hDimension)
            {
                return null;
            }
            return result;
        }
    }
}
