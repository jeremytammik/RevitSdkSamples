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
using System.Windows.Forms;
using Autodesk.Revit;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// The class is used to create window wizard form
    /// </summary>
    public class WindowWizard
    {
        /// <summary>
        /// store the WizardParameter
        /// </summary>
        private WizardParameter m_para;
        
        /// <summary>
        /// store the WindowCreation
        /// </summary>
        private WindowCreation m_winCreator;

        /// <summary>
        /// store the ExternalCommandData
        /// </summary>
        private ExternalCommandData m_commandData;

        /// <summary>
        /// constructor of WindowWizard
        /// </summary>
        /// <param name="commandData">the ExternalCommandData parameter</param>
        public WindowWizard(ExternalCommandData commandData)
        {
            m_commandData = commandData;
        }

        /// <summary>
        /// the method is used to show wizard form and do the creation
        /// </summary>
        /// <returns>the process result</returns>
        public int RunWizard()
        {
            int result = 0;
            m_para = new WizardParameter();           
            m_para.m_template = "DoubleHung";
            if (m_para.m_template == "DoubleHung")
            {
                m_winCreator = new DoubleHungWinCreation(m_para, m_commandData);
            }
            using (WizardForm form = new WizardForm(m_para))
            {
                switch(form.ShowDialog())
                {
                    case DialogResult.Cancel:
                        result=0;
                        break;
                    case DialogResult.OK:
                        if (Creation())
                            result = 1;
                        else
                            result = -1;
                        break;
                    default :
                        result=-1;
                        break;
                }           
            }
            return result;
        }

        /// <summary>
        /// The window creation process
        /// </summary>
        /// <returns>the result</returns>
        private bool Creation()
        {
            return m_winCreator.Creation();
        }

    }
}
