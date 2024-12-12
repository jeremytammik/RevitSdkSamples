ReadMe for Revit API Introduction Labs

Last updated March 3. 2011 by Jeremy Tammik, Autodesk Developer Technical Services, Autodesk Inc.

This sample solution includes the labs for the Revit API Introduction.

There are two projects, one in C# and one in VB.NET. Their functionality is identical. Some enhancements have been made to the C# version only.

To install and run the labs, you can proceed as follows:

1. Unpack the zip file to a directory of your choice.

2. Open the solution file labs.sln in Visual Studio.

3. Ensure that the references to the RevitAPI.dll and RevitAPIUI.dll assemblies are valid, and update them otherwise.

4. Build the solution.

5. Make a note of the full path of the assembly you wish to use, either the C# or the VB one.

6. Edit add-in manifest file Labs.addin by globally specifying the full assembly path within the Assembly tags.

7. Copy the manifest file to the appropriate directory for Revit to pick it up and load the plug-in assembly.

8. Set up the solution to start Revit.exe as the debugging program, and start debugging.
