//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.RayTraceBounce.CS
{
   /// <summary>
   /// 1. This form allowing entry of a coordinate location (X, Y, Z) within the model and a coordinate direction (i, j, k).  
   /// 2. Launch a ray from this location in this direction to find the first intersection with a face
   /// 3. Calculate the reflection angle of the ray from the face and launch another ray to find the next intersection
   /// 4. For each ray/intersection, create a model line connecting the two points.  The end result should be a series of model lines bouncing from item to item.
   /// 5. Provide a hard limit of say, 100 intersections, to prevent endless reflections within an enclosed space.
   /// 6. Write a log file of the intersection containing: the element type, id, and material of the intersected face.
   /// </summary>
   public partial class RayTraceBounceForm : System.Windows.Forms.Form
   {
      #region Class Memeber Variables
      /// <summary>
      /// current assembly name
      /// </summary>
      private static string AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().Location;

      /// <summary>
      /// current assembly directory
      /// </summary>
      private static string AssemblyDirectory = Path.GetDirectoryName(AssemblyName);

      /// <summary>
      /// epsilon limit
      /// </summary>
      private static double epsilon = 0.00000001;

      /// <summary>
      /// how many bounces to run
      /// </summary>
      private static int rayLimit = 100;

      /// <summary>
      /// revit application
      /// </summary>
      private Autodesk.Revit.UI.UIApplication m_app;

      /// <summary>
      /// current document
      /// </summary>
      private Document m_doc;

      /// <summary>
      /// closet geometry reference between origin/intersection and ray/intersection
      /// </summary>
      private Autodesk.Revit.DB.Reference m_rClosest = null;

      /// <summary>
      /// 3D view
      /// </summary>
      private Autodesk.Revit.DB.View3D m_view = null; 

      /// <summary>
      /// the count of line between origin/intersection and ray/intersection
      /// </summary>
      private int m_LineCount = 0; 

      /// <summary>
      /// the count of ray between origin/intersection and ray/intersection
      /// </summary>
      private int m_RayCount = 0;  

      /// <summary>
      /// the face which the ray intersect with
      /// </summary>
      private Face m_face = null; 

      /// <summary>
      /// ray start from here
      /// </summary>
      private Autodesk.Revit.DB.XYZ m_origin = new Autodesk.Revit.DB.XYZ (0, 0, 0); 

      /// <summary>
      /// ray direction
      /// </summary>
      private Autodesk.Revit.DB.XYZ m_direction = new Autodesk.Revit.DB.XYZ (0, 0, 0); 

      /// <summary>
      /// for time calculation
      /// </summary>
      private Stopwatch m_stopWatch = new Stopwatch(); 

      /// <summary>
      /// output string list
      /// </summary>
      private List<string> m_outputInfo = new List<string>(); 

      /// <summary>
      /// trace listener
      /// </summary>
      private TraceListener m_txtListener = null; 
      #endregion

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="commandData">Revit application</param>
      /// <param name="v">3D View</param>
      public RayTraceBounceForm(ExternalCommandData commandData, Autodesk.Revit.DB.View3D v)
      {
         InitializeComponent();

         string logFile = AssemblyName.Replace(".dll", DateTime.Now.ToString("yyyyMMddhhmmss") + ".log");
         if (File.Exists(logFile)) File.Delete(logFile);
         m_txtListener = new TextWriterTraceListener(logFile);
         Trace.Listeners.Add(m_txtListener);

         m_app = commandData.Application;
         m_doc = commandData.Application.ActiveUIDocument.Document;
         m_view = v;
         UpdateData(true);
         // Delete lines it created
         DeleteLines();
      }
      /// <summary>
      /// OK button click event
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void buttonOK_Click(object sender, EventArgs e)
      {
         DeleteLines();
         if (!UpdateData(false))
         {
            return;
         }
         m_outputInfo.Clear();
         m_stopWatch.Start();
         SubTransaction transaction = new SubTransaction(m_doc);
         transaction.Start();
         m_LineCount = 0;
         m_RayCount = 0; 
         // Start Find References By Direction
         Autodesk.Revit.DB.XYZ startpt = m_origin;
         m_outputInfo.Add("Start Find References By Direction: ");
         for (int ctr = 1; ctr <= rayLimit; ctr++)
         {
            Autodesk.Revit.DB.ReferenceArray references = m_doc.FindReferencesByDirection(startpt, m_direction, m_view);
            m_rClosest = null;
            FindClosestReference(references);
            if (m_rClosest == null)
            {
               string info = "Ray " + ctr + " aborted. No closest face reference found. ";
               m_outputInfo.Add(info);
               if (ctr == 1)
               {
               MessageBox.Show(info);
               }
               break;
            }
            else
            {
               Autodesk.Revit.DB.XYZ endpt = new Autodesk.Revit.DB.XYZ (m_rClosest.GlobalPoint.X, m_rClosest.GlobalPoint.Y, m_rClosest.GlobalPoint.Z);
               if (startpt.IsAlmostEqualTo(endpt))
               {
                  m_outputInfo.Add("Start and end points are equal. Ray " + ctr + " aborted\n" + startpt.X + ", " + startpt.Y + ", " + startpt.Z);
                  break;
               }
               else
               {
                  MakeLine(startpt, endpt, m_direction, "bounce");
                  m_RayCount = m_RayCount + 1;
                  string info = "Intersected Element Type: [" + m_rClosest.Element.GetType().ToString() + "] ElementId: [" + m_rClosest.Element.Id.IntegerValue.ToString();
                  m_face = m_rClosest.GeometryObject as Face;
                  if (m_face.MaterialElement != null)
                  {
                     info += "] Face MaterialElement Name: [" + m_face.MaterialElement.Name + "] Shininess: [" + m_face.MaterialElement.Shininess;
                  }
                  else
                  {
                     info += "] Face.MaterialElement is null [" + m_rClosest.Element.Category.Name;
                  }
                  info += "]";
                  m_outputInfo.Add(info);
                  Autodesk.Revit.DB.UV endptUV = m_rClosest.UVPoint;
                  Autodesk.Revit.DB.XYZ FaceNormal = m_face.ComputeDerivatives(endptUV).BasisZ;  // face normal where ray hits
                  FaceNormal = m_rClosest.Transform.OfVector(FaceNormal); // transformation to get it in terms of document coordinates instead of the parent symbol
                  Autodesk.Revit.DB.XYZ directionMirrored = m_direction - 2 * m_direction.DotProduct(FaceNormal) * FaceNormal; //http://www.fvastro.org/presentations/ray_tracing.htm
                  m_direction = directionMirrored; // get ready to shoot the next ray
                  startpt = endpt;
               }
            }
         }
         transaction.Commit();
         m_stopWatch.Stop();
         TimeSpan ts = m_stopWatch.Elapsed;
         string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
         m_outputInfo.Add(elapsedTime + "\n" + "Lines = " + m_LineCount + "\n" + "Rays = " + m_RayCount);
         m_stopWatch.Reset();
         OutputInformation();
      }
      /// <summary>
      /// Cancel button click event
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void buttonCancel_Click(object sender, EventArgs e)
      {
         Close();
      }
      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         Trace.Flush();
         m_txtListener.Close();
         Trace.Close();
         Trace.Listeners.Remove(m_txtListener);

         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }
      /// <summary>
      /// Update textbox data with member variable
      /// </summary>
      /// <param name="updateControl">if get/set date from control</param>
      public bool UpdateData(bool updateControl)
      {
         try
         {
            if (updateControl)
            {
               textBoxLocationX.Text = Convert.ToString(0.0);
               textBoxLocationY.Text = Convert.ToString(0.0);
               textBoxLocationZ.Text = Convert.ToString(0.0);

               textBoxDirectionI.Text = Convert.ToString(Math.Cos(0));
               textBoxDirectionJ.Text = Convert.ToString(Math.Sin(0));
               textBoxDirectionK.Text = Convert.ToString(0.0);
            }
            else
            {
               if (string.IsNullOrEmpty(textBoxLocationX.Text) || string.IsNullOrEmpty(textBoxLocationY.Text) || 
                   string.IsNullOrEmpty(textBoxLocationZ.Text) || string.IsNullOrEmpty(textBoxDirectionI.Text) ||  
                   string.IsNullOrEmpty(textBoxDirectionJ.Text) || string.IsNullOrEmpty(textBoxDirectionK.Text))
               {
                  MessageBox.Show("Value cannot be empty.");
                  return false;
               }

               m_origin = new XYZ(
                   Convert.ToDouble(textBoxLocationX.Text),
                   Convert.ToDouble(textBoxLocationY.Text),
                   Convert.ToDouble(textBoxLocationZ.Text));

               m_direction = new XYZ(
                   Convert.ToDouble(textBoxDirectionI.Text),
                   Convert.ToDouble(textBoxDirectionJ.Text),
                   Convert.ToDouble(textBoxDirectionK.Text));
            }
            return true;
         }
         catch (System.Exception)
         {
            return false;
         }
      }
      /// <summary>
      /// Find the first intersection with a face
      /// </summary>
      /// <param name="references"></param>
      /// <returns></returns>
      public Autodesk.Revit.DB.Reference FindClosestReference(ReferenceArray references)
      {
         double face_prox = Double.PositiveInfinity;
         double edge_prox = Double.PositiveInfinity;
         foreach (Autodesk.Revit.DB.Reference r in references)
         {
            m_face = null;
            m_face = r.GeometryObject as Face;
            Edge edge = null;
            edge = r.GeometryObject as Edge;
            if (m_face != null)
            {
               if ((r.ProximityParameter < face_prox)&& (r.ProximityParameter > epsilon))
               {
                  m_rClosest = r;
                  face_prox = Math.Abs(r.ProximityParameter);
               }
            }
            else if (edge != null)
            {
               if ((r.ProximityParameter < edge_prox) && (r.ProximityParameter > epsilon))
               {
                  edge_prox = Math.Abs(r.ProximityParameter);
               }
            }
         }
         if (edge_prox <= face_prox)
         {
            // stop bouncing if there is an edge at least as close as the neareast face - there is no single angle of reflection for a ray striking a line
            m_outputInfo.Add("there is an edge at least as close as the neareast face - there is no single angle of reflection for a ray striking a line");
            m_rClosest = null; 
         }
         return m_rClosest;
      }
      /// <summary>
      /// Make a line from start point to end point with the direction and style
      /// </summary>
      /// <param name="startpt">start point</param>
      /// <param name="endpt">end point</param>
      /// <param name="direction">the direction which decide the plane</param>
      /// <param name="style">line style name</param>
      public void MakeLine(Autodesk.Revit.DB.XYZ startpt, Autodesk.Revit.DB.XYZ endpt, Autodesk.Revit.DB.XYZ direction, string style)
      {
         try
         {
            m_LineCount = m_LineCount + 1;
            Line line = m_app.Application.Create.NewLineBound(startpt, endpt);
            Plane geometryPlane = m_app.Application.Create.NewPlane(direction, startpt);
            SketchPlane skplane = m_app.ActiveUIDocument.Document.Create.NewSketchPlane(geometryPlane);
            ModelCurve mcurve = m_app.ActiveUIDocument.Document.Create.NewModelCurve(line, skplane);
            ElementArray lsArr = mcurve.LineStyles;
            foreach (Autodesk.Revit.DB.Element e in lsArr)
            {
               if (e.Name == style)
               {
                  mcurve.LineStyle = e;
                  break;
               }
            }
         }
         catch (System.Exception)
         {
         }
      }
      /// <summary>
      /// Delete all unnecessary lines
      /// </summary>
      public void DeleteLines()
      {
         int delLineNum = 0;
         try
         {
             SubTransaction transaction = new SubTransaction(m_app.ActiveUIDocument.Document);
            transaction.Start();
            List<Autodesk.Revit.DB.Element> list = new List<Autodesk.Revit.DB.Element>();
            ElementClassFilter filter = new ElementClassFilter(typeof(Autodesk.Revit.DB.CurveElement));
            FilteredElementCollector collector = new FilteredElementCollector(m_app.ActiveUIDocument.Document);
             list.AddRange(collector.WherePasses(filter).ToElements());
            foreach (Autodesk.Revit.DB.Element e in list)
            {
               ModelCurve mc = e as ModelCurve;
               if (mc != null)
               {
                  if (mc.LineStyle.Name == "bounce" || mc.LineStyle.Name == "normal")
                  {
                      m_app.ActiveUIDocument.Document.Delete(e);
                     delLineNum++;
                  }
               }
            }
            transaction.Commit();
         }
         catch (System.Exception)
         {     	
         }
      }
      /// <summary>
      /// Output the information to log file
      /// </summary>
      protected void OutputInformation()
      {
         foreach (string item in m_outputInfo)
         {
            Trace.WriteLine(item);
         }
         Trace.Flush();
      }
   }
}
