//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// This class is used for window creation
    /// </summary>
    abstract class WindowCreation
    {
        /// <summary>
        /// The parameter of Window wizard
        /// </summary>
        public WizardParameter m_para;
        
        /// <summary>
        /// The constructor of WindowCreation
        /// </summary>
        /// <param name="parameter">WizardParameter</param>
        public WindowCreation(WizardParameter parameter)
        {
            m_para = parameter;
        }
        
        /// <summary>
        /// The function is used to create frame
        /// </summary>
        public abstract void CreateFrame();
        
        /// <summary>
        /// The function is used to create sash
        /// </summary>
        public abstract void CreateSash();
        
        /// <summary>
        /// The function is used to create glass
        /// </summary>
        public abstract void CreateGlass();
        
        /// <summary>
        /// The function is used to create material
        /// </summary>
        public abstract void CreateMaterial();
        
        /// <summary>
        /// The function is used to combine and build the window family
        /// </summary>
        public abstract void CombineAndBuild();
    }
}
