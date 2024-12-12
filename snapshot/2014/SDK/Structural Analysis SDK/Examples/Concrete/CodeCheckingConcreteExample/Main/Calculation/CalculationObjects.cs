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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.Revit.DB.CodeChecking.Engineering.Tools;
using Autodesk.CodeChecking.Concrete;
using CodeCheckingConcreteExample.Engine;
using CodeCheckingConcreteExample.Concrete;
using CodeCheckingConcreteExample.Properties;
using Autodesk.Revit.DB.ExtensibleStorage.Framework.Documentation;
using CodeCheckingConcreteExample.Utility;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.CodeChecking.Storage;

namespace CodeCheckingConcreteExample.Main.Calculation
{
   /// <summary>
   /// Represents user's calculation object which fills cref="Result" with data.
   /// </summary>
   public class FillResultData : ICalculationObject
   {
      #region ICalculationObject Members

      /// <summary>
      /// Runs calculation\operations for cref="BeamElement" and cref="ColumnElement".
      /// </summary>
      /// <param name="obj">cref="BeamElement" object or cref="ColumnElement" object.</param>
      /// <returns>Result of calculation.</returns>
      public bool Run(ObjectDataBase obj)
      {
         ElementDataBase elementData = obj as ElementDataBase;
         if (obj != null)
         {
            switch (elementData.Category)
            {
               default:
                  break;
               case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                  if (elementData.Result is ResultLinearElement)
                  {
                     CodeCheckingConcreteExample.Main.ResultLinearElement res = elementData.Result as ResultLinearElement;
                     if (res != null)
                     {
                        elementData.ListSectionData.ForEach(s => res.ValuesInPointsData.AddRange((s as LinearSection).GetCalcResultsInPoint().DataRaw));
                     }

                     UnitType xUnitType = ResultTypeLinear.X.GetUnitType();
                     DisplayUnitType xInternalDisplayUnitType = UnitsConverter.GetInternalUnit(xUnitType);
                     
                     // add info
                     IEnumerable<IGrouping<String, Tuple<String, Double>>> infoInSec = elementData.ListSectionData.
                        SelectMany(lsd => (lsd as LinearSection).DesignInfo.Select(dw => new Tuple<String, Double>(dw, (lsd.CalcPoint as CalcPointLinear).CoordAbsolute))).
                        GroupBy(s => s.Item1);

                     // add warnings
                     IEnumerable<IGrouping<String, Tuple<String, Double>>> warningsInSec = elementData.ListSectionData.
                        SelectMany(lsd => (lsd as LinearSection).DesignWarning.Select(dw => new Tuple<String, Double>(dw, (lsd.CalcPoint as CalcPointLinear).CoordAbsolute))).
                        GroupBy(s => s.Item1);

                     // add errors
                     IEnumerable<IGrouping<String, Tuple<String, Double>>> errosInSec = elementData.ListSectionData.
                        SelectMany(lsd => (lsd as LinearSection).DesignError.Select(dw => new Tuple<String, Double>(dw, (lsd.CalcPoint as CalcPointLinear).CoordAbsolute))).
                        GroupBy(s => s.Item1);

                     if (errosInSec.Count() > 0)
                     {
                        getResultStatusMessage(errosInSec, xInternalDisplayUnitType).ForEach(s => elementData.AddFormatedError(s));
                        elementData.Status.Status = Status.Failed;
                     }
                     else
                     {
                        if (warningsInSec.Count() > 0)
                        {
                           getResultStatusMessage(warningsInSec, xInternalDisplayUnitType).ForEach(s => elementData.AddFormatedWarning(s));
                           if (Status.Failed != elementData.Status.Status)
                              elementData.Status.Status = Status.Warning;
                        }
                        if (infoInSec.Count() > 0)
                        {
                           getResultStatusMessage(infoInSec, xInternalDisplayUnitType).ForEach(s => elementData.AddFormatedInfo(s));
                        }
                     } 
                     if (Autodesk.Revit.DB.CodeChecking.Storage.Status.Undefined == elementData.Status.Status)
                        elementData.Status.Status = Autodesk.Revit.DB.CodeChecking.Storage.Status.Succeed;
                  }
                  break;
            }
         }

         return true;
      }

      List<ResultStatusMessage> getResultStatusMessage(IEnumerable<IGrouping<String, Tuple<String, Double>>> messages, DisplayUnitType xInternalDisplayUnitType)
      {
         List<ResultStatusMessage> resultStatusMessages = new List<ResultStatusMessage>();
         foreach (IGrouping<String, Tuple<String, Double>> message in messages)
         {
            ResultStatusMessage msg = new ResultStatusMessage();
            msg.Message.Add(new ResultStatusMessageItem(message.Key));
            msg.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("AppliesToSections")));

            foreach (Tuple<String, Double> s in message)
            {
               msg.Message.Add(new ResultStatusMessageItem(UnitType.UT_Length, xInternalDisplayUnitType, (double)s.Item2));
               msg.Message.Add(new ResultStatusMessageItem("; "));
            }
            resultStatusMessages.Add(msg);
         }
         return resultStatusMessages;
      }

      /// <summary>
      /// Sets and gets common parameters.
      /// </summary>
      public CommonParametersBase Parameters { get; set; }

      /// <summary>
      /// Gets and sets type of the calculation object.
      /// </summary>
      public CalculationObjectType Type { get; set; }

      /// <summary>
      /// Gets and sets a type of error response of the calculation object.
      /// </summary>
      public ErrorResponse ErrorResponse { get; set; }

      /// <summary>
      /// Gets and sets categories for the calculation object.
      /// </summary>
      public IList<Autodesk.Revit.DB.BuiltInCategory> Categories { get; set; }

      #endregion
   }
   /// <summary>
   /// Represents user's calculation object which calculate deflection of a beam epresented by cref="BeamElement".
   /// </summary>
   public class CalculateDeflection : ICalculationObject
   {
      #region ICalculationObject Members

      /// <summary>
      /// Runs calculation\operations for cref="BeamElement".
      /// </summary>
      /// <param name="obj">cref="BeamElement".</param>
      /// <returns>Result of calculation.</returns>
      public bool Run(ObjectDataBase obj)
      {
         ElementDataBase elementData = obj as ElementDataBase;
         if (obj != null)
         {
            BeamElement objBeam = obj as BeamElement;
            if (objBeam != null)
            {
               CodeCheckingConcreteExample.Main.ResultBeam res = elementData.Result as CodeCheckingConcreteExample.Main.ResultBeam;
               if (res != null)
               {
                  double young = objBeam.Info.Material.Characteristics.YoungModulus.X;
                  double maxCoef = 0;
                  double stifCoef = 0.0;
                  double avrCoef = 0.0;
                  int sectionNo = 0;
                  foreach (SectionDataBase sd in elementData.ListSectionData)
                  {
                     BeamSection sec = sd as BeamSection;
                     
                     if (sec != null)
                     {
                        if (sec.MinStiffness > Double.Epsilon)
                        {
                           CalcPointLinear calcPoint = sec.CalcPoint as CalcPointLinear;
                           double Iy = sec.Info.SectionsParams.AtThePoint(calcPoint.CoordRelative).Characteristics.Iy;
                           stifCoef = (Iy * young) / sec.MinStiffness;
                        }
                        else
                        {
                           stifCoef = 0;
                           break;
                        }
                        ++sectionNo;
                        maxCoef = Math.Max(maxCoef, stifCoef);
                        avrCoef += stifCoef;
                     }
                  }
                  double finStiffnes = 0;
                  if (stifCoef > Double.Epsilon)
                  {
                     avrCoef /= sectionNo;
                     finStiffnes = 0.5 * (maxCoef + avrCoef);
                  }
                  else
                  {
                     objBeam.AddFormatedWarning(new ResultStatusMessage("Calculation of deflections was not performed."));
                  }
                  foreach (SectionDataBase sd in elementData.ListSectionData)
                  {
                     BeamSection sec = sd as BeamSection;
                     if (sec != null)
                        sec.StiffnesCoeff = finStiffnes;
                  }
               }
            }
         }
         return true;
      }

      /// <summary>
      /// Sets and gets common parameters.
      /// </summary>
      public CommonParametersBase Parameters { get; set; }

      /// <summary>
      /// Gets and sets type of the calculation object.
      /// </summary>
      public CalculationObjectType Type { get; set; }

      /// <summary>
      /// Gets and sets a type of error response of the calculation object.
      /// </summary>
      public ErrorResponse ErrorResponse { get; set; }

      /// <summary>
      /// Gets and sets categories for the calculation object.
      /// </summary>
      public IList<Autodesk.Revit.DB.BuiltInCategory> Categories { get; set; }

      #endregion
   }
 
   /// <summary>
   /// Represents user's calculation object which modify element forces.
   /// </summary>
   public class ModifyElementForces : ICalculationObject
   {
      #region ICalculationObject Members

      /// <summary>
      /// Runs calculation\operations for cref="BeamElement" and cref="ColumnElement".
      /// </summary>
      /// <param name="obj">cref="BeamElement" object or cref="ColumnElement" object.</param>
      /// <returns>Result of calculation.</returns>
      public bool Run(ObjectDataBase obj)
      {
         bool isOk = true;
         if (obj != null)
         {
            switch (obj.Category)
            {
               default:
                  isOk = false;
                  break;
               case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                  {
                     ModifyForcesBecauseOfBuckling(obj);
                  }
                  break;
               case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                  {
                     ModifySupportForces(obj);
                  }
                  break;
            }
         }
         return isOk;
      }
      /// <summary>
      /// Example explaining how to use force modification for beam:
      /// additonal minimum bending moment over support M=0.1*Mmax in ULS 
      /// </summary>
      /// <param name="obj">cref="BeamElement" object or cref="ColumnElement" object.</param>
      private void ModifySupportForces(ObjectDataBase obj)
      {
         BeamElement elem = obj as BeamElement;
         double maximumM = Double.MinValue;
         double maximumL = Double.MinValue;
         double minimumL = Double.MaxValue;
         int beginIndexSupport = -1;
         int endIndexSupport = -1;
         int index = 0;
         bool isMaximumM = false;
         foreach (SectionDataBase sd in elem.ListSectionData)
         {
            BeamSection sec = sd as BeamSection;
            CalcPointLinear calcPoint = sec.CalcPoint as CalcPointLinear;
            if (calcPoint != null)
            {
               if (maximumL < calcPoint.CoordRelative)
               {
                  maximumL = calcPoint.CoordRelative;
                  endIndexSupport = index;
               }
               if (minimumL > calcPoint.CoordRelative)
               {
                  minimumL = calcPoint.CoordRelative;
                  beginIndexSupport = index;
               }
            }
            if (sec != null)
            {
               foreach (InternalForcesBase forces in sec.ListInternalForces)
               {
                  InternalForcesLinear f = forces as InternalForcesLinear;
                  if (f != null)
                  {
                     if (f.Forces.LimitState == ForceLimitState.Uls)
                     {
                        if (maximumM < f.Forces.MomentMy)
                        {
                           maximumM = f.Forces.MomentMy;
                           isMaximumM = true;
                        }
                     }
                  }
               }
            }
            index++;
         }
         if (isMaximumM)
         {
            if (beginIndexSupport > -1)
            {
               BeamSection bs = elem.ListSectionData[beginIndexSupport] as BeamSection;
               if (bs != null)
               {
                  InternalForcesLinear f = new InternalForcesLinear();
                  f.Forces.LimitState = ForceLimitState.Uls;
                  f.Forces.MomentMy = 0.1 * maximumM;
                  bs.ListInternalForces.Add(f);
               }
            }
            if (endIndexSupport > -1)
            {
               BeamSection bs = elem.ListSectionData[endIndexSupport] as BeamSection;
               if (bs != null)
               {
                  InternalForcesLinear f = new InternalForcesLinear();
                  f.Forces.LimitState = ForceLimitState.Uls;
                  f.Forces.MomentMy = 0.1 * maximumM;
                  bs.ListInternalForces.Add(f);
               }
            }
         }
      }
      /// <summary>
      /// Simple buckling effect calculation:
      /// This is only example: braced (non-sway) structure and single curvature!
      /// </summary>
      /// <param name="obj">cref="BeamElement" object or cref="ColumnElement" object.</param>
      private void ModifyForcesBecauseOfBuckling(ObjectDataBase obj)
      {
         ColumnElement elem = obj as ColumnElement;
         CodeCheckingConcreteExample.Main.LabelColumn rcLabelColumn = elem.Label as CodeCheckingConcreteExample.Main.LabelColumn;
         bool dirY = rcLabelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MY);
         bool dirZ = rcLabelColumn.EnabledInternalForces.Contains(ConcreteTypes.EnabledInternalForces.MZ);
         dirY &= rcLabelColumn.BucklingDirectionY;
         dirZ &= rcLabelColumn.BucklingDirectionZ;
         if (dirY || dirZ)
         {
            double lambdaLim = 15.0;
            double lambdaY = 0;
            double lambdaZ = 0;
            double length = elem.Info.GeomLength();

            bool modifMy = false;
            bool modifMz = false;
            if (dirY)
            {
               lambdaY = (length * rcLabelColumn.LengthCoefficientY) / elem.Info.SectionsParams.AtTheBeg.Characteristics.ry;
               modifMy = lambdaY > lambdaLim;
            }
            if (dirZ)
            {
               lambdaZ = (length * rcLabelColumn.LengthCoefficientZ) / elem.Info.SectionsParams.AtTheBeg.Characteristics.rz;
               modifMz = lambdaZ > lambdaLim;
            }
            double additionalMy = 0;
            double additionalMz = 0;
            double My = 0;
            double Mz = 0;
            double N = 0;
            double dimY = elem.Info.SectionsParams.AtTheBeg.Dimensions.vpy + elem.Info.SectionsParams.AtTheBeg.Dimensions.vy;
            double dimZ = elem.Info.SectionsParams.AtTheBeg.Dimensions.vpz + elem.Info.SectionsParams.AtTheBeg.Dimensions.vz;
            foreach (SectionDataBase sd in elem.ListSectionData)
            {
               ColumnSection sec = sd as ColumnSection;
               if (sec != null)
               {
                  foreach (InternalForcesBase forces in sec.ListInternalForces)
                  {
                     InternalForcesLinear f = forces as InternalForcesLinear;
                     if (f != null)
                     {
                        if (f.Forces.LimitState == ForceLimitState.Uls)
                        {
                           CalcPointLinear calcPoint = sec.CalcPoint as CalcPointLinear;
                           N = f.Forces.ForceFx;
                           if (N > Double.Epsilon)
                           {
                              if (dirY)
                              {
                                 My = f.Forces.MomentMy;
                                 additionalMy = (-0.002 * N / dimZ);
                                 additionalMy *= rcLabelColumn.LengthCoefficientY * rcLabelColumn.LengthCoefficientY;
                                 additionalMy *= (calcPoint.CoordAbsolute * calcPoint.CoordAbsolute - length * calcPoint.CoordAbsolute);
                                 f.Forces.MomentMy += f.Forces.MomentMy > 0.0 ? additionalMy : -additionalMy;
                              }
                              if (dirZ)
                              {
                                 Mz = f.Forces.MomentMz;
                                 additionalMz = (-0.002 * N / dimY);
                                 additionalMz *= rcLabelColumn.LengthCoefficientZ * rcLabelColumn.LengthCoefficientZ;
                                 additionalMz *= (calcPoint.CoordAbsolute * calcPoint.CoordAbsolute - length * calcPoint.CoordAbsolute);
                                 f.Forces.MomentMz += f.Forces.MomentMz > 0.0 ? additionalMz : -additionalMz;
                              }
                           }
                        }
                     }
                  }
               }
            }
         }
      }
      /// <summary>
      /// Sets and gets common parameters.
      /// </summary>
      public CommonParametersBase Parameters { get; set; }

      /// <summary>
      /// Gets and sets type of the calculation object.
      /// </summary>
      public CalculationObjectType Type { get; set; }

      /// <summary>
      /// Gets and sets a type of error response of the calculation object.
      /// </summary>
      public ErrorResponse ErrorResponse { get; set; }

      /// <summary>
      /// Gets and sets categories for the calculation object.
      /// </summary>
      public IList<Autodesk.Revit.DB.BuiltInCategory> Categories { get; set; }

      #endregion
   }

   /// <summary>
   /// Represents user's calculation object which prepare data for elements' sections.
   /// </summary>
   public class PrepareSectionData : ICalculationObject
   {
      #region ICalculationObject Members

     /// <summary>
     /// Runs calculation\operations for cref="BeamSection" and cref="ColumnSection".
     /// </summary>
     /// <param name="obj">cref="BeamSection" object or cref="ColumnSection" object.</param>
     /// <returns>Result of calculation.</returns>
     public bool Run(ObjectDataBase obj)
	  {
		  SectionDataBase sectionData = obj as SectionDataBase;
		  if (obj != null)
		  {
			  CommonParameters commParams = Parameters as CommonParameters;

			  ForceResultsCache cache = null;
			  if (commParams != null)
				  cache = commParams.ResultCache;

           Section sec = null;

           switch (sectionData.Category)
           {
				 default:
					 break;
				 case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                if (sectionData is ColumnSection)
                {
                   ColumnSection section = sectionData as ColumnSection;

                   if (section.Info != null)
                   {
                      SectionsInfo secInfo = section.Info.Sections.AtTheBeg;

                      if (secInfo.Sections.Count > 0)
                         sec = secInfo.Sections[0];
                   }
                }
                break;
				 case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
                if (sectionData is BeamSection && sectionData.Label is LabelBeam)
                {
                   BeamSection section = sectionData as BeamSection;
                   LabelBeam label = sectionData.Label as LabelBeam;

                   if (section.Info != null)
                   {
                      SectionsInfo secInfo = section.Info.Sections.AtTheBeg;

                      if (secInfo.Sections.Count > 0)
                      {
                         sec = secInfo.Sections[0];

                         if (label.SlabBeamInteraction == ConcreteTypes.BeamSectionType.WithSlabBeamInteraction && section.Info.Slabs != null && section.Info.Slabs.TSection != null)
                         {
                            int nbrPoints = (sec.Contour != null && sec.Contour.Points != null) ? sec.Contour.Points.Count : 0;
                            double relativeX = (section.CalcPoint as CalcPointLinear).CoordRelative;
                            double maxFlangeWidth = 0.5 * section.Info.SectionsParams.AtThePoint(relativeX).Dimensions.h;
                            sec = section.Info.Slabs.TSection.GetContour( relativeX, maxFlangeWidth, maxFlangeWidth );
                            if (sec.Contour.Points.Count != nbrPoints)
                            {
                               section.IsTSection = true;
                               section.DesignInfo.Add(Resources.ResourceManager.GetString("SlabBeamInteraction"));
                            }
                         }
                      }
                   }
                }
                break;
          }

			 switch (sectionData.Category)
			 {
				 default:
					 break;
				 case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
				 case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
					 if (sectionData is LinearSection)
					 {
						 LinearSection section = sectionData as LinearSection;

						if (sec != null && sec.Contour != null)
						{
							foreach (Autodesk.Revit.DB.XYZ p in sec.Contour.Points)
								section.Geometry.Add(p.X, p.Y);

							Autodesk.Revit.DB.XYZ pmin = sec.GetMinimumBoundary();
							Autodesk.Revit.DB.XYZ pmax = sec.GetMaximumBoundary();
							section.Width = Math.Abs(pmin.X - pmax.X);
							section.Height = Math.Abs(pmin.Y - pmax.Y);
						}


						 if (cache != null)
						 {
                      IList<ForceLoadCaseDescriptor> loadCaseDescriptors = cache.GetLoadCaseDescriptors();
							 List<double> vFx = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.Fx);
							 List<double> vFy = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.Fy);
							 List<double> vFz = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.Fz);
							 List<double> vMx = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.Mx);
							 List<double> vMy = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.My);
							 List<double> vMz = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.Mz);

                      List<double> vUx = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.Ux);
                      List<double> vUy = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.Uy);
                      List<double> vUz = cache.GetForceForPoint(sectionData.ElementId, loadCaseDescriptors, sectionData.CalcPoint, ForceType.Uz);

							 int i = -1;
                      foreach (ForceLoadCaseDescriptor cas in loadCaseDescriptors)
							 {
								 i++;
								 InternalForcesLinear linForces = new InternalForcesLinear();
								 InternalForcesContainer forces = new InternalForcesContainer();

                         forces.CaseName = cas.Name;
								 forces.LimitState = cas.State;

                           forces.ForceFx = vFx.Count == 0 ? 0.0 : vFx[i];
                           forces.ForceFy = vFy.Count == 0 ? 0.0 : vFy[i];
                           forces.ForceFz = vFz.Count == 0 ? 0.0 : vFz[i];
                           forces.MomentMx = vMx.Count == 0 ? 0.0 : vMx[i];
                           forces.MomentMy = vMy.Count == 0 ? 0.0 : vMy[i];
                           forces.MomentMz = vMz.Count == 0 ? 0.0 : vMz[i];
                           forces.DeflectionUx = vUx.Count == 0 ? 0.0 : vUx[i];
                           forces.DeflectionUy = vUy.Count == 0 ? 0.0 : vUy[i];
                           forces.DeflectionUz = vUz.Count == 0 ? 0.0 : vUz[i];

								 linForces.Forces = forces;

								 section.ListInternalForces.Add(linForces);
							 }
						 }
					 }
					 break;
				 }

		 }
		 return true;
	  }

     /// <summary>
     /// Sets and gets common parameters.
     /// </summary>
     public CommonParametersBase Parameters { get; set; }

     /// <summary>
     /// Gets and sets type of the calculation object.
     /// </summary>
     public CalculationObjectType Type { get; set; }

     /// <summary>
     /// Gets and sets a type of error response of the calculation object.
     /// </summary>
     public ErrorResponse ErrorResponse { get; set; }

     /// <summary>
     /// Gets and sets categories for the calculation object.
     /// </summary>
     public IList<Autodesk.Revit.DB.BuiltInCategory> Categories { get; set; }

     #endregion
   }

   /// <summary>
   /// Represents user's calculation object which calculate sections.
   /// </summary>
   public class CalculateSection : ICalculationObject
   {
      #region ICalculationObject Members

      /// <summary>
      /// Runs calculation\operations for cref="BeamSection" and cref="ColumnSection".
      /// </summary>
      /// <param name="obj">cref="BeamSection" object or cref="ColumnSection" object.</param>
      /// <returns>Result of calculation.</returns>
      public bool Run(ObjectDataBase obj)
      {
         bool ret = true;
         SectionDataBase sectionData = obj as SectionDataBase;
         if (obj != null)
         {

            Concrete.ConcreteSectionDesign design = new ConcreteSectionDesign();
            design.ElementType = sectionData.Category;

            switch (sectionData.Category)
            {
               default:
                  break;
               case BuiltInCategory.OST_ColumnAnalytical:
               case BuiltInCategory.OST_BeamAnalytical:
                  if (sectionData.Label is LabelColumn)
                  {
                     CodeCheckingConcreteExample.Main.LabelColumn label = sectionData.Label as CodeCheckingConcreteExample.Main.LabelColumn;
                     design.LongitudinalReinforcementMinimumYieldStress = label.LongitudinalReinforcement.MinimumYieldStress;
                     design.LongitudinalReinforcementArea = label.LongitudinalReinforcement.Area;
                     design.LongitudinalReinforcementDiameter = label.LongitudinalReinforcement.BarDiameter;
                     design.LongitudinalCalculationType = label.LongitudinalCalculationType;
                     ///
                     design.TransversalReinforcementMinimumYieldStress = label.TransversalReinforcement.MinimumYieldStress;
                     design.TransversalReinforcementArea = label.TransversalReinforcement.Area;
                     design.TransversalReinforcementDiameter = label.TransversalReinforcement.BarDiameter;
                     design.TransversalCalculationType = label.TransversalCalculationType;
                     design.CreepCoefficient = label.CreepCoefficient;
                  }

                  if (sectionData.Label is LabelBeam)
                  {
                     CodeCheckingConcreteExample.Main.LabelBeam label = sectionData.Label as CodeCheckingConcreteExample.Main.LabelBeam;

                     design.LongitudinalReinforcementMinimumYieldStress = label.LongitudinalReinforcement.MinimumYieldStress;
                     design.LongitudinalReinforcementArea = label.LongitudinalReinforcement.Area;
                     design.LongitudinalReinforcementDiameter = label.LongitudinalReinforcement.BarDiameter;
                     design.LongitudinalCalculationType = label.LongitudinalCalculationType;
                     ///
                     design.TransversalReinforcementMinimumYieldStress = label.TransversalReinforcement.MinimumYieldStress;
                     design.TransversalReinforcementArea = label.TransversalReinforcement.Area;
                     design.TransversalReinforcementDiameter = label.TransversalReinforcement.BarDiameter;
                     design.TransversalCalculationType = label.TransversalCalculationType;
                     design.CreepCoefficient = label.CreepCoefficient;
                  }

                  if (sectionData is LinearSection)
                  {
                     LinearSection section = sectionData as LinearSection;
                     if (section.Info != null)
                     {
                        design.CoverTop = 0.0;
                        design.CoverBottom = 0.0;

                        if (section.Info.RebarCovers != null)
                        {
                           if (section is BeamSection)
                           {
                              if (section.Info.RebarCovers.Exist(BuiltInParameter.CLEAR_COVER_BOTTOM))
                                 design.CoverBottom = section.Info.RebarCovers.GetDistance(BuiltInParameter.CLEAR_COVER_BOTTOM);
                              if (section.Info.RebarCovers.Exist(BuiltInParameter.CLEAR_COVER_TOP))
                                 design.CoverTop = section.Info.RebarCovers.GetDistance(BuiltInParameter.CLEAR_COVER_TOP);
                           }
                           if (section is ColumnSection)
                           {
                              if (section.Info.RebarCovers.Exist(BuiltInParameter.CLEAR_COVER_OTHER))
                              {
                                 design.CoverBottom = section.Info.RebarCovers.GetDistance(BuiltInParameter.CLEAR_COVER_OTHER);
                                 design.CoverTop = section.Info.RebarCovers.GetDistance(BuiltInParameter.CLEAR_COVER_OTHER);
                              }
                           }
                        }

                        if (section.Info.SectionsParams == null)
                           design.Type = SectionShapeType.RectangularBar;
                        else
                           design.Type = section.Info.SectionsParams.ShapeType;
                        design.YoungModulus = section.Info.Material.Characteristics.YoungModulus.X;

                        MaterialConcreteCharacteristics concrete = (MaterialConcreteCharacteristics)section.Info.Material.Characteristics.Specific;

                        if (concrete != null)
                           design.Compression = concrete.Compression;

                        design.Geometry = section.Geometry;
                        design.Width = section.Width;
                        design.Height = section.Height;

                        List<InternalForcesContainer> listInternalForces = new List<InternalForcesContainer>();
                        foreach (InternalForcesLinear force in section.ListInternalForces)
                           listInternalForces.Add(force.Forces);

                        design.ListInternalForces = listInternalForces; ;

                        //
                        // calculation
                        //
                        design.Calculate();

                        //
                        // save result
                        //
                        section.AsBottom = design.AsBottom;
                        section.AsTop = design.AsTop;
                        section.AsRight = design.AsRight;
                        section.AsLeft = design.AsLeft;
                        section.Spacing = design.Spacing;
                        section.TransversalDensity = design.TransversalDensity;
                        section.MinStiffness = design.MinStiffness;
                        section.DesignInfo.AddRange(design.DesignInfo);
                        section.DesignError.AddRange(design.DesignError);
                        section.DesignWarning.AddRange(design.DesignWarning);


                        if (section.DesignError.Count > 0)
                        {
                           ret = false;
                     }
                  }
                  }
                  break;

            }

         }
         return ret;
      }

      /// <summary>
      /// Sets and gets common parameters.
      /// </summary>
      public CommonParametersBase Parameters { get; set; }

      /// <summary>
      /// Gets and sets type of the calculation object.
      /// </summary>
      public CalculationObjectType Type { get; set; }

      /// <summary>
      /// Gets and sets a type of error response of the calculation object.
      /// </summary>
      public ErrorResponse ErrorResponse { get; set; }

      /// <summary>
      /// Gets and sets categories for the calculation object.
      /// </summary>
      public IList<Autodesk.Revit.DB.BuiltInCategory> Categories { get; set; }

      #endregion
   }
}
