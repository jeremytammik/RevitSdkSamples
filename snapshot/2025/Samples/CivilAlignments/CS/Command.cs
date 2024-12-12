//
// (C) Copyright 2003-2023 by Autodesk, Inc.
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
using System.Linq;
using System.Collections.Generic;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Infrastructure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using TaskDialog = Autodesk.Revit.UI.TaskDialog;

namespace Revit.SDK.Samples.CivilAlignments.CS
{
   /// <summary>
   /// Selection filter class to allow selection of alignments.
   /// </summary>
   public class AlignmentSelectionFilter : ISelectionFilter
   {
      /// <summary>
      /// Alignment selection filter constructor.
      /// </summary>
      public AlignmentSelectionFilter() { }

      /// <summary>
      /// Allows element if it is an alignment.
      /// </summary>
      /// <param name="element"></param>
      /// <returns></returns>
      public bool AllowElement(Element element)
      {
         return element.Category.BuiltInCategory == BuiltInCategory.OST_Alignments;
      }

      /// <summary>
      /// Allows any alignment reference.
      /// </summary>
      /// <param name="refer"></param>
      /// <param name="point"></param>
      /// <returns></returns>
      public bool AllowReference(Reference refer, XYZ point)
      {
         return true;
      }

      /// <summary>
      /// Initiate alignment selection.
      /// </summary>
      /// <param name="document"></param>
      /// <returns></returns>
      public static ElementId SelectAlignment(Autodesk.Revit.DB.Document document)
      {
         UIDocument uidoc = new UIDocument(document);
         ISelectionFilter selFilter = new AlignmentSelectionFilter();
         Reference alignmentRef = uidoc.Selection.PickObject
            (ObjectType.Element, selFilter, "Select an alignment");

         return alignmentRef == null ? ElementId.InvalidElementId : alignmentRef.ElementId;
      }
   }

   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// For a selected alignment, places major and minor alignment station label sets.
   /// The set placement settings are tailored for a metric project, to demonstrate necessary units conversions.
   /// The major stations are created every 100m along the whole length of the alignment, 
   /// the minor stations every 10m for the first 100m.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class CreateAlignmentStationLabelsCmd : IExternalCommand
   {
      #region StationSettings
      // settings related to how major and minor station sets are created:
      // type names, intervals, offsets
      const string minorStationLeaderArrowheadName = "Minor Station Arrowhead";
      const string majorStationLeaderArrowheadName = "Major Station Arrowhead";
      const string majorStationSetLabelTypeName    = "Major Station Set Label Type";
      const string minorStationSetLabelTypeName    = "Minor Station Set Label Type";
      const string horizontalCurvatureChangeLabelTypeName = "Horizontal Curvature Change Label Type";
      const double majorStationInterval   = 100.0;   // 100m
      const double minorStationInterval   = 10.0;    // 10m
      const double labelTextOffset        = 0.005;   // 5mm, defined in paper space
      #endregion

      #region InterfaceImplementation
      /// <summary>
      /// Implement this method as an external command for Revit.
      /// </summary>
      /// <param name="commandData">An object that is passed to the external application 
      /// which contains data related to the command, 
      /// such as the application object and active view.</param>
      /// <param name="message">A message that can be set by the external application 
      /// which will be displayed if a failure or cancellation is returned by 
      /// the external command.</param>
      /// <param name="elements">A set of elements to which the external application 
      /// can add elements that are to be highlighted in case of failure or cancellation.</param>
      /// <returns>Return the status of the external command. 
      /// A result of Succeeded means that the API external method functioned as expected. 
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public virtual Result Execute(ExternalCommandData commandData
         , ref string message, ElementSet elements)
      {
         try
         {
            Document document = commandData.Application.ActiveUIDocument.Document;
            Autodesk.Revit.DB.View view = commandData.View;

            // find major and minor station arrow styles
            FilteredElementCollector fec = new FilteredElementCollector(document);
            Element majorStationLeaderArrowheadType = fec.OfClass(typeof(ElementType))
               .Cast<ElementType>()
               .FirstOrDefault(x => x.Name.Contains(majorStationLeaderArrowheadName));
            Element minorStationLeaderArrowheadType = fec.OfClass(typeof(ElementType))
               .Cast<ElementType>()
               .FirstOrDefault(x => x.Name.Contains(minorStationLeaderArrowheadName));
            if (majorStationLeaderArrowheadType == null || minorStationLeaderArrowheadType == null)
            {
               TaskDialog td = new TaskDialog("Missing arrowheads");
               td.MainContent = "In Manage>Additional Settings>Arrowheads, create two styles of arrowheads, named\r\n" +
                  "Major Station Arrowhead and Minor Station Arrowhead";
               td.Show();

               return Result.Failed;
            }

            // find major and minor station label types; if not there, create them
            SpotDimensionType majorLabelType = fec.OfClass(typeof(SpotDimensionType))
               .Cast<SpotDimensionType>()
               .FirstOrDefault(sdt => sdt.StyleType == DimensionStyleType.AlignmentStationLabel && sdt.Name.Contains(majorStationSetLabelTypeName));

            SpotDimensionType minorLabelType = fec.OfClass(typeof(SpotDimensionType))
               .Cast<SpotDimensionType>()
               .FirstOrDefault(sdt => sdt.StyleType == DimensionStyleType.AlignmentStationLabel && sdt.Name.Contains(minorStationSetLabelTypeName));

            SpotDimensionType curvatureLabelType = fec.OfClass(typeof(SpotDimensionType))
               .Cast<SpotDimensionType>()
               .FirstOrDefault(sdt => sdt.StyleType == DimensionStyleType.AlignmentStationLabel && sdt.Name.Contains(horizontalCurvatureChangeLabelTypeName));

            using (Transaction t = new Transaction(document, "Create major station labels"))
            {
               t.Start();
               if (majorLabelType == null)
               {
                  // create major station label type with the given arrowhead style
                  majorLabelType = document.GetElement(AlignmentStationLabel.CreateRecommendedTypeForSet(document)) as SpotDimensionType;
                  majorLabelType.get_Parameter(BuiltInParameter.SPOT_ELEV_LEADER_ARROWHEAD).Set(majorStationLeaderArrowheadType.Id);
                  majorLabelType.Name = majorStationSetLabelTypeName;
               }

               if (minorLabelType == null)
               {
                  // create minor station label type with the given arrowhead style
                  // exclude the station text, which leave only the arrowheads
                  // make the minor station's color grey
                  // make the text 60% of the original value, in case text is later turned on
                  minorLabelType = document.GetElement(AlignmentStationLabel.CreateRecommendedTypeForSet(document)) as SpotDimensionType;
                  minorLabelType.get_Parameter(BuiltInParameter.SPOT_ELEV_LEADER_ARROWHEAD).Set(minorStationLeaderArrowheadType.Id);
                  minorLabelType.get_Parameter(BuiltInParameter.ALIGNMENT_STATION_LABEL_INCLUDE_STATION).Set(0);
                  minorLabelType.get_Parameter(BuiltInParameter.LINE_COLOR).Set(8421504 /* 127*2^0 + 127*2^8 + 127*2^16: grey */);
                  Parameter textSizeParam = minorLabelType.get_Parameter(BuiltInParameter.TEXT_SIZE);
                  textSizeParam.Set(textSizeParam.AsDouble() * 0.6);
                  minorLabelType.Name = minorStationSetLabelTypeName;
               }

               if (curvatureLabelType == null)
               {
                  // create a new label type, based on the default alignment station label type,
                  // but with some adjustments to the label contents, as described below
                  ElementType defaultAlignmentLabelType = document.GetElement(
                           document.GetDefaultElementTypeId(ElementTypeGroup.AlignmentStationLabelType)) as ElementType;

                  curvatureLabelType = defaultAlignmentLabelType.Duplicate(horizontalCurvatureChangeLabelTypeName) as SpotDimensionType;

                  curvatureLabelType.get_Parameter(BuiltInParameter.SPOT_COORDINATE_BASE).Set(1);                    // "Shared" coordinate base

                  // Label position and content
                  curvatureLabelType.get_Parameter(BuiltInParameter.SPOT_ELEV_ROTATE_WITH_COMPONENT).Set(0);         // do not rotate with component
                  curvatureLabelType.get_Parameter(BuiltInParameter.SPOT_ELEV_TEXT_ORIENTATION).Set(0);              // horizontal text
                  curvatureLabelType.get_Parameter(BuiltInParameter.SPOT_ELEV_TEXT_LOCATION).Set(0);                 // text location above leader
                  curvatureLabelType.get_Parameter(BuiltInParameter.ALIGNMENT_STATION_LABEL_INCLUDE_STATION).Set(1); // include station
                  curvatureLabelType.get_Parameter(BuiltInParameter.SPOT_COORDINATE_INCLUDE_ELEVATION).Set(0);       // do not include elevation
                  curvatureLabelType.get_Parameter(BuiltInParameter.SPOT_ELEV_BOT_VALUE).Set(0);                     // do not include bottom value
                  curvatureLabelType.get_Parameter(BuiltInParameter.SPOT_ELEV_TOP_VALUE).Set(0);                     // do not include top value
                  curvatureLabelType.get_Parameter(BuiltInParameter.ALIGNMENT_STATION_LABEL_IND_STATION).Set("");    // empty station indicator

                  // Text
                  curvatureLabelType.get_Parameter(BuiltInParameter.DIM_TEXT_BACKGROUND).Set(0);                     // nontransparent text
                  curvatureLabelType.get_Parameter(BuiltInParameter.LINE_COLOR).Set(255 /* 255*2^0 + 0*2^8 + 0*2^16: red */); // text in red color
                  Parameter textSizeParam = curvatureLabelType.get_Parameter(BuiltInParameter.TEXT_SIZE);
                  textSizeParam.Set(textSizeParam.AsDouble() * 0.6);                                                 // text size 60% of default

                  // Leader
                  curvatureLabelType.get_Parameter(BuiltInParameter.SPOT_ELEV_LEADER_ARROWHEAD).Set(ElementId.InvalidElementId); // no leader arrowhead
               }
               t.Commit();
            }

            // create major and minor station label sets
            ElementId alignmentId = AlignmentSelectionFilter.SelectAlignment(document);
            Alignment alignment = Alignment.Get(document.GetElement(alignmentId));

            // start placement from a multiple of the major station interval
            // make sure to compute the multiple in the proper unit system and then convert it back to internal units for further use
            double labelSetsPlacementStartStation = UnitUtils.ConvertFromInternalUnits(alignment.DisplayedStartStation, UnitTypeId.StationingMeters);
            labelSetsPlacementStartStation =
               Math.Ceiling(labelSetsPlacementStartStation / majorStationInterval) * majorStationInterval;
            labelSetsPlacementStartStation =
               UnitUtils.ConvertToInternalUnits(labelSetsPlacementStartStation, UnitTypeId.StationingMeters);

            var majorStations = new List<double>();
            using (Transaction t = new Transaction(document, "Create major station labels"))
            {
               t.Start();
               AlignmentStationLabelSetOptions options = new AlignmentStationLabelSetOptions();
               options.Interval = UnitUtils.ConvertToInternalUnits(majorStationInterval, UnitTypeId.StationingMeters);
               options.Offset = UnitUtils.ConvertToInternalUnits(labelTextOffset, UnitTypeId.StationingMeters);
               options.StartStation = labelSetsPlacementStartStation;
               options.EndStation = alignment.DisplayedEndStation;
               options.TypeId = majorLabelType.Id;

               var labels = AlignmentStationLabel.CreateSet(alignment, view, options);
               foreach (var label in labels)
                  majorStations.Add(label.Station);
               t.Commit();
            }

            using (Transaction t = new Transaction(document, "Create minor station labels"))
            {
               t.Start();
               AlignmentStationLabelSetOptions options = new AlignmentStationLabelSetOptions();
               options.Interval = UnitUtils.ConvertToInternalUnits(minorStationInterval, UnitTypeId.StationingMeters);
               // setting text offset specification can be skipped, 
               // as in this example the minor station labels do not include any label text, only leader arrowheads
               options.StartStation = labelSetsPlacementStartStation;
               options.EndStation = labelSetsPlacementStartStation + UnitUtils.ConvertToInternalUnits(majorStationInterval, UnitTypeId.StationingMeters);
               options.TypeId = minorLabelType.Id;

               // delete the minor station labels which overlap with the major ones
               var labels = AlignmentStationLabel.CreateSet(alignment, view, options);
               foreach (var label in labels)
               {
                  foreach (var majorStation in majorStations)
                  {
                     if (MathComparisonUtils.IsAlmostEqual(label.Station, majorStation))
                     {
                        label.Element.Pinned = false;
                        document.Delete(label.Element.Id);
                        break;
                     }
                  }
               }

               t.Commit();
            }

            if (view.ViewType == ViewType.FloorPlan || view.ViewType == ViewType.CeilingPlan || view.ViewType == ViewType.EngineeringPlan)
            {
               IList<HorizontalCurveEndpoint> curveEndpoints = alignment.GetDisplayedHorizontalCurveEndpoints();
               using (TransactionGroup tg = new TransactionGroup(document, "Create horizontal curvature changes labels"))
               {
                  tg.Start();

                  double previousStation = alignment.DisplayedStartStation;
                  foreach (var curveEndpoint in curveEndpoints)
                  {
                     using (Transaction t = new Transaction(document, "Create one horizontal curvature change label"))
                     {
                        double thisStation = curveEndpoint.Station;
                        // skip placing curvature labels at the start and end points of the alignment
                        if (MathComparisonUtils.IsAlmostEqual((alignment.DisplayedStartStation), thisStation) ||
                        MathComparisonUtils.IsAlmostEqual((alignment.DisplayedEndStation), thisStation))
                           continue;

                        t.Start();

                        AlignmentStationLabelOptions options = new AlignmentStationLabelOptions(thisStation);
                        options.HasLeader = false;
                        options.TypeId = curvatureLabelType.Id;

                        AlignmentStationLabel label = AlignmentStationLabel.Create(alignment, view, options);

                        // regeneration is necessary before the label's positional properties (such as Origin)L can be properly evaluated
                        document.Regenerate();

                        // set the shoulder and end to coincide, creating a leader pointing along the view's up direction
                        SpotDimension dim = label.Element as SpotDimension;

                        XYZ leaderDirection = view.UpDirection;
                        // compute the distance to the previous label
                        // if the previous label is too close, flip the placement direction
                        {
                           var dimBBox = dim.get_BoundingBox(view);
                           double dimOffset = Math.Abs(dimBBox.Max.X - dimBBox.Min.X);
                           if (MathComparisonUtils.IsGreaterThanOrAlmostEqual(dimOffset, thisStation - previousStation))
                              leaderDirection = leaderDirection.Negate();
                        }

                        dim.HasLeader = true;
                        dim.LeaderHasShoulder = true;
                        dim.LeaderShoulderPosition = dim.Origin +
                           leaderDirection * UnitUtils.ConvertToInternalUnits(labelTextOffset, UnitTypeId.StationingMeters) * view.Scale;
                        dim.LeaderEndPosition = dim.LeaderShoulderPosition;
                        dim.TextPosition = dim.LeaderShoulderPosition;

                        previousStation = thisStation;
                        t.Commit();
                     }
                  }
                  tg.Assimilate();
               }
            }

            return Result.Succeeded;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
      #endregion
   }

   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// For a selected alignment, displays its properties as well as the properties of its attached station labels.
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
   public class ShowPropertiesCmd : IExternalCommand
   {
      #region AlignmentPropertiesUtilities
      /// <summary>
      /// Formats a station value to a string, using the stationing settings in the document.
      /// </summary>
      /// <param name="station">The station value.</param>
      /// <param name="doc">The document for which to format the value.</param>
      /// <returns></returns>
      private string GetStationFormattedValue(double station, Document doc)
      {
         FormatOptions options = doc.GetUnits().GetFormatOptions(SpecTypeId.Stationing);
         return UnitFormatUtils.Format(doc.GetUnits(), SpecTypeId.Stationing, station, false);
      }

      /// <summary>
      /// Returns a summary of several important alignment element properties.
      /// </summary>
      /// <param name="alignment"></param>
      /// <returns></returns>
      private string GetAlignmentProperties(Alignment alignment)
      {
         Document doc = alignment.Element.Document;
         return string.Format("Alignment Properties:\r\n" +
            "Name:{0}, " +
            "Description:{1}, " +
            "Geometric Definition Range: ({2},{3}), " +
            "Displayed Range: ({4},{5})," +
            "GUID:{6}\r\n\r\n",
            alignment.Name,
            alignment.Description,
            GetStationFormattedValue(alignment.StartStation, doc),
            GetStationFormattedValue(alignment.EndStation, doc),
            GetStationFormattedValue(alignment.DisplayedStartStation, doc),
            GetStationFormattedValue(alignment.DisplayedEndStation, doc),
            alignment.GUID.ToString());
      }

      /// <summary>
      /// Returns the alignment's horizontal curve types ordered from start to end.
      /// </summary>
      /// <param name="alignment"></param>
      /// <returns></returns>
      private string GetAlignmentHorizontalCurveTypes(Alignment alignment)
      {
         Document doc = alignment.Element.Document;
         string curveTypesSummary = "Alignment Horizontal Curves, from start to end:\r\n";
         IList<HorizontalCurveEndpoint> curveEndpoints = alignment.GetDisplayedHorizontalCurveEndpoints();
         foreach (var curveEndpoint in curveEndpoints)
         {
            HorizontalCurveType curveType = curveEndpoint.PreviousCurveType;
            if (curveType != HorizontalCurveType.Unknown) // skip the Unknown curve at the start
               curveTypesSummary += curveType.ToString() + "; ";
         }
         curveTypesSummary += "\r\n\r\n";
         return curveTypesSummary;
      }

      /// <summary>
      /// Returns a summary of the station, type and category of all station labels associated with the alignment.
      /// </summary>
      /// <param name="alignment">The alignment for which to return the summary.</param>
      /// <returns></returns>
      private string GetAlignmentStationLabelsProperties(Alignment alignment)
      {
         string properties = "Alignment Station Label Properties:\r\n";
         Document doc = alignment.Element.Document;
         ICollection<AlignmentStationLabel> labels = AlignmentStationLabel.GetAlignmentStationLabels(alignment);
         foreach (AlignmentStationLabel label in labels)
         {
            ElementType alignmentLabelType = doc.GetElement(label.Element.GetTypeId()) as ElementType;
            properties += string.Format("Type:{0}, Category: {1}, Station: {2}\r\n",
               alignmentLabelType.Name,
               label.Element.Category.Name,
               GetStationFormattedValue(label.Station, doc));
         }

         return properties;
      }
      #endregion

      #region InterfaceImplementation
      /// <summary>
      /// Implement this method as an external command for Revit.
      /// </summary>
      /// <param name="commandData">An object that is passed to the external application 
      /// which contains data related to the command, 
      /// such as the application object and active view.</param>
      /// <param name="message">A message that can be set by the external application 
      /// which will be displayed if a failure or cancellation is returned by 
      /// the external command.</param>
      /// <param name="elements">A set of elements to which the external application 
      /// can add elements that are to be highlighted in case of failure or cancellation.</param>
      /// <returns>Return the status of the external command. 
      /// A result of Succeeded means that the API external method functioned as expected. 
      /// Cancelled can be used to signify that the user cancelled the external operation 
      /// at some point. Failure should be returned if the application is unable to proceed with 
      /// the operation.</returns>
      public virtual Result Execute(ExternalCommandData commandData
         , ref string message, ElementSet elements)
      {
         try
         {
            Document document = commandData.Application.ActiveUIDocument.Document;
            Autodesk.Revit.DB.View view = commandData.View;

            ElementId alignmentId = AlignmentSelectionFilter.SelectAlignment(document);
            Alignment alignment = Alignment.Get(document.GetElement(alignmentId));

            TaskDialog td = new TaskDialog("Alignment and Station Label Properties");
            td.MainContent = 
               GetAlignmentProperties(alignment) +
               GetAlignmentHorizontalCurveTypes(alignment) + 
               GetAlignmentStationLabelsProperties(alignment);
            td.Show();

            return Result.Succeeded;
         }
         catch (Exception ex)
         {
            message = ex.Message;
            return Result.Failed;
         }
      }
      #endregion
   }
}

