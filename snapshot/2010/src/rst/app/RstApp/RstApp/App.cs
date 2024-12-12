#region Header
//
// App.cs - Revit Structure Demo External APplication
//
// Implement a custom ribbon panel user interface for
//
// RstLink Export
// RstLink Import 
// Rebar Commands
//   Segment
//   Arc1
//   Arc2
//   L-Shape
//   Angled L
// About...
//
// Copyright (C) 2008 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging; // for ribbon, requires references to PresentationCore and WindowsBase .NET assemblies
using Autodesk.Revit;
#endregion // Namespaces

namespace RstApp
{
  public class App : IExternalApplication
  {
    const string root_path = "C:/a/j/adn/train/revit/2010/src/rst";
    //const string root_path = "C:/DevDay2009";

    /// <summary>
    /// Full path of RstLink client application.
    /// </summary>
    const string path_rst_link = root_path + "/link/cs/RstLinkRevitClientCs/bin/RstLinkRevitClient.dll";
    //const string path_rst_link = "C:/DevDay2009/RSTLink/bin/RstLink.dll";

    /// <summary>
    /// Full path of rebar commands application.
    /// </summary>
    //const string path_rebar_commands = "C:/a/lib/revit/2010/SDK/Samples";
    const string path_rebar_commands = root_path + "/rebar/RevitCommands/RevitCommands/bin/Debug/RevitCommands.dll";
    //const string path_rebar_commands = "C:/DevDay2009/Rebar/RevitCommands.dll";

    const string bitmap_name_prefix = "RstApp.img.";
    const string bitmap_name_suffix16 = "16";
    const string bitmap_name_suffix32 = "32";
    const string bitmap_name_suffix = ".bmp";

    const string rebar_class_name_prefix = "Rebar.Cmd";

    /// <summary>
    /// Menu item text for rebar commands.
    /// </summary>
    static string[] rebar_text = new string[] {
      "Segment",
      "Circle",
      "Arc1",
      "Arc2",
      "L-Shape",
      "Angled L",
    };

    /// <summary>
    /// The full external command class name 
    /// is defined by prepending the 
    /// rebar_class_name_prefix 
    /// = "RevitCommands.RvtCmd_CreateNewRebarShape_".
    /// </summary>
    static string[] rebar_class_name = new string[] {
      "SegmentSample",
      "CircleSample",
      "ArcSample1",
      "ArcSample2",
      //"Test",
      //"Simple",
      "LShape",
      "AngledL"
    };

    /// <summary>
    /// The full bitmap resource name is defined by 
    /// prepending the rebar_bitmap_name_prefix = "RstApp.Rebar"
    /// followed by weeither "16" or "32" and ".bmp".
    /// </summary>
    static string[] rebar_bitmap_name = new string[] {
      "Segment",
      "Circle",
      "ArcOne",
      "ArcTwo",
      "Lshape",
      "AngledL",
    };

    static string RebarClassName( int i )
    {
      return rebar_class_name_prefix + rebar_class_name[i];
    }

    static string BitmapName( string name_stem, bool large )
    {
      string size = large
        ? bitmap_name_suffix32
        : bitmap_name_suffix16;

      string name = bitmap_name_prefix
        + name_stem
        + size
        + bitmap_name_suffix;

      return name;
    }

    static string RebarBitmapName( int i, bool large )
    {
      return BitmapName( 
        "Rebar" + rebar_bitmap_name[i], 
        large );
    } 

    /*
    /// <summary>
    /// Gets the ImageSource from the embedded stream.
    /// </summary>
    /// <param name="imageFullPath">The image full path.</param>
    /// <returns>ImageSource</returns>
    private ImageSource GetImageStream( string imageFullPath )
    {    
      Stream stream = GetType().Assembly.GetManifestResourceStream(
        imageFullPath );

      if( null != stream )
      {
        var bitmapDecoder = new PngBitmapDecoder(
          stream, 
          BitmapCreateOptions.PreservePixelFormat, 
          BitmapCacheOption.Default );

        return bitmapDecoder.Frames[0];
      }
      return null;
    }
    */

    /// <summary>
    /// Return embedded bitmap resource.
    /// The 'name' argument is the fully qualified resource name, 
    /// i.e. includes the application namespace etc. 
    /// For example, for an application namespace "MyNamespace" 
    /// and an image "MyImage.png" you would call the function 
    /// like this: 
    /// 
    ///   MyPushButton.LargeImage = GetEmebeddedImage( 
    ///     "MyNamespace.MyImage.png" );
    /// </summary>
    /// <param name="name">Fully qualified resource name, e.g. "MyNamespace.MyImage.png"</param>
    /// <returns>Embedded bitmap resource</returns>
    static BitmapSource GetEmbeddedImage( string name )
    {
      try
      {
        Assembly a = Assembly.GetExecutingAssembly();
        Stream s = a.GetManifestResourceStream( name );
        return BitmapFrame.Create( s );
      }
      catch
      {
        return null;
      }
    }

    static void AddRibbonPanel(
      ControlledApplication a )
    {
      //ButtonData b = new PushButtonData( "One", "One", "C:/R2010/CommandOne.dll", "Redbolts.CommandOne.Command");
      //b.LargeImage = GetImageStream("Redbolts.CommandOne.Images.One24.png");
      //b.Image = GetImageStream("Redbolts.CommandOne.One16.png");

      string path = Assembly.GetExecutingAssembly().Location;

      RibbonPanel panel = a.CreateRibbonPanel(
        "Revit Structure Demo" );

      PushButton pb;

      pb = panel.AddPushButton( 
        "RstLink Export", "RstLink Export", path_rst_link, "RstLinkRevitClient.RSLinkExport" );

      pb.Image = GetEmbeddedImage( BitmapName( "RstLinkExport", false ) );
      pb.LargeImage = GetEmbeddedImage( BitmapName( "RstLinkExport", true ) );
      pb.ToolTip = "Export RST model to external analysis application";

      pb = panel.AddPushButton( 
        "RstLink Import", "RstLink Import", path_rst_link, "RstLinkRevitClient.RSLinkImport" );

      pb.Image = GetEmbeddedImage( BitmapName( "RstLinkImport", false ) );
      pb.LargeImage = GetEmbeddedImage( BitmapName( "RstLinkImport", true ) );
      pb.ToolTip = "Import RST model from external analysis application";

      PulldownButton pulldown = panel.AddPulldownButton(
        "Rebar", "Rebar" );

      pulldown.Image = GetEmbeddedImage( BitmapName( "Rebar", false ) );
      pulldown.LargeImage = GetEmbeddedImage( BitmapName( "Rebar", true ) );
      pulldown.ToolTip = "RST Rebar sample commands";
      //
      // add subitems to the rebar pulldown button:
      //
      int n = rebar_text.Length;
      Debug.Assert( rebar_class_name.Length == n, "expected equal number of rebar command text and class names" );
      Debug.Assert( rebar_bitmap_name.Length == n, "expected equal number of rebar command text and bitmap names" );

      for( int i = 0; i < n; ++i )
      {
        pb = pulldown.AddItem( 
          rebar_text[i],
          path_rebar_commands,
          RebarClassName( i ) );

        pb.ToolTip = rebar_text[i];
        pb.Image = GetEmbeddedImage( RebarBitmapName( i, false ) );
        pb.LargeImage = GetEmbeddedImage( RebarBitmapName( i, true ) );
      }
    }

    public IExternalApplication.Result OnStartup(
      ControlledApplication a )
    {
      AddRibbonPanel( a );
      return IExternalApplication.Result.Succeeded;
    }

    public IExternalApplication.Result OnShutdown(
      ControlledApplication a )
    {
      return IExternalApplication.Result.Succeeded;
    }
  }
}
