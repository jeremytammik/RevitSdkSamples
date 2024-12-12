//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace Revit.SDK.Samples.WindowWizard.CS
{
    /// <summary>
    /// A common class for users to get some specified element
    /// </summary>
    class Utility
    {
        /// <summary>
        /// This method is used to allow user to get reference plane by name,if there is no proper reference plane,will return null
        /// </summary>
        /// <param name="name">the name property of reference plane</param>
        /// <param name="app">the application</param>
        /// <param name="doc">the document</param>
        /// <returns>the reference plane or null</returns>
        public static ReferencePlane GetRefPlaneByName(string name, UIApplication app,Document doc)
        {
            ReferencePlane r = null;
            FilteredElementCollector collector = new FilteredElementCollector(app.ActiveUIDocument.Document);
            collector.OfClass(typeof(ReferencePlane));
            FilteredElementIterator eit = collector.GetElementIterator();
            eit.Reset();
            while (eit.MoveNext())
            {
                r = eit.Current as ReferencePlane;
                if (r.Name.Equals(name))
                {
                    break;
                }
            }
            return r;
        }

        /// <summary>
        /// This method allows user to get view by name
        /// </summary>
        /// <param name="name">the name property of view</param>
        /// <param name="app">the application</param>
        /// <param name="doc">the document</param>
        /// <returns>the view or null</returns>
        public static View GetViewByName(string name,UIApplication app,Document doc)
        {
            View v = null;
            FilteredElementCollector collector = new FilteredElementCollector(app.ActiveUIDocument.Document);
            collector.OfClass(typeof(View));
            FilteredElementIterator eit = collector.GetElementIterator();
            eit.Reset();
            while (eit.MoveNext())
            {
                v = eit.Current as View;
                if (v.Name.Equals(name))
                {
                    break;
                }
            }
            return v;
        }

        /// <summary>
        /// This method is used to get elements by type filter
        /// </summary>
        /// <typeparam name="T">the type</typeparam>
        /// <param name="app">the application</param>
        /// <param name="doc">the document</param>
        /// <returns>the list of elements</returns>
        public static List<T> GetElements<T>(UIApplication app,Document doc) where T : Autodesk.Revit.DB.Element
        {
            List<T> elements = new List<T>();

            FilteredElementCollector collector = new FilteredElementCollector(app.ActiveUIDocument.Document);
            collector.OfClass(typeof(T));
            FilteredElementIterator eit = collector.GetElementIterator();
            eit.Reset();
            while (eit.MoveNext())
            {
                T element = eit.Current as T;
                if (element != null)
                {
                    elements.Add(element);                 
                }
            }         
            return elements;
        }

        /// <summary>
        /// This function is used to convert from metric to imperial
        /// </summary>
        /// <param name="value">the metric value</param>
        /// <returns>the result</returns>
        public static double MetricToImperial(double value)
        {
            return value / 304.8; //* 0.00328;
        }

        /// <summary>
        ///  This function is used to convert from imperial to metric
        /// </summary>
        /// <param name="value">the imperial value</param>
        /// <returns>the result</returns>
        public static double ImperialToMetric(double value)
        {
            return value*304.8;
        }
    }
}
