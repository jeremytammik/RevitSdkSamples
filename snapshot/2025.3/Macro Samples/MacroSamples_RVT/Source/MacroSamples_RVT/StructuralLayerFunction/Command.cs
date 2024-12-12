//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using MacroCSharpSamples;
using MacroSamples_RVT;

namespace Revit.SDK.Samples.StructuralLayerFunction.CS
{
   /// <summary>
   /// With the selected floor, display the function of each of its structural layers
   /// in order from outside to inside in a dialog box
   /// </summary>
   public class StructuralLayerFunction
   {
      #region Private data members
      Autodesk.Revit.DB.Floor? m_slab = null;   // Store the selected floor
      ArrayList? m_functions;                   // Store the function of each floor
      ThisApplication? m_app;                        // host document for ThisDocument
      #endregion


      #region class public property
      /// <summary>
      /// With the selected floor, export the function of each of its structural layers
      /// </summary>
      public ArrayList? Functions
      {
         get
         {
            return m_functions;
         }
      }
      #endregion


      #region Class ctor implemetation
      /// <summary>
      /// Ctor without parameter is not allowed
      /// </summary>
      private StructuralLayerFunction()
      {
         // no codes
      }

      /// <summary>
      /// Default constructor of StructuralLayerFunction
      /// </summary>
      public StructuralLayerFunction(ThisApplication hostApp)
      {
         // Init for varialbes
         // this document handler
         m_app = hostApp;
         m_functions = new ArrayList();
      }

      /// <summary>
      /// Run this sample now
      /// </summary>
      public void Run()
      {
         // 
         // Get the selected floor
         if (m_app == null)
            return;
         Selection choices = m_app.ActiveUIDocument.Selection;
         ICollection<ElementId> collection = choices.GetElementIds();
         //
         // Only allow to select one floor, or else report the failure
         if (1 != collection.Count)
         {
            MessageBox.Show("Please select a floor firstly.");
            return;
         }
         foreach (ElementId id in collection)
         {
            m_slab = m_app.ActiveUIDocument.Document.GetElement(id) as Autodesk.Revit.DB.Floor;
            if (null == m_slab)
            {
               MessageBox.Show("Please select a floor firstly.");
               return;
            }
         }
         //
         // Get the function of each of its structural layers
         if (m_slab == null)
            return;
         foreach (CompoundStructureLayer e in m_slab.FloorType.GetCompoundStructure().GetLayers())
         {
            // With the selected floor, judge if the function of each of its structural layers
            // is exist, if it's not exist, there should be zero.
            if (0 == e.Function)
            {
               m_functions?.Add("No function");
            }
            else
            {
               m_functions?.Add(e.Function.ToString());
            }

         }
         //
         // Display them in a form
         StructuralLayerFunctionForm displayForm = new StructuralLayerFunctionForm(this);
         displayForm.ShowDialog();
      }
      #endregion
   }
}
