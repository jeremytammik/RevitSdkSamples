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


namespace Revit.SDK.Samples.AreaReinParameters.CS
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    /// <summary>
    /// Layout Rules possible values of AreaReinforcement
    /// </summary>
    public enum LayoutRules
    {
        Fixed_Number = 2,
        Maximum_Spacing = 3
    }

    /// <summary>
    /// Hook Orientation possible values of AreaReinforcement which is on a floor
    /// </summary>
    public enum FloorHookOrientations
    {
        Up = 0,
        Down =2
    }

    /// <summary>
    /// Hook Orientation possible values of AreaReinforcement which is on a wall
    /// </summary>
    public enum WallHookOrientations
    {
        Towards_Exterior = 0,
        Towards_Interior = 2
    }
}
