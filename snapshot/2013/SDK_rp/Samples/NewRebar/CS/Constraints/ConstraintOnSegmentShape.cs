//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
    /// Bend orientation enum.
    /// </summary>
    enum BendOrientation
    {
        /// <summary>
        /// Turn left.
        /// </summary>
        Left = 1,

        /// <summary>
        /// Turn right.
        /// </summary>
        Right = -1
    }

    /// <summary>
    /// Segment's ends reference enum.
    /// </summary>
    enum EndReference
    {
        /// <summary>
        /// Segment's start reference.
        /// </summary>
        Begin = 0,

        /// <summary>
        /// Segment's end reference.
        /// </summary>
        End = 1
    }

    /// <summary>
    /// Constraint to be added to RebarShapeDefBySegment.
    /// </summary>
    abstract class ConstraintOnSegmentShape : ConstraintOnRebarShape
    {
        public ConstraintOnSegmentShape(RebarShapeDefBySegment def)
            : base(def)
        {
        }

        /// <summary>
        /// Update list value for property grid.
        /// </summary>
        protected void UpdateSegmentIdTypeConverter()
        {
            TypeConverterSegmentId.SegmentCount = GetRebarShapeDefinitionBySegments.NumberOfSegments;
        }

        /// <summary>
        /// Get RebarShapeDefinitionBySegments object.
        /// </summary>
        protected RebarShapeDefinitionBySegments GetRebarShapeDefinitionBySegments
        {
            get
            {
                return m_shapeDef.RebarshapeDefinition as RebarShapeDefinitionBySegments;
            }
        }
    }

    /// <summary>
    /// Default radius dimension of bend.
    /// </summary>
    class ConstraintBendDefaultRadius : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Segment to be added constraint on.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// Bend orientation field.
        /// </summary>
        private BendOrientation m_turn;

        /// <summary>
        /// Bend angle field.
        /// </summary>
        private RebarShapeBendAngle m_bendAngle;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="def"></param>
        public ConstraintBendDefaultRadius(RebarShapeDefBySegment def)
            : base(def)
        {
            m_turn = BendOrientation.Left;
            m_bendAngle = RebarShapeBendAngle.Obtuse;
        }

        /// <summary>
        /// Segment to be added constraint on.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment
        {
            get
            {
                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// Bend orientation property.
        /// </summary>
        public BendOrientation Turn
        {
            get
            {
                return m_turn;
            }
            set
            { 
                m_turn = value;
            }
        }        

        /// <summary>
        /// Bend angle property.
        /// </summary>
        public RebarShapeBendAngle BendAngle
        {
            get
            { 
                return m_bendAngle;
            }
            set 
            { 
                m_bendAngle = value;
            }
        }

        /// <summary>
        /// Add bend default radius constraint to RebarShapeDefinitionBySegments.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.AddBendDefaultRadius(
                m_segment, (int)m_turn, m_bendAngle);
        }
    }

    /// <summary>
    /// Variable radius dimension of bend.
    /// </summary>
    class ConstraintBendVariableRadius : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Segment to be added constraint on.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// Bend orientation field.
        /// </summary>
        private BendOrientation m_turn;

        /// <summary>
        /// Bend angle field.
        /// </summary>
        private RebarShapeBendAngle m_bendAngle;

        /// <summary>
        /// Radius dimension field.
        /// </summary>
        private RebarShapeParameter m_radiusParameter;

        /// <summary>
        /// Measure length including bar thickness or not.
        /// </summary>
        private bool m_measureIncludingBarThickness;

        public ConstraintBendVariableRadius(RebarShapeDefBySegment def)
            : base(def)
        {
            m_bendAngle = RebarShapeBendAngle.Obtuse;
            m_turn = BendOrientation.Left;
            m_measureIncludingBarThickness = true;
        }

        /// <summary>
        /// Segment to be added constraint on.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment
        {
            get
            {

                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// Bend orientation property.
        /// </summary>
        public BendOrientation Turn
        {
            get { return m_turn; }
            set { m_turn = value; }
        }        

        /// <summary>
        /// Bend angle property.
        /// </summary>
        public RebarShapeBendAngle BendAngle
        {
            get { return m_bendAngle; }
            set { m_bendAngle = value; }
        }        

        /// <summary>
        /// Radius dimension property.
        /// </summary>
        [TypeConverter(typeof(TypeConverterRebarShapeParameter))]
        public RebarShapeParameter RadiusParameter
        {
            get 
            {
                UpdateParameterTypeConverter();
                return m_radiusParameter;
            }
            set
            { 
                m_radiusParameter = value;
            }
        }
        

        /// <summary>
        /// Measure including bar thickness or not.
        /// </summary>
        public bool MeasureIncludingBarThickness
        {
            get 
            { 
                return m_measureIncludingBarThickness;
            }
            set 
            { 
                m_measureIncludingBarThickness = value; 
            }
        }

        /// <summary>
        /// Add Dimension to constrain the bend radius.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.AddBendVariableRadius( 
                m_segment, (int)m_turn, m_bendAngle, m_radiusParameter.Parameter,
                m_measureIncludingBarThickness);
        }
    }

    /// <summary>
    /// Parallel dimension to segment.
    /// </summary>
    class ConstraintParallelToSegment : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Segment to be added constraint on.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// Dimension to constrain the length of segment.
        /// </summary>
        private RebarShapeParameter m_parameter;

        /// <summary>
        /// Measure segment's length to outside of bend 0 or not.
        /// </summary>
        private bool m_measureToOutsideOfBend0;

        /// <summary>
        /// Measure segment's length to outside of bend 1 or not.
        /// </summary>
        private bool m_measureToOutsideOfBend1;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="def"></param>
        public ConstraintParallelToSegment(RebarShapeDefBySegment def)
            : base(def)
        {
            m_measureToOutsideOfBend0 = true;
            m_measureToOutsideOfBend1 = true;
        }
        
        /// <summary>
        /// Segment to be added constraint on.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment
        {
            get
            {

                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// Dimension to constrain the length of segment.
        /// </summary>
        [TypeConverter(typeof(TypeConverterRebarShapeParameter))]
        public RebarShapeParameter Parameter
        {
            get 
            {
                UpdateParameterTypeConverter();
                return m_parameter;
            }
            set { m_parameter = value; }
        }
        
        /// <summary>
        /// Measure segment's length to outside of bend 0 or not.
        /// </summary>
        public bool MeasureToOutsideOfBend0
        {
            get { return m_measureToOutsideOfBend0; }
            set { m_measureToOutsideOfBend0 = value; }
        }
        
        /// <summary>
        /// Measure segment's length to outside of bend 1 or not.
        /// </summary>
        public bool MeasureToOutsideOfBend1
        {
            get { return m_measureToOutsideOfBend1; }
            set { m_measureToOutsideOfBend1 = value; }
        }

        /// <summary>
        /// Add Dimension to constrain the segment length.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.AddConstraintParallelToSegment(
                m_segment, m_parameter.Parameter, m_measureToOutsideOfBend0, 
                m_measureToOutsideOfBend1);
        }
    }

    /// <summary>
    /// Length dimension of segment in specified direction.
    /// </summary>
    class ConstraintToSegment : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Segment to be added constraint on.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// Dimension to constraint the length of segment in specified direction.
        /// </summary>
        private RebarShapeParameter m_parameter;

        /// <summary>
        /// X coordinate of constraint direction. 
        /// </summary>
        private double m_constraintDirCoordX;

        /// <summary>
        /// Y coordinate of constraint direction.
        /// </summary>
        private double m_constraintDirCoordY;

        /// <summary>
        /// Sign of Z coordinate of cross product of constraint direction by segment direction.
        /// </summary>
        private int m_signOfZCoordOfCrossProductOfConstraintDirBySegmentDir;

        /// <summary>
        /// Measure segment's length to outside of bend 0 or not.
        /// </summary>
        private bool m_measureToOutsideOfBend0;

        /// <summary>
        /// Measure segment's length to outside of bend 1 or not.
        /// </summary>
        private bool m_measureToOutsideOfBend1;

        public ConstraintToSegment(RebarShapeDefBySegment def)
            : base(def)
        {
            m_measureToOutsideOfBend0 = true;
            m_measureToOutsideOfBend1 = false;
            m_signOfZCoordOfCrossProductOfConstraintDirBySegmentDir = -1;
        }
        
        /// <summary>
        /// Segment to be added constraint on.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment
        {
            get
            {

                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// Dimension to constraint the length of segment in specified direction.
        /// </summary>
        [TypeConverter(typeof(TypeConverterRebarShapeParameter))]
        public RebarShapeParameter Parameter
        {
            get
            {
                UpdateParameterTypeConverter();
                return m_parameter;
            }
            set { m_parameter = value; }
        }

        /// <summary>
        /// X coordinate of constraint direction. 
        /// </summary>
        public double ConstraintDirCoordX
        {
            get { return m_constraintDirCoordX; }
            set { m_constraintDirCoordX = value; }
        }

        /// <summary>
        /// Y coordinate of constraint direction.
        /// </summary>
        public double ConstraintDirCoordY
        {
            get { return m_constraintDirCoordY; }
            set { m_constraintDirCoordY = value; }
        }

        /// <summary>
        /// Sign of Z coordinate of cross product of constraint direction by segment direction.
        /// </summary>
        public int SignOfZCoordOfCrossProductOfConstraintDirBySegmentDir
        {
            get { return m_signOfZCoordOfCrossProductOfConstraintDirBySegmentDir; }
            set { m_signOfZCoordOfCrossProductOfConstraintDirBySegmentDir = value; }
        }

        /// <summary>
        /// Measure segment's length to outside of bend 0 or not.
        /// </summary>
        public bool MeasureToOutsideOfBend0
        {
            get { return m_measureToOutsideOfBend0; }
            set { m_measureToOutsideOfBend0 = value; }
        }

        /// <summary>
        /// Measure segment's length to outside of bend 1 or not.
        /// </summary>
        public bool MeasureToOutsideOfBend1
        {
            get { return m_measureToOutsideOfBend1; }
            set { m_measureToOutsideOfBend1 = value; }
        }

        /// <summary>
        /// Add dimension to constrain the length of segment in the specified direction.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.AddConstraintToSegment( m_segment,
                m_parameter.Parameter, m_constraintDirCoordX, m_constraintDirCoordY,
                m_signOfZCoordOfCrossProductOfConstraintDirBySegmentDir,
                m_measureToOutsideOfBend0, m_measureToOutsideOfBend1);
        }
    }

    /// <summary>
    /// Listening length dimension between two bends.
    /// </summary>
    class ListeningDimensionBendToBend : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Dimension to constraint the length of two bends in the specified direction.
        /// </summary>
        private RebarShapeParameter m_parameter;

        /// <summary>
        /// X coordinate of constraint direction. 
        /// </summary>
        private double m_constraintDirCoordX;

        /// <summary>
        /// Y coordinate of constraint direction. 
        /// </summary>
        private double m_constraintDirCoordY;

        /// <summary>
        /// Reference of segment 0.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// End reference of segment 0.
        /// </summary>
        private EndReference m_end;

        /// <summary>
        /// Reference of segment 1.
        /// </summary>
        private int m_segment1;

        /// <summary>
        /// End reference of segment 1.
        /// </summary>
        private EndReference m_end1;

        public ListeningDimensionBendToBend(RebarShapeDefBySegment def)
            : base(def)
        {
            m_end = EndReference.Begin;
            m_end1 = EndReference.End;
            m_constraintDirCoordX = 0;
            m_constraintDirCoordY = 0;
        }        


        /// <summary>
        /// Dimension to constraint the length of two bends in the specified direction.
        /// </summary>
        [TypeConverter(typeof(TypeConverterRebarShapeParameter))]
        public RebarShapeParameter Parameter
        {
            get
            {
                UpdateParameterTypeConverter();

                return m_parameter;
            }
            set { m_parameter = value; }
        }

        /// <summary>
        /// X coordinate of constraint direction. 
        /// </summary>
        public double ConstraintDirCoordX
        {
            get { return m_constraintDirCoordX; }
            set { m_constraintDirCoordX = value; }
        }

        /// <summary>
        /// Y coordinate of constraint direction. 
        /// </summary>
        public double ConstraintDirCoordY
        {
            get { return m_constraintDirCoordY; }
            set { m_constraintDirCoordY = value; }
        }

        /// <summary>
        /// Reference of segment 0.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment0
        {
            get
            {

                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// End reference of segment 0.
        /// </summary>
        public EndReference End0
        {
            get { return m_end; }
            set { m_end = value; }
        }

        /// <summary>
        /// Reference of segment 1.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment1
        {
            get
            {
                UpdateSegmentIdTypeConverter();

                return m_segment1;
            }
            set { m_segment1 = value; }
        }

        /// <summary>
        /// End reference of segment 1.
        /// </summary>
        public EndReference End1
        {
            get { return m_end1; }
            set { m_end1 = value; }
        }

        /// <summary>
        /// Add listening dimension to constrain the length of two bend in the specified direction.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.AddListeningDimensionBendToBend(
                m_parameter.Parameter, m_constraintDirCoordX, m_constraintDirCoordY,
                m_segment, (int)m_end, m_segment1, (int)m_end1);
        }
    }

    /// <summary>
    /// Listening length dimension between a segment and a bend.
    /// </summary>
    class ListeningDimensionSegmentToBend : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Dimension to constrain the length between a segment and a bend
        /// in the specified direction.
        /// </summary>
        private RebarShapeParameter m_parameter;

        /// <summary>
        /// X coordinate of constraint direction.
        /// </summary>
        private double m_constraintDirCoordX;

        /// <summary>
        /// Y coordinate of constraint direction.
        /// </summary>
        private double m_constraintDirCoordY;

        /// <summary>
        /// Reference of segment 0.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// Reference of segment 1.
        /// </summary>
        private int m_segment1;

        /// <summary>
        /// End reference of segment 1.
        /// </summary>
        private EndReference m_end1;

        public ListeningDimensionSegmentToBend(RebarShapeDefBySegment def)
            : base(def)
        {
            m_constraintDirCoordX = 0;
            m_constraintDirCoordY = 0;
            m_end1 = EndReference.End;
        }

        /// <summary>
        /// Dimension to constrain the length between a segment and a bend
        /// in the specified direction.
        /// </summary>
        [TypeConverter(typeof(TypeConverterRebarShapeParameter))]
        public RebarShapeParameter Parameter
        {
            get
            {
                UpdateParameterTypeConverter();
                return m_parameter;
            }
            set 
            {
                m_parameter = value;
            }
        }        

        /// <summary>
        /// X coordinate of constraint direction.
        /// </summary>
        public double ConstraintDirCoordX
        {
            get 
            { 
                return m_constraintDirCoordX; 
            }
            set
            { 
                m_constraintDirCoordX = value;
            }
        }        

        /// <summary>
        /// Y coordinate of constraint direction.
        /// </summary>
        public double ConstraintDirCoordY
        {
            get 
            { 
                return m_constraintDirCoordY; 
            }
            set
            { 
                m_constraintDirCoordY = value; 
            }
        }

        /// <summary>
        /// Reference of segment 0.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment0
        {
            get
            {
                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set { m_segment = value; }
        }

        /// <summary>
        /// Reference of segment 1.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment1
        {
            get
            {
                UpdateSegmentIdTypeConverter();

                return m_segment1;
            }
            set { m_segment1 = value; }
        }

        /// <summary>
        /// End reference of segment 1.
        /// </summary>
        public EndReference End1
        {
            get { return m_end1; }
            set { m_end1 = value; }
        }

        /// <summary>
        /// Add listening dimension to constrain the length between a segment and a bend  
        /// in the specified direction.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.AddListeningDimensionSegmentToBend(
                m_parameter.Parameter, m_constraintDirCoordX, m_constraintDirCoordY,
                m_segment, m_segment1, (int)m_end1);
        }
    }

    /// <summary>
    /// Listening length dimension between two segments.
    /// </summary>
    class ListeningDimensionSegmentToSegment : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Dimension to constrain the perpendicular distance between two segment.
        /// The two segment should be parallel. 
        /// </summary>
        private RebarShapeParameter m_parameter;

        /// <summary>
        /// X coordinate of constraint direction.
        /// </summary>
        private double m_constraintDirCoordX;

        /// <summary>
        /// Y coordinate of constraint direction.
        /// </summary>
        private double m_constraintDirCoordY;

        /// <summary>
        /// The first segment to be constrained.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// The second segment to be constrained.
        /// </summary>
        private int m_segment1;

        public ListeningDimensionSegmentToSegment(RebarShapeDefBySegment def)
            : base(def)
        {
            m_constraintDirCoordX = 1;
            m_constraintDirCoordY = 0;
        }

        /// <summary>
        /// Dimension to constrain the perpendicular distance between two segment.
        /// </summary>
        [TypeConverter(typeof(TypeConverterRebarShapeParameter))]
        public RebarShapeParameter Parameter
        {
            get
            {
                UpdateParameterTypeConverter();
                return m_parameter;
            }
            set 
            { 
                m_parameter = value;
            }
        }

        /// <summary>
        /// X coordinate of constraint direction.
        /// </summary>
        public double ConstraintDirCoordX
        {
            get
            { 
                return m_constraintDirCoordX;
            }
            set
            { 
                m_constraintDirCoordX = value; 
            }
        }

        /// <summary>
        /// Y coordinate of constraint direction.
        /// </summary>
        public double ConstraintDirCoordY
        {
            get 
            { 
                return m_constraintDirCoordY;
            }
            set 
            { 
                m_constraintDirCoordY = value;
            }
        }

        /// <summary>
        /// The second segment to be constrained.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment0
        {
            get
            {
                UpdateSegmentIdTypeConverter();
                return m_segment;
            }
            set { m_segment = value; }
        }

        /// <summary>
        /// The second segment to be constrained.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment1
        {
            get
            {
                UpdateSegmentIdTypeConverter();
                return m_segment1;
            }
            set { m_segment1 = value; }
        }

        /// <summary>
        /// Add dimension to constrain the perpendicular distance between two segment.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.AddListeningDimensionSegmentToSegment(
                m_parameter.Parameter, m_constraintDirCoordX, m_constraintDirCoordY, 
                m_segment, m_segment1);
        }
    }

    /// <summary>
    /// Remove a dimension from a segment.
    /// </summary>
    class RemoveParameterFromSegment : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Reference of segment.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// Dimension to be removed.
        /// </summary>
        private RebarShapeParameter m_radiusParameter;

        public RemoveParameterFromSegment(RebarShapeDefBySegment def)
            : base(def)
        {
        }

        /// <summary>
        /// Reference of segment.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment
        {
            get
            {

                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// Dimension to be removed.
        /// </summary>
        [TypeConverter(typeof(TypeConverterRebarShapeParameter))]
        public RebarShapeParameter RadiusParameter
        {
            get
            {
                UpdateParameterTypeConverter();
                return m_radiusParameter;
            }
            set
            { 
                m_radiusParameter = value;
            }
        }

        /// <summary>
        /// Remove dimension from Rebar shape.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.RemoveParameterFromSegment(
                m_segment, m_radiusParameter.Parameter);
        }
    }

    /// <summary>
    /// A 180 degree bend dimension.
    /// </summary>
    class SetSegmentAs180DegreeBend : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Reference of segment.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// Dimension to constrain the bend's radius.
        /// </summary>
        private RebarShapeParameter m_radiusParameter;

        /// <summary>
        /// If measure to outside of bend.
        /// </summary>
        private bool m_measureToOutsideOfBend;

        public SetSegmentAs180DegreeBend(RebarShapeDefBySegment def)
            : base(def)
        {
            m_measureToOutsideOfBend = true;
        }

        /// <summary>
        /// Reference of segment.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment
        {
            get
            {

                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// Dimension to constrain the bend's radius.
        /// </summary>
        [TypeConverter(typeof(TypeConverterRebarShapeParameter))]
        public RebarShapeParameter RadiusParameter
        {
            get
            {
                UpdateParameterTypeConverter();
                return m_radiusParameter;
            }
            set { m_radiusParameter = value; }
        }

        /// <summary>
        /// If measure the length to outside of bend.
        /// </summary>
        public bool MeasureToOutsideOfBend
        {
            get { return m_measureToOutsideOfBend; }
            set { m_measureToOutsideOfBend = value; }
        }

        /// <summary>
        /// Add a dimension of 180 degree bend for a segment.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.SetSegmentAs180DegreeBend(
                m_segment, m_radiusParameter.Parameter, m_measureToOutsideOfBend);
        }
    }

    /// <summary>
    /// Length dimension of segment in its parallel direction.
    /// </summary>
    class SetSegmentFixedDirection : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Reference of segment.
        /// </summary>
        private int m_segment;

        /// <summary>
        /// X coordinate of constraint direction.
        /// </summary>
        private double m_vecCoordX;

        /// <summary>
        /// Y coordinate of constraint direction.
        /// </summary>
        private double m_vecCoordY;

        public SetSegmentFixedDirection(RebarShapeDefBySegment def)
            : base(def)
        {
            m_vecCoordX = 1;
            m_vecCoordY = 0;
        }

        /// <summary>
        /// Reference of segment.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment
        {
            get
            {

                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// X coordinate of constraint direction.
        /// </summary>
        public double VecCoordX
        {
            get
            { 
                return m_vecCoordX; 
            }
            set
            { 
                m_vecCoordX = value;
            }
        }

        /// <summary>
        /// Y coordinate of constraint direction.
        /// </summary>
        public double VecCoordY
        {
            get
            { 
                return m_vecCoordY; 
            }
            set
            { 
                m_vecCoordY = value;
            }
        }

        /// <summary>
        /// Add dimension to constrain the direction of the segment.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.SetSegmentFixedDirection(
                m_segment, m_vecCoordX, m_vecCoordY);
        }
    }

    /// <summary>
    /// Remove a dimension from a segment.
    /// </summary>
    class SetSegmentVariableDirection : ConstraintOnSegmentShape
    {
        /// <summary>
        /// Reference of segment.
        /// </summary>
        private int m_segment;

        public SetSegmentVariableDirection(RebarShapeDefBySegment def)
            : base(def)
        {
        }

        /// <summary>
        /// Reference of segment.
        /// </summary>
        [TypeConverter(typeof(TypeConverterSegmentId))]
        public int Segment
        {
            get
            {

                UpdateSegmentIdTypeConverter();

                return m_segment;
            }
            set
            {
                m_segment = value;
            }
        }

        /// <summary>
        /// Remove the direction dimension of segment.
        /// </summary>
        public override void Commit()
        {
            GetRebarShapeDefinitionBySegments.SetSegmentVariableDirection(m_segment);
        }
    }
}
