#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2010 by Autodesk, Inc.
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
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using WinForms = System.Windows.Forms;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

// Added reference to Microsoft Excel 11.0 Object Library
// http://support.microsoft.com/kb/306023/
// http://support.microsoft.com/kb/302084/EN-US/
using X = Microsoft.Office.Interop.Excel;
#endregion // Namespaces

namespace Labs
{
  #region Lab4_1_ElementParameters
  /// <summary>
  /// List all parameters for selected elements.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="4-1"]/*' />
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab4_1_ElementParameters : IExternalCommand
  {

    #region TEST_2
#if TEST_2
    List<string> GetParameters( Document doc, Element element )
    {
      List<string> _parameters = new List<string>();
      foreach( Parameter param in element.Parameters )
      {
        if( param == null ) continue;
        string _name = param.Definition.Name;
        switch( param.StorageType )
        {
          case Autodesk.Revit.Parameters.StorageType.Double:
            _parameters.Add( _name + " : " + param.AsDouble().ToString() );
            break;
          case Autodesk.Revit.Parameters.StorageType.Integer:
            if( param.Definition.ParameterType == ParameterType.YesNo )
            {
              if( param.AsInteger() == 0 ) _parameters.Add( _name + " : " + "false" );
              else _parameters.Add( _name + " : " + "true" );
            }
            else
            {
              _parameters.Add( _name + " : " + param.AsInteger().ToString() );
            }
            break;
          case Autodesk.Revit.Parameters.StorageType.String:
            _parameters.Add( _name + " : " + param.AsString() );
            break;
          case Autodesk.Revit.Parameters.StorageType.ElementId:
            ElementId id = param.AsElementId();
            if( id.Value >= 0 ) _parameters.Add( _name + " : " + doc.get_Element( ref id ).Name );
            else _parameters.Add( _name + " : " + id.Value.ToString() );
            break;
          case Autodesk.Revit.Parameters.StorageType.None:
            _parameters.Add( _name + " : " + "none" );
            break;
        }
      }
      return _parameters;
    }
#endif // TEST_2
    #endregion // TEST_2

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {

      #region TEST_1
#if TEST_1
      //
      // you cannot create your own parameter, because the
      // constuctor is for internal use only. This is due
      // to the fact that a parameter cannot live on its own,
      // it is linked to a definition and needs to be hooked
      // up properly in the Revit database system to work
      // ... case 1245614 [Formatting units strings]:
      //
      bool iReallyWantToCrash = false;
      if( iReallyWantToCrash )
      {
        Parameter p = new Parameter();
        p.Set( 1.0 );
        string s = p.AsDouble().ToString();
        string t = p.AsValueString();
        Debug.WriteLine( "Value " + s );
        Debug.WriteLine( "Value string " + t );
      }
#endif // TEST
      #endregion // TEST_1

      UIDocument uidoc = commandData.Application.ActiveUIDocument;
      Document doc = uidoc.Document;

      // Loop through all pre-selected elements:

      foreach( Element e in uidoc.Selection.Elements )
      {
        Element e2 = e; // enable us to assign to e2 in case analyseTypeParameters == true
        string s = string.Empty;

        // set this variable to false to analyse the element's own parameters,
        // i.e. instance parameters for a family instance, and set it to true
        // to analyse a family instance's type parameters:

        bool analyseTypeParameters = false;

        if( analyseTypeParameters )
        {
          if( e2 is FamilyInstance )
          {
            FamilyInstance inst = e2 as FamilyInstance;
            if( null != inst.Symbol )
            {
              e2 = inst.Symbol;
              s = " type";
            }
          }
          else if( e2 is Wall )
          {
            Wall wall = e2 as Wall;
            if( null != wall.WallType )
            {
              e2 = wall.WallType;
              s = " type";
            }
          }
          // ... add support for other types if desired ...
        }

        // Loop through and list all UI-visible element parameters

        List<string> a = new List<string>();

        #region 4.1.a Iterate over element parameters and retrieve their name, type and value:

        foreach( Parameter p in e2.Parameters )
        {
          string name = p.Definition.Name;
          string type = p.StorageType.ToString();
          string value = LabUtils.GetParameterValue2( p, uidoc.Document );
          bool read_only = p.Definition.IsReadOnly;
          a.Add( string.Format(
            "Name={0}; Type={1}; Value={2}; ValueString={3}; read-{4}",
            name, type, value, p.AsValueString(),
            ( read_only ? "only" : "write" ) ) );
        }

        #endregion // 4.1.a

        string what = e2.Category.Name
          + " (" + e2.Id.IntegerValue.ToString() + ")";

        LabUtils.InfoMsg( what + " has {0} parameter{1}{2}", a );

        // If we know which param we are looking for, then:
        // A) If a standard parameter, we can get it via BuiltInParam
        // signature of Parameter method:

        try
        {

          #region 4.1.b Retrieve a specific built-in parameter:

          Parameter p = e.get_Parameter(
            BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM );

          #endregion // 4.1.b

          if( null == p )
          {
            LabUtils.InfoMsg( "FAMILY_BASE_LEVEL_OFFSET_PARAM is NOT available for this element." );
          }
          else
          {
            string name = p.Definition.Name;
            string type = p.StorageType.ToString();
            string value = LabUtils.GetParameterValue2( p, doc );
            LabUtils.InfoMsg( "FAMILY_BASE_LEVEL_OFFSET_PARAM: Name=" + name
              + "; Type=" + type + "; Value=" + value );
          }
        }
        catch( Exception )
        {
          LabUtils.InfoMsg( "FAMILY_BASE_LEVEL_OFFSET_PARAM is NOT available for this element." );
        }

        // B) For a shared parameter, we can get it via "GUID" signature
        // of Parameter method  ... this will be shown later in Labs 4 ...

        // C) or we can get the parameter by name:
        // alternatively, loop through all parameters and
        // search for the name (this works for either
        // standard or shared):

        const string paramName = "Base Offset";
        Parameter parByName = e.get_Parameter( paramName );
        if( null == parByName )
        {
          LabUtils.InfoMsg( paramName + " is NOT available for this element." );
        }
        else
        {
          string parByNameName = parByName.Definition.Name;
          string parByNameType = parByName.StorageType.ToString();
          string parByNameValue = LabUtils.GetParameterValue2( parByName, doc );
          LabUtils.InfoMsg( paramName + ": Name=" + parByNameName
            + "; Type=" + parByNameType + "; Value=" + parByNameValue );
        }

        #region TEST_2
#if TEST_2
        List<string> a = GetParameters( doc, e2 );
        foreach( string s2 in a )
        {
          Debug.WriteLine( s2 );
        }
#endif // TEST_2
        #endregion // TEST_2

      }
      return Result.Failed;
    }
  }
  #endregion // Lab4_1_ElementParameters

  #region Lab4_2_ExportParametersToExcel
  /// <summary>
  /// Export all parameters for each model element to Excel, one sheet per category.
  /// <include file='../doc/labs.xml' path='labs/lab[@name="4-2"]/*' />
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab4_2_ExportParametersToExcel : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      // extract and group the data from Revit in a dictionary, where
      // the key is the category name and the value is a list of elements.
      //Autodesk.Revit.Collections.Map sortedElements = app.Create.NewMap();

      Dictionary<string, List<Element>> sortedElements = new Dictionary<string, List<Element>>();

      // iterate all non-symbol elements and store in dictionary

      FilteredElementCollector collector
        = new FilteredElementCollector( doc ).WhereElementIsNotElementType();

      foreach( Element e in collector )
      {
        Debug.Assert( !(e is ElementType), "expected no ElementType elements" );

        Category category = e.Category;

        if( null != category )
        {
          string name = category.Name;

          List<Element> elementSet;

          // if we already have this key, get its value (set);
          // otherwise, create the new key and value (set):

          if( sortedElements.ContainsKey( name ) )
          {
            elementSet = sortedElements[name];
          }
          else
          {
            elementSet = new List<Element>();
            sortedElements.Add( name, elementSet );
          }
          elementSet.Add( e );
        }
      }

      // Launch/Get Excel via COM Interop:

      X.Application excel = new X.Application();

      if( null == excel )
      {
        LabUtils.ErrorMsg( "Failed to get or start Excel." );
        return Result.Failed;
      }
      excel.Visible = true;
      X.Workbook workbook = excel.Workbooks.Add( Missing.Value );
      X.Worksheet worksheet;

      //while( 1 < workbook.Sheets.Count ) // we cannot delete all work sheets, excel requires at least one
      //{
      //  worksheet = workbook.Sheets.get_Item(1) as X.Worksheet;
      //  worksheet.Delete();
      //}

      // Loop through all collected categories and create a worksheet for each except first

      List<string> keys = new List<string>( sortedElements.Keys );
      keys.Sort();
      keys.Reverse(); // the worksheet added last shows up first in the excel tab
      bool first = true;
      foreach( string categoryName in keys )
      {
        List<Element> elementSet = sortedElements[categoryName];

        // create and name the worksheet

        if( first )
        {
          worksheet = workbook.Sheets.get_Item( 1 ) as X.Worksheet;
          first = false;
        }
        else
        {
          worksheet = excel.Worksheets.Add( Missing.Value, Missing.Value,
            Missing.Value, Missing.Value ) as X.Worksheet;
        }

        worksheet.Name = ( 31 < categoryName.Length )
          ? categoryName.Substring( 0, 31 )
          : categoryName;

        // we could find the list of Parameter names available for ALL the Elements
        // in this Set, but let's keep it simple and use all parameters encountered:

        List<string> allParamNamesEncountered = new List<string>();

        // loop through all the elements passed to the method

        foreach( Element e in elementSet )
        {
          ParameterSet parameters = e.Parameters;

          // an easier way to loop the parameters than ParameterSetIterator:

          foreach( Parameter parameter in parameters )
          {
            string name = parameter.Definition.Name;
            if( !allParamNamesEncountered.Contains( name ) )
            {
              allParamNamesEncountered.Add( name );
            }
          }
        }
        allParamNamesEncountered.Sort();

        // add the HEADER row in Bold

        worksheet.Cells[1, 1] = "ID";
        int column = 2;

        foreach( string paramName in allParamNamesEncountered )
        {
          worksheet.Cells[1, column] = paramName;
          ++column;
        }
        worksheet.get_Range( "A1", "Z1" ).Font.Bold = true;
        worksheet.get_Range( "A1", "Z1" ).EntireColumn.AutoFit();
        int row = 2;
        foreach( Element e in elementSet )
        {
          // first column is the element id, which we display as an integer

          worksheet.Cells[row, 1] = e.Id.IntegerValue;
          column = 2;
          foreach( string paramName in allParamNamesEncountered )
          {
            string paramValue;
            try
            {
              paramValue = LabUtils.GetParameterValue( e.get_Parameter( paramName ) );
            }
            catch( Exception )
            {
              paramValue = "*NA*";
            }
            worksheet.Cells[row, column] = paramValue;
            ++column;
          }
          ++row;
        } // row
      } // category = worksheet
      return Result.Failed;
    }
  }
  #endregion // Lab4_2_ExportParametersToExcel

  #region Lab4_3_1_CreateAndBindSharedParam
  /// <summary>
  /// Create and bind shared parameter.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab4_3_1_CreateAndBindSharedParam : IExternalCommand
  {
    //
    // what element type are we interested in? the standard SDK FireRating
    // sample uses BuiltInCategory.OST_Doors. we also test using
    // BuiltInCategory.OST_Walls to demonstrate that the same technique
    // works with system families just as well as with standard ones.
    //
    // To test attaching shared parameters to inserted DWG files,
    // which generate their own category on the fly, we also identify
    // the category by category name.
    //
    // The last test is for attaching shared parameters to model groups.
    //
    static public BuiltInCategory Target = BuiltInCategory.OST_Doors;
    //static public BuiltInCategory Target = BuiltInCategory.OST_Walls;
    //static public string Target = "Drawing1.dwg";
    //static public BuiltInCategory Target = BuiltInCategory.OST_IOSModelGroups; // doc.Settings.Categories.get_Item returns null
    //static public string Target = "Model Groups"; // doc.Settings.Categories.get_Item throws an exception SystemInvalidOperationException "Operation is not valid due to the current state of the object."
    //static public BuiltInCategory Target = BuiltInCategory.OST_Lines; // model lines
    //static public BuiltInCategory Target = BuiltInCategory.OST_SWallRectOpening; // Rectangular Straight Wall Openings, case 1260656 [Add Parameters Wall Opening]

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      Application app = uiapp.Application;
      Document doc = uiapp.ActiveUIDocument.Document;
      Category cat = null;

      #region Determine model group category
#if DETERMINE_MODEL_GROUP_CATEGORY
      List<Element> modelGroups = new List<Element>();
      //Filter fType = app.Create.Filter.NewTypeFilter( typeof( Group ) ); // "Binding the parameter to the category Model Groups is not allowed"
      Filter fType = app.Create.Filter.NewTypeFilter( typeof( GroupType ) ); // same result "Binding the parameter to the category Model Groups is not allowed"
      Filter fCategory = app.Create.Filter.NewCategoryFilter( BuiltInCategory.OST_IOSModelGroups );
      Filter f = app.Create.Filter.NewLogicAndFilter( fType, fCategory );
      if ( 0 < doc.get_Elements( f, modelGroups ) )
      {
        cat = modelGroups[0].Category;
      }
#endif // DETERMINE_MODEL_GROUP_CATEGORY
      #endregion // Determine model group category

      if ( null == cat )
      {
        // the category we are defining the parameter for
        try
        {
          cat = doc.Settings.Categories.get_Item( Target );
        }
        catch ( Exception ex )
        {
          message = "Error obtaining the shared param document category: "
            + ex.Message;
          return Result.Failed;
        }
        if ( null == cat )
        {
          message = "Unable to obtain the shared param document category.";
          return Result.Failed;
        }
      }

      // get the current shared params definition file
      DefinitionFile sharedParamsFile = LabUtils.GetSharedParamsFile( app );
      if( null == sharedParamsFile )
      {
        message = "Error getting the shared params file.";
        return Result.Failed;
      }

      // get or create the shared params group
      DefinitionGroup sharedParamsGroup = LabUtils.GetOrCreateSharedParamsGroup(
        sharedParamsFile, LabConstants.SharedParamsGroupAPI );
      if( null == sharedParamsGroup )
      {
        message = "Error getting the shared params group.";
        return Result.Failed;
      }

      // visibility of the new parameter:
      // Category.AllowsBoundParameters property indicates if a category can
      // have shared or project parameters. If it is false, it may not be bound
      // to shared parameters using the BindingMap. Please not that non-user-visible
      // parameters can still be bound to these categories.
      bool visible = cat.AllowsBoundParameters;

      // get or create the shared params definition
      Definition fireRatingParamDef = LabUtils.GetOrCreateSharedParamsDefinition(
        sharedParamsGroup, ParameterType.Number, LabConstants.SharedParamsDefFireRating, visible );
      if( null == fireRatingParamDef )
      {
        message = "Error in creating shared parameter.";
        return Result.Failed;
      }

      // create the category set for binding and add the category
      // we are interested in, doors or walls or whatever:
      CategorySet catSet = app.Create.NewCategorySet();
      try
      {
        catSet.Insert( cat );
      }
      catch( Exception )
      {
        message = string.Format(
          "Error adding '{0}' category to parameters binding set.",
          cat.Name );
        return Result.Failed;
      }

      // bind the param
      try
      {
        Binding binding = app.Create.NewInstanceBinding( catSet );
        // We could check if already bound, but looks like Insert will just ignore it in such case
        doc.ParameterBindings.Insert( fireRatingParamDef, binding );
        // You can also specify the parameter group here:
        //doc.ParameterBindings.Insert( fireRatingParamDef, binding, BuiltInParameterGroup.PG_GEOMETRY );
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return Result.Failed;
      }
      return Result.Succeeded;
    }
  }
  #endregion // Lab4_3_1_CreateAndBindSharedParam

  #region Lab4_3_2_ExportSharedParamToExcel
  /// <summary>
  /// Export all target element ids and their FireRating param values to Excel.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab4_3_2_ExportSharedParamToExcel : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      Application app = uiapp.Application;
      Document doc = uiapp.ActiveUIDocument.Document;
      Category cat = doc.Settings.Categories.get_Item( Lab4_3_1_CreateAndBindSharedParam.Target );
      // Launch Excel (same as in Lab 4_2, so we really should have better created some utils...)
      X.Application excel = new X.ApplicationClass();
      if( null == excel )
      {
        LabUtils.ErrorMsg( "Failed to get or start Excel." );
        return Result.Failed;
      }
      excel.Visible = true;
      X.Workbook workbook = excel.Workbooks.Add( Missing.Value );
      X.Worksheet worksheet;
      //while( 1 < workbook.Sheets.Count )
      //{
      //  worksheet = workbook.Sheets.get_Item( 0 ) as X.Worksheet;
      //  worksheet.Delete();
      //}
      worksheet = excel.ActiveSheet as X.Worksheet;
      worksheet.Name = "Revit " + cat.Name;
      worksheet.Cells[1, 1] = "ID";
      worksheet.Cells[1, 2] = "Level";
      worksheet.Cells[1, 3] = "Tag";
      worksheet.Cells[1, 4] = LabConstants.SharedParamsDefFireRating;
      worksheet.get_Range( "A1", "Z1" ).Font.Bold = true;

      List<Element> elems = LabUtils.GetTargetInstances( doc,
        Lab4_3_1_CreateAndBindSharedParam.Target );

      // Get Shared param Guid

      Guid paramGuid = LabUtils.SharedParamGUID( app,
        LabConstants.SharedParamsGroupAPI, LabConstants.SharedParamsDefFireRating );

      if( paramGuid.Equals( Guid.Empty ) )
      {
        LabUtils.ErrorMsg( "No Shared param found in the file - aborting..." );
        return Result.Failed;
      }

      // Loop through all elements and export each to an Excel row

      int row = 2;
      foreach( Element e in elems )
      {
        worksheet.Cells[row, 1] = e.Id.IntegerValue; // ID
        worksheet.Cells[row, 2] = e.Level.Name; // Level
        // Tag:
        Parameter tagParameter = e.get_Parameter( BuiltInParameter.ALL_MODEL_MARK );
        if( null != tagParameter )
        {
          worksheet.Cells[row, 3] = tagParameter.AsString();
        }
        // FireRating:
        Parameter parameter = e.get_Parameter( paramGuid );
        if( null != parameter )
        {
          worksheet.Cells[row, 4] = parameter.AsDouble();
        }
        ++row;
      }
      return Result.Succeeded;
    }
  }
  #endregion // Lab4_3_2_ExportSharedParamToExcel

  #region Lab4_3_3_ImportSharedParamFromExcel
  /// <summary>
  /// Import updated FireRating param values from Excel.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab4_3_3_ImportSharedParamFromExcel : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication uiapp = commandData.Application;
      Application app = uiapp.Application;
      Document doc = uiapp.ActiveUIDocument.Document;

      //BindingMap bindingMap = doc.ParameterBindings; // slow, fixed in 2009 wu 3, cf. case 1247995

      Guid paramGuid = LabUtils.SharedParamGUID( app,
        LabConstants.SharedParamsGroupAPI, LabConstants.SharedParamsDefFireRating );

      // Let user select the Excel file
      WinForms.OpenFileDialog dlg = new WinForms.OpenFileDialog();
      dlg.Title = "Select source Excel file from which to update Revit shared parameters";
      dlg.Filter = "Excel spreadsheet files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*)|*";
      if( WinForms.DialogResult.OK != dlg.ShowDialog() )
      {
        return Result.Cancelled;
      }
      //
      // Launch/Get Excel via COM Interop:
      //
      X.Application excel = new X.Application();
      if( null == excel )
      {
        LabUtils.ErrorMsg( "Failed to get or start Excel." );
        return Result.Failed;
      }
      excel.Visible = true;
      X.Workbook workbook = excel.Workbooks.Open( dlg.FileName,
        Missing.Value, Missing.Value, Missing.Value,
        Missing.Value, Missing.Value, Missing.Value, Missing.Value,
        Missing.Value, Missing.Value, Missing.Value, Missing.Value,
        Missing.Value, Missing.Value, Missing.Value );
      X.Worksheet worksheet = workbook.ActiveSheet as X.Worksheet;
      //
      // Starting from row 2, loop the rows and extract Id and FireRating param.
      //
      int id;
      double fireRatingValue;
      int row = 2;
      while( true )
      {
        try
        {
          // Extract relevant XLS values
          X.Range r = worksheet.Cells[row, 1] as X.Range;
          if( null == r.Value2 )
          {
            break;
          }
          double d = (double) r.Value2;
          id = (int) d;
          if( 0 >= id )
          {
            break;
          }
          r = worksheet.Cells[row, 4] as X.Range;
          fireRatingValue = (double) r.Value2;
          // Get document's door element via Id
          ElementId elementId = new ElementId( id );
          Element door = doc.get_Element( elementId );
          // Set the param
          if( null != door )
          {
            //Parameter parameter = door.get_Parameter( LabConstants.SharedParamsDefFireRating );
            Parameter parameter = door.get_Parameter( paramGuid );
            parameter.Set( fireRatingValue );
          }
        }
        catch( Exception )
        {
          break;
        }
        ++row;
      }

#if ACTIVATE_REVIT
      //
      // Set focus back to Revit (there may be a better way, but this works :-)
      //

#if USE_PROCESS_GET_PROCESSES
      foreach( Process p in Process.GetProcesses() )
      {
        try
        {
          if( "REVIT" == p.ProcessName.ToUpper().Substring( 0, 5 ) )
          {
            // In VB, we can use AppActivate( p.Id );
            // Pre-3.0, I think you may need to use p/invoke and call the native Windows
            // SetForegroundWindow() function directly.
            // http://www.codeproject.com/csharp/windowhider.asp?df=100
            break;
          }
        }
        catch( Exception )
        {
        }
      }
#endif // USE_PROCESS_GET_PROCESSES

      JtRevitWindow w = new JtRevitWindow();
      w.Activate();
#endif // ACTIVATE_REVIT

      return Result.Succeeded;
    }
  }
  #endregion // Lab4_3_3_ImportSharedParamFromExcel

  #region Lab4_4_1_CreatePerDocParameters
  /// <summary>
  /// Add and bind a visible and an invisible per-doc parameter.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab4_4_1_CreatePerDocParameters : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      if( doc.IsFamilyDocument )
      {
        message = "This command can only be used in a project, not in a family file.";
        return Result.Failed;
      }

      // get the current shared params definition file
      DefinitionFile sharedParamsFile = LabUtils.GetSharedParamsFile( app.Application );
      if( null == sharedParamsFile )
      {
        message = "Error getting the shared params file.";
        return Result.Failed;
      }
      // get or create the shared params group
      DefinitionGroup sharedParamsGroup = LabUtils.GetOrCreateSharedParamsGroup(
        sharedParamsFile, LabConstants.ParamGroupName );

      if( null == sharedParamsGroup )
      {
        message = "Error getting the shared params group.";
        return Result.Failed;
      }
      // visible param
      Definition docParamDefVisible = LabUtils.GetOrCreateSharedParamsDefinition(
        sharedParamsGroup, ParameterType.Integer, LabConstants.ParamNameVisible, true );

      if( null == docParamDefVisible )
      {
        message = "Error creating visible per-doc parameter.";
        return Result.Failed;
      }
      // invisible param
      Definition docParamDefInvisible = LabUtils.GetOrCreateSharedParamsDefinition(
        sharedParamsGroup, ParameterType.Integer, LabConstants.ParamNameInvisible, false );

      if( null == docParamDefInvisible )
      {
        message = "Error creating invisible per-doc parameter.";
        return Result.Failed;
      }
      // bind the param
      try
      {
        CategorySet catSet = app.Application.Create.NewCategorySet();

        catSet.Insert( doc.Settings.Categories.get_Item(
          BuiltInCategory.OST_ProjectInformation ) );

        Binding binding = app.Application.Create.NewInstanceBinding( catSet );
        doc.ParameterBindings.Insert( docParamDefVisible, binding );
        doc.ParameterBindings.Insert( docParamDefInvisible, binding );
      }
      catch( Exception e )
      {
        message = "Error binding shared parameter: " + e.Message;
        return Result.Failed;
      }
      // set the initial values
      // get the singleton project info element
      Element projInfoElem = LabUtils.GetProjectInfoElem( doc );

      if( null == projInfoElem )
      {
        message = "No project info element found. Aborting command...";
        return Result.Failed;
      }
      // for simplicity, access params by name rather than by GUID:
      projInfoElem.get_Parameter( LabConstants.ParamNameVisible ).Set( 55 );
      projInfoElem.get_Parameter( LabConstants.ParamNameInvisible ).Set( 0 );
      return Result.Succeeded;
    }
  }
  #endregion // Lab4_4_1_CreatePerDocParameters

  #region Lab4_4_2_IncrementPerDocParameters
  /// <summary>
  /// Increment the invisible per-doc param.
  /// </summary>
  [Transaction( TransactionMode.Automatic )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab4_4_2_IncrementPerDocParameters : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      Document doc = app.ActiveUIDocument.Document;

      if( doc.IsFamilyDocument )
      {
        message = "This command can only be used in a project, not in a family file.";
        return Result.Failed;
      }

      // get the singleton project info element
      Element projInfoElem = LabUtils.GetProjectInfoElem( doc );
      if( null == projInfoElem )
      {
        message = "No project info elem found. Aborting command...";
        return Result.Failed;
      }

      // For simplicity, access invisible param by name rather than by GUID:

      try
      {
        Parameter param = projInfoElem.get_Parameter( LabConstants.ParamNameInvisible );

        // report OLD value

        int iOldValue = param.AsInteger();
        LabUtils.InfoMsg( "OLD value = " + iOldValue.ToString() );

        // set and report NEW value

        param.Set( iOldValue + 1 );
        LabUtils.InfoMsg( "NEW value = " + param.AsInteger().ToString() );
      }
      catch( System.Exception e )
      {
        message = "Failed: " + e.Message;
        return Result.Failed;
      }
      return Result.Succeeded;
    }
  }
  #endregion // Lab4_4_2_IncrementPerDocParameters
}
