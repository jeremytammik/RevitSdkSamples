#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2008 by Autodesk, Inc.
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using WinForms = System.Windows.Forms;
using System.Reflection;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;

// Added reference to Microsoft Excel 11.0 Object Library
// http://support.microsoft.com/kb/306023/
// http://support.microsoft.com/kb/302084/EN-US/
using X = Microsoft.Office.Interop.Excel;
#endregion // Namespaces

namespace Labs
{
  #region Lab4_1_ParametersForSelectedObjects
  /// <summary>
  /// List all parameters for selected elements.
  /// </summary>
  public class Lab4_1_ParametersForSelectedObjects : IExternalCommand
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

    public IExternalCommand.Result Execute(
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

      Document doc = commandData.Application.ActiveDocument;
      // Loop all elements in the selection
      foreach( Element elem in doc.Selection.Elements )
      {
        Element elem2 = elem; // enable us to assign to elem2 in case analyseTypeParameters == true
        string s = string.Empty;

        // set this variable to false to analyse the element's own parameters,
        // i.e. instance parameters for a family instance, and set it to true
        // to analyse a family instance's type parameters:
        bool analyseTypeParameters = false;
        if( analyseTypeParameters )
        {
          if( elem2 is FamilyInstance )
          {
            FamilyInstance inst = elem2 as FamilyInstance;
            if( null != inst.Symbol )
            {
              elem2 = inst.Symbol;
              s = " type";
            }
          }
          else if( elem2 is Wall )
          {
            Wall wall = elem2 as Wall;
            if( null != wall.WallType )
            {
              elem2 = wall.WallType;
              s = " type";
            }
          }
        }

        // Loop through and list all UI-visible element parameters
        string sMsg = elem2.Parameters.Size.ToString() + s
          + " parameters for the selected " + elem2.Category.Name
          + " (" + elem2.Id.Value.ToString() + "):\r\n";
        foreach( Parameter param in elem2.Parameters )
        {
          string name = param.Definition.Name;
          string type = param.StorageType.ToString();
          string value = LabUtils.GetParameterValue2( param, doc );
          sMsg += "\r\n  Name=" + name + "; Type=" + type + "; Value=" + value;
        }
        LabUtils.InfoMsg( sMsg );

        // If we know WHICH param we are looking for, then:
        // A) If a standard parameter, we can get it via BuiltInParam signature of Parameter method:
        Parameter parBuiltIn;
        try
        {
          parBuiltIn = elem.get_Parameter( BuiltInParameter.FAMILY_BASE_LEVEL_OFFSET_PARAM );
          if( null == parBuiltIn )
          {
            LabUtils.InfoMsg( "FAMILY_BASE_LEVEL_OFFSET_PARAM is NOT available for this element." );
          }
          else
          {
            string parBuiltInName = parBuiltIn.Definition.Name;
            string parBuiltInType = parBuiltIn.StorageType.ToString();
            string parBuiltInValue = LabUtils.GetParameterValue2( parBuiltIn, doc );
            LabUtils.InfoMsg( "FAMILY_BASE_LEVEL_OFFSET_PARAM: Name=" + parBuiltInName
              + "; Type=" + parBuiltInType + "; Value=" + parBuiltInValue );
          }
        }
        catch( Exception )
        {
          LabUtils.InfoMsg( "FAMILY_BASE_LEVEL_OFFSET_PARAM is NOT available for this element." );
        }

        // B) For a shared parameter, we can get it via "GUID" signature 
        // of Parameter method  ... this will be shown later in Labs 4 ...

        // C) or we can get the parameter by name, since Revit 2009 ... 
        // previously we had to use a utility method to loop over all 
        // parameters and search for the name (this works for either 
        // standard or shared):
        const string paramName = "Base Offset";
        Parameter parByName = elem.get_Parameter( paramName );
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
        List<string> a = GetParameters( doc, elem2 );
        foreach( string s2 in a )
        {
          Debug.WriteLine( s2 );
        }
#endif // TEST_2
        #endregion // TEST_2

      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab4_1_ParametersForSelectedObjects

  #region Lab4_2_ExportParametersToExcel
  /// <summary>
  /// Export all parameters for each model element to Excel, one sheet per category.
  /// </summary>
  public class Lab4_2_ExportParametersToExcel : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      // extract and group the data from Revit in a dictionary, where 
      // the key is the category name and the value is a list of elements.
      //Autodesk.Revit.Collections.Map sortedElements = app.Create.NewMap();
      Dictionary<string, List<Element>> sortedElements = new Dictionary<string, List<Element>>();
      // iterate all non-symbol elements and store in dictionary
      ElementIterator iter = doc.Elements;
      while( iter.MoveNext() )
      {
        Element element = iter.Current as Element;
        if( !(element is Symbol) )
        {
          Category category = element.Category;
          if( null != category )
          {
            List<Element> elementSet;
            // If we already have this Key, get its Value (Set)
            // Otherwise, create the new Key and Value (Set
            if( sortedElements.ContainsKey( category.Name ) )
            {
              elementSet = sortedElements[category.Name];
            }
            else
            {
              elementSet = new List<Element>();
              sortedElements.Add( category.Name, elementSet );
            }
            // Add the element to the Set
            elementSet.Add( element );
          }
        }
      }

      // Launch/Get Excel via COM Interop:
      X.Application excel = new X.Application();
      if( null == excel )
      {
        LabUtils.ErrorMsg( "Failed to get or start Excel." );
        return IExternalCommand.Result.Failed;
      }
      excel.Visible = true;
      X.Workbook workbook = excel.Workbooks.Add( Missing.Value );
      X.Worksheet worksheet;
      //while( 1 < workbook.Sheets.Count ) // we cannot delete all work sheets, excel requires at least one
      //{
      //  worksheet = workbook.Sheets.get_Item(1) as X.Worksheet;
      //  worksheet.Delete();
      //}
      // Loop all collected Categories and create one worksheet for each except first
      //KeyCollection 
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
        worksheet.Name = categoryName;

        // we could find the list of Parameter names available for ALL the Elements 
        // in this Set, but let's keep it simple and use all parameters encountered:
        //Autodesk.Revit.Collections.Set allParamNamesEncountered = app.Create.NewSet();
        List<string> allParamNamesEncountered = new List<string>();
        // loop through all the elements passed to the method
        foreach( Element el in elementSet )
        {
          ParameterSet parameters = el.Parameters;
          if( !parameters.IsEmpty )
          {
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
        foreach( Element elem in elementSet )
        {
          // first column is the element id, which we display as an integer
          worksheet.Cells[row, 1] = elem.Id.Value;
          column = 2;
          foreach( string paramName in allParamNamesEncountered )
          {
            string paramValue;
            try
            {
              paramValue = LabUtils.GetParameterValue( elem.get_Parameter( paramName ) );
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
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab4_2_ExportParametersToExcel

  #region Lab4_3_1_CreateAndBindSharedParam
  /// <summary>
  /// 4.3.1 Create and bind shared parameter.
  /// </summary>
  public class Lab4_3_1_CreateAndBindSharedParam : IExternalCommand
  {
    //
    // what element type are we interested in? the standard SDK FireRating
    // sample uses BuiltInCategory.OST_Doors. we also test using
    // BuiltInCategory.OST_Walls to demonstrate that the same technique
    // works with system families just as well as with standard ones.
    //
    // Finally, to test attaching shared parameters to inserted DWG files,
    // which generate their own category on the fly, we also identify the 
    // category by category name.
    //
    static public BuiltInCategory Target = BuiltInCategory.OST_Doors;
    //static public BuiltInCategory Target = BuiltInCategory.OST_Walls;
    //static public string Target = "Drawing1.dwg";

    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      // Get the current Shared Params Definition File
      DefinitionFile sharedParamsFile = LabUtils.GetSharedParamsFile( app );
      if( null == sharedParamsFile )
      {
        LabUtils.ErrorMsg( "Error getting the shared params file." );
        return IExternalCommand.Result.Failed;
      }
      // Get or Create the Shared Params Group
      DefinitionGroup sharedParamsGroup = LabUtils.GetOrCreateSharedParamsGroup( sharedParamsFile, LabConstants.gsSharedParamsGroupAPI );
      if( null == sharedParamsGroup )
      {
        LabUtils.ErrorMsg( "Error getting the shared params group." );
        return IExternalCommand.Result.Failed;
      }
      // Get or Create the Shared Params Definition
      Definition fireRatingParamDef = LabUtils.GetOrCreateSharedParamsDefinition( sharedParamsGroup, ParameterType.Number, LabConstants.gsSharedParamsDefFireRating, true );
      if( null == fireRatingParamDef )
      {
        LabUtils.ErrorMsg( "Error in creating shared parameter." );
        return IExternalCommand.Result.Failed;
      }
      // create the category set for binding and add the category we are 
      // interested in, doors or walls or whatever:
      CategorySet catSet = app.Create.NewCategorySet();
      Category cat = doc.Settings.Categories.get_Item( Target );
      try
      {
        catSet.Insert( cat );
      }
      catch( Exception )
      {
        LabUtils.ErrorMsg(string.Format(
          "Error adding '{0}' category to parameters binding set.", cat.Name));
        return IExternalCommand.Result.Failed;
      }
      // Bind the Param
      try
      {
        Binding binding = app.Create.NewInstanceBinding( catSet );
        // We could check if already bound, but looks like Insert will just ignore it in such case
        doc.ParameterBindings.Insert( fireRatingParamDef, binding );
      }
      catch( Exception )
      {
        LabUtils.ErrorMsg( "Error binding shared parameter" );
        return IExternalCommand.Result.Failed;
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab4_3_1_CreateAndBindSharedParam

  #region Lab4_3_2_ExportSharedParamToExcel
  /// <summary>
  /// 4.3.2 Export all door ids and FireRating param values to Excel.
  /// </summary>
  public class Lab4_3_2_ExportSharedParamToExcel : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;
      Category cat = doc.Settings.Categories.get_Item( Lab4_3_1_CreateAndBindSharedParam.Target );
      // Launch Excel (same as in Lab 4_2, so we really should have better created some utils...)
      X.Application excel = new X.ApplicationClass();
      if( null == excel )
      {
        LabUtils.ErrorMsg( "Failed to get or start Excel." );
        return IExternalCommand.Result.Failed;
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
      worksheet.Cells[1, 4] = LabConstants.gsSharedParamsDefFireRating;
      worksheet.get_Range( "A1", "Z1" ).Font.Bold = true;

      // since walls are not standard system families, implement and use
      // GetAllModelInstancesForACategory() instead to support both doors
      // and walls:
      //ElementSet doors = LabUtils.GetAllStandardFamilyInstancesForACategory( app, cat.Name );
      List<Element> elems = LabUtils.GetTargetInstances( app, Lab4_3_1_CreateAndBindSharedParam.Target );
      // Get Shared param Guid
      Guid paramGuid = LabUtils.SharedParamGUID( 
        app, LabConstants.gsSharedParamsGroupAPI, LabConstants.gsSharedParamsDefFireRating );
      if( paramGuid.Equals( Guid.Empty ) )
      {
        LabUtils.ErrorMsg( "No Shared param found in the file - aborting..." );
        return IExternalCommand.Result.Failed;
      }
      // Loop all elements and export each to an Excel row
      int row = 2;
      foreach( Element elem in elems ) {
        worksheet.Cells[row, 1] = elem.Id.Value; // ID
        worksheet.Cells[row, 2] = elem.Level.Name; // Level
        // Tag:
        Autodesk.Revit.Parameter tagParameter = elem.get_Parameter( BuiltInParameter.ALL_MODEL_MARK );
        if( null != tagParameter )
        {
          worksheet.Cells[row, 3] = tagParameter.AsString();
        }
        // FireRating:
        Parameter parameter = elem.get_Parameter( paramGuid );
        if( null != parameter )
        {
          worksheet.Cells[row, 4] = parameter.AsDouble();
        }
        ++row;
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab4_3_2_ExportSharedParamToExcel

  #region Lab4_3_3_ImportSharedParamFromExcel
  /// <summary>
  /// 4.3.3 Import updated FireRating param values from Excel.
  /// </summary>
  public class Lab4_3_3_ImportSharedParamFromExcel : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      //BindingMap bindingMap = doc.ParameterBindings; // slow, fixed in 2009 wu 3, cf. case 1247995

      Guid paramGuid = LabUtils.SharedParamGUID(
        app, LabConstants.gsSharedParamsGroupAPI, LabConstants.gsSharedParamsDefFireRating );

      // Let user select the Excel file
      WinForms.OpenFileDialog dlg = new WinForms.OpenFileDialog();
      dlg.Title = "Select source Excel file from which to update Revit shared parameters";
      dlg.Filter = "Excel spreadsheet files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*)|*";
      if( WinForms.DialogResult.OK != dlg.ShowDialog() )
      {
        return IExternalCommand.Result.Cancelled;
      }
      //
      // Launch/Get Excel via COM Interop:
      //
      X.Application excel = new X.Application();
      if( null == excel )
      {
        LabUtils.ErrorMsg( "Failed to get or start Excel." );
        return IExternalCommand.Result.Failed;
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
          ElementId elementId;
          elementId.Value = id;
          Element door = doc.get_Element( ref elementId );
          // Set the param
          if( null != door )
          {
            //Parameter parameter = door.get_Parameter( LabConstants.gsSharedParamsDefFireRating );
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

      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab4_3_3_ImportSharedParamFromExcel

  #region Lab4_4_1_CreatePerDocParameters
  /// <summary>
  /// Command to add and bind a visible and an invisible per-doc parameter.
  /// </summary>
  public class Lab4_4_1_PerDocParams : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;

      // Get the current Shared Params Definition File
      DefinitionFile sharedParamsFile = LabUtils.GetSharedParamsFile( app );
      if( null == sharedParamsFile )
      {
        LabUtils.ErrorMsg( "Error getting the shared params file." );
        return IExternalCommand.Result.Failed;
      }
      // Get or Create the Shared Params Group
      DefinitionGroup sharedParamsGroup = LabUtils.GetOrCreateSharedParamsGroup( sharedParamsFile, LabConstants.sParamGroupName );
      if( null == sharedParamsGroup )
      {
        LabUtils.ErrorMsg( "Error getting the shared params group." );
        return IExternalCommand.Result.Failed;
      }
      //  Visible param
      Definition docParamDefVisible = LabUtils.GetOrCreateSharedParamsDefinition( sharedParamsGroup, ParameterType.Integer, LabConstants.sParamNameVisible, true );
      if( null == docParamDefVisible )
      {
        LabUtils.ErrorMsg( "Error creating visible per-doc parameter." );
        return IExternalCommand.Result.Failed;
      }
      // INVisible param
      Definition docParamDefInvisible = LabUtils.GetOrCreateSharedParamsDefinition( sharedParamsGroup, ParameterType.Integer, LabConstants.sParamNameInvisible, false );
      if( null == docParamDefInvisible )
      {
        LabUtils.ErrorMsg( "Error creating invisible per-doc parameter." );
        return IExternalCommand.Result.Failed;
      }
      // Bind the Param
      try
      {
        Document doc = app.ActiveDocument;
        CategorySet catSet = app.Create.NewCategorySet();
        catSet.Insert( doc.Settings.Categories.get_Item( BuiltInCategory.OST_ProjectInformation ) );
        Binding binding = app.Create.NewInstanceBinding( catSet );
        doc.ParameterBindings.Insert( docParamDefVisible, binding );
        doc.ParameterBindings.Insert( docParamDefInvisible, binding );
      }
      catch( System.Exception e )
      {
        LabUtils.ErrorMsg( "Error binding shared parameter: " + e.Message );
        return IExternalCommand.Result.Failed;
      }
      // Set the initial values
      // Get the singleton Project Info Element
      Element projInfoElem = LabUtils.GetProjectInfoElem( app );

      if( null == projInfoElem )
      {
        LabUtils.ErrorMsg( "No project info elem found. Aborting command..." );
        return IExternalCommand.Result.Failed;
      }
      //  For simplicity, access params by name rather than by GUID:
      projInfoElem.get_Parameter( LabConstants.sParamNameVisible ).Set( 55 );
      projInfoElem.get_Parameter( LabConstants.sParamNameInvisible ).Set( 0 );
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab4_4_1_CreatePerDocParameters

  #region Lab4_4_2_IncrementPerDocParameters
  /// <summary>
  /// Command to increment the invisible per-doc param
  /// </summary>
  public class Lab4_4_2_PerDocParams : IExternalCommand
  {
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Application app = commandData.Application;
      // Get the singleton Project Info Element
      Element projInfoElem = LabUtils.GetProjectInfoElem( app );
      if( null == projInfoElem )
      {
        LabUtils.ErrorMsg( "No project info elem found. Aborting command..." );
        return IExternalCommand.Result.Failed;
      }
      // For simplicity, access invisible param by name rather than by GUID:
      try
      {
        Parameter param = projInfoElem.get_Parameter( LabConstants.sParamNameInvisible );
        // report OLD value
        int iOldValue = param.AsInteger();
        LabUtils.ErrorMsg( "OLD value = " + iOldValue.ToString() );
        // set and report NEW value
        param.Set( iOldValue + 1 );
        LabUtils.ErrorMsg( "NEW value = " + param.AsInteger().ToString() );
      }
      catch( System.Exception e )
      {
        LabUtils.ErrorMsg( "Failed: " + e.Message );
        return IExternalCommand.Result.Failed;
      }
      return IExternalCommand.Result.Succeeded;
    }
  }
  #endregion // Lab4_4_2_IncrementPerDocParameters
}
