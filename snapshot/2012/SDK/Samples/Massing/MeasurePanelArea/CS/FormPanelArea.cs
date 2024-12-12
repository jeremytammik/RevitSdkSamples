//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Panel = Autodesk.Revit.DB.Panel;
using Element = Autodesk.Revit.DB.Element;
using Instance = Autodesk.Revit.DB.Instance;

namespace Revit.SDK.Samples.MeasurePanelArea.CS
{
   /// <summary>
   /// The window designed for interactive operations
   /// </summary>
   public partial class frmPanelArea : System.Windows.Forms.Form
   {
      /// <summary>
      /// The revit application instance
      /// </summary>
      Autodesk.Revit.UI.UIApplication m_uiApp;
      /// <summary>
      /// The active Revit document
      /// </summary>
      UIDocument m_uiDoc;
      /// <summary>
      /// record the panel type specified by the user.
      /// the panel with an area is greater than "m_maxValue" will be changed to this type
      /// </summary>
      string m_maxType = "";
      /// <summary>
      /// record the panel type specified by the user.
      /// the panel with an area in the range [m_minValue, m_maxValue] will be changed to this type
      /// </summary>
      string m_midType = "";
      /// <summary>
      /// record the panel type specified by the user.
      /// the panel with an area is smaller than "m_minValue" will be changed to this type
      /// </summary>
      string m_minType = "";
      /// <summary>
      /// Record the minimum value of the desired panel area
      /// </summary>
      double m_minValue = 0;
      /// <summary>
      /// Record the maximum value of the desired panel area
      /// </summary>
      double m_maxValue = 0;
      /// <summary>
      /// Record how many panels have an area larger than the maximum value
      /// </summary>
      int m_maxCounter = 0;
      /// <summary>
      /// Record how many panels have an area smaller than the minimum value
      /// </summary>
      int m_minCounter = 0;
      /// <summary>
      /// Record how many panels have an area in the range [m_minValue, m_maxValue] 
      /// </summary>
      int m_okCounter = 0;
      /// <summary>
      /// Store all the divided surface selected by user or store all the divided surface in the document if user selects nothing
      /// </summary>
      List<DividedSurface> m_dividedSurfaceList = new List<DividedSurface>();
      /// <summary>
      /// A stream used to record the panel's element id and area to a text file
      /// </summary>
      StreamWriter m_writeFile = null;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="commandData">
      /// An object that is passed to the external application 
      /// which contains data related to the command, 
      /// such as the application object and active view.</param>
      public frmPanelArea(ExternalCommandData commandData)
      {
         m_uiApp = commandData.Application;
         m_uiDoc = m_uiApp.ActiveUIDocument;

         InitializeComponent();
         BuildPanelTypeList(commandData);
      }

      /// <summary>
      /// Handle the event triggered when user clicks the "Compute" button:
      /// 1. compute all the panel areas;
      /// 2. compare the areas with the range, mark the panels with different types;
      /// 3. record the result to a text file
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnCompute_Click(object sender, EventArgs e)
      {
         m_minValue = Convert.ToDouble(txtMin.Text);
         m_maxValue = Convert.ToDouble(txtMax.Text);

         SetPanelTypesFromUI();

         string assemblyName = Assembly.GetExecutingAssembly().Location;
         string assemblyDirectory = Path.GetDirectoryName(assemblyName);
         m_writeFile = new StreamWriter(assemblyDirectory + @"\" + "_PanelArea.txt");
         m_writeFile.WriteLine("Panel Element ID : Area");

         GetDividedSurfaces();

         foreach (DividedSurface ds in m_dividedSurfaceList)
         {
            ExamineDividedSurface(ds);
         }
         m_writeFile.WriteLine(m_maxCounter + " panels larger than " + m_maxValue);
         m_writeFile.WriteLine(m_minCounter + " panels smaller than " + m_minValue);
         m_writeFile.WriteLine(m_okCounter + " panels within desired range");
         m_writeFile.Close();
         Close();
      }

      /// <summary>
      /// Get names of Panel families and populate drop-down lists in the UI
      /// </summary>
      private void BuildPanelTypeList(ExternalCommandData commandData)
      {
         List<FamilyInstance> list = GetElements<FamilyInstance>();
         if (list.Count == 0)
         {
            MessageBox.Show("There are no panel families loaded in your project");
            btnCompute.Enabled = false;
            Close();
            return;
         }

         FamilySymbol fs = commandData.Application.ActiveUIDocument.Document.get_Element(list[0].GetTypeId()) as FamilySymbol;
         string famDelimiter = ":";
         foreach (Autodesk.Revit.DB.FamilySymbol famSymbol in fs.Family.Symbols)
         {
            cboxMax.Items.Add(fs.Family.Name + famDelimiter + famSymbol.Name);
            cboxMin.Items.Add(fs.Family.Name + famDelimiter + famSymbol.Name);
            cboxMid.Items.Add(fs.Family.Name + famDelimiter + famSymbol.Name);
         }
         cboxMax.SelectedIndex = 0;
         cboxMin.SelectedIndex = 0;
         cboxMid.SelectedIndex = 0;
      }

      /// <summary>
      /// Analyse the panel types set by UI operation
      /// </summary>
      private void SetPanelTypesFromUI()
      {
         //Set the min, mid, and max panel types based on user selections in the UI
         string minFamilyAndType = Convert.ToString(cboxMin.Text);
         string maxFamilyAndType = Convert.ToString(cboxMax.Text);
         string midFamilyAndType = Convert.ToString(cboxMid.Text);

         string delimStr = ":";
         char[] delimiter = delimStr.ToCharArray();
         string[] split = minFamilyAndType.Split(delimiter);
         m_minType = split[1];
         split = maxFamilyAndType.Split(delimiter);
         m_maxType = split[1];
         split = midFamilyAndType.Split(delimiter);
         m_midType = split[1];
      }

      /// <summary>
      /// Populate DividedSurfaceArray with the selected surfaces or all surfaces in the model
      /// </summary>
      private void GetDividedSurfaces()
      {
         // want to compute all the divided surfaces
         if (m_uiDoc.Selection.Elements.Size == 0)
         {
            m_dividedSurfaceList = GetElements<DividedSurface>();
            return;
         }

         // user selects some divided surface
         foreach (Element element in m_uiDoc.Selection.Elements)
         {
            DividedSurface ds = element as DividedSurface;
            if (ds != null)
            {
               m_dividedSurfaceList.Add(ds);
            }
         }
      }

      /// <summary>
      /// Compute the area of the curtain panel instance
      /// </summary>
      /// <param name="familyinstance">
      /// the curtain panel which needs to be computed
      /// </param>
      /// <returns>
      /// the area of the curtain panel
      /// </returns>
      private double GetAreaOfTileInstance(FamilyInstance familyinstance)
      {
         double panelArea = 0d;
         Autodesk.Revit.DB.Options opt = m_uiApp.Application.Create.NewGeometryOptions();
         opt.ComputeReferences = true;
         Autodesk.Revit.DB.GeometryElement geomElem = familyinstance.get_Geometry(opt);
         foreach (GeometryObject geomObject1 in geomElem.Objects)
         {
            Solid solid = null;
            // find area of partial border panels
            if (geomObject1 is Solid)
            {
               solid = (Solid)geomObject1;
               if (null == solid)
               {
                  continue;
               }
            }
            // find area of non-partial panels
            else if (geomObject1 is GeometryInstance)
            {
                GeometryInstance geomInst = geomObject1 as GeometryInstance;
               foreach (Object geomObj in geomInst.SymbolGeometry.Objects)
               {
                  solid = geomObj as Solid;
                  if (solid != null)
                     break;
               }
            }

            if (null == solid.Faces || 0 == solid.Faces.Size)
            {
               continue;
            }

            // get the area and write the data to a text file
            foreach (Face face in solid.Faces)
            {
               panelArea = face.Area;
               m_writeFile.WriteLine(familyinstance.Id.IntegerValue + " : " + panelArea);
            }
         }
         return panelArea;
      }

      /// <summary>
      /// Check all the panels whose areas are below/above/within the range in the divided surface, mark them with different symbols
      /// </summary>
      /// <param name="ds">
      /// The divided surfaces created in the document, it contains the panels for checking
      /// </param>
      void ExamineDividedSurface(DividedSurface ds)
      {
         ElementType sym = ds.Document.get_Element(ds.GetTypeId()) as ElementType;
         FamilySymbol fs_min = null;
         FamilySymbol fs_max = null;
         FamilySymbol fs_mid = null;

         // get the panel types which are used to identify the panels in the divided surface
         FamilySymbol fs = sym as FamilySymbol;
         foreach (FamilySymbol symbol in fs.Family.Symbols)
         {
            if (symbol.Name == m_maxType)
            {
               fs_max = symbol;
            }
            if (symbol.Name == m_minType)
            {
               fs_min = symbol;
            }
            if (symbol.Name == m_midType)
            {
               fs_mid = symbol;
            }
         }

         // find all the panels areas and compare with the range
         for (int u = 0; u < ds.NumberOfUGridlines; u++ )
         {
            for (int v = 0; v < ds.NumberOfVGridlines; v++ )
            {
               GridNode gn = new GridNode(u, v);
               if (false == ds.IsSeedNode(gn))
               {
                  continue;
               }
               
               FamilyInstance familyinstance = ds.GetTileFamilyInstance(gn, 0);
               if (familyinstance != null)
               {
                  double panelArea = GetAreaOfTileInstance(familyinstance);
                  // identify the panels drop in different ranges with different types
                  if (panelArea > m_maxValue)
                  {
                     familyinstance.Symbol = fs_max;
                     m_maxCounter++;
                  }
                  else if (panelArea < m_minValue)
                  {
                     familyinstance.Symbol = fs_min;
                     m_minCounter++;
                  }
                  else
                  {
                     familyinstance.Symbol = fs_mid;
                     m_okCounter++;
                  }
               }
            }
         }
      }

      protected List<T> GetElements<T>() where T : Element
      {
         List<T> returns = new List<T>();
         FilteredElementCollector collector = new FilteredElementCollector(m_uiDoc.Document);
         ICollection<Element> founds = collector.OfClass(typeof(T)).ToElements();
         foreach (Element elem in founds)
         {
            returns.Add(elem as T);
         }
         return returns;
      }

   }
}