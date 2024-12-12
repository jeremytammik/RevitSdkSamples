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
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Windows.Forms;

namespace Revit.SDK.Samples.Ribbon.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand, create a file to record all the informations of Add-In ribbons
    /// </summary>
    public class OutputPanelInfo : IExternalCommand
    {
        #region IExternalCommand Members Implementation
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
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string assemLocation = this.GetType().Assembly.Location;
            string assemDirectory = Path.GetDirectoryName(assemLocation);
            string txtPath = Path.Combine(assemDirectory, "RibbonPanelInfo.txt");
            if (File.Exists(txtPath)) File.Delete(txtPath);
            TraceListener txtListener = new TextWriterTraceListener(txtPath);
            Trace.Listeners.Add(txtListener);
            try
            {
                GetRibbonInfo(commandData, ref message, elements);
                return IExternalCommand.Result.Succeeded;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
                message = ex.ToString();
                return IExternalCommand.Result.Failed;
            }
            finally
            {
                Trace.Flush();
                txtListener.Close();
                Trace.Close();
                Trace.Listeners.Remove(txtListener);

                // display the results of the output. 
                Process.Start(txtPath);
            }
        }
        #endregion

        /// <summary>
        /// Get all the information of Add-In RibbonItems
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        private void GetRibbonInfo(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            List<RibbonPanel> panelsAll = commandData.Application.GetRibbonPanels();
            foreach (RibbonPanel panel in panelsAll)
            {
                TestRibbonPanel(panel);
            }
        }

        /// <summary>
        /// Test RibbonPanel's property and write info to txt file
        /// </summary>
        /// <param name="panel"> the RibbonPanel which is used to test </param>
        private void TestRibbonPanel(RibbonPanel panel)
        {
            Trace.WriteLine("--------------------------RibbonPanel------------------------------------------------");
            Trace.WriteLine("Panel.Enabled = " + panel.Enabled.ToString());
            Trace.WriteLine("Panel.IsReadOnly = " + panel.IsReadOnly.ToString());
            Trace.WriteLine("Panel.Name = " + panel.Name);
            Trace.WriteLine("Panel.Title = " + panel.Title);
            Trace.WriteLine("Panel.Visible = " + panel.Visible.ToString());
            Trace.WriteLine("");
            Trace.WriteLine("");
            TestSubItems(panel.Items);
            Trace.WriteLine("------------------------------RibbonPanel End----------------------------------------");
            Trace.WriteLine("");
            Trace.WriteLine("");
        }

        /// <summary>
        /// Test PulldownButton's property and write info to txt file
        /// </summary>
        /// <param name="panel"> the PulldownButton which is used to test </param>
        private void TestPulldownButton(PulldownButton pulldownButton)
        {
            Trace.WriteLine("--------------------PulldownButton----------------------");
            Trace.WriteLine("PulldownButton.Name = " + pulldownButton.Name);
            Trace.WriteLine("PulldownButton.Enabled = " + pulldownButton.Enabled.ToString());
            TracePulldownButtonImage(pulldownButton);
            Trace.WriteLine("PulldownButton.IsReadOnly = " + pulldownButton.IsReadOnly.ToString());
            Trace.WriteLine("PulldownButton.ItemText = " + pulldownButton.ItemText.ToString());
            Trace.WriteLine("PulldownButton.ItemType = " + pulldownButton.ItemType.ToString());
            Trace.WriteLine("PulldownButton.ToolTip = " + pulldownButton.ToolTip);
            Trace.WriteLine("");
            Trace.WriteLine("");
            TestSubItems(pulldownButton.Items);
            Trace.WriteLine("--------------PulldownButton End---------------------------");
            Trace.WriteLine("");
        }

        /// <summary>
        /// Test PushButton's property and write info to txt file
        /// </summary>
        /// <param name="panel"> the PushButton which is used to test </param>
        private void TestPushButton(PushButton pushButton)
        {
            Trace.WriteLine("----------------------PushButton-----------------------");
            Trace.WriteLine("PushButton.Name = " + pushButton.Name);
            Trace.WriteLine("PushButton.AssemblyName = " + Path.GetFileName(pushButton.AssemblyName));
            Trace.WriteLine("PushButton.ClassName = " + pushButton.ClassName);
            Trace.WriteLine("PushButton.Enabled = " + pushButton.Enabled.ToString());
            TracePushButtonImage(pushButton);
            Trace.WriteLine("PushButton.IsReadOnly = " + pushButton.IsReadOnly.ToString());
            Trace.WriteLine("PushButton.ItemText = " + pushButton.ItemText);
            Trace.WriteLine("PushButton.ItemType = " + pushButton.ItemType.ToString());
            Trace.WriteLine("PushButton.ToolTip = " + pushButton.ToolTip);
            Trace.WriteLine("--------------------PushButton End----------------------");
            Trace.WriteLine("");
        }

        /// <summary>
        /// Test each RibbonItems stored in the list and write info to txt file
        /// </summary>
        /// <param name="subItems">
        /// a RibbonItems list which will be tested.
        /// </param>
        private void TestSubItems(List<RibbonItem> subItems)
        {
            foreach (RibbonItem eachitem in subItems)
            {
                if (RibbonItem.RibbonItemType.PulldownButton == eachitem.ItemType)
                { TestPulldownButton(eachitem as PulldownButton); }
                else if (RibbonItem.RibbonItemType.PushButton == eachitem.ItemType)
                { TestPushButton(eachitem as PushButton); }
            }
        }

        /// <summary>
        /// Test Image and LargeImage of PushButton and write info to txt file.
        /// </summary>
        /// <param name="pushButton">
        /// the PushButton whose Image and LargeImage will be tested.
        /// </param>
        private void TracePushButtonImage(PushButton pushButton)
        {
            if (null != pushButton.LargeImage)
            {
                String fullPathPic = pushButton.LargeImage.ToString();
                Trace.WriteLine("PushButton.LargeImage = " + fullPathPic.Split('/')[fullPathPic.Split('/').Length - 1]);
            }
            else
            {
                Trace.WriteLine("PushButton.LargeImage = null");
            }

            if (null != pushButton.Image)
            {
                String fullPathPic = pushButton.Image.ToString();
                Trace.WriteLine("PushButton.Image = " + fullPathPic.Split('/')[fullPathPic.Split('/').Length - 1]);
            }
            else
            {
                Trace.WriteLine("PushButton.Image = null");
            }
        }

        /// <summary>
        /// Test Image and LargeImage of  PulldownButton and write info to txt file.
        /// </summary>
        /// <param name="pulldownButton">
        ///  the PulldownButton whose Image and LargeImage will be tested.
        ///  </param>
        private void TracePulldownButtonImage(PulldownButton pulldownButton)
        {
            if (null != pulldownButton.LargeImage)
            {
                String fullPathPic = pulldownButton.LargeImage.ToString();
                Trace.WriteLine("PulldownButton.LargeImage = " + fullPathPic.Split('/')[fullPathPic.Split('/').Length - 1]);
            }
            else
            {
                Trace.WriteLine("PulldownButton.LargeImage = null");
            }

            if (null != pulldownButton.Image)
            {
                String fullPathPic = pulldownButton.Image.ToString();
                Trace.WriteLine("PulldownButton.Image = " + fullPathPic.Split('/')[fullPathPic.Split('/').Length - 1]);
            }
            else
            {
                Trace.WriteLine("pulldownButton.Image = null");
            }
        }
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand, open the Ribbon design guidelines document.
    /// </summary>
    public class DesignGuidelines : IExternalCommand
    {
        const string GuidelinesFileName = "Ribbon design guidelines.pdf";

        #region IExternalCommand Members
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
        public IExternalCommand.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string sampleAssemblyDir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            // the Ribbon design guidelines is located in the root directory of the SDK folder.
            string guidelinesFile = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(sampleAssemblyDir)), GuidelinesFileName);

            if (File.Exists(guidelinesFile))
            {
                // open the document if found
                Process.Start(guidelinesFile);
            }
            else // show a message indicating the name of the guidelines file and that it can be found in the SDK installation folder
            {
                MessageBox.Show(@"Please search for " + GuidelinesFileName + " in SDK installation folder.", "Add-in Panel Info");

                return IExternalCommand.Result.Failed;
            }

            return IExternalCommand.Result.Succeeded;
        }

        #endregion
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,create a wall
    /// </summary>
    public class CreateWall : IExternalCommand
    {
        public static ElementSet CreatedWalls = new ElementSet(); //restore all the walls created by API.

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
            Autodesk.Revit.Application app = revit.Application;
            Autodesk.Revit.Creation.Application creApp = app.Create;
            Autodesk.Revit.Creation.Document creDoc = app.ActiveDocument.Create;

            // prepare the wall base line
            XYZ startPoint = new XYZ(-50 + CreatedWalls.Size * 10, 0, 0);
            XYZ endPoint = new XYZ(0 + CreatedWalls.Size * 10, 50, 0);
            Line line = creApp.NewLine(startPoint, endPoint, true);

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
                if (null != newWall)
                {
                    CreatedWalls.Insert(newWall);
                }
            }

            return IExternalCommand.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand, delete the last wall created by the CreateWall tool.
    /// </summary>
    public class DeleteLastWall : IExternalCommand
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

            // get current last wall created by Create Wall command.
            int wallsSize = CreateWall.CreatedWalls.Size;
            if (0 == wallsSize)
            {
                return IExternalCommand.Result.Cancelled;
            }

            Autodesk.Revit.Element lastWall = null;
            int index = 0;
            ElementSetIterator iter = CreateWall.CreatedWalls.ForwardIterator();
            while (iter.MoveNext())
            {
                index++;
                if (wallsSize == index)
                {
                    lastWall = iter.Current as Autodesk.Revit.Element;
                }
            }

            // delete the last wall created by the CreateWall tool.
            doc.Delete(lastWall);

            // erase the deleted wall for CreatedWall set.
            CreateWall.CreatedWalls.Erase(lastWall);

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

            // delete all the walls which create by RibbonSample
            doc.Delete(CreateWall.CreatedWalls);
            CreateWall.CreatedWalls.Clear();

            return IExternalCommand.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,Move walls, X direction
    /// </summary>
    public class XMoveWalls : IExternalCommand
    {
        #region IExternalCommand Members Implementation

        public IExternalCommand.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            IEnumerator iter = CreateWall.CreatedWalls.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                Wall wall = iter.Current as Wall;
                if (null != wall)
                {
                    wall.Location.Move(new XYZ(12, 0, 0));
                }
            }

            return IExternalCommand.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }

    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand,Move walls, Y direction
    /// </summary>
    public class YMoveWalls : IExternalCommand
    {
        #region IExternalCommand Members Implementation

        public IExternalCommand.Result Execute(ExternalCommandData revit,
                                               ref string message,
                                               ElementSet elements)
        {
            IEnumerator iter = CreateWall.CreatedWalls.GetEnumerator();
            iter.Reset();
            while (iter.MoveNext())
            {
                Wall wall = iter.Current as Wall;
                if (null != wall)
                {
                    wall.Location.Move(new XYZ(0, 12, 0));
                }
            }

            return IExternalCommand.Result.Succeeded;
        }
        #endregion IExternalCommand Members Implementation
    }
}
