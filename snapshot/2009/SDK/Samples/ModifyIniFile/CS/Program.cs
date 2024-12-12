//
// (C) Copyright 2003-2008 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Revit.SDK.Samples.ModifyIniFile.CS 
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!AppendCommand())
            {
                Console.WriteLine("Add External Command failed.");
            }
        }

        private static bool AppendCommand()
        {
            // location of the Revit.ini file
            string iniFilename = 
                "C:\\Program Files\\Revit Architecture 2009\\Program\\Revit.ini";

            // verify the file Revit.ini exists
            if (!File.Exists(iniFilename))
            {
                Console.WriteLine("File \"{0}\" not found.", iniFilename);
                return false;
            }
            // ExternalCommands section
            string commandSection = "ExternalCommands";

            // constants for you application
            string myCommandName = "My Command";
            string myClassName = "Test.Command";
            string myCommandDescription = "My Command Description";

            // create an instance of IniFile to read or write the ini file
            IniFile revitIni = new IniFile(iniFilename);

            // get the current command count if it exists. If it doesn't exist then 0 will be returned
            int ecCount = revitIni.IniReadInt(commandSection, "ECCount", 0);

            // increment the command count and use that number as the basis for our command entries
            ecCount++;

            // write the increased ECCount
            if (!revitIni.IniWriteString(commandSection, "ECCount", ecCount.ToString()))
                return false;

            // write our command name that will appear in the menu to the ini file
            if (!revitIni.IniWriteString(commandSection, "ECName" + ecCount.ToString(), myCommandName))
                return false;

            // write our class name to the ini file
            if (!revitIni.IniWriteString(commandSection, "ECClassName" + ecCount.ToString(), myClassName))
                return false;

            // write our command description that will appear in the status bar to the ini file
            if (!revitIni.IniWriteString(commandSection, "ECDescription" + ecCount.ToString(), myCommandDescription))
                return false;

            return true;
        }
    }
}
