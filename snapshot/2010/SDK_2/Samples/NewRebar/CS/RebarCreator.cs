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
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Structural.Enums;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Geometry;
using System.Windows.Forms;
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// This class wraps the creation of Rebar. Its "Execute" method shows
    /// the main dialog for user and after that a Rebar will be created 
    /// if user click OK button on the main dialog.
    /// </summary>
    class RebarCreator
    {
        /// <summary>
        /// Revit Application object.
        /// </summary>
        Autodesk.Revit.Application m_rvtApp;

        /// <summary>
        /// Revit FamilyInstance object, it will be the host of Rebar.
        /// </summary>
        FamilyInstance m_rebarHost;

        /// <summary>
        /// GeometrySupport object.
        /// </summary>
        GeometrySupport m_geometryData;

        /// <summary>
        /// Newly created Rebar.
        /// </summary>
        Rebar m_createdRebar = null;

        /// <summary>
        /// Constructor, initialize fields and do some assert.
        /// If the Assert throw exception, creation will fail.
        /// </summary>
        /// <param name="commandData">ExternalCommandData</param>
        public RebarCreator(ExternalCommandData commandData)
        {
            m_rvtApp = commandData.Application;
            Assert();
        }

        /// <summary>
        /// Do some check for the selection elements, includes geometry check.
        /// If the data doesn't meet our need, Exception will be thrown.
        /// </summary>
        private void Assert()
        {
            FamilyInstance rebarHost = null;
            foreach (Autodesk.Revit.Element elem in m_rvtApp.ActiveDocument.Selection.Elements)
            {
                if (elem is FamilyInstance)
                {
                    rebarHost = elem as FamilyInstance;
                    if ((rebarHost.StructuralType == StructuralType.Beam ||
                        rebarHost.StructuralType == StructuralType.Column) &&
                        rebarHost.Material == Autodesk.Revit.Structural.Enums.Material.Concrete)
                    {
                        // Make sure the selected beam or column is rectangular.
                        try
                        {
                            m_geometryData = new GeometrySupport(rebarHost);
                        }
                        catch
                        {
                            throw new Exception("Please select a beam or column in rectangular shape.");
                        }

                        m_rebarHost = rebarHost;                        

                        // Make sure the selected beam or column doesn't contain any rebar.
                        ElementIterator itor = m_rvtApp.ActiveDocument.get_Elements(typeof(Rebar));
                        while (itor.MoveNext())
                        {
                            Rebar rebar = itor.Current as Rebar;
                            if (null != rebar && null != rebar.Host && rebar.Host.Id.Value == m_rebarHost.Id.Value)
                            {
                                throw new Exception("Please select a beam or a column which doesn't contain any rebar.");
                            }
                        }                        

                        return;
                    }
                }
            }

            throw new Exception("Please select a concrete beam or column to create rebar.");
        }

        /// <summary>
        /// Present the main dialog for user to prepare the parameters for Rebar creation,
        /// and after that if user click the OK button, a new Rebar will be created.
        /// </summary>
        public void Execute()
        {
            using (NewRebarForm form = new NewRebarForm(m_rvtApp))
            {
                if (DialogResult.OK == form.ShowDialog())
                {
                    RebarBarType barType = form.RebarBarType;
                    RebarShape barShape = form.RebarShape;

                    List<XYZ> profilePoints = m_geometryData.ProfilePoints;
                    XYZ origin = profilePoints[0];
                    XYZ yVec = profilePoints[1] - origin;
                    XYZ xVec = profilePoints[3] - origin;

                    m_createdRebar = m_rvtApp.ActiveDocument.Create.
                        NewRebar(barShape, barType, m_rebarHost, origin, xVec, yVec);

                    ScaleRebarToBox();
                    SetRebarLayoutRuleWithNumberAndSpacing(0.1);
                }
            }
        }

        /// <summary>
        /// Move and scale the Rebar to a specified box.
        /// </summary>
        private void ScaleRebarToBox()
        {
            List<XYZ> profilePoints = m_geometryData.OffsetPoints(0.1);

            XYZ origin = profilePoints[0];
            XYZ yVec = profilePoints[1] - origin;
            XYZ xVec = profilePoints[3] - origin;
            m_createdRebar.ScaleToBox(origin, xVec, yVec);
        }

        /// <summary>
        /// Set layout rule of Rebar to "number and spacing".
        /// </summary>
        /// <param name="spacing">Rebar spacing</param>
        private void SetRebarLayoutRuleWithNumberAndSpacing(double spacing)
        {
            m_createdRebar.SetLayoutRuleWithoutExaminingHost(RebarLayoutRule.NumberWithSpacing);
            Parameter spacingParam = m_createdRebar.
                get_Parameter(BuiltInParameter.REBAR_ELEM_BAR_SPACING);
            Parameter quantityParam = m_createdRebar.
                get_Parameter(BuiltInParameter.REBAR_ELEM_QUANTITY_OF_BARS);

            quantityParam.Set(m_geometryData.DrivingLength / spacing);
            spacingParam.Set(spacing);
        }
    }
}
