#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2009 by Autodesk, Inc.
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
#endregion // Header

namespace Labs
{
  /// <summary>
  /// Define global numerical and string constants.
  /// </summary>
  static class LabConstants
  {
    #region 2.0 Revit unit conversion constants:
    #endregion

    #region 2.1 Revit element listing output file constants:
    #endregion

    // Lab 3_2 and 3_3
    //public const string _libPath = @"C:\Documents and Settings\All Users\Application Data\Autodesk\RST 2009\Metric Library\";
    private const string _libPath = @"C:\Documents and Settings\All Users\Application Data\Autodesk\RAC 2010\Metric Library\Structural\Framing\Steel\";

    public const string WholeFamilyFileToLoad1 = _libPath + "M_C-Channel.rfa"; // has TXT catalog file
    public const string WholeFamilyFileToLoad2 = _libPath + "M_Plate.rfa"; // no TXT catalog file

    public const string FamilyFileToLoadSingleSymbol = _libPath + "M_L-Angle.rfa";
    public const string SymbolName = "L152x102x12.7";

    // Lab 4_3
    public const string SharedParamFilePath = _temp_dir + "SharedParams.txt";
    public const string SharedParamsGroupAPI = "API Parameters";
    public const string SharedParamsDefFireRating = "API FireRating";

    // Lab 4_4
    public const string ParamGroupName = "Per-doc Params";
    public const string ParamNameVisible = "Visible per-doc Integer";
    public const string ParamNameInvisible = "Invisible per-doc Integer";
  }
}
