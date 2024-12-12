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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace RvtSamples
{
    /// <summary>
    /// Main external application class.
    /// A generic menu generator application.
    /// Read a text file and add entries to the Revit menu.
    /// Any number and location of entries is supported.
    /// </summary>
    public class Application : IExternalApplication
    {
        /// <summary>
        /// Default pulldown menus for samples
        /// </summary>
        public enum DefaultPulldownMenus
        {
            /// <summary>
            /// Menu for Basics category
            /// </summary>
            Basics,
            /// <summary>
            /// Menu for Geometry category
            /// </summary>
            Geometry,
            /// <summary>
            /// Menu for Parameters category
            /// </summary>
            Parameters,
            /// <summary>
            /// Menu for Elements category
            /// </summary>
            Elements,
            /// <summary>
            /// Menu for Families category
            /// </summary>
            Families,
            /// <summary>
            /// Menu for Materials category
            /// </summary>
            Materials,
            /// <summary>
            /// Menu for Annotation category
            /// </summary>
            Annotation,
            /// <summary>
            /// Menu for Views category
            /// </summary>
            Views,
            /// <summary>
            /// Menu for Rooms/Spaces category
            /// </summary>
            RoomsAndSpaces,
            /// <summary>
            /// Menu for Data Exchange category
            /// </summary>
            DataExchange,
            /// <summary>
            /// Menu for MEP category
            /// </summary>
            MEP,
            /// <summary>
            /// Menu for Structure category
            /// </summary>
            Structure,
            /// <summary>
            /// Menu for Analysis category
            /// </summary>
            Analysis,
            /// <summary>
            /// Menu for Massing category
            /// </summary>
            Massing,
            /// <summary>
            /// Menu for Selection category
            /// </summary>
            Selection
        }

        #region Member Data
        /// <summary>
        /// Separator of category for samples have more than one category
        /// </summary>
        static char[] s_charSeparatorOfCategory = new char[] { ',' };
        /// <summary>
        /// chars which will be trimmed in the file to include extra sample list files
        /// </summary>
        static char[] s_trimChars = new char[] { ' ', '"', '\'', '<', '>' };
        /// <summary>
        /// The start symbol of lines to include extra sample list files
        /// </summary>
        static string s_includeSymbol = "#include";
        /// <summary>
        /// Assembly directory
        /// </summary>
        static string s_assemblyDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        /// <summary>
        /// Name of file which contains information required for menu items
        /// </summary>
        public const string m_fileNameStem = "RvtSamples.txt";
        /// <summary>
        /// The controlled application of Revit
        /// </summary>
        private UIControlledApplication m_application;
        /// <summary>
        /// List contains all pulldown button items contain sample items
        /// </summary>
        private SortedList<string, PulldownButton> m_pulldownButtons = new SortedList<string, PulldownButton>();
        /// <summary>
        /// List contains information of samples not belong to default pulldown menus
        /// </summary>
        private SortedList<string, List<SampleItem>> m_customizedMenus = new SortedList<string, List<SampleItem>>();
        /// <summary>
        /// List contains information of samples belong to default pulldown menus
        /// </summary>
        private SortedList<string, List<SampleItem>> m_defaultMenus = new SortedList<string, List<SampleItem>>();
        /// <summary>
        /// Panel for RvtSamples
        /// </summary>
        RibbonPanel m_panelRvtSamples;
        #endregion // Member Data

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
        /// If false is returned then Revit should inform the user that the external application 
        /// failed to load and the release the internal reference.</returns>
        public Autodesk.Revit.UI.Result OnStartup(UIControlledApplication application)
        {
            m_application = application;
            Autodesk.Revit.UI.Result rc = Autodesk.Revit.UI.Result.Failed;
            string[] lines = null;
            int n = 0;
            int k = 0;

            try
            {
                // Check whether the file contains samples' list exists
                // If not, return failure
                string filename = m_fileNameStem;
                if (!GetFilepath(ref filename))
                {
                    ErrorMsg(m_fileNameStem + " not found.");
                    return rc;
                }

                // Read all lines from the file
                lines = ReadAllLinesWithInclude(filename);
                // Remove comments
                lines = RemoveComments(lines);

                // Add default pulldown menus of samples to Revit
                m_panelRvtSamples = application.CreateRibbonPanel("RvtSamples");
                int i = 0;
                List<PulldownButtonData> pdData = new List<PulldownButtonData>(3);
                foreach (string category in Enum.GetNames(typeof(DefaultPulldownMenus)))
                {
                    if ((i + 1) % 3 == 1)
                    {
                        pdData.Clear();
                    }

                    //
                    // Prepare PulldownButtonData for add stacked buttons operation
                    //
                    string displayName = GetDisplayNameByEnumName(category);

                    List<SampleItem> sampleItems = new List<SampleItem>();
                    m_defaultMenus.Add(category, sampleItems);

                    PulldownButtonData data = new PulldownButtonData(displayName, displayName);
                    pdData.Add(data);

                    //
                    // Add stacked buttons to RvtSamples panel and set their display names and images
                    //
                    if ((i + 1) % 3 == 0)
                    {
                        IList<RibbonItem> addedButtons = m_panelRvtSamples.AddStackedItems(pdData[0], pdData[1], pdData[2]);
                        foreach (RibbonItem item in addedButtons)
                        {
                            String name = item.ItemText;
                            string enumName = GetEnumNameByDisplayName(name);
                            PulldownButton button = item as PulldownButton;
                            button.Image = new BitmapImage(
                                new Uri(Path.Combine(s_assemblyDirectory, "Icons\\" + enumName + ".ico"), UriKind.Absolute));
                            button.ToolTip = Properties.Resource.ResourceManager.GetString(enumName);
                            m_pulldownButtons.Add(name, button);
                        }
                    }

                    i++;
                }

                //
                // Add sample items to the pulldown buttons
                //
                n = lines.GetLength(0);
                k = 0;
                while (k < n)
                {
                    AddSample(lines, n, ref k);
                }

                AddSamplesToDefaultPulldownMenus();
                AddCustomizedPulldownMenus();

                rc = Autodesk.Revit.UI.Result.Succeeded;
            }
            catch (Exception e)
            {
                string s = string.Format("{0}: n = {1}, k = {2}, lines[k] = {3}",
                  e.Message, n, k, (k < n ? lines[k] : "eof"));

                ErrorMsg(s);
            }
            return rc;
        }

        /// <summary>
        /// Get a button's enum name by its display name
        /// </summary>
        /// <param name="name">display name</param>
        /// <returns>enum name</returns>
        private string GetEnumNameByDisplayName(string name)
        {
            string enumName = null;
            if (name.Equals("Rooms/Spaces"))
            {
                enumName = DefaultPulldownMenus.RoomsAndSpaces.ToString();
            }
            else if (name.Equals("Data Exchange"))
            {
                enumName = DefaultPulldownMenus.DataExchange.ToString();
            }
            else
            {
                enumName = name;
            }

            return enumName;
        }

        /// <summary>
        /// Get a button's display name by its enum name
        /// </summary>
        /// <param name="enumName">The button's enum name</param>
        /// <returns>The button's display name</returns>
        private string GetDisplayNameByEnumName(string enumName)
        {
            string displayName = null;
            if (enumName.Equals(DefaultPulldownMenus.RoomsAndSpaces.ToString()))
            {
                displayName = "Rooms/Spaces";
            }
            else if (enumName.Equals(DefaultPulldownMenus.DataExchange.ToString()))
            {
                displayName = "Data Exchange";
            }
            else
            {
                displayName = enumName;
            }

            return displayName;
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
        public Autodesk.Revit.UI.Result OnShutdown(UIControlledApplication application)
        {
            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion // IExternalApplication Members

        /// <summary>
        /// Display error message
        /// </summary>
        /// <param name="msg">Message to display</param>
        public void ErrorMsg(string msg)
        {
            Debug.WriteLine("RvtSamples: " + msg);
            TaskDialog.Show("RvtSamples", msg, TaskDialogCommonButtons.Ok);
        }

        #region Parser
        /// <summary>
        /// Read file contents, including contents of files included in the current file
        /// </summary>
        /// <param name="filename">Current file to be read</param>
        /// <returns>All lines of file contents</returns>
        private string[] ReadAllLinesWithInclude(string filename)
        {
            if (!File.Exists(filename))
            {
                ErrorMsg(filename + " not found.");
                return new string[] { };
            }
            string[] lines = File.ReadAllLines(filename);

            int n = lines.GetLength(0);
            ArrayList all_lines = new ArrayList(n);
            foreach (string line in lines)
            {
                string s = line.TrimStart();
                if (s.ToLower().StartsWith(s_includeSymbol))
                {
                    string filename2 = s.Substring(s_includeSymbol.Length);
                    filename2 = filename2.Trim(s_trimChars);
                    all_lines.AddRange(ReadAllLinesWithInclude(filename2));
                }
                else
                {
                    all_lines.Add(line);
                }
            }
            return all_lines.ToArray(typeof(string)) as string[];
        }

        /// <summary>
        /// Get the input file path.
        /// Search and return the full path for the given file
        /// in the current exe directory or one or two directory levels higher.
        /// </summary>
        /// <param name="filename">Input filename stem, output full file path</param>
        /// <returns>True if found, false otherwise.</returns>
        bool GetFilepath(ref string filename)
        {
            string path = Path.Combine(s_assemblyDirectory, filename);
            bool rc = File.Exists(path);

            // Get full path of the file
            if (rc)
            {
                filename = Path.GetFullPath(path);
            }
            return rc;
        }

        /// <summary>
        /// Remove all comments and empty lines from a given array of lines.
        /// Comments are delimited by '#' to the end of the line.
        /// </summary>
        string[] RemoveComments(string[] lines)
        {
            int n = lines.GetLength(0);
            string[] a = new string[n];
            int i = 0;
            foreach (string line in lines)
            {
                string s = line;
                int j = s.IndexOf('#');
                if (0 <= j)
                {
                    s = s.Substring(0, j);
                }
                s = s.Trim();
                if (0 < s.Length)
                {
                    a[i++] = s;
                }
            }
            string[] b = new string[i];
            n = i;
            for (i = 0; i < n; ++i)
            {
                b[i] = a[i];
            }
            return b;
        }
        #endregion // Parser

        #region Menu Helpers
        /// <summary>
        /// Add a new command to the corresponding pulldown button.
        /// </summary>
        /// <param name="lines">Array of lines defining sample's category, display name, description, large image, image, assembly and classname</param>
        /// <param name="n">Total number of lines in array</param>
        /// <param name="i">Current index in array</param>
        void AddSample(string[] lines, int n, ref int i)
        {
            if (n < i + 6)
            {
                throw new Exception(string.Format("Incomplete record at line {0} of {1}", i, m_fileNameStem));
            }

            string categories = lines[i++].Trim();
            string displayName = lines[i++].Trim();
            string description = lines[i++].Trim();
            string largeImage = lines[i++].Remove(0, 11).Trim();
            string image = lines[i++].Remove(0, 6).Trim();
            string assembly = lines[i++].Trim();
            string className = lines[i++].Trim();

            if (!File.Exists(assembly)) // jeremy
            {
                ErrorMsg(string.Format("Assembly '{0}' specified in line {1} of {2} not found",
                  assembly, i, m_fileNameStem));
            }

            bool testClassName = false; // jeremy
            if (testClassName)
            {
                Debug.Print("RvtSamples: testing command {0} in assembly '{1}'.", className, assembly);

                try
                {
                    // first load the revit api assembly, otherwise we cannot query the external app for its types:

                    //Assembly revit = Assembly.LoadFrom( "C:/Program Files/Revit Architecture 2009/Program/RevitAPI.dll" );
                    //string root = "C:/Program Files/Autodesk Revit Architecture 2010/Program/";
                    //Assembly adWindows = Assembly.LoadFrom( root + "AdWindows.dll" );
                    //Assembly uiFramework = Assembly.LoadFrom( root + "UIFramework.dll" );
                    //Assembly revit = Assembly.LoadFrom( root + "RevitAPI.dll" );

                    // load the assembly into the current application domain:

                    Assembly a = Assembly.LoadFrom(assembly);

                    if (null == a)
                    {
                        ErrorMsg(string.Format("Unable to load assembly '{0}' specified in line {1} of {2}",
                          assembly, i, m_fileNameStem));
                    }
                    else
                    {
                        // get the type to use:
                        Type t = a.GetType(className);
                        if (null == t)
                        {
                            ErrorMsg(string.Format("External command class {0} in assembly '{1}' specified in line {2} of {3} not found",
                              className, assembly, i, m_fileNameStem));
                        }
                        else
                        {
                            // get the method to call:
                            MethodInfo m = t.GetMethod("Execute");
                            if (null == m)
                            {
                                ErrorMsg(string.Format("External command class {0} in assembly '{1}' specified in line {2} of {3} does not define an Execute method",
                                  className, assembly, i, m_fileNameStem));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorMsg(string.Format("Exception '{0}' \ntesting assembly '{1}' \nspecified in line {2} of {3}",
                      ex.Message, assembly, i, m_fileNameStem));
                }
            }

            //
            // If sample belongs to default category, add the sample item to the sample list of the default category
            // If not, store the information for adding to RvtSamples panel later
            //
            string[] entries = categories.Split(s_charSeparatorOfCategory, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value in entries)
            {
                string category = value.Trim();
                SampleItem item = new SampleItem(category, displayName, description, largeImage, image, assembly, className);
                if (m_pulldownButtons.ContainsKey(category))
                {
                    m_defaultMenus.Values[m_defaultMenus.IndexOfKey(GetEnumNameByDisplayName(category))].Add(item);
                }
                else if (m_customizedMenus.ContainsKey(category))
                {
                    List<SampleItem> sampleItems = m_customizedMenus.Values[m_customizedMenus.IndexOfKey(category)];
                    sampleItems.Add(item);
                }
                else
                {
                    List<SampleItem> sampleItems = new List<SampleItem>();
                    sampleItems.Add(item);
                    m_customizedMenus.Add(category, sampleItems);
                }
            }
        }

        /// <summary>
        /// Add sample item to pulldown menu
        /// </summary>
        /// <param name="pullDownButton">Pulldown menu</param>
        /// <param name="item">Sample item to be added</param>
        private void AddSampleToPulldownMenu(PulldownButton pullDownButton, SampleItem item)
        {
            PushButtonData pushButtonData = new PushButtonData(item.DisplayName, item.DisplayName, item.Assembly, item.ClassName);
            PushButton pushButton = pullDownButton.AddPushButton(pushButtonData);
            if (!string.IsNullOrEmpty(item.LargeImage))
            {
                BitmapImage largeImageSource = new BitmapImage(new Uri(item.LargeImage, UriKind.Absolute));
                pushButton.LargeImage = largeImageSource;
            }
            if (!string.IsNullOrEmpty(item.Image))
            {
                BitmapImage imageSource = new BitmapImage(new Uri(item.Image, UriKind.Absolute));
                pushButton.Image = imageSource;
            }

            pushButton.ToolTip = item.Description;
        }

        /// <summary>
        /// Comparer to sort sample items by their display name
        /// </summary>
        /// <param name="s1">sample item 1</param>
        /// <param name="s2">sample item 2</param>
        /// <returns>compare result</returns>
        private static int SortByDisplayName(SampleItem item1, SampleItem item2)
        {
            return string.Compare(item1.DisplayName, item2.DisplayName);
        }

        /// <summary>
        /// Sort samples in one category by the sample items' display name
        /// </summary>
        /// <param name="menus">samples to be sorted</param>
        private void SortSampleItemsInOneCategory(SortedList<string, List<SampleItem>> menus)
        {
            int iCount = menus.Count;

            for (int j = 0; j < iCount; j++)
            {
                List<SampleItem> sampleItems = menus.Values[j];
                sampleItems.Sort(SortByDisplayName);
            }
        }

        /// <summary>
        /// Add samples of categories in default categories
        /// </summary>
        private void AddSamplesToDefaultPulldownMenus()
        {
            int iCount = m_defaultMenus.Count;

            // Sort sample items in every category by display name
            SortSampleItemsInOneCategory(m_defaultMenus);

            for (int i = 0; i < iCount; i++)
            {
                string category = m_defaultMenus.Keys[i];
                List<SampleItem> sampleItems = m_defaultMenus.Values[i];
                PulldownButton menuButton = m_pulldownButtons.Values[m_pulldownButtons.IndexOfKey(GetDisplayNameByEnumName(category))];
                foreach (SampleItem item in sampleItems)
                {
                    AddSampleToPulldownMenu(menuButton, item);
                }
            }
        }

        /// <summary>
        /// Add samples of categories not in default categories
        /// </summary>
        private void AddCustomizedPulldownMenus()
        {
            int iCount = m_customizedMenus.Count;

            // Sort sample items in every category by display name
            SortSampleItemsInOneCategory(m_customizedMenus);

            int i = 0;

            while (iCount >= 3)
            {
                string name = m_customizedMenus.Keys[i++];
                PulldownButtonData data1 = new PulldownButtonData(name, name);
                name = m_customizedMenus.Keys[i++];
                PulldownButtonData data2 = new PulldownButtonData(name, name);
                name = m_customizedMenus.Keys[i++];
                PulldownButtonData data3 = new PulldownButtonData(name, name);
                IList<RibbonItem> buttons = m_panelRvtSamples.AddStackedItems(data1, data2, data3);
                AddSamplesToStackedButtons(buttons);

                iCount -= 3;
            }

            if (iCount == 2)
            {
                string name = m_customizedMenus.Keys[i++];
                PulldownButtonData data1 = new PulldownButtonData(name, name);
                name = m_customizedMenus.Keys[i++];
                PulldownButtonData data2 = new PulldownButtonData(name, name);
                IList<RibbonItem> buttons = m_panelRvtSamples.AddStackedItems(data1, data2);
                AddSamplesToStackedButtons(buttons);
            }
            else if (iCount == 1)
            {
                string name = m_customizedMenus.Keys[i];
                PulldownButtonData pulldownButtonData = new PulldownButtonData(name, name);
                PulldownButton button = m_panelRvtSamples.AddItem(pulldownButtonData) as PulldownButton;
                List<SampleItem> sampleItems = m_customizedMenus.Values[m_customizedMenus.IndexOfKey(button.Name)];
                foreach (SampleItem item in sampleItems)
                {
                    AddSampleToPulldownMenu(button, item);
                }
            }
        }

        /// <summary>
        /// Add samples to corresponding pulldown button
        /// </summary>
        /// <param name="buttons">pulldown buttons</param>
        private void AddSamplesToStackedButtons(IList<RibbonItem> buttons)
        {
            foreach (RibbonItem rItem in buttons)
            {
                PulldownButton button = rItem as PulldownButton;
                List<SampleItem> sampleItems = m_customizedMenus.Values[m_customizedMenus.IndexOfKey(button.Name)];
                foreach (SampleItem item in sampleItems)
                {
                    AddSampleToPulldownMenu(button, item);
                }
            }
        }
        #endregion // Menu Helpers
    }
}
