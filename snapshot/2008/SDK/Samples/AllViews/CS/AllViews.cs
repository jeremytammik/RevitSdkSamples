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
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Collections;
using System.Xml;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.AllViews.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
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
        public Autodesk.Revit.IExternalCommand.Result Execute(Autodesk.Revit.ExternalCommandData commandData,
            ref string message, Autodesk.Revit.ElementSet elements)
        {
            try
            {
                if (null == commandData)
                {
                    throw new ArgumentNullException("commandData");
                }

                Document doc = commandData.Application.ActiveDocument;
                ViewsMgr view = new ViewsMgr(doc);

                AllViewsForm dlg = new AllViewsForm(view);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    view.GenerateSheet(doc);
                }

                return IExternalCommand.Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return IExternalCommand.Result.Failed;
            }
        }

        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Generating a new sheet that has all the selected views placed in.
    /// </summary>
    public class ViewsMgr
    {
        private TreeNode m_allViewsNames = new TreeNode("Views (all)");
        private ViewSet m_allViews = new ViewSet();
        private ViewSet m_selectedViews = new ViewSet();
        private FamilySymbol m_titleBlock;
        private FamilySymbolSet m_allTitleBlocks = new FamilySymbolSet();
        private ArrayList m_allTitleBlocksNames = new ArrayList();
        private string m_sheetName;
        private double m_rows;

        private double TITLEBAR = 0.2;
        private double GOLDENSECTION = 0.618;

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
            GetAllViews(doc);
            GetTitleBlocks(doc);
        }

        /// <summary>
        /// Finds all the views in the active document.
        /// </summary>
        /// <param name="doc">the active document</param>
        private void GetAllViews(Document doc)
        {
            ElementIterator itor = doc.Elements;
            itor.Reset();

            while (itor.MoveNext())
            {
                Autodesk.Revit.Elements.View view = itor.Current as Autodesk.Revit.Elements.View;

                if (null == view)
                {
                    continue;
                }
                else
                {
                    Symbol objType = view.ObjectType;
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

            foreach (Autodesk.Revit.Elements.View v in m_allViews)
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
        public void GenerateSheet(Document doc)
        {
            if (null == doc)
            {
                throw new ArgumentNullException("doc");
            }

            if (0 == m_selectedViews.Size)
            {
                throw new InvalidOperationException("No view be selected, generate sheet be canceled.");
            }
            ViewSheet sheet = doc.Create.NewViewSheet(m_titleBlock);
            sheet.Name = SheetName;
            PlaceViews(m_selectedViews, sheet);
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
                if (name.Equals(f.Name))
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
            m_allTitleBlocks = doc.TitleBlocks;
            if (0 == m_allTitleBlocks.Size)
            {
                throw new InvalidOperationException("There is no title block to generate sheet.");
            }

            foreach (FamilySymbol f in m_allTitleBlocks)
            {
                AllTitleBlocksNames.Add(f.Name);

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

            UV origin = GetOffSet(sheet.Outline, xDistance, yDistance);
            UV temp = new UV(origin.U, origin.V);

            int n = 1;
            foreach (Autodesk.Revit.Elements.View v in views)
            {
                UV location = new UV(temp.U, temp.V);
                Autodesk.Revit.Elements.View view = v;
                Rescale(view, xDistance, yDistance);
                try
                {
                    sheet.AddView(view, ref location);
                }
                catch (ArgumentException /*ae*/)
                {
                    throw new InvalidOperationException("The view " + view.Name + " is not appropriate, or it was placed in another sheet.");
                }

                if (0 != n++ % m_rows)
                {
                    temp.U = temp.U + xDistance * (1 - TITLEBAR);
                }
                else
                {
                    temp.U = origin.U;
                    temp.V = temp.V + yDistance;
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
        private UV GetOffSet(BoundingBoxUV bBox, double x, double y)
        {
            return new UV(bBox.Min.U + x * GOLDENSECTION, bBox.Min.V + y * GOLDENSECTION);
        }

        /// <summary>
        /// Calculate the apropriate distance between the views lay on the sheet.
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
        static private void Rescale(Autodesk.Revit.Elements.View view, double x, double y)
        {
            double Rescale = 2;
            UV outline = new UV(view.Outline.Max.U - view.Outline.Min.U,
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
    }
}
