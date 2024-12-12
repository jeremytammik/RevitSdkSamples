using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Collections;
using RView = Autodesk.Revit.DB.View;
using RApplication = Autodesk.Revit.ApplicationServices.Application;

namespace Revit.SDK.Samples.UIAPI.CS
{
  public partial class PreviewModel : System.Windows.Forms.Form
  {
    public PreviewModel( RApplication application, ElementId viewId )
    {
      InitializeComponent();
      _application = application;
      _uiApplication = new UIApplication( application );
      _dbDocument = _uiApplication.ActiveUIDocument.Document;

      updateDocumentList( _dbDocument );
      updateViewsList( _uiApplication.ActiveUIDocument.ActiveView.Id );
    }

    private void updateViewsList( ElementId viewId )
    {
      // fill the combobox with printable views <name + id>
      FilteredElementCollector collector = new FilteredElementCollector( _dbDocument );
      collector.OfClass( typeof( Autodesk.Revit.DB.View ) );
      IEnumerable<Autodesk.Revit.DB.View> secs = from Element f in collector where ( f as Autodesk.Revit.DB.View ).CanBePrinted == true select f as Autodesk.Revit.DB.View;
      _cbViews.Items.Clear();
      DBViewItem activeItem = null;
      foreach( Autodesk.Revit.DB.View dbView in secs )
      {
        if( viewId == null || viewId.IntegerValue < 0 )
        {
          activeItem = new DBViewItem( dbView, _dbDocument );
          viewId = dbView.Id;
        }
        if( dbView.Id == viewId )
        {
          activeItem = new DBViewItem( dbView, _dbDocument );
          _cbViews.Items.Add( activeItem );
        }
        else
          _cbViews.Items.Add( new DBViewItem( dbView, _dbDocument ) );
      }
      _cbViews.SelectedItem = activeItem;
    }

    private void updateDocumentList( Document selectedDocument )
    {
      // fill the documents to the comboxbox _cbDocuments.
      DBDocumentItem activeItem = null;
      _cbDocuments.Items.Clear();
      DocumentSetIterator docIter = _application.Documents.ForwardIterator();
      docIter.Reset();
      while( docIter.MoveNext() )
      {
        Document dbDoc = docIter.Current as Document;
        String documentName = null;
        DBDocumentItem item = null;
        if( dbDoc != null )
        {
          if( dbDoc.IsFamilyDocument )
          {
            item = new DBDocumentItem( dbDoc.PathName, dbDoc );
          }
          else
          {
            String projName = dbDoc.ProjectInformation.Name;
            if( String.IsNullOrEmpty( projName ) || projName.ToLower().CompareTo( "project name" ) == 0 )
            {
              if( String.IsNullOrEmpty( dbDoc.PathName ) )
                documentName = projName;
              else
                documentName = new System.IO.FileInfo( dbDoc.PathName ).Name;
            }
            else
              documentName = projName;

            item = new DBDocumentItem( documentName, dbDoc );

          }
          if( dbDoc.Equals( selectedDocument ) )
          {
            _dbDocument = selectedDocument;
            activeItem = item;
          }
          _cbDocuments.Items.Add( item );
        }
      }
      _cbDocuments.Items.Add( new DBDocumentItem() );
      _cbDocuments.SelectedItem = activeItem;
    }

    private void cbViews_SelIdxChanged( object sender, EventArgs e )
    {
      System.Windows.Forms.ComboBox cb = sender as System.Windows.Forms.ComboBox;
      if( cb == null )
        return;

      DBViewItem dbItem = cb.SelectedItem as DBViewItem;
      if( dbItem == null )
        return;

      //if (_currentDBViewId == null)
      //    return;

      //RView currentView = _dbDocument.get_Element(_currentDBViewId) as RView;
      //if(currentView == null)
      //    return;

      //if (dbItem.UniqueId.ToLower().CompareTo(currentView.UniqueId.ToLower()) != 0)
      //    return;

      PreviewControl vc = _elementHostWPF.Child 
        as PreviewControl;

      if( vc != null )
        vc.Dispose();

      _elementHostWPF.Child = new PreviewControl( _dbDocument, dbItem.Id );
      _currentDBViewId = dbItem.Id;
    }


    private ElementId _currentDBViewId = null;
    private Document _dbDocument = null;
    private RApplication _application = null;
    private UIApplication _uiApplication = null;

    private void cbDocs_SelIdxChanged( object sender, EventArgs e )
    {
      DBDocumentItem documentItem = _cbDocuments.SelectedItem as DBDocumentItem;
      if( documentItem.Document == _dbDocument )
        return;

      if( documentItem.IsNull )
      {
        OpenFileDialog ofd = new OpenFileDialog();
        ofd.DefaultExt = "rvt";
        ofd.Filter = "Revit project files (*.rvt)|*.rvt|Revit family files (*.rfa)|*.rfa|Revit family template files (*.rft)|*.rft";
        if( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
        {
          try
          {
            _dbDocument = _application.OpenDocumentFile( ofd.FileName );
          }
          catch( System.Exception )
          {

          }
          if( _dbDocument != null )
          {
            updateDocumentList( _dbDocument );
            updateViewsList( null );
          }
        }
        else
        {
          // the combobox should show the current document item.
          String documentName;
          String projName = _dbDocument.ProjectInformation.Name;
          if( String.IsNullOrEmpty( projName ) || projName.ToLower().CompareTo( "project name" ) == 0 )
          {
            if( String.IsNullOrEmpty( _dbDocument.PathName ) )
              documentName = projName;
            else
              documentName = new System.IO.FileInfo( _dbDocument.PathName ).Name;
          }
          else
            documentName = projName;

          foreach( DBDocumentItem dbItem in _cbDocuments.Items )
          {
            if( dbItem.Name.ToLower().CompareTo( documentName.ToLower() ) == 0 )
            {
              _cbDocuments.SelectedItem = dbItem;
              break;
            }
          }
        }
      }
      else
      {
        _dbDocument = documentItem.Document;
        updateViewsList( null );
      }
    }
  }

  public class DBViewItem
  {
    public DBViewItem( RView dbView, Document dbDoc )
    {
      ElementType viewType = dbDoc.GetElement( dbView.GetTypeId() ) as ElementType;
      Name = viewType.Name + " " + dbView.Name;
      Id = dbView.Id;
      UniqueId = dbView.UniqueId;
    }

    public override String ToString()
    {
      return Name;
    }

    public String Name { get; set; }

    public ElementId Id { get; set; }

    public String UniqueId { get; set; }
  }

  public class DBDocumentItem
  {
    public DBDocumentItem( String name, Document doc )
    {
      Name = name;
      Document = doc;
      IsNull = false;
    }

    public DBDocumentItem()
    {
      IsNull = true;
    }

    public override string ToString()
    {
      if( IsNull )
        return "<Open Document...>";
      return Name;
    }

    public bool IsNull { get; set; }
    public String Name { get; set; }
    public Document Document { get; set; }
  }
}
