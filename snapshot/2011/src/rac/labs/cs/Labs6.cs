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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using WinForms = System.Windows.Forms;
using System.Windows.Media.Imaging; // for ribbon, requires references to PresentationCore and WindowsBase .NET assemblies
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using DocumentSavingEventArgs = Autodesk.Revit.DB.Events.DocumentSavingEventArgs;
#endregion // Namespaces

namespace Labs
{
  #region SampleAccessibilityCheck IExternalCommandAvailability
  public class SampleAccessibilityCheck
    : IExternalCommandAvailability
  {
    public bool IsCommandAvailable(
      UIApplication applicationData,
      CategorySet selectedCategories )
    {
      // Allow button click if there is
      // no active selection

      if( selectedCategories.IsEmpty )
        return true;

      // Allow button click if there is
      // at least one wall selected

      foreach( Category c in selectedCategories )
      {
        if( c.Id.IntegerValue
          == ( int ) BuiltInCategory.OST_Walls )
          return true;
      }
      return false;
    }
  }
  #endregion // SampleAccessibilityCheck IExternalCommandAvailability

  #region Lab6_1_HelloWorldExternalApplication
  /// <summary>
  /// A minimal external application saying hello.
  /// Explain the concept of an external application and its minimal interface.
  /// Explore how to create a manifest file for an external application.
  /// </summary>
  [Regeneration( RegenerationOption.Manual )]
  public class Lab6_1_HelloWorldExternalApplication : IExternalApplication
  {
    /// <summary>
    /// This method will run automatically when Revit start up.
    /// It shows a message box. Initiative task can be done in this function.
    /// </summary>
    public Result OnStartup(
      UIControlledApplication a )
    {
      LabUtils.InfoMsg( "Hello World from an external application in C#." );
      return Result.Succeeded;
    }

    public Result OnShutdown(
      UIControlledApplication a )
    {
      return Result.Succeeded;
    }
  }
  #endregion // Lab6_1_HelloWorldExternalApplication

  #region Lab6_2_Ribbon
  /// <summary>
  /// Add various controls to the Revit ribbon.
  /// </summary>
  [Regeneration( RegenerationOption.Manual )]
  public class Lab6_2_Ribbon : IExternalApplication
  {
    public Result OnStartup(
      UIControlledApplication a )
    {
      try
      {
        CreateRibbonItems( a );
      }
      catch ( Exception ex )
      {
        LabUtils.InfoMsg( ex.Message );
        return Result.Failed;
      }
      return Result.Succeeded;
    }

    public Result OnShutdown(
      UIControlledApplication a )
    {
      return Result.Succeeded;
    }

    void CreateRibbonItems( UIControlledApplication a )
    {
      // get the path of our dll
      string addInPath = GetType().Assembly.Location;
      string imgDir = Path.Combine( Path.GetDirectoryName( addInPath ), "img" );

      const string panelName = "Lab 6 Panel";

      const string cmd1 = "Labs.Lab1_1_HelloWorld";
      const string name1 = "HelloWorld";
      const string text1 = "Hello World";
      const string tooltip1 = "Run Lab1_1_HelloWorld command";
      const string img1 = "ImgHelloWorld.png";
      const string img31 = "ImgHelloWorldSmall.png";

      const string cmd2 = "Labs.Lab1_2_CommandArguments";
      const string name2 = "CommandArguments";
      const string text2 = "Command Arguments";
      const string tooltip2 = "Run Lab1_2_CommandArguments command";
      const string img2 = "ImgCommandArguments.png";
      const string img32 = "ImgCommandArgumentsSmall.png";

      const string name3 = "Lab1Commands";
      const string text3 = "Lab 1 Commands";
      const string tooltip3 = "Run a Lab 1 command";
      const string img33 = "ImgCommandSmall.png";

      // create a Ribbon Panel

      RibbonPanel panel = a.CreateRibbonPanel( panelName );

      // add a button for Lab1's Hello World command

      PushButtonData pbd = new PushButtonData( name1, text1, addInPath, cmd1 );
      PushButton pb1 = panel.AddItem( pbd ) as PushButton;
      pb1.ToolTip = tooltip1;
      pb1.LargeImage = new BitmapImage( new Uri( Path.Combine( imgDir, img1 ) ) );

      // add a vertical separation line in the panel
      panel.AddSeparator();

      // prepare data for creating stackable buttons
      PushButtonData pbd1 = new PushButtonData( name1 + " 2", text1, addInPath, cmd1 );
      pbd1.ToolTip = tooltip1;
      pbd1.Image = new BitmapImage( new Uri( Path.Combine( imgDir, img31 ) ) );

      PushButtonData pbd2 = new PushButtonData( name2, text2, addInPath, cmd2 );
      pbd2.ToolTip = tooltip2;
      pbd2.Image = new BitmapImage( new Uri( Path.Combine( imgDir, img32 ) ) );

      PulldownButtonData pbd3 = new PulldownButtonData( name3, text3 );
      pbd3.ToolTip = tooltip3;
      pbd3.Image = new BitmapImage( new Uri( Path.Combine( imgDir, img33 ) ) );

      // add stackable buttons
      IList<RibbonItem> ribbonItems = panel.AddStackedItems( pbd1, pbd2, pbd3 );

      // add two push buttons as sub-items of the Lab 1 commands
      PulldownButton pb3 = ribbonItems[2] as PulldownButton;

      pbd = new PushButtonData( name1, text1, addInPath, cmd1 );
      PushButton pb3_1 = pb3.AddPushButton( pbd );
      pb3_1.ToolTip = tooltip1;
      pb3_1.LargeImage = new BitmapImage( new Uri( Path.Combine( imgDir, img1 ) ) );

      pbd = new PushButtonData( name2, text2, addInPath, cmd2 );
      PushButton pb3_2 = pb3.AddPushButton( pbd );
      pb3_2.ToolTip = tooltip2;
      pb3_2.LargeImage = new BitmapImage( new Uri( Path.Combine( imgDir, img2 ) ) );
    }
  }
  #endregion // Lab6_2_Ribbon

  #region Lab6_3_PreventSaveEvent
  /// <summary>
  /// This external application subscribes to, handles,
  /// and unsubscribes from the document saving event.
  /// Using the event mechnism pre-event features, some actions can be prevented.
  /// This sample displays a message dialog to let the user decide whether to save changes to the document.
  /// Note: The document must have been already saved to disk prior to running this application.
  /// </summary>
  [Regeneration( RegenerationOption.Manual )]
  public class Lab6_3_PreventSaveEvent : IExternalApplication
  {
    public Result OnStartup(
      UIControlledApplication a )
    {
      try
      {
        // subscribe to the DocumentSaving event:
        a.ControlledApplication.DocumentSaving
          += new EventHandler<DocumentSavingEventArgs>(
            a_DocumentSaving );
      }
      catch ( Exception ex )
      {
        LabUtils.InfoMsg( ex.Message );
        return Result.Failed;
      }
      return Result.Succeeded;
    }

    public Result OnShutdown(
      UIControlledApplication a )
    {
      // remove the event subscription:
      a.ControlledApplication.DocumentSaving
        -= new EventHandler<DocumentSavingEventArgs>(
          a_DocumentSaving );
      return Result.Succeeded;
    }

    // Show a message to decide whether to save the document.
    void a_DocumentSaving(
      object obj,
      DocumentSavingEventArgs args )
    {
      // Ask whether to prevent from saving:

      args.Cancel = args.Cancellable
        && LabUtils.QuestionMsg(
          "Saving event handler was triggered.\r\n"
          + "Using the pre-event mechanism, we can cancel the save.\r\n"
          + "Continue saving the document?" );
    }
  }
  #endregion //Lab6_3_PreventSaveEvent

  #region Lab6_4_DismissDialog
  /// <summary>
  /// This external application subscribes to, handles,
  /// and unsubscribe from the dialog box showing event.
  /// Its can dismiss the "Family already exists" dialog when
  /// reloading an already loaded family, and overwrite the existing version.
  /// It dismisses message box by simulating a click on the "Yes" button.
  /// In addition, it can dismiss the property dialog.
  /// </summary>
  [Regeneration( RegenerationOption.Manual )]
  public class Lab6_4_DismissDialog : IExternalApplication
  {
    public Result OnStartup(
      UIControlledApplication a )
    {
      a.DialogBoxShowing
        += new EventHandler<DialogBoxShowingEventArgs>(
          DismissDialog );

      return Result.Succeeded;
    }

    public Result OnShutdown(
      UIControlledApplication a )
    {
      a.DialogBoxShowing
        -= new EventHandler<DialogBoxShowingEventArgs>(
          DismissDialog );

      return Result.Succeeded;
    }

    void DismissDialog(
      object sender,
      DialogBoxShowingEventArgs e )
    {
      TaskDialogShowingEventArgs te = e as TaskDialogShowingEventArgs;
      if ( te != null )
      {
        if ( te.DialogId == "TaskDialog_Family_Already_Exists" )
        {
          // In this task dialog, 1001 maps to the first button,
          // which is the "override the existing version"
          int iReturn = 1001;

          // Set OverrideResult argument to 1001 mimic clicking the first button.
          e.OverrideResult( iReturn );

          // 1002 maps the second button in this dialog.
          // DialogResult.Cancel maps to the cancel button.
        }
      }
      else
      {
        MessageBoxShowingEventArgs msgArgs = e as MessageBoxShowingEventArgs;
        if ( null != msgArgs ) // this is a message box
        {
          e.OverrideResult( (int) WinForms.DialogResult.Yes );
          Debug.Print( "Dialog id is {0}\r\nMessage is {1}",
            msgArgs.HelpId, msgArgs.Message );
        }
        else // this is some other dialog, for example, element property dialog.
        {
          //Use the HelpId to identify the dialog.
          if ( e.HelpId == 1002 ) // Element property dialog's HelpId is 1002
          {
            e.OverrideResult( (int) WinForms.DialogResult.No );
            Debug.Print(
              "We just dismissed the element property dialog "
              + "and set the return value to No." );
          }
        }
      }
    }
  }
  #endregion //Lab6_4_DismissDialog

  #region Lab6_5_RibbonExplorer
  /// <summary>
  /// List the contents of the Revit ribbon in the Visual Studio debug output window.
  /// </summary>
  [Transaction( TransactionMode.ReadOnly )]
  [Regeneration( RegenerationOption.Manual )]
  public class Lab6_5_RibbonExplorer : IExternalCommand
  {
    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      UIApplication app = commandData.Application;
      List<RibbonPanel> panels = app.GetRibbonPanels();
      foreach( RibbonPanel panel in panels )
      {
        Debug.Print( panel.Name );
        IList<RibbonItem> items = panel.GetItems();
        foreach ( RibbonItem item in items )
        {
          RibbonItemType t = item.ItemType;
          if ( RibbonItemType.PushButton == t )
          {
            PushButton b = item as PushButton;
            Debug.Print( "  {0} : {1}", item.ItemText, b.Name );
          }
          else
          {
            Debug.Assert( RibbonItemType.PulldownButton == t, "expected pulldown button" );
            PulldownButton b = item as PulldownButton;
            Debug.Print( "  {0} : {1}", item.ItemText, b.Name );
            foreach ( RibbonItem item2 in b.GetItems() )
            {
              Debug.Assert( RibbonItemType.PushButton == item2.ItemType, "expected push button in pulldown menu" );
              Debug.Print( "    {0} : {1}", item2.ItemText, ((PushButton)item2).Name );
            }
          }
        }
      }
      return Result.Failed;
    }
  }
  #endregion // Lab6_5_RibbonExplorer
}
