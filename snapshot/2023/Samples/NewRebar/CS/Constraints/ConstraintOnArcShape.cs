//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.ComponentModel;

using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// Dimension on RebarShapeDefByArc.
    /// </summary>
    abstract class ConstraintOnArcShape : ConstraintOnRebarShape
    {
        /// <summary>
        /// Dimension to constrain the arc shape.
        /// </summary>
        protected RebarShapeParameter m_rebarShapeParameter;

        public ConstraintOnArcShape(RebarShapeDefByArc def)
            : base(def)
        {
        }

        /// <summary>
        /// Dimension to constrain the arc shape.
        /// </summary>
        [DisplayName("RebarShape parameter"), TypeConverter(typeof(TypeConverterRebarShapeParameter)), ReadOnly(false)]
        public virtual RebarShapeParameter RebarShapeParameter
        {
            get
            {
                UpdateParameterTypeConverter();

                return m_rebarShapeParameter;
            }
            set
            {
                m_rebarShapeParameter = value;
            }
        }

        /// <summary>
        /// Get RebarShapeDefinitionByArc object.
        /// </summary>
        protected RebarShapeDefinitionByArc GetRebarShapeDefinitionByArc
        {
            get
            {
                return m_shapeDef.RebarshapeDefinition as RebarShapeDefinitionByArc;
            }
        }
    }

    /// <summary>
    /// Arc length dimension.
    /// </summary>
    class ConstraintArcLength : ConstraintOnArcShape
    {
        public ConstraintArcLength(RebarShapeDefByArc def)
            : base(def)
        {
        }

        /// <summary>
        /// Add dimension to constrain the arc length.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionByArc.AddConstraintArcLength( 
                RebarShapeParameter.Parameter);
                
        }
    }

    /// <summary>
    /// Arc chord length dimension.
    /// </summary>
    class ConstraintChordLength : ConstraintOnArcShape
    {
        public ConstraintChordLength(RebarShapeDefByArc def)
            : base(def)
        {
        }

        /// <summary>
        /// Add dimension to constrain the arc chord length.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionByArc.AddConstraintChordLength(
                RebarShapeParameter.Parameter);                
        }
    }

    /// <summary>
    /// Arc circumference dimension.
    /// </summary>
    class ConstraintCircumference : ConstraintOnArcShape
    {
        /// <summary>
        /// Arc reference type.
        /// </summary>
        private RebarShapeArcReferenceType m_arcReferenceType;

        public ConstraintCircumference(RebarShapeDefByArc def)
            : base(def)
        {
            m_arcReferenceType = RebarShapeArcReferenceType.External;
        }        

        /// <summary>
        /// Arc reference type.
        /// </summary>
        public RebarShapeArcReferenceType ArcReferenceType
        {
            get { return m_arcReferenceType; }
            set { m_arcReferenceType = value; }
        }

        /// <summary>
        /// Add dimension to constrain the arc circumference.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionByArc.AddConstraintCircumference(
                RebarShapeParameter.Parameter, ArcReferenceType);
         }
    }

    /// <summary>
    /// Arc diameter dimension.
    /// </summary>
    class ConstraintDiameter : ConstraintOnArcShape
    {
        /// <summary>
        /// Arc reference type.
        /// </summary>
        private RebarShapeArcReferenceType m_arcReferenceType;

        public ConstraintDiameter(RebarShapeDefByArc def)
            : base(def)
        {
            m_arcReferenceType = RebarShapeArcReferenceType.External;
        }

        /// <summary>
        /// Arc reference type.
        /// </summary>
        public RebarShapeArcReferenceType ArcReferenceType
        {
            get { return m_arcReferenceType; }
            set { m_arcReferenceType = value; }
        }

        /// <summary>
        /// Add dimension to constrain arc diameter.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionByArc.AddConstraintDiameter(
                RebarShapeParameter.Parameter, ArcReferenceType);
                
        }
    }

    /// <summary>
    /// Arc radius dimension.
    /// </summary>
    class ConstraintRadius : ConstraintOnArcShape
    {
        /// <summary>
        /// Arc reference type.
        /// </summary>
        private RebarShapeArcReferenceType m_arcReferenceType;

        public ConstraintRadius(RebarShapeDefByArc def)
            : base(def)
        {
            m_arcReferenceType = RebarShapeArcReferenceType.External;
        }        

        /// <summary>
        /// Arc reference type.
        /// </summary>
        public RebarShapeArcReferenceType ArcReferenceType
        {
            get { return m_arcReferenceType; }
            set { m_arcReferenceType = value; }
        }

        /// <summary>
        /// Add dimension to constrain the radius of arc.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionByArc.AddConstraintRadius(
                RebarShapeParameter.Parameter, ArcReferenceType);
        }
    }

    /// <summary>
    /// Arc Sagittarius length dimension.
    /// </summary>
    class ConstraintSagittaLength : ConstraintOnArcShape
    {
        public ConstraintSagittaLength(RebarShapeDefByArc def)
            : base(def)
        {
        }

        /// <summary>
        /// Add dimension to constrain the Sagittarius length of arc.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionByArc.AddConstraintSagittaLength(
                RebarShapeParameter.Parameter);
        }
    }
}
