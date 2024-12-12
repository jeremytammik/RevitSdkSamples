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
    /// A collection of ProgressItem objects arranged in a stack
    /// </summary>
    public class ProgressStack
    {
        /// <summary>
        /// ProgressItem stack
        /// </summary>
        public System.Collections.Generic.Stack<ProgressItem> m_itemStack;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressStack()
        {
            this.m_itemStack = new Stack<ProgressItem>();
        }

        /// <summary>
        /// Add event data
        /// </summary>
        /// <param name="progressEvent"></param>
        /// <returns></returns>
        public ProgressItem AddEventData(Autodesk.Revit.DB.Events.ProgressChangedEventArgs progressEvent)
        {
            ProgressItem currentProgressItem = null;

            
            switch (progressEvent.Stage)
            {
                case ProgressStage.Started:
                    {
                        ProgressItem pi = new ProgressItem(progressEvent.Caption, progressEvent.LowerRange, progressEvent.UpperRange, progressEvent.Position, progressEvent.Stage);
                        m_itemStack.Push(pi);
                        currentProgressItem = pi;
                        break;
                    }
                case ProgressStage.PositionChanged:
                    {

                        ProgressItem pi = m_itemStack.Peek();
                        if (pi.Name != progressEvent.Caption)
                        {
                            Debug.WriteLine("Name not matching?");
                        }
                        pi.Position = progressEvent.Position;
                        pi.Stage = progressEvent.Stage;
                        currentProgressItem = pi;
                        break;
                    }

                case ProgressStage.RangeChanged:
                    {
                        ProgressItem pi = m_itemStack.Peek();
                        pi.Upper = progressEvent.UpperRange;
                        pi.Stage = progressEvent.Stage;
                        currentProgressItem = pi;
                        break;
                    }

                case ProgressStage.Finished:
                    {
                        ProgressItem pi = m_itemStack.Pop();
                        pi.IsDone = true;
                        pi.Stage = progressEvent.Stage;
                        currentProgressItem = pi;
                        break;
                    }

                case ProgressStage.CaptionChanged:
                    {
                        ProgressItem pi = m_itemStack.Peek();
                        pi.Name = progressEvent.Caption;
                        pi.Stage = progressEvent.Stage;
                        Debug.WriteLine("Caption Change at top.");
                        currentProgressItem = pi;
                        break;
                    }

                case ProgressStage.Unchanged:
                    {
                        Debug.WriteLine("Idle.");
                        currentProgressItem = new ProgressItem(progressEvent.Caption, progressEvent.LowerRange, progressEvent.UpperRange, progressEvent.Position, progressEvent.Stage);
                        break;
                    }


                default:
                    throw new Exception("Unknown stage.");
            }


            if (m_itemStack.Count == 0)
            {
                Debug.WriteLine("Stack empty");
            }
            else
                Debug.WriteLine(this.ToString());

            return currentProgressItem;
        }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("-ProgressBar Stack-");
            foreach (ProgressItem pi in m_itemStack)
            {
                sb.AppendLine(pi.ToString());
            }
            return sb.ToString();
        }


        /// <summary>
        /// ToStringList
        /// </summary>
        /// <param name="padDepth"></param>
        /// <returns></returns>
        public List<String> ToStringList(int padDepth = 0)
        {
            List<string> itemList = new List<string>();

            if (padDepth != 0)
            {
                int padding = padDepth - m_itemStack.Count;

                for (int index = 0; index != padding; ++index)
                {
                    itemList.Add("");
                }
            }
            foreach (ProgressItem pi in m_itemStack)
            {
                itemList.Add(pi.ToString());
            }
            return itemList;
        }
    }
}
