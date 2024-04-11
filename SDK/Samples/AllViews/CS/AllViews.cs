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

using System.Windows.Forms;
using System.Collections;
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.AllViews.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Command : IExternalCommand
    {
        #region IExternalCommand Members Implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
       public Autodesk.Revit.UI.Result Execute(Autodesk.Revit.UI.ExternalCommandData commandData,
         ref string message, Autodesk.Revit.DB.ElementSet elements)
       {
           if (null == commandData)
           {
               throw new ArgumentNullException("commandData");
           }

           Document doc = commandData.Application.ActiveUIDocument.Document;
           ViewsMgr view = new ViewsMgr(doc);

           AllViewsForm dlg = new AllViewsForm(view);

           try
           {
               if (dlg.ShowDialog() == DialogResult.OK)
               {
                   return view.GenerateSheet(doc);
               }
           }
           catch (Exception e)
           {
               message = e.Message;
               return Autodesk.Revit.UI.Result.Failed;
           }

           return Autodesk.Revit.UI.Result.Succeeded;
      }

      #endregion IExternalCommand Members Implementation
      }

    /// <summary>
    /// Generating a new sheet that has all the selected views placed in.
    /// Updating and retrieving properties of a selected viewport.
    /// </summary>
    public class ViewsMgr : IDisposable
    {
      private TreeNode m_allViewsNames = new TreeNode("Views (all)");
      private ViewSet m_allViews = new ViewSet();
      private ViewSet m_selectedViews = new ViewSet();
      private FamilySymbol m_titleBlock;
      private IList<Element> m_allTitleBlocks = new List<Element>();
      private ArrayList m_allTitleBlocksNames = new ArrayList();
      private string m_sheetName;
      private double m_rows;

      private double TITLEBAR = 0.2;
      private double GOLDENSECTION = 0.618;

      private Document m_doc;

      private Viewport m_VP;

      /// <summary>
      /// Update Form data members bonded to UI controls.
      /// </summary>
      /// <param name="form">The Form to be updated.</param>
      public void UpdateViewportProperties(AllViewsForm form)
      {
         form.m_getMinBoxOutline = m_VP.GetBoxOutline().MinimumPoint;
         form.m_getMaxBoxOutline = m_VP.GetBoxOutline().MaximumPoint;

         form.m_getMinLabelOutline = m_VP.GetLabelOutline().MinimumPoint;
         form.m_getMaxLabelOutline = m_VP.GetLabelOutline().MaximumPoint;

         form.m_getLabelLineOffset = m_VP.LabelOffset;
         form.m_getLabelLineLength = m_VP.LabelLineLength;

         form.m_getBoxCenter = m_VP.GetBoxCenter();
         form.m_getOrientation = m_VP.Rotation;
      }

      /// <summary>
      /// Select a viewport by its associated view name and sheet name.
      /// </summary>
      /// <param name="form">The Form to be updated.</param>
      /// <param name="selectSheetName"> Sheet name.</param>
      /// <param name="selectAssociatedViewName">Associated view name.</param>
      public bool SelectViewport(AllViewsForm form, string selectSheetName, string selectAssociatedViewName)
      {
         m_VP = null;
         form.invalidViewport = true;

         FilteredElementCollector fec = new FilteredElementCollector(m_doc);
         fec.OfClass(typeof(Autodesk.Revit.DB.View));
         var viewSheets = fec.Cast<Autodesk.Revit.DB.View>().Where<Autodesk.Revit.DB.View>(vp => !vp.IsTemplate && vp.ViewType == ViewType.DrawingSheet);

         foreach (var view in viewSheets)
         {
            if (view.Name.Equals(selectSheetName))
            {
               ViewSheet viewSheet = (ViewSheet)view;
               foreach (var vp in viewSheet.GetAllViewports())
               {
                  Viewport VP = (Viewport)(m_doc.GetElement(vp));

                  Autodesk.Revit.DB.View associatedView = m_doc.GetElement(VP.ViewId) as Autodesk.Revit.DB.View;

                  if (associatedView.Name.Equals(selectAssociatedViewName))
                  {
                     m_VP = VP;
                     break;
                  }
               }
            }
         }

         if (m_VP == null)
         {
            throw new InvalidOperationException("Viewport not found.");
         }

         form.invalidViewport = false;
         UpdateViewportProperties(form);
         return true;
      }

      /// <summary>
      /// Change viewport label offset.
      /// </summary>
      /// <param name="form">The Form to be updated.</param>
      /// <param name="labelOffsetX">Label offset X component.</param>
      /// <param name="labelOffsetY">Label offset Y component.</param>
      public void SetLabelOffset(AllViewsForm form,
         double labelOffsetX, double labelOffsetY)
      {
         using (Transaction t = new Transaction(m_doc, "Change label offset"))
         {
            t.Start();

            m_VP.LabelOffset = new XYZ(labelOffsetX, labelOffsetY, 0.0);

            t.Commit();

            UpdateViewportProperties(form);
         }
      }

      /// <summary>
      /// Change viewport label length.
      /// </summary>
      /// <param name="form">The Form to be updated.</param>
      /// <param name="labelLineLength">Label line length.</param>
      public void SetLabelLength(AllViewsForm form, double labelLineLength)
      {
         using (Transaction t = new Transaction(m_doc, "Change label length"))
         {
            t.Start();

            m_VP.LabelLineLength = labelLineLength;

            t.Commit();

            UpdateViewportProperties(form);
         }
      }

      /// <summary>
      /// Change viewport orientation.
      /// </summary>
      /// <param name="form">The Form to be updated.</param>
      /// <param name="rotation">Label line rotation.</param>
      public void SetRotation(AllViewsForm form, ViewportRotation rotation)
      {
         using (Transaction t = new Transaction(m_doc, "Change label orientation"))
         {
            t.Start();

            m_VP.Rotation = rotation;

            t.Commit();

            UpdateViewportProperties(form);
         }
      }

        /// <summary>
        /// Tree node store all views' names.
        /// </summary>
        public TreeNode AllViewsNames
        {
            get
            {
                return m_allViewsNames;
            }
        }

        /// <summary>
        /// List of all title blocks' names.
        /// </summary>
        public ArrayList AllTitleBlocksNames
        {
            get
            {
                return m_allTitleBlocksNames;
            }
        }

        /// <summary>
        /// The selected sheet's name.
        /// </summary>
        public string SheetName
        {
            get
            {
                return m_sheetName;
            }
            set
            {
                m_sheetName = value;
            }
        }

        /// <summary>
        /// Constructor of views object.
        /// </summary>
        /// <param name="doc">the active document</param>
        public ViewsMgr(Document doc)
        {
            m_doc = doc;
            GetAllViews(doc);
            GetTitleBlocks(doc);
        }

        /// <summary>
        /// Finds all the views in the active document.
        /// </summary>
        /// <param name="doc">the active document</param>
        private void GetAllViews(Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            FilteredElementIterator itor = collector.OfClass(typeof(Autodesk.Revit.DB.View)).GetElementIterator();
            itor.Reset();
            while (itor.MoveNext())
            {
                Autodesk.Revit.DB.View view = itor.Current as Autodesk.Revit.DB.View;
                // skip view templates because they're invisible in project browser
                if (null == view || view.IsTemplate)
                {
                    continue;
                }
                else
                {
                    ElementType objType = doc.GetElement(view.GetTypeId()) as ElementType;
                    if (null == objType || objType.Name.Equals("Schedule")
                        || objType.Name.Equals("Drawing Sheet"))
                    {
                        continue;
                    }
                    else
                    {
                        m_allViews.Insert(view);
                        AssortViews(view.Name, objType.Name);
                    }
                }
            }
        }

        /// <summary>
        /// Assort all views for tree view displaying.
        /// </summary>
        /// <param name="view">The view assorting</param>
        /// <param name="type">The type of view</param>
        private void AssortViews(string view, string type)
        {
            foreach (TreeNode t in AllViewsNames.Nodes)
            {
                if (t.Tag.Equals(type))
                {
                    t.Nodes.Add(new TreeNode(view));
                    return;
                }
            }

            TreeNode categoryNode = new TreeNode(type);
            categoryNode.Tag = type;
            if (type.Equals("Building Elevation"))
            {
                categoryNode.Text = "Elevations [" + type + "]";
            }
            else
            {
                categoryNode.Text = type + "s";
            }
            categoryNode.Nodes.Add(new TreeNode(view));
            AllViewsNames.Nodes.Add(categoryNode);
        }

        /// <summary>
        /// Retrieve the checked view from tree view.
        /// </summary>
        public void SelectViews()
        {
            ArrayList names = new ArrayList();
            foreach (TreeNode t in AllViewsNames.Nodes)
            {
                foreach (TreeNode n in t.Nodes)
                {
                    if (n.Checked && 0 == n.Nodes.Count)
                    {
                        names.Add(n.Text);
                    }
                }
            }

            foreach (Autodesk.Revit.DB.View v in m_allViews)
            {
                foreach (string s in names)
                {
                    if (s.Equals(v.Name))
                    {
                        m_selectedViews.Insert(v);
                        break;
                    }
                }
            }
        }

      /// <summary>
      /// Generate sheet in active document.
      /// </summary>
      /// <param name="doc">the currently active document</param>
      public Autodesk.Revit.UI.Result GenerateSheet(Document doc)
      {
         if (null == doc)
         {
                throw new ArgumentNullException("doc");
         }

         if (m_selectedViews.IsEmpty)
         {
                throw new InvalidOperationException("No view be selected, generate sheet be canceled.");
         }

         Result result = Result.Succeeded;

         using (Transaction newTran = new Transaction(doc, "AllViews_Sample"))
         {
            newTran.Start();

            try
            {
                  ViewSheet sheet = ViewSheet.Create(doc, m_titleBlock.Id);
                  sheet.Name = SheetName;
                  PlaceViews(m_selectedViews, sheet);
            }
            catch(Exception)
            {
                  result = Result.Failed;
            }

            if (result == Result.Succeeded)
            {
                  newTran.Commit();
            }
            else
            {
                  newTran.RollBack();
                  throw new InvalidOperationException("Failed to generate sheet view and/or its viewports.");
            }
         }

         return result;
      }

        /// <summary>
        /// Retrieve the title block to be generate by its name.
        /// </summary>
        /// <param name="name">The title block's name</param>
        public void ChooseTitleBlock(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            foreach (FamilySymbol f in m_allTitleBlocks)
            {
               if (name.Equals(f.Family.Name + ":" + f.Name))
               {
                  m_titleBlock = f;
                  return;
               }
            }
        }

        /// <summary>
        /// Retrieve all available title blocks in the currently active document.
        /// </summary>
        /// <param name="doc">the currently active document</param>
        private void GetTitleBlocks(Document doc)
        {
            FilteredElementCollector filteredElementCollector = new FilteredElementCollector(doc);
            filteredElementCollector.OfClass(typeof(FamilySymbol));
            filteredElementCollector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            m_allTitleBlocks = filteredElementCollector.ToElements();
            if (0 == m_allTitleBlocks.Count)
            {
                throw new InvalidOperationException("There is no title block to generate sheet.");
            }

            foreach (Element element in m_allTitleBlocks)
            {
                FamilySymbol f = element as FamilySymbol;
                AllTitleBlocksNames.Add(f.Family.Name + ":" + f.Name);
                if (null == m_titleBlock)
                {
                   m_titleBlock = f;
                }
            }
        }

        /// <summary>
        /// Place all selected views on this sheet's appropriate location.
        /// </summary>
        /// <param name="views">all selected views</param>
        /// <param name="sheet">all views located sheet</param>
        private void PlaceViews(ViewSet views, ViewSheet sheet)
        {
            double xDistance = 0;
            double yDistance = 0;
            CalculateDistance(sheet.Outline, views.Size, ref xDistance, ref yDistance);

            Autodesk.Revit.DB.UV origin = GetOffSet(sheet.Outline, xDistance, yDistance);
            //Autodesk.Revit.DB.UV temp = new Autodesk.Revit.DB.UV (origin.U, origin.V);
            double tempU = origin.U;
            double tempV = origin.V;
            int n = 1;
            foreach (Autodesk.Revit.DB.View v in views)
            {
                Autodesk.Revit.DB.UV location = new Autodesk.Revit.DB.UV(tempU, tempV);
                Autodesk.Revit.DB.View view = v;
                Rescale(view, xDistance, yDistance);
                try
                {
                    //sheet.AddView(view, location);
                    Viewport.Create(view.Document, sheet.Id, view.Id, new XYZ(location.U, location.V, 0));
                }
                catch (ArgumentException /*ae*/)
                {
                    throw new InvalidOperationException("The view '" + view.Name +
                        "' can't be added, it may have already been placed in another sheet.");
                }

                if (0 != n++ % m_rows)
                {
                    tempU = tempU + xDistance * (1 - TITLEBAR);
                }
                else
                {
                    tempU = origin.U;
                    tempV = tempV + yDistance;
                }
            }
        }

        /// <summary>
        /// Retrieve the appropriate origin.
        /// </summary>
        /// <param name="bBox"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Autodesk.Revit.DB.UV GetOffSet(BoundingBoxUV bBox, double x, double y)
        {
            return new Autodesk.Revit.DB.UV(bBox.Min.U + x * GOLDENSECTION, bBox.Min.V + y * GOLDENSECTION);
        }

        /// <summary>
        /// Calculate the appropriate distance between the views lay on the sheet.
        /// </summary>
        /// <param name="bBox">The outline of sheet.</param>
        /// <param name="amount">Amount of views.</param>
        /// <param name="x">Distance in x axis between each view</param>
        /// <param name="y">Distance in y axis between each view</param>
        private void CalculateDistance(BoundingBoxUV bBox, int amount, ref double x, ref double y)
        {
            double xLength = (bBox.Max.U - bBox.Min.U) * (1 - TITLEBAR);
            double yLength = (bBox.Max.V - bBox.Min.V);

            //calculate appropriate rows numbers.
            double result = Math.Sqrt(amount);

            while (0 < (result - (int)result))
            {
                amount = amount + 1;
                result = Math.Sqrt(amount);
            }
            m_rows = result;
            double area = xLength * yLength / amount;

            //calculate appropriate distance between the views.
            if (bBox.Max.U > bBox.Max.V)
            {
                x = Math.Sqrt(area / GOLDENSECTION);
                y = GOLDENSECTION * x;
            }
            else
            {
                y = Math.Sqrt(area / GOLDENSECTION);
                x = GOLDENSECTION * y;
            }
        }

        /// <summary>
        /// Rescale the view's Scale value for suitable.
        /// </summary>
        /// <param name="view">The view to be located on sheet.</param>
        /// <param name="x">Distance in x axis between each view</param>
        /// <param name="y">Distance in y axis between each view</param>
        static private void Rescale(Autodesk.Revit.DB.View view, double x, double y)
        {
            double Rescale = 2;
            Autodesk.Revit.DB.UV outline = new Autodesk.Revit.DB.UV(view.Outline.Max.U - view.Outline.Min.U,
                view.Outline.Max.V - view.Outline.Min.V);

            if (outline.U > outline.V)
            {
                Rescale = outline.U / x * Rescale;
            }
            else
            {
                Rescale = outline.V / y * Rescale;
            }

            if (1 != view.Scale && 0 != Rescale)
            {
                view.Scale = (int)(view.Scale * Rescale);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_allViews.Dispose();
                m_selectedViews.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
   }
}
