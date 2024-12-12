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
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.PanelEdgeLengthAngle.CS
{
   /// <summary>
   /// A class inherits IExternalCommand interface.
   /// This class shows how to compute the length and angle data of curtain panels
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class SetLengthAngleParams : IExternalCommand
   {
      /// <summary>
      /// The Revit application instance
      /// </summary>
      Autodesk.Revit.ApplicationServices.Application m_app;
      /// <summary>
      /// The active Revit document
      /// </summary>
      Autodesk.Revit.DB.Document m_doc;

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
      public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
      {
         m_app = commandData.Application.Application;
         m_doc = commandData.Application.ActiveUIDocument.Document;

         // step 1: get all the divided surfaces in the Revit document
         List<DividedSurface> dsList = GetElements<DividedSurface>();

         foreach (DividedSurface ds in dsList)
         {
            // step 2: get the panel instances from the divided surface
            List<FamilyInstance> fiList = GetFamilyInstances(ds);
            foreach (FamilyInstance inst in fiList)
            {
               // step 3: compute the length and angle and set them to the parameters
               InstParameters instParams = GetParams(inst);
               EdgeArray edges = GetEdges(inst);
               SetParams(edges, instParams);
            }
         }
         return Autodesk.Revit.UI.Result.Succeeded;
      }

      /// <summary>
      /// Get all the panel instances from a divided surface
      /// </summary>
      /// <param name="ds">The divided surface with some panels</param>
      /// <returns>A list containing all the panel instances</returns>
      private List<FamilyInstance> GetFamilyInstances(DividedSurface ds)
      {
         List<FamilyInstance> fiList = new List<FamilyInstance>();

         for (int u = 0; u < ds.NumberOfUGridlines; ++u)
         {
            for (int v = 0; v < ds.NumberOfVGridlines; ++v)
            {
               GridNode gn = new GridNode(u, v);
               FamilyInstance familyInstance = ds.GetTileFamilyInstance(gn, 0);
               if (familyInstance != null)
               {
                  fiList.Add(familyInstance);
               }
            }
         }
         return fiList;
      }

      /// <summary>
      /// Get all the edges from the given family instance
      /// </summary>
      /// <param name="familyInstance">The family instance with some edges</param>
      /// <returns>Edges of the family instance</returns>
      private EdgeArray GetEdges(FamilyInstance familyInstance)
      {
         Autodesk.Revit.DB.Options opt = m_app.Create.NewGeometryOptions();
         opt.ComputeReferences = true;
         Autodesk.Revit.DB.GeometryElement geomElem = familyInstance.get_Geometry(opt);
         //foreach (GeometryObject geomObject1 in geomElem.Objects)
         IEnumerator<GeometryObject> Objects = geomElem.GetEnumerator();
         while (Objects.MoveNext())
         {
            GeometryObject geomObject1 = Objects.Current;

            Solid solid = null;
            // partial panels
            if (geomObject1 is Solid)
            {
               solid = (Solid)geomObject1;
               if (null == solid)
                  continue;
            }
            // non-partial panels
            else if (geomObject1 is Autodesk.Revit.DB.GeometryInstance)
            {
               GeometryInstance geomInst = geomObject1 as GeometryInstance;
               //foreach (Object geomObj in geomInst.SymbolGeometry.Objects)
               IEnumerator<GeometryObject> Objects1 = geomInst.SymbolGeometry.GetEnumerator();
               while (Objects1.MoveNext())
               {
                  Object geomObj = Objects1.Current;

                  solid = geomObj as Solid;
                  if (solid != null)
                     break;
               }
            }

            if (null == solid ||    // the solid can't be null
                null == solid.Faces || 0 == solid.Faces.Size ||   // the solid must have 1 or more faces
                null == solid.Faces.get_Item(0) ||   // the solid must have a NOT-null face
                null == solid.Faces.get_Item(0).EdgeLoops || 0 == solid.Faces.get_Item(0).EdgeLoops.Size) // the face must have some edges
               continue;

            return solid.Faces.get_Item(0).EdgeLoops.get_Item(0);
         }

         return null;
      }

      /// <summary>
      /// Compute the length and angle data of the edges, then update the parameters with these values
      /// </summary>
      /// <param name="edge_ar">The edges of the curtain panel</param>
      /// <param name="instParams">The parameters which records the length and angle data</param>
      private void SetParams(EdgeArray edge_ar, InstParameters instParams)
      {
         double length4 = 0d;
         double angle3 = 0d;
         double angle4 = 0d;
         Edge edge1 = edge_ar.get_Item(0);
         Edge edge2 = edge_ar.get_Item(1);
         Edge edge3 = edge_ar.get_Item(2);
         double length1 = edge1.ApproximateLength;
         double length2 = edge2.ApproximateLength;
         double length3 = edge3.ApproximateLength;
         double angle1 = AngleBetweenEdges(edge1, edge2);
         double angle2 = AngleBetweenEdges(edge2, edge3);

         if (edge_ar.Size == 3)
         {
            angle3 = AngleBetweenEdges(edge3, edge1);
         }
         else if (edge_ar.Size > 3)
         {
            Edge edge4 = edge_ar.get_Item(3);
            length4 = edge4.ApproximateLength;
            angle3 = AngleBetweenEdges(edge3, edge4);
            angle4 = AngleBetweenEdges(edge4, edge1);
         }

         instParams["Length1"].Set(length1);
         instParams["Length2"].Set(length2);
         instParams["Length3"].Set(length3);
         instParams["Length4"].Set(length4);
         instParams["Angle1"].Set(angle1);
         instParams["Angle2"].Set(angle2);
         instParams["Angle3"].Set(angle3);
         instParams["Angle4"].Set(angle4);
      }

      /// <summary>
      /// Compute the angle between two edges
      /// </summary>
      /// <param name="edgeA">The 1st edge</param>
      /// <param name="edgeB">The 2nd edge</param>
      /// <returns>The angle of the 2 edges</returns>
      private double AngleBetweenEdges(Edge edgeA, Edge edgeB)
      {
         Autodesk.Revit.DB.XYZ vectorA = null;
         Autodesk.Revit.DB.XYZ vectorB = null;

         // find coincident vertices
         Autodesk.Revit.DB.XYZ A_0 = edgeA.Evaluate(0);
         Autodesk.Revit.DB.XYZ A_1 = edgeA.Evaluate(1);
         Autodesk.Revit.DB.XYZ B_0 = edgeB.Evaluate(0);
         Autodesk.Revit.DB.XYZ B_1 = edgeB.Evaluate(1);
         if (A_0.IsAlmostEqualTo(B_0))
         {
            vectorA = edgeA.ComputeDerivatives(0).BasisX.Normalize();
            vectorB = edgeA.ComputeDerivatives(0).BasisX.Normalize();
         }
         else if (A_0.IsAlmostEqualTo(B_1))
         {
            vectorA = edgeA.ComputeDerivatives(0).BasisX.Normalize();
            vectorB = edgeB.ComputeDerivatives(1).BasisX.Normalize();
         }
         else if (A_1.IsAlmostEqualTo(B_0))
         {
            vectorA = edgeA.ComputeDerivatives(1).BasisX.Normalize();
            vectorB = edgeB.ComputeDerivatives(0).BasisX.Normalize();
         }
         else if (A_1.IsAlmostEqualTo(B_1))
         {
            vectorA = edgeA.ComputeDerivatives(1).BasisX.Normalize();
            vectorB = edgeB.ComputeDerivatives(1).BasisX.Normalize();
         }

         if (A_1.IsAlmostEqualTo(B_0) || A_0.IsAlmostEqualTo(B_1)) vectorA = vectorA.Negate();

         if (null == vectorA || null == vectorB)
         {
            return 0d;
         }
         double angle = Math.Acos(vectorA.DotProduct(vectorB));
         return angle;
      }

      /// <summary>
      /// Get all the parameters and store them into a list
      /// </summary>
      /// <param name="familyInstance">The instance of a curtain panel</param>
      /// <returns>A list containing all the required parameters</returns>
      private InstParameters GetParams(FamilyInstance familyInstance)
      {
         InstParameters iParams = new InstParameters();
         Parameter L1 = familyInstance.get_Parameter("Length1");
         Parameter L2 = familyInstance.get_Parameter("Length2");
         Parameter L3 = familyInstance.get_Parameter("Length3");
         Parameter L4 = familyInstance.get_Parameter("Length4");
         Parameter A1 = familyInstance.get_Parameter("Angle1");
         Parameter A2 = familyInstance.get_Parameter("Angle2");
         Parameter A3 = familyInstance.get_Parameter("Angle3");
         Parameter A4 = familyInstance.get_Parameter("Angle4");

         if (L1 == null || L2 == null || L3 == null || L4 == null || A1 == null || A2 == null || A3 == null || A4 == null)
         {
            string errorstring = "Panel family: " + familyInstance.Id.IntegerValue + " '" + familyInstance.Symbol.Family.Name + "' must have instance parameters Length1, Length2, Length3, Length4, Angle1, Angle2, Angle3, and Angle4";
            TaskDialog.Show("Revit", errorstring);
            //   throw new ArgumentException(errorstring);
         }

         iParams["Length1"] = L1;
         iParams["Length2"] = L2;
         iParams["Length3"] = L3;
         iParams["Length4"] = L4;
         iParams["Angle1"] = A1;
         iParams["Angle2"] = A2;
         iParams["Angle3"] = A3;
         iParams["Angle4"] = A4;

         return iParams;
      }

      protected List<T> GetElements<T>() where T : Element
      {
         List<T> returns = new List<T>();
         FilteredElementCollector collector = new FilteredElementCollector(m_doc);
         ICollection<Element> founds = collector.OfClass(typeof(T)).ToElements();
         foreach (Element elem in founds)
         {
            returns.Add(elem as T);
         }
         return returns;
      }

   }

   /// <summary>
   /// This class contains a dictionary which stores the parameter and parameter name pairs
   /// </summary>
   class InstParameters
   {
      private Dictionary<string, Parameter> m_parameters = new Dictionary<string, Parameter>(8);

      /// <summary>
      /// Get/Set the parameter by its name
      /// </summary>
      /// <param name="index">the name of the parameter</param>
      /// <returns>The parameter which matches the name</returns>
      public Parameter this[string index]
      {
         get
         {
            return m_parameters[index];
         }
         set
         {
            m_parameters[index] = value;
         }
      }
   }
}
