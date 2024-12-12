//
// RevitCodingTips_VC.h
//
// Copyright (c) 2007-2009 by Autodesk, Inc.
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
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
// Created by: Bill Zhang and Jeremy Tammik, DevTech, Autodesk, Oct. 2007
//
#pragma once

// avoid includes and using statements in header files! ... except includes in stdafx.h, of course

namespace RevitCodingTips_cpp
{
  //
  // Tip 10 - List out wall family, type and parameter information with VB.NET
  //
  // We do the same thing in C#, VB.NET and VC.NET
  //
  public ref class CommandListWallInfo : public Autodesk::Revit::IExternalCommand
  {
  public:
    virtual Autodesk::Revit::IExternalCommand::Result Execute( Autodesk::Revit::ExternalCommandData ^ cmdData, System::String ^% , Autodesk::Revit::ElementSet ^ );

  private:
    System::String ^ ListWallFamilyAndTypeInfo( Autodesk::Revit::Element ^, System::String ^% );
    System::String ^ ListParameters( Autodesk::Revit::Element ^ );
  };
}
