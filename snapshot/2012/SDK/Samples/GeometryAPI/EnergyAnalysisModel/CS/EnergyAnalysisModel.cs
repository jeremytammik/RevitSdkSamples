//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.EnergyAnalysisModel.CS
{
  public class EnergyAnalysisModel
  {
    // An EnergyAnalysisDetailModel member that can get all analysis data includes surfaces, spaces and openings.
    private EnergyAnalysisDetailModel m_energyAnalysisDetailModel;
    // Options for Energy Analysis process
    private EnergyAnalysisDetailModelOptions m_options;
    // revit document
    private Document RevitDoc;

    // Options Property
    public EnergyAnalysisDetailModelOptions Options
    {
      get
      {
        return m_options;
      }
      set
      {
        m_options = value;
      }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="doc">Revit Document</param>
    public EnergyAnalysisModel( Document doc )
    {
      RevitDoc = doc;
      m_options = new EnergyAnalysisDetailModelOptions();
    }

    /// <summary>
    /// Get EnergyAnalysisDetailModel object and Initialize it.
    /// </summary>
    public void Initialize()
    {
      // create the model with a document and options.
      m_energyAnalysisDetailModel
        = EnergyAnalysisDetailModel.Create( RevitDoc, m_options );

      m_energyAnalysisDetailModel.TransformModel();
    }

    /// <summary>
    /// This method get all openings surfaces from current model
    /// </summary>
    /// <returns>XElement that places openings surfaces</returns>
    public XElement GetAnalyticalOpenings()
    {
      // openings for the first EnergyAnalysisDetailModel whose openings should not be merged
      XElement openingsNode = new XElement( "OpeningsModels" );
      openingsNode.Add( new XAttribute( "Name", "OpeningsModels" ) );

      // get EnergyAnalysisOpenings from Model1
      IList<EnergyAnalysisOpening> openings
        = m_energyAnalysisDetailModel.GetAnalyticalOpenings();

      foreach( EnergyAnalysisOpening opening in openings )
      {
        XElement openNode = new XElement( "Open" );
        openNode.Add( new XAttribute( "Name", opening.Name ) );
        // add individual opening node to whol openings node
        openingsNode.Add( openNode );

        // get surfaces from opening
        EnergyAnalysisSurface openingSurface
          = opening.GetAnalyticalSurface();

        if( null == openingSurface )
          continue;
        XElement surfaceNode = new XElement( "Surface" );
        surfaceNode.Add( new XAttribute( "Name", openingSurface.Name ) );
        openNode.Add( surfaceNode );
      }

      // return the whole openings node
      return openingsNode;
    }

    /// <summary>
    /// This method get all Analytical ShadingSurfaces from current model
    /// </summary>
    /// <returns>XElement that places shading surfaces</returns>
    public XElement GetAnalyticalShadingSurfaces()
    {
      // create a node that places all shading surfaces
      XElement shadingSurfacesNode = new XElement( "ShadingSurfaces1" );
      shadingSurfacesNode.Add( new XAttribute( "Name", "ShadingSurfaces" ) );

      // get shadingSurfaces from Model
      IList<EnergyAnalysisSurface> shadingSurfaces
        = m_energyAnalysisDetailModel.GetAnalyticalShadingSurfaces();

      SurfacesToXElement( shadingSurfacesNode, shadingSurfaces );

      return shadingSurfacesNode;
    }

    /// <summary>
    /// Extract Analytical data about Space and its surfaces
    /// </summary>
    /// <returns>XElment that includes all data about AnalyticalSpace</returns>
    public XElement GetAnalyticalSpaces()
    {
      // create a node that place all spaces.
      XElement energyAnalysisSpacesNode = new XElement( "AnalyticalSpaces" );
      energyAnalysisSpacesNode.Add( new XAttribute( "Name", "AnalyticalSpaces" ) );
      // get EnergyAnalysisSpaces from m_energyAnalysisDetailModel
      IList<EnergyAnalysisSpace> energyAnalysisSpaces
        = m_energyAnalysisDetailModel.GetAnalyticalSpaces();

      // get surface from each Space
      foreach( EnergyAnalysisSpace space in energyAnalysisSpaces )
      {
        XElement spaceNode = new XElement( "Space" );
        spaceNode.Add( new XAttribute( "Name", space.ComposedName ) );
        // add individual space node to spaces collection node
        energyAnalysisSpacesNode.Add( spaceNode );

        IList<EnergyAnalysisSurface> analyticalSurfaces = space.GetAnalyticalSurfaces();
        SurfacesToXElement( spaceNode, analyticalSurfaces );
      }
      // return the whole Spaces Node
      return energyAnalysisSpacesNode;
    }

    /// <summary>
    /// The method adds given surfaces to specific XElement
    /// </summary>
    /// <param name="node">Parent node</param>
    /// <param name="analyticalSurfaces">The surfaces list that will be added into the para node</param>
    private void SurfacesToXElement( XElement node, IList<EnergyAnalysisSurface> analyticalSurfaces )
    {
      // go through all surfaces
      foreach( EnergyAnalysisSurface surface in analyticalSurfaces )
      {
        XElement surfaceNode = new XElement( "Surface" );
        surfaceNode.Add( new XAttribute( "Name", surface.Name ) );
        // add individual surface node to parent node
        node.Add( surfaceNode );
      }
    }

    /// <summary>
    /// Get Analytical data and pass them to UI controls
    /// </summary>
    /// <param name="treeView"></param>
    public void RefreshAnalysisData( TreeView treeView )
    {
      treeView.Nodes.Clear();

      //treeView.Nodes adds first level node
      TreeNode node = new TreeNode( "BuildingModel" );
      treeView.Nodes.Add( node );

      // append space surfaces node
      TreeNode spaceNode = XElementToTreeNode( GetAnalyticalSpaces() );
      node.Nodes.Add( spaceNode );

      // append opening surfaces node
      TreeNode openingNode = XElementToTreeNode( GetAnalyticalOpenings() );
      node.Nodes.Add( openingNode );

      // append shading surfaces node
      TreeNode shadingNode = XElementToTreeNode( GetAnalyticalShadingSurfaces() );
      node.Nodes.Add( shadingNode );
    }

    /// <summary>
    /// This method converts XElement nodes to Tree nodes so that analysis data could be displayed in UI treeView
    /// </summary>
    /// <param name="element">XElement to be converted</param>
    /// <returns>Tree Node that comes from XElement</returns>
    private TreeNode XElementToTreeNode( XElement element )
    {
      if( null == element.FirstAttribute )
        return null;
      TreeNode node = new TreeNode( element.FirstAttribute.Value );
      if( !element.HasElements )
        // return if it is leaf node
        return node;
      // convert its child elements
      foreach( XElement ele in element.Elements() )
      {
        node.Nodes.Add( XElementToTreeNode( ele ) );
      }
      // return whole node
      return node;
    }

    /// <summary>
    /// This method converts UI selected string to EnergyAnalysisDetailModelTier enum
    /// </summary>
    /// <param name="tierValue">Selected string from UI</param>
    public void SetTier( String tierValue )
    {
      switch( tierValue )
      {
        case "Final":
          m_options.Tier = EnergyAnalysisDetailModelTier.Final;
          break;
        case "FirstLevelBoundaries":
          m_options.Tier = EnergyAnalysisDetailModelTier.FirstLevelBoundaries;
          break;
        case "NotComputed":
          m_options.Tier = EnergyAnalysisDetailModelTier.NotComputed;
          break;
        case "SecondLevelBoundaries":
          m_options.Tier = Autodesk.Revit.DB.Analysis.EnergyAnalysisDetailModelTier.SecondLevelBoundaries;
          break;
        // the default Tier is SecondLevelBoundaries
        default:
          m_options.Tier = Autodesk.Revit.DB.Analysis.EnergyAnalysisDetailModelTier.SecondLevelBoundaries;
          break;
      }
    }
  }
}
