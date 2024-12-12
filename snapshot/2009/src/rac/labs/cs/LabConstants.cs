#region Header
// Revit API .NET Labs
//
// Copyright (C) 2007-2008 by Autodesk, Inc.
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
  static class LabConstants
  {
    private const string _temp_dir = "C:/tmp/";
    // Strings than MUST be localized for non-English versions!
    // (to be added later)

    public const double DegreesToRadians = System.Math.PI / 180;

    // Strings than can be adjusted in any case or localized for non-English version
    public const string gsFilePathLab2_1 = _temp_dir + "RevitElements.txt";

    // Lab 2_1
    public const double MeterToFeet = 3.2808399;

    // Lab 3_3
    public const string gsLibPath = @"C:\Documents and Settings\All Users\Application Data\Autodesk\RST 2009\Metric Library\";

    public const string gsWholeFamilyFileToLoad1 = gsLibPath + @"Structural\Framing\Steel\M_C-Channel.rfa"; // has TXT catalog file
    public const string gsWholeFamilyFileToLoad2 = gsLibPath + @"Structural\Framing\Steel\M_Plate.rfa"; // no TXT catalog file

    public const string gsFamilyFileToLoadSingleSymbol = gsLibPath + @"Structural\Framing\Steel\M_L-Angle.rfa";
    public const string gsSymbolName = "L152x102x12.7";

    // Lab 4_3
    public const string gsSharedParamFilePath = _temp_dir + "SharedParams.txt";
    public const string gsSharedParamsGroupAPI = "API Parameters";
    public const string gsSharedParamsDefFireRating = "API FireRating";

    // Lab 4_4
    public const string sParamGroupName = "Per-doc Params";
    public const string sParamNameVisible = "Visible per-doc Integer";
    public const string sParamNameInvisible = "Invisible per-doc Integer";

    // Lab 5_1
    public const string gsGroupTypeModel = "Model Group"; // BEWARE: In the browser, it says only "Model"
  }
}
