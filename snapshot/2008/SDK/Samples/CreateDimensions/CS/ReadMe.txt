Autodesk Revit API application: CreateDimensions

This is a really minimal sample that add a command that takes a selection and adds a dimension from the start of the Basic wall
to the end of the wall into the project. Name the dimensions ¡°Dimension X¡± where X is an increasing number for each dimension. 
Note: this functionality demonstrates the use of graphical references.

To build the *.dll, edit project properties and make sure additional reference component include the RevitAPI.dll in Revit installation directory.
Then, paste the contents of Revit.ini into Revit.ini. Launch Revit, note the Tools -> External Commands menu.
