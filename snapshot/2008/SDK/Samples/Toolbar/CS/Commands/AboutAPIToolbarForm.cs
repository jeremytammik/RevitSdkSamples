//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.Toolbar.CS
{
    public partial class AboutAPIToolbarForm : Form
    {
        public AboutAPIToolbarForm(AboutAPIToolbar dataBuffer)
        {
            InitializeComponent();             
            Initialize(dataBuffer);
        }

        private AboutAPIToolbar m_dataBuffer;
        private Font m_captionFont;

        private void Initialize(AboutAPIToolbar dataBuffer)
        {
            m_dataBuffer = dataBuffer;
            m_captionFont = new Font(aboutToolbarRichTextBox.Font.FontFamily,
                                 aboutToolbarRichTextBox.Font.Size,
                                 FontStyle.Bold);
            contactLinkLabel.Links.Clear();
            contactLinkLabel.Links.Add(0, contactLinkLabel.Text.Length, "mailto:cxlou@hslcn.com;junyang@hslcn.com"); 

            Dictionary<string, string>  aboutToolbar      =  m_dataBuffer.AboutToolbar;
            Dictionary<string,string> difToolbarItemUsage = m_dataBuffer.DifToolbarItemUsage;
            aboutToolbarRichTextBox.Text = "";
            foreach (string key in aboutToolbar.Keys)
            {
                AddCaption(key);
                AddBodyText(aboutToolbar[key]);
            }

            foreach (string key in difToolbarItemUsage.Keys)
            {
                AddCaption(key);
                AddBodyText(difToolbarItemUsage[key]);
            }
        }

        private void AddCaption(string value)
        {
            aboutToolbarRichTextBox.AppendText(value + "\r\n");

            // format the text
            int index  = aboutToolbarRichTextBox.Text.IndexOf(value);

            if (-1 != index)
            {
                aboutToolbarRichTextBox.Select(index, value.Length);
                aboutToolbarRichTextBox.SelectionFont = m_captionFont;
                aboutToolbarRichTextBox.SelectionBullet = true;
            }
        }

        private void AddBodyText(string value)
        {
            aboutToolbarRichTextBox.AppendText(value + "\r\n\r\n");
            
            // format the text
            int length = value.IndexOf("\r\n");
            if (-1 == length)
            {
                length = value.Length;
            }

            int index = aboutToolbarRichTextBox.Text.IndexOf(value.Substring(0,length));

            if (-1 != index)
            {
                aboutToolbarRichTextBox.Select(index, value.Length);
                aboutToolbarRichTextBox.SelectionIndent = 10;
            }
        }

        private void contactLinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string target = e.Link.LinkData as string;
            if (null != target)
            {
                System.Diagnostics.Process.Start(target);
            }
        }
    }
}