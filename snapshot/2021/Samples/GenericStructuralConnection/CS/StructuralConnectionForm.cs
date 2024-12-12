//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.GenericStructuralConnection.CS
{
    /// <summary>
    /// 
    /// </summary>
    public partial class StructuralConnectionForm : Form
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public StructuralConnectionForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Property set with the user's choice.
        /// </summary>
        public CommandOption UserOption {get; set;}

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.rbCreateGeneric.Checked)
                UserOption = CommandOption.CreateGeneric;
            else if (this.rbDeleteGeneric.Checked)
                UserOption = CommandOption.DeleteGeneric;
            else if (this.rbReadGeneric.Checked)
                UserOption = CommandOption.ReadGeneric;
            else if (this.rbDeleteGeneric.Checked)
                UserOption = CommandOption.DeleteGeneric;
            else if (this.rbUpdateGeneric.Checked)
                UserOption = CommandOption.UpdateGeneric;
            else if (this.rbCreateDetailed.Checked)
                UserOption = CommandOption.CreateDetailed;
            else if (this.rbChangedDetail.Checked)
                UserOption = CommandOption.ChangeDetailed;
            else if (this.rbCopyDetailed.Checked)
                UserOption = CommandOption.CopyDetailed;
            else if (this.rbMatchPropDetailed.Checked)
                UserOption = CommandOption.MatchPropDetailed;
            else if (this.rbResetDetailed.Checked)
                UserOption = CommandOption.ResetDetailed;
        }
    }
}
