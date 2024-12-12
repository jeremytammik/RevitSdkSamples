//
// (C) Copyright 2003-2010 by Autodesk, Inc.
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
using System.Runtime.InteropServices;

namespace Revit.SDK.Samples.ModifyIniFile.CS 
{
    /// <summary>
    /// Contains methods to read and write value to specialized key in an ini file
    /// </summary>
    public class IniFile
    {
        //path of ini file
        private string m_path; 

        /// <summary>
        /// Gets or sets the path of ini file
        /// </summary>
        public string Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        /// <summary>
        /// Copies a string into the specified section of an initialization file.
        /// </summary>
        /// <param name="section">The name of the section to which the string will be copied. 
        /// If the section does not exist, it is created. The name of the section is case-independent; 
        /// the string can be any combination of uppercase and lowercase letters. </param>
        /// <param name="key">The name of the key to be associated with a string. 
        /// If the key does not exist in the specified section, it is created. If this parameter is NULL, 
        /// the entire section, including all entries within the section, is deleted.</param>
        /// <param name="val">A string to be written to the file. If this parameter is NULL, 
        /// the key pointed to by the key parameter is deleted.</param>
        /// <param name="filePath">The name of the initialization file.</param>
        /// <returns>If the function successfully copies the string to the initialization file, 
        /// the return value is nonzero. If the function fails, or if it flushes the cached version of 
        /// the most recently accessed initialization file, the return value is zero.</returns>
        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string section, string key, string val, string filePath);

        /// <summary>
        /// Retrieves an integer associated with a key in the specified section of an initialization file.
        /// </summary>
        /// <param name="section">The name of the section in the initialization file. </param>
        /// <param name="key">The name of the key whose value is to be retrieved. This value is 
        /// in the form of a string; the GetPrivateProfileInt function converts the string into an integer 
        /// and returns the integer.</param>
        /// <param name="def">The default value to return if the key name cannot be found in the initialization file.</param>
        /// <param name="filePath">The name of the initialization file. If this parameter does not contain 
        /// a full path to the file, the system searches for the file in the Windows directory. </param>
        /// <returns> The integer equivalent of the string following the specified key name in the 
        /// specified initialization file. If the key is not found, the return value is the specified 
        /// default value. </returns>
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);

        /// <summary>
        /// Initializes a new instance of IniFile to read and write
        /// </summary>
        /// <param name="inipath">Path of ini file to read or write</param>
        public IniFile(string inipath)
        {
            m_path = inipath;
        }

        /// <summary>
        /// Copies a string into the specified section of an initialization file.
        /// </summary>
        /// <param name="section">The name of the section to which the string will be copied.</param>
        /// <param name="key">The name of the key to be associated with a string.</param>
        /// <param name="value">A string to be written to the file.</param>
        /// <returns>If the function successfully copies the string return true otherwise false</returns>
        public bool IniWriteString(string section, string key, string value)
        {
            return WritePrivateProfileString(section, key, value, this.m_path) == 0 ? false : true;
        }

        /// <summary>
        /// Retrieves an integer associated with a key in the specified section.
        /// </summary>
        /// <param name="section">The name of the section to which the string will be copied.</param>
        /// <param name="key">The name of the key to be associated with a string.</param>
        /// <param name="def">The default value to return if the key name cannot be found.</param>
        /// <returns>The integer equivalent of the string following the specified key name, 
        /// If the key is not found, the return value is the specified default value.</returns>
        public int IniReadInt(string section, string key, int def)
        {
            return GetPrivateProfileInt(section, key, def, this.m_path);
        }
    } 
}
