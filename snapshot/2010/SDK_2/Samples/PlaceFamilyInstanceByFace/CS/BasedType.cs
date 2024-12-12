//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.PlaceFamilyInstanceByFace.CS
{
    /// <summary>
    ///  This form class is for user choose a based-type of creating family instance
    /// </summary>
    public partial class BasedTypeForm : Form
    {
        #region Fields
        // based-type
        private BasedType m_baseType = BasedType.Point; 
        #endregion

        #region Property
        /// <summary>
        /// based-type
        /// </summary>
        public BasedType BaseType
        {
            get
            {
                return m_baseType;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public BasedTypeForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Process the click event of "Next" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (radioButtonPoint.Checked)
            {
                m_baseType = BasedType.Point;
            }
            else if (radioButtonLine.Checked)
            {
                m_baseType = BasedType.Line;
            }
            else
            {
                throw new Exception("An error occured in selecting based type.");
            }
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Process the click event of "Cancel" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }
        #endregion
    }

    /// <summary>
    /// Based-type
    /// </summary>
    public enum BasedType
    {
        /// <summary>
        /// Point-based
        /// </summary>
        Point = 0,

        /// <summary>
        /// Line-based
        /// </summary>
        Line,
    }
}