//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using Autodesk.Revit;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Revit.SDK.Samples.Ribbon.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication,
    /// show user how to create RibbonItems by API in Revit.
    /// we add two RibbonPanel here:
    /// 1. the first panel contains one single PushButton and three stackable buttons to wall commands (create, delete and move) ;
    /// 2. the second panel contains two stackable PushButtons to popup the information of customer RibbonPanels and the Ribbon design guidelines document.
    /// </summary>
    public class RibbonSample : IExternalApplication
    {
        // ExternalCommands assembly path
        static string AddInPath            = typeof(RibbonSample).Assembly.Location.Replace("Ribbon.dll", "AddInCommands.dll");
        // Button icons directory
        static string ButtonIconsFolder = Path.GetDirectoryName(typeof(RibbonSample).Assembly.Location);

        #region IExternalApplication Members
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
        /// If Failed is returned then Revit should inform the user that the external application 
        /// failed to load and the release the internal reference.</returns>
        public IExternalApplication.Result OnStartup(ControlledApplication application)
        {
            try
            {
                // create customer Ribbon Items
                CreateRibbonSamplePanel(application);
                CreateRibbonInfosPanel(application);

                return IExternalApplication.Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ribbon Sample");

                return IExternalApplication.Result.Failed;
            }
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
        /// If Failed is returned then the Revit user should be warned of the failure of the external 
        /// application to shut down correctly.</returns>
        public IExternalApplication.Result OnShutdown(ControlledApplication application)
        {
            return IExternalApplication.Result.Succeeded;
        }
        #endregion

        /// <summary>
        /// This method is used to create RibbonSamppanel, and add wall related command buttons to it.
        /// 1. the Create Wall is a single PushButton.
        /// 2. Delete Last Wall, Delete All Walls and Move Wall (pulldown) are added as stackable buttons.
        /// 3. Move Wall is a stackable PulldownButton which contains two sub-PushButtons.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        private void CreateRibbonSamplePanel(ControlledApplication application)
        {
            // create a Ribbon panel which contains three stackable buttons and one single push button.
            string firstPanelName                  = "Ribbon Sample";
            RibbonPanel ribbonSamplePanel = application.CreateRibbonPanel(firstPanelName);

            # region create one single large push button.

            // use AddPushButton method to add one single large PushButton
            PushButton createWallBut  = ribbonSamplePanel.AddPushButton("createWall", "Create Wall",
                                                       AddInPath, "Revit.SDK.Samples.Ribbon.CS.CreateWall");
            createWallBut.ToolTip        = "Create a wall on level 1.";
            createWallBut.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "CreateWallLarge.png"), UriKind.Absolute));
            createWallBut.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "CreateWall.png"), UriKind.Absolute));


            #endregion

            // add one vertical separation line in the panel.
            ribbonSamplePanel.AddSeparator();

            # region create stackable buttons

            // prepare data for creating stackable buttons
            PushButtonData delLastWallButtonData = new PushButtonData("deleteLastWall", "Delete Last Wall",
                                                                         AddInPath, "Revit.SDK.Samples.Ribbon.CS.DeleteLastWall");
            delLastWallButtonData.ToolTip              = "Delete the last wall created by the Create Wall tool.";
            delLastWallButtonData.Image               = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "DeleteWall.png"), UriKind.Absolute));

            PushButtonData deleteWallsButtonData = new PushButtonData("deleteWalls", "Delete Walls", AddInPath, "Revit.SDK.Samples.Ribbon.CS.DeleteWalls");
            deleteWallsButtonData.ToolTip              = "Delete all the walls created by the Create Wall tool.";
            deleteWallsButtonData.Image               = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "DeleteWalls.png"), UriKind.Absolute));

            PulldownButtonData moveWallsButtonData = new PulldownButtonData("moveWalls", "Move Walls");
            moveWallsButtonData.ToolTip                    = "Move all the walls in X or Y direction";
            moveWallsButtonData.Image                     = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "MoveWalls.png"), UriKind.Absolute));

            // add stackable buttons.
            List<RibbonItem> ribbonItems = ribbonSamplePanel.AddStackedButtons(delLastWallButtonData, deleteWallsButtonData, moveWallsButtonData);


            // add two push buttons as sub-items of the moveWalls PulldownButton. 
            PulldownButton moveWallItem = ribbonItems[2] as PulldownButton;

            PushButton moveX  = moveWallItem.AddItem("X Direction", AddInPath, "Revit.SDK.Samples.Ribbon.CS.XMoveWalls");
            moveX.ToolTip        = "move all walls 10 feet in X direction.";
            moveX.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "MoveWallsXLarge.png"), UriKind.Absolute));

            PushButton moveY  = moveWallItem.AddItem("Y Direction", AddInPath, "Revit.SDK.Samples.Ribbon.CS.YMoveWalls");
            moveY.ToolTip        = "move all walls 10 feet in Y direction.";
            moveY.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "MoveWallsYLarge.png"), UriKind.Absolute));

            # endregion
        }

        /// <summary>
        /// This method is used to Create Ribbon Information panel with two stackable buttons.
        /// 1. The first stackable button is used to output customer RibbonItems information.
        /// 2. The second stackable button is used to show users the Ribbon design guidelines document.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        private void CreateRibbonInfosPanel(ControlledApplication application)
        {
            // Create a RibbonPanel
            string thirdPanelName            = "Add-in Panel Info";
            RibbonPanel ribbonPanellInfo = application.CreateRibbonPanel(thirdPanelName);

             // prepare data for creating stackable buttons
            PushButtonData panelInfoButtonData = new PushButtonData("outputlInfo", "Ribbon Panel Info", AddInPath, "Revit.SDK.Samples.Ribbon.CS.OutputPanelInfo");
            panelInfoButtonData.ToolTip              = "Output info of all the Add-In RibbonPanels.";
            panelInfoButtonData.Image               = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "OutputAddInPanelInfo.png"), UriKind.Absolute));
            
            PushButtonData guidLinesButtonData = new PushButtonData("designGuidLines", "Design Guidelines", AddInPath, "Revit.SDK.Samples.Ribbon.CS.DesignGuidelines");
            guidLinesButtonData.ToolTip              = "Open the Ribbon design guidelines document.";
            guidLinesButtonData.Image               = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "DesignGuidelines.png"), UriKind.Absolute));

            // Create stackable Buttons
            ribbonPanellInfo.AddStackedButtons(panelInfoButtonData, guidLinesButtonData);
        }

    }
}
