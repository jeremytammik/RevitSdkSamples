//
// (C) Copyright 2003-2018 by Autodesk, Inc.
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

using Autodesk.Revit.DB;

namespace Revit.SDK.Samples.ViewTemplateCreation.CS
{
   /// <summary>
   /// A form for the sample, which allows user to create and configure a new view template.
   /// </summary>
   public partial class ViewTemplateCreationForm : System.Windows.Forms.Form
   {
      #region FormInitialization
      /// <summary>
      /// Creates a form for the sample, which allows user to create and configure a new view template.
      /// </summary>
      public ViewTemplateCreationForm(Document activeDocument)
      {
         InitializeComponent();

         this.Text = Utils.SampleName;
         m_activeDocument = activeDocument;
         InitViewNameComboBox();
         InitPartVisibilityComboBox();
         InitDetailLevelValueComboBox();
      }

      /// <summary>
      /// Populates the viewNameComboBox with the names of all views from the current document that are valid for view template creation.
      /// </summary>
      private void InitViewNameComboBox()
      {
         FilteredElementCollector viewCollector = new FilteredElementCollector(m_activeDocument);
         viewCollector.OfCategory(BuiltInCategory.OST_Views);

         m_views = new List<View>();
         foreach (View curView in viewCollector)
         {
            if ( (!curView.IsTemplate) && (curView.IsViewValidForTemplateCreation()) )
            {
               m_views.Add(curView);
               // add view type to prevent duplication of names with different view types 
               string extendedViewName = String.Format("{0}:{1}", curView.ViewType.ToString(), curView.Name);
               viewNameComboBox.Items.Add(extendedViewName);
            }
         }
      }

      private void InitPartVisibilityComboBox()
      {
         partsVisibilityStateComboBox.Items.Add(IncludeString);
         partsVisibilityStateComboBox.Items.Add(ExcludeString);
      }

      private void InitDetailLevelValueComboBox()
      {
         detailLevelValueComboBox.Items.Add(ViewDetailLevel.Coarse.ToString());
         detailLevelValueComboBox.Items.Add(ViewDetailLevel.Medium.ToString());
         detailLevelValueComboBox.Items.Add(ViewDetailLevel.Fine.ToString());
      }
      #endregion FormInitialization

      #region EventHandlers
      private void CancelButton_Click(object sender, EventArgs e)
      {
         this.Close();
      }

      /// <summary>
      /// Creates a new view template based on the selected view, applying all settings and assigning the view to the newly created template.
      /// </summary>
      private void ApplyButton_Click(object sender, EventArgs e)
      {
         try
         {
            using (Transaction transaction = new Transaction(m_activeDocument, "View Template Creation sample"))
            {
               transaction.Start();

               View selectedView = GetSelectedView();
               View viewTemplate = selectedView.CreateViewTemplate();
               Utils.ShowInformationMessageBox(string.Format("View template '{0}' has been created.", viewTemplate.Name));

               SetPartsVisibilityIncludeState(viewTemplate);
               SetDetailLevelValue(viewTemplate);
               ChangeVgOverridesModelSettings(viewTemplate);

               selectedView.ViewTemplateId = viewTemplate.Id;
               Utils.ShowInformationMessageBox(string.Format("View template '{0}' has been assigned to '{1}' view.", viewTemplate.Name, selectedView.Name));

               transaction.Commit();
            }
         }
         catch (Exception ex)
         {
            Utils.ShowWarningMessageBox(ex.ToString());
         }
         this.Close();
      }
      #endregion EventHandlers

      #region Methods
      private View GetSelectedView()
      {
         string extendedViewName = viewNameComboBox.SelectedItem.ToString();
         // retrieve view name and type strings so we can find the right view by name and type
         int idxColon = extendedViewName.IndexOf(":");
         string viewName = extendedViewName.Substring(idxColon + 1);
         string viewType = extendedViewName.Substring(0, idxColon);

         return m_views.Find((View v) => v.Name == viewName && v.ViewType.ToString() == viewType);
      }

      private void SetPartsVisibilityIncludeState(View view)
      {
         if (!GetSelectedPartsVisibilityIncludeState())
         {
            ICollection<ElementId> nonControlledParameterIds = view.GetNonControlledTemplateParameterIds();
            nonControlledParameterIds.Add(new ElementId(BuiltInParameter.VIEW_PARTS_VISIBILITY));
            view.SetNonControlledTemplateParameterIds(nonControlledParameterIds);
         }
      }

      private void SetDetailLevelValue(View view)
      {
         if (!view.HasDetailLevel())
         {
            Utils.ShowWarningMessageBox(string.Format("'{0}' view does not have '{1}' parameter.", view.Name, "Detail level"));
            return;
         }

         if (!view.CanModifyDetailLevel())
         {
            Utils.ShowWarningMessageBox(string.Format("'{0}' can not be modified in view '{1}'.", "Detail level", view.Name));
            return;
         }

         view.DetailLevel = GetSelectedDetailLevelValue();
      }

      private bool GetSelectedPartsVisibilityIncludeState()
      {
         string partsVisibility = partsVisibilityStateComboBox.SelectedItem.ToString();
         return partsVisibility == IncludeString;
      }

      private ViewDetailLevel GetSelectedDetailLevelValue()
      {
         string newDetailLevelName = detailLevelValueComboBox.SelectedItem.ToString();
         return (ViewDetailLevel)Enum.Parse(typeof(ViewDetailLevel), newDetailLevelName);
      }

      private void ChangeVgOverridesModelSettings(View view)
      {
         if (!view.AreGraphicsOverridesAllowed())
         {
            Utils.ShowWarningMessageBox(string.Format("Graphic overrides are not alowed for the '{0}' view", view.Name));
            return;
         }

         Autodesk.Revit.DB.Color blackColor = new Autodesk.Revit.DB.Color(0, 0, 0);
         FillPatternElement foregroundFillPattern = FillPatternElement.GetFillPatternElementByName(view.Document, FillPatternTarget.Drafting, "<Solid fill>");

         SetCutPatternSettings(view, BuiltInCategory.OST_Columns, blackColor, foregroundFillPattern);
         SetCutPatternSettings(view, BuiltInCategory.OST_Doors, blackColor, foregroundFillPattern);
         SetCutPatternSettings(view, BuiltInCategory.OST_Walls, blackColor, foregroundFillPattern);
         SetCutPatternSettings(view, BuiltInCategory.OST_Windows, blackColor, foregroundFillPattern);
      }

      private void SetCutPatternSettings(View view, BuiltInCategory buildInCategory, Autodesk.Revit.DB.Color backgroundColor, FillPatternElement foregroundFillPattern)
      {
         ElementId categoryId = new ElementId(buildInCategory);

         OverrideGraphicSettings ogSettings = view.GetCategoryOverrides(categoryId);
         if( (ogSettings == null) || (!ogSettings.IsValidObject) )
         {
            Utils.ShowWarningMessageBox(string.Format("Graphic overrides category '{0}' is not found or is not valid", buildInCategory.ToString()));
            return;
         }

         if (!(view.IsCategoryOverridable(categoryId)))
         {
            Utils.ShowWarningMessageBox(string.Format("Graphic overrides category '{0}' is not overridable", buildInCategory.ToString()));
            return;
         }

         ogSettings.SetCutBackgroundPatternColor(backgroundColor);
         ogSettings.SetCutForegroundPatternId(foregroundFillPattern.Id);
         view.SetCategoryOverrides(categoryId, ogSettings);
      }
      #endregion Methods

      #region ClassMembers
      private const string IncludeString = "include";
      private const string ExcludeString = "exclude";

      private List<View> m_views;
      private Document m_activeDocument;
      #endregion ClassMembers
   }
}
