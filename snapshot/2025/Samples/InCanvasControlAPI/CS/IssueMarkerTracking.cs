//
// (C) Copyright 2003-2020 by Autodesk, Inc. All rights reserved.
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

using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Revit.SDK.Samples.InCanvasControlAPI.CS
{
   /// <summary>
   /// A class tracks all issue markers in a given document. It also tracks the index of the active selected marker.
   /// </summary>
   public class IssueMarkerTracking
   {
      #region Class implementation

      /// <summary>
      /// Creates IssueMarkerTracking for the opened document and initializes selected index
      /// </summary>
      /// <param name="document">An opened Revit document</param>
      public IssueMarkerTracking(Document document)
      {
         this.document = document;
         guid = Guid.NewGuid();
         selectedIndex = -1;
      }

      /// <summary>
      /// Adds a marker to this tracking
      /// </summary>
      /// <param name="marker">Marker to be updated by selector or updater.</param>
      public void SubscribeMarker(IssueMarker marker)
      {
         issueMarkerSet.Add(marker);
      }

      /// <summary>
      /// Removes marker that tracks the element specified by id.
      /// </summary>
      /// <param name="elementId">Tracked element id</param>
      public void RemoveMarkerByElement(ElementId elementId)
      {
         issueMarkerSet.RemoveWhere((m) => m.TrackedElementId == elementId);
      }

      /// <summary>
      /// Gets the issue marker that tracks element specified by id
      /// </summary>
      /// <param name="elementId">Tracked element id</param>
      /// <returns>A corresponding Issue Marker</returns>
      public IssueMarker GetMarkerByElementId(ElementId elementId)
      {
         return issueMarkerSet.Where((m) => m.TrackedElementId == elementId).FirstOrDefault();
      }

      /// <summary>
      /// Gets the issue marker by it's id in TemporaryGraphicsManager
      /// </summary>
      /// <param name="index">Index of the in-canvas control</param>
      /// <returns>A corresponding Issue Marker</returns>
      public IssueMarker GetMarkerByIndex(int index)
      {
         return issueMarkerSet.Where((m) => m.ControlIndex == index).FirstOrDefault();
      }

      /// <summary>
      /// Document this object tracks
      /// </summary>
      public Document Document { get => document; }

      /// <summary>
      /// Tracker's GUID. This is needed to safely clean up after document closes.
      /// </summary>
      public Guid Id { get => guid; }

      /// <summary>
      /// Gets the index of selected marker. This is used by selector
      /// </summary>
      /// <returns>The index of selected marker</returns>
      public int GetSelected()
      {
         return selectedIndex;
      }

      /// <summary>
      /// Sets the index of selected marker. This is used by selector
      /// </summary>
      /// <param name="index">Index of the marker</param>
      public void SetSelected(int index)
      {
         selectedIndex = index;
      }

      #endregion

      #region Class member variables

      private int selectedIndex;

      private HashSet<IssueMarker> issueMarkerSet = new HashSet<IssueMarker>();

      private Document document;

      private Guid guid;

      #endregion
   }
}
