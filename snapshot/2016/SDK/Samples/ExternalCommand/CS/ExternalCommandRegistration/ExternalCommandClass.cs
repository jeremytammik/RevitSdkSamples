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
using System.Linq;
using System.Text;
using Autodesk.Revit.UI;
using System.Windows.Forms;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ExternalCommandRegistration.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand, create a wall
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class ExternalCommandCreateWall : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData,
           ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Autodesk.Revit.Creation.Application creApp = uiDoc.Document.Application.Create;
            Autodesk.Revit.Creation.Document creDoc = uiDoc.Document.Create;
            List<Curve> curves = new List<Curve>();
            //create rectangular curve: wall length: 60 , wall width: 40
            Line line1 = Line.CreateBound(new Autodesk.Revit.DB.XYZ(0, 0, 0),
               new Autodesk.Revit.DB.XYZ(0, 60, 0));
            Line line2 = Line.CreateBound(new Autodesk.Revit.DB.XYZ(0, 60, 0),
               new Autodesk.Revit.DB.XYZ(0, 60, 40));
            Line line3 = Line.CreateBound(new Autodesk.Revit.DB.XYZ(0, 60, 40),
               new Autodesk.Revit.DB.XYZ(0, 0, 40));
            Line line4 = Line.CreateBound(new Autodesk.Revit.DB.XYZ(0, 0, 40),
               new Autodesk.Revit.DB.XYZ(0, 0, 0));
            curves.Add(line1);
            curves.Add(line2);
            curves.Add(line3);
            curves.Add(line4);
            //create wall
            Wall.Create(uiDoc.Document, curves, false);

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        #endregion
    };

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand, show a message box
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.ReadOnly)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ExternalCommand3DView : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData,
           ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            TaskDialog.Show("External Command Registration Sample", "Hello, 3D View!");

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        #endregion
    };

    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication, 
    /// show message box when Revit start up and shut down
    /// </summary>
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ExternalApplicationClass : IExternalApplication
    {
        #region IExternalApplication Members

        public Result OnStartup(UIControlledApplication application)
        {
            TaskDialog.Show("External command Registration Sample", "Revit is starting up.");
            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            TaskDialog.Show("External command Registration Sample", "Revit is shutting down.");
            return Result.Succeeded;
        }

        #endregion
    };
}
