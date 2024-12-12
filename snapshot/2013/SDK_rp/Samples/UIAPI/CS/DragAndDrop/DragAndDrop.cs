//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.UIAPI.CS
{
  public partial class FurnitureFamilyDragAndDropForm : System.Windows.Forms.Form
  {
    public static BuiltInCategory FamilyCategory
    {
      get
      {
        return BuiltInCategory.OST_Furniture;
      }
    }

    private static FurnitureFamilyDragAndDropForm s_form;
    public static FurnitureFamilyDragAndDropForm GetTheForm( Document doc )
    {
      if( s_form == null )
      {
        s_form = new FurnitureFamilyDragAndDropForm( doc );
      }
      s_form.UpdateLoadedFamilies();
      return s_form;
    }

    /// <summary>
    /// Display class for the external family file listbox.
    /// </summary>
    private class FamilyListBoxMember
    {
      public string FullPath;
      public string Name;
      public FamilyListBoxMember( string fullPath, string name )
      {
        FullPath = fullPath;
        Name = name;
      }

      public override String ToString()
      {
        return Name;
      }
    }

    private Document m_document;

    /// <summary>
    /// Construct and populate the form.
    /// </summary>
    /// <param name="doc"></param>
    private FurnitureFamilyDragAndDropForm( Document doc )
    {
      InitializeComponent();
      m_document = doc;

      UpdateFamilyFileList();
    }

    private void UpdateFamilyFileList()
    {
      // Visit each Revit library looking for Furniture families
      IDictionary<String, String> libraryPaths = m_document.Application.GetLibraryPaths();
      foreach( String libraryPath in libraryPaths.Values )
      {
        foreach( String directory in System.IO.Directory.EnumerateDirectories( libraryPath, "*Furniture", SearchOption.AllDirectories ) )
        {
          foreach( String familyFile in System.IO.Directory.EnumerateFiles( directory, "*.rfa", SearchOption.AllDirectories ) )
          {
            // Add each Furniture family to the listbox
            String fileName = Path.GetFileName( familyFile );
            FamilyListBoxMember member = new FamilyListBoxMember( familyFile, fileName );
            listBox1.Items.Add( member );
          }
        }
      }
    }

    private void UpdateLoadedFamilies()
    {
      ListView.ListViewItemCollection collection = listView1.Items;
      collection.Clear();

      // Setup list view with loaded families
      ImageList imageList = new ImageList();
      Size size = new Size( 50, 50 );
      imageList.ImageSize = size;

      FilteredElementCollector collector = new FilteredElementCollector( m_document );
      collector.OfCategory( FamilyCategory );
      collector.OfClass( typeof( FamilySymbol ) );

      foreach( FamilySymbol familySymbol in collector.Cast<FamilySymbol>() )
      {
        ListViewItem item = new ListViewItem();
        item.Tag = familySymbol.Id;
        item.Text = familySymbol.Family.Name + "::" + familySymbol.Name;
        item.ToolTipText = "Drag to place instances of " + item.Text + " in the active document.";

        Bitmap bitmap = familySymbol.GetPreviewImage( size );

        if( bitmap != null )
        {
          imageList.Images.Add( bitmap );
          int index = imageList.Images.Count - 1;
          item.ImageIndex = index;
        }

        collection.Add( item );
      }

      listView1.LargeImageList = imageList;
    }

    // Drag action from list view
    private void listView_MouseMove( 
      object sender, 
      MouseEventArgs e )
    {
      if( System.Windows.Forms.Control.MouseButtons 
        == MouseButtons.Left )
      {
        ListViewItem selectedItem = this.listView1
          .SelectedItems.Cast<ListViewItem>()
          .FirstOrDefault<ListViewItem>();

        if( selectedItem != null )
        {
          // Use custom Revit drag and drop behavior

          LoadedFamilyDropHandler myhandler 
            = new LoadedFamilyDropHandler();

          UIApplication.DoDragDrop( 
            selectedItem.Tag, myhandler );
        }
      }
    }

    // Drag action from list box
    private void listBox1_MouseMove( 
      object sender, 
      MouseEventArgs e )
    {
      if( System.Windows.Forms.Control.MouseButtons 
        == MouseButtons.Left )
      {
        FamilyListBoxMember member 
          = (FamilyListBoxMember) listBox1.SelectedItem;

        // Use standard Revit drag and drop behavior

        List<String> data = new List<String>();
        data.Add( member.FullPath );
        UIApplication.DoDragDrop( data );
      }
    }

    private void FurnitureFamilyDragAndDropForm_FormClosed( object sender, FormClosedEventArgs e )
    {
      s_form = null;
    }
  }


  /// <summary>
  /// Custom handler for placement of loaded family types
  /// </summary>
  public class LoadedFamilyDropHandler : IDropHandler
  {
    public void Execute( 
      UIDocument doc, 
      object data )
    {
      ElementId familySymbolId = (ElementId) data;

      FamilySymbol symbol = doc.Document.GetElement( 
        familySymbolId ) as FamilySymbol;

      if( symbol != null )
      {
        doc.PromptForFamilyInstancePlacement( 
          symbol );
      }
    }
  }
}
