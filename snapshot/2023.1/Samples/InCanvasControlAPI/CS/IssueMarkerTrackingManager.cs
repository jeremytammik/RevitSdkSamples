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
   /// This class manages instances of IssueMarkerTracking on per-document basis. 
   /// </summary>
   public class IssueMarkerTrackingManager
   {
      #region Class implementation

      private IssueMarkerTrackingManager()
      {
      }

      /// <summary>
      /// Gets an instance of IssueMarkerTrackingManager.
      /// </summary>
      /// <returns>An instance of IssueMarkerTrackingManager</returns>
      public static IssueMarkerTrackingManager GetInstance()
      {
         if(manager == null)
         {
            manager = new IssueMarkerTrackingManager();
         }
         return manager;
      }

      /// <summary>
      /// Gets tracking for specified document
      /// </summary>
      /// <param name="doc">A Revit document</param>
      /// <returns>A corresponding instance of IssueMarkerTracking</returns>
      public IssueMarkerTracking GetTracking(Document doc) 
      {
         if (trackings.Where((track) => track.Document.Equals(doc)).FirstOrDefault() is IssueMarkerTracking tracking)
            return tracking;
         return null;
      }

      /// <summary>
      /// Adds IssueMarkerTracking for the given document
      /// </summary>
      /// <param name="doc">A Revit document</param>
      public void AddTracking(Document doc) 
      {
         if(!trackings.Any((track) => track.Document.Equals(doc)))
            trackings.Add(new IssueMarkerTracking(doc));
      }

      /// <summary>
      /// Removes IssueMarkerTracking from this manager
      /// </summary>
      /// <param name="guid">A GUID of the tracking</param>
      public void DeleteTracking(Guid guid) 
      {
         trackings.RemoveWhere((track) => track.Id == guid);
      }

      /// <summary>
      /// Clears all trackings
      /// </summary>
      public void ClearTrackings()
      {
         trackings.Clear();
      }

      #endregion

      #region Class member variables

      private static IssueMarkerTrackingManager manager;

      private HashSet<IssueMarkerTracking> trackings = new HashSet<IssueMarkerTracking>();

      #endregion
   }
}
