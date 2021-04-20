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

namespace Revit.SDK.Samples.InCanvasControlAPI.CS
{
   /// <summary>
   /// This class demonstrates marking a marker selected by changing the in-canvas control's image.
   /// </summary>
   public class IssueMarkerSelector
   {
      #region Class implementation

      /// <summary>
      /// Changes selected issue marker in given document's tracking
      /// </summary>
      /// <param name="document">A Revit document</param>
      /// <param name="controlIndex">Id of the clicked In-Canvas control</param>
      public static void SelectMarker(Document document, int controlIndex)
      {
         TemporaryGraphicsManager tempGraphicsManager = TemporaryGraphicsManager.GetTemporaryGraphicsManager(document);
         IssueMarkerTracking issueMarkerTracking = IssueMarkerTrackingManager.GetInstance().GetTracking(document);
         ResourceProvider provider = ResourceProvider.GetInstance();

         // Check if the new selection is valid
         IssueMarker newSelectedMarker = issueMarkerTracking.GetMarkerByIndex(controlIndex);
         if (newSelectedMarker == null)
            return;

         // clear previous selection
         IssueMarker selectedMarker = issueMarkerTracking.GetMarkerByIndex(issueMarkerTracking.GetSelected());
         if (selectedMarker != null)
         {
            selectedMarker.InCanvasControlData.ImagePath = provider.IssueImage;

            // This is how to set updated data to a control
            tempGraphicsManager.UpdateControl(selectedMarker.ControlIndex, selectedMarker.InCanvasControlData);

            issueMarkerTracking.SetSelected(-1);
         }

         if (newSelectedMarker != selectedMarker)
         {
            newSelectedMarker.InCanvasControlData.ImagePath = provider.SelectedIssueImage;

            // This is how to set updated data to a control
            tempGraphicsManager.UpdateControl(newSelectedMarker.ControlIndex, newSelectedMarker.InCanvasControlData);

            issueMarkerTracking.SetSelected(controlIndex);
         }
      }
      
      #endregion
   }
}
