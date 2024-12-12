//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Revit.SDK.Samples.ValidateParameters.CS
{
    /// <summary>
    /// The form is used to show the result
    /// </summary>
    public partial class MessageForm : System.Windows.Forms.Form
    {  
        /// <summary>
        /// store the log file name
        /// </summary>
        string m_logFileName;

        /// <summary>
        /// construction of form
        /// </summary>
        public MessageForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// construction method with parameter
        /// </summary>
        /// <param name="messages">messages</param>
        public MessageForm(string[] messages)
            : this()
        {
            string msgText = "";
            //If the size of error messages is 0, means the validate parameters is successful
            this.Text = "Validate Parameters Message Form";
            //create regeneration log file
            string assemblyPath;            
            assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            m_logFileName = assemblyPath + "\\ValidateParametersLog.txt"; 
            StreamWriter writer = File.CreateText(m_logFileName);  
            writer.Close();
            if (messages.Length == 0)
            {
                msgText = "All types and parameters passed the validation for API";
                WriteLog(msgText);
                messageRichTextBox.Text = msgText;
            }
            else
            {
                foreach (string row in messages)
                {
                    if (row == null) continue;
                    else
                    {
                        WriteLog(row);
                        msgText += row + "\n";
                    }
                }
            }
            msgText += "\n\nIf you want to know the validating parameters result, please get the log file at \n"+m_logFileName;
            messageRichTextBox.Text = msgText;
            this.StartPosition = FormStartPosition.CenterParent;           
            CheckForIllegalCrossThreadCalls = false;
        }
       
        /// <summary>
        /// The method is used to write line to log file
        /// </summary>
        /// <param name="logStr">the log string</param>
        private void WriteLog(string logStr)
        {
            StreamWriter writer = null;
            writer = File.AppendText(m_logFileName);
            writer.WriteLine(logStr);
            writer.Close();
        }
    }
}
