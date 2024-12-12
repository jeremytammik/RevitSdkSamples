ReadMe for Revit API Introduction Labs

Last updated April 23, 2008, by Jeremy Tammik, Autodesk Developer Technical Services, Autodesk Inc.

This sample solution includes the labs for the Revit API Introduction.

There are two projects, one in C# and one in VB.NET. Their functionality is identical. Some enhancements have been made to the C# version only.

To install and run the labs, you can proceed as follows:

1. unpack the zip file to a directory of your choice.

2. open the solution file labs.sln in Visual Studio.

3. ensure that the RevitAPI.dll reference is valid, or update it if not.

4. build the solution.

5. make a note of the full path of the assembly you wish to use, either the C# or the VB one.

6. edit Add_to_Revit_ini.txt by globally replacing "=Labs.dll" by the full assembly path.

7. add the contents of Add_to_Revit_ini.txt to Revit.ini.

8. set up the solution to start Revit.exe as the debugging program.
