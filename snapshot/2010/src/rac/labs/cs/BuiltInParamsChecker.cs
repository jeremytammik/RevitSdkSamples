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
//
// History:
//
// 2008-06-03 jeremy implemented copy to clipboard
#endregion // Header

#region Namespaces
using System;
using System.Collections;
using System.Diagnostics;
using WinForms = System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace Labs
{
  #region BuiltInParamsChecker_using_MessageBox
  /// <summary>
  /// List all accessible parameters on a selected element in a message box.
  /// </summary>
  public class BuiltInParamsChecker_using_MessageBox : IExternalCommand
  {
    private Document _doc;
    private Hashtable _dictEnum = new Hashtable();
    private Hashtable _dictName = new Hashtable();

    /// <summary>
    /// Helper method to display the element parameter values collected in a dictionary.
    /// </summary>
    void DisplayParameters( bool isSymbol, string sorting, Hashtable dict )
    {
      string msg = dict.Count + " valid" + (isSymbol ? " type" : "")
        + " parameters for this element sorted by " + sorting + ":";
      ArrayList keys = new ArrayList( dict.Keys );
      keys.Sort();
      foreach( string key in keys )
      {
        msg += "\r\n  " + key + " : " + dict[key];
      }
      LabUtils.InfoMsg( msg );
    }

    /// <summary>
    /// Revit external command to list all valid built-in parameters for a given selected element.
    /// </summary>
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      _doc = commandData.Application.ActiveDocument;
      ElementSet ss = _doc.Selection.Elements;
      if( 1 != ss.Size )
      {
        LabUtils.ErrorMsg( "Please pre-select a single element." );
        return CmdResult.Cancelled;
      }
      ElementSetIterator iter = ss.ForwardIterator();
      iter.MoveNext();
      Element elem = iter.Current as Element;
      bool isSymbol = false;
      //
      // for a family instance, ask user whether to display instance or type parameters:
      //
      if( elem is FamilyInstance )
      {
        string msg = "This element is a family instance. By default, the instance parameters are displayed. Would you like to display the type parameters instead?";
        if( LabUtils.QuestionMsg( msg ) )
        {
          FamilyInstance inst = elem as FamilyInstance;
          if( null != inst.Symbol )
          {
            elem = inst.Symbol;
            isSymbol = true;
          }
        }
      }
      //
      // takes some time, so change cursor:
      //
      WinForms.Cursor oldCursor = WinForms.Cursor.Current;
      WinForms.Cursor.Current = WinForms.Cursors.WaitCursor;
      string e, n, r, v; // parameter enum, name, read-only and value
      foreach( BuiltInParameter a in Enum.GetValues( typeof( BuiltInParameter ) ) )
      {
        try
        {
          Parameter p = elem.get_Parameter( a );
          if( null != p ) // this check is faster than throwing an exception for each invalid param
          {
            e = a.ToString();
            n = p.Definition.Name;
            r = p.IsReadOnly ? " read-only" : string.Empty;
            v = " = '" + p.AsValueString() + "'.";
            switch( p.StorageType )
            {
              case StorageType.Double:
                v = " =" + r + " Double: " + p.AsDouble().ToString() + v;
                _dictEnum[e] = "'" + n + "'" + v;
                _dictName[n] = e + v;
                break;
              case StorageType.Integer:
                v = " =" + r + " Integer: " + p.AsInteger().ToString() + v;
                _dictEnum[e] = "'" + n + "'" + v;
                _dictName[n] = e + v;
                break;
              case StorageType.String:
                v = " =" + r + " String: '" + p.AsString() + "'" + v;
                _dictEnum[e] = "'" + n + "'" + v;
                _dictName[n] = e + v;
                break;
              case StorageType.ElementId:
                v = " =" + r + " Id: " + p.AsElementId().Value.ToString() + v;
                _dictEnum[e] = "'" + n + "'" + v;
                _dictName[n] = e + v;
                break;
              case StorageType.None:
                v = " =" + r + " None" + v;
                _dictEnum[e] = "'" + n + "'" + v;
                _dictName[n] = e + v;
                break;
            }
          }
        }
        catch( Exception )
        {
        }
      } // looping field infos
      WinForms.Cursor.Current = oldCursor;
      // KIS for now - in future may display in an user-friendly form...
      DisplayParameters( isSymbol, "built-in parameter enum", _dictEnum );
      DisplayParameters( isSymbol, "parameter definition name", _dictName );
      return CmdResult.Succeeded;
    }
  }
  #endregion // BuiltInParamsChecker_using_MessageBox

  /// <summary>
  /// List all accessible parameters on a selected element in a DataGridView.
  /// </summary>
  public class BuiltInParamsChecker : IExternalCommand
  {
    /// <summary>
    /// A class used to manage the data of an element parameter.
    /// </summary>
    public class ParameterData
    {
      BuiltInParameter _enum;
      Parameter _parameter;
      string _valueString; // store the value string or the element description in case of an element id
      //string _value; // store the raw value, since in case of an element id, we need access to the document to retrieve the referred-to element type

      public ParameterData( BuiltInParameter bip, Parameter parameter, string valueStringOrElementDescription )
      {
        _enum = bip;
        _parameter = parameter;
        _valueString = valueStringOrElementDescription;
      }

      public string Enum
      {
        get { return _enum.ToString(); }
      }

      public string Name
      {
        get { return _parameter.Definition.Name; }
      }

      public string Type
      {
        get 
        {
          ParameterType pt = _parameter.Definition.ParameterType; // returns 'Invalid' for 'ElementId'
          string s = ParameterType.Invalid == pt ? "" : "/" + pt.ToString();
          return _parameter.StorageType.ToString() + s;
        }
      }

      public string ReadWrite
      {
        get { return _parameter.IsReadOnly ? "read-only" : "read-write"; }
      }

      public string ValueString
      {
        //get { return _parameter.AsValueString(); }
        get { return _valueString; }
      }

      public string Value
      {
        get
        {
          //return _value;
          string s;
          switch( _parameter.StorageType )
          {
            // database value, internal units, e.g. feet:
            case StorageType.Double: s = LabUtils.RealString( _parameter.AsDouble() ); break;
            case StorageType.Integer: s = _parameter.AsInteger().ToString(); break;
            case StorageType.String: s = _parameter.AsString(); break;
            case StorageType.ElementId: s = _parameter.AsElementId().Value.ToString(); break;
            case StorageType.None: s = "None"; break;
            default: Debug.Assert( false, "unexpected storage type" ); s = string.Empty; break;
          }
          return s;
        }
      }

      //public string RowString
      //{
      //  get
      //  {
      //    return Enum + "\t" + Name + "\t" + ReadWrite + "\t" + ValueString + "\t" + Value;
      //  }
      //}
		}

    /// <summary>
    /// Revit external command to list all valid built-in parameters for a given selected element.
    /// </summary>
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Document doc = commandData.Application.ActiveDocument;
      Element elem = LabUtils.GetSingleSelectedElement( doc );

      if( null == elem )
      {
        return CmdResult.Cancelled;
      }
      bool isSymbol = false;
      //
      // for a family instance, ask user whether to display instance or type parameters;
      // in a similar manner, we could add dedicated switches for Wall --> WallType,
      // Floor --> FloorType etc. ...
      //
      if( elem is FamilyInstance )
      {
        string msg = 
          "This element is a family instance, so it has both type and instance parameters."
          + " By default, the instance parameters are displayed."
          + " If you select 'No', the type parameters will be displayed instead."
          + " Would you like to see the instance parameters?";

        if( !LabUtils.QuestionMsg( msg ) )
        {
          FamilyInstance inst = elem as FamilyInstance;
          if( null != inst.Symbol )
          {
            elem = inst.Symbol;
            isSymbol = true;
          }
        }
      }
      SortableBindingList<ParameterData> data = new SortableBindingList<ParameterData>();
      {
        WaitCursor waitCursor = new WaitCursor();
        Array bips = Enum.GetValues( typeof( BuiltInParameter ) );
        int n = bips.Length;
        Parameter p;
        foreach( BuiltInParameter a in bips )
        {
          try
          {
            p = elem.get_Parameter( a );

            #region Check for external definition
#if CHECK_FOR_EXTERNAL_DEFINITION
            Definition d = p.Definition;
            ExternalDefinition e = d as ExternalDefinition; // this is never possible
            string guid = ( null == e ) ? null : e.GUID.ToString();
#endif // CHECK_FOR_EXTERNAL_DEFINITION
            #endregion // Check for external definition

            if( null != p )
            {
              //string value = LabUtils.GetParameterValue2( p, doc );
              string valueString = LabUtils.GetParameterValueString2( p, doc );
              data.Add( new ParameterData( a, p, valueString ) );
            }
          }
          catch( Exception ex )
          {
            Debug.Print( "Exception retrieving built-in parameter {0}: {1}",
              a, ex );
          }
        }
      }
      string description = LabUtils.ElementDescription( elem, true ) + ( isSymbol ? " Type" : " Instance" );
      using( BuiltInParamsCheckerForm form = new BuiltInParamsCheckerForm( description, data ) )
      {
        form.ShowDialog();
      }
      return CmdResult.Succeeded;
    }
  }
}
