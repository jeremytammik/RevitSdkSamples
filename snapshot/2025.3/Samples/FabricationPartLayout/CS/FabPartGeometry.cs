//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

using TaskDialog = Autodesk.Revit.UI.TaskDialog;
using TaskDialogIcon = Autodesk.Revit.UI.TaskDialogIcon;

namespace Revit.SDK.Samples.FabricationPartLayout.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class FabPartGeometry : IExternalCommand
   {
      private IList<Mesh> getMeshes(GeometryElement ge)
      {
         IList<Mesh> rv = new List<Mesh>();
         if (ge != null)
         {
            foreach (GeometryObject g in ge)
            {
               GeometryInstance i = g as GeometryInstance;
               if (i != null)
               {
                  GeometryElement ge2 = i.GetInstanceGeometry();
                  if (ge2 != null)
                     rv = rv.Concat(getMeshes(ge2)).ToList();
               }
               else
               {
                  Mesh mesh = g as Mesh;
                  if (mesh != null && mesh.Vertices.Count > 0)
                     rv.Add(mesh);
               }
            }
         }
         return rv;
      }

      private bool exportMesh(String filename, Mesh mesh)
      {
         bool bSaved = false;

         try
         {
            StreamWriter sout = new StreamWriter(filename, false);
            sout.WriteLine("P1X, P1Y, P1Z, P2X, P2Y, P2Z, P3X, P3Y, P3Z");
            for (int tlp = 0; tlp < mesh.NumTriangles; tlp++)
            {
               MeshTriangle tri = mesh.get_Triangle(tlp);

               XYZ p1 = mesh.Vertices[(int)tri.get_Index(0)];
               XYZ p2 = mesh.Vertices[(int)tri.get_Index(1)];
               XYZ p3 = mesh.Vertices[(int)tri.get_Index(2)];

               String tstr = String.Format("{0:0.000}, {1:0.000}, {2:0.000}, {3:0.000}, {4:0.000}, {5:0.000}, {6:0.000}, {7:0.000}, {8:0.000}", new object[] { p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z, p3.X, p3.Y, p3.Z });
               sout.WriteLine(tstr);
            }
            sout.Close();

            bSaved = true;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "Unable to write file", MessageBoxButtons.OK, MessageBoxIcon.Error);
         }

         return bSaved;
      }


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
      public virtual Result Execute(ExternalCommandData commandData
          , ref string message, ElementSet elements)
      {
         try
         {
            // check user selection
            var uidoc = commandData.Application.ActiveUIDocument;
            var doc = uidoc.Document;

            ISet<ElementId> parts = null;

            using (Transaction tr = new Transaction(doc, "Optimise Preselection"))
            {
               tr.Start();
               ICollection<ElementId> selElems = uidoc.Selection.GetElementIds();
               if (selElems.Count > 0)
               {
                  parts = new HashSet<ElementId>(selElems);
               }

               tr.Commit();
            }

            if (parts == null)
            {
               MessageBox.Show("Select parts to export.");
               return Result.Failed;
            }

            var callingFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var saveAsDlg = new FileSaveDialog("CSV Files (*.csv)|*.csv");

            saveAsDlg.InitialFileName = callingFolder + "\\geomExport";
            saveAsDlg.Title = "Save Part Geometry As";
            var result = saveAsDlg.Show();

            if (result == ItemSelectionDialogResult.Canceled)
               return Result.Cancelled;

            string filename = ModelPathUtils.ConvertModelPathToUserVisiblePath(saveAsDlg.GetSelectedModelPath());
            string ext = Path.GetExtension(filename);
            filename = Path.GetFileNameWithoutExtension(filename);

            int partcount = 1, exported = 0;

            foreach (ElementId eid in parts)
            {
               // get all rods and kist with rods 
               FabricationPart part = doc.GetElement(eid) as FabricationPart;
               if (part != null)
               {
                  Options options = new Options();
                  options.DetailLevel = ViewDetailLevel.Coarse;

                  IList<Mesh> main = getMeshes(part.get_Geometry(options));
                  IList<Mesh> ins = getMeshes(part.GetInsulationLiningGeometry());

                  int mlp = 0;
                  foreach (Mesh mesh in main)
                  {
                     String file = String.Concat( filename, partcount.ToString(), "-main-", (++mlp).ToString(), ext);
                     if (exportMesh(file, mesh))
                        exported++;
                  }

                  int ilp = 0;
                  foreach (Mesh mesh in ins)
                  {
                     String file = String.Concat(filename, partcount.ToString(), "-ins-", (++ilp).ToString(), ext);
                     if (exportMesh(file, mesh))
                        exported++;
                  }
               }
               partcount++;
            }

            String res = (exported > 0) ? "Export was successful" : "Nothing was exported";
            String manywritten = String.Format("{0} Parts were exported", exported);

            TaskDialog td = new TaskDialog("Export Part Mesh Geometry")
            {
               MainIcon = TaskDialogIcon.TaskDialogIconInformation,
               TitleAutoPrefix = false,
               MainInstruction = res,
               MainContent = manywritten,
               AllowCancellation = false,
               CommonButtons = TaskDialogCommonButtons.Ok
            };

            td.Show();

            return Result.Succeeded;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
   }
}
