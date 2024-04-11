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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revit.SDK.Samples.DisallowEndWrapping.CS
{
   class UnownedDisposable<T>
    where T : IDisposable
   {
      private readonly T _value;

      public UnownedDisposable(T value)
      {
         _value = value;
      }

      public T Deref() => _value;
   }

   /// <summary>
   /// UI to display the wall locations' end wrapping information
   /// </summary>
   public partial class WallInformationForm : System.Windows.Forms.Form
   {

      UnownedDisposable<Wall> curWall = null;

      /// <summary>
      /// constructor
      /// </summary>
      public WallInformationForm()
      {
         InitializeComponent();
      }

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="wall"></param>
      public WallInformationForm(Wall wall)
      {
         InitializeComponent();
         updateDisplay(wall);
      }

      /// <summary>
      /// display data on the list
      /// </summary>
      public void updateDisplay(Wall wall)
      {
         locationsView.Items.Clear();
         if (wall == null)
            return;

         curWall = new UnownedDisposable<Wall>(wall);

         List<AllowData> wrappingAllowData = new List<AllowData>();
         if (wall != null)
         {
            var locations = wall.GetValidWrappingLocationIndices();
            foreach (var location in locations)
            {
               AllowData data = new AllowData();
               data.location = location;
               data.allowed = wall.IsWrappingAtLocationAllowed(location);
               data.parameter = wall.GetWrappingLocationAsCurveParameter(location);
               data.faces = wall.GetWrappingLocationAsReferences(location);
               wrappingAllowData.Add(data);
            }
         }

         foreach (var allowdata in wrappingAllowData)
         {
            ListViewItem tmpItem = new ListViewItem();
            tmpItem.SubItems.Add(allowdata.location.ToString());
            tmpItem.SubItems.Add(allowdata.allowed.ToString());
            tmpItem.SubItems.Add(allowdata.parameter.ToString());
            tmpItem.SubItems.Add(getFaceTags(allowdata.faces));
            tmpItem.SubItems.RemoveAt(0);
            locationsView.Items.Add(tmpItem);
         }
         locationsView.Update();
      }

      private string getFaceTags(IList<Reference> refs)
      {
         string str = "";
         foreach(var refface in refs)
         {
            var face = curWall.Deref().GetGeometryObjectFromReference(refface) as Face;
            if (face != null)
               str += face.Id + " ";
         }
         return str;
      }

      private int getInputLocation()
      {
         int location = -1;
         bool convert = int.TryParse(textBox1.Text, out location);
         if (!convert)
         {
            Autodesk.Revit.UI.TaskDialog.Show("Revit", "please input a valid number");
            return -1;
         }
         return location;
      }

      private void startTransactionDoSomething(Action<int> f, string transName)
      {
         Document doc = curWall.Deref().Document;
         try
         {
            using (Transaction trans = new Transaction(doc, transName))
            {
               trans.Start();
               f(0);
               trans.Commit();
            }
         }
         catch (Exception ex)
         {
            Autodesk.Revit.UI.TaskDialog.Show("Revit", "exception happens: " + ex.ToString());
         }
      }

      private void button0_Click(object sender, EventArgs e)
      {
         if (curWall.Deref() == null)
            return;

         int location = getInputLocation();
         if (location == -1)
            return;

         startTransactionDoSomething((int i) =>
         {
            curWall.Deref().AllowWrappingAtLocation(location);
         }, "Revit.SDK.Samples.AllowEndWrapping");

         updateDisplay(curWall.Deref());
      }

      private void button1_Click(object sender, EventArgs e)
      {
         if (curWall.Deref() == null)
            return;

         int location = getInputLocation();
         if (location == -1)
            return;

         startTransactionDoSomething((int i) =>
         {
            curWall.Deref().DisallowWrappingAtLocation(location);
         }, "Revit.SDK.Samples.DisallowEndWrapping");
         updateDisplay(curWall.Deref());
      }
   }

   /// <summary>
   /// 
   /// </summary>
   public class AllowData
   {
      /// <summary>
      /// location number
      /// </summary>
      public int location;
      /// <summary>
      /// is this location's end wrapping allowed
      /// </summary>
      public bool allowed;
      /// <summary>
      /// location's raw parameter on wall's curve
      /// </summary>
      public double parameter;
      /// <summary>
      /// related faces at the location
      /// </summary>
      public IList<Reference> faces;
   }
}
