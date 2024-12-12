#pragma region Header
//
// RevitCodingTips_VC.cpp
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
#pragma endregion

#pragma region Includes and Namespaces
#include "stdafx.h"
#include "RevitCodingTips.h"

using namespace System;
using namespace System::Windows::Forms;

// Tip 1 - complete namespace prefix to avoid ambiguity:
//namespace Rvt = Autodesk::Revit;
//namespace RvtGeom = Autodesk::Revit::Geometry;
//namespace RvtSymbs = Autodesk::Revit::Symbols;
//namespace RvtElems = Autodesk::Revit::Elements;

// Tip 1 - use namespaces globally and disambiguate individual symbols when needed:
using namespace Autodesk::Revit;
using namespace Autodesk::Revit::Elements;
using namespace Autodesk::Revit::Symbols;
//using CmdResult = Autodesk::Revit::IExternalCommand::Result;
#pragma endregion

namespace RevitCodingTips_cpp
{
  //
  // Tip 10 - List out wall family, type and parameter information with VB.NET
  //
  // Compare equivalent code in C#, VB.NET and VC.NET.
  //
  IExternalCommand::Result CommandListWallInfo::Execute(
    ExternalCommandData ^ cmdData,
    String ^% message,
    ElementSet ^ elements )
  {
    try
    {
      Document ^ doc = cmdData->Application->ActiveDocument;
      ElementSet ^ selSet = doc->Selection->Elements;

      if( 1 != selSet->Size ) {
        message = "Please select a wall element";
        return IExternalCommand::Result::Cancelled;
      }

      for each( Element ^ elem in selSet ) {
        String ^ catName("");
        MessageBox::Show( ListWallFamilyAndTypeInfo( elem, catName ), catName );

        MessageBox::Show( ListParameters( elem ), "Wall Parameters" );
        Wall ^ wall = dynamic_cast<Wall ^>( elem );
        if( wall ) {
          Symbols::WallType ^ wallType = wall->WallType;
          MessageBox::Show( ListParameters( wallType ), "Wall Type  Parameters" );
        }
        else {
          message = "Please select a wall element";
          return IExternalCommand::Result::Cancelled;
        }
      }
      return IExternalCommand::Result::Succeeded;
    }
    catch( Exception ^ ex ) {
      message = ex->Message;
      return IExternalCommand::Result::Failed;
    }
  }

  String ^ CommandListWallInfo::ListParameters( Element ^ elem )
  {
    ParameterSet ^ pars(elem->Parameters);
    if (pars->IsEmpty) {
      return "Parameters are empty!";
    }

    String ^ str = "";
    for each ( Parameter ^ param in pars) {
      String ^ name = param->Definition->Name;
      Parameters::StorageType ^ type = param->StorageType;
      String ^ val = "";
      ElementId eId;
      Element ^ paraElem;
      switch (*type) {
        case Parameters::StorageType::Double:
          val = (*param).AsDouble().ToString();
          break;
        case Parameters::StorageType::ElementId:
          eId = param->AsElementId();
          paraElem = elem->Document->Element::get(eId);
          if (paraElem) {
            val = paraElem->Name;
          }
          break;
        case Parameters::StorageType::Integer:
          val = (*param).AsInteger().ToString();
          break;
        case Parameters::StorageType::String:
          val = (*param).AsString();
          break;
        default:
          break;
      }
      str = str + name + ": " + val + "\n";
    }

    return str;
  }

  String ^ CommandListWallInfo::ListWallFamilyAndTypeInfo( Element ^ elem, String ^% cat )
  {
    String ^ infoStr = "";
    String ^ title = "";

    title = "Element category";
    if (elem->Category)
    {
      title = title + " : " + elem->Category->Name;
    }
    Wall ^ wall = dynamic_cast<Wall ^>(elem);
    if (wall)
    {
      WallType ^ wallType = wall->WallType;
      switch (wallType->Kind)
      {
        case WallType::WallKind::Basic:
          infoStr = infoStr + "Basic : ";
          break;
        case WallType::WallKind::Curtain:
          infoStr = infoStr + "Curtain : ";
          break;
        case WallType::WallKind::Stacked:
          infoStr = infoStr + "Stacked : ";
          break;
        case WallType::WallKind::Unknown:
          infoStr = infoStr + "Unknown : ";
          break;
      }
      infoStr = infoStr + wallType->Name;
    }
    cat = title;
    return infoStr;
  }
}
