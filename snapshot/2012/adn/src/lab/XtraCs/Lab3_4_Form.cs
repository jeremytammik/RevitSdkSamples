#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2011 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Windows.Forms;
using FamilySymbol = Autodesk.Revit.DB.FamilySymbol;
#endregion // Namespaces

namespace XtraCs
{
  public partial class Lab3_4_Form : Form
  {
    private IDictionary<string, List<FamilySymbol>> _dict;

    public Lab3_4_Form( IDictionary<string, List<FamilySymbol>> dict )
    {
      _dict = dict;

      InitializeComponent();

      // initialise family combo box:
      foreach( string key in _dict.Keys )
      {
        cmbFamily.Items.Add( key );
      }
      cmbFamily.SelectedIndex = 0;

      // initialise type combo box:
      cmbFamily_SelectedIndexChanged( this, null );
      string catName = null; // ( cmbType.SelectedItem as Autodesk.Revit.Symbols.FamilySymbol ).Category.Name;
      this.Text = "Select a " + catName + " type";
    }

    private void cmbFamily_SelectedIndexChanged( object sender, EventArgs e )
    {
      try
      {
        cmbType.DataSource = _dict[cmbFamily.Text];
        cmbType.DisplayMember = "Name";
        //cmbType.ValueMember = "Id";
        cmbType.SelectedIndex = 0;
      }
      catch( Exception )
      {
      }
    }
  }
}
