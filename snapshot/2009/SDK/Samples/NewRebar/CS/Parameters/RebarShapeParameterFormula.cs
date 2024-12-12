//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// This class wraps a formula parameter which will be the dimension of 
    /// RebarShape definition.
    /// </summary>
    class RebarShapeParameterFormula : RebarShapeParameter
    {
        /// <summary>
        /// Parameter formula sting.
        /// </summary>
        private string m_formula;

        /// <summary>
        /// Parameter formula sting.
        /// </summary>
        public string Formula
        {
            get { return m_formula; }
            set { m_formula = value; }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="shapeDef">RebarShapeDefinition proxy</param>
        /// <param name="name">Parameter name</param>
        /// <param name="formula">Parameter formula sting</param>
        public RebarShapeParameterFormula(RebarShapeDef shapeDef, string name, string formula)
            :base(shapeDef,name)
        {
            m_formula = formula;
        }

        /// <summary>
        /// Add a formula parameter to RebarShapeDefinition.
        /// </summary>
        /// <param name="defGroup">Definition group</param>
        public override void Commit(DefinitionGroup defGroup)
        {
            ExternalDefinition def = GetOrCreateDef(defGroup);
            m_paramerter = m_rebarShapeDef.
                RebarshapeDefinition.AddFormulaParameter(def, m_formula);                
        }
    }
}
