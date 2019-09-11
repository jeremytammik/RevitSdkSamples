//
// (C) Copyright 2003-2019 by Autodesk, Inc. All rights reserved.
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
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.DuplicateViews.CS
{
    /// <summary>
    /// Utilities that facilitate duplication of drafting views and schedules into another document.
    /// </summary>
    class DuplicateViewUtils
    {
        /// <summary>
        /// Utility to duplicate schedules from one document to another.
        /// </summary>
        /// <param name="fromDocument">The source document.</param>
        /// <param name="views">The collection of schedule views.</param>
        /// <param name="toDocument">The target document.</param>
        public static void DuplicateSchedules(Document fromDocument,
                                                    IEnumerable<ViewSchedule> views,
                                                    Document toDocument)
        {
            // Use LINQ to convert to list of ElementIds for use in CopyElements() method
            List<ElementId> ids = 
                views.AsEnumerable<View>().ToList<View>().ConvertAll<ElementId>(ViewConvertToElementId);
            
            // Duplicate.  Pass false to make the function skip returning the map from source element to its copy
            DuplicateElementsAcrossDocuments(fromDocument, ids, toDocument, false);
        }

        /// <summary>
        /// Utility to duplicate drafting views and their contents from one document to another.
        /// </summary>
        /// <param name="fromDocument">The source document.</param>
        /// <param name="views">The collection of drafting views.</param>
        /// <param name="toDocument">The target document.</param>
        /// <returns>The number of drafting elements created in the copied views.</returns>
        public static int DuplicateDraftingViews(Document fromDocument, 
                                                    IEnumerable<ViewDrafting> views, 
                                                    Document toDocument)
        {
            // Return value
            int numberOfDetailElements = 0;

            // Transaction group for all activities
            using (TransactionGroup tg = new TransactionGroup(toDocument,
                "API - Duplication across documents with detailing"))
            {
                tg.Start();

                // Use LINQ to convert to list of ElementIds for use in CopyElements() method
                List<ElementId> ids = 
                    views.AsEnumerable<View>().ToList<View>().ConvertAll<ElementId>(ViewConvertToElementId);

                // Duplicate.  Pass true to get a map from source element to its copy
                Dictionary<ElementId, ElementId> viewMap =
                    DuplicateElementsAcrossDocuments(fromDocument, ids, toDocument, true);

                // For each copied view, copy the contents
                foreach (ElementId viewId in viewMap.Keys)
                {
                    View fromView = fromDocument.GetElement(viewId) as View;
                    View toView = toDocument.GetElement(viewMap[viewId]) as View;
                    numberOfDetailElements += DuplicateDetailingAcrossViews(fromView, toView);
                }
                          
                tg.Assimilate();
            }

            return numberOfDetailElements;
        }

        /// <summary>
        /// Duplicates a set of elements across documents.
        /// </summary>
        /// <param name="fromDocument">The source document.</param>
        /// <param name="elementIds">Collection of view ids.</param>
        /// <param name="toDocument">The target document.</param>
        /// <param name="findMatchingElements">True to return a map of matching elements 
        /// (matched by Name).  False to skip creation of this map.</param>
        /// <returns>The map of matching elements, if findMatchingElements was true.</returns>
        private static Dictionary<ElementId, ElementId> DuplicateElementsAcrossDocuments(Document fromDocument,
                                                   ICollection<ElementId> elementIds,
                                                   Document toDocument,
                                                    bool findMatchingElements)
        {
            // Return value
            Dictionary<ElementId, ElementId> elementMap = new Dictionary<ElementId, ElementId>();

            ICollection<ElementId> copiedIds;
            using (Transaction t1 = new Transaction(toDocument, "Duplicate elements"))
            {
                t1.Start();

                // Set options for copy-paste to hide the duplicate types dialog
                CopyPasteOptions options = new CopyPasteOptions();
                options.SetDuplicateTypeNamesHandler(new HideAndAcceptDuplicateTypeNamesHandler());

                // Copy the input elements.
                copiedIds =
                    ElementTransformUtils.CopyElements(fromDocument, elementIds, toDocument, Transform.Identity, options);

                // Set failure handler to hide duplicate types warnings which may be posted.
                FailureHandlingOptions failureOptions = t1.GetFailureHandlingOptions();
                failureOptions.SetFailuresPreprocessor(new HidePasteDuplicateTypesPreprocessor());
                t1.Commit(failureOptions);
            }

            // Find matching elements if required
            if (findMatchingElements)
            {
                // Build a map from name -> source element
                Dictionary<String, ElementId> nameToFromElementsMap = new Dictionary<string, ElementId>();

                foreach (ElementId id in elementIds)
                {
                    Element e = fromDocument.GetElement(id);
                    String name = e.Name;
                    if (!String.IsNullOrEmpty(name))
                        nameToFromElementsMap.Add(name, id);
                }

                // Build a map from name -> target element
                Dictionary<String, ElementId> nameToToElementsMap = new Dictionary<string, ElementId>();

                foreach (ElementId id in copiedIds)
                {
                    Element e = toDocument.GetElement(id);
                    String name = e.Name;
                    if (!String.IsNullOrEmpty(name))
                        nameToToElementsMap.Add(name, id);
                }

                // Merge to make source element -> target element map
                foreach (String name in nameToFromElementsMap.Keys)
                {
                    ElementId copiedId;
                    if (nameToToElementsMap.TryGetValue(name, out copiedId))
                    {
                        elementMap.Add(nameToFromElementsMap[name], copiedId);
                    }
                }
            }

            return elementMap;
        }

        /// <summary>
        /// Copies all view-specific elements from a source view to a target view.
        /// </summary>
        /// <remarks>
        /// The source and target views do not have to be in the same document.
        /// </remarks>
        /// <param name="fromView">The source view.</param>
        /// <param name="toView">The target view.</param>
        /// <returns>The number of new elements created during the copy operation.</returns>
        private static int DuplicateDetailingAcrossViews(View fromView, 
                                                        View toView)
        {
            // Collect view-specific elements in source view
            FilteredElementCollector collector = new FilteredElementCollector(fromView.Document, fromView.Id);

            // Skip elements which don't have a category.  In testing, this was
            // the revision table and the extents element, which should not be copied as they will
            // be automatically created for the copied view.
            collector.WherePasses(new ElementCategoryFilter(ElementId.InvalidElementId, true));

            // Get collection of elements to copy for CopyElements()
            ICollection<ElementId> toCopy = collector.ToElementIds();

            // Return value
            int numberOfCopiedElements = 0;

            if (toCopy.Count > 0)
            {
                using (Transaction t2 = new Transaction(toView.Document, "Duplicate view detailing"))
                {
                    t2.Start();
                    // Set handler to skip the duplicate types dialog
                    CopyPasteOptions options = new CopyPasteOptions();
                    options.SetDuplicateTypeNamesHandler(new HideAndAcceptDuplicateTypeNamesHandler());

                    // Copy the elements using no transformation
                    ICollection<ElementId> copiedElements =
                        ElementTransformUtils.CopyElements(fromView, toCopy, toView, Transform.Identity, options);
                    numberOfCopiedElements = copiedElements.Count;

                    // Set failure handler to skip any duplicate types warnings that are posted.
                    FailureHandlingOptions failureOptions = t2.GetFailureHandlingOptions();
                    failureOptions.SetFailuresPreprocessor(new HidePasteDuplicateTypesPreprocessor());
                    t2.Commit(failureOptions);
                }
            }

            return numberOfCopiedElements;
        }

        /// <summary>
        /// Converter delegate for conversion of collections
        /// </summary>
        /// <param name="view">The view.</param>
        /// <returns>The view's id.</returns>
        private static ElementId ViewConvertToElementId(View view)
        {
            return view.Id;
        }

    }

    /// <summary>
    /// A handler to accept duplicate types names created by the copy/paste operation.
    /// </summary>
    class HideAndAcceptDuplicateTypeNamesHandler : IDuplicateTypeNamesHandler
    {
        #region IDuplicateTypeNamesHandler Members

        /// <summary>
        /// Implementation of the IDuplicateTypeNameHandler
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DuplicateTypeAction OnDuplicateTypeNamesFound(DuplicateTypeNamesHandlerArgs args)
        {
            // Always use duplicate destination types when asked
            return DuplicateTypeAction.UseDestinationTypes;
        }

        #endregion
    }

    /// <summary>
    /// A failure preprocessor to hide the warning about duplicate types being pasted.
    /// </summary>
    class HidePasteDuplicateTypesPreprocessor : IFailuresPreprocessor
    {
        #region IFailuresPreprocessor Members

        /// <summary>
        /// Implementation of the IFailuresPreprocessor.
        /// </summary>
        /// <param name="failuresAccessor"></param>
        /// <returns></returns>
        public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
        {
            foreach (FailureMessageAccessor failure in failuresAccessor.GetFailureMessages())
            {
                // Delete any "Can't paste duplicate types.  Only non duplicate types will be pasted." warnings
                if (failure.GetFailureDefinitionId() == BuiltInFailures.CopyPasteFailures.CannotCopyDuplicates)
                {
                    failuresAccessor.DeleteWarning(failure);
                }
            }

            // Handle any other errors interactively
            return FailureProcessingResult.Continue;
        }

        #endregion
    }
}
