Autodesk Revit API application: ObjectViewer

This is a really minimal sample that shows how to display one selected element in preview window. All the interesting code is in ObjectViewer project, NewRevitviewer file collects the code implement drawing. AnalyticalView and ObjectViewer retrieved the geometry of the elemement.

To build the *.dll, edit project properties and make sure additional reference component
include the Revit.dll in Revit Structure 3 installation directory. Then, paste the contents of Revit.ini into Revit.ini. Launch Revit, note the Tools -> External Commands menu.

To view the element, select it and execute the External Command -> ObjectViewer command.

The user will switch between physical and analytical model if the element have an analytical model 