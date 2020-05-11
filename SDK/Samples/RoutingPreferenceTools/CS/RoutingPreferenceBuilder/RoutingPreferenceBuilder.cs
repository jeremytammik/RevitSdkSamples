//
// (C) Copyright 2003-2019 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Plumbing;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Diagnostics;


namespace Revit.SDK.Samples.RoutingPreferenceTools.CS
{
    /// <summary>
    /// Class to read and write XML and routing preference data
    /// </summary>
    public class RoutingPreferenceBuilder
    {
        #region Data
        private IEnumerable<Segment> m_segments;
        private IEnumerable<FamilySymbol> m_fittings;
        private IEnumerable<Material> m_materials;
        private IEnumerable<PipeScheduleType> m_pipeSchedules;
        private IEnumerable<PipeType> m_pipeTypes;
        private Autodesk.Revit.DB.Document m_document;
        #endregion

        #region Public interface
        /// <summary>
        /// Create an instance of the class and initialize lists of all segments, fittings, materials, schedules, and pipe types in the document.
        /// </summary>
        public RoutingPreferenceBuilder(Document document)
        {
            m_document = document;
            m_segments = GetAllPipeSegments(m_document);
            m_fittings = GetAllFittings(m_document);
            m_materials = GetAllMaterials(m_document);
            m_pipeSchedules = GetAllPipeScheduleTypes(m_document);
            m_pipeTypes = GetAllPipeTypes(m_document);

        }
        /// <summary>
        /// Reads data from an Xml source and loads pipe fitting families, creates segments, sizes, schedules, and routing preference rules from the xml data.
        /// </summary>
        /// <param name="xDoc">The Xml data source to read from</param>
        public void ParseAllPipingPoliciesFromXml(XDocument xDoc)
        {
            if (m_pipeTypes.Count() == 0)
                throw new RoutingPreferenceDataException("No pipe pipes defined in this project.  At least one must be defined.");


            FormatOptions formatOptionPipeSize = m_document.GetUnits().GetFormatOptions(SpecTypeId.PipeSize);

            string docPipeSizeUnit = formatOptionPipeSize.GetUnitTypeId().TypeId;
            string xmlPipeSizeUnit = xDoc.Root.Attribute("pipeSizeUnits").Value;
            if (docPipeSizeUnit != xmlPipeSizeUnit)
                throw new RoutingPreferenceDataException("Units from XML do not match current pipe size units.");


            FormatOptions formatOptionRoughness = m_document.GetUnits().GetFormatOptions(SpecTypeId.PipingRoughness);

            string docRoughnessUnit = formatOptionRoughness.GetUnitTypeId().TypeId;
            string xmlRoughnessUnit = xDoc.Root.Attribute("pipeRoughnessUnits").Value;
            if (docRoughnessUnit != xmlRoughnessUnit)
                throw new RoutingPreferenceDataException("Units from XML do not match current pipe roughness units.");

            Transaction loadFamilies = new Transaction(m_document, "Load Families");
            loadFamilies.Start();
            IEnumerable<XElement> families = xDoc.Root.Elements("Family");
            FindFolderUtility findFolderUtility = new FindFolderUtility(m_document.Application);

            foreach (XElement xfamily in families)
                try
                {
                    ParseFamilyFromXml(xfamily, findFolderUtility);  //Load families.
                }
                catch (Exception ex)
                {
                    loadFamilies.RollBack();
                    throw ex;
                }
            loadFamilies.Commit();

            Transaction addPipeTypes = new Transaction(m_document, "Add PipeTypes");
            addPipeTypes.Start();
            IEnumerable<XElement> pipeTypes = xDoc.Root.Elements("PipeType");
            foreach (XElement xpipeType in pipeTypes)
                try
                {
                    ParsePipeTypeFromXml(xpipeType);  //Define new pipe types.
                }
                catch (Exception ex)
                {
                    addPipeTypes.RollBack();
                    throw ex;
                }
            addPipeTypes.Commit();

            Transaction addPipeSchedules = new Transaction(m_document, "Add Pipe Schedule Types");
            addPipeSchedules.Start();
            IEnumerable<XElement> pipeScheduleTypes = xDoc.Root.Elements("PipeScheduleType");
            foreach (XElement xpipeScheduleType in pipeScheduleTypes)
                try
                {
                    ParsePipeScheduleTypeFromXml(xpipeScheduleType);  //Define new pipe schedule types.
                }
                catch (Exception ex)
                {
                    addPipeSchedules.RollBack();
                    throw ex;
                }
            addPipeSchedules.Commit();

            //The code above have added some new pipe types, schedules, or fittings, so update the lists of all of these.
            UpdatePipeTypesList();
            UpdatePipeTypeSchedulesList();
            UpdateFittingsList();

            Transaction addPipeSegments = new Transaction(m_document, "Add Pipe Segments");
            addPipeSchedules.Start();
            IEnumerable<XElement> pipeSegments = xDoc.Root.Elements("PipeSegment");  //Define new segments.
            foreach (XElement xpipeSegment in pipeSegments)
                try
                {
                    ParsePipeSegmentFromXML(xpipeSegment);
                }
                catch (Exception ex)
                {
                    addPipeSchedules.RollBack();
                    throw ex;
                }
            addPipeSchedules.Commit();

            UpdateSegmentsList();  //More segments may have been created, so update the segment list.


            //Now that all of the various types that routing preferences use have been created or loaded, add all the routing preferences.
            Transaction addRoutingPreferences = new Transaction(m_document, "Add Routing Preferences");
            addRoutingPreferences.Start();
            IEnumerable<XElement> routingPreferenceManagers = xDoc.Root.Elements("RoutingPreferenceManager");
            foreach (XElement xroutingPreferenceManager in routingPreferenceManagers)
                try
                {
                    ParseRoutingPreferenceManagerFromXML(xroutingPreferenceManager);
                }
                catch (Exception ex)
                {
                    addRoutingPreferences.RollBack();
                    throw ex;
                }
            addRoutingPreferences.Commit();

        }

        /// <summary>
      /// Reads pipe fitting family, segment, size, schedule, and routing preference data from a document and summarizes it in Xml.
      /// </summary>
      /// <returns>An XDocument containing an Xml summary of routing preference information</returns>
      public XDocument CreateXmlFromAllPipingPolicies(ref bool pathsNotFound)
      {
         //To export the full path name of all .rfa family files, use the FindFolderUtility class.
         FindFolderUtility findFolderUtility = new FindFolderUtility(m_document.Application);

         XDocument routingPreferenceBuilderDoc = new XDocument();
         XElement xroot = new XElement(XName.Get("RoutingPreferenceBuilder"));

         FormatOptions formatOptionPipeSize = m_document.GetUnits().GetFormatOptions(SpecTypeId.PipeSize);
         string unitStringPipeSize = formatOptionPipeSize.GetUnitTypeId().TypeId;
         xroot.Add(new XAttribute(XName.Get("pipeSizeUnits"), unitStringPipeSize));

         FormatOptions formatOptionRoughness = m_document.GetUnits().GetFormatOptions(SpecTypeId.PipingRoughness);
         string unitStringRoughness = formatOptionRoughness.GetUnitTypeId().TypeId;
         xroot.Add(new XAttribute(XName.Get("pipeRoughnessUnits"), unitStringRoughness));

         foreach (FamilySymbol familySymbol in this.m_fittings)
         {
            xroot.Add(CreateXmlFromFamily(familySymbol, findFolderUtility, ref pathsNotFound));
         }

         foreach (PipeType pipeType in m_pipeTypes)
         {
            xroot.Add(CreateXmlFromPipeType(pipeType));
         }

         foreach (PipeScheduleType pipeScheduleType in m_pipeSchedules)
         {
            xroot.Add(CreateXmlFromPipeScheduleType(pipeScheduleType));
         }

         foreach (PipeSegment pipeSegment in m_segments)
         {
            xroot.Add(CreateXmlFromPipeSegment(pipeSegment));
         }

         foreach (PipeType pipeType in m_pipeTypes)
         {
            xroot.Add(CreateXmlFromRoutingPreferenceManager(pipeType.RoutingPreferenceManager));
         }

         routingPreferenceBuilderDoc.Add(xroot);
         return routingPreferenceBuilderDoc;
      }
      #endregion

      #region XML parsing and generation
       /// <summary>
       /// Load a family from xml
       /// </summary>
       /// <param name="familyXElement"></param>
       /// <param name="findFolderUtility"></param>
      private void ParseFamilyFromXml(XElement familyXElement, FindFolderUtility findFolderUtility)
      {
       
         XAttribute xafilename = familyXElement.Attribute(XName.Get("filename"));
         string familyPath = xafilename.Value;
         if (!System.IO.File.Exists(familyPath))
         {
            string filename = System.IO.Path.GetFileName(familyPath);
            familyPath = findFolderUtility.FindFileFolder(filename);
            if (!System.IO.File.Exists(familyPath))
               throw new RoutingPreferenceDataException("Cannot find family file: " + xafilename.Value);
         }


         if (string.Compare(System.IO.Path.GetExtension(familyPath), ".rfa", true) != 0)
            throw new RoutingPreferenceDataException(familyPath + " is not a family file.");

         try
         {
            if (!m_document.LoadFamily(familyPath))
               return;  //returns false if already loaded.
         }
         catch (System.Exception ex)
         {
            throw new RoutingPreferenceDataException("Cannot load family: " + xafilename.Value + ": " + ex.ToString());
         }

      }
       /// <summary>
       /// Create xml from a family
       /// </summary>
       /// <param name="pipeFitting"></param>
       /// <param name="findFolderUtility"></param>
       /// <param name="pathNotFound"></param>
       /// <returns></returns>
      private static XElement CreateXmlFromFamily(FamilySymbol pipeFitting, FindFolderUtility findFolderUtility, ref bool pathNotFound)
      {
         //Try to find the path of the .rfa file.
         string path = findFolderUtility.FindFileFolder(pipeFitting.Family.Name + ".rfa");
         string pathToWrite;
         if (path == "")
         {
            pathNotFound = true;
            pathToWrite = pipeFitting.Family.Name + ".rfa";
         }
         else
            pathToWrite = path;

         XElement xFamilySymbol = new XElement(XName.Get("Family"));
         xFamilySymbol.Add(new XAttribute(XName.Get("filename"), pathToWrite));
         return xFamilySymbol;
      }

       /// <summary>
       /// Greate a PipeType from xml
       /// </summary>
       /// <param name="pipetypeXElement"></param>
      private void ParsePipeTypeFromXml(XElement pipetypeXElement)
      {
         XAttribute xaName = pipetypeXElement.Attribute(XName.Get("name"));

         ElementId pipeTypeId = GetPipeTypeByName(xaName.Value);

         if (pipeTypeId == ElementId.InvalidElementId)  //If the pipe type does not exist, create it.
         {
            PipeType newPipeType = m_pipeTypes.First().Duplicate(xaName.Value) as PipeType;
            ClearRoutingPreferenceRules(newPipeType);
         }

      }

       /// <summary>
       /// Clear all routing preferences in a PipeType
       /// </summary>
       /// <param name="pipeType"></param>
      private static void ClearRoutingPreferenceRules(PipeType pipeType)
      {
         foreach ( RoutingPreferenceRuleGroupType group in System.Enum.GetValues(typeof(RoutingPreferenceRuleGroupType)))
         {
            int ruleCount = pipeType.RoutingPreferenceManager.GetNumberOfRules(group);
            for (int index = 0; index != ruleCount; ++index)
            {
               pipeType.RoutingPreferenceManager.RemoveRule(group, 0);
            }
         }
      }

       /// <summary>
       /// Create Xml from a PipeType
       /// </summary>
       /// <param name="pipeType"></param>
       /// <returns></returns>
      private static XElement CreateXmlFromPipeType(PipeType pipeType)
      {
         XElement xPipeType = new XElement(XName.Get("PipeType"));
         xPipeType.Add(new XAttribute(XName.Get("name"), pipeType.Name));
         return xPipeType;
      }

      private void ParsePipeScheduleTypeFromXml(XElement pipeScheduleTypeXElement)
      {
         XAttribute xaName = pipeScheduleTypeXElement.Attribute(XName.Get("name"));
         ElementId pipeScheduleTypeId = GetPipeScheduleTypeByName(xaName.Value);
         if (pipeScheduleTypeId == ElementId.InvalidElementId)  //If the pipe schedule type does not exist, create it.
            m_pipeSchedules.First().Duplicate(xaName.Value);
      }

       /// <summary>
       /// Create Xml from a PipeScheduleType
       /// </summary>
       /// <param name="pipeScheduleType"></param>
       /// <returns></returns>
      private static XElement CreateXmlFromPipeScheduleType(PipeScheduleType pipeScheduleType)
      {
         XElement xPipeSchedule = new XElement(XName.Get("PipeScheduleType"));
         xPipeSchedule.Add(new XAttribute(XName.Get("name"), pipeScheduleType.Name));
         return xPipeSchedule;
      }

       /// <summary>
       /// Create a PipeSegment from XML
       /// </summary>
       /// <param name="segmentXElement"></param>
      private void ParsePipeSegmentFromXML(XElement segmentXElement)
      {
         XAttribute xaMaterial = segmentXElement.Attribute(XName.Get("materialName"));
         XAttribute xaSchedule = segmentXElement.Attribute(XName.Get("pipeScheduleTypeName"));
         XAttribute xaRoughness = segmentXElement.Attribute(XName.Get("roughness"));
        
         ElementId materialId = GetMaterialByName(xaMaterial.Value);  //There is nothing in the xml schema for creating new materials -- any material specified must already exist in the document.
         if (materialId == ElementId.InvalidElementId)
         {
            throw new RoutingPreferenceDataException("Cannot find Material: " + xaMaterial.Value + " in: " + segmentXElement.ToString());  
         }
         ElementId scheduleId = GetPipeScheduleTypeByName(xaSchedule.Value);

         double roughness;
         bool r1 = double.TryParse(xaRoughness.Value, out roughness);

         if (!r1)
            throw new RoutingPreferenceDataException("Invalid roughness value: " + xaRoughness.Value + " in: " + segmentXElement.ToString());

         if (roughness <= 0)
            throw new RoutingPreferenceDataException("Invalid roughness value: " + xaRoughness.Value + " in: " + segmentXElement.ToString());

         if (scheduleId == ElementId.InvalidElementId)
         {
            throw new RoutingPreferenceDataException("Cannot find Schedule: " + xaSchedule.Value + " in: " + segmentXElement.ToString());  //we will not create new schedules.
         }

         ElementId existingPipeSegmentId = GetSegmentByIds(materialId, scheduleId);
         if (existingPipeSegmentId != ElementId.InvalidElementId)
            return;   //Segment found, no need to create.

         ICollection<MEPSize> sizes = new List<MEPSize>();
         foreach (XNode sizeNode in segmentXElement.Nodes())
         {
            if (sizeNode is XElement)
            {
               MEPSize newSize = ParseMEPSizeFromXml(sizeNode as XElement, m_document);
               sizes.Add(newSize);
            }
         }
         PipeSegment pipeSegment = PipeSegment.Create(m_document, materialId, scheduleId, sizes);
         pipeSegment.Roughness =  Convert.ConvertValueToFeet(roughness, m_document);

         return;
      }

       /// <summary>
       /// Create Xml from a PipeSegment
       /// </summary>
       /// <param name="pipeSegment"></param>
       /// <returns></returns>
      private XElement CreateXmlFromPipeSegment(PipeSegment pipeSegment)
      {
         XElement xPipeSegment = new XElement(XName.Get("PipeSegment"));
         string segmentName = pipeSegment.Name;

         xPipeSegment.Add(new XAttribute(XName.Get("pipeScheduleTypeName"), GetPipeScheduleTypeNamebyId(pipeSegment.ScheduleTypeId)));
         xPipeSegment.Add(new XAttribute(XName.Get("materialName"), GetMaterialNameById(pipeSegment.MaterialId)));

         double roughnessInDocumentUnits = Convert.ConvertValueDocumentUnits(pipeSegment.Roughness, m_document);
         xPipeSegment.Add(new XAttribute(XName.Get("roughness"), roughnessInDocumentUnits.ToString("r") ));

         foreach (MEPSize size in pipeSegment.GetSizes())
            xPipeSegment.Add(CreateXmlFromMEPSize(size, m_document));

         return xPipeSegment;

      }

       /// <summary>
       /// Create an MEPSize from Xml
       /// </summary>
       /// <param name="sizeXElement"></param>
       /// <param name="document"></param>
       /// <returns></returns>
      private static MEPSize ParseMEPSizeFromXml(XElement sizeXElement, Autodesk.Revit.DB.Document document)
      {
         XAttribute xaNominal = sizeXElement.Attribute(XName.Get("nominalDiameter"));
         XAttribute xaInner = sizeXElement.Attribute(XName.Get("innerDiameter"));
         XAttribute xaOuter = sizeXElement.Attribute(XName.Get("outerDiameter"));
         XAttribute xaUsedInSizeLists = sizeXElement.Attribute(XName.Get("usedInSizeLists"));
         XAttribute xaUsedInSizing = sizeXElement.Attribute(XName.Get("usedInSizing"));
       
         double nominal, inner, outer;
         bool usedInSizeLists, usedInSizing;
         bool r1 = double.TryParse(xaNominal.Value, out nominal);
         bool r2 = double.TryParse(xaInner.Value, out inner);
         bool r3 = double.TryParse(xaOuter.Value, out outer);
         bool r4 = bool.TryParse(xaUsedInSizeLists.Value, out usedInSizeLists);
         bool r5 = bool.TryParse(xaUsedInSizing.Value, out usedInSizing);

         if (!r1 || !r2 || !r3 || !r4 || !r5)
            throw new RoutingPreferenceDataException("Cannot parse MEPSize attributes:" + xaNominal.Value + ", " + xaInner.Value + ", " + xaOuter.Value + ", " + xaUsedInSizeLists.Value + ", " + xaUsedInSizing.Value);

         MEPSize newSize = null;

         try
         {

            newSize = new MEPSize(Convert.ConvertValueToFeet(nominal, document), Convert.ConvertValueToFeet(inner, document), Convert.ConvertValueToFeet(outer, document), usedInSizeLists, usedInSizing);
         }

         catch (Exception)
         {
            throw new RoutingPreferenceDataException("Invalid MEPSize values: " + nominal.ToString() + ", " + inner.ToString() + ", " + outer.ToString());
         }
         return newSize;

      }

       /// <summary>
       /// Create Xml from an MEPSize
       /// </summary>
       /// <param name="size"></param>
       /// <param name="document"></param>
       /// <returns></returns>
      private static XElement CreateXmlFromMEPSize(MEPSize size, Autodesk.Revit.DB.Document document)
      {
         XElement xMEPSize = new XElement(XName.Get("MEPSize"));

         xMEPSize.Add(new XAttribute(XName.Get("innerDiameter"), (Convert.ConvertValueDocumentUnits(size.InnerDiameter, document) ).ToString()));
         xMEPSize.Add(new XAttribute(XName.Get("nominalDiameter"), (Convert.ConvertValueDocumentUnits(size.NominalDiameter, document) ).ToString()));
         xMEPSize.Add(new XAttribute(XName.Get("outerDiameter"), (Convert.ConvertValueDocumentUnits(size.OuterDiameter, document) ).ToString()));
         xMEPSize.Add(new XAttribute(XName.Get("usedInSizeLists"), size.UsedInSizeLists));
         xMEPSize.Add(new XAttribute(XName.Get("usedInSizing"), size.UsedInSizing));
         return xMEPSize;
      }
       /// <summary>
       /// Populate a routing preference manager from Xml
       /// </summary>
       /// <param name="routingPreferenceManagerXElement"></param>
      private void ParseRoutingPreferenceManagerFromXML(XElement routingPreferenceManagerXElement)
      {

         XAttribute xaPipeTypeName = routingPreferenceManagerXElement.Attribute(XName.Get("pipeTypeName"));
         XAttribute xaPreferredJunctionType = routingPreferenceManagerXElement.Attribute(XName.Get("preferredJunctionType"));
         
         PreferredJunctionType preferredJunctionType;
         bool r1 = Enum.TryParse<PreferredJunctionType>(xaPreferredJunctionType.Value, out preferredJunctionType);

         if (!r1)
            throw new RoutingPreferenceDataException("Invalid Preferred Junction Type in: " + routingPreferenceManagerXElement.ToString());

         ElementId pipeTypeId = GetPipeTypeByName(xaPipeTypeName.Value);
         if (pipeTypeId == ElementId.InvalidElementId)
            throw new RoutingPreferenceDataException("Could not find pipe type element in: " + routingPreferenceManagerXElement.ToString());

         PipeType pipeType = m_document.GetElement(pipeTypeId) as PipeType;

         RoutingPreferenceManager routingPreferenceManager = pipeType.RoutingPreferenceManager;
         routingPreferenceManager.PreferredJunctionType = preferredJunctionType;

         foreach (XNode xRule in routingPreferenceManagerXElement.Nodes())
         {
            if (xRule is XElement)
            {
               RoutingPreferenceRuleGroupType groupType;
               RoutingPreferenceRule rule = ParseRoutingPreferenceRuleFromXML(xRule as XElement, out groupType);
               routingPreferenceManager.AddRule(groupType, rule);
            }
         }

      }
      /// <summary>
      /// Create Xml from a RoutingPreferenceManager
      /// </summary>
      /// <param name="routingPreferenceManager"></param>
      /// <returns></returns>
      private XElement CreateXmlFromRoutingPreferenceManager(RoutingPreferenceManager routingPreferenceManager)
      {
         XElement xRoutingPreferenceManager = new XElement(XName.Get("RoutingPreferenceManager"));

         xRoutingPreferenceManager.Add(new XAttribute(XName.Get("pipeTypeName"), GetPipeTypeNameById(routingPreferenceManager.OwnerId)));

         xRoutingPreferenceManager.Add(new XAttribute(XName.Get("preferredJunctionType"), routingPreferenceManager.PreferredJunctionType.ToString()));

         for (int indexCrosses = 0; indexCrosses != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Crosses); indexCrosses++)
         {
            xRoutingPreferenceManager.Add(createXmlFromRoutingPreferenceRule(routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Crosses, indexCrosses), RoutingPreferenceRuleGroupType.Crosses));
         }

         for (int indexElbows = 0; indexElbows != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Elbows); indexElbows++)
         {
            xRoutingPreferenceManager.Add(createXmlFromRoutingPreferenceRule(routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Elbows, indexElbows), RoutingPreferenceRuleGroupType.Elbows));
         }

         for (int indexSegments = 0; indexSegments != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Segments); indexSegments++)
         {
            xRoutingPreferenceManager.Add(createXmlFromRoutingPreferenceRule(routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Segments, indexSegments), RoutingPreferenceRuleGroupType.Segments));
         }

         for (int indexJunctions = 0; indexJunctions != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Junctions); indexJunctions++)
         {
            xRoutingPreferenceManager.Add(createXmlFromRoutingPreferenceRule(routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Junctions, indexJunctions), RoutingPreferenceRuleGroupType.Junctions));
         }

         for (int indexTransitions = 0; indexTransitions != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Transitions); indexTransitions++)
         {
            xRoutingPreferenceManager.Add(createXmlFromRoutingPreferenceRule(routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Transitions, indexTransitions), RoutingPreferenceRuleGroupType.Transitions));
         }

         for (int indexUnions = 0; indexUnions != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.Unions); indexUnions++)
         {
            xRoutingPreferenceManager.Add(createXmlFromRoutingPreferenceRule(routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.Unions, indexUnions), RoutingPreferenceRuleGroupType.Unions));
         }

         for (int indexMechanicalJoints = 0; indexMechanicalJoints != routingPreferenceManager.GetNumberOfRules(RoutingPreferenceRuleGroupType.MechanicalJoints); indexMechanicalJoints++)
         {
            xRoutingPreferenceManager.Add(createXmlFromRoutingPreferenceRule(routingPreferenceManager.GetRule(RoutingPreferenceRuleGroupType.MechanicalJoints, indexMechanicalJoints), RoutingPreferenceRuleGroupType.MechanicalJoints));
         }


         return xRoutingPreferenceManager;
      }

       /// <summary>
       /// Create a RoutingPreferenceRule from Xml
       /// </summary>
       /// <param name="ruleXElement"></param>
       /// <param name="groupType"></param>
       /// <returns></returns>
      private RoutingPreferenceRule ParseRoutingPreferenceRuleFromXML(XElement ruleXElement, out RoutingPreferenceRuleGroupType groupType)
      {

         XAttribute xaDescription = null;
         XAttribute xaPartName = null;
         XAttribute xaMinSize = null;
         XAttribute xaMaxSize = null;
         XAttribute xaGroup = null;

         xaDescription = ruleXElement.Attribute(XName.Get("description"));
         xaPartName = ruleXElement.Attribute(XName.Get("partName"));
         xaGroup = ruleXElement.Attribute(XName.Get("ruleGroup"));
         xaMinSize = ruleXElement.Attribute(XName.Get("minimumSize"));

         ElementId partId;

         bool r3 = Enum.TryParse<RoutingPreferenceRuleGroupType>(xaGroup.Value, out groupType);
         if (!r3)
            throw new RoutingPreferenceDataException("Could not parse rule group type: " + xaGroup.Value);

         string description = xaDescription.Value;

         if (groupType == RoutingPreferenceRuleGroupType.Segments)
            partId = GetSegmentByName(xaPartName.Value);
         else
            partId = GetFittingByName(xaPartName.Value);

         if (partId == ElementId.InvalidElementId)
            throw new RoutingPreferenceDataException("Could not find MEP Part: " + xaPartName.Value + ".  Is this the correct family name, and is the correct family loaded?");

         RoutingPreferenceRule rule = new RoutingPreferenceRule(partId, description);


         PrimarySizeCriterion sizeCriterion;
         if (string.Compare(xaMinSize.Value, "All", true) == 0)  //If "All" or "None" are specified, set min and max values to documented "Max" values.
         {
            sizeCriterion = PrimarySizeCriterion.All();
         }
         else if (string.Compare(xaMinSize.Value, "None", true) == 0)
         {
            sizeCriterion = PrimarySizeCriterion.None();
         }
         else  // "maximumSize" attribute is only needed if not specifying "All" or "None."
         {
            try
            {
               xaMaxSize = ruleXElement.Attribute(XName.Get("maximumSize"));
            }
            catch (System.Exception)
            {
               throw new RoutingPreferenceDataException("Cannot get maximumSize attribute in: " + ruleXElement.ToString());
            }
            double min, max;
            bool r1 = double.TryParse(xaMinSize.Value, out min);
            bool r2 = double.TryParse(xaMaxSize.Value, out max);
            if (!r1 || !r2)
               throw new RoutingPreferenceDataException("Could not parse size values: " + xaMinSize.Value + ", " + xaMaxSize.Value);
            if (min > max)
               throw new RoutingPreferenceDataException("Invalid size range.");

            min = Convert.ConvertValueToFeet(min, m_document);
            max = Convert.ConvertValueToFeet(max, m_document);
            sizeCriterion = new PrimarySizeCriterion(min, max);
         }

         rule.AddCriterion(sizeCriterion);

         return rule;

      }
       /// <summary>
       /// Create Xml from a RoutingPreferenceRule
       /// </summary>
       /// <param name="rule"></param>
       /// <param name="groupType"></param>
       /// <returns></returns>
      private XElement createXmlFromRoutingPreferenceRule(RoutingPreferenceRule rule, RoutingPreferenceRuleGroupType groupType)
      {
         XElement xRoutingPreferenceRule = new XElement(XName.Get("RoutingPreferenceRule"));
         xRoutingPreferenceRule.Add(new XAttribute(XName.Get("description"), rule.Description));
         xRoutingPreferenceRule.Add(new XAttribute(XName.Get("ruleGroup"), groupType.ToString()));
         if (rule.NumberOfCriteria >= 1)
         {
            PrimarySizeCriterion psc = rule.GetCriterion(0) as PrimarySizeCriterion;

            if (psc.IsEqual(PrimarySizeCriterion.All()))
            {
               xRoutingPreferenceRule.Add(new XAttribute(XName.Get("minimumSize"), "All"));
            }
            else
               if (psc.IsEqual(PrimarySizeCriterion.None()))
               {
                  xRoutingPreferenceRule.Add(new XAttribute(XName.Get("minimumSize"), "None"));
               }
               else  //Only specify "maximumSize" if not specifying "All" or "None" for minimum size, just like in the UI.
               {

                  xRoutingPreferenceRule.Add(new XAttribute(XName.Get("minimumSize"), (Convert.ConvertValueDocumentUnits(psc.MinimumSize, m_document)).ToString()));
                  xRoutingPreferenceRule.Add(new XAttribute(XName.Get("maximumSize"), (Convert.ConvertValueDocumentUnits(psc.MaximumSize, m_document)).ToString()));
               }
         }
         else
         {
            xRoutingPreferenceRule.Add(new XAttribute(XName.Get("minimumSize"), "All"));
         }

         if (groupType == RoutingPreferenceRuleGroupType.Segments)
         {
            xRoutingPreferenceRule.Add(new XAttribute(XName.Get("partName"), GetSegmentNameById(rule.MEPPartId)));
         }
         else
            xRoutingPreferenceRule.Add(new XAttribute(XName.Get("partName"), GetFittingNameById(rule.MEPPartId)));

         return xRoutingPreferenceRule;
      }
      #endregion   



        #region Accessors and finders
        /// <summary>
        /// Get PipeScheduleTypeName by Id
        /// </summary>
        /// <param name="pipescheduleTypeId"></param>
        /// <returns></returns>
        private string GetPipeScheduleTypeNamebyId(ElementId pipescheduleTypeId)
        {
            return m_document.GetElement(pipescheduleTypeId).Name;
        }

        /// <summary>
        /// Get material name by Id
        /// </summary>
        /// <param name="materialId"></param>
        /// <returns></returns>
        private string GetMaterialNameById(ElementId materialId)
        {
            return m_document.GetElement(materialId).Name;
        }

        /// <summary>
        /// Get segment name by Id
        /// </summary>
        /// <param name="segmentId"></param>
        /// <returns></returns>
        private string GetSegmentNameById(ElementId segmentId)
        {
            return m_document.GetElement(segmentId).Name;
        }

        /// <summary>
        /// Get fitting name by Id
        /// </summary>
        /// <param name="fittingId"></param>
        /// <returns></returns>
        private string GetFittingNameById(ElementId fittingId)
        {
            FamilySymbol fs = m_document.GetElement(fittingId) as FamilySymbol;
            return fs.Family.Name + " " + fs.Name;
        }

        /// <summary>
        /// Get segment by Ids
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="pipeScheduleTypeId"></param>
        /// <returns></returns>
        private ElementId GetSegmentByIds(ElementId materialId, ElementId pipeScheduleTypeId)
        {
            if ((materialId == ElementId.InvalidElementId) || (pipeScheduleTypeId == ElementId.InvalidElementId))
                return ElementId.InvalidElementId;

            Element material = m_document.GetElement(materialId);
            Element pipeScheduleType = m_document.GetElement(pipeScheduleTypeId);
            string segmentName = material.Name + " - " + pipeScheduleType.Name;
            return GetSegmentByName(segmentName);

        }

        /// <summary>
        /// Get pipe type name by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetPipeTypeNameById(ElementId id)
        {
            return m_document.GetElement(id).Name;
        }

        /// <summary>
        /// Get segment by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ElementId GetSegmentByName(string name)
        {
            foreach (Segment segment in m_segments)
                if (segment.Name == name)
                    return segment.Id;
            return ElementId.InvalidElementId;
        }

        /// <summary>
        /// Get fitting by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ElementId GetFittingByName(string name)
        {
            foreach (FamilySymbol fitting in m_fittings)
                if ((fitting.Family.Name + " " + fitting.Name) == name)

                    return fitting.Id;
            return ElementId.InvalidElementId;
        }

        /// <summary>
        /// Get material by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ElementId GetMaterialByName(string name)
        {
            foreach (Material material in m_materials)
                if (material.Name == name)
                    return material.Id;
            return ElementId.InvalidElementId;
        }

        /// <summary>
        /// Get pipe schedule type by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ElementId GetPipeScheduleTypeByName(string name)
        {
            foreach (PipeScheduleType pipeScheduleType in m_pipeSchedules)
                if (pipeScheduleType.Name == name)
                    return pipeScheduleType.Id;
            return ElementId.InvalidElementId;

        }

        /// <summary>
        /// Get pipe type by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ElementId GetPipeTypeByName(string name)
        {
            foreach (PipeType pipeType in m_pipeTypes)
                if (pipeType.Name == name)
                    return pipeType.Id;
            return ElementId.InvalidElementId;

        }

        /// <summary>
        /// Update fittings list
        /// </summary>
        private void UpdateFittingsList()
        {
            m_fittings = GetAllFittings(m_document);
        }

        /// <summary>
        /// Update segments list
        /// </summary>
        private void UpdateSegmentsList()
        {
            m_segments = GetAllPipeSegments(m_document);
        }

        /// <summary>
        /// Update pipe types list
        /// </summary>
        private void UpdatePipeTypesList()
        {
            m_pipeTypes = GetAllPipeTypes(m_document);
        }

        /// <summary>
        /// Update pipe type schedules list
        /// </summary>
        private void UpdatePipeTypeSchedulesList()
        {
            m_pipeSchedules = GetAllPipeScheduleTypes(m_document);
        }

        /// <summary>
        /// Update materials list
        /// </summary>
        private void UpdateMaterialsList()
        {
            m_materials = GetAllMaterials(m_document);
        }

        /// <summary>
        /// Get all pipe segments
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public IEnumerable<PipeSegment> GetAllPipeSegments(Document document)
        {
            FilteredElementCollector fec = new FilteredElementCollector(document);
            fec.OfClass(typeof(PipeSegment));
            IEnumerable<PipeSegment> segments = fec.ToElements().Cast<PipeSegment>();
            return segments;
        }

        /// <summary>
        /// Get all fittings
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public IEnumerable<FamilySymbol> GetAllFittings(Document document)
        {
            FilteredElementCollector fec = new FilteredElementCollector(document);
            fec.OfClass(typeof(FamilySymbol));
            fec.OfCategory(BuiltInCategory.OST_PipeFitting);
            IEnumerable<FamilySymbol> fittings = fec.ToElements().Cast<FamilySymbol>();
            return fittings;
        }

        /// <summary>
        /// Get all materials
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private IEnumerable<Material> GetAllMaterials(Document document)
        {
            FilteredElementCollector fec = new FilteredElementCollector(document);
            fec.OfClass(typeof(Material));
            IEnumerable<Material> materials = fec.ToElements().Cast<Material>();
            return materials;
        }

        /// <summary>
        /// Get all pipe schedule types
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public IEnumerable<PipeScheduleType> GetAllPipeScheduleTypes(Document document)
        {
            FilteredElementCollector fec = new FilteredElementCollector(document);
            fec.OfClass(typeof(Autodesk.Revit.DB.Plumbing.PipeScheduleType));
            IEnumerable<Autodesk.Revit.DB.Plumbing.PipeScheduleType> pipeScheduleTypes = fec.ToElements().Cast<Autodesk.Revit.DB.Plumbing.PipeScheduleType>();
            return pipeScheduleTypes;
        }

        /// <summary>
        /// Get all pipe types
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public IEnumerable<PipeType> GetAllPipeTypes(Document document)
        {
            ElementClassFilter ecf = new ElementClassFilter(typeof(PipeType));

            FilteredElementCollector fec = new FilteredElementCollector(document);
            fec.WherePasses(ecf);
            IEnumerable<PipeType> pipeTypes = fec.ToElements().Cast<PipeType>();
            return pipeTypes;
        }


        #endregion


    }
}
