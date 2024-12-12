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
using Autodesk.Revit;
using System.ComponentModel;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// Dimension on RebarShape.
    /// </summary>
    public abstract class ConstraintOnRebarShape
    {
        /// <summary>
        /// A wrapper of RebarShapeDefinition.
        /// </summary>
        protected RebarShapeDef m_shapeDef;

        /// <summary>
        /// Constructor, initialize fields.
        /// </summary>
        /// <param name="def">RebarShapeDef object</param>
        protected ConstraintOnRebarShape(RebarShapeDef def)
        {
            m_shapeDef = def;
        }

        /// <summary>
        /// Update the parameter list value for property grid.
        /// </summary>
        protected void UpdateParameterTypeConverter()
        {
            TypeConverterRebarShapeParameter.RebarShapeParameters = m_shapeDef.Parameters;
        }

        /// <summary>
        /// Name of the constraint.
        /// </summary>
        public String Name
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Commit the dimension.
        /// </summary>
        public abstract void Commit();
    }
}
