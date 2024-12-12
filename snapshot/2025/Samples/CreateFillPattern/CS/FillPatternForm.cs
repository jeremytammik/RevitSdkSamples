//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
using System.Data;
using System.Linq;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.CreateFillPattern.CS
{
   /// <summary>
   /// 
   /// </summary>
   public partial class PatternForm : System.Windows.Forms.Form
   {
      /// <summary>
      /// Current UI document
      /// </summary>
      UIDocument docUI;
      /// <summary>
      /// Current revit document
      /// </summary>
      Document doc;

      /// <summary>
      /// Constructor
      /// </summary>
      public PatternForm(ExternalCommandData commandData)
      {
         docUI = commandData.Application.ActiveUIDocument;
         doc = commandData.Application.ActiveUIDocument.Document;
         InitializeComponent();

         IniTreeView();
      }

      /// <summary>
      /// Select all elements in current document with specified element type filter
      /// </summary>
      /// <typeparam name="T">Target element type</typeparam>
      /// <returns>List of the elements</returns>
      private List<T> GetAllElements<T>()
      {
         ElementClassFilter elementFilter = new ElementClassFilter(typeof(T));
         FilteredElementCollector collector = new FilteredElementCollector(doc);
         collector = collector.WherePasses(elementFilter);
         return collector.Cast<T>().ToList();
      }

      /// <summary>
      /// Close the window after close button is clicked
      /// </summary>
      /// <param name="sender">This object</param>
      /// <param name="e">A object contains the event data</param>
      private void buttonCancel_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      /// <summary>
      /// Initial nodes of treeviews
      /// </summary>
      private void IniTreeView()
      {
         this.treeViewLinePattern.Nodes.Clear();
         TreeNode iniNode2 = new TreeNode("LinePatterns");
         treeViewLinePattern.Nodes.Add(iniNode2);

         List<LinePatternElement> lstLinePatterns = GetAllElements<LinePatternElement>();
         for (int i = 0; i < lstLinePatterns.Count; i++)
         {
            TreeNode node = new TreeNode(lstLinePatterns[i].Name);
            node.Name = lstLinePatterns[i].Id.ToString();
            iniNode2.Nodes.Add(node);
         }

         this.treeViewFillPattern.Nodes.Clear();
         TreeNode iniNode1 = new TreeNode("FillPatterns");
         treeViewFillPattern.Nodes.Add(iniNode1);

         List<FillPatternElement> lstFillPatterns = GetAllElements<FillPatternElement>();
         for (int i = 0; i < lstFillPatterns.Count; i++)
         {
            TreeNode node = new TreeNode(lstFillPatterns[i].Name);
            node.Name = i.ToString();
            iniNode1.Nodes.Add(node);
         }
      }

      /// <summary>
      /// Create a fillpattern element
      /// </summary>
      /// <param name="patternName">The fillpattern name</param>
      /// <returns>Created fillpattern element</returns>
      private FillPatternElement GetOrCreateFacePattern(string patternName)
      {
         FillPatternTarget target = FillPatternTarget.Model;
         FillPatternElement fillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, target, patternName);

         if (fillPatternElement == null)
         {
            //Create a fillpattern with specified angle and spacing
            FillPattern fillPattern = new FillPattern(patternName, target,
                FillPatternHostOrientation.ToView, 0.5, 0.5, 0.5);

            Transaction trans = new Transaction(doc);
            trans.Start("Create a fillpattern element");
            fillPatternElement = FillPatternElement.Create(doc, fillPattern);
            trans.Commit();
         }
         return fillPatternElement;
      }

      /// <summary>
      /// Create a complex fillpattern element
      /// </summary>
      /// <param name="patternName">The fillpattern name</param>
      /// <returns>Created fillpattern element</returns>
      private FillPatternElement GetOrCreateComplexFacePattern(string patternName)
      {
         FillPatternTarget target = FillPatternTarget.Model;
         FillPatternElement fillPatternElement = FillPatternElement.GetFillPatternElementByName(doc, target, patternName);

         if (fillPatternElement == null)
         {
            // Create the fill pattern
            FillPattern fillPattern = new FillPattern(patternName, target,
                                                      FillPatternHostOrientation.ToHost);

            // Add grids
            List<FillGrid> grids = new List<FillGrid>();

            //Horizontal lines.  
            grids.Add(CreateGrid(new UV(0, 0.1), 0.5, 0, 0.55, 1.0, 0.1));
            grids.Add(CreateGrid(new UV(0, 0.5), 0.5, 0, 0.55, 1.0, 0.1));

            // Vertical lines.  
            grids.Add(CreateGrid(new UV(0, 0.1), 0.55, Math.PI / 2, 0.5, 0.4, 0.6));
            grids.Add(CreateGrid(new UV(1.0, 0.1), 0.55, Math.PI / 2, 0.5, 0.4, 0.6));

            fillPattern.SetFillGrids(grids);

            // Create the fill pattern element. Now document is modified; transaction is needed
            Transaction t = new Transaction(doc, "Create fill pattern");
            t.Start();
            fillPatternElement = FillPatternElement.Create(doc, fillPattern);

            t.Commit();
         }

         return fillPatternElement;
      }

      /// <summary>
      /// Create a fillgrid
      /// </summary>
      /// <param name="origin"></param>
      /// <param name="offset"></param>
      /// <param name="angle"></param>
      /// <param name="shift"></param>
      /// <param name="segments"></param>
      /// <returns></returns>
      private FillGrid CreateGrid(UV origin, double offset, double angle,
                                  double shift, params double[] segments)
      {
         FillGrid fillGrid = new FillGrid();
         // The arguments: origin, offset (vertical distance between lines), 
         // angle, shift (delta between location of start point per line)
         // The last two arguments are the segments: e.g. 1.0 units on, 
         // 0.1 units off (units are Revit units (ft))
         fillGrid.Origin = origin;
         fillGrid.Offset = offset;
         fillGrid.Angle = angle;
         fillGrid.Shift = shift;
         List<double> segmentsList = new List<double>();
         foreach (double d in segments)
         {
            segmentsList.Add(d);
         }
         fillGrid.SetSegments(segmentsList);

         return fillGrid;
      }


      /// <summary>
      /// Create a linepattern element
      /// </summary>
      /// <param name="patternName">The linepattern name</param>
      /// <returns>Created linepattern element</returns>
      private LinePatternElement CreateLinePatternElement(string patternName)
      {
         //Create list of segments which define the line pattern
         List<LinePatternSegment> lstSegments = new List<LinePatternSegment>();
         lstSegments.Add(new LinePatternSegment(LinePatternSegmentType.Dot, 0.0));
         lstSegments.Add(new LinePatternSegment(LinePatternSegmentType.Space, 0.02));
         lstSegments.Add(new LinePatternSegment(LinePatternSegmentType.Dash, 0.03));
         lstSegments.Add(new LinePatternSegment(LinePatternSegmentType.Space, 0.02));

         LinePattern linePattern = new LinePattern(patternName);
         linePattern.SetSegments(lstSegments);

         Transaction trans = new Transaction(doc);
         trans.Start("Create a linepattern element");
         LinePatternElement linePatternElement = LinePatternElement.Create(doc, linePattern);
         trans.Commit();
         return linePatternElement;
      }

      /// <summary>
      /// Create a fillpattern element and apply it to target material
      /// </summary>
      /// <param name="sender">This object</param>
      /// <param name="e">A object contains the event data</param>
      private void buttonCreateFillPattern_Click(object sender, EventArgs e)
      {
         Wall targetWall = GetSelectedWall();
         if (targetWall == null)
         {
            TaskDialog.Show("Create Fill Pattern",
                "Before applying a FillPattern to a wall's surfaces, you must first select a wall.");
            this.Close();
            return;
         }

         FillPatternElement mySurfacePattern = GetOrCreateFacePattern("MySurfacePattern");
         Material targetMaterial = doc.GetElement(targetWall.GetMaterialIds(false).First<ElementId>()) as Material;
         Transaction trans = new Transaction(doc);
         trans.Start("Apply fillpattern to surface");
         targetMaterial.SurfaceForegroundPatternId = mySurfacePattern.Id;
         trans.Commit();
         this.Close();
      }

      /// <summary>
      /// Create a linepattern element and apply it to grids
      /// </summary>
      /// <param name="sender">This object</param>
      /// <param name="e">A object contains the event data</param>
      private void buttonCreateLinePattern_Click(object sender, EventArgs e)
      {
         List<ElementId> lstGridTypeIds = new List<ElementId>();
         GetSelectedGridTypeIds(lstGridTypeIds);
         if (lstGridTypeIds.Count == 0)
         {
            TaskDialog.Show("Apply To Grids",
                "Before applying a LinePattern to Grids, you must first select at least one grid.");
            this.Close();
            return;
         }

         LinePatternElement myLinePatternElement = CreateLinePatternElement("MyLinePattern");
         foreach (ElementId typeId in lstGridTypeIds)
         {
            Element gridType = doc.GetElement(typeId);
            //set the parameter value of End Segment Pattern
            SetParameter("End Segment Pattern", myLinePatternElement.Id, gridType);
         }
         this.Close();
      }

      /// <summary>
      /// Set a parameter value of a target element
      /// </summary>
      /// <param name="paramName">The parameter name</param>
      /// <param name="eid">Id value of the parameter</param>
      /// <param name="elem">Target element</param>
      private void SetParameter(string paramName, ElementId eid, Element elem)
      {
         foreach (Parameter param in elem.Parameters)
         {
            if (param.Definition.Name == paramName)
            {
               Transaction trans = new Transaction(doc);
               trans.Start("Set parameter value");
               param.Set(eid);
               trans.Commit();
               break;
            }
         }
      }

      /// <summary>
      /// Apply fillpattern to surface
      /// </summary>
      /// <param name="sender">This object</param>
      /// <param name="e">A object contains the event data</param>
      private void buttonApplyToSurface_Click(object sender, EventArgs e)
      {
         Wall targetWall = GetSelectedWall();
         if (targetWall == null)
         {
            TaskDialog.Show("Apply To Surface",
                "Before applying a FillPattern to a wall's surfaces, you must first select a wall.");
            this.Close();
            return;
         }

         if (treeViewFillPattern.SelectedNode == null || treeViewFillPattern.SelectedNode.Parent == null)
         {
            TaskDialog.Show("Apply To Surface",
                "Before applying a FillPattern to a wall's surfaces, you must first select one FillPattern.");
            return;
         }

         List<FillPatternElement> lstPatterns = GetAllElements<FillPatternElement>();
         int patternIndex = int.Parse(treeViewFillPattern.SelectedNode.Name);
         Material targetMaterial = doc.GetElement(targetWall.GetMaterialIds(false).First<ElementId>()) as Material;
         Transaction trans = new Transaction(doc);
         trans.Start("Apply fillpattern to surface");
         targetMaterial.SurfaceForegroundPatternId = lstPatterns[patternIndex].Id;
         trans.Commit();

         this.Close();
      }

      /// <summary>
      /// Get a selected wall
      /// </summary>
      /// <returns>Selected wall</returns>
      private Wall GetSelectedWall()
      {
         Wall wall = null;
         foreach (ElementId elemId in docUI.Selection.GetElementIds())
         {
            Element elem = doc.GetElement(elemId);
            wall = elem as Wall;
            if (wall != null)
               return wall;
         }
         return wall;
      }

      /// <summary>
      /// Apply fillpatterns to cutting surface
      /// </summary>
      /// <param name="sender">This object</param>
      /// <param name="e">A object contains the event data</param>
      private void buttonApplyToCutSurface_Click(object sender, EventArgs e)
      {
         Wall targetWall = GetSelectedWall();
         if (targetWall == null)
         {
            TaskDialog.Show("Apply To CutSurface",
                "Before applying a FillPattern to a wall's cut surfaces, you must first select a wall.");
            this.Close();
            return;
         }

         if (treeViewFillPattern.SelectedNode == null || treeViewFillPattern.SelectedNode.Parent == null)
         {
            TaskDialog.Show("Apply To CutSurface",
                "Before applying a FillPattern to a wall's cutting surfaces, you must first select a FillPattern.");
            return;
         }

         List<FillPatternElement> lstPatterns = GetAllElements<FillPatternElement>();
         int patternIndex = int.Parse(treeViewFillPattern.SelectedNode.Name);
         Material targetMaterial = doc.GetElement(targetWall.GetMaterialIds(false).First<ElementId>()) as Material;

         Transaction trans = new Transaction(doc);
         trans.Start("Apply fillpattern to cutting surface");
         targetMaterial.CutForegroundPatternId = lstPatterns[patternIndex].Id;
         trans.Commit();

         this.Close();
      }

      /// <summary>
      /// Apply linepattern to grids
      /// </summary>
      /// <param name="sender">This object</param>
      /// <param name="e">A object contains the event data</param>
      private void buttonApplyToGrids_Click(object sender, EventArgs e)
      {
         List<ElementId> lstGridTypeIds = new List<ElementId>();
         GetSelectedGridTypeIds(lstGridTypeIds);
         if (lstGridTypeIds.Count == 0)
         {
            TaskDialog.Show("Apply To Grids",
                "Before applying a LinePattern to Grids, you must first select at least one grid.");
            this.Close();
            return;
         }

         if (treeViewLinePattern.SelectedNode == null || treeViewLinePattern.Parent == null)
         {
            TaskDialog.Show("Apply To Grids",
                "Before applying a LinePattern to Grids, you must first select a LinePattern.");
            return;
         }
         ElementId eid = ElementId.Parse(treeViewLinePattern.SelectedNode.Name);
         foreach (ElementId typeId in lstGridTypeIds)
         {
            Element gridType = doc.GetElement(typeId);
            //set the parameter value of End Segment Pattern
            SetParameter("End Segment Pattern", eid, gridType);
         }
         this.Close();
      }

      /// <summary>
      /// Get selected GridType Ids
      /// </summary>
      /// <param name="lstGridTypeIds">Selected GridType Ids</param>
      private void GetSelectedGridTypeIds(List<ElementId> lstGridTypeIds)
      {
         foreach (ElementId elemId in docUI.Selection.GetElementIds())
         {
            Element elem = doc.GetElement(elemId);
            Grid grid = elem as Grid;
            if (grid != null)
            {
               ElementId gridTypeId = grid.GetTypeId();
               if (!lstGridTypeIds.Contains(gridTypeId))
                  lstGridTypeIds.Add(gridTypeId);
            }
         }
      }

      /// <summary>
      /// To create a brick-like patternss
      /// </summary>
      /// <param name="sender">This object</param>
      /// <param name="e">A object contains the event data</param>
      private void buttonCreateComplexFillPattern_Click(object sender, EventArgs e)
      {
         Wall targetWall = GetSelectedWall();
         if (targetWall == null)
         {
            TaskDialog.Show("Create Fill Pattern",
                "Before applying a FillPattern to a wall's surfaces, you must first select a wall.");
            this.Close();
            return;
         }

         FillPatternElement mySurfacePattern = GetOrCreateComplexFacePattern("MyComplexPattern");
         Material targetMaterial = doc.GetElement(targetWall.GetMaterialIds(false).First<ElementId>()) as Material;
         Transaction trans = new Transaction(doc);
         trans.Start("Apply complex fillpattern to surface");
         targetMaterial.SurfaceForegroundPatternId = mySurfacePattern.Id;
         trans.Commit();
         this.Close();
      }

      private void PatternForm_Load(object sender, EventArgs e)
      {

      }
   }
}
