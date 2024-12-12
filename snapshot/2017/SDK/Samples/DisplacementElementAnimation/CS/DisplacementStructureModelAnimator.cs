
//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using Autodesk.Revit.UI.Events;

namespace Revit.SDK.Samples.DisplacementElementAnimation.CS
{
    /// <summary>
    /// A class that executes an animation of structural model elements using DisplacementElements.
    /// </summary>
    class DisplacementStructureModelAnimator
    {
        /// <summary>
        /// Starts the animation by creating the needed displacement elements, and setting up the events to allow it proceed.
        /// </summary>
        public void StartAnimation()
        {
            // Instantiate or reset animation variables
            m_displacementElements = new List<DisplacementElement>();
            m_currentDisplacementIndex = 0;

            UIDocument uiDoc = uiApplication.ActiveUIDocument;
            View view = uiDoc.ActiveView;
            Document doc = uiDoc.Document;

            // Get levels in order of elevation
            FilteredElementCollector collector = new FilteredElementCollector(doc).OfClass(typeof(Level));
            List<ICollection<ElementId>> idGroupsInOrder = new List<ICollection<ElementId>>();
            IEnumerable<Level> levels = collector.Cast<Level>().OrderBy<Level, double>(lvl => lvl.Elevation);

            // Create lists of "elements on level" in ascending order:
            // * Foundation
            // * Framing (assumes framing consists of beams is placed to butt up to level above)
            // * Floors
            // * Walls
            // * Columns
            foreach (Level level in levels)
            {
                AddInstancesOnLevelToIdGroupList(idGroupsInOrder, level, BuiltInCategory.OST_StructuralFoundation);
                AddInstancesOnReferenceLevelToIdGroupList(idGroupsInOrder, level, BuiltInCategory.OST_StructuralFraming);
                AddInstancesOnLevelToIdGroupList(idGroupsInOrder, level, BuiltInCategory.OST_Floors);
                AddInstancesOnLevelToIdGroupList(idGroupsInOrder, level, BuiltInCategory.OST_Walls);
                AddInstancesOnLevelToIdGroupList(idGroupsInOrder, level, BuiltInCategory.OST_StructuralColumns);
            }

            // Initial setup of displacement elements for animation
            using (Transaction t = new Transaction(doc, "Start animation"))
            {
                t.Start();
                foreach (ICollection<ElementId> idGroups in idGroupsInOrder)
                {
                    BuildDisplacementElementGroup(doc, idGroups, view);
                }
                if (m_displacementElements.Count == 0)
                {
                   t.RollBack();
                   return;
                }
                m_displacementElement = m_displacementElements[0];
                UnhideDisplacedElements();
                t.Commit();
            }

            displacementParameter = 1.0;

            if (m_isUsingIdling)
            {
                // Register idling for animation frames
                uiApplication.Idling += IdlingResponse;
                
                // Register timer for animation framews
                m_timer = new System.Timers.Timer();
                m_timer.Interval = timerInterval;
                m_timer.Elapsed += TimerElapsed;
                m_timer.Start();
            }
        }

        /// <summary>
        /// Executes the next step on the animation.
        /// </summary>
        public void AnimateNextStep()
        {
            bool groupFinished = false;  // Is the current animation group finished?
            bool allFinished = false;    // Are all animation groups finished?
            if (displacementParameter <= 0)
            {
                displacementParameter = 1.0;
                groupFinished = true;
                m_currentDisplacementIndex++;
                if (m_currentDisplacementIndex == m_displacementElements.Count)
                {
                    allFinished = true;
                    if (m_isUsingIdling)
                    {
                        m_timer.Stop();
                        uiApplication.Idling -= IdlingResponse;
                    }
                }
            }

            // Execute transaction for next animation
            using (Transaction t = new Transaction(uiApplication.ActiveUIDocument.Document,
                                                   groupFinished ? "Next animation group" : "Animation step"))
            {
                t.Start();
                if (groupFinished)
                {
                    // Delete displacement element (and children)
                    uiApplication.ActiveUIDocument.Document.Delete(m_displacementElement.Id);

                    // Increment to next group
                    m_displacementElement = allFinished ? null : m_displacementElements[m_currentDisplacementIndex];
                    UnhideDisplacedElements();
                }
                else
                {
                    ChangeDisplacementLocationForParent();
                    ChangeDisplacementLocationsForChildren();

                    // Decrement displacement parameter
                    displacementParameter -= displacementIncrement;
                }
                t.Commit();
            }
        }

        /// <summary>
        /// Unhides the elements in the next displacement group.
        /// </summary>
        private void UnhideDisplacedElements()
        {
            if (m_displacementElement != null)
            {
                View view = m_displacementElement.Document.GetElement(m_displacementElement.OwnerViewId) as View;
                view.UnhideElements(m_displacementElement.GetDisplacedElementIdsFromAllChildren());
            }
        }

        /// <summary>
        /// Find all instances matching category and level, and add to the collection of groups of sorted ids.
        /// </summary>
        /// <param name="idGroupsInOrder">The collection of groups of ids, sorted.</param>
        /// <param name="level">The level to match.</param>
        /// <param name="category">The category to match.</param>
        private static void AddInstancesOnLevelToIdGroupList(List<ICollection<ElementId>> idGroupsInOrder,
                                                      Level level, BuiltInCategory category)
        {
            FilteredElementCollector collector = new FilteredElementCollector(level.Document);
            collector.WherePasses(new ElementLevelFilter(level.Id));
            collector.OfCategory(category);
            collector.WhereElementIsNotElementType();
            ICollection<ElementId> idGroup = collector.ToElementIds();

            // Only add non-empty groups
            if (idGroup.Count > 0)
                idGroupsInOrder.Add(idGroup);
        }

        /// <summary>
        /// Find all instances matching category and "Reference Level", and add to the collection of groups of sorted ids.
        /// </summary>
        /// <param name="idGroupsInOrder">The collection of groups of ids, sorted.</param>
        /// <param name="level">The level to match.</param>
        /// <param name="category">The category to match.</param>
        private static void AddInstancesOnReferenceLevelToIdGroupList(List<ICollection<ElementId>> idGroupsInOrder,
                                                               Level level, BuiltInCategory category)
        {
            FilteredElementCollector collector = new FilteredElementCollector(level.Document);
            collector.OfCategory(category);
            collector.WhereElementIsNotElementType();

            // Use a parameter filter to match the Reference Level parameter
            FilterRule rule =
                ParameterFilterRuleFactory.CreateEqualsRule(new ElementId(BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM),
                                                            level.Id);
            collector.WherePasses(new ElementParameterFilter(rule));
            ICollection<ElementId> idGroup = collector.ToElementIds();

            // Only add non-empty groups
            if (idGroup.Count > 0)
                idGroupsInOrder.Add(idGroup);
        }

        /// <summary>
        /// Builds a group of displacement elements from a collection of element ids, and 
        /// sets the displacement to be uniform in Z but vary in XY based on location.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="ids">The collection of ids.</param>
        /// <param name="view">The view.</param>
        private void BuildDisplacementElementGroup(Document doc, ICollection<ElementId> ids, View view)
        {
            // The last element will be in the "parent" displacement element.  (At least one element
            // must remain assigned to the parent, so all child displacements will be relative to the 
            // parent's displacement.)
            Element lastElement = doc.GetElement(ids.Last<ElementId>());
            XYZ parentDisplacedLocation = GetDisplacementXYFor(lastElement, XYZ.Zero);
            parentDisplacedLocation = MoveToElevation(parentDisplacedLocation, initialHeight);

            // All elements are added to the parent displacement element. 
            DisplacementElement parent = DisplacementElement.Create(doc, ids, parentDisplacedLocation, view, null);
            m_displacementElements.Add(parent);

            int count = ids.Count;
            List<ElementId> childIds = new List<ElementId>();
            List<ElementId> idsList = ids.ToList<ElementId>();

            // Add all elements except the last one to child displacement elements
            for (int index = 0; index < count - 1; index++)
            {
                ElementId childId = idsList[index];
                Element e = doc.GetElement(childId);

                XYZ displacedLocation = GetDisplacementXYFor(e, parentDisplacedLocation);

                // Setup id container for child DisplacementElement creation
                childIds.Clear();
                childIds.Add(childId);
                DisplacementElement child = DisplacementElement.Create(doc, childIds, displacedLocation, view, parent);
            }
            view.HideElements(ids);
        }

        /// <summary>
        /// Change the displacement location for the current parent element.
        /// </summary>
        private void ChangeDisplacementLocationForParent()
        {
            // Displacement includes displacement in Z
            XYZ displacement = GetDisplacementXY(m_displacementElement);
            displacement = MoveToElevation(displacement, GetHeightDisplacementValue());
            m_displacementElement.SetRelativeDisplacement(displacement);
        }

        /// <summary>
        /// Change the displacement location for children elements.
        /// </summary>
        private void ChangeDisplacementLocationsForChildren()
        {
            IList<DisplacementElement> subDisplacementElements = m_displacementElement.GetChildren();
            foreach (DisplacementElement element in subDisplacementElements)
            {
                // Displacement does not include change in Z
                XYZ displacedLocation = GetDisplacementXY(element);

                element.SetRelativeDisplacement(displacedLocation);
            }
        }

        /// <summary>
        /// Identifies if the displacement element is a parent or a child.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>True if the element is a child of another element.</returns>
        private static bool DisplacementElementIsChild(DisplacementElement element)
        {
            return element.ParentId != ElementId.InvalidElementId;
        }

        /// <summary>
        /// Gets the XY displacement for a given element.
        /// </summary>
        /// <param name="element">The displacement element.</param>
        /// <returns>The displacement.</returns>
        private XYZ GetDisplacementXY(DisplacementElement element)
        {
            // If the element is a child, need to take into account the displacement of the parent.
            XYZ displacementDueToParent = XYZ.Zero;
            if (DisplacementElementIsChild(element))
            {
                DisplacementElement parent = element.Document.GetElement(element.ParentId) as DisplacementElement;
                displacementDueToParent = parent.GetRelativeDisplacement();
            }

            // Assume one element per displacement, use that element to get the displacement needed.
            ElementId id = element.GetDisplacedElementIds().First<ElementId>();
            Element e = element.Document.GetElement(id);

            return GetDisplacementXYFor(e, displacementDueToParent);
        }

        /// <summary>
        /// Gets the XY displacement for a given element relative to a parent displacement.
        /// </summary>
        /// <param name="e">The element.</param>
        /// <param name="displacementDueToParent">The parent element displacement.</param>
        /// <returns>The XY displacement.</returns>
        private XYZ GetDisplacementXYFor(Element e, XYZ displacementDueToParent)
        {
            XYZ displacementDueToParentXY = MoveToElevationZero(displacementDueToParent);
            XYZ location = GetNominalCenterLocation(e);
            XYZ delta = (location - modelCenter);
            XYZ displacedLocation = delta * GetXYDisplacementRatio() - displacementDueToParentXY;

            return displacedLocation;
        }

        /// <summary>
        /// Returns the current XY displacement ratio.
        /// </summary>
        /// <returns>The XY displacement ratio.</returns>
        private double GetXYDisplacementRatio()
        {
            if (displacementParameter == 1.0)
                return initialXYRatio;
            return initialXYRatio * 1 / Math.Pow(initialHeight - GetHeightDisplacementValue(), 0.75);
        }

        /// <summary>
        /// Gets the current displcement in Z.
        /// </summary>
        /// <returns></returns>
        private double GetHeightDisplacementValue()
        {
            return initialHeight * displacementParameter;
        }

        /// <summary>
        /// The timer callback for the animation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timerTripped = true;
        }

        /// <summary>
        /// The idling callback for the animation.
        /// </summary>
        /// <param name="senter"></param>
        /// <param name="e"></param>
        private void IdlingResponse(object senter, IdlingEventArgs e)
        {
            // Set to reraise idling immediately (unaffected by user activity)
            e.SetRaiseWithoutDelay();

            if (timerTripped)
            {
                timerTripped = false;
                AnimateNextStep();
            }
        }

        /// <summary>
        /// Constructs an animator instance. 
        /// </summary>
        /// <param name="uiApp">The application.</param>
        /// <param name="isUsingIdling">The switch for using Idling event.</param>
        public DisplacementStructureModelAnimator(UIApplication uiApp, bool isUsingIdling)
        {
            uiApplication = uiApp;
            m_isUsingIdling = isUsingIdling;
        }

        /// <summary>
        /// Utility to move a point to the Z=0 projection 
        /// </summary>
        /// <param name="location">The point.</param>
        /// <returns>The point at Z=0 projection.</returns>
        private static XYZ MoveToElevationZero(XYZ location)
        {
            return MoveToElevation(location, 0);
        }

        /// <summary>
        /// Utility to move a point to an arbitrary Z projection 
        /// </summary>
        /// <param name="location">The point.</param>
        /// <param name="z">The elevation to set for the point.</param>
        /// <returns>The point at Z=0 projection.</returns>
        private static XYZ MoveToElevation(XYZ location, double z)
        {
            return new XYZ(location.X, location.Y, z);
        }

        /// <summary>
        /// Utility to get the "center" of the element based on location parameters, projected to Z=0.
        /// </summary>
        /// <param name="e">The element.</param>
        /// <returns>The center location.</returns>
        private static XYZ GetNominalCenterLocation(Element e)
        {
            LocationPoint lp = e.Location as LocationPoint;
            if (lp != null)
            {
                return MoveToElevationZero(lp.Point);
            }

            LocationCurve lc = e.Location as LocationCurve;
            if (lc != null)
            {
                return MoveToElevationZero(lc.Curve.Evaluate(0.5, true));
            }
            return XYZ.Zero;
        }

        /// <summary>
        /// The application.
        /// </summary>
        private UIApplication uiApplication;

        /// <summary>
        /// The collection of top level displacement elements.
        /// </summary>
        private List<DisplacementElement> m_displacementElements;


        /// <summary>
        /// The current parent displacement element being animated.
        /// </summary>
        private DisplacementElement m_displacementElement = null;

        /// <summary>
        /// The index of the current parent element being animated.
        /// </summary>
        private int m_currentDisplacementIndex = 0;

        /// <summary>
        /// The displacement parameter (proceeds from 1 -> 0 during the animation)
        /// </summary>
        private double displacementParameter = 1.0;

        /// <summary>
        /// The increment by which the displacement parameter is reduced during animation.
        /// </summary>
        private double displacementIncrement = 0.05;

        /// <summary>
        /// The maximum ratio of displacement in XY.
        /// </summary>
        private double initialXYRatio = 1.25;

        /// <summary>
        /// The initial height for the initial displacement.
        /// </summary>
        private double initialHeight = 100;

        /// <summary>
        /// Signals that the timer has triggered.
        /// </summary>
        private bool timerTripped = false;

        /// <summary>
        /// The number of milliseconds in between frames.
        /// </summary>
        private int timerInterval = 60;

        /// <summary>
        /// The timer that governs the automation.
        /// </summary>
        private System.Timers.Timer m_timer = null;

        /// <summary>
        /// The model center.  Currently hardcoded.
        /// </summary>
        private XYZ modelCenter = XYZ.Zero;

        /// <summary>
        /// The switch for using Idling event.
        /// </summary>
        private bool m_isUsingIdling = true;
    }
}
