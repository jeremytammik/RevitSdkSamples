//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Diagnostics;
using Autodesk.Revit.DB.Events;


namespace Revit.SDK.Samples.ProgressNotifier.CS
{
    /// <summary>
    /// An object containing a progress event name, position, and status.
    /// </summary>
    public class ProgressItem
    {
        /// <summary>
        /// Flag of progress
        /// </summary>
        private bool m_done;
        /// <summary>
        /// Name
        /// </summary>
        private string m_name;
        /// <summary>
        /// Lower
        /// </summary>
        private int m_lower;
        /// <summary>
        /// Upper
        /// </summary>
        private int m_upper;
        /// <summary>
        /// Position
        /// </summary>
        private int m_position;
        /// <summary>
        /// Progress stage
        /// </summary>
        ProgressStage m_stage;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lower"></param>
        /// <param name="upper"></param>
        /// <param name="position"></param>
        /// <param name="stage"></param>
        public ProgressItem(string name, int lower, int upper, int position, ProgressStage stage)
        {
            m_name = name;
            m_lower = lower;
            m_upper = upper;
            m_position = position;
            m_done = false;
            m_stage = stage;
        }

        /// <summary>
        /// Name property
        /// </summary>
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(m_name) || (m_name == " "))
                    return "<None>";
                else
                    return m_name;
            }
            set 
            { 
                m_name = value;
            }
        }

        /// <summary>
        /// IsDone property
        /// </summary>
        public bool IsDone
        {
            get 
            {
                return m_done;
            }
            set 
            { 
                m_done = value;
            }
        }

        /// <summary>
        /// Lower property
        /// </summary>
        public int Lower
        {
            get 
            { 
                return m_lower; 
            }
            set 
            { 
                m_lower = value; 
            }
        }

        /// <summary>
        /// Upper property
        /// </summary>
        public int Upper
        {
            get 
            { 
                return m_upper;
            }
            set 
            { 
                m_upper = value;
            }
        }

        /// <summary>
        /// Position property
        /// </summary>
        public int Position
        {
            get 
            { 
                return m_position; 
            }
            set 
            { 
                m_position = value;
            }
        }

        /// <summary>
        /// progress stage property
        /// </summary>
        public ProgressStage Stage
        {
            get 
            { 
                return m_stage; 
            }
            set 
            { 
                m_stage = value; 
            }
        }

        /// <summary>
        /// percent of progress
        /// </summary>
        /// <returns></returns>
        public double PercentDone()
        {
            return ((double)m_position / (double)(m_upper - m_lower)) * 100;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Name: " + Name + ", Stage: " + m_stage.ToString() + ", Percent Done: " + this.PercentDone().ToString("F") + ", Upper: " + m_upper + ", Position: " + m_position;
        }
    }
}
