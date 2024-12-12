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
using System.Linq;
using System.Text;
using Autodesk.Revit;
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;

namespace Revit.SDK.Samples.Ribbon.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication,
    /// show user how to create RibbonItems by API in Revit.
    /// we add one RibbonPanel:
    /// 1. contains a SplitButton for user to create Non-Structural or Structural Wall;
    /// 2. contains a StackedButton which is consisted with one PushButton and two Comboboxes, 
    /// PushButton is used to reset all the RibbonItem, Comboboxes are use to select Level and WallShape
    /// 3. contains a RadioButtonGroup for user to select WallType.
    /// 4. Adds a Slide-Out Panel to existing panel with following functionalities:
    /// 5. a text box is added to set mark for new wall, mark is a instance parameter for wall, 
    /// Eg: if user set text as "wall", then Mark for each new wall will be "wall1", "wall2", "wall3"....
    /// 6. a StackedButton which consisted of a PushButton (delete all the walls) and a PulldownButton (move all the walls in X or Y direction)
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class Ribbon : IExternalApplication
    {
        // ExternalCommands assembly path
        static string AddInPath = typeof(Ribbon).Assembly.Location;
        // Button icons directory
        static string ButtonIconsFolder = Path.GetDirectoryName(AddInPath);
        // uiApplication
        static UIApplication uiApplication = null;

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
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            try
            {
                // create customer Ribbon Items
                CreateRibbonSamplePanel(application);

                return Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ribbon Sample");

                return Autodesk.Revit.UI.Result.Failed;
            }
        }

        /// <summary>
        /// Implement this method to implement the external application which should be called when 
        /// Revit is about to exit, Any documents must have been closed before this method is called.
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        /// <returns>Return the status of the external application. 
        /// A result of Succeeded means that the external application successfully shutdown. 
        /// Cancelled can be used to signify that the user cancelled the external operation at 
        /// some point.
        /// If Failed is returned then the Revit user should be warned of the failure of the external 
        /// application to shut down correctly.</returns>
        public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
        {
            //remove events
            List<RibbonPanel> myPanels = application.GetRibbonPanels();
            Autodesk.Revit.UI.ComboBox comboboxLevel = (Autodesk.Revit.UI.ComboBox)(myPanels[0].GetItems()[2]);
            application.ControlledApplication.DocumentCreated -= new EventHandler<
               Autodesk.Revit.DB.Events.DocumentCreatedEventArgs>(DocumentCreated);
            Autodesk.Revit.UI.TextBox textBox = myPanels[0].GetItems()[5] as Autodesk.Revit.UI.TextBox;
            textBox.EnterPressed -= new EventHandler<
               Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs>(SetTextBoxValue);

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion

        /// <summary>
        /// This method is used to create RibbonSample panel, and add wall related command buttons to it:
        /// 1. contains a SplitButton for user to create Non-Structural or Structural Wall;
        /// 2. contains a StackedBotton which is consisted with one PushButton and two Comboboxes, 
        /// PushButon is used to reset all the RibbonItem, Comboboxes are use to select Level and WallShape
        /// 3. contains a RadioButtonGroup for user to select WallType.
        /// 4. Adds a Slide-Out Panel to existing panel with following functionalities:
        /// 5. a text box is added to set mark for new wall, mark is a instance parameter for wall, 
        /// Eg: if user set text as "wall", then Mark for each new wall will be "wall1", "wall2", "wall3"....
        /// 6. a StackedButton which consisted of a PushButton (delete all the walls) and a PulldownButton (move all the walls in X or Y direction)
        /// </summary>
        /// <param name="application">An object that is passed to the external application 
        /// which contains the controlled application.</param>
        private void CreateRibbonSamplePanel(UIControlledApplication application)
        {
            // create a Ribbon panel which contains three stackable buttons and one single push button.
            string firstPanelName = "Ribbon Sample";
            RibbonPanel ribbonSamplePanel = application.CreateRibbonPanel(firstPanelName);

            #region Create a SplitButton for user to create Non-Structural or Structural Wall
            SplitButtonData splitButtonData = new SplitButtonData("NewWallSplit", "Create Wall");
            SplitButton splitButton = ribbonSamplePanel.AddItem(splitButtonData) as SplitButton;
            PushButton pushButton = splitButton.AddPushButton(new PushButtonData("WallPush", "Wall", AddInPath, "Revit.SDK.Samples.Ribbon.CS.CreateWall"));
            pushButton.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "CreateWall.png"), UriKind.Absolute));
            pushButton.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "CreateWall-S.png"), UriKind.Absolute));
            pushButton.ToolTip = "Creates a partition wall in the building model.";
            pushButton.ToolTipImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "CreateWallTooltip.bmp"), UriKind.Absolute));
            pushButton = splitButton.AddPushButton(new PushButtonData("StrWallPush", "Structure Wall", AddInPath, "Revit.SDK.Samples.Ribbon.CS.CreateStructureWall"));
            pushButton.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "StrcturalWall.png"), UriKind.Absolute));
            pushButton.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "StrcturalWall-S.png"), UriKind.Absolute));
            #endregion

            ribbonSamplePanel.AddSeparator();

            #region Add a StackedButton which is consisted of one PushButton and two Comboboxes
            PushButtonData pushButtonData = new PushButtonData("Reset", "Reset", AddInPath, "Revit.SDK.Samples.Ribbon.CS.ResetSetting");
            ComboBoxData comboBoxDataLevel = new ComboBoxData("LevelsSelector");
            ComboBoxData comboBoxDataShape = new ComboBoxData("WallShapeComboBox");
            IList<RibbonItem> ribbonItemsStacked = ribbonSamplePanel.AddStackedItems(pushButtonData, comboBoxDataLevel, comboBoxDataShape);
            ((PushButton)(ribbonItemsStacked[0])).Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Reset.png"), UriKind.Absolute));
            //Add options to WallShapeComboBox
            Autodesk.Revit.UI.ComboBox comboboxWallShape = (Autodesk.Revit.UI.ComboBox)(ribbonItemsStacked[2]);
            ComboBoxMemberData comboBoxMemberData = new ComboBoxMemberData("RectangleWall", "RectangleWall");
            ComboBoxMember comboboxMember = comboboxWallShape.AddItem(comboBoxMemberData);
            comboboxMember.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "RectangleWall.png"), UriKind.Absolute));
            comboBoxMemberData = new ComboBoxMemberData("CircleWall", "CircleWall");
            comboboxMember = comboboxWallShape.AddItem(comboBoxMemberData);
            comboboxMember.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "CircleWall.png"), UriKind.Absolute));
            comboBoxMemberData = new ComboBoxMemberData("TriangleWall", "TriangleWall");
            comboboxMember = comboboxWallShape.AddItem(comboBoxMemberData);
            comboboxMember.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "TriangleWall.png"), UriKind.Absolute));
            comboBoxMemberData = new ComboBoxMemberData("SquareWall", "SquareWall");
            comboboxMember = comboboxWallShape.AddItem(comboBoxMemberData);
            comboboxMember.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "SquareWall.png"), UriKind.Absolute));
            #endregion

            ribbonSamplePanel.AddSeparator();

            #region Add a RadioButtonGroup for user to select WallType
            RadioButtonGroupData radioButtonGroupData = new RadioButtonGroupData("WallTypeSelector");
            RadioButtonGroup radioButtonGroup = (RadioButtonGroup)(ribbonSamplePanel.AddItem(radioButtonGroupData));
            ToggleButton toggleButton = radioButtonGroup.AddItem(new ToggleButtonData("Generic8", "Generic - 8\"", AddInPath, "Revit.SDK.Samples.Ribbon.CS.Dummy"));
            toggleButton.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Generic8.png"), UriKind.Absolute));
            toggleButton.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "Generic8-S.png"), UriKind.Absolute));
            toggleButton = radioButtonGroup.AddItem(new ToggleButtonData("ExteriorBrick", "Exterior - Brick", AddInPath, "Revit.SDK.Samples.Ribbon.CS.Dummy"));
            toggleButton.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "ExteriorBrick.png"), UriKind.Absolute));
            toggleButton.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "ExteriorBrick-S.png"), UriKind.Absolute));
            #endregion

            //slide-out panel:
            ribbonSamplePanel.AddSlideOut();

            #region add a Text box to set the mark for new wall
            TextBoxData testBoxData = new TextBoxData("WallMark");
            Autodesk.Revit.UI.TextBox textBox = (Autodesk.Revit.UI.TextBox)(ribbonSamplePanel.AddItem(testBoxData));
            textBox.Value = "new wall"; //default wall mark
            textBox.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "WallMark.png"), UriKind.Absolute));
            textBox.ToolTip = "Set the mark for new wall";
            textBox.ShowImageAsButton = true;
            textBox.EnterPressed += new EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs>(SetTextBoxValue);
            #endregion

            ribbonSamplePanel.AddSeparator();

            #region Create a StackedButton which consisted of a PushButton (delete all the walls) and a PulldownButton (move all the walls in X or Y direction)
            PushButtonData deleteWallsButtonData = new PushButtonData("deleteWalls", "Delete Walls", AddInPath, "Revit.SDK.Samples.Ribbon.CS.DeleteWalls");
            deleteWallsButtonData.ToolTip = "Delete all the walls created by the Create Wall tool.";
            deleteWallsButtonData.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "DeleteWalls.png"), UriKind.Absolute));

            PulldownButtonData moveWallsButtonData = new PulldownButtonData("moveWalls", "Move Walls");
            moveWallsButtonData.ToolTip = "Move all the walls in X or Y direction";
            moveWallsButtonData.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "MoveWalls.png"), UriKind.Absolute));

            // create stackable buttons
            IList<RibbonItem> ribbonItems = ribbonSamplePanel.AddStackedItems(deleteWallsButtonData, moveWallsButtonData);

            // add two push buttons as sub-items of the moveWalls PulldownButton. 
            PulldownButton moveWallItem = ribbonItems[1] as PulldownButton;

            PushButton moveX = moveWallItem.AddPushButton(new PushButtonData("XDirection", "X Direction", AddInPath, "Revit.SDK.Samples.Ribbon.CS.XMoveWalls"));
            moveX.ToolTip = "move all walls 10 feet in X direction.";
            moveX.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "MoveWallsXLarge.png"), UriKind.Absolute));

            PushButton moveY = moveWallItem.AddPushButton(new PushButtonData("YDirection", "Y Direction", AddInPath, "Revit.SDK.Samples.Ribbon.CS.YMoveWalls"));
            moveY.ToolTip = "move all walls 10 feet in Y direction.";
            moveY.LargeImage = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "MoveWallsYLarge.png"), UriKind.Absolute));
            #endregion

            ribbonSamplePanel.AddSeparator();

            application.ControlledApplication.DocumentCreated += new EventHandler<Autodesk.Revit.DB.Events.DocumentCreatedEventArgs>(DocumentCreated);
        }

        /// <summary>
        /// Insert Level into ComboBox - LevelsSelector
        /// </summary>
        /// <param name="evnetArgs">Autodesk.Revit.DB.Events.DocumentCreatedEventArgs</param>
        public void DocumentCreated(object sender, Autodesk.Revit.DB.Events.DocumentCreatedEventArgs e)
        {
            uiApplication = new UIApplication(e.Document.Application);
            List<RibbonPanel> myPanels = uiApplication.GetRibbonPanels();

            Autodesk.Revit.UI.ComboBox comboboxLevel = (Autodesk.Revit.UI.ComboBox)(myPanels[0].GetItems()[2]);
            if (null == comboboxLevel) { return; }
            FilteredElementCollector collector = new FilteredElementCollector(uiApplication.ActiveUIDocument.Document);
            ICollection<Element> founds = collector.OfClass(typeof(Level)).ToElements();
            foreach (Element elem in founds)
            {
                Level level = elem as Level;
                ComboBoxMemberData comboBoxMemberData = new ComboBoxMemberData(level.Name, level.Name);
                ComboBoxMember comboboxMember = comboboxLevel.AddItem(comboBoxMemberData);
                comboboxMember.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "LevelsSelector.png"), UriKind.Absolute));
            }
            //refresh level list (in case user created new level after document created)
            comboboxLevel.DropDownOpened += new EventHandler<ComboBoxDropDownOpenedEventArgs>(AddNewLevels);
        }

        /// <summary>
        /// Bind to combobox's DropDownOpened Event, add new levels that created by user.
        /// </summary>
        /// <param name="evnetArgs">Autodesk.Revit.UI.Events.ComboBoxDropDownOpenedEventArgs</param>
        public void AddNewLevels(object sender, ComboBoxDropDownOpenedEventArgs args)
        {
            Autodesk.Revit.UI.ComboBox comboboxLevel = sender as Autodesk.Revit.UI.ComboBox;
            if (null == comboboxLevel) { return; }
            FilteredElementCollector collector = new FilteredElementCollector(uiApplication.ActiveUIDocument.Document);
            ICollection<Element> founds = collector.OfClass(typeof(Level)).ToElements();
            foreach (Element elem in founds)
            {
               Level level = elem as Level;
               bool alreadyContained = false;
               foreach (ComboBoxMember comboboxMember in comboboxLevel.GetItems())
               {
                  if (comboboxMember.Name == level.Name)
                  {
                     alreadyContained = true;
                  }
               }
               if (!alreadyContained)
               {
                  ComboBoxMemberData comboBoxMemberData = new ComboBoxMemberData(level.Name, level.Name);
                  ComboBoxMember comboboxMember = comboboxLevel.AddItem(comboBoxMemberData);
                  comboboxMember.Image = new BitmapImage(new Uri(Path.Combine(ButtonIconsFolder, "LevelsSelector.png"), UriKind.Absolute));
               }
            }
           
        }

        /// <summary>
        /// Bind to text box's EnterPressed Event, show a dialogue tells user value of test box changed.
        /// </summary>
        /// <param name="evnetArgs">Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs</param>
        public void SetTextBoxValue(object sender, TextBoxEnterPressedEventArgs args)
        {
           MessageBox.Show("New wall's mark changed.", "TextBox EnterPressed Event");
        }

    }
}
