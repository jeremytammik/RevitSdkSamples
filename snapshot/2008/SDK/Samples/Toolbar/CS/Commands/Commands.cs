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

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;

namespace Revit.SDK.Samples.Toolbar.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,create a wall
    /// </summary>
    public class CreateWall : IExternalCommand
    {
        #region IExternalCommand Members Implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
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
        public IExternalCommand.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            Application app                            = revit.Application;
            Autodesk.Revit.Creation.Application creApp = app.Create;
            Autodesk.Revit.Creation.Document creDoc    = app.ActiveDocument.Create;
            
            // prepare the wall base line
            XYZ startPoint= new XYZ(0,0,0);
            XYZ endPoint = new XYZ(50,50,0);
            Line line = creApp.NewLine (ref startPoint, ref endPoint, true);

            // prepare the level where the wall will be located
            ElementIterator iter = app.ActiveDocument.Elements;
            while (iter.MoveNext())
            {
                Level level1 = iter.Current as Level;
                if (null == level1)
                {
                    continue;
                }

                if (!level1.Name.Equals("Level 1"))
                {
                    continue;
                }

                // create a wall in level1
                Wall newWall = creDoc.NewWall(line, level1, true);
            }
            //creDoc.NewWall(line, false);
            return IExternalCommand.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,delete the wall
    /// </summary>
    public class DeleteWalls : IExternalCommand
    {
        #region IExternalCommand Members Implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
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
        public IExternalCommand.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            Document doc = revit.Application.ActiveDocument;

            ElementIterator iter = doc.Elements;
            while (iter.MoveNext())
            {
                Wall wall = iter.Current as Wall;
                if (null == wall)
                {
                    continue;
                }

                // delete all the walls
                doc.Delete(wall);
            }
            //creDoc.NewWall(line, false);
            return IExternalCommand.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    public class AboutAPIToolbar : IExternalCommand
    {
        public Dictionary<string, string> AboutToolbar
        {
            get
            {
                return m_aboutToolbar;
            }
        }

        public Dictionary<string, string> DifToolbarItemUsage
        {
            get
            {
                return m_difToolbarItemUsage;
            }
        }

        static AboutAPIToolbar()
        {
            string backdrop       = "Maybe you need a manner in which you can add your own designed Toolbar into Revit." + 
                                    "Formerly you can only add your applications to the External Tools' submenu. " +
                                    "But now your applications can integrate seamlessly into the platform and " + 
                                    "make them look like actually created by the Revit Team.";
            string createToolbar  = "Firstly, use Application.CreateToolbar to create a Toolbar.\r\n" +
                                    "Then, use Toolbar.AddItem to add a ToolbarItem button which corresponds to an ExternalCommand. \r\n" +
                                    "ToolbarItem button has three types which will be introduced later.";
            string getToolbarItem = "Firstly, use Application.GetToolbars to return a ToolbarArray which includes all the custom Toolbars.\r\n" +
                                    "Secondly, you can get each custom Toolbar from the ToolbarArray.\r\n" +
                                    "Then, use Toolbar.ToolbarItems to return a ToolbarItemArray which returns all the custom ToolbarItems in this Toolbar.\r\n" +
                                    "Finally, you can get each ToolbarItem in this ToolbarItemArray";
            string others         = "Every button should have a corresponding command to execute. " +
                                    "And the command should implement the interface of IExternalCommand.\r\n" +
                                    "Toolbar and ToolbarItem should be added when Revit starts up, so we must add Toolbar and ToolbarItem in External Application";
            m_aboutToolbar.Add("Backdrop of API", backdrop);
            m_aboutToolbar.Add("How to create custom Toolbar", createToolbar);
            m_aboutToolbar.Add("Get existent toolbarItem", getToolbarItem);
            m_aboutToolbar.Add("Others", others);

            string stdBtn   = "Standard toolbar type\r\n" +
                              "Usage:  button with image\r\n" +
                              "Create: Toolbar.AddItem(MenuItem) or Toolbar.AddItem(string pAssemblyName, string pClassName)";
            string textBtn =  "Rich text toolbar type\r\n" +
                              "Usage:  button with both text and image\r\n" +
                              "Create: Toolbar.AddItem(MenuItem) or Toolbar.AddItem(string pAssemblyName, string pClassName)" +
                              "Then, set ToolbarItem.ItemType with ToolbarItem.ToolbarItemType.BtnRText and set ToolbarItem.ItemText";
            string separBtn = "Button separator\r\nUsage: a separator line\r\nCreate: Toolbar.AddItem(nullptr)";
            m_difToolbarItemUsage.Add(ToolbarItem.ToolbarItemType.BtnStd.ToString(), stdBtn);
            m_difToolbarItemUsage.Add(ToolbarItem.ToolbarItemType.BtnRText.ToString(), textBtn);
            m_difToolbarItemUsage.Add(ToolbarItem.ToolbarItemType.BtnSeparator.ToString(), separBtn);
        }

        #region IExternalCommand Members Implementation
        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
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
        public IExternalCommand.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            // show UI
            using (AboutAPIToolbarForm displayForm = new AboutAPIToolbarForm(this))
            {
                displayForm.ShowDialog();
            }

            return IExternalCommand.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation

        private static Dictionary<string, string> m_aboutToolbar = new Dictionary<string, string>();
        private static Dictionary<string, string> m_difToolbarItemUsage = new Dictionary<string, string>();
    }
}
