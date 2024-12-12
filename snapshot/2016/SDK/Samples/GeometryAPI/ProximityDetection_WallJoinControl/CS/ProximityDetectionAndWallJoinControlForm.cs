//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ProximityDetection_WallJoinControl.CS
{
   /// <summary>
   /// The form that show the operations and results
   /// </summary>
   public partial class ProximityDetectionAndWallJoinControlForm : System.Windows.Forms.Form
   {
      /// <summary>
      /// revit document
      /// </summary>
      private Autodesk.Revit.DB.Document m_doc;
      /// <summary>
      /// The object that is responsible for proximity detection
      /// </summary>
      private ProximityDetection m_proximityDetection;
      /// <summary>
      /// The object that is responsible for controlling the joint of walls
      /// </summary>
      private WallJoinControl m_walljoinControl;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="doc">Revit document</param>
      /// <param name="proximityDetection">The object that is responsible for proximity detection</param>
      /// <param name="walljoinControl">The object that is responsible for controlling the joint of walls</param>
      public ProximityDetectionAndWallJoinControlForm(Autodesk.Revit.DB.Document doc, 
         ProximityDetection proximityDetection, 
         WallJoinControl walljoinControl)
      {
         InitializeComponent();

         m_doc = doc;
         m_proximityDetection = proximityDetection;
         m_walljoinControl = walljoinControl;
      }

      /// <summary>
      /// Clear the treeview's data
      /// </summary>
      private void ClearTreeviewData()
      {
         this.treeViewResults.Nodes.Clear();
      }

      /// <summary>
      /// Refresh the treeview's data by given XML
      /// </summary>
      /// <param name="operation">The operation name</param>
      /// <param name="element">The given XML</param>
      private void RefreshTreeviewData(string operation, XElement element)
      {
         this.treeViewResults.Nodes.Clear();

         //treeView.Nodes adds first level node
         TreeNode node = new TreeNode(operation);
         this.treeViewResults.Nodes.Add(node);

         // append node
         TreeNode spaceNode = XElementToTreeNode(element);
         node.Nodes.Add(spaceNode);
      }
      
      /// <summary>
      /// This method converts XElement nodes to Tree nodes
      /// </summary>
      /// <param name="element">XElement to be converted</param>
      /// <returns>Tree Node that comes from XElement</returns>
      private TreeNode XElementToTreeNode(XElement element)
      {
         if (0 == element.Name.LocalName.Length)
            return null;
         
         string nodename = element.Name.LocalName;
         foreach(XAttribute att in element.Attributes())
         {
            nodename += " (" + att.Name.LocalName + ":" + att.Value + ")";
         }
         //TreeNode node = new TreeNode(element.FirstAttribute.Value);
         TreeNode node = new TreeNode(nodename);

         if (!element.HasElements)
            // return if it is leaf node
            return node;
         // convert its child elements
         foreach (XElement ele in element.Elements())
         {
            node.Nodes.Add(XElementToTreeNode(ele));
         }
         
         // return whole node
         return node;
      }

      /// <summary>
      /// Get all of the walls
      /// </summary>
      /// <returns>All walls' set</returns>
      private IEnumerable<Wall> getAllWalls()
      {
         FilteredElementCollector wallCollector = new FilteredElementCollector(m_doc);
         wallCollector.OfClass(typeof(Wall));

         return wallCollector.OfType<Wall>();
      }

      /// <summary>
      /// Get all of the egresses
      /// </summary>
      /// <returns>All egresses' set</returns>
      private ICollection<Element> getAllEgresses()
      {
         FilteredElementCollector collector = new FilteredElementCollector(m_doc);
         ElementClassFilter filterFamilyInstance = new ElementClassFilter(typeof(FamilyInstance));
         ElementCategoryFilter filterDoorCategory = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
         LogicalAndFilter filterDoorInstance = new LogicalAndFilter(filterDoorCategory, filterFamilyInstance);
         return collector.WherePasses(filterDoorInstance).ToElements();
      }

      private void radioButtonFindColumnsInWall_CheckedChanged(object sender, EventArgs e)
      {
         ClearTreeviewData();

         labelDescription.Text = "Find columns in wall";
      }

      private void radioButtonFindBlockingElements_CheckedChanged(object sender, EventArgs e)
      {
         ClearTreeviewData();

         labelDescription.Text = "Find elements blocking egress";
      }

      private void radioButtonFindNearbyWalls_CheckedChanged(object sender, EventArgs e)
      {
         ClearTreeviewData();

         labelDescription.Text = "Find walls (nearly joined to) end of walls";
      }

      private void radioButtonCheckJoinedWalls_CheckedChanged(object sender, EventArgs e)
      {
         ClearTreeviewData();

         labelDescription.Text = "Check walls join/disjoin states";
      }

      private void buttonOK_Click(object sender, EventArgs e)
      {
         Transaction tran = new Transaction(m_doc, "GeometryCreation_BooleanOperation");
         tran.Start();

         if (radioButtonFindColumnsInWall.Checked)
         {
            RefreshTreeviewData("Find columns in wall", m_proximityDetection.findColumnsInWall(getAllWalls()));
         }
         else if (radioButtonFindBlockingElements.Checked)
         {
            RefreshTreeviewData("Find elements blocking egress", m_proximityDetection.findBlockingElements(getAllEgresses()));
         }
         else if (radioButtonFindNearbyWalls.Checked)
         {
            RefreshTreeviewData("Find walls (nearly joined to) end of walls", m_proximityDetection.findNearbyWalls(getAllWalls()));
         }
         else
         {
            RefreshTreeviewData("Check walls join/disjoin states", m_walljoinControl.checkJoinedWalls(getAllWalls()));
         }

         // expand all child
         this.treeViewResults.ExpandAll();

         tran.Commit();
      }

   }
}
