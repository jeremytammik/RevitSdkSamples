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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI.Events;
using System.Diagnostics;

namespace Revit.SDK.Samples.AppearanceAssetEditing.CS
{  /// <summary>
   /// Implements the Revit add-in interface IExternalApplication
   /// </summary>
   public class Application : IExternalApplication
   {
      // class instance
      internal static Application thisApp = null;

      #region Class member variables
      // ModelessForm instance
      private AppearanceAssetEditingForm m_MyForm;
      Autodesk.Revit.UI.UIApplication m_revit;
      Document m_document;
      Material m_currentMaterial;
      ElementId m_currentAppearanceAssetElementId;
      #endregion

      #region IExternalApplication Members
      /// <summary>
      /// Implements the OnShutdown event
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnShutdown(UIControlledApplication application)
      {
         if (m_MyForm != null && !m_MyForm.IsDisposed)
         {
            m_MyForm.Dispose();
            m_MyForm = null;

            // if we've had a dialog, we had subscribed
            application.Idling -= IdlingHandler;
         }

         return Result.Succeeded;
      }

      /// <summary>
      /// Implements the OnStartup event
      /// </summary>
      /// <param name="application"></param>
      /// <returns></returns>
      public Result OnStartup(UIControlledApplication application)
      {
         m_MyForm = null;   // no dialog needed yet; the command will bring it
         thisApp = this;  // static access to this application instance

         return Result.Succeeded;
      }

      /// <summary>
      ///   This method creates and shows a modeless dialog, unless it already exists.
      /// </summary>
      /// <remarks>
      ///   The external command invokes this on the end-user's request
      /// </remarks>
      /// 
      public void ShowForm(UIApplication uiapp)
      {
         m_revit = uiapp;
         m_document = uiapp.ActiveUIDocument.Document;

         // If we do not have a dialog yet, create and show it
         if (m_MyForm == null || m_MyForm.IsDisposed)
         {
            m_MyForm = new AppearanceAssetEditingForm();
            m_MyForm.Show();

            // if we have a dialog, we need Idling too
            uiapp.Idling += IdlingHandler;
         }            
      }

      /// <summary>
      /// Compares two colors.
      /// </summary>
      /// <param name="color1">The first color.</param>
      /// <param name="color2">The second color.</param>
      /// <returns>True if the colors are equal, false otherwise.</returns>
      private bool ColorsEqual(Color color1, Color color2)
      {
         return color1.Red == color2.Red && color1.Green == color2.Green && color1.Blue == color2.Blue;
      }
      void Log(string msg)
      {
         string dt = DateTime.Now.ToString("u");
         Trace.WriteLine(dt + " " + msg);
      }

      /// <summary>
      /// Custom filter for selection.
      /// </summary>
      private class IsPaintedFaceSelectionFilter : ISelectionFilter
      {
         Document selectedDocument = null;

         public bool AllowElement(Element element)
         {
            selectedDocument = element.Document;
            return true;
         }

         public bool AllowReference(Reference refer, XYZ point)
         {
            if (selectedDocument == null)
            {
               throw new Exception("AllowElement was never called for this reference...");
            }

            Element element = selectedDocument.GetElement(refer);
            Face face = element.GetGeometryObjectFromReference(refer) as Face;

            return selectedDocument.IsPainted(element.Id, face);
         }
      }

      /// <summary>
      /// Get the painted material from selection.
      /// </summary>  
      internal void GetPaintedMaterial()
      {

         Reference refer;

         try
         {
            refer = m_revit.ActiveUIDocument.Selection.PickObject(ObjectType.Face, new IsPaintedFaceSelectionFilter(), "Select a painted face for editing.");
         }
         catch (Autodesk.Revit.Exceptions.OperationCanceledException)
         {
            // Selection Cancelled.
            return;
         }
         catch (Exception ex)
         {
            // If any error, give error information and return failed
            Log(ex.Message);
            return;
         }

         if (refer == null)
         {
            return;
         }
         else
         {
            Element element = m_document.GetElement(refer);
            Face face = element.GetGeometryObjectFromReference(refer) as Face;
            ElementId matId = m_document.GetPaintedMaterial(element.Id, face);
            m_currentMaterial = m_document.GetElement(matId) as Material;

            if (m_currentMaterial != null)
            {
               m_currentAppearanceAssetElementId = m_currentMaterial.AppearanceAssetId;

               // Clear selection
               m_revit.ActiveUIDocument.Selection.GetElementIds().Clear();
            }
         }

      }

      /// <summary>
      /// Check if the selected material supports "tint".
      /// </summary>
      /// <returns>True if the selected material supports "tint" or not.</returns>
      private bool SupportTintColor()
      {
         if (this.m_currentAppearanceAssetElementId == ElementId.InvalidElementId)
            return false;

         AppearanceAssetElement assetElem = m_document.GetElement(this.m_currentAppearanceAssetElementId) as AppearanceAssetElement;
         if (assetElem == null)
            return false;

         Asset asset = assetElem.GetRenderingAsset();
         AssetPropertyDoubleArray4d tintColorProp = asset.FindByName("common_Tint_color") as AssetPropertyDoubleArray4d;
         if (tintColorProp == null)
            return false;

         // If the material supports tint but it is not enabled, it will be enabled first with a value (255 255 255)
         AssetPropertyBoolean tintToggleProp = asset.FindByName("common_Tint_color") as AssetPropertyBoolean;
         if ((tintToggleProp != null) && !(tintToggleProp.Value))
         {
            EnableTintColor();
         }

         return true;
      }

      /// <summary>
      /// Enable tint color.
      /// </summary>             
      private void EnableTintColor()
      {
         using (Transaction transaction = new Transaction(m_document, "Enable tint color"))
         {
            transaction.Start();

            using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(m_document))
            {
               Asset editableAsset = editScope.Start(m_currentAppearanceAssetElementId);

               //  If the material supports tint but it is not enabled, it will be enabled first with a value (255 255 255)
               AssetPropertyBoolean tintToggleProp = editableAsset.FindByName("common_Tint_color") as AssetPropertyBoolean;
               AssetPropertyDoubleArray4d tintColorProp = editableAsset.FindByName("common_Tint_color") as AssetPropertyDoubleArray4d;

               tintToggleProp.Value = true;
               tintColorProp.SetValueAsColor(new Color(255, 255, 255));

               editScope.Commit(true);
            }

            transaction.Commit();
         }

      }

      /// <summary>
      /// Check if the button Lighter is enabled.
      /// </summary>
      /// <returns>True if the material can be made lighter or not.</returns>
      internal bool IsLighterEnabled()
      {
         if (!SupportTintColor())
            return false;

         AppearanceAssetElement assetElem = m_document.GetElement(m_currentAppearanceAssetElementId) as AppearanceAssetElement;
         if (assetElem == null)
            return false;

         Asset asset = assetElem.GetRenderingAsset();
         AssetPropertyDoubleArray4d tintColorProp = asset.FindByName("common_Tint_color") as AssetPropertyDoubleArray4d;
         Color tintColor = tintColorProp.GetValueAsColor();
         Color white = new Color(255, 255, 255);
         return !ColorsEqual(tintColor, white);
      }

      /// <summary>
      /// Check if the button Darker is enabled.
      /// </summary>
      /// <returns>True if the material can be made darker or not.</returns>
      internal bool IsDarkerEnabled()
      {
         if (!SupportTintColor())
            return false;

         AppearanceAssetElement assetElem = m_document.GetElement(this.m_currentAppearanceAssetElementId) as AppearanceAssetElement;
         if (assetElem == null)
            return false;

         Asset asset = assetElem.GetRenderingAsset();
         AssetPropertyDoubleArray4d tintColorProp = asset.FindByName("common_Tint_color") as AssetPropertyDoubleArray4d;
         Color tintColor = tintColorProp.GetValueAsColor();
         Color black = new Color(0, 0, 0);
         return !ColorsEqual(tintColor, black);
      }

      /// <summary>
      ///   A handler for the Idling event.
      /// </summary>
      /// <remarks>
      ///   We keep the handler very simple. First we check
      ///   if we still have the dialog. If not, we unsubscribe from Idling,
      ///   for we no longer need it and it makes Revit speedier.
      ///   If we do have the dialog around, we check if it has a request ready
      ///   and process it accordingly.
      /// </remarks>
      /// 
      public void IdlingHandler(object sender, IdlingEventArgs args)
      {
         UIApplication uiapp = sender as UIApplication;
         UIDocument uidoc = uiapp.ActiveUIDocument;

         if (m_MyForm.IsDisposed)
         {
            uiapp.Idling -= IdlingHandler;
            return;
         }
         else   // dialog still exists
         {
            // fetch the request from the dialog           
            RequestId requestId = m_MyForm.Request.Take();
            if (requestId != RequestId.None)
            {
               try
               {
                  // we take the request, if any was made,
                  // and pass it on to the request executor     
                  RequestHandler.Execute(this, requestId);
               }
               finally
               {
                  // The dialog may be in its waiting state;
                  // make sure we wake it up even if we get an exception.    
                  m_MyForm.EnableButtons(IsLighterEnabled(), IsDarkerEnabled());
               }
            }
         }

         return;
      }

      /// <summary>
      /// Limit value to 0-255
      /// </summary>
      /// <returns>True if the material can be made darker or not.</returns>
      private int LimitValue(int value)
      {
         return (value < 0) ? 0 : (value > 255) ? 255 : value;
      }

      /// <summary>
      ///   The main material-modification subroutine - called from every request 
      /// </summary>
      /// <remarks>
      ///   It searches the current selection for all doors
      ///   and if it finds any it applies the requested operation to them
      /// </remarks>
      /// <param name="text">Caption of the transaction for the operation.</param>
      /// <param name="lighter">Increase the tint color property or not.</param>
      /// 
      internal void ModifySelectedMaterial(String text, bool lighter)
      {
         // Since we'll modify the document, we need a transaction
         // It's best if a transaction is scoped by a 'using' block
         using (Transaction trans = new Transaction(m_document))
         {
            // The name of the transaction was given as an argument  
            if (trans.Start(text) == TransactionStatus.Started)
            {
               // apply the requested operation to every door    
               EditMaterialTintColorProperty(lighter);

               trans.Commit();
            }
         }
      }

      /// <summary>
      /// Edit tint color property.
      /// </summary>  
      /// <param name="lighter">Increase the tint color property or not.</param>
      internal void EditMaterialTintColorProperty(bool lighter)
      {
         using (AppearanceAssetEditScope editScope = new AppearanceAssetEditScope(m_document))
         {
            Asset editableAsset = editScope.Start(m_currentAppearanceAssetElementId);

            AssetPropertyDoubleArray4d metalColorProp = editableAsset.FindByName("common_Tint_color") as AssetPropertyDoubleArray4d;

            Color color = metalColorProp.GetValueAsColor();
            byte red = color.Red;
            byte green = color.Green;
            byte blue = color.Blue;

            // Increment factor  (value related to 255)
            int factor = 25;

            if (lighter)
            {
               red = (byte)LimitValue(red + factor);
               green = (byte)LimitValue(green + factor);
               blue = (byte)LimitValue(blue + factor);
            }
            else
            {
               red = (byte)LimitValue(red - factor);
               green = (byte)LimitValue(green - factor);
               blue = (byte)LimitValue(blue - factor);
            }

            if (metalColorProp.IsValidValue(color))
               metalColorProp.SetValueAsColor(new Color(red, green, blue));

            editScope.Commit(true);
         }

      }

      #endregion
   }
}
