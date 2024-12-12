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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Revit.DB.CodeChecking.Engineering;

namespace SectionPropertiesExplorer
{
    static public class Tools
    {
        static public string SectionShapeTypeName(SectionShapeType shapeType) 
        {
            string secName = "";
            switch (shapeType)
            {
                case SectionShapeType.RectangularHollowNotConstant:
                    secName += "RectangularHollowNotConstant";
                    break;
                case SectionShapeType.BoxBiSymmetrical:
                    secName += "BoxBiSymmetrical";
                    break;
                case SectionShapeType.BoxMonoSymmetrical:
                    secName += "BoxMonoSymmetrical";
                    break;
                case SectionShapeType.C:
                    secName += "C";
                    break;
                case SectionShapeType.CompoundSection:
                    secName += "CompoundSection";
                    break;
                case SectionShapeType.CrossBiSymmetrical:
                    secName += "CrossBiSymmetrical";
                    break;
                case SectionShapeType.DoubleRectangularBar:
                    secName += "DoubleRectangularBar";
                    break;
                case SectionShapeType.DoubleSection:
                    secName += "DoubleSection";
                    break;
                case SectionShapeType.I:
                    secName += "I";
                    break;
                case SectionShapeType.IASymmetrical:
                    secName += "IASymmetrical";
                    break;
                case SectionShapeType.L:
                    secName += "L";
                    break;
                case SectionShapeType.PolygonalBar:
                    secName += "PolygonalBar";
                    break;
                case SectionShapeType.PolygonalHollow:
                    secName += "PolygonalHollow";
                    break;
                case SectionShapeType.RectangularBar:
                    secName += "RectangularBar";
                    break;
                case SectionShapeType.RectangularHollowConstant:
                    secName += "RectangularHollowConstant";
                    break;
                case SectionShapeType.RoundBar:
                    secName += "RoundBar";
                    break;
                case SectionShapeType.HalfRoundBar:
                    secName += "HalfRoundBar";
                    break;
                case SectionShapeType.QuarterRoundBar:
                    secName += "QuarterRoundBar";
                    break;
                case SectionShapeType.T:
                    secName += "T";
                    break;
                case SectionShapeType.TAsymmetrical:
                    secName += "TAsymmetrical";
                    break;
                case SectionShapeType.RoundTube:
                    secName += "RoundTube";
                    break;
                case SectionShapeType.Unusual:
                    secName += "Unusual";
                    break;
                case SectionShapeType.Z:
                    secName += "Z";
                    break;
                default:
                    break;
            }

            return secName;
        }
    }
}
