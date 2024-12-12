//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Xml;
using System.Xml.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;

namespace Revit.SDK.Samples.RoutingPreferenceTools.CS
{

    /// <summary>
    /// Queries all routing preferences and reports potential problems in the form of an XDocument.
    /// </summary>
    internal class Analyzer
    {
        #region Data
        private Autodesk.Revit.DB.Document m_document;
        private Autodesk.Revit.DB.RoutingPreferenceManager m_routingPreferenceManager;
        private double m_mepSize;
        #endregion

        #region Public interface
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="routingPreferenceManager"></param>
        /// <param name="document"></param>
        public Analyzer(RoutingPreferenceManager routingPreferenceManager, Document document)
        {
            m_routingPreferenceManager = routingPreferenceManager;
            m_document = document;
            m_mepSize = 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="routingPreferenceManager"></param>
        /// <param name="mepSize"></param>
        /// <param name="document"></param>
        public Analyzer(RoutingPreferenceManager routingPreferenceManager, double mepSize, Document document)
        {
            m_routingPreferenceManager = routingPreferenceManager;
            m_document = document;


            m_mepSize = Convert.ConvertValueToFeet(mepSize, m_document);
        }

        /// <summary>
        /// Get specific size query
        /// </summary>
        /// <returns></returns>
        public XDocument GetSpecificSizeQuery()
        {

            XDocument xReportDoc = new XDocument();
            XElement xroot = new XElement(XName.Get("RoutingPreferenceAnalysisSizeQuery"));
            xroot.Add(GetHeaderInformation());


            foreach (PartIdInfo partId in GetPreferredFittingsAndSegments())
            {
                xroot.Add(partId.GetXml(m_document));
            }

            xReportDoc.Add(xroot);
            return xReportDoc;

        }


        /// <summary>
        /// Get all segments from a the currently selected pipe type, get each size from each segment,
        /// collect, sort, and return.
        /// </summary>
        public static List<double> GetAvailableSegmentSizes(RoutingPreferenceManager routingPreferenceManager, Autodesk.Revit.DB.Document document)
        {

            System.Collections.Generic.HashSet<double> sizes = new HashSet<double>();
            int segmentCount = routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Segments);
            for (int index = 0; index != segmentCount; ++index)
            {
                RoutingPreferenceRule segmentRule = routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Segments, index);

                Segment segment = document.GetElement(segmentRule.MEPPartId) as Segment;
                foreach (MEPSize size in segment.GetSizes())
                {
                    sizes.Add(size.NominalDiameter);
                }
            }
            List<double> sizesSorted = sizes.ToList();
            sizesSorted.Sort();
            return sizesSorted;
        }


        /// <summary>
        /// Returns XML data for a variety of potential routing-preference problems.
        /// </summary>
        /// <returns></returns>
        public XDocument GetWarnings()
        {
            XDocument xReportDoc = new XDocument();
            XElement xroot = new XElement(XName.Get("RoutingPreferenceAnalysis"));
            xroot.Add(GetHeaderInformation());
            XElement xWarnings = new XElement(XName.Get("Warnings"));

            //None-range-warnings         
            foreach (RoutingPreferenceRuleGroupType groupType in Enum.GetValues(typeof(RoutingPreferenceRuleGroupType)).Cast<RoutingPreferenceRuleGroupType>())
            {
                if (IsRuleSetToRangeNone(m_routingPreferenceManager, groupType, 0))
                {

                    XElement xNoRangeSet = new XElement(XName.Get("NoRangeSet"));
                    xNoRangeSet.Add(new XAttribute(XName.Get("groupType"), groupType.ToString()));

                    if (IsGroupSetToRangeNone(m_routingPreferenceManager, groupType))
                    {
                        xNoRangeSet.Add(new XAttribute(XName.Get("rule"), "allRules"));
                    }
                    else
                    {
                        xNoRangeSet.Add(new XAttribute(XName.Get("rule"), "firstRule"));
                    }
                    xWarnings.Add(xNoRangeSet);
                }
            }

            //tee/tap warnings

            if (!IsPreferredJunctionTypeValid(m_routingPreferenceManager))
            {
                XElement xJunctionFittingsNotDefined = new XElement(XName.Get("FittingsNotDefinedForPreferredJunction"));
                xWarnings.Add(xJunctionFittingsNotDefined);
            }

            //size range warnings for elbow, Junction, and Cross

            XElement xSegmentElbowWarning = GetSegmentRangeNotCoveredWarning(m_routingPreferenceManager, RoutingPreferenceRuleGroupType.Elbows);
            if (xSegmentElbowWarning != null)
                xWarnings.Add(xSegmentElbowWarning);
            XElement xSegmentTeeWarning = GetSegmentRangeNotCoveredWarning(m_routingPreferenceManager, RoutingPreferenceRuleGroupType.Junctions);
            if (xSegmentTeeWarning != null)
                xWarnings.Add(xSegmentTeeWarning);
            XElement xSegmentCrossWarning = GetSegmentRangeNotCoveredWarning(m_routingPreferenceManager, RoutingPreferenceRuleGroupType.Crosses);
            if (xSegmentCrossWarning != null)
                xWarnings.Add(xSegmentCrossWarning);

            xroot.Add(xWarnings);
            xReportDoc.Add(xroot);
            return xReportDoc;
        }
        #endregion

        #region Analyis helper methods

        /// <summary>
        /// Get basic information about the PipeType
        /// </summary>
        private XElement GetHeaderInformation()
        {
            XElement xHeader = new XElement(XName.Get("PipeType"));
            string pipeTypeName = m_document.GetElement(m_routingPreferenceManager.OwnerId).Name;

            xHeader.Add(new XAttribute(XName.Get("name"), pipeTypeName));
            xHeader.Add(new XAttribute(XName.Get("elementId"), m_routingPreferenceManager.OwnerId.ToString()));
            return xHeader;
        }

        /// <summary>
        /// Checks to see if any segments in the routing preference manager have sizes that cannot be fitted with fittings defined in a rule group type, such as "Elbow."
        /// For example, if a segment rule defines a segment be used from sizes 2" to 12", and there are three elbows rules defined to be used from ranges
        /// 2"-4", 4"-7", and 9"-14", this method will return warning information specifying the sizes (8", 8.5", etc...) not covered by elbow fittings.
        /// </summary>
        private XElement GetSegmentRangeNotCoveredWarning(RoutingPreferenceManager routingPreferenceManager, RoutingPreferenceRuleGroupType groupType)
        {
            for (int index = 0; index != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Segments); ++index)
            {
                RoutingPreferenceRule rule = routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Segments, index);
                if (rule.MEPPartId == ElementId.InvalidElementId)
                    continue;

                if (rule.NumberOfCriteria == 0)  //double check all/none
                    continue;

                PrimarySizeCriterion psc = rule.GetCriterion(0) as PrimarySizeCriterion;

                PipeSegment segment = m_document.GetElement(rule.MEPPartId) as PipeSegment;
                List<double> sizesNotCovered = new List<double>();
                bool isCovered = CheckSegmentForValidCoverage(routingPreferenceManager, psc.MinimumSize, psc.MaximumSize, rule.MEPPartId, groupType, sizesNotCovered);
                if (!isCovered)
                {
                    XElement xSegmentNotCovered = new XElement(XName.Get("SegmentRangeNotCovered"));
                    xSegmentNotCovered.Add(new XAttribute(XName.Get("name"), segment.Name));
                    StringBuilder sBuilder = new StringBuilder();

                    foreach (double size in sizesNotCovered)
                    {
                        double roundedSize = Convert.ConvertValueDocumentUnits(size, m_document);

                        sBuilder.Append(roundedSize.ToString() + " ");
                    }
                    sBuilder.Remove(sBuilder.Length - 1, 1);
                    xSegmentNotCovered.Add(new XAttribute(XName.Get("sizes"), sBuilder.ToString()));
                    xSegmentNotCovered.Add(new XAttribute(XName.Get("unit"), m_document.GetUnits().GetFormatOptions(UnitType.UT_PipeSize).DisplayUnits.ToString()));
                    xSegmentNotCovered.Add(new XAttribute(XName.Get("groupType"), groupType.ToString()));


                    return xSegmentNotCovered;
                }
            }
            return null;
        }


        private bool CheckSegmentForValidCoverage(RoutingPreferenceManager routingPreferenceManager, double lowerBound, double upperBound, ElementId segmentId, RoutingPreferenceRuleGroupType groupType, List<double> sizesNotCovered)
        {

            bool retval = true;
            if (segmentId == ElementId.InvalidElementId)
                throw new Exception("Invalid segment ElementId");

            PipeSegment segment = this.m_document.GetElement(segmentId) as PipeSegment;
            foreach (MEPSize size in segment.GetSizes())
            {
                //skip sizes outside of rp bounds
                if (size.NominalDiameter < lowerBound)
                    continue;
                if (size.NominalDiameter > upperBound)
                    break;

                RoutingConditions conditions = new RoutingConditions(RoutingPreferenceErrorLevel.None);
                conditions.AppendCondition(new RoutingCondition(size.NominalDiameter));
                ElementId foundFitting = routingPreferenceManager.GetMEPPartId(groupType, conditions);
                if (foundFitting == ElementId.InvalidElementId)
                {
                    sizesNotCovered.Add(size.NominalDiameter);
                    retval = false;
                }
            }
            return retval;
        }

        private bool IsRuleSetToRangeNone(RoutingPreferenceManager routingPreferenceManager, RoutingPreferenceRuleGroupType groupType, int index)
        {

            if (routingPreferenceManager.GetNumberOfRules(groupType) == 0)
            {
                return false;
            }

            RoutingPreferenceRule rule = routingPreferenceManager.GetRule(groupType, index);
            if (rule.NumberOfCriteria == 0)
            {
                return false;
            }

            PrimarySizeCriterion psc = rule.GetCriterion(0) as PrimarySizeCriterion;
            if (psc.IsEqual(PrimarySizeCriterion.None()))
            {
                return true;
            }
            else
                return false;
        }

        private bool IsGroupSetToRangeNone(RoutingPreferenceManager routingPreferenceManager, RoutingPreferenceRuleGroupType groupType)
        {
            bool retval = true;

            if (routingPreferenceManager.GetNumberOfRules(groupType) == 0)
            {
                return false;
            }
            for (int index = 0; index != routingPreferenceManager.GetNumberOfRules(groupType); ++index)
            {
                if (!(IsRuleSetToRangeNone(routingPreferenceManager, groupType, index)))
                    retval = false;
            }

            return retval;

        }
        /// <summary>
        /// Check to see if the routing preferences specify a preferred junction type but do not have any
        /// rules with valid fittings for that type (e.g, "Tee" is the preferred junction type, but only "Tap" fittings
        /// are specified in junction rules.)
        /// </summary>
        /// <param name="routingPreferenceManager"></param>
        /// <returns></returns>
        private bool IsPreferredJunctionTypeValid(RoutingPreferenceManager routingPreferenceManager)
        {
            PreferredJunctionType preferredJunctionType = routingPreferenceManager.PreferredJunctionType;

            if (routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Junctions) == 0)
                return false;

            bool teeDefined = false;
            bool tapDefined = false;
            for (int index = 0; index != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Junctions); ++index)
            {
                RoutingPreferenceRule rule = routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Junctions, index);
                if (rule.MEPPartId == ElementId.InvalidElementId)
                    continue;

                FamilySymbol familySymbol = this.m_document.GetElement(rule.MEPPartId) as FamilySymbol;

                Parameter paramPartType = familySymbol.Family.get_Parameter(BuiltInParameter.FAMILY_CONTENT_PART_TYPE);
                if (paramPartType == null)
                    throw new Exception("Null partType parameter.");

                PartType partType = (PartType)paramPartType.AsInteger();

                if ((partType == PartType.Tee))
                    teeDefined = true;
                else if (
                   (partType == PartType.TapAdjustable) ||
                   (partType == PartType.TapPerpendicular) ||
                   (partType == PartType.SpudPerpendicular) ||
                   (partType == PartType.SpudAdjustable)
                   )
                    tapDefined = true;
            }
            if ((preferredJunctionType == PreferredJunctionType.Tap) && !tapDefined)
                return false;
            if ((preferredJunctionType == PreferredJunctionType.Tee) && !teeDefined)
                return false;

            return true;
        }
        private string GetFittingName(ElementId id)
        {
            if (id == ElementId.InvalidElementId)
                throw new Exception("Invalid ElementId");
            FamilySymbol symbol = m_document.GetElement(id) as FamilySymbol;
            return symbol.Family.Name + " " + symbol.Name;
        }
        private string GetSegmentName(ElementId id)
        {
            if (id == ElementId.InvalidElementId)
                throw new Exception("Invalid ElementId");
            PipeSegment segment = m_document.GetElement(id) as PipeSegment;
            return segment.Name;
        }



        /// <summary>
        ///Using routing preferences, display found segment and fitting info from the size and pipe type specified in the dialog.
        /// </summary>
        private List<PartIdInfo> GetPreferredFittingsAndSegments()
        {
            List<PartIdInfo> partIdInfoList = new List<PartIdInfo>();

            RoutingConditions conditions = new RoutingConditions(RoutingPreferenceErrorLevel.None);


            conditions.AppendCondition(new RoutingCondition(m_mepSize));
            foreach (RoutingPreferenceRuleGroupType groupType in Enum.GetValues(typeof(RoutingPreferenceRuleGroupType)))
            {
                if (groupType == RoutingPreferenceRuleGroupType.Undefined)
                    continue;

                IList<ElementId> preferredTypes = new List<ElementId>();
                ElementId preferredType = m_routingPreferenceManager.GetMEPPartId(groupType, conditions);
                //GetMEPPartId is the main "query" method of the
                //routing preferences API that evaluates conditions and criteria and returns segment and fitting elementIds that meet
                //those criteria.

                if (groupType != RoutingPreferenceRuleGroupType.Segments)
                {
                    preferredTypes.Add(preferredType);
                }
                else  //Get all segments that support a given size, not just the first segment.
                {
                    preferredTypes = m_routingPreferenceManager.GetSharedSizes(m_mepSize, ConnectorProfileType.Round);
                }
                partIdInfoList.Add(new PartIdInfo(groupType, preferredTypes));  //Collect a PartIdInfo object for each group type.
            }

            return partIdInfoList;
        }
        #endregion
    }
}
