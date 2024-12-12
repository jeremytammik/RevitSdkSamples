#region Header
// Copyright (C) 2007 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software
// for any purpose and without fee is hereby granted, provided
// that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION
// OF THE PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autodesk.Revit;
using RvtMenuItem = Autodesk.Revit.MenuItem;
#endregion // Namespaces

namespace RvtSamples
{
  /// <summary>
  /// Main external application class.
  /// A generic menu generator application.
  /// Read a text file and add entries to the Revit menu.
  /// Any number and location of entries is supported.
  /// </summary>
  public class App : IExternalApplication
  {
    #region Member Data
    static char[] _charSeparators = new char[] { '/', '\\' };

    public const string Title = "RvtSamples";
    public const string FilenameStem = Title + ".txt";
    
    ControlledApplication _app;
    Hashtable _menus = new Hashtable();
    #endregion // Member Data

    #region Message Handling
    public void ErrorMsg( string msg )
    {
      MessageBox.Show( msg, Title, MessageBoxButtons.OK, MessageBoxIcon.Error );
    }

    static void InfoMsg( string msg )
    {
      MessageBox.Show( msg, Title, MessageBoxButtons.OK, MessageBoxIcon.Information );
    }

    public void ShowAboutDlg()
    {
      Type t = this.GetType();
      Version ver = t.Assembly.GetName().Version;
      string s = Title + " " + ver.ToString()
        + ".\nWritten 2007 by Jeremy Tammik, Autodesk Inc.";
      InfoMsg( s );
    }
    #endregion // Message Handling

    #region Parser
    /// <summary>
    /// Get the input file path.
    /// Search and return the full path for the given file
    /// in the current exe directory or one or two directory levels higher.
    /// </summary>
    /// <param name="filename">Input filename stem, output full file path</param>
    /// <returns>True if found, false otherwise.</returns>
    bool GetFilepath( ref string filename )
    {
      string binarydir = Path.GetDirectoryName( Assembly.GetExecutingAssembly().Location );
      string path = Path.Combine( binarydir, filename );
      bool rc = File.Exists( path );
      if( !rc )
      {
        path = Path.Combine( binarydir, "../" + filename );
        rc = File.Exists( path );
      }
      if( !rc )
      {
        path = Path.Combine( binarydir, "../../" + filename );
        rc = File.Exists( path );
      }
      if( rc )
      {
        filename = Path.GetFullPath( path );
      }
      return rc;
    }

    /// <summary>
    /// Remove all comments and empty lines from a given array of lines.
    /// Comments are delimited by '#' to the end of the line.
    /// </summary>
    string[] RemoveComments( string[] lines )
    {
      int n = lines.GetLength( 0 );
      string[] a = new string[n];
      int i = 0;
      foreach( string line in lines )
      {
        string s = line;
        int j = s.IndexOf( '#' );
        if( 0 <= j )
        {
          s = s.Substring( 0, j );
        }
        s = s.Trim();
        if( 0 < s.Length )
        {
          a[i++] = s;
        }
      }
      string[] b = new string[i];
      n = i;
      for( i = 0; i < n; ++i )
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
    void AddSample( string[] lines, int n, ref int i )
    {
      if( n < i + 3 ) 
      {
        throw new Exception( string.Format( "Incomplete record at line {0} of {1}", i, FilenameStem ) );
      }
      string menuentry = lines[i++];
      //
      // create popup menu hierarchy:
      //
      string[] a = menuentry.Split( _charSeparators, StringSplitOptions.RemoveEmptyEntries );
      string key = "/";
      int m = a.Length;
      int j;
      for( j = 0; j < m; ++j )
      {
        a[j] = a[j].Trim();
      }
      bool addToTools = false;
      j = 0;
      if( a[j].ToLower() == "tools" )
      {
        key += "Tools/";
        addToTools = true;
        if( a[j].ToLower() == "external tools" )
        {
          ++j;
        }
      }
      //
      // create top level menu:
      //
      RvtMenuItem mi;
      string s = a[j++];
      key += s;
      if( !_menus.ContainsKey( key ) )
      {
        _menus[key] = mi = _app.CreateTopMenu( s );
        if( addToTools )
        {
          bool success = mi.AddToExternalTools();
        }
      }
      mi = _menus[key] as RvtMenuItem;
      //
      // create submenu hierarchy:
      //
      while( j < m - 1 )
      {
        s = a[j++];
        key += "/" + s;
        if( !_menus.ContainsKey( key ) )
        {
          _menus[key] = mi.Append( RvtMenuItem.MenuType.PopupMenu, s );
        }
        mi = _menus[key] as RvtMenuItem;
      }
      //
      // create menu entry for command:
      //
      string name = a[j];
      string description = lines[i++];
      string assembly = lines[i++];
      if( !File.Exists( assembly ) )
      {
        throw new Exception( string.Format( "Assembly '{0}' specified in line {1} of {2} not found", assembly, i, FilenameStem ) );
      }
      string className = lines[i++];
      Debug.WriteLine( key + "/" + name );
      if( !key.StartsWith( "/AU/Rst" ) || _app.VersionName.Contains( "Structure" ) ) // kludge to work around bug, menu too big in rac 2008, throw exception "Specified cast is invalid."
      {
        mi = mi.Append( RvtMenuItem.MenuType.BasicMenu, name, assembly, className );
        mi.StatusbarTip = description;
      }
    }
    #endregion // Menu Helpers

    #region IExternalApplication Members
    public IExternalApplication.Result OnStartup( ControlledApplication app )
    {
      _app = app;
      IExternalApplication.Result rc = IExternalApplication.Result.Failed;

      try
      {
        string filename = FilenameStem;
        if( !GetFilepath( ref filename ) )
        {
          ErrorMsg( FilenameStem + " not found." );
        }
        else
        {
          string[] lines = File.ReadAllLines( filename );
          lines = RemoveComments( lines );
          int n = lines.GetLength( 0 );
          int i = 0;
          while( i < n )
          {
            AddSample( lines, n, ref i );
          }
          rc = IExternalApplication.Result.Succeeded;
        }
      }
      catch( Exception e )
      {
        ErrorMsg( e.Message );
      }
      return rc;
    }

    public IExternalApplication.Result OnShutdown( ControlledApplication app )
    {
      return IExternalApplication.Result.Succeeded;
    }
    #endregion // IExternalApplication Members
  }
}
