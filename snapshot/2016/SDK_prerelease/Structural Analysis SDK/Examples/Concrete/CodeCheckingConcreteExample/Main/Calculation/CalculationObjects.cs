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
using CodeCheckingConcreteExample.ConcreteTypes;
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
                        getResultStatusMessageLinear(errosInSec, xInternalDisplayUnitType, false, elementData.ListSectionData.Count).ForEach(s => elementData.AddFormatedError(s));
                        elementData.Status.Status = Status.Failed;
                     }
                     else
                     {
                        if (warningsInSec.Count() > 0)
                        {
                           getResultStatusMessageLinear(warningsInSec, xInternalDisplayUnitType, true, elementData.ListSectionData.Count).ForEach(s => elementData.AddFormatedWarning(s));
                           if (Status.Failed != elementData.Status.Status)
                              elementData.Status.Status = Status.Warning;
                        }
                        if (infoInSec.Count() > 0)
                        {
                           getResultStatusMessageLinear(infoInSec, xInternalDisplayUnitType, true, elementData.ListSectionData.Count).ForEach(s => elementData.AddFormatedInfo(s));
                        }
                     } 
                     if (Autodesk.Revit.DB.CodeChecking.Storage.Status.Undefined == elementData.Status.Status)
                        elementData.Status.Status = Autodesk.Revit.DB.CodeChecking.Storage.Status.Succeed;
                  }
                  break;
               /// <structural_toolkit_2015>
               case Autodesk.Revit.DB.BuiltInCategory.OST_FloorAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_FoundationSlabAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                  if (elementData.Result is CodeCheckingConcreteExample.Main.ResultSurfaceElement)
                  {
                     CodeCheckingConcreteExample.Main.ResultSurfaceElement res = elementData.Result as CodeCheckingConcreteExample.Main.ResultSurfaceElement;
                     if (res != null)
                     {
                        foreach (SectionDataBase sd in elementData.ListSectionData)
                        {
                           SurfaceSection sec = sd as SurfaceSection;
                           if (sec != null)
                              res.ValuesInPointsData.AddRange(sec.GetCalcResultsInPt().DataRaw);
                        }
                        res.ClearStructuralLayers();
                        if (elementData is FloorElement)
                        {
                           FloorElement elem = elementData as FloorElement;
                           res.MultiLayer = (elem.Info.Slabs.AsElement.Layers.Count() != 1);
                           res.AddStructuralLayer(elem.Info.Material.Properties.Name, elem.Info.Slabs.AsElement.StructuralLayer().Thickness);
                        }
                        if (elementData is WallElement)
                        {
                           WallElement elem = elementData as WallElement;
                           res.MultiLayer = (elem.Info.Walls.AsElement.Layers.Count() != 1);
                           res.AddStructuralLayer(elem.Info.Material.Properties.Name, elem.Info.Walls.AsElement.StructuralLayer().Thickness);
                        }
                     }
                     UnitType xUnitType = ResultTypeLinear.X.GetUnitType();
                     DisplayUnitType xInternalDisplayUnitType = UnitsConverter.GetInternalUnit(xUnitType);
                     int itemOnDirection = elementData.ListSectionData.Count();

                     int errorIndexX = elementData.ListSectionData.FindIndex(ss => (ss as SurfaceSection).DesignError[DimensioningDirection.X].Count() > 0);
                     int errorIndexY = elementData.ListSectionData.FindIndex(ss => (ss as SurfaceSection).DesignError[DimensioningDirection.Y].Count() > 0);
                     if (errorIndexX >= 0 || errorIndexY >= 0)
                     {
                        //// add errors
                        if (errorIndexX >= 0)
                        {
                           IEnumerable<IGrouping<String, Tuple<String, XYZ>>> errosInSecX = elementData.ListSectionData.
                              SelectMany(lsd => (lsd as SurfaceSection).DesignError[DimensioningDirection.X].Select(dw => new Tuple<String, XYZ>(dw, (lsd.CalcPoint as CalcPointSurface).Coord))).
                              GroupBy(s => s.Item1);
                           getResultStatusMessageSurface(errosInSecX, xInternalDisplayUnitType, DimensioningDirection.X,elementData.Category, false).ForEach(s => elementData.AddFormatedError(s));

                        }
                        if (errorIndexY >= 0)
                        {
                           IEnumerable<IGrouping<String, Tuple<String, XYZ>>> errosInSecY = elementData.ListSectionData.
                              SelectMany(lsd => (lsd as SurfaceSection).DesignError[DimensioningDirection.Y].Select(dw => new Tuple<String, XYZ>(dw, (lsd.CalcPoint as CalcPointSurface).Coord))).
                              GroupBy(s => s.Item1);
                           getResultStatusMessageSurface(errosInSecY, xInternalDisplayUnitType, DimensioningDirection.Y, elementData.Category, false).ForEach(s => elementData.AddFormatedError(s));
                        }
                     }
                     else
                     {
                        int warningX = elementData.ListSectionData.FindIndex(ss => (ss as SurfaceSection).DesignWarning[DimensioningDirection.X].Count() > 0);
                        int warningY = elementData.ListSectionData.FindIndex(ss => (ss as SurfaceSection).DesignWarning[DimensioningDirection.Y].Count() > 0);
                        if (warningX >= 0 || warningY >= 0)
                        {
                           //// add warnings
                           if (warningX >= 0)
                           {
                              IEnumerable<IGrouping<String, Tuple<String, XYZ>>> warningInSecX = elementData.ListSectionData.
                                 SelectMany(lsd => (lsd as SurfaceSection).DesignWarning[DimensioningDirection.X].Select(dw => new Tuple<String, XYZ>(dw, (lsd.CalcPoint as CalcPointSurface).Coord))).
                                 GroupBy(s => s.Item1);
                              getResultStatusMessageSurface(warningInSecX, xInternalDisplayUnitType, DimensioningDirection.X, elementData.Category, true, itemOnDirection).ForEach(s => elementData.AddFormatedWarning(s));
                           }
                           if (warningX >= 0)
                           {
                              IEnumerable<IGrouping<String, Tuple<String, XYZ>>> warningInSecY = elementData.ListSectionData.
                                 SelectMany(lsd => (lsd as SurfaceSection).DesignWarning[DimensioningDirection.Y].Select(dw => new Tuple<String, XYZ>(dw, (lsd.CalcPoint as CalcPointSurface).Coord))).
                                 GroupBy(s => s.Item1);
                              getResultStatusMessageSurface(warningInSecY, xInternalDisplayUnitType, DimensioningDirection.Y, elementData.Category, true, itemOnDirection).ForEach(s => elementData.AddFormatedWarning(s));
                           }
                        }
                        int infoX = elementData.ListSectionData.FindIndex(ss => (ss as SurfaceSection).DesignInfo[DimensioningDirection.X].Count() > 0);
                        int infoY = elementData.ListSectionData.FindIndex(ss => (ss as SurfaceSection).DesignInfo[DimensioningDirection.Y].Count() > 0);
                        if (infoX >= 0 || infoY >= 0)
                        {
                           //// add infos
                           if (infoX >= 0)
                           {
                              IEnumerable<IGrouping<String, Tuple<String, XYZ>>> infoInSecX = elementData.ListSectionData.
                                 SelectMany(lsd => (lsd as SurfaceSection).DesignInfo[DimensioningDirection.X].Select(dw => new Tuple<String, XYZ>(dw, (lsd.CalcPoint as CalcPointSurface).Coord))).
                                 GroupBy(s => s.Item1);
                              getResultStatusMessageSurface(infoInSecX, xInternalDisplayUnitType, DimensioningDirection.X, elementData.Category, true, itemOnDirection).ForEach(s => elementData.AddFormatedInfo(s));
                           }
                           if (infoY >= 0)
                           {
                              IEnumerable<IGrouping<String, Tuple<String, XYZ>>> infoInSecY = elementData.ListSectionData.
                                 SelectMany(lsd => (lsd as SurfaceSection).DesignInfo[DimensioningDirection.Y].Select(dw => new Tuple<String, XYZ>(dw, (lsd.CalcPoint as CalcPointSurface).Coord))).
                                 GroupBy(s => s.Item1);
                              getResultStatusMessageSurface(infoInSecY, xInternalDisplayUnitType, DimensioningDirection.Y, elementData.Category, true, itemOnDirection).ForEach(s => elementData.AddFormatedInfo(s));
                           }
                        }
                     } 
                     if (Autodesk.Revit.DB.CodeChecking.Storage.Status.Undefined == elementData.Status.Status)
                        elementData.Status.Status = Autodesk.Revit.DB.CodeChecking.Storage.Status.Succeed;
                  }
                  break;
                  /// </structural_toolkit_2015>

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

      /// <summary>
      /// Creats list of status messages for linear setions cref="LinearSection".
      /// </summary>
      /// <param name="messages">List of messages</param>
      /// <param name="xInternalDisplayUnitType">Type of display unit  as cref="DisplayUnitType"</param>
      /// <param name="detailSecInfo">Messages with information about section position.</param>
      /// <param name="sectionCount">Number of sections.</param>
      /// <returns>List of status messages.for linear element</returns>
      List<ResultStatusMessage> getResultStatusMessageLinear(IEnumerable<IGrouping<String, Tuple<String, Double>>> messages, DisplayUnitType xInternalDisplayUnitType, bool detailSecInfo = true, int sectionCount = 0)
      {
         List<ResultStatusMessage> resultStatusMessages = new List<ResultStatusMessage>();
         foreach (IGrouping<String, Tuple<String, Double>> message in messages)
         {
            ResultStatusMessage msg = new ResultStatusMessage();
            msg.Message.Add(new ResultStatusMessageItem(message.Key));
            if (detailSecInfo)
            {
               msg.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("AppliesToSections")));
               if (sectionCount != message.Count())
               {
                  foreach (Tuple<String, Double> s in message)
                  {
                     msg.Message.Add(new ResultStatusMessageItem(UnitType.UT_Length, xInternalDisplayUnitType, (double)s.Item2));
                     msg.Message.Add(new ResultStatusMessageItem("; "));
                  }
               }
               else
               {
                  msg.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("AllSections")));
               }
            }
            resultStatusMessages.Add(msg);
         }
         return resultStatusMessages;
      }

      /// <structural_toolkit_2015>

      /// <summary>
      /// Creats list of status messages for surface setions cref="SurfaceSection".
      /// </summary>
      /// <param name="messages">List of messages</param>
      /// <param name="xInternalDisplayUnitType">Type of display unit  as cref="DisplayUnitType"</param>
      /// <param name="direction">Direction of calculation as cref="DimensioningDirection"</param>
      /// <param name="elementType">Type of elemets as cref="Autodesk.Revit.DB.BuiltInCategory"</param>
      /// <param name="detailSecInfo">Messages with information about section position.</param>
      /// <param name="sectionCount">Number of sections.</param>
      /// <returns>List of status messages for surface element</returns>
      List<ResultStatusMessage> getResultStatusMessageSurface(IEnumerable<IGrouping<String, Tuple<String, XYZ>>> messages, DisplayUnitType xInternalDisplayUnitType, DimensioningDirection direction, Autodesk.Revit.DB.BuiltInCategory elementType, bool detailSecInfo = true, int sectionCount = 0)
      {
         List<ResultStatusMessage> resultStatusMessages = new List<ResultStatusMessage>();
         if (messages.Count() > 0)
         {
            ResultStatusMessage msgDirection = new ResultStatusMessage();
            switch (elementType)
            {
               case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                  if (direction == DimensioningDirection.X)
                     msgDirection.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("DimensioningDirectionXWall") + ". "));
                  else
                     msgDirection.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("DimensioningDirectionYWall") + ". "));
                  break;
               case BuiltInCategory.OST_FloorAnalytical:
               case BuiltInCategory.OST_FoundationSlabAnalytical:
                  if (direction == DimensioningDirection.X)
                     msgDirection.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("DimensioningDirectionXFloor") + ". "));
                  else
                     msgDirection.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("DimensioningDirectionYFloor") + ". "));
                  break;
               default:
                  break;
            }
            resultStatusMessages.Add(msgDirection);
            foreach (IGrouping<String, Tuple<String, XYZ>> message in messages)
            {
               ResultStatusMessage msg = new ResultStatusMessage();
               msg.Message.Add(new ResultStatusMessageItem(message.Key));
               if (detailSecInfo)
               {
                  msg.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("AppliesToSections")));
                  if (sectionCount != message.Count())
                  {
                     foreach (Tuple<String, XYZ> s in message)
                     {
                        msg.Message.Add(new ResultStatusMessageItem("x="));
                        msg.Message.Add(new ResultStatusMessageItem(UnitType.UT_Length, xInternalDisplayUnitType, (double)s.Item2.X));
                        msg.Message.Add(new ResultStatusMessageItem("/y="));
                        msg.Message.Add(new ResultStatusMessageItem(UnitType.UT_Length, xInternalDisplayUnitType, (double)s.Item2.Y));
                        msg.Message.Add(new ResultStatusMessageItem("; "));
                     }
                  }
                  else
                  {
                     msg.Message.Add(new ResultStatusMessageItem(" " + Resources.ResourceManager.GetString("AllSections") + "."));
                  }
               }
               resultStatusMessages.Add(msg);
            }
         }
         return resultStatusMessages;
      }
      /// </structural_toolkit_2015>
      
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
               /// <structural_toolkit_2015>
               case Autodesk.Revit.DB.BuiltInCategory.OST_FloorAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_FoundationSlabAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                  {
                     isOk = true;
                  }
                  break;
               /// </structural_toolkit_2015>
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
      /// <structural_toolkit_2015>

      /// <summary>
      /// Example explaining how to use force modification for beam:
      /// additonal minimum bending moment over support M=0.1*Mmax in ULS 
      /// </summary>
      /// <param name="obj">cref="BeamElement" object or cref="ColumnElement" object.</param>
      private void ModifySupportForces(ObjectDataBase obj)
      {
         BeamElement elem = obj as BeamElement;
         if (elem != null && elem.ListSectionData.Count() > 0)
         {
            double maximumM = elem.ListSectionData.Max(sd => ((sd as BeamSection).ListInternalForces).Max(f => (f as InternalForcesLinear).Forces.MomentMy));
            bool isMaximumM = (maximumM > 0.0);
            if (isMaximumM)
            {
               List<int> indexSupport = new List<int>();
               double maximumL = elem.ListSectionData.Max(sd => ((sd as BeamSection).CalcPoint as CalcPointLinear).CoordRelative);
               indexSupport.Add(elem.ListSectionData.FindIndex(sd => ((sd as BeamSection).CalcPoint as CalcPointLinear).CoordRelative.Equals(maximumL)));
               double minimumL = elem.ListSectionData.Min(sd => ((sd as BeamSection).CalcPoint as CalcPointLinear).CoordRelative);
               indexSupport.Add(elem.ListSectionData.FindIndex(sd => ((sd as BeamSection).CalcPoint as CalcPointLinear).CoordRelative.Equals(minimumL)));
               foreach (int i in indexSupport)
               {
                  if (i > -1)
                  {
                     BeamSection bs = elem.ListSectionData[i] as BeamSection;
                     if (bs != null)
                     {
                        if (bs.ListInternalForces != null)
                        {
                           IEnumerable<InternalForcesBase> ULSForces = bs.ListInternalForces.Where(f => (f as InternalForcesLinear).Forces.LimitState == ForceLimitState.Uls);
                           if (ULSForces.Count() > 0)
                           {
                              double fVMax = ULSForces.Max(fUls => Math.Abs((fUls as InternalForcesLinear).Forces.ForceFz));
                              if (CalculationUtility.IsZeroN(fVMax))
                                 continue;
                              double Mmin = ULSForces.Min(fUls => (fUls as InternalForcesLinear).Forces.MomentMy);
                              double MAdd = -0.1 * maximumM;
                              if (Mmin > MAdd)
                              {
                                 double Mmax = Mmin;
                                 if (bs.ListInternalForces.Count != 1)
                                    Mmax = ULSForces.Max(fUls => (fUls as InternalForcesLinear).Forces.MomentMy);
                                 InternalForcesLinear forces = bs.ListInternalForces.Find(f => (f as InternalForcesLinear).Forces.MomentMy.Equals(Mmax)) as InternalForcesLinear;
                                 forces.Forces.MomentMy = MAdd;
                                 bs.ListInternalForces.Add(forces);
                              }
                           }
                        }
                     }
                  }
               }
            }
         }
      }

      /// </structural_toolkit_2015>
  
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

                           forces.ForceFx      = vFx.Count == 0 ? 0.0 : vFx[i];
                           forces.ForceFy      = vFy.Count == 0 ? 0.0 : vFy[i];
                           forces.ForceFz      = vFz.Count == 0 ? 0.0 : vFz[i];
                           forces.MomentMx     = vMx.Count == 0 ? 0.0 : vMx[i];
                           forces.MomentMy     = vMy.Count == 0 ? 0.0 : vMy[i];
                           forces.MomentMz     = vMz.Count == 0 ? 0.0 : vMz[i];
                           forces.DeflectionUx = vUx.Count == 0 ? 0.0 : vUx[i];
                           forces.DeflectionUy = vUy.Count == 0 ? 0.0 : vUy[i];
                           forces.DeflectionUz = vUz.Count == 0 ? 0.0 : vUz[i];

                         linForces.Forces = forces;

                         section.ListInternalForces.Add(linForces);
                      }
                   }
                }
                break;
            /// <structural_toolkit_2015>
             case Autodesk.Revit.DB.BuiltInCategory.OST_FloorAnalytical:
             case Autodesk.Revit.DB.BuiltInCategory.OST_FoundationSlabAnalytical:
                if (sectionData is FloorSection)
                {
                   FloorSection section = sectionData as FloorSection;
                   if (section.Info != null)
                   {
                      section.Width = 1.0;
                      section.Height = section.Info.Slabs.AsElement.StructuralLayer().Thickness;

                      section.Geometry.Add(-0.5*section.Width, -0.5*section.Height);
                      section.Geometry.Add( 0.5*section.Width, -0.5*section.Height);
                      section.Geometry.Add( 0.5*section.Width,  0.5*section.Height);
                      section.Geometry.Add(-0.5*section.Width,  0.5*section.Height);
                   }
                   if (cache != null)
                   {
                      List<double> vFxx = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Fxx);
                      List<double> vFyy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Fyy);
                      List<double> vFxy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Fxy);
                      List<double> vMxx = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Mxx);
                      List<double> vMyy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Myy);
                      List<double> vMxy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Mxy);
                      List<double> vQxx = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Qxx);
                      List<double> vQyy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Qyy);

                      int i = -1;
                      foreach (ForceLoadCaseDescriptor cas in cache.GetLoadCaseDescriptors())
                      {
                         i++;
                         InternalForcesSurface forces = new InternalForcesSurface();

                         forces.ForceDescription = cas.Name;
                         forces.LimitState = cas.State;

                         forces.ForceFxx = i < vFxx.Count ? vFxx[i] : 0.0;
                         forces.ForceFyy = i < vFyy.Count ? vFyy[i] : 0.0;
                         forces.ForceFxy = i < vFxy.Count ? vFxy[i] : 0.0;
                         forces.MomentMxx = i < vMxx.Count ? vMxx[i] : 0.0;
                         forces.MomentMyy = i < vMyy.Count ? vMyy[i] : 0.0;
                         forces.MomentMxy = i < vMxy.Count ? vMxy[i] : 0.0;
                         forces.ForceQxx = i < vQxx.Count ? vQxx[i] : 0.0;
                         forces.ForceQyy = i < vQyy.Count ? vQyy[i] : 0.0;

                         section.ListInternalForces.Add(forces);
                      }
                   }
                }
                break;
             case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                if (sectionData is WallSection)
                {
                   WallSection section = sectionData as WallSection;
                   if (section.Info != null)
                   {
                      section.Width = 1.0;
                      section.Height = section.Info.Walls.AsElement.StructuralLayer().Thickness;

                      section.Geometry.Add(-0.5 * section.Width, -0.5 * section.Height);
                      section.Geometry.Add(0.5 * section.Width, -0.5 * section.Height);
                      section.Geometry.Add(0.5 * section.Width, 0.5 * section.Height);
                      section.Geometry.Add(-0.5 * section.Width, 0.5 * section.Height);
                   }
                   if (cache != null)
                   {
                      List<double> vFxx = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Fxx);
                      List<double> vFyy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Fyy);
                      List<double> vFxy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Fxy);
                      // here we are reinterprating direction vectors for bending moments
                      List<double> vMxx = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Myy);
                      List<double> vMyy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Mxx);
                      List<double> vMxy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Mxy);
                      List<double> vQxx = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Qxx);
                      List<double> vQyy = cache.GetForceForPoint(sectionData.ElementId, cache.GetLoadCaseDescriptors(), sectionData.CalcPoint, ForceType.Qyy);

                      int i = -1;
                      foreach (ForceLoadCaseDescriptor cas in cache.GetLoadCaseDescriptors())
                      {
                         i++;
                         InternalForcesSurface forces = new InternalForcesSurface();

                         forces.ForceDescription = cas.Name;
                         forces.LimitState = cas.State;

                         forces.ForceFxx = i < vFxx.Count ? vFxx[i] : 0.0;
                         forces.ForceFyy = i < vFyy.Count ? vFyy[i] : 0.0;
                         forces.ForceFxy = i < vFxy.Count ? vFxy[i] : 0.0;
                         forces.MomentMxx = i < vMxx.Count ? vMxx[i] : 0.0;
                         forces.MomentMyy = i < vMyy.Count ? vMyy[i] : 0.0;
                         forces.MomentMxy = i < vMxy.Count ? vMxy[i] : 0.0;
                         forces.ForceQxx = i < vQxx.Count ? vQxx[i] : 0.0;
                         forces.ForceQyy = i < vQyy.Count ? vQyy[i] : 0.0;

                         section.ListInternalForces.Add(forces);
                      }
                   }
                }
                break;
               /// </structural_toolkit_2015>
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

                        if (section is BeamSection)
                           if ((section as BeamSection).IsTSection)
                              design.Type = SectionShapeType.T;

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
               /// <structural_toolkit_2015>
               case Autodesk.Revit.DB.BuiltInCategory.OST_FloorAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_FoundationSlabAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                  if (sectionData is SurfaceSection)
                  {
                     CodeCheckingConcreteExample.LabelFloor labelFloor = sectionData.Label as CodeCheckingConcreteExample.LabelFloor;
                     CodeCheckingConcreteExample.LabelWall labelWall = sectionData.Label as CodeCheckingConcreteExample.LabelWall;

                     SurfaceSection section = sectionData as SurfaceSection;
                     double[] coverT = { 0, 0 };
                     double[] coverB = { 0, 0 };
                     if (section.Info != null)
                     {
                        if (section.Info.RebarCovers != null)
                        {
                           if (section is FloorSection)
                           {
                              if (section.Info.RebarCovers.Exist(BuiltInParameter.CLEAR_COVER_BOTTOM))
                                 coverB[0] = section.Info.RebarCovers.GetDistance(BuiltInParameter.CLEAR_COVER_BOTTOM);
                              if (section.Info.RebarCovers.Exist(BuiltInParameter.CLEAR_COVER_TOP))
                                 coverT[0] = section.Info.RebarCovers.GetDistance(BuiltInParameter.CLEAR_COVER_TOP);
                           }
                           if (section is WallSection)
                           {
                              if (section.Info.RebarCovers.Exist(BuiltInParameter.CLEAR_COVER_INTERIOR)) 
                                 coverB[0] = section.Info.RebarCovers.GetDistance(BuiltInParameter.CLEAR_COVER_INTERIOR);
                              if (section.Info.RebarCovers.Exist(BuiltInParameter.CLEAR_COVER_EXTERIOR))
                                 coverT[0] = section.Info.RebarCovers.GetDistance(BuiltInParameter.CLEAR_COVER_EXTERIOR);
                           }
                           coverT[1] = coverT[0];
                           coverB[1] = coverB[0];
                        }
                        if( labelFloor!=null)
                        { 
                           coverT[1] += labelFloor.PrimaryReinforcement.BarDiameter;
                           coverB[1] += labelFloor.PrimaryReinforcement.BarDiameter;
                           design.LongitudinalCalculationType = labelFloor.LongitudinalCalculationType;
                           design.CreepCoefficient = labelFloor.CreepCoefficient;
                        } 
                        if( labelWall!=null)
                        { 
                           coverT[1] += labelWall.VerticalReinforcement.BarDiameter;
                           coverB[1] += labelWall.VerticalReinforcement.BarDiameter;
                           design.LongitudinalCalculationType = labelWall.LongitudinalCalculationType;
                           design.CreepCoefficient = labelWall.CreepCoefficient;
                        }

                        design.Type = SectionShapeType.RectangularBar;
                        design.YoungModulus = section.Info.Material.Characteristics.YoungModulus.X;
                        if (section.Info.Material.Characteristics.YoungModulus.X < 1)
                           design.YoungModulus = 20e9;
                        MaterialConcreteCharacteristics concrete = (MaterialConcreteCharacteristics)section.Info.Material.Characteristics.Specific;
                        if (concrete != null)
                           design.Compression = concrete.Compression;
                        else
                           design.Compression = 25e6;
                     }
                     design.Geometry = section.Geometry;
                     design.Width = 1.0;
                     design.Height = section.Height;
                     DimensioningDirection[] directions = new DimensioningDirection[] { DimensioningDirection.X, DimensioningDirection.Y };
                     foreach (DimensioningDirection direction in directions)
                     {
                        if (direction == DimensioningDirection.Y)
                        {
                           if ((section is FloorSection) && section.Info.Slabs.AsElement.IsOneWay)
                           {
                              break;
                           }
                           else
                           {
                              if( labelFloor != null ) 
                              {
                                 design.LongitudinalReinforcementMinimumYieldStress = labelFloor.SecondaryReinforcement.MinimumYieldStress;
                                 design.LongitudinalReinforcementArea =  labelFloor.SecondaryReinforcement.Area;
                                 design.LongitudinalReinforcementDiameter = labelFloor.SecondaryReinforcement.BarDiameter;
                              }
                              if( labelWall != null ) 
                              {
                                 design.LongitudinalReinforcementMinimumYieldStress = labelWall.HorizontalReinforcement.MinimumYieldStress;
                                 design.LongitudinalReinforcementArea = labelWall.HorizontalReinforcement.Area;
                                 design.LongitudinalReinforcementDiameter = labelWall.HorizontalReinforcement.BarDiameter;
                              }
                              design.CoverBottom = coverB[1];
                              design.CoverTop = coverT[1];
                           }
                        }
                        else
                        {
                           if( labelFloor!=null ) 
                           {
                              design.LongitudinalReinforcementMinimumYieldStress = labelFloor.PrimaryReinforcement.MinimumYieldStress;
                              design.LongitudinalReinforcementArea               = labelFloor.PrimaryReinforcement.Area;
                              design.LongitudinalReinforcementDiameter           = labelFloor.PrimaryReinforcement.BarDiameter;
                           }
                           if( labelWall!=null)
                           {
                              design.LongitudinalReinforcementMinimumYieldStress = labelWall.VerticalReinforcement.MinimumYieldStress;
                              design.LongitudinalReinforcementArea               = labelWall.VerticalReinforcement.Area;
                              design.LongitudinalReinforcementDiameter           = labelWall.VerticalReinforcement.BarDiameter;
                           }
                           design.CoverBottom = coverB[0];
                           design.CoverTop = coverT[0];
                        }
                        design.DimensioningDirection = direction;
                        design.ListInternalForces = section.ListInternalForces.Select(s => (s as InternalForcesSurface).Forces(direction)).ToList();
                        //
                        // calculation
                        //
                        design.Calculate();
                        //
                        // save result
                        //
                        section.AsBottom[direction] = design.AsBottom;
                        section.AsTop[direction] = design.AsTop;
                        section.MinStiffnes[direction] = design.MinStiffness;
                        section.DesignInfo[direction] = design.DesignInfo;
                        section.DesignWarning[direction] = design.DesignWarning;
                        section.DesignError[direction] = design.DesignError;
                        if (section.DesignError[direction].Count > 0)
                        {
                           ret = false;
                        }
                     }
                  }
                  break;
                  /// </structural_toolkit_2015>
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

}
