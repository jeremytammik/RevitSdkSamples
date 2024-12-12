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
using System.IO;

using System.Diagnostics;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.Samples.MaterialQuantities
{
    /// <summary>
    /// Outputs an analysis of the materials that make up walls, floors, and roofs, and displays the output in Excel.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]

    public class Command : IExternalCommand
    {
        static AddInId appId = new AddInId(new Guid("7E5CAC0D-F3D8-4040-89D6-0828D681561B"));
        /// <summary>
        /// The top level command.
        /// </summary>
        /// <param name="revit">An object that is passed to the external application 
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
        public Result Execute(ExternalCommandData revit,
                                                              ref string message,
                                                              ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = revit.Application.Application;
            m_doc = revit.Application.ActiveUIDocument.Document;

            String filename = "CalculateMaterialQuantities.txt";

            m_writer = new StreamWriter(filename);

            ExecuteCalculationsWith<RoofMaterialQuantityCalculator>();
            ExecuteCalculationsWith<WallMaterialQuantityCalculator>();
            ExecuteCalculationsWith<FloorMaterialQuantityCalculator>();

            m_writer.Close();

            // This operation doesn't change the model, so return cancelled to cancel the transaction
            return Result.Cancelled;
        }

        /// <summary>
        /// Executes a calculator for one type of Revit element.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private void ExecuteCalculationsWith<T>() where T : MaterialQuantityCalculator, new()
        {
            T calculator = new T();
            calculator.SetDocument(m_doc);
            calculator.CalculateMaterialQuantities();
            calculator.ReportResults(m_writer);
        }

        #region Basic Command Data
        private Document m_doc;

        private TextWriter m_writer;
        #endregion

    }

    /// <summary>
    /// The wall material quantity calculator specialized class.
    /// </summary>
    class WallMaterialQuantityCalculator : MaterialQuantityCalculator
    {
        protected override void CollectElements()
        {
            // filter for non-symbols that match the desired category so that inplace elements will also be found
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);
            m_elementsToProcess = collector.OfCategory(BuiltInCategory.OST_Walls).WhereElementIsNotElementType().ToElements();
        }

        protected override string GetElementTypeName()
        {
            return "Wall";
        }
    }

    /// <summary>
    /// The floor material quantity calculator specialized class.
    /// </summary>
    class FloorMaterialQuantityCalculator : MaterialQuantityCalculator
    {
        protected override void CollectElements()
        {
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);
            m_elementsToProcess = collector.OfCategory(BuiltInCategory.OST_Floors).WhereElementIsNotElementType().ToElements();
        }

        protected override string GetElementTypeName()
        {
            return "Floor";
        }
    }

    /// <summary>
    /// The roof material quantity calculator specialized class.
    /// </summary>
    class RoofMaterialQuantityCalculator : MaterialQuantityCalculator
    {
        protected override void CollectElements()
        {
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);
            m_elementsToProcess = collector.OfCategory(BuiltInCategory.OST_Roofs).WhereElementIsNotElementType().ToElements();
        }

        protected override string GetElementTypeName()
        {
            return "Roof";
        }
    }

    /// <summary>
    /// The base material quantity calculator for all element types.
    /// </summary>
    abstract class MaterialQuantityCalculator
    {
        /// <summary>
        /// The list of elements for material quantity extraction.
        /// </summary>
        protected IList<Element> m_elementsToProcess;

        /// <summary>
        /// Override this to populate the list of elements for material quantity extraction.
        /// </summary>
        protected abstract void CollectElements();

        /// <summary>
        /// Override this to return the name of the element type calculated by this calculator.
        /// </summary>
        protected abstract String GetElementTypeName();

        /// <summary>
        /// Sets the document for the calculator class.
        /// </summary>
        public void SetDocument(Document d)
        {
            m_doc = d;
            Autodesk.Revit.ApplicationServices.Application app = d.Application;
        }

        /// <summary>
        /// Executes the calculation.
        /// </summary>
        public void CalculateMaterialQuantities()
        {
            CollectElements();
            CalculateNetMaterialQuantities();
            CalculateGrossMaterialQuantities();
        }

        /// <summary>
        /// Calculates net material quantities for the target elements.
        /// </summary>
        private void CalculateNetMaterialQuantities()
        {
            foreach (Element e in m_elementsToProcess)
            {
                CalculateMaterialQuantitiesOfElement(e);
            }
        }

        /// <summary>
        /// Calculates gross material quantities for the target elements (material quantities with 
        /// all openings, doors and windows removed). 
        /// </summary>
        private void CalculateGrossMaterialQuantities()
        {
            m_calculatingGrossQuantities = true;
            Transaction t = new Transaction(m_doc);
            t.SetName("Delete Cutting Elements");
            t.Start();
            DeleteAllCuttingElements();
            m_doc.Regenerate();
            foreach (Element e in m_elementsToProcess)
            {
                CalculateMaterialQuantitiesOfElement(e);
            }
            t.RollBack();
        }

        /// <summary>
        /// Delete all elements that cut out of target elements, to allow for calculation of gross material quantities.
        /// </summary>
        private void DeleteAllCuttingElements()
        {
            IList<ElementFilter> filterList = new List<ElementFilter>();
            FilteredElementCollector collector = new FilteredElementCollector(m_doc);

            // (Type == FamilyInstance && (Category == Door || Category == Window) || Type == Opening
            ElementClassFilter filterFamilyInstance = new ElementClassFilter(typeof(FamilyInstance));
            ElementCategoryFilter filterWindowCategory = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
            ElementCategoryFilter filterDoorCategory = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
            LogicalOrFilter filterDoorOrWindowCategory = new LogicalOrFilter(filterWindowCategory, filterDoorCategory);
            LogicalAndFilter filterDoorWindowInstance = new LogicalAndFilter(filterDoorOrWindowCategory, filterFamilyInstance);

            ElementClassFilter filterOpening = new ElementClassFilter(typeof(Opening));

            LogicalOrFilter filterCuttingElements = new LogicalOrFilter(filterOpening, filterDoorWindowInstance);
            ICollection<Element> cuttingElementsList = collector.WherePasses(filterCuttingElements).ToElements();

            foreach (Element e in cuttingElementsList)
            {
                // Doors in curtain grid systems cannot be deleted.  This doesn't actually affect the calculations because
                // material quantities are not extracted for curtain systems.
                if (e.Category != null)
                {
                    if (e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Doors)
                    {
                        FamilyInstance door = e as FamilyInstance;
                        Wall host = door.Host as Wall;

                        if (host.CurtainGrid != null)
                            continue;
                    }
                    ICollection<ElementId> deletedElements = m_doc.Delete(e.Id);

                    // Log failed deletion attempts to the output.  (These may be other situations where deletion is not possible but 
                    // the failure doesn't really affect the results.
                    if (deletedElements == null || deletedElements.Count < 1)
                    {
                        m_warningsForGrossQuantityCalculations.Add(
                                 String.Format("   The tool was unable to delete the {0} named {2} (id {1})", e.GetType().Name, e.Id, e.Name));
                    }
                }
            }
        }

        /// <summary>
        /// Store calculated material quantities in the storage collection.
        /// </summary>
        /// <param name="materialId">The material id.</param>
        /// <param name="volume">The extracted volume.</param>
        /// <param name="area">The extracted area.</param>
        /// <param name="quantities">The storage collection.</param>
        private void StoreMaterialQuantities(ElementId materialId, double volume, double area,
                                            Dictionary<ElementId, MaterialQuantities> quantities)
        {
            MaterialQuantities materialQuantityPerElement;
            bool found = quantities.TryGetValue(materialId, out materialQuantityPerElement);
            if (found)
            {
                if (m_calculatingGrossQuantities)
                {
                    materialQuantityPerElement.GrossVolume += volume;
                    materialQuantityPerElement.GrossArea += area;
                }
                else
                {
                    materialQuantityPerElement.NetVolume += volume;
                    materialQuantityPerElement.NetArea += area;
                }
            }
            else
            {
                materialQuantityPerElement = new MaterialQuantities();
                if (m_calculatingGrossQuantities)
                {
                    materialQuantityPerElement.GrossVolume = volume;
                    materialQuantityPerElement.GrossArea = area;
                }
                else
                {
                    materialQuantityPerElement.NetVolume = volume;
                    materialQuantityPerElement.NetArea = area;
                }
                quantities.Add(materialId, materialQuantityPerElement);
            }
        }

        /// <summary>
        /// Calculate and store material quantities for a given element.
        /// </summary>
        /// <param name="e">The element.</param>
        private void CalculateMaterialQuantitiesOfElement(Element e)
        {
            ElementId elementId = e.Id;
            ICollection<ElementId> materials = e.GetMaterialIds(false);

            foreach (ElementId materialId in materials)
            {
                double volume = e.GetMaterialVolume(materialId);
                double area = e.GetMaterialArea(materialId, false);

                if (volume > 0.0 || area > 0.0)
                {
                    StoreMaterialQuantities(materialId, volume, area, m_totalQuantities);

                    Dictionary<ElementId, MaterialQuantities> quantityPerElement;
                    bool found = m_quantitiesPerElement.TryGetValue(elementId, out quantityPerElement);
                    if (found)
                    {
                        StoreMaterialQuantities(materialId, volume, area, quantityPerElement);
                    }
                    else
                    {
                        quantityPerElement = new Dictionary<ElementId, MaterialQuantities>();
                        StoreMaterialQuantities(materialId, volume, area, quantityPerElement);
                        m_quantitiesPerElement.Add(elementId, quantityPerElement);
                    }
                }
            }
        }

        /// <summary>
        /// Write results in CSV format to the indicated output writer.
        /// </summary>
        /// <param name="writer">The output text writer.</param>
        public void ReportResults(TextWriter writer)
        {
            if (m_totalQuantities.Count == 0)
                return;

            String legendLine = "Gross volume(cubic ft),Net volume(cubic ft),Gross area(sq ft),Net area(sq ft)";

            writer.WriteLine();
            writer.WriteLine(String.Format("Totals for {0} elements,{1}", GetElementTypeName(), legendLine));

            // If unexpected deletion failures occurred, log the warnings to the output.
            if (m_warningsForGrossQuantityCalculations.Count > 0)
            {
                writer.WriteLine("WARNING: Calculations for gross volume and area may not be completely accurate due to the following warnings: ");
                foreach (String s in m_warningsForGrossQuantityCalculations)
                    writer.WriteLine(s);
                writer.WriteLine();
            }
            ReportResultsFor(m_totalQuantities, writer);

            foreach (ElementId keyId in m_quantitiesPerElement.Keys)
            {
                ElementId id = keyId;
                Element e = m_doc.GetElement(id);

                writer.WriteLine();
                writer.WriteLine(String.Format("Totals for {0} element {1} (id {2}),{3}",
                    GetElementTypeName(),
                    e.Name.Replace(',', ':'), // Element names may have ',' in them
                    id.IntegerValue, legendLine));

                Dictionary<ElementId, MaterialQuantities> quantities = m_quantitiesPerElement[id];

                ReportResultsFor(quantities, writer);
            }
        }

        /// <summary>
        /// Write the contents of one storage collection to the indicated output writer.
        /// </summary>
        /// <param name="quantities">The storage collection for material quantities.</param>
        /// <param name="writer">The output writer.</param>
        private void ReportResultsFor(Dictionary<ElementId, MaterialQuantities> quantities, TextWriter writer)
        {
            foreach (ElementId keyMaterialId in quantities.Keys)
            {
                ElementId materialId = keyMaterialId;
                MaterialQuantities quantity = quantities[materialId];

                Material material = m_doc.GetElement(materialId) as Material;

                //writer.WriteLine(String.Format("   {0} Net: [{1:F2} cubic ft {2:F2} sq. ft]  Gross: [{3:F2} cubic ft {4:F2} sq. ft]", material.Name, quantity.NetVolume, quantity.NetArea, quantity.GrossVolume, quantity.GrossArea));
                writer.WriteLine(String.Format("{0},{3:F2},{1:F2},{4:F2},{2:F2}",
                    material.Name.Replace(',', ':'),  // Element names may have ',' in them
                    quantity.NetVolume, quantity.NetArea, quantity.GrossVolume, quantity.GrossArea));
            }
        }

        #region Results Storage
        /// <summary>
        /// A storage of material quantities per individual element.
        /// </summary>
        private Dictionary<ElementId, Dictionary<ElementId, MaterialQuantities>> m_quantitiesPerElement = new Dictionary<ElementId, Dictionary<ElementId, MaterialQuantities>>();

        /// <summary>
        /// A storage of material quantities for the entire project.
        /// </summary>
        private Dictionary<ElementId, MaterialQuantities> m_totalQuantities = new Dictionary<ElementId, MaterialQuantities>();

        /// <summary>
        /// Flag indicating the mode of the calculation.
        /// </summary>
        private bool m_calculatingGrossQuantities = false;

        /// <summary>
        /// A collection of warnings generated due to failure to delete elements in advance of gross quantity calculations.
        /// </summary>
        private List<String> m_warningsForGrossQuantityCalculations = new List<string>();
        #endregion

        protected Document m_doc;
    }

    /// <summary>
    /// A storage class for the extracted material quantities.
    /// </summary>
    class MaterialQuantities
    {
        /// <summary>
        /// Gross volume (cubic ft)
        /// </summary>
        public double GrossVolume { get; set; }

        /// <summary>
        /// Gross area (sq. ft)
        /// </summary>
        public double GrossArea { get; set; }

        /// <summary>
        /// Net volume (cubic ft)
        /// </summary>
        public double NetVolume { get; set; }

        /// <summary>
        /// Net area (sq. ft)
        /// </summary>
        public double NetArea { get; set; }
    }
}
