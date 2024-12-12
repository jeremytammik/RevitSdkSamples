#Region "Header"
' Revit API .NET Labs
'
' Copyright (C) 2006-2010 by Autodesk, Inc.
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
    ''' <summary>
    ''' Define global numerical and string constants.
    ''' </summary>
    Module LabConstants

        '#region 2.0 Revit unit conversion constants:
        '#endregion

        '#region 2.1 Revit element listing output file constants:
        '#endregion

        ' Lab 3_2 and 3_3
        Const _libPath As String = "C:\Documents and Settings\All Users\Application Data\Autodesk\RST 2011\Metric Library\Structural\Framing\Steel\"

        Public Const WholeFamilyFileToLoad1 As String = _libPath + "M_C-Channel.rfa" ' has TXT catalog file
        Public Const WholeFamilyFileToLoad2 As String = _libPath + "M_Plate.rfa" ' no TXT catalog file

        Public Const FamilyFileToLoadSingleSymbol As String = _libPath + "M_L-Angle.rfa"
        Public Const SymbolName As String = "L152x102x12.7"

        ' Lab 4_3
        Public Const SharedParamFilePath As String = _temp_dir + "SharedParams.txt"
        Public Const SharedParamsGroupAPI As String = "API Parameters"
        Public Const SharedParamsDefFireRating As String = "API FireRating"

        ' Lab 4_4
        Public Const ParamGroupName As String = "Per-doc Params"
        Public Const ParamNameVisible As String = "Visible per-doc Integer"
        Public Const ParamNameInvisible As String = "Invisible per-doc Integer"

        ' Lab 5_1
        Public Const GroupTypeModel As String = "Model Group" 'BEWARE: In the browser, it says only "Model"

    End Module
End Namespace
