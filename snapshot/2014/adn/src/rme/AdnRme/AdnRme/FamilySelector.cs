#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2013 by Jeremy Tammik, Autodesk, Inc.
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
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION OF THE 
// PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
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
using System.Windows.Forms;
#endregion // Namespaces

namespace AdnRme
{
  public partial class FamilySelector : Form
  {
    public FamilySelector( IEnumerable a )
    {
      InitializeComponent();
      foreach( string s in a )
      {
        checkedListBox1.Items.Add( s, true );
      }
    }

    public bool IsChecked( string s )
    {
      int i = checkedListBox1.Items.IndexOf( s );
      Debug.Assert( -1 < i, "expected item to be contained in listbox" );
      return ( -1 < i ) ? checkedListBox1.GetItemChecked( i ) : false;
    }
  }
}
