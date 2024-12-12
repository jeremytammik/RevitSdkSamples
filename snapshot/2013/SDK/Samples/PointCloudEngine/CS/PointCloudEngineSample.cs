//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.PointClouds;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.CS.PointCloudEngine
{
    /// <summary>
    /// ExternalApplication used to register the point cloud engines managed by this sample.
    /// </summary>
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class PointCloudTestApplication: IExternalApplication
    { 
        #region IExternalApplication Members

      /// <summary>
      /// The implementation of IExternalApplication.OnStartup()
      /// </summary>
      /// <param name="application">The Revit application.</param>
      /// <returns>Result.Succeeded</returns>
      public Result OnStartup(UIControlledApplication application)
      {
          try
          {
              // Register point cloud engines for the sample.

              // Predefined engine (non-randomized)
              IPointCloudEngine engine = new BasicPointCloudEngine(PointCloudEngineType.Predefined);
              PointCloudEngineRegistry.RegisterPointCloudEngine("apipc", engine, false);

              // Predefined engine with randomized points at the cell borders
              engine = new BasicPointCloudEngine(PointCloudEngineType.RandomizedPoints);
              PointCloudEngineRegistry.RegisterPointCloudEngine("apipcr", engine, false);

              // XML-based point cloud definition
              engine = new BasicPointCloudEngine(PointCloudEngineType.FileBased);
              PointCloudEngineRegistry.RegisterPointCloudEngine("xml", engine, true);

              // Create user interface for accessing predefined point clouds
              RibbonPanel panel = application.CreateRibbonPanel("Point cloud testing");

              System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

              panel.AddItem(new PushButtonData("AddPredefinedInstance", 
                                                "Add predefined instance", 
                                                assembly.Location,
                                                "Revit.SDK.Samples.CS.PointCloudEngine.AddPredefinedInstanceCommand"));
              panel.AddSeparator();
              
              panel.AddItem(new PushButtonData("AddRandomizedInstance",
                                                "Add randomized instance",
                                                assembly.Location,
                                                "Revit.SDK.Samples.CS.PointCloudEngine.AddRandomizedInstanceCommand"));
              panel.AddSeparator();
                
              panel.AddItem(new PushButtonData("AddTransformedInstance",
                                                "Add randomized instance\nat transform",
                                                assembly.Location,
                                                "Revit.SDK.Samples.CS.PointCloudEngine.AddTransformedInstanceCommand"));
              panel.AddSeparator();

              panel.AddItem(new PushButtonData("SerializePointCloud",
                                                "Serialize point cloud (utility)",
                                                assembly.Location,
                                                "Revit.SDK.Samples.CS.PointCloudEngine.SerializePredefinedPointCloud"));
          }
          catch (Exception e)
          {
              TaskDialog.Show("Exception from OnStartup", e.ToString());
          }
 
          return Result.Succeeded;
      }

      /// <summary>
      /// The implementation of IExternalApplication.OnShutdown()
      /// </summary>
      /// <param name="application">The Revit application.</param>
      /// <returns>Result.Succeeded.</returns>
      public Result OnShutdown(UIControlledApplication application)
      {
          return Result.Succeeded;
      }

      #endregion
    }

     /// <summary>
     /// ExternalCommand to add a predefined point cloud.
     /// </summary>
     [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
     public class AddPredefinedInstanceCommand : AddInstanceCommandBase, IExternalCommand
     {
         #region IExternalCommand Members
         /// <summary>
         /// The implementation for IExternalCommand.Execute()
         /// </summary>
         /// <param name="commandData">The Revit command data.</param>
         /// <param name="message">The error message (ignored).</param>
         /// <param name="elements">The elements to display in the failure dialog (ignored).</param>
         /// <returns>Result.Succeeded</returns>
         public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
         {
             Document doc = commandData.View.Document;

             AddInstance(doc, "apipc", "", Transform.Identity);

             return Result.Succeeded;
         }

         #endregion
     }

     /// <summary>
     /// ExternalCommand to a predefined point cloud with randomized points.
     /// </summary>
     [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
     public class AddRandomizedInstanceCommand : AddInstanceCommandBase, IExternalCommand
     {
         #region IExternalCommand Members
         /// <summary>
         /// The implementation for IExternalCommand.Execute()
         /// </summary>
         /// <param name="commandData">The Revit command data.</param>
         /// <param name="message">The error message (ignored).</param>
         /// <param name="elements">The elements to display in the failure dialog (ignored).</param>
         /// <returns>Result.Succeeded</returns>
         public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
         {
             Document doc = commandData.View.Document;

             AddInstance(doc, "apipcr", "", Transform.Identity);

             return Result.Succeeded;
         }

         #endregion
     }

     /// <summary>
     /// ExternalCommand to add a predefined point cloud at a non-default transform.
     /// </summary>
     [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
     public class AddTransformedInstanceCommand : AddInstanceCommandBase, IExternalCommand
     {
         #region IExternalCommand Members
         /// <summary>
         /// The implementation for IExternalCommand.Execute()
         /// </summary>
         /// <param name="commandData">The Revit command data.</param>
         /// <param name="message">The error message (ignored).</param>
         /// <param name="elements">The elements to display in the failure dialog (ignored).</param>
         /// <returns>Result.Succeeded</returns>
         public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
         {
             Document doc = commandData.View.Document;

             Transform trf = Transform.get_Rotation(new XYZ(10, 5, 0), XYZ.BasisZ, Math.PI / 6.0);
             AddInstance(doc, "apipcr", "", trf);

             return Result.Succeeded;
         }

         #endregion
     }

     /// <summary>
     /// Base class for ExternalCommands used to add point cloud instances programmatically.
     /// </summary>
     public class AddInstanceCommandBase
     {
         /// <summary>
         /// Adds a point cloud instance programmatically.
         /// </summary>
         /// <param name="doc">The document.</param>
         /// <param name="engineType">The engine identifier string.</param>
         /// <param name="identifier">The identifier for the particular point cloud.</param>
         /// <param name="trf">The transform to apply to the new point cloud instance.</param>
         public void AddInstance(Document doc, String engineType, String identifier, Transform trf)
         {
             Transaction t = new Transaction(doc, "Create PC instance");
             t.Start();
             PointCloudType type = PointCloudType.Create(doc, engineType, identifier);
             PointCloudInstance.Create(doc, type.Id, trf);
             t.Commit();
         }
     }

     /// <summary>
     /// Utility ExternalCommand to take a predefined point cloud and write the corresponding XML for it to disk.
     /// </summary>
     [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
     public class SerializePredefinedPointCloud : AddInstanceCommandBase, IExternalCommand
     {
         #region IExternalCommand Members
         /// <summary>
         /// The implementation for IExternalCommand.Execute()
         /// </summary>
         /// <param name="commandData">The Revit command data.</param>
         /// <param name="message">The error message (ignored).</param>
         /// <param name="elements">The elements to display in the failure dialog (ignored).</param>
         /// <returns>Result.Succeeded</returns>
         public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
         {
             PredefinedPointCloud cloud = new PredefinedPointCloud("dummy");

             XDocument doc = new XDocument();
             XElement root = new XElement("PointCloud");
             cloud.SerializeObjectData(root);
             doc.Add(root);

             TextWriter writer = new StreamWriter(@"c:\serializedPC.xml");
             doc.WriteTo(new XmlTextWriter(writer));

             writer.Close();

             return Result.Succeeded;
         }

         #endregion
     }


}
