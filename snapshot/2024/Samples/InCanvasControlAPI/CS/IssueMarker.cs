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
   /// A simple object to keep the connection between marker control and the given element.
   /// </summary>
   public class IssueMarker
   {
      #region Class implementation

      private IssueMarker(ElementId elementId, int controlIndex, InCanvasControlData inCanvasControlData)
      {
         this.elementId = elementId;
         this.controlIndex = controlIndex;
         inCanvasData = inCanvasControlData;
      }

      /// <summary>
      /// Creates an issue marker. It also creates an In-Canvas control on given element's position.
      /// </summary>
      /// <param name="document">Document in which the tracked element is.</param>
      /// <param name="elementId">Tracked element id.</param>
      /// <returns>IssueMarker created from data</returns>
      public static IssueMarker Create(Document document, ElementId elementId)
      {
         ResourceProvider resourceProvider = ResourceProvider.GetInstance();

         // Prepare InCanvasControlData. It needs position and image path. 
         // In this example, all controls will share the same image - though it is possible to create controls with different images, or even change it via an update (see IssueMarkerSelector::SelectMarker).
         Element elementTracked = document.GetElement(elementId);

         XYZ elementLocation = new XYZ();
         if (elementTracked.Location is LocationPoint pointLoc)
         {
            elementLocation = pointLoc.Point;
         }
         else if (elementTracked.Location is LocationCurve curveLoc)
         {
            elementLocation = curveLoc.Curve.GetEndPoint(0);
         }

         InCanvasControlData inCanvasControlData = new InCanvasControlData(resourceProvider.IssueImage, elementLocation);

         // Create In-Canvas control
         TemporaryGraphicsManager manager = TemporaryGraphicsManager.GetTemporaryGraphicsManager(document);
         int controlIndex = manager.AddControl(inCanvasControlData, ElementId.InvalidElementId);

         return new IssueMarker(elementId, controlIndex, inCanvasControlData);
      }

      /// <summary>
      /// Data with which an In-Canvas control was created. We need to keep this to make small changes later on.
      /// </summary>
      public InCanvasControlData InCanvasControlData
      {
         get
         {
            return inCanvasData;
         }

         set
         {
            inCanvasData = value;
         }
      }

      /// <summary>
      /// Index of the control, returned by TemporaryGraphicsManager
      /// </summary>
      public int ControlIndex { get => controlIndex; }

      /// <summary>
      /// Id of the element that the marker tracks
      /// </summary>
      public ElementId TrackedElementId { get => elementId; }

      #endregion

      #region Class member variables

      private int controlIndex;
      private ElementId elementId;
      private InCanvasControlData inCanvasData;

      #endregion
   }
}
