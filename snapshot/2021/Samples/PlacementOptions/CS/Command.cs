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
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace Revit.SDK.Samples.PlacementOptions.CS
{
    /// <summary>
    /// Implements the Revit add-in interface IExternalCommand
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
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
        /// Cancelled can be used to signify that the user canceled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public virtual Result Execute(ExternalCommandData commandData
            , ref string message, ElementSet elements)
        {
            try
            {
                Document document = commandData.Application.ActiveUIDocument.Document;

                // Show the OptionsForm for options selection
                using (OptionsForm optionsForm = new OptionsForm())
                {
                    if (DialogResult.OK == optionsForm.ShowDialog())
                    {
                        List<FamilySymbol> famSymbolList = new List<FamilySymbol>();
                        if (optionsForm.OptionType == PlacementOptionsEnum.FaceBased)
                        {
                            famSymbolList = FindProperFamilySymbol(document, BuiltInCategory.OST_GenericModel);
                            if (famSymbolList == null || famSymbolList.Count == 0)
                            {
                                TaskDialog.Show("Error", "There is no Face-Based family symbol, please load one first.");
                                return Result.Cancelled;
                            }
                            // Show the FacebasedForm for setting the face based family symbol and FaceBasedPlacementType option.
                            using (FacebasedForm facebaseForm = new FacebasedForm(famSymbolList))
                            {
                                if (DialogResult.OK == facebaseForm.ShowDialog())
                                {
                                    commandData.Application.ActiveUIDocument.PromptForFamilyInstancePlacement(facebaseForm.SelectedFamilySymbol, facebaseForm.FIPlacementOptions);
                                }
                            }
                        }
                        else
                        {
                            famSymbolList = FindProperFamilySymbol(document, BuiltInCategory.OST_StructuralFraming);
                            if (famSymbolList == null || famSymbolList.Count == 0)
                            {
                                TaskDialog.Show("Error", "There is no Sketch-Based family symbol, please load one first.");
                                return Result.Cancelled;
                            }
                            // Show the FacebasedForm for setting the face based family symbol and SketchGalleryOptions option.
                            using (SketchbasedForm sketchbasedForm = new SketchbasedForm(famSymbolList))
                            {
                                if (DialogResult.OK == sketchbasedForm.ShowDialog())
                                {
                                    commandData.Application.ActiveUIDocument.PromptForFamilyInstancePlacement(sketchbasedForm.SelectedFamilySymbol, sketchbasedForm.FIPlacementOptions);
                                }
                            }
                        }
                    }
                }

                return Result.Succeeded;
            }
            catch (Autodesk.Revit.Exceptions.OperationCanceledException)
            {
                // UI preempted operation is finished peacefully, so we consider it successful
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }

        /// <summary>
        /// Find proper family symbol for family instance placement.
        /// </summary>
        /// <param name="document">The Revit document.</param>
        /// <param name="category">The category of the family symbol.</param>
        /// <returns>The list of family symbol.</returns>
        private List<FamilySymbol> FindProperFamilySymbol(Document document, BuiltInCategory category)
        {
            FilteredElementCollector collector = new FilteredElementCollector(document);
            // OST_GenericModel for looking up the face based family symbol.
            // OST_StructuralFraming for looking up the beam framing symbol, which is sketch based.
            ElementCategoryFilter filter = new ElementCategoryFilter(category);
            collector.WherePasses(filter);
            List<Element> symbollList = collector.ToElements().ToList();
            if (symbollList.Count() == 0)
                return null;

            List<FamilySymbol> famSymbolList = new List<FamilySymbol>();
            foreach (Element elem in symbollList)
            {
                FamilySymbol famSymbol = elem as FamilySymbol;
                if (famSymbol != null)
                    famSymbolList.Add(famSymbol);
            }
            return famSymbolList;
        }
    }

    /// <summary>
    /// The placement options for user to place the family instance.
    /// </summary>
    public enum PlacementOptionsEnum
    {
        /// <summary>
        /// Face based
        /// </summary>
        FaceBased,

        /// <summary>
        /// Sketch based
        /// </summary>
        SketchBased
    }
}

