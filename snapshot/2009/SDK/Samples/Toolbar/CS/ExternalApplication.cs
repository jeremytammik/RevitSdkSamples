//
// (C) Copyright 2003-2008 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.Toolbar.CS
{
    /// <summary>
    /// Class implements the Revit interface IExternalApplication to create a custom tool bar.
    /// </summary
    class CreateToolbar : IExternalApplication
    {
        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit starts before a file or default template is actually loaded.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully started. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then Revit should inform the user that the external application 
        /// failed to load and the release the internal reference.</returns>
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            try
            {
                // application path
                string path = this.GetType().Assembly.Location;

                // an external command called bu toolbar item
                string dllPath = path.Replace("Toolbar.dll", "ToolbarCommands.dll");

                // image of toolbar
                string imagePath = path.Replace("Toolbar.dll", "Toolbar.bmp");

                #region Create custom toobar

                // create a custom tool bar and set its image path
                Autodesk.Revit.Toolbar toolBar = application.CreateToolbar();
                toolBar.Name = "Custom Toolbar";
                toolBar.Image = imagePath;

                #endregion


                #region Create one BtnRText toolbar item by using menu item

                // Add toolbar item by using menu item, 
                // The toolbar item will call the external command(Commands.dll) which will create a wall in project
                MenuItem menuItem   = application.CreateTopMenu("custom menu");
                MenuItem menuItem1  = menuItem.Append(MenuItem.MenuType.BasicMenu,
                    "Create Wall", dllPath, "Revit.SDK.Samples.Toolbar.CS.CreateWall");

                // add this menu item to create a new toolbar item
                ToolbarItem item1   = toolBar.AddItem(menuItem1);
                item1.ItemType      = ToolbarItem.ToolbarItemType.BtnRText; // the item is a button with rich text
                item1.ItemText      = "Create Wall"; 
                item1.StatusbarTip  = "Create Wall"; 
                item1.ToolTip       = "Create Wall";

                #endregion


                #region Create BtnRText toolbar item by using specified assembly and class name

                // Add the second button will delete all walls in current project. 
                // Use Toolbar.AddItem(String, String) to add.
                ToolbarItem item2   = toolBar.AddItem(dllPath, "Revit.SDK.Samples.Toolbar.CS.DeleteWalls");
                item2.ItemType      = ToolbarItem.ToolbarItemType.BtnRText; // button with rich text
                item2.ItemText      = "Delete Walls";
                item2.StatusbarTip  = "Delete Walls";
                item2.ToolTip       = "Delete Walls";

                #endregion


                #region Create a separator toolbar item

                // Add a separator between the second button and the third button 
                ToolbarItem item3   = toolBar.AddItem("sep", "sep");
                item3.ItemType      = ToolbarItem.ToolbarItemType.BtnSeparator; // separator button

                #endregion


                #region Create a standard toolbar item

                // The button which pops up a dialog box to show some information about Custom Toolbar; 
                ToolbarItem item4   = toolBar.AddItem(dllPath, "Revit.SDK.Samples.Toolbar.CS.AboutAPIToolbar");
                item4.ItemType      = ToolbarItem.ToolbarItemType.BtnStd; // standard item, image only.
                item4.StatusbarTip  = "Help of Custom Toolbar";
                item4.ToolTip       = "Help of Custom Toolbar";

                #endregion
            }
            catch (System.Exception)
            {
                return IExternalApplication.Result.Failed;
            }
           
            return IExternalApplication.Result.Succeeded;
        }

        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit is about to exit,Any documents must have been closed before this method is called.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully shutdown. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If false is returned then the Revit user should be warned of the failure of the external 
        /// application to shut down correctly.</returns>
        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            return IExternalApplication.Result.Succeeded;
        }
    }
}
