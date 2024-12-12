#region Header
//
// CmdWindowHandle.cs - determine Revit
// application main window handle.
//
// Copyright (C) 2009 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Diagnostics;
using Autodesk.Revit;
using CmdResult
  = Autodesk.Revit.IExternalCommand.Result;
using IWin32Window 
  = System.Windows.Forms.IWin32Window;
#endregion // Namespaces

namespace BuildingCoder
{
  /// <summary>
  /// Wrapper class for converting IntPtr to IWin32Window.
  /// </summary>
  public class WindowHandle : IWin32Window
  {
    IntPtr _hwnd;

    public WindowHandle( IntPtr h )
    {
      Debug.Assert( IntPtr.Zero != h, 
        "expected non-null window handle" );

      _hwnd = h;
    }

    public IntPtr Handle
    {
      get 
      { 
        return _hwnd; 
      }
    }
  }

  class CmdWindowHandle : IExternalCommand
  {
    const string _prompt 
      = "Please select some elements.";

    static WindowHandle _hWndRevit = null;

    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      if( null == _hWndRevit )
      {
        //Process[] processes
        //  = Process.GetProcessesByName( "Revit" );

        //if( 0 < processes.Length )
        //{
        //  IntPtr h = processes[0].MainWindowHandle;
        //  _hWndRevit = new WindowHandle( h );
        //}

        Process process
          = Process.GetCurrentProcess();

        IntPtr h = process.MainWindowHandle;
        _hWndRevit = new WindowHandle( h );
      }

      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Selection sel = doc.Selection;

      using( CmdWindowHandleForm f 
        = new CmdWindowHandleForm() )
      {
        f.Show( _hWndRevit );
        bool go = true;
        while( go )
        {
          SelElementSet ss = sel.Elements;
          int n = ss.Size;

          string s = string.Format(
            "{0} element{1} selected{2}",
            n, Util.PluralSuffix( n ),
            ((0 == n)
              ? ";\n" + _prompt 
              : ":" ) );

          foreach( Element e in ss )
          {
            s += "\n";
            s += Util.ElementDescription( e );
          }
          f.LabelText = s;
          sel.StatusbarTip = _prompt;
          go = sel.PickOne();
          Debug.Print( "go = " + go.ToString() ); 
        }
      }
      return CmdResult.Failed;
    }
  }
}
