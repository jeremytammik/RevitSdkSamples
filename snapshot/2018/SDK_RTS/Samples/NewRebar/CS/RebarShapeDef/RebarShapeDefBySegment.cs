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
    /// This class wraps RebarShapeDefinitionBySegments.
    /// </summary>
    class RebarShapeDefBySegment : RebarShapeDef
    {
        public RebarShapeDefBySegment(RebarShapeDefinitionBySegments segmentShapeDef)
            :base(segmentShapeDef)
        {
        }

        /// <summary>
        /// Get all the constraints supported by RebarShapeDefinitionBySegments.
        /// </summary>
        /// <returns>all the constraints supported by RebarShapeDefinitionBySegments</returns>
        public override List<Type> AllowedConstraintTypes()
        {
            List<Type> allowedTypes = base.AllowedConstraintTypes();
            allowedTypes.Add(typeof(ConstraintBendDefaultRadius));
            allowedTypes.Add(typeof(ConstraintBendVariableRadius));
            allowedTypes.Add(typeof(ConstraintParallelToSegment));
            allowedTypes.Add(typeof(ConstraintToSegment));
            allowedTypes.Add(typeof(ListeningDimensionBendToBend));
            allowedTypes.Add(typeof(ListeningDimensionSegmentToBend));
            allowedTypes.Add(typeof(ListeningDimensionSegmentToSegment));
            allowedTypes.Add(typeof(RemoveParameterFromSegment));
            allowedTypes.Add(typeof(SetSegmentAs180DegreeBend));
            allowedTypes.Add(typeof(SetSegmentFixedDirection));
            allowedTypes.Add(typeof(SetSegmentVariableDirection));
            return allowedTypes;
        }
    }
}
