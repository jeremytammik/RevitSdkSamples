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
using Autodesk.Revit.Symbols;
using Autodesk.Revit;
using Autodesk.Revit.Parameters;
using System.Windows.Forms;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// This class wraps RebarShapeDefinition object.
    /// </summary>
    public abstract class RebarShapeDef
    {
        /// <summary>
        /// RearShape definition, the real object to be wrapped.
        /// </summary>
        protected RebarShapeDefinition m_rebarshapeDefinition;

        /// <summary>
        /// All the parameters will be added to RebarShapeDefinition.
        /// </summary>
        protected List<RebarShapeParameter> m_parameters;

        /// <summary>
        /// All the dimensions will be added to RebarShapeDefinition.
        /// </summary>
        protected List<ConstraintOnRebarShape> m_constraints;

        /// <summary>
        /// Return the real object RebarShapeDefinition.
        /// </summary>
        public RebarShapeDefinition RebarshapeDefinition
        {
            get { return m_rebarshapeDefinition; }
        }

        /// <summary>
        /// Return all the parameters.
        /// </summary>
        public List<RebarShapeParameter> Parameters
        {
            get { return m_parameters; }
        }

        /// <summary>
        /// Return all the dimensions.
        /// </summary>
        public List<ConstraintOnRebarShape> Constraints
        {
            get { return m_constraints; }
        }

        /// <summary>
        /// Constructor, initialize the fields.
        /// </summary>
        /// <param name="shapeDef">RebarShapeDefinition object to be wrapped</param>
        public RebarShapeDef(RebarShapeDefinition shapeDef)
        {
            m_rebarshapeDefinition = shapeDef;
            m_parameters = new List<RebarShapeParameter>();
            m_constraints = new List<ConstraintOnRebarShape>();
        }

        /// <summary>
        /// Add a parameter to RebarShapeDefinition.
        /// </summary>
        /// <param name="parameterType">Parameter type: 
        /// (type of RebarShapeParameterDouble or type of RebarShapeParameterFormula)</param>        
        /// <param name="name">Parameter name</param>
        /// <param name="value">Parameter value (double value or formula string)</param>
        /// <returns></returns>
        public RebarShapeParameter AddParameter(Type parameterType, object name, object value)
        {
            RebarShapeParameter param =
                Activator.CreateInstance(parameterType, this, name, value) as RebarShapeParameter;
            m_parameters.Add(param);
            return param;
        }

        /// <summary>
        /// Add a constraint to RebarShapeDefinition.
        /// </summary>
        /// <param name="constraintType">Type of constraint
        /// (the class must be subclass of ConstraintOnRebarShape).</param>
        /// <returns></returns>
        public ConstraintOnRebarShape AddConstraint(Type constraintType)
        {
            ConstraintOnRebarShape constraintIns =
                Activator.CreateInstance(constraintType, this) as ConstraintOnRebarShape;
            m_constraints.Add(constraintIns);
            return constraintIns;
        }

        /// <summary>
        /// Submit RebarShapeDefinition. All the parameters and constraints
        /// will be added to RebarShape. The RebarShape will be added to Revit document after 
        /// successfully submitted.
        /// </summary>
        /// <param name="defGroup">Parameter definition group</param>
        public void Commit(DefinitionGroup defGroup)
        {
            // Submit all the parameters.
            foreach (RebarShapeParameter param in m_parameters)
            {
                param.Commit(defGroup);
            }

            // Submit all the constraints.
            foreach (ConstraintOnRebarShape constraint in m_constraints)
            {
                constraint.Commit();
            }

            // Submit the RebarShape.
            m_rebarshapeDefinition.Commit();
            m_rebarshapeDefinition.CheckDefaultParameterValues(0, 0);
        }

        /// <summary>
        /// Return all the parameter types supported by RebarShape definition.
        /// </summary>
        /// <returns>All the parameter types supported by RebarShape definition</returns>
        public List<Type> AllParameterTypes()
        {
            List<Type> types = new List<Type>();
            types.Add(typeof(RebarShapeParameterDouble));
            types.Add(typeof(RebarShapeParameterFormula));
            return types;
        }

        /// <summary>
        /// Return all the constraint types supported by RebarShapeDefinition.
        /// </summary>
        /// <returns>all the constraint types supported by RebarShapeDefinition</returns>
        public virtual List<Type> AllowedConstraintTypes()
        {
            return new List<Type>();
        }
    }
}
