#
# db.py - database of all Revit SDK samples
#
# I aborted work on this and concentrated on the 
# Revit_SDK_Samples.xlsx spreadsheet instead ...
#
# Copyright (C) 2007 Jeremy Tammik, Autodesk Inc.
# 2007-08-23
#
# This file contains a database of all Revit SDK samples classifying them by
# programming topic, level of complexity advanced or basic, base platform
# Architectural, Structural or MEP, history, programming language C# or VB.NET.
#
# root: the Revit SDK samples root directory, default is 'C:/a/lib/revit/2008/sdk/Samples'
#
# Each sample is assigned a unique key <key>, which is mapped to a list of values:
#
# name: defaults to <key>
# description: defaults to <key>
# level: basic or advanced, defaults to basic
# type: external app or cmd, defaults to cmd
# language: programming language, which is either 'CS' or 'VB' and defaults to 'CS'
# assembly: defaults to <root>/<key>/<lang>/bin/Debug/<key>.dll, but also searches for ??? in case not found ... if key contains a '.', also searches using only the prefix, e.g. RevitCommands.dll for RevitCommands.Selection
# class name: defaults to Revit.SDK.Samples.<key>.<lang>.Command - todo: implement a .net exe which checks that the class actually exists in the assembly
# version: one of 8.0, 8.1, 9.0, 0.1, 2008.0, 2008.2, defaults to 8.0
# flavour: one of RAC, RST, RME, defaults to RAC
#
Key	ECName	ECDescription	ECClassName	ECAssembly	Flavour	Version	Level	Type	Lang	Notes

KEY = 0
NAME = 1
DESC = 2
CLS = 3
ASM = 4
FLAV = 5
VER = 6
LVL = 7
TYP = 8
LANG = 9
NOTE = 10

levels = ( 'basic', 'advanced' )
types = ( 'cmd', 'app' )
languages = ( 'CS', 'VB' )
versions = ( '8.0', '8.1', '9.0', '9.1', '2008.0', '2008.2' )
flavour = ( 'RAC', 'RST', 'RME' )

default_data = ( '', '', 'basic', 'cmd', 'CS', '', '', '8.0', 'RAC' )

db = {
  'APIAppStartup' : ( '', '', '', 'app', '', '', '', '', '' ),
  'AllViews' : ( '', '', '', '', '', '', '', '', '' ),
  'AnalyticalSupportData_Info' : ( '', '', '', '', '', '', '', '', '' ),
  'AnalyticalViewer' : ( '', '', '', '', '', '', '', '', '' ),
  'ApplicationEvents' : ( '', '', '', 'app', '', '', '', '', '' ),
  'ArchSample' : ( '', '', '', '', 'VB', '', '', '', '' ),
  'AreaReinCurve' : ( '', '', '', '', '', '', '', '', '' ),
  'AreaReinParameters' : ( '', '', '', '', '', '', '', '', '' ),
  'AutoTagRooms' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'BarDescriptions' : ( '', '', '', '', '', '', '', '', '' ),
  'BeamAndSlabNewParameter' : ( '', '', '', '', '', '', '', '', '' ),
  'BlendVertexConnectTable' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'BoundaryConditions' : ( '', '', '', '', '', '', '', '', '' ),
  'BrowseBindings' : ( '', '', '', '', 'VB', '', '', '', '' ),
  'CreateBeamSystem' : ( '', '', '', '', '', '', '', '', '' ),
  'CreateBeamsColumnsBraces' : ( '', '', '', '', 'VB', '', '', '', '' ),
  'CreateComplexAreaRein' : ( '', '', '', '', '', '', '', '', '' ),
  'CreateDimensions' : ( '', '', '', '', '', '', '', '', '' ),
  'CreateShared' : ( '', '', '', '', 'VB', '', '', '', '' ),
  'CreateSimpleAreaRein' : ( '', '', '', '', '', '', '', '', '' ),
  'CreateViewSection' : ( '', '', '', '', '', '', '', '', '' ),
  'CreateWallinBeamProfile' : ( '', '', '', '', '', '', '', '', '' ),
  'CreateWallsUnderBeams' : ( '', '', '', '', '', '', '', '', '' ),
  'CurvedBeam' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'DeckProperties' : ( '', '', '', '', '', '', '', '', '' ),
  'DeleteDimensions' : ( '', '', '', '', '', '', '', '', '' ),
  'DeleteObject' : ( '', '', '', '', 'VB', '', '', '', '' ),
  'DesignOptionReader' : ( '', '', '', '', 'VB', '', '', '', '' ),
  'ElementViewer' : ( '', '', '', '', '', '', '', '', '' ),
  'FamilyExplorer' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'FireRating.Apply' : ( '', '', '', '', 'VB', '', 'ApplyParameter', '', '' ),
  'FireRating.Export' : ( '', '', '', '', 'VB', '', '', '', '' ),
  'FireRating.Import' : ( '', '', '', '', 'VB', '', '', '', '' ),
  'FoundationSlab' : ( '', '', '', '', '', '', '', '', '' ),
  'FrameBuilder' : ( '', '', '', '', '', '', '', '', '' ),
  'GenerateFloor' : ( '', '', '', '', '', '', '', '', '' ),
  'HelloRevit' : ( '', '', '', '', '', '', '', '', '' ),
  'HelloWorld' : ( '', '', '', '', '', '', '', '', '' ),
  'ImportExportDWG' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'InPlaceMembers' : ( '', '', '', '', '', '', '', '', '' ),
  'InplaceFamilyAnalyticalModel3D' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'InvisibleParam' : ( '', '', '', '', '', '', '', '', '' ),
  'Journaling' : ( '', '', '', '', '', '', '', '', '' ),
  'LevelsProperty' : ( '', '', '', '', '', '', '', '', '' ),
  'Loads' : ( '', '', '', '', '', '', '', '', '' ),
  'MaterialProperties' : ( '', '', '', '', '', '', '', '', '' ),
  'Materials' : ( '', '', '', '', '', '', '', '', '' ),
  'ModelLines' : ( '', '', '', '', '', '', '', '', '' ),
  'ModifyIniFile' : ( '', '', '', '', '', '', '', '', '' ),
  'MoveLinear' : ( '', '', '', '', '', '', '', '', '' ),
  'NewOpenings' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'NewPathReinforcement' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'ObjectViewer' : ( '', '', '', '', '', '', '', '', '' ),
  'Openings' : ( '', '', '', '', '', '', '', '', '' ),
  'ParameterUtils' : ( '', '', '', '', '', '', '', '', '' ),
  'PathReinforcement' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'PhaseSample' : ( '', '', '', '', '', '', '', '', '' ),
  'PhysicalProp' : ( '', '', '', '', '', '', '', '', '' ),
  'ProjectInfo' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'ProjectUnit' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'RDBLink' : ( '', '', '', '', '', '', '', '2008.2', '' ),
  'ReferencePlane' : ( '', '', '', '', '', '', '', '', '' ),
  'Reinforcement' : ( '', '', '', '', '', '', '', '', '' ),
  'RevitCommands.Selection' : ( '', 'Show how to use selection set', '', '', '', '', 'Revit.SDK.Samples.RevitCommands.VB.NET.RvtCmd_Selection', '', '' ),
  'RevitCommands.ShowElementData' : ( '', 'Show parameters in selected entities', '', '', '', '', 'Revit.SDK.Samples.RevitCommands.VB.NET.RvtCmd_ShowElementData', '', '' ),
  'RevitCommands.LibraryPaths' : ( '', 'List the Revit library paths', '', '', '', '', 'Revit.SDK.Samples.RevitCommands.VB.NET.RvtCmd_LibraryPaths', '', '' ),
  'RevitCommands.LoadFamilySymbol' : ( '', 'Load a family symbol/type', '', '', '', '', 'Revit.SDK.Samples.RevitCommands.VB.NET.RvtCmd_LoadFamilySymbol', '', '' ),
  'RevitCommands.LoadFamily' : ( '', 'Load a family', '', '', '', '', 'Revit.SDK.Samples.RevitCommands.VB.NET.RvtCmd_LoadFamily', '', '' ),
  'RoomSchedule' : ( '', '', '', '', '', '', '', '2008.2', '' ),
  'Rooms' : ( '', '', '', '', '', '', '', '', '' ),
  'RoomViewer' : ( '', '', '', '', '', '', '', '', '' ),
  'RotateFramingObjects' : ( '', '', '', '', '', '', '', '', '' ),
  'ShaftHolePuncher' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'SharedCoordinateSystem' : ( '', '', '', '', '', '', '', '', '' ),
  'SlabProperties' : ( '', '', '', '', '', '', '', '', '' ),
  'SpanDirection' : ( '', '', '', '', '', '', '', '', '' ),
  'SpotDimension' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'StructSample' : ( '', '', '', '', '', '', '', '', '' ),
  'StructuralLayerFunction' : ( '', '', '', '', '', '', '', '', '' ),
  'TagBeam' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'TestFloorThickness' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'TestWallThickness' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'Toolbar' : ( '', '', '', 'app', '', '', '', '', '' ),
  'TransactionControl' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'TypeSelector' : ( '', '', '', '', '', '', '', '', '' ),
  'VersionChecking' : ( '', '', '', '', '', '', '', '', '' ),
  'ViewPrinter' : ( '', '', '', '', '', '', '', '2008.0', '' ),
  'VisibilityControl' : ( '', '', '', '', '', '', '', '2008.0', '' )
}
