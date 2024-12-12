#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2008 by Jeremy Tammik, Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  
// AUTODESK, INC. DOES NOT WARRANT THAT THE OPERATION OF THE 
// PROGRAM WILL BE UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject
// to restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
#endregion // Header

#region Namespaces
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using W = System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using CmdResult = Autodesk.Revit.IExternalCommand.Result;
#endregion // Namespaces

namespace mep
{
  /// <summary>
  /// Command to change the size of diffusers based on their flow.
  /// </summary>
  //
  // Air Terminal Schedule 2 in the CFM per SF Color Fill 2.rvt 
  // has a value in the Comments field that indicates the 
  // size that the air terminal should be changed to… this is just a 
  // sample suggestion and dataset.
  //
  // The UI for this can be pretty simple for #6. Perhaps a table that 
  // lists min/max values, and the associated size that the diffuser 
  // neck should be sized to, i.e:
  //
  // 100-150 cfm – 6 inch
  // 151-275 cfm – 8 inch
  // 275-400 cfm – 10 inch
  //
  // 2007-08-02:
  //
  // new procedure: 
  // 
  class CmdChangeSize : IExternalCommand
  {
    struct SymbMinMax
    {
      public FamilySymbol Symbol;
      public double Min;
      public double Max;
    }

    #region Obsolete 2008 element iteration
    bool ChangeDiffuserSize2008( Application app )
    {
      Document doc = app.ActiveDocument;
      //
      // iterate over all air terminal families and determine the min and max flow assigned to each type:
      //
      Category categoryAirTerminal = doc.Settings.Categories.get_Item( BuiltInCategory.OST_DuctTerminal );
      Hashtable dictFamilyToSymbols = new Hashtable();
      {
        WaitCursor waitCursor = new WaitCursor();
        // loop all model elements to find the corresponding Families/Types
        ElementIterator iter = doc.Elements;
        Element elem;
        while( iter.MoveNext() )
        {
          elem = iter.Current as Element;
          if( elem is Family )
          {
            Family fam = elem as Family;
            bool categoryMatches = false;
            foreach( FamilySymbol symb in fam.Symbols )
            {
              categoryMatches = ( null != symb.Category && symb.Category.Equals( categoryAirTerminal ) );
              break; // only first one needed
            }
            // if match, store the data:
            if( categoryMatches )
            {
              // New ArrayList for the Value
              ArrayList familySymbols = new ArrayList();
              // Loop all Symbols
              foreach( FamilySymbol symb in fam.Symbols )
              {
                SymbMinMax a = new SymbMinMax();
                a.Symbol = symb;
                a.Min = Util.GetParameterValueFromName( symb, ParameterName.MinFlow );
                a.Max = Util.GetParameterValueFromName( symb, ParameterName.MaxFlow );
                familySymbols.Add( a );
              }
              // Add the Key-Value pair to the dictionary
              dictFamilyToSymbols.Add( fam.Name, familySymbols );
            }
          } // Got Family
        } // All elements
      }
      //
      // prompt user to select which families to process:
      //
      ArrayList familyNames = new ArrayList( dictFamilyToSymbols.Keys );
      familyNames.Sort();
      FamilySelector fs = new FamilySelector( familyNames );
      if( W.DialogResult.OK == fs.ShowDialog() )
      {
        WaitCursor waitCursor = new WaitCursor();
        ElementSet terminals = Util.GetSupplyAirTerminals( app );
        foreach( FamilyInstance terminal in terminals )
        {
          string familyName = terminal.Symbol.Family.Name;
          if( fs.IsChecked( familyName ) )
          {
            bool found = false;
            ArrayList familySymbols = dictFamilyToSymbols[familyName] as ArrayList;
            double flow = Util.GetTerminalFlowParameter( terminal ).AsDouble();
            foreach( SymbMinMax a in familySymbols )
            {
              //
              // pick the first symbol found which matches our flow;
              // todo: this could be improved:
              // 1. we could sort the symbols by flow, and that would speed up the search
              // 2. we could report an error if multiple possible assignments are availabe
              // 3. we could improve the handling of borderline cases ... tend towards the smaller or bigger?
              // 4. we could build in a check after building each familySymbols ArrayList to ensure
              //    that there is no ovelap in the flows and that the entire required flow spectrum is covered
              //
              if( a.Min <= flow && flow <= a.Max )
              {
                terminal.Symbol = a.Symbol;
                found = true;
                break;
              }
            }
            if( !found )
            {
              double flowInCfm = flow * Const.SecondsPerMinute;
              Util.ErrorMsg( string.Format( "No matching flow found for {0} with flow {1}.",
                Util.ElementDescription( terminal ), flowInCfm ) );
            }
          }
        }
      }
      return true;
    }
    #endregion // Obsolete 2008 element iteration

    bool ChangeDiffuserSize2009( Application app )
    {
      Document doc = app.ActiveDocument;
      //
      // iterate over all air terminal families and determine 
      // the min and max flow assigned to each type.
      //
      // for each family, create a list of all its symbols 
      // with their respective min and max flows. collect 
      // these lists in a map keyed by family name:
      //
      Dictionary<string, List<SymbMinMax>> dictFamilyToSymbols = new Dictionary<string, List<SymbMinMax>>();
      {
        WaitCursor waitCursor = new WaitCursor();
        Category categoryAirTerminal = doc.Settings.Categories.get_Item( BuiltInCategory.OST_DuctTerminal );
        ElementId categoryId = categoryAirTerminal.Id;
        ElementIterator it = doc.get_Elements( typeof( Family ) );
        while( it.MoveNext() )
        {
          Family family = it.Current as Family;
          // family category is not implemented, so check the symbols instead:
          bool categoryMatches = false;
          foreach( FamilySymbol symbol in family.Symbols )
          {
            // in 2008 and even 2009 beta 1, you could compare categories directly:
            //categoryMatches = ( null != symbol.Category && symbol.Category.Equals( categoryAirTerminal ) );
            // in 2009, check the category id instead:
            categoryMatches = ( null != symbol.Category && symbol.Category.Id.Equals( categoryId ) );
            break; // we only need to check the first one
          }
          if( categoryMatches )
          {
            List<SymbMinMax> familySymbols = new List<SymbMinMax>();
            foreach( FamilySymbol symbol in family.Symbols )
            {
              SymbMinMax a = new SymbMinMax();
              a.Symbol = symbol;
              a.Min = Util.GetParameterValueFromName( symbol, ParameterName.MinFlow );
              a.Max = Util.GetParameterValueFromName( symbol, ParameterName.MaxFlow );
              familySymbols.Add( a );
            }
            dictFamilyToSymbols.Add( family.Name, familySymbols );
          }
        }
      }
      //
      // prompt user to select which families to process:
      //
      //List<string> familyNames = new List<string>( dictFamilyToSymbols.Count );
      //foreach( string s in dictFamilyToSymbols.Keys )
      //{
      //  familyNames.Add( string.Format( "{0}({1})", s, dictFamilyToSymbols[s].Count ) );
      //}
      List<string> familyNames = new List<string>( dictFamilyToSymbols.Keys );
      familyNames.Sort();
      FamilySelector fs = new FamilySelector( familyNames );
      if( W.DialogResult.OK == fs.ShowDialog() )
      {
        WaitCursor waitCursor = new WaitCursor();
        ElementSet terminals = Util.GetSupplyAirTerminals( app );
        int n = terminals.Size;
        string s = "{0} of " + n.ToString() + " terminals processed...";
        string caption = "Change Diffuser Size";
        using( ProgressForm pf = new ProgressForm( caption, s, n ) )
        {
          foreach( FamilyInstance terminal in terminals )
          {
            string familyName = terminal.Symbol.Family.Name;
            if( fs.IsChecked( familyName ) )
            {
              bool found = false;
              List<SymbMinMax> familySymbols = dictFamilyToSymbols[familyName];
              double flow = Util.GetTerminalFlowParameter( terminal ).AsDouble();
              foreach( SymbMinMax a in familySymbols )
              {
                //
                // pick the first symbol found which matches our flow;
                // todo: this could be improved:
                // 1. we could sort the symbols by flow, and that would speed up the search
                // 2. we could report an error if multiple possible assignments are availabe
                // 3. we could improve the handling of borderline cases ... tend towards the smaller or bigger?
                // 4. we could build in a check after building each familySymbols ArrayList to ensure
                //    that there is no ovelap in the flows and that the entire required flow spectrum is covered
                //
                if( a.Min <= flow && flow <= a.Max )
                {
                  terminal.Symbol = a.Symbol;
                  found = true;
                  break;
                }
              }
              if( !found )
              {
                double flowInCfm = flow * Const.SecondsPerMinute;
                Util.ErrorMsg( string.Format( "No matching flow found for {0} with flow {1}.",
                  Util.ElementDescription( terminal ), flowInCfm ) );
              }
            }
            pf.Increment();
          }
        }
      }
      return true;
    }

    #region Execute Command
    /// <summary>
    /// Execute the command to change the size of diffusers based on their flow.
    /// </summary>
    public CmdResult Execute(
      ExternalCommandData commandData,
      ref String message,
      ElementSet elements )
    {
      try
      {
        Application app = commandData.Application;
        bool rc = Const.UseRevitApiFilters
          ? ChangeDiffuserSize2009( app )
          : ChangeDiffuserSize2008( app );
        return CmdResult.Succeeded;
      }
      catch( Exception ex )
      {
        message = ex.Message;
        return CmdResult.Failed;
      }
    }
    #endregion // Execute Command
  }
}
