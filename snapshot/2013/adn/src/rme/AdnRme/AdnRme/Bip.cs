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

#region Namespaces
using Autodesk.Revit.DB;
#endregion // Namespaces

namespace AdnRme
{
  /// <summary>
  /// Provide abbreviated access to the required BuiltInParameter enumeration values.
  /// </summary>
  class Bip
  {
    //
    // create constants for the built-in parameters used, 
    // since their full names are so monstrously long:
    //
    public const BuiltInParameter Airflow = BuiltInParameter.ROOM_ACTUAL_SUPPLY_AIRFLOW_PARAM;
    public const BuiltInParameter Area = BuiltInParameter.ROOM_AREA;
    public const BuiltInParameter CalculatedSupplyAirFlow = BuiltInParameter.ROOM_CALCULATED_SUPPLY_AIRFLOW_PARAM;
    public const BuiltInParameter Flow = BuiltInParameter.RBS_DUCT_FLOW_PARAM;
    public const BuiltInParameter Host = BuiltInParameter.INSTANCE_FREE_HOST_PARAM;

    // Hi Jeremy, hur står det till med dig nu för tiden?
    // You should be able to just use RBS_SYSTEM_CLASSIFICATION_PARAM instead of RBS_SYSTEM_TYPE_PARAM if you like. 
    // The former System Type property on duct and piping systems (RBS_SYSTEM_TYPE_PARAM) has been renamed to System Classification (RBS_SYSTEM_CLASSIFICATION_PARAM). We are renaming it for clarify and to avoid confusion with the new duct and piping system type elements. This naming also goes in line with other similar parameters like Load Classification.
    // The former System Type property on duct and pipe connectors to has been renamed to System Classification. The associated parameters RBS_DUCT_SYSTEM_TYPE and RBS_PIPE_SYSTEM_TYPE has been renamed to RBS_DUCT_CONNECTOR_SYSTEM_CLASSIFICATION_PARAM and RBS_PIPE_CONNECTOR_SYSTEM_CLASSIFICATION_PARAM.
    // Another more “correct” way to do this would be to get the Autodesk::Revit::DB::ElementType::MEPSystemType using the *new* RBS_DUCT_SYSTEM_TYPE_PARAM param from the family instance as above. You should then be able to check the system classification enum (Autodesk::Revit::DB::MEPSystemClassification) in the duct system type element to identify what kind of air system it is (Supply, Return, Exhaust etc). This would also work for piping system components using the RBS_PIPING_SYSTEM_TYPE_PARAM param.
    // Thomas Olsson

    //public const BuiltInParameter SystemType = BuiltInParameter.RBS_SYSTEM_TYPE_PARAM; // 2011
    public const BuiltInParameter SystemType = BuiltInParameter.RBS_SYSTEM_CLASSIFICATION_PARAM; // 2012
  }
}
