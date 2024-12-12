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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;

using Autodesk.Revit;
using RvtMenuItem = Autodesk.Revit.MenuItem;


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
        #region Member Data
        // Separators in the file which contains information required for menu items
        static char[] s_charSeparators = new char[] { '/', '\\' };

        /// Title of the top menu to be added
        public const string m_title = "RvtSamples";
        /// Name of file which contains information required for menu items
        public const string m_fileNameStem = m_title + ".txt";

        // The controlled application of Revit
        ControlledApplication m_application;
        // Table contains all menu items of samples
        SortedList<string, RvtMenuItem> m_menus = new SortedList<string, Autodesk.Revit.MenuItem>();
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
        public IExternalApplication.Result OnStartup(
          ControlledApplication application )
        {
            m_application = application;
            IExternalApplication.Result rc = IExternalApplication.Result.Failed;

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
                //string[] lines = File.ReadAllLines(filename); // removed by jeremy
                string[] lines = ReadAllLinesWithInclude( filename ); // added by jeremy
                // Remove comments
                lines = RemoveComments(lines);

                // Add menu items of samples to Revit
                int n = lines.GetLength(0);
                int i = 0;
                while (i < n)
                {
                    AddSample(lines, n, ref i);
                }

                rc = IExternalApplication.Result.Succeeded;
            }
            catch (Exception e)
            {
                ErrorMsg(e.Message);
            }
            return rc;
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
        #endregion // IExternalApplication Members

        /// <summary>
        /// Display error message
        /// </summary>
        /// <param name="msg">Message to display</param>
        public void ErrorMsg(string msg)
        {
            MessageBox.Show(msg, m_title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #region Parser
        /// <summary>
        /// Get the input file path.
        /// Search and return the full path for the given file
        /// in the current exe directory or one or two directory levels higher.
        /// </summary>
        /// <param name="filename">Input filename stem, output full file path</param>
        /// <returns>True if found, false otherwise.</returns>
        bool GetFilepath(ref string filename)
        {
            string binarydir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path = Path.Combine(binarydir, "../../" + filename);//Path.Combine(binarydir, filename);
            bool rc = File.Exists(path);

            // Get full path of the file
            if (rc)
            {
                filename = Path.GetFullPath(path);
            }
            return rc;
        }

        char[] _trimChars = new char[] { 
            ' ', '"', '\'', '<', '>' };

        string[] ReadAllLinesWithInclude( 
            string filename )
        {
            if( !File.Exists( filename ) )
            {
                ErrorMsg( filename + " not found." );
                return new string[] { };
            }
            string[] lines = File.ReadAllLines( 
                filename );

            int n = lines.GetLength(0);
            ArrayList all_lines = new ArrayList( n );
            foreach( string line in lines )
            {
                string s = line.TrimStart();
                if( s.ToLower().StartsWith( "#include" ) )
                {
                    string filename2 = s.Substring( 8 );
                    filename2 = filename2.Trim( _trimChars );
                    all_lines.AddRange( ReadAllLinesWithInclude( 
                        filename2 ) );
                }
                else
                {
                    all_lines.Add( line );
                }
            }

            //int i = 0;
            //n = all_lines.Count;
            //lines = new string[n];
            //foreach( string line in all_lines )
            //{
            //    lines[i++] = line;
            //}
            //return lines;

            return all_lines.ToArray( typeof( string ) ) 
                as string[];
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
        /// Add a new command to the Revit menu.
        /// </summary>
        /// <param name="lines">Array of lines defining command menu entry, description, assembly and classname</param>
        /// <param name="n">Total number of lines in array</param>
        /// <param name="i">Current index in array</param>
        void AddSample(string[] lines, int n, ref int i)
        {
            if (n < i + 3)
            {
                throw new Exception(string.Format("Incomplete record at line {0} of {1}", i, m_fileNameStem));
            }
            string menuEntry = lines[i++];

            //
            // create popup menu hierarchy:
            //
            string[] a = menuEntry.Split(s_charSeparators, StringSplitOptions.RemoveEmptyEntries);
            string key = "/";
            int m = a.Length;
            int j;
            for (j = 0; j < m; ++j)
            {
                a[j] = a[j].Trim();
            }
            bool addToTools = false;
            j = 0;

            if (a[j].ToLower() == "external tools")
            {
                j++;
                addToTools = true;
                key += "ExternalTools/";
            }

            //
            // create top level menu
            //
            RvtMenuItem mi;
            string s = a[j++];
            key += s;
            if (!m_menus.ContainsKey(key))
            {
                m_menus[key] = mi = m_application.CreateTopMenu(s);
                if (addToTools)
                {
                    bool success = mi.AddToExternalTools();
                }
            }
            mi = m_menus[key] as RvtMenuItem;

            //
            // create submenu hierarchy
            //
            while (j < m - 1)
            {
                s = a[j++];
                key += "/" + s;
                if (!m_menus.ContainsKey(key))
                {
                    m_menus[key] = mi.Append(RvtMenuItem.MenuType.PopupMenu, s);
                }
                mi = m_menus[key] as RvtMenuItem;
            }

            //
            // create menu entry for command
            //
            string name = a[j];
            string description = lines[i++];
            string assembly = lines[i++];
            if (!File.Exists(assembly))
            {
                throw new Exception(string.Format("Assembly '{0}' specified in line {1} of {2} not found", assembly, i, m_fileNameStem));
            }
            string className = lines[i++];
            mi = mi.Append(RvtMenuItem.MenuType.BasicMenu, name, assembly, className);
            mi.StatusbarTip = description;
        }
        #endregion // Menu Helpers
    }
}
