//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Drawing;

using Autodesk.Revit;
using Autodesk.Revit.Elements;

using CreApp = Autodesk.Revit.Creation.Application;

namespace Revit.SDK.Samples.FamilyExplorer.CS
{
    /// <summary>
    /// manages all families in current document.
    /// </summary>
    public class FamilyMgr
    {
        /// <summary>
        /// all families in current document
        /// </summary>
        List<FamilyWireFrame> m_allFamilies;
        public List<FamilyWireFrame> AllFamilies
        {
            get
            {
                return m_allFamilies;
            }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="document"></param>
        /// <param name="creApp"></param>
        public FamilyMgr(Document document, CreApp creApp)
        {
            if (null == document)
            {
                throw new ArgumentNullException("document");
            }

            if (null == creApp)
            {
                throw new ArgumentNullException("creApp");
            }

            m_allFamilies = new List<FamilyWireFrame>();
            ElementIterator itor = document.get_Elements(typeof(Family));
            itor.Reset();
            while (itor.MoveNext())
            {
                Family rfa = itor.Current as Family;
                if (null != rfa)
                {
                    m_allFamilies.Add(new FamilyWireFrame(rfa, creApp));
                }
            }
        }
                
    }
}
