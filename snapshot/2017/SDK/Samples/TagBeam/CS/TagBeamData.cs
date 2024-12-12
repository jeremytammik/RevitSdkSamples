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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit;
using Autodesk.Revit.UI.Selection;

namespace Revit.SDK.Samples.TagBeam.CS
{
    /// <summary>
    /// Tag beam data class.
    /// </summary>
    public class TagBeamData
    {
        //Required designer variable.
        private Autodesk.Revit.DB.View m_view; // current view
        private Autodesk.Revit.Creation.Document m_docCreator; // document creation
        private UIDocument m_revitDoc;
        /// <summary>
        /// Selected beams
        /// </summary>
        private List<FamilyInstance> m_beamList =
            new List<FamilyInstance>();

        /// <summary>
        /// Tag types whose category is "Structural Framing Tags"
        /// </summary>
        private List<FamilySymbolWrapper> m_categoryTagTypes =
            new List<FamilySymbolWrapper>();

        /// <summary>
        /// Tag types whose category is "Multi-Category Tags"
        /// </summary>
        private List<FamilySymbolWrapper> m_multiCategoryTagTypes =
            new List<FamilySymbolWrapper>();

        /// <summary>
        /// Tag types whose category is "Material Tags"
        /// </summary>
        private List<FamilySymbolWrapper> m_materialTagTypes =
            new List<FamilySymbolWrapper>();

        /// <summary>
        /// Initializes a new instance of TagBeamData. 
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command</param>
        public TagBeamData(ExternalCommandData commandData)
        {
            //Get beams selected 
           m_revitDoc = commandData.Application.ActiveUIDocument;
           m_docCreator = m_revitDoc.Document.Create;
           m_view = m_revitDoc.Document.ActiveView;

           ElementSet elementSet = new ElementSet();
           foreach (ElementId elementId in m_revitDoc.Selection.GetElementIds())
           {
              elementSet.Insert(m_revitDoc.Document.GetElement(elementId));
           }
            ElementSetIterator itor = elementSet.ForwardIterator();
            while (itor.MoveNext())
            {
                FamilyInstance familyInstance = itor.Current as FamilyInstance;
                if ((familyInstance != null) && (familyInstance.StructuralType == Autodesk.Revit.DB.Structure.StructuralType.Beam))
                {
                    m_beamList.Add(familyInstance);
                }
            }
            if (m_beamList.Count < 1)
            {
                throw new ApplicationException("there is no beam selected");
            } 

            //Get the family symbols of tag in this document.
            FilteredElementCollector collector = new FilteredElementCollector(commandData.Application.ActiveUIDocument.Document);
            IList<Element> elements = collector.OfClass(typeof(Family)).ToElements();

            foreach (Family family in elements)
            {
               if (family != null && family.GetFamilySymbolIds() != null)
               {
                  List<FamilySymbol> ffs = new List<FamilySymbol>(); 
                  foreach (ElementId elementId in family.GetFamilySymbolIds())
                  {
                     ffs.Add((FamilySymbol)(commandData.Application.ActiveUIDocument.Document.GetElement(elementId)));
                  }
                  foreach(FamilySymbol tagSymbol in ffs)
                  {
                     try
                     {
                        if (tagSymbol != null)
                        {
                           switch (tagSymbol.Category.Name)
                           {
                              case "Structural Framing Tags":
                                 m_categoryTagTypes.Add(new FamilySymbolWrapper(tagSymbol));
                                 continue;
                              case "Material Tags":
                                 m_materialTagTypes.Add(new FamilySymbolWrapper(tagSymbol));
                                 continue;
                              case "Multi-Category Tags":
                                 m_multiCategoryTagTypes.Add(new FamilySymbolWrapper(tagSymbol));
                                 continue;
                              default:
                                 continue;
                           }
                        }
                     }
                     catch (Exception)
                     {
                        continue;
                     }
                  }
               }
            }
        }

        /// <summary>
        /// Tag families with specified mode
        /// </summary>
        /// <param name="mode">mode of tag families to get</param>
        /// <returns></returns>
        public List<FamilySymbolWrapper> this[TagMode mode]
        {
            get
            {
                switch (mode)
                {
                    case TagMode.TM_ADDBY_CATEGORY:
                        return m_categoryTagTypes;
                    case TagMode.TM_ADDBY_MATERIAL:
                        return m_materialTagTypes;
                    case TagMode.TM_ADDBY_MULTICATEGORY:
                        return m_multiCategoryTagTypes;
                    default:
                        return null;
                }
            }
        }

        /// <summary>
        /// Tag the beam's start and end.
        /// </summary>
        /// <param name="tagMode">Mode of tag</param>
        /// <param name="tagSymbol">Tag symbol wrapper</param>
        /// <param name="leader">Whether the tag has leader</param>
        /// <param name="tagOrientation">Orientation of tag</param>
        public void CreateTag(TagMode tagMode,
            FamilySymbolWrapper tagSymbol, bool leader,
            TagOrientation tagOrientation)
        {
            foreach(FamilyInstance beam in m_beamList)
            {
                //Get the start point and end point of the selected beam.
                Autodesk.Revit.DB.LocationCurve location = beam.Location as Autodesk.Revit.DB.LocationCurve;
                Autodesk.Revit.DB.Curve curve = location.Curve;

                Transaction t = new Transaction(m_revitDoc.Document);
                t.Start("Create new tag");
                //Create tag on the beam's start and end.
                IndependentTag tag1 = m_docCreator.NewTag(
                    m_view, beam, leader, tagMode, tagOrientation, curve.GetEndPoint(0));
                IndependentTag tag2 = m_docCreator.NewTag(
                    m_view, beam, leader, tagMode, tagOrientation, curve.GetEndPoint(1));

                //Change the tag's object Type.
                tag1.ChangeTypeId(tagSymbol.FamilySymbol.Id);
                tag2.ChangeTypeId(tagSymbol.FamilySymbol.Id);
                t.Commit();
            }
        }
    }
}
