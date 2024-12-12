#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2008 by Autodesk, Inc.
'
' Permission to use, copy, modify, and distribute this software
' for any purpose and without fee is hereby granted, provided
' that the above copyright notice appears in all copies and
' that both that copyright notice and the limited warranty and
' restricted rights notice below appear in all supporting
' documentation.
'
' AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
' AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
' MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
' DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
' UNINTERRUPTED OR ERROR FREE.
'
' Use, duplication, or disclosure by the U.S. Government is subject to
' restrictions set forth in FAR 52.227-19 (Commercial Computer
' Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
' (Rights in Technical Data and Computer Software), as applicable.
#End Region
Namespace Labs
    Module LabConstants

        ' Strings than MUST be localized for non-English versions!
        ' (to be added later)
        Private Const _temp_dir As String = "C:/tmp/"

        Public Const DegreesToRadians As Double = System.Math.PI / 180

        ' Strings than can be adjusted in any case or localized for non-English version
        Public gsFilePathLab2_1 As String = _temp_dir + "RevitElements.txt"

        ' Lab 2_1
        Public Const MeterToFeet As Double = 3.2808399

        ' Lab 3_3
        Public Const gsLibPath As String = "C:\Documents and Settings\All Users\Application Data\Autodesk\RST 2009\Metric Library\"

        Public Const gsWholeFamilyFileToLoad1 As String = gsLibPath + "Structural\Framing\Steel\M_C-Channel.rfa" ' has TXT catalog file
        Public Const gsWholeFamilyFileToLoad2 As String = gsLibPath + "Structural\Framing\Steel\M_Plate.rfa" ' no TXT catalog file

        Public Const gsFamilyFileToLoadSingleSymbol As String = gsLibPath + "Structural\Framing\Steel\M_L-Angle.rfa"
        Public Const gsSymbolName As String = "L152x102x12.7"

        ' Lab 4_3
        Public Const gsSharedParamFilePath As String = _temp_dir + "SharedParams.txt"
        Public Const gsSharedParamsGroupAPI As String = "API Parameters"
        Public Const gsSharedParamsDefFireRating As String = "API FireRating"

        ' Lab 4_4
        Public Const sParamGroupName As String = "Per-doc Params"
        Public Const sParamNameVisible As String = "Visible per-doc Integer"
        Public Const sParamNameInvisible As String = "Invisible per-doc Integer"


        ' Lab 5_1
        Public Const gsGroupTypeModel As String = "Model Group" 'BEWARE: In the browser, it says only "Model"

    End Module
End Namespace
