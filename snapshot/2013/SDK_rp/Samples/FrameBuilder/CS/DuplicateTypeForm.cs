//
// (C) Copyright 2003-2012 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.FrameBuilder.CS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;

    using Autodesk.Revit.DB;

    /// <summary>
    /// form to duplicate FamilySymbol and edit its name and parameters
    /// </summary>
    public partial class DuplicateTypeForm : System.Windows.Forms.Form
    {
        private FamilySymbol m_copiedSymbol;    // FamilySymbol object to be copied
        private FamilySymbol m_newSymbol;        // duplicate FamilySymbol
        private FrameTypesMgr m_typesMgr;        // object manage FamilySymbols

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="obj">FamilySymbol object</param>
        /// <param name="typesMgr">FamilySymbols' manager</param>
        public DuplicateTypeForm(object obj, FrameTypesMgr typesMgr)
        {
            InitializeComponent();

            m_copiedSymbol = obj as FamilySymbol;
            m_typesMgr = typesMgr;
        }

        /// <summary>
        /// hidden subclass' method;
        /// display EditTypeNameForm to get type name;
        /// then duplicate FamilySymbol
        /// </summary>
        public new DialogResult ShowDialog()
        {
            try
            {
                // generate the duplicate one's initial Name
                string initialTypeName = m_typesMgr.GenerateSymbolName(m_copiedSymbol.Name);
                // provide UI for user to edit the duplicate one's name
                using (EditTypeNameForm typeNameFrm = new EditTypeNameForm(initialTypeName))
                {
                    // cancel the command of duplicate
                    if (typeNameFrm.ShowDialog() != DialogResult.OK)
                    {
                        return DialogResult.Cancel;
                    }

                    // generate the duplicate one's Name used to create with Name edited in EditTypeNameForm
                    string finalTypeName = m_typesMgr.GenerateSymbolName(typeNameFrm.TypeName);
                    // duplicate FamilySymbol
                    m_newSymbol = m_typesMgr.DuplicateSymbol(m_copiedSymbol, finalTypeName);
                }
            }
            catch
            {
                MessageBox.Show("Failed to duplicate Type.", "Revit");
                return DialogResult.Abort;
            }
            return base.ShowDialog();
        }

        /// <summary>
        /// constructor without parameter is forbidden
        /// </summary>
        private DuplicateTypeForm()
        {
        }

        /// <summary>
        /// provide UI to edit is parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DuplicateTypeForm_Load(object sender, EventArgs e)
        {
            // initialize controls
            typeNameTextBox.Text = m_newSymbol.Name;
            familyTextBox.Text = m_newSymbol.Family.Name;
            FrameTypeParameters symbolParas = FrameTypeParameters.CreateInstance(m_newSymbol);
            if (null != symbolParas)
            {
                typeParameterPropertyGrid.SelectedObject = symbolParas;
            }
            else
            {
                typeParameterPropertyGrid.Enabled = false;
            }
        }

        /// <summary>
        /// to finish duplicate process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// cancel the duplicate and roll back the command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            // delete the duplicate one
            bool result = m_typesMgr.DeleteSymbol(m_newSymbol);
            if (result == false)
            {
                throw new ErrorMessageException("can not delete the new duplicate symbol");
            }
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}