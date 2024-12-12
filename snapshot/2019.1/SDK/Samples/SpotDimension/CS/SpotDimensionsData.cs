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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;


namespace Revit.SDK.Samples.SpotDimension.CS
{
    
    /// <summary>
    /// Store all the views and spot dimensions in Revit.
    /// </summary>
    public class SpotDimensionsData
    {
        UIApplication m_revit;  // Store the reference of the application in Revit
        List<string>          m_views         = new List<string>();
        List<Autodesk.Revit.DB.SpotDimension> m_spotDimensions = new List<Autodesk.Revit.DB.SpotDimension>(); //a list to store all SpotDimensions in the project

        /// <summary>
        /// a list of all the SpotDimensions in the project
        /// </summary>
        public ReadOnlyCollection<Autodesk.Revit.DB.SpotDimension> SpotDimensions
        {
            get
            {
                return new ReadOnlyCollection<Autodesk.Revit.DB.SpotDimension>(m_spotDimensions);
            }
        }

        /// <summary>
        /// a list of all the views that have SpotDimentions in the project
        /// </summary>
        public ReadOnlyCollection<string> Views
        {
            get
            {
                return new ReadOnlyCollection<string>(m_views);
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="commandData"></param>
        public SpotDimensionsData(ExternalCommandData commandData)
        {
             m_revit = commandData.Application;
             GetSpotDimensions();
        }
        
        /// <summary>
        /// try to find all the SpotDimensions and add them to the list
        /// </summary>
        private void GetSpotDimensions()
        {
            //get the active document 
            Document document = m_revit.ActiveUIDocument.Document;

            FilteredElementIterator elementIterator = (new FilteredElementCollector(document)).OfClass(typeof(Autodesk.Revit.DB.SpotDimension)).GetElementIterator();
            elementIterator.Reset();

            while (elementIterator.MoveNext())
            {

                //find all the SpotDimensions and views
                Autodesk.Revit.DB.SpotDimension tmpSpotDimension = elementIterator.Current as Autodesk.Revit.DB.SpotDimension;
                if (null != tmpSpotDimension)
                {
                    m_spotDimensions.Add(tmpSpotDimension);
                    if (m_views.Contains(tmpSpotDimension.View.Name) == false)
                    {
                        m_views.Add(tmpSpotDimension.View.Name);
                    }
                }
            }
        }
    }
}
