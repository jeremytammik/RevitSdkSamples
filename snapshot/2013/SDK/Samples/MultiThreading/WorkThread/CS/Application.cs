//
// (C) Copyright 2003-2012 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Autodesk;
using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Analysis;

namespace Revit.SDK.Samples.WorkThread.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalApplication
    /// </summary>
    public class Application : IExternalApplication
    {
        // instance of class Application
        internal static Application thisApp = null;

        // instance of class FaceAnalyzer
        private FaceAnalyzer m_analyzer = null;
        // event handler of idling
        private EventHandler<IdlingEventArgs> m_hIdling = null;
        // event handler of document changed
        private EventHandler<DocumentChangedEventArgs> m_hDocChanged = null;

        #region IExternalApplication Members
        /// <summary>
        /// Implements the OnShutdown event
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            if (m_analyzer != null)
            {
                m_analyzer.StopCalculation();
            }
            if (m_hIdling != null)
            {
                application.Idling -= m_hIdling;
            }
            if (m_hDocChanged != null)
            {
                application.ControlledApplication.DocumentChanged -= m_hDocChanged;
            }

            return Result.Succeeded;
        }

        /// <summary>
        /// Implements the OnStartup event
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public Result OnStartup(UIControlledApplication application)
        {
            thisApp = this;
            return Result.Succeeded;
        }

        /// <summary>
        ///   Kicking off an analysis of a wall face.
        /// </summary>
        /// <remarks>
        ///   After successfully starting a new application
        ///   the method subscribe to the Idling event in order to
        ///   periodically fetch data from the analyzer to Revit.
        ///   It also subscribes to DocumentChange to monitor
        ///   eventual changes to the element being analyzed.
        /// </remarks>
        /// 
        public void RunAnalyzer(UIApplication uiapp, String sref)
        {
            if (uiapp.ActiveUIDocument != null)
            {
                // we could have our document ready with a display style
                // but here we create our display style pro grammatically

                View view = uiapp.ActiveUIDocument.ActiveView;
                SetupDisplayStyle(view);

                // setting up a new analyzer,
                // then initializing it and starting it

                m_analyzer = new FaceAnalyzer(view, sref);
                m_analyzer.Initialize();

                // if we the calculating when off successfully
                // we know we need to subscribe to Idling
                // to get the results as they'll keep pouring in

                if (m_analyzer.StartCalculation())
                {
                    SubscribeToIdling(uiapp);
                    SubscribeToChanges(uiapp);
                }
            }
        }


        /// <summary>
        ///   Subscription to Idling
        /// </summary>
        /// <remarks>
        ///   We hold the delegate to remember we we have subscribed.
        /// </remarks>
        /// 
        private void SubscribeToIdling(UIApplication uiapp)
        {
            if (m_hIdling == null)
            {
                m_hIdling = new EventHandler<IdlingEventArgs>(IdlingHandler);
                uiapp.Idling += m_hIdling;
            }
        }


        /// <summary>
        ///   Unsubscribing from Idling event
        /// </summary>
        /// 
        private void UnsubscribeFromIdling(UIApplication uiapp)
        {
            if (m_hIdling != null)
            {
                uiapp.Idling -= m_hIdling;
                m_hIdling = null;
            }
        }


        /// <summary>
        ///   Subscription to DocumentChanged
        /// </summary>
        /// <remarks>
        ///   We hold the delegate to remember we we have subscribed.
        /// </remarks>
        /// 
        private void SubscribeToChanges(UIApplication uiapp)
        {
            if (m_hDocChanged == null)
            {
                m_hDocChanged = new EventHandler<DocumentChangedEventArgs>(DocChangedHandler);
                uiapp.Application.DocumentChanged += m_hDocChanged;
            }
        }


        /// <summary>
        ///   Unsubscribing from DocumentChanged event
        /// </summary>
        /// 
        private void UnsubscribeFromChanges(UIApplication uiapp)
        {
            if (m_hDocChanged != null)
            {
                uiapp.Application.DocumentChanged -= m_hDocChanged;
                m_hDocChanged = null;
            }
        }


        /// <summary>
        ///   Idling Handler
        /// </summary>
        /// <remarks>
        ///   It reaches out to the analyzer and ask it to update
        ///   the results in Revit if more data has been calculated
        ///   since the last time we asked.
        ///   <para>
        ///   If there is no more data available, we unsubscribe
        ///   from the Idling event, for we do not need it anymore.
        ///   </para>
        /// </remarks>
        /// 
        public void IdlingHandler(object sender, IdlingEventArgs args)
        {
            bool processing = false;
            if (m_analyzer != null)
            {
                UIApplication uiapp = sender as UIApplication;
                if (uiapp.ActiveUIDocument != null)
                {
                    // In order for the analysis to appear correctly in the view
                    // we seem to need the mechanism of a transaction to be run
                    // even though the results are not really parts of the document.

                    using (Transaction trans = new Transaction(uiapp.ActiveUIDocument.Document))
                    {
                        trans.Start("bogus transaction");
                        processing = m_analyzer.UpdateResults();
                        trans.Commit();
                    }

                    // In our case, we want Revit to get back to as as soon as possible
                    args.SetRaiseWithoutDelay();
                }
            }

            // We do not need the event once the analysis is over

            if (!processing)
            {
                UnsubscribeFromIdling(sender as UIApplication);
                m_analyzer = null;
            }
        }


        /// <summary>
        ///   DocumentChanged Handler
        /// </summary>
        /// <remarks>
        ///   It monitors changes to the element that is being analyzed.
        ///   If the element was changed, we ask it to restart the analysis.
        ///   If the element was deleted, we ask the analyzer to stop.
        /// </remarks>
        /// 
        public void DocChangedHandler(object sender, DocumentChangedEventArgs args)
        {
            if (m_analyzer != null)
            {
                // first we check if the element was deleted

                ICollection<ElementId> elems = args.GetDeletedElementIds();
                if (elems.Contains(m_analyzer.AnalyzedElementId))
                {
                    m_analyzer.StopCalculation();
                    m_analyzer = null;

                    // if we've stopped, we do not need events anymore
                    UnsubscribeFromIdling(sender as UIApplication);
                    UnsubscribeFromChanges(sender as UIApplication);
                }
                else   // not deleted? what about changed?
                {
                    elems = args.GetModifiedElementIds();
                    if (elems.Contains(m_analyzer.AnalyzedElementId))
                    {
                        m_analyzer.RestartCalculation();
                    }
                }
            }
            else   // no analyzer => no need for the events anymore
            {
                UnsubscribeFromIdling(sender as UIApplication);
                UnsubscribeFromChanges(sender as UIApplication);
            }
        }


        /// <summary>
        ///   We setup our preferred style for displaying the results
        /// </summary>
        /// <remarks>
        ///   This is to make it easier to run this sample on any document.
        ///   We create a gradient-like style (unless it already exists)
        ///   and register it with the given view. Then we set it as the
        ///   default analysis stile in that view.
        /// </remarks>
        /// 
        private void SetupDisplayStyle(Autodesk.Revit.DB.View view)
        {
            const string styleName = "SDK2013-AL Style";
            AnalysisDisplayStyle ourStyle = null;

            // check if we are already using our preferred display style

            if (ElementId.InvalidElementId != view.AnalysisDisplayStyleId)
            {
                ourStyle = view.Document.GetElement(view.AnalysisDisplayStyleId) as AnalysisDisplayStyle;
                if (ourStyle.Name == styleName)
                {
                    return;
                }
            }

            // Look if the style exist at all in the document

            FilteredElementCollector collector = new FilteredElementCollector(view.Document);
            ICollection<Element> allStyles = collector.OfClass(typeof(AnalysisDisplayStyle)).ToElements();
            foreach (Element elem in allStyles)
            {
                if (elem.Name == styleName)
                {
                    using (Transaction trans = new Transaction(view.Document))
                    {
                        trans.Start("Change Analysis Display Style");
                        view.AnalysisDisplayStyleId = elem.Id;
                        trans.Commit();
                        return;
                    }
                }
            }

            // we do not have out style yet - let's create it

            // a) grid lines
            AnalysisDisplayColoredSurfaceSettings surface = new AnalysisDisplayColoredSurfaceSettings();
            surface.ShowGridLines = true;

            // b) colors
            AnalysisDisplayColorSettings colors = new AnalysisDisplayColorSettings();
            Color orange = new Color(255, 205, 0);
            Color green = new Color(0, 255, 0);
            colors.MinColor = orange;
            colors.MaxColor = green;

            // c) the legend
            AnalysisDisplayLegendSettings legend = new AnalysisDisplayLegendSettings();
            legend.NumberOfSteps = 10;
            legend.Rounding = 0.1;
            legend.ShowDataDescription = false;
            legend.ShowLegend = false;

            // creation of a style needs to be in a transaction
            using (Transaction trans = new Transaction(view.Document))
            {
                trans.Start("Set Analysis Display Style");
                ourStyle = AnalysisDisplayStyle.CreateAnalysisDisplayStyle(view.Document, styleName, surface, colors, legend);
                view.AnalysisDisplayStyleId = ourStyle.Id;
                trans.Commit();
            }
        }

        #endregion
    }
}
