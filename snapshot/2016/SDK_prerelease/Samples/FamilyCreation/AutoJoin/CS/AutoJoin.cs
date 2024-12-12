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
using System.Collections.Generic;

using Autodesk.Revit;
using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.AutoJoin.CS
{
    /// <summary>
    /// Join all the overlapping generic forms in this document.
    /// </summary>
    public class AutoJoin
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AutoJoin()
        {
            m_elements = new List<CombinableElement>();
        }

        /// <summary>
        /// Join geometry between overlapping solids.
        /// </summary>
        /// <param name="document">The active document</param>
        /// <returns>The number of geometry combination be joined in this document.</returns>
        public int Join(Document document)
        {
            int combinated = 0;

            // CombinableElement is of an element type that exists in the API, but not in Revit's native object model. 
            // We use a combination of GenericForm and GeomCombination elements instead to find all CombinableElement.
            LogicalOrFilter filter = new LogicalOrFilter(
                new ElementClassFilter(typeof(GenericForm)),
                new ElementClassFilter(typeof(GeomCombination)));

            FilteredElementIterator itor = (new FilteredElementCollector(document)).WherePasses(filter).GetElementIterator();
            itor.Reset();
            while (itor.MoveNext())
            {
                GenericForm gf = itor.Current as GenericForm;
                if (null != gf && !gf.IsSolid)
                    continue;

                CombinableElement ce = itor.Current as CombinableElement;
                if (null == ce)
                    continue;
                else
                    m_elements.Add(ce);
            }
            // Added all solid forms in this document.

            while (1 < m_elements.Count)
            {
                GeomCombination geomCombination = JoinOverlapping(m_elements, document);
                if (null == geomCombination)
                {
                    return combinated;//No overlapping.
                }

                combinated++;
            }

            return combinated;

        }

        /// <summary>
        /// Join the overlapped elements in the list.
        /// </summary>
        /// <param name="elements">the element list may includes overlapping.</param>
        /// <param name="document">the active document</param>
        /// <returns>the joined geometry combination, the joined elements is removed from the list.</returns>
        public GeomCombination JoinOverlapping(List<CombinableElement> elements, Document document)
        {
            CombinableElementArray joinedElements = new CombinableElementArray();

            // try to find the first overlapping.
            foreach (CombinableElement aElement in elements)
            {
                foreach (CombinableElement xElement in elements)
                {
                    if (IsOverlapped(aElement, xElement))
                    {
                        joinedElements.Append(aElement);
                        break;
                    }
                }
                if (1 == joinedElements.Size)
                    break;
            }

            if (0 == joinedElements.Size)
            {
                return null;//Can not find any overlapping.
            }

            // try to find all elements overlapped the first element.
            foreach (CombinableElement aElement in elements)
            {
                if (IsOverlapped(aElement, joinedElements.get_Item(0)))
                {
                    joinedElements.Append(aElement);
                }
            }

            List<CombinableElement> allCanJoin = new List<CombinableElement>();
            bool isNew = false;
            do
            {
                allCanJoin.Clear();
                isNew = false;

                // try to find all elements overlapped joinedElements
                foreach (CombinableElement aElement in joinedElements)
                {
                    foreach (CombinableElement xElement in elements)
                    {
                        if (IsOverlapped(aElement, xElement))
                        {
                            allCanJoin.Add(xElement);
                        }
                    }
                }

                foreach (CombinableElement aElement in allCanJoin)
                {
                    bool isContained = false;

                    for (int ii = 0; ii < joinedElements.Size; ii++)
                    {
                        if (aElement.Id.IntegerValue == joinedElements.get_Item(ii).Id.IntegerValue)
                        {
                            isContained = true;
                            break;
                        }
                    }

                    if (!isContained)
                    {
                        isNew = true;
                        joinedElements.Append(aElement);
                    }
                }
            } while (isNew);// find all elements which overlapped with joined geometry combination.


            // removed the joined elements from the input list.
            foreach (CombinableElement aElement in joinedElements)
            {
                elements.Remove(aElement);
            }

            return document.CombineElements(joinedElements);
        }

        /// <summary>
        /// Tell if the element A and B are overlapped.
        /// </summary>
        /// <param name="elementA">element A</param>
        /// <param name="elementB">element B</param>
        /// <returns>return true if A and B are overlapped, or else return false.</returns>
        public bool IsOverlapped(CombinableElement elementA, CombinableElement elementB)
        {
            if (elementA.Id.IntegerValue == elementB.Id.IntegerValue)
            {
                return false;
            }

            Options geOptions = Command.s_appCreation.NewGeometryOptions();
            return Intersection.IsOverlapped(elementA.get_Geometry(geOptions), elementB.get_Geometry(geOptions));
        }

        private List<CombinableElement> m_elements;// this element list to combine geometry.
    }
}
