#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2009 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Runtime.InteropServices;
using System.Text;
#endregion // Namespaces

namespace Labs
{
  //
  // based on Window Hiding with C# by Taylor Wood at
  // http://www.codeproject.com/csharp/windowhider.asp?df=100&forumid=3881&exp=0&select=1310572#xx1310572xx
  //
  class JtRevitWindow
  {
    /// <summary>
    /// Win32 API Imports
    /// </summary>
    [DllImport( "user32.dll" )]
    private static extern int GetWindowModuleFileName( int hWnd, StringBuilder title, int size );
    [DllImport( "user32.dll" )]
    private static extern int EnumWindows( EnumWindowsProc ewp, int lParam );
    [DllImport( "user32.dll" )]
    private static extern bool SetForegroundWindow( IntPtr hWnd );
    [DllImport( "user32.dll" )]
    private static extern IntPtr GetForegroundWindow();

    //delegate used for EnumWindows() callback function
    public delegate bool EnumWindowsProc( int hWnd, int lParam );

    // private member variables
    private IntPtr _hWnd = IntPtr.Zero;

    public JtRevitWindow()
    {
      EnumWindowsProc ewp = new EnumWindowsProc( EvalWindow );
      EnumWindows( ewp, 0 );
    }

    private bool EvalWindow( int hWnd, int lParam )
    {
      //StringBuilder title = new StringBuilder( 256 );
      //GetWindowText( hWnd, title, 256 );
      StringBuilder module = new StringBuilder( 256 );
      GetWindowModuleFileName( hWnd, module, 256 );
      if( module.ToString().ToLower().EndsWith( "revit.exe" ) )
      {
        _hWnd = (IntPtr) hWnd;
        return false;
      }
      return true;
    }

    /// <summary>
    /// Set focus to this window
    /// </summary>
    public void Activate()
    {
      if( IntPtr.Zero != _hWnd && _hWnd != GetForegroundWindow() )
      {
        SetForegroundWindow( _hWnd );
      }
    }
  }
}
