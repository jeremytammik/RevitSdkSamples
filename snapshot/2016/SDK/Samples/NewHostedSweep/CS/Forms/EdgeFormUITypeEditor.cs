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
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.NewHostedSweep.CS
{
    /// <summary>
    /// This class is intent to provide a model dialog in property grid control.
    /// </summary>
    class EdgeFormUITypeEditor : UITypeEditor
    {
        /// <summary>
        /// Return the Modal style.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override UITypeEditorEditStyle GetEditStyle(
            System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Show a form to add or remove edges from hosted sweep, and also can change the 
        /// type of hosted sweep.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService winSrv = (IWindowsFormsEditorService)provider.
                GetService(typeof(IWindowsFormsEditorService));

            CreationData creationData = value as CreationData;
            creationData.BackUp();
            using (EdgeFetchForm form = new EdgeFetchForm(creationData))
            {
                if (winSrv.ShowDialog(form) == System.Windows.Forms.DialogResult.OK)
                    creationData.Update();
                else
                    creationData.Restore();                
            }
            return creationData;
        }
    }
}
