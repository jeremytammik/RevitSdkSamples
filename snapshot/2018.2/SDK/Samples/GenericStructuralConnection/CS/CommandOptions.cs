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
using System.Linq;
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.GenericStructuralConnection.CS
{
    /// <summary>
    /// Enum for user commands. 
    /// </summary>
    public enum CommandOption
    {
        /// <summary>
        /// Create generic connection. 
        /// </summary>
        CreateGeneric,
        /// <summary>
        /// Delete generic connection. 
        /// </summary>
        DeleteGeneric,
        /// <summary>
        /// Read generic connection. 
        /// </summary>
        ReadGeneric,
        /// <summary>
        /// Update generic connection. 
        /// </summary>
        UpdateGeneric,
        /// <summary>
        /// Create detailed connection. 
        /// </summary>
        CreateDetailed,
        /// <summary>
        /// Change detailed connection. 
        /// </summary>
        ChangeDetailed,
        /// <summary>
        /// Copy detailed connection. 
        /// </summary>
        CopyDetailed,
        /// <summary>
        /// Match properties of detailed connections. 
        /// </summary>
        MatchPropDetailed,
        /// <summary>
        /// Reset detailed connection. 
        /// </summary>
        ResetDetailed
    }
}
