Autodesk Revit API application: Openings

This is a really minimal sample that shows how to get the property of an Opening, show profile of Opening
in preview window and how to create X model line on Opening.
All the interesting code is in Openings project, Openings file collect the code implement drawing. 
WireFrame retrieved the profile of the elemement.

To build the *.dll, edit project properties and make sure additional reference component
include the Revit.dll in Revit Structure 4 installation directory. 
Then, paste the contents of Revit.ini into Revit.ini. 
Launch Revit, note the Tools -> External Tools -> Openings Commands menu.