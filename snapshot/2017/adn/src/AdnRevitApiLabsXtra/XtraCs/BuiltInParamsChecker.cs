#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2017 by Autodesk, Inc.
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

//#define CHECK_FOR_FAMILY_INSTANCE
#define CHECK_GET_TYPE_ID

#region Namespaces
using System;
using System.Diagnostics;
using WinForms = System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace XtraCs
{
  /// <summary>
  /// List all accessible parameters on a selected element in a DataGridView.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  public class BuiltInParamsChecker : IExternalCommand
  {
    /// <summary>
    /// A class used to manage the data of an element parameter.
    /// </summary>
    public class ParameterData
    {
      BuiltInParameter _enum;
      Parameter _parameter;
      string _valueString; // value string or element description in case of an element id

      public ParameterData(
        BuiltInParameter bip,
        Parameter parameter,
        string valueStringOrElementDescription )
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
            case StorageType.ElementId: s = _parameter.AsElementId().IntegerValue.ToString(); break;
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
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIDocument uidoc = commandData.Application.ActiveUIDocument;
      Document doc = uidoc.Document;

      Element e
        = LabUtils.GetSingleSelectedElementOrPrompt(
          uidoc );

      if( null == e )
      {
        return Result.Cancelled;
      }

      bool isSymbol = false;

      #region Check for FamilyInstance
#if CHECK_FOR_FAMILY_INSTANCE
      //
      // for a family instance, ask user whether to 
      // display instance or type parameters;
      // in a similar manner, we could add dedicated 
      // switches for Wall --> WallType,
      // Floor --> FloorType etc. ...
      //
      if( e is FamilyInstance )
      {
        FamilyInstance inst = e as FamilyInstance;
        if( null != inst.Symbol )
        {
          string symbol_name 
            = LabUtils.ElementDescription( 
              inst.Symbol, true );

          string family_name 
            = LabUtils.ElementDescription( 
              inst.Symbol.Family, true );

          string msg =
          "This element is a family instance, so it "
          + "has both type and instance parameters. "
          + "By default, the instance parameters are "
          + "displayed. If you select 'No', the type "
          + "parameters will be displayed instead. "
          + "Would you like to see the instance "
          + "parameters?";

          if( !LabUtils.QuestionMsg( msg ) )
          {
            e = inst.Symbol;
            isSymbol = true;
          }
        }
      }
#endif // CHECK_FOR_FAMILY_INSTANCE
      #endregion // Check for FamilyInstance

      #region Check for element type
#if CHECK_GET_TYPE_ID
      ElementId idType = e.GetTypeId();

      if( ElementId.InvalidElementId != idType )
      {
        // The selected element has a type; ask user 
        // whether to display instance or type 
        // parameters.

        ElementType typ = doc.GetElement( idType )
          as ElementType;

        Debug.Assert( null != typ,
          "expected to retrieve a valid element type" );

        string type_name = LabUtils.ElementDescription(
          typ, true );

        string msg =
          "This element has an ElementType, so it has "
          + "both type and instance parameters. By "
          + "default, the instance parameters are "
          + "displayed. If you select 'No', the type "
          + "parameters will be displayed instead. "
          + "Would you like to see the instance "
          + "parameters?";

        if( !LabUtils.QuestionMsg( msg ) )
        {
          e = typ;
          isSymbol = true;
        }
      }
#endif // CHECK_GET_TYPE_ID
      #endregion // Check for element type

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
            p = e.get_Parameter( a );

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
              string valueString = ( StorageType.ElementId == p.StorageType )
                ? LabUtils.GetParameterValue2( p, doc )
                : p.AsValueString();

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
      string description = LabUtils.ElementDescription( e, true )
        + ( isSymbol
          ? " Type"
          : " Instance" );

      using( BuiltInParamsCheckerForm form = new BuiltInParamsCheckerForm( description, data ) )
      {
        form.ShowDialog();
      }
      return Result.Succeeded;
    }
  }
}
