#region Header
//
// CmdLandXml.cs - import LandXML data and create TopographySurface
//
// Copyright (C) 2010 by Jeremy Tammik,
// Autodesk Inc. All rights reserved.
//
#endregion // Header

#region Namespaces
using System;
using System.Collections.Generic;
using System.Xml;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Geometry;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
using W = System.Windows.Forms;
#endregion // Namespaces

namespace BuildingCoder
{
  class CmdLandXml : IExternalCommand
  {
    public CmdResult Execute( 
      ExternalCommandData commandData, 
      ref string message, 
      ElementSet elements )
    {
      Application app = commandData.Application;
      Document doc = app.ActiveDocument;

      W.OpenFileDialog dlg = new W.OpenFileDialog();

      // select file to open

      dlg.Filter = "LandXML files (*.xml)|*.xml";

      dlg.Title = "Import LandXML and "
        + "Create TopographySurface";

      if( dlg.ShowDialog() != W.DialogResult.OK )
      {
        return CmdResult.Cancelled;
      }

      XmlDocument xmlDoc = new XmlDocument();
      xmlDoc.Load( dlg.FileName );

      XmlNodeList pnts 
        = xmlDoc.GetElementsByTagName( "Pnts" );

      char[] separator = new char[] { ' ' };
      double x = 0, y = 0, z = 0;
      XYZ xyz;

      //List<XYZ> pntList = new List<XYZ>();
      XYZArray pts = app.Create.NewXYZArray();

      for( int k = 0; k < pnts.Count; ++k )
      {
        for( int i = 0; 
          i < pnts[k].ChildNodes.Count; ++i )
        {
          int j = 1;

          string text = pnts[k].ChildNodes[i].InnerText;
          string[] coords = text.Split( separator );

          foreach( string coord in coords )
          {
            switch( j )
            {
              case 1:
                x = Double.Parse( coord );
                break;
              case 2:
                y = Double.Parse( coord );
                break;
              case 3:
                z = Double.Parse( coord );
                break;
              default:
                break;
            }
            j++;
          }
          xyz = new XYZ( x, y, z );
          //pntList.Add( xyz );
          pts.Append( xyz );
        }
      }

      //TopographySurface surface 
      //  = doc.Create.NewTopographySurface( pntList );

      TopographySurface surface 
        = doc.Create.NewTopographySurface( pts );

      return CmdResult.Succeeded;
    }
  }
}
