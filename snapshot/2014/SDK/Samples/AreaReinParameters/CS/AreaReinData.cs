//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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


namespace Revit.SDK.Samples.AreaReinParameters.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Autodesk.Revit.DB;
    using Autodesk.Revit.DB.Structure;

    /// <summary>
    /// all class that can be the datasource of propertygrid must inherit from it
    /// </summary>
    public interface IAreaReinData
    {
        /// <summary>
        /// fill datasource with the data of AreaReinforcement
        /// </summary>
        /// <param name="areaRein"></param>
        /// <returns>is successful</returns>
        bool FillInData(AreaReinforcement areaRein);
    }
}
