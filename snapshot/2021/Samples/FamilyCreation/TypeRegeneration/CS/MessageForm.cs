//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;

namespace Revit.SDK.Samples.TypeRegeneration.CS
{
    /// <summary>
    /// The form is used to show the result
    /// </summary>
    public partial class MessageForm : System.Windows.Forms.Form
    {        
        /// <summary>
        /// new a Timer,set the interval 2 seconds
        /// </summary>
        System.Timers.Timer timer = new System.Timers.Timer(2000);
        
        /// <summary>
        /// construction of MessageForm
        /// </summary>
        public MessageForm()
        {
            InitializeComponent();
            this.Text = "Type Regeneration Message Form";
            //set the timer elapsed event
            timer.Elapsed += new System.Timers.ElapsedEventHandler(onTimeOut);//Set the executed event when time is out          
            timer.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// add text to the richtextbox and set time enable is true, then timer starts timing
        /// </summary>
        /// <param name="message">message from the regeneration</param>
        /// <param name="enableTimer">enable or disable the timer elapsed event</param>
        public void AddMessage(string message,bool enableTimer)
        {
            messageRichTextBox.AppendText(message);
            timer.Enabled = enableTimer;
        }

        /// <summary>
        /// the method is executed when time is out, and set the timer enabled false,then timer stop timing
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e">time elapsed event args</param>
        private void onTimeOut(object source, System.Timers.ElapsedEventArgs e)
        {            
            timer.Enabled = false;
            this.Close();
        }        
    }
}
