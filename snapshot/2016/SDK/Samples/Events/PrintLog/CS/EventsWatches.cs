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
using System.Text;
using System.Diagnostics;

using Autodesk.Revit;

namespace Revit.SDK.Samples.PrintLog.CS
{
    /// <summary>
    /// This class consists of two watches for view print and document print processes. 
    /// Each watch will be used to calculate the cost time of print individual view and total views..
    /// </summary>
    public sealed class EventsWatches
    {
        #region Class Member Variables
        /// <summary>
        /// Time watch for view print process.
        /// It'll be renewed when ViewPrinting is raised and then used to calculate the print time 
        /// for each view; the calculation will occurs in ViewPrinted event.
        /// </summary>
        private Stopwatch m_viewPrintWatch;

        /// <summary>
        /// Time watch for document print process.
        /// It'll be renewed when DocumentPrinting is raised and then used to calculate the print time 
        /// for all views; the calculation will occurs in DocumentPrinted event.
        /// </summary>
        private Stopwatch m_docPrintWatch;
        #endregion


        #region Class Public Properties
        /// <summary>
        /// Get/set the watch of view print process.
        /// </summary>
        public Stopwatch ViewPrintWatch
        {
            get 
            { 
                return m_viewPrintWatch; 
            }
            set
            { 
                m_viewPrintWatch = value; 
            }
        }

        /// <summary>
        /// Get/set the watch of document print process.
        /// </summary>
        public Stopwatch DocPrintWatch
        {
            get 
            { 
                return m_docPrintWatch;
            }
            set 
            { 
                m_docPrintWatch = value;
            }
        }
        #endregion
    }
}
