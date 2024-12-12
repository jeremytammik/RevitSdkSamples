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
  class ParameterValue
  {
    //
    // In Revit 2008, there are built-in parameters to access the actual, 
    // calculated and specified supply airflows, but not for the "Supply Air":
    // 
    // Actual Supply Airflow : ROOM_ACTUAL_SUPPLY_AIRFLOW_PARAM = Double: 2.08333333333333
    // Calculated Supply Airflow : ROOM_CALCULATED_SUPPLY_AIRFLOW_PARAM = Double: 2.29236666666667
    // Specified Supply Airflow : ROOM_DESIGN_SUPPLY_AIRFLOW_PARAM = Double: 2.29236666666667
    //
    public const string SupplyAir = "Supply Air";
    //
    // Revit 2009 provides Autodesk.Revit.MEP.Enums.DuctSystemType.SupplyAir.
    // But that is not a parameter value, and I do not see where it can be queried.
    //
  }
}
