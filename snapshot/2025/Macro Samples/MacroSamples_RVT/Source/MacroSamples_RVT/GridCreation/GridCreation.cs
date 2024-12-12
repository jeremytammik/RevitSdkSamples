//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;

using Application = Autodesk.Revit.ApplicationServices.Application;
using Element = Autodesk.Revit.DB.Element;

using SamplePropertis = MacroCSharpSamples.GridCreation.GridCreationProperties;

using MacroCSharpSamples;
using System.Diagnostics;
using MacroSamples_RVT;

namespace Revit.SDK.Samples.GridCreation.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   public class GridCreation
   {
      #region 
      ThisApplication? m_app;
      #endregion

      /// <summary>
      /// Default constructor without parameter is not allowed 
      /// </summary>
      private GridCreation()
      {
      }

      /// <summary>
      /// GridCreation init
      /// </summary>
      /// <param name="hostApp"></param>
      public GridCreation(ThisApplication App)
      {
         m_app = App;
      }

      /// <summary>
      /// Run this sample now
      /// </summary>
      public void Run()
      {
         try
         {
            Document? document = m_app?.ActiveUIDocument.Document;

            // Get all selected lines and arcs 
            CurveArray? selectedCurves = GetSelectedCurves(m_app);

            // Show UI
            GridCreationOptionData? gridCreationOption = new GridCreationOptionData(selectedCurves == null || selectedCurves.IsEmpty);
            using (GridCreationOptionForm gridCreationOptForm = new GridCreationOptionForm(gridCreationOption))
            {
               DialogResult result = gridCreationOptForm.ShowDialog();
               if (result == DialogResult.Cancel)
               {
                  return;
               }

               ArrayList labels = GetAllLabelsOfGrids(document);
               ForgeTypeId? dut = GetLengthUnitType(document);
               switch (gridCreationOption.CreateGridsMode)
               {
                  case CreateMode.Select: // Create grids with selected lines/arcs
                     CreateWithSelectedCurvesData data = new CreateWithSelectedCurvesData(m_app, selectedCurves, labels);
                     using (CreateWithSelectedCurvesForm createWithSelected = new CreateWithSelectedCurvesForm(data))
                     {
                        result = createWithSelected.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                           // Create grids
                           data.CreateGrids();
                        }
                     }
                     break;

                  case CreateMode.Orthogonal: // Create orthogonal grids
                     CreateOrthogonalGridsData orthogonalData = new CreateOrthogonalGridsData(m_app, dut, labels);
                     using (CreateOrthogonalGridsForm orthogonalGridForm = new CreateOrthogonalGridsForm(orthogonalData))
                     {
                        result = orthogonalGridForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                           // Create grids
                           orthogonalData.CreateGrids();
                        }
                     }
                     break;

                  case CreateMode.RadialAndArc: // Create radial and arc grids
                     CreateRadialAndArcGridsData radArcData = new CreateRadialAndArcGridsData(m_app, dut, labels);
                     using (CreateRadialAndArcGridsForm radArcForm = new CreateRadialAndArcGridsForm(radArcData))
                     {
                        result = radArcForm.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                           // Create grids
                           radArcData.CreateGrids();
                        }
                     }
                     break;
               }
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
         }
      }

      /// <summary>
      /// Get all selected lines and arcs
      /// </summary>
      /// <param name="document">Revit's document</param>
      /// <returns>CurveArray contains all selected lines and arcs</returns>
      private CurveArray? GetSelectedCurves(ThisApplication? document)
      {
         CurveArray? selectedCurves = m_app?.ActiveUIDocument.Document.Application.Create.NewCurveArray();
         ICollection<ElementId>? elements = document?.ActiveUIDocument.Selection.GetElementIds();
         if (elements == null)
         {
            return null;
         }
         foreach (Autodesk.Revit.DB.ElementId elementId in elements)
         {
            Element? element = document?.ActiveUIDocument.Document.GetElement(elementId);
            if ((element is ModelLine) || (element is ModelArc))
            {
               ModelCurve? modelCurve = element as ModelCurve;
               Curve? curve = modelCurve?.GeometryCurve;
               if (curve != null)
               {
                  selectedCurves?.Append(curve);
               }
            }
            else if ((element is DetailLine) || (element is DetailArc))
            {
               DetailCurve? detailCurve = element as DetailCurve;
               Curve? curve = detailCurve?.GeometryCurve;
               if (curve != null)
               {
                  selectedCurves?.Append(curve);
               }
            }
         }

         return selectedCurves;
      }

      /// <summary>
      /// Get all model and detail lines/arcs within selected elements
      /// </summary>
      /// <param name="document">Revit's document</param>
      /// <returns>ElementSet contains all model and detail lines/arcs within selected elements </returns>
      public static ICollection<ElementId> GetSelectedModelLinesAndArcs(ThisApplication thisDocument)
      {
         var tmpIds = new List<ElementId>();
         ICollection<ElementId> elements = thisDocument.ActiveUIDocument.Selection.GetElementIds();
         foreach (ElementId id in elements)
         {
            Element element = thisDocument.ActiveUIDocument.Document.GetElement(id);
            if ((element is ModelLine) || (element is ModelArc) || (element is DetailLine) || (element is DetailArc))
            {
               tmpIds.Add(element.Id);
            }
         }

         return tmpIds;
      }

      /// <summary>
      /// Get current length display unit type
      /// </summary>
      /// <param name="document">Revit's document</param>
      /// <returns>Current length display unit type</returns>
      private static ForgeTypeId? GetLengthUnitType(Document? document)
      {
         ForgeTypeId specTypeId = SpecTypeId.Length;
         Units? projectUnit = document?.GetUnits();
         try
         {
            Autodesk.Revit.DB.FormatOptions? formatOption = projectUnit?.GetFormatOptions(specTypeId);
            return formatOption?.GetUnitTypeId();
         }
         catch (System.Exception /*e*/)
         {
            return UnitTypeId.Feet;
         }
      }

      /// <summary>
      /// Get all grid labels in current document
      /// </summary>
      /// <param name="document">Revit's document</param>
      /// <returns>ArrayList contains all grid labels in current document</returns>
      private static ArrayList GetAllLabelsOfGrids(Document? document)
      {
         ArrayList labels = new ArrayList();

         ElementClassFilter gridFilter = new ElementClassFilter(typeof(Grid));
         FilteredElementCollector collector = new FilteredElementCollector(document);
         collector.WherePasses(gridFilter);
         FilteredElementIterator iter = collector.GetElementIterator();

         iter.Reset();
         while (iter.MoveNext())
         {
            Grid? grid = iter.Current as Grid;
            if (null != grid)
            {
               labels.Add(grid.Name);
            }
         }
         return labels;
      }
   }
}

