//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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

using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// This class wraps RebarShapeDefinitionByArc.
    /// </summary>
    class RebarShapeDefByArc : RebarShapeDef
    {
        public RebarShapeDefByArc(RebarShapeDefinitionByArc arcShapeDef)
            : base(arcShapeDef)
        {
        }

        /// <summary>
        /// Get all the constraint types supported by RebarShapeDefinitionByArc.
        /// </summary>
        /// <returns>all the constraint types supported by RebarShapeDefinitionByArc</returns>
        public override List<Type> AllowedConstraintTypes()
        {
            RebarShapeDefinitionByArc definitionByArc = RebarshapeDefinition as RebarShapeDefinitionByArc;
            
            List<Type> allowedTypes = base.AllowedConstraintTypes();
            
            allowedTypes.Add(typeof(ConstraintRadius));
            allowedTypes.Add(typeof(ConstraintDiameter));
            allowedTypes.Add(typeof(ConstraintArcLength));
            allowedTypes.Add(typeof(ConstraintCircumference));
            allowedTypes.Add(typeof(ConstraintChordLength));
            allowedTypes.Add(typeof(ConstraintSagittaLength)); 

            return allowedTypes;           
        }
    }
}
