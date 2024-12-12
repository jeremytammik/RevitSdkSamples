#region Header
// Revit MEP API sample application
//
// Copyright (C) 2007-2010 by Jeremy Tammik, Autodesk, Inc.
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
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#endregion // Namespaces

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "Revit MEP Sample Application" )]
[assembly: AssemblyDescription
  ( "Demonstrate use of the Revit API for MEP: "
  + "1. use of the generic API for HVAC specific tasks: "
  + "determine air terminals for each space; "
  + "assign flow to the air terminals depending on the space's calculated supply air flow; "
  + "change size of diffuser based on flow; "
  + "populate the value of the 'CFM per SF' variable on all spaces; "
  + "determine unhosted elements; "
  + "reset demo. "
  + "2. use of the MEP specific API to traverse an electrical system "
  + "and display its hierarchy in a tree view.")]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "DevTech, Autodesk, Inc." )]
[assembly: AssemblyProduct( "Revit MEP Sample Application" )]
[assembly: AssemblyCopyright( "Copyright © 2007-2010 by Jeremy Tammik, Autodesk, Inc." )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "d46fe037-86c8-4693-bd27-5bcbb9e930b2" )]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion( "2011.0.0.2" )]
[assembly: AssemblyFileVersion( "2011.0.0.2" )]
