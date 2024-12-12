#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2013 by Jeremy Tammik, Autodesk, Inc.
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

namespace AdnRme
{
  class ParameterName
  {
    //
    // used for display only
    //
    public const string CalculatedSupplyAirFlow = "Calculated Supply Air Flow";
    //
    // The "CFM per SF" is a project parameter, and as such, there is no enumeration for this;
    // In 2009, "Calculated Supply Airflow per area" exists, built-in parameter
    // ROOM_CALCULATED_SUPPLY_AIRFLOW_PER_AREA_PARAM:
    //
    public const string CfmPerSf = "CFM per SF";
    //
    // The other issue regarding the two 'Flow' params is quite simple ... 
    // the Flow we are accessing is not a Built-in-Parameter of the Air Terminal itself ... 
    // the Flow is simply a Family Parameter that is mapped to the Connector on the Air Terminal ... 
    // I would guess that the 'Flow' that is the built-in-parameter is actually the Flow on the connector 
    // (and/or - perhaps it is the Flow parameter of a Duct object) ... 
    // at this point, the API doesn't expose Connector objects, so to get the Flow from the connector, 
    // we have to get it via the Family parameter on the terminal.  
    // The Flow in our case could be named anything, i.e., it could be "SomeFlowParameter" ... 
    // it really just depends on how the Family is created, which in our case could be problematic 
    // if the user is using a custom Family that doesn't have the parameter named "Flow" ... 
    // but they could also create an air terminal that has >1 connector ... 
    // would they actually do that?  Hmm ... likely not, but you never know :).
    //
    // The way the "Flow" family parameter is mapped to the Flow built-in-parameter of the Connector 
    // is through Family Editor (FE).  In FE, select the connector object, click Element Properties, 
    // and note that the Flow parameter here has a button in the right hand column ... 
    // click this, and you will see that the "Flow" family parameter is selected.
    //
    public const string Flow = "Flow";
    //
    // air terminal family symbol parameters used in CmdChangeSize:
    //
    public const string MaxFlow = "Max Flow";
    public const string MinFlow = "Min Flow";
  }
}
