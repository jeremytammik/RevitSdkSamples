//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using WinForms = System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Parameters;

namespace Revit.SDK.Samples.AutoParameter.CS
{
  /// <summary>
  /// A class inherits IExternalCommand interface.
  /// this class read parameter data from txt files and add them to the active family document.
  /// </summary>
  public class AddParameterToFamily : IExternalCommand
  {
    // the active Revit application
    private Autodesk.Revit.Application m_app;

    /// <summary>
    /// Implement this method as an external command for Revit.
    /// </summary>
    /// <param name="commandData">An object that is passed to the external application 
    /// which contains data related to the command, 
    /// such as the application object and active view.</param>
    /// <param name="message">A message that can be set by the external application 
    /// which will be displayed if a failure or cancellation is returned by 
    /// the external command.</param>
    /// <param name="elements">A set of elements to which the external application 
    /// can add elements that are to be highlighted in case of failure or cancellation.</param>
    /// <returns>Return the status of the external command. 
    /// A result of Succeeded means that the API external method functioned as expected. 
    /// Cancelled can be used to signify that the user cancelled the external operation 
    /// at some point. Failure should be returned if the application is unable to proceed with 
    /// the operation.</returns>
    public IExternalCommand.Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements)
    {
      m_app = commandData.Application;
      MessageManager.MessageBuff = new StringBuilder();

      try
      {
        bool succeeded = AddParameters();

        if (succeeded)
        {
          ListParameterTypes( m_app.ActiveDocument ); // added by jeremy for case 1251477 [Revit Family Editor API - Detecting Parameter Definition Type]
          return IExternalCommand.Result.Succeeded;
        }
        else
        {
          message = MessageManager.MessageBuff.ToString();
          return IExternalCommand.Result.Failed;
        }
      }
      catch (Exception e)
      {
        message = e.Message;
        return IExternalCommand.Result.Failed;
      }
    }

    /// <summary>
    /// add parameters to the active document
    /// </summary>
    /// <returns>
    /// if succeeded, return true; otherwise false
    /// </returns>
    private bool AddParameters()
    {
      Document doc = m_app.ActiveDocument;
      if (null == doc)
      {
        MessageManager.MessageBuff.Append("There's no available document. \n");
        return false;
      }

      if (!doc.IsFamilyDocument)
      {
        MessageManager.MessageBuff.Append("The active document is not a family document. \n");
        return false;
      }

      FamilyParameterAssigner assigner = new FamilyParameterAssigner(m_app, doc);
      // the parameters to be added are defined and recorded in a text file, read them from that file and load to memory
      bool succeeded = assigner.LoadParametersFromFile();
      if (!succeeded)
      {
        return false;
      }

      doc.BeginTransaction();
      succeeded = assigner.AddParameters();
      if (succeeded)
      {
        doc.EndTransaction();
        return true;
      }
      else
      {
        doc.AbortTransaction();
        return false;
      }
    }

    #region ListParameterTypes added by Jeremy for case 1251477
    enum DefinitionType
    {
      BuiltIn, Family, Shared
    };

    /// <summary>
    /// Names of parameters that we are interested in classifying:
    /// </summary>
    string[] _parameter_names = new string[] {
      "Cost", // parameter would set sDefinitionType to "built-in"
      "FamilyParam1", // parameter would set sDefinitionType to "family"
      "Shared_Length" // parameter would set sDefinitionType to "shared"
    };

    Dictionary<string, DefinitionType> GetFamilyParameterTypes(
      List<string> parameter_names,
      Document family_doc )
    {
      Dictionary<string, DefinitionType> types
        = new Dictionary<string, DefinitionType>(
          parameter_names.Count );

      foreach( FamilyParameter p in
        family_doc.FamilyManager.Parameters )
      {
        Definition d = p.Definition;
        ExternalDefinition e = d as ExternalDefinition;
        InternalDefinition i = d as InternalDefinition;

        Debug.Assert( null == e ^ null == i,
          "expected either external or internal definition" );

        BuiltInParameter invalid = BuiltInParameter.INVALID;

        DefinitionType t = ( null != e ) ? DefinitionType.Shared
          : ( invalid == i.BuiltInParameter ) ? DefinitionType.Family
          : DefinitionType.BuiltIn;

        if( parameter_names.Contains( d.Name ) )
        {
          types[d.Name] = t;
        }
      }
      return types;
    }

    Dictionary<string, List<Guid>> GetSharedParameterTypesGuids(
      List<string> parameter_names )
    {
      Dictionary<string, List<Guid>> guids
       = new Dictionary<string, List<Guid>>( parameter_names.Count );

      DefinitionFile f = m_app.OpenSharedParameterFile();

      foreach( DefinitionGroup g in f.Groups )
      {
        int n = g.Definitions.Size;
        Debug.Print(
          "Group '{0}' contains {1} definition{2}:",
          g.Name, n, ( 1 == n ? "" : "s" ) );

        foreach( Definition d in g.Definitions )
        {
          ExternalDefinition e = d as ExternalDefinition;
          if( null != e )
          {
            Debug.Print(
              "  '{0}': external, {1}, {2}, {3}, {4}",
              e.Name, e.ParameterGroup, e.ParameterType,
              e.OwnerGroup.Name, e.GUID );

            if( parameter_names.Contains( e.Name ) )
            {
              if( !guids.ContainsKey( e.Name ) )
              {
                guids[e.Name] = new List<Guid>();
              }
              guids[e.Name].Add( e.GUID );
            }
          }
          else
          {
            InternalDefinition i = d as InternalDefinition;

            Debug.Assert( null != i,
              "expected definition to be either internal or external" );

            Debug.Print(
              "  '{0}': {1}, {2}, {3}",
              i.Name, i.ParameterGroup,
              i.ParameterType, i.BuiltInParameter );
          }
        }
      }
      return guids;
    }

    void ListParameterTypes( Document family_doc )
    {
      List<string> parameter_names
       = new List<string>( _parameter_names );

      Dictionary<string, DefinitionType> types 
        = GetFamilyParameterTypes( parameter_names, family_doc );

      Dictionary<string, List<Guid>> guids 
        = GetSharedParameterTypesGuids( parameter_names );

      string report = string.Empty;
      List<string> keys = new List<string>( types.Keys );
      keys.Sort();
      foreach( string key in keys )
      {
        DefinitionType t = guids.ContainsKey( key )
          ? DefinitionType.Shared
          : types[key];
        report += string.Format( "\r\n{0}: {1}", key, t );
      }
      if( 0 < report.Length )
      {
        report = report.Substring( 2 );
      }
      Debug.Print( report );
      WinForms.MessageBox.Show( report, 
        "Family Parameter Types" );
    }

    /*
    /// <summary>
    /// Template file to use to create a new temporary in-memory project:
    /// </summary>
    const string _template_file_path = "C:/Documents and Settings/All Users/Application Data/Autodesk/RAC 2010/Metric Templates/DefaultMetric.rte";

    void GetParameterTypes( Document family_doc )
    {
      Document doc = m_app.NewProjectDocument( _template_file_path );
      Family family = family_doc.LoadFamily( doc );
    }
    */
    #endregion // ListParameterTypes added by Jeremy for case 1251477

  } // end of class "AddParameterToFamily"

  /// <summary>
  /// A class inherits IExternalCommand interface.
  /// this class read parameter data from txt files and add them to the family files in a folder.
  /// </summary>
  public class AddParameterToFamilies : IExternalCommand
  {
    // the active Revit application
    private Autodesk.Revit.Application m_app;

    /// <summary>
    /// Implement this method as an external command for Revit.
    /// </summary>
    /// <param name="commandData">An object that is passed to the external application 
    /// which contains data related to the command, 
    /// such as the application object and active view.</param>
    /// <param name="message">A message that can be set by the external application 
    /// which will be displayed if a failure or cancellation is returned by 
    /// the external command.</param>
    /// <param name="elements">A set of elements to which the external application 
    /// can add elements that are to be highlighted in case of failure or cancellation.</param>
    /// <returns>Return the status of the external command. 
    /// A result of Succeeded means that the API external method functioned as expected. 
    /// Cancelled can be used to signify that the user cancelled the external operation 
    /// at some point. Failure should be returned if the application is unable to proceed with 
    /// the operation.</returns>
    public IExternalCommand.Result Execute(ExternalCommandData commandData,
                              ref string message,
                              ElementSet elements)
    {
      m_app = commandData.Application;
      MessageManager.MessageBuff = new StringBuilder();

      try
      {
        bool succeeded = LoadFamiliesAndAddParameters();

        if (succeeded)
        {
          return IExternalCommand.Result.Succeeded;
        }
        else
        {
          message = MessageManager.MessageBuff.ToString();
          return IExternalCommand.Result.Failed;
        }
      }
      catch (Exception e)
      {
        message = e.Message;
        return IExternalCommand.Result.Failed;
      }
    }

    /// <summary>
    /// search for the family files and the corresponding parameter records
    /// load each family file, add parameters and then save and close.
    /// </summary>
    /// <returns>
    /// if succeeded, return true; otherwise false
    /// </returns>
    private bool LoadFamiliesAndAddParameters()
    {
      bool succeeded = true;

      List<string> famFilePaths = new List<string>();

      Environment.SpecialFolder myDocumentsFolder = Environment.SpecialFolder.MyDocuments;
      string myDocs = Environment.GetFolderPath(myDocumentsFolder);
      string families = myDocs + "\\AutoParameter_Families";
      if (!Directory.Exists(families))
      {
        MessageManager.MessageBuff.Append("The folder [AutoParameter_Families] doesn't exist in [MyDocuments] folder.\n");
      }
      DirectoryInfo familiesDir = new DirectoryInfo(families);
      FileInfo[] files = familiesDir.GetFiles("*.rfa");
      if (0 == files.Length)
      {
        MessageManager.MessageBuff.Append("No family file exists in [AutoParameter_Families] folder.\n");
      }
      foreach (FileInfo info in files)
      {
        if (info.IsReadOnly)
        {
          MessageManager.MessageBuff.Append("Family file: \"" + info.FullName + "\" is read only. Can not add parameters to it.\n");
          continue;
        }

        string famFilePath = info.FullName;
        Document doc = m_app.OpenDocumentFile(famFilePath);

        if (!doc.IsFamilyDocument)
        {
          succeeded = false;
          MessageManager.MessageBuff.Append("Document: \"" + famFilePath + "\" is not a family document.\n");
          continue;
        }
        
        // return and report the errors
        if (!succeeded)
        {
          return false;
        }

        FamilyParameterAssigner assigner = new FamilyParameterAssigner(m_app, doc);
        // the parameters to be added are defined and recorded in a text file, read them from that file and load to memory
        succeeded = assigner.LoadParametersFromFile();
        if (!succeeded)
        {
          MessageManager.MessageBuff.Append("Failed to load parameters from parameter files.\n");
          return false;
        }
        doc.BeginTransaction();
        succeeded = assigner.AddParameters();
        if (succeeded)
        {
          doc.EndTransaction();
          doc.Save();
          doc.Close();
        }
        else
        {
          doc.AbortTransaction();
          doc.Close();
          MessageManager.MessageBuff.Append("Failed to add parameters to " + famFilePath + ".\n");
          return false;
        }
      }
      return true;
    }
  } // end of class "AddParameterToFamilies"

  /// <summary>
  /// store the warning/error messeges when executing the sample
  /// </summary>
  static class MessageManager
  {
    static StringBuilder m_messageBuff = new StringBuilder();
    /// <summary>
    /// store the warning/error messages
    /// </summary>
    public static StringBuilder MessageBuff
    {
      get 
      { 
        return m_messageBuff; 
      }
      set 
      { 
        m_messageBuff = value; 
      }
    }
  }
}
