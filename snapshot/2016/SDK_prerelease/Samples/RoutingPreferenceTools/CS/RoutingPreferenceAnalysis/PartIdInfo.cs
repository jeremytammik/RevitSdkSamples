//
// (C) Copyright 2003-2014 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.RoutingPreferenceTools.CS
{

    /// <summary>
    /// This class contains a routing preference rule group and list of elementIds that correspond
    /// to found segments and fittings that meet criteria specified through a routing preference manager.
    /// </summary>
    public class PartIdInfo
    {
        // List of part Ids
        private List<ElementId> m_ids;
        // group type
        private RoutingPreferenceRuleGroupType m_groupType;

        /// <summary>
        /// Id
        /// </summary>
        public List<ElementId> Id
        {
            get { return m_ids; }
        }

        /// <summary>
        ///  Group type
        /// </summary>
        public RoutingPreferenceRuleGroupType GroupType
        {
            get { return m_groupType; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="groupType"></param>
        /// <param name="ids"></param>
        public PartIdInfo(RoutingPreferenceRuleGroupType groupType, IList<ElementId> ids)
        {
            m_ids = new List<ElementId>();
            m_groupType = groupType;
            m_ids.AddRange(ids);
        }

        /// <summary>
        /// Build XML information of document
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public XElement GetXml(Autodesk.Revit.DB.Document document)
        {
            XElement xPartInfo = new XElement(XName.Get("PartInfo"));
            xPartInfo.Add(new XAttribute(XName.Get("groupType"), m_groupType.ToString()));
            xPartInfo.Add(new XAttribute(XName.Get("partNames"), GetFittingNames(document)));
            return xPartInfo;
        }


        private string GetFittingName(Document document, ElementId id)
        {
            string fittingName = " None ";


            if (id != ElementId.InvalidElementId)
            {
                Element element = document.GetElement(id);
                if (element is Segment)
                {
                    fittingName = element.Name;
                }
                else
                {
                    FamilySymbol familySymbol = element as FamilySymbol;
                    fittingName = familySymbol.Family.Name + " " + familySymbol.Name;
                }
            }


            return fittingName;
        }

        /// <summary>
        /// Fitting name
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private string GetFittingNames(Document document)
        {
            string fittingNames = "";

            if (m_ids.Count == 0)
            {
                fittingNames += "None -1";
            }
            foreach (ElementId id in m_ids)
            {
                fittingNames += GetFittingName(document, id) + " " + id.ToString() + ", ";
            }
            return fittingNames.Remove(fittingNames.Length - 2, 2);
        }


    }
}
