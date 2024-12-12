Autodesk Revit API application: RotateFramingObjects

This is a really minimal sample that shows how to rotate objects selected from a
Revit model via the API. All the interesting code is in RotateFramingObjects project,

To build the *.dll, edit project properties and make sure additional reference component
include the Revit.dll in Revit installation directory. Then, paste the contents of Revit.ini into Revit.ini. Launch Revit, note the Tools -> External Commands menu.

To rotate the objects, such as beams, braces and columns, select them and execute the External -> RotateFramingObjects command.

The user will be prompted for the amount, in degrees that the objects should be rotated. The dialog also contain option boxes for the user to specify this value is absolute or relative. the rotate is anticlockwise.