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
using System.ComponentModel;
using System.Windows.Forms;

using Autodesk.Revit;

namespace Revit.SDK.Samples.ObjectViewer.CS
{
    /// <summary>
    /// a class used to create BindingList
    /// </summary>
    public class ParasFactory
    {
        private Element m_element;  // element that selected in Revit by Mouse
        private BindingList<Para> m_parasList;  //a list store parameters' information


        /// <summary>
        /// constructor of ParametersFactory
        /// </summary>
        /// <param name="element">the element of which parameters will be gotten</param>
        public ParasFactory(Element element)
        {
            m_element = element;
            m_parasList = new BindingList<Para>();
            //set m_parasList can be edit;
            m_parasList.AllowEdit = true;
        }


        /// <summary>
        /// add Para's instances to m_parasList, finally return it
        /// </summary>
        /// <returns></returns>
        public BindingList<Para> CreateParas()
        {
            try
            {
                ParameterSetIterator iterator = m_element.Parameters.ForwardIterator();
                Parameter parameter;
                iterator.Reset();

                //use Iterator to loop, new a Para for each parameter and 
                //add it to m_parameter; if failed return null
                for (; iterator.MoveNext(); )
                {
                    parameter = iterator.Current as Parameter;
                    if (null != parameter)
                    {
                        m_parasList.Add(new Para(parameter));
                    }
                }
            }
            catch(Exception e)
            {
                string errorText = "Create Paras failed: " + e.Message;
                MessageBox.Show(errorText);
                return null;
            }
            return m_parasList;
        }
    }
}
