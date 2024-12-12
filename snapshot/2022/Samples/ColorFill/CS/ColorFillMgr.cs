//
// (C) Copyright 2003-2020 by Autodesk, Inc.
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
using Autodesk.Revit.UI;


namespace Revit.SDK.Samples.ColorFill.CS
{
   /// <summary>
   /// This is a helper class to deal with the color fill
   /// </summary>
   public class ColorFillMgr
   {
      private Document document;
      private ExternalCommandData externalCommandData;
      private ElementId schemeCategoryId;
      private FilteredElementCollector fec;
      private List<ElementId> levelIds = new List<ElementId>();
      private List<ElementId> fillPatternIds = new List<ElementId>();
      private List<ElementId> parameterDefinitions = new List<ElementId>();
      /// <summary>
      /// Construction
      /// </summary>
      /// <param name="doc">Revit document that will be dealt with</param>
      /// <param name="commandData"></param>
      public ColorFillMgr(Document doc, ExternalCommandData commandData)
      {
         document = doc;
         fec = new FilteredElementCollector(document);
         schemeCategoryId = new ElementId(BuiltInCategory.OST_Rooms);
         externalCommandData = commandData;
      }
        /// <summary>
        /// All room schemes from document
        /// </summary>
        public List<ColorFillScheme> RoomSchemes { get; private set; }
        /// <summary>
        /// DB Views from the document
        /// </summary>
        public List<Autodesk.Revit.DB.View> Views { get; private set; } 

      /// <summary>
      /// Get Data from document
      /// </summary>
      public void RetrieveData()
      {
         RoomSchemes = fec.OfClass(typeof(ColorFillScheme))
                          .OfType<ColorFillScheme>()
                          .Where(s => s.CategoryId == schemeCategoryId).ToList();
         fillPatternIds = new FilteredElementCollector(document)
                .OfClass(typeof(FillPatternElement))
                .OfType<FillPatternElement>()
                .Where(fp => fp.GetFillPattern().Target == FillPatternTarget.Drafting)
                .Select(f => f.Id)
                .ToList();
         //Select all floor views and elevation views.
         Views = new FilteredElementCollector(document)
                   .OfClass(typeof(Autodesk.Revit.DB.View))
                   .OfType<Autodesk.Revit.DB.View>().
                   Where(v => !v.IsTemplate && 
                   (v.ViewType == ViewType.FloorPlan || v.ViewType==ViewType.Elevation)).
                   ToList();
      }
      /// <summary>
      /// Duplicate a color fill scheme based on an existing one.
      /// </summary>
      /// <param name="scheme">The color fill which that is duplicated.</param>
      /// <param name="schemeName">Name for new color fill scheme.</param>
      /// <param name="schemeTitle">Title for new color fill scheme.</param>
      /// <returns></returns>
      public void DuplicateScheme(ColorFillScheme scheme, string schemeName, string schemeTitle)
      {
         ElementId parameterId = new ElementId(BuiltInParameter.AREA_SCHEME_NAME);
         using (Transaction tr = new Transaction(document))
         {
            tr.Start("CopyScheme");
            ElementId newSchemeId = scheme.Duplicate(schemeName);
            ColorFillScheme newScheme = scheme.Document.GetElement(newSchemeId) as ColorFillScheme;
            newScheme.Title = schemeTitle;
            parameterDefinitions = newScheme.GetSupportedParameterIds().ToList<ElementId>();
            if (parameterDefinitions.Contains(parameterId))
               newScheme.ParameterDefinition = parameterId;
            tr.Commit();
         }
      }

      /// <summary>
      /// Create color legend on view with the specific color fill scheme
      /// </summary>
      /// <param name="scheme"></param>
      /// <param name="view"></param>
      public void CreateAndPlaceLegend(ColorFillScheme scheme, Autodesk.Revit.DB.View view)
      {
         using (Transaction transaction = new Transaction(document))
         {
            transaction.Start("Create legend");
            var origin = view.Origin.Add(view.UpDirection.Multiply(20));
           
            if (view.CanApplyColorFillScheme(schemeCategoryId, scheme.Id))
            {
               view.SetColorFillSchemeId(schemeCategoryId, scheme.Id);
               var legend = ColorFillLegend.Create(document, view.Id, schemeCategoryId, origin);
               legend.Height = legend.Height / 2;
               transaction.Commit();
            }
            else
            {
               throw new Exception("The scheme can not be applied on the view.");
            }
         }
      }

       /// <summary>
      /// Make modify to existing color fill scheme
      /// </summary>
      /// <param name="scheme"></param>
      public void ModifyByValueScheme(ColorFillScheme scheme)
      {
         List<ColorFillSchemeEntry> entries = scheme.GetEntries().ToList();
         List<ColorFillSchemeEntry> newEntries = new List<ColorFillSchemeEntry>();
         StorageType storageType = entries[0].StorageType;
         Random random = new Random();
         int seed = random.Next(0, 256);
         foreach(var entry in entries)
         {
            seed++;
            ColorFillSchemeEntry newEntry = CreateEntry(scheme, storageType, entry.FillPatternId, GenerateRandomColor(seed));
            switch (storageType)
            {
               case StorageType.Double:
                  newEntry.SetDoubleValue(entry.GetDoubleValue());
                  break;
               case StorageType.Integer:
                  newEntry.SetIntegerValue(entry.GetIntegerValue());
                  break;
               case StorageType.String:
                  newEntry.SetStringValue(entry.GetStringValue());
                  break;
               case StorageType.ElementId:
                  newEntry.SetElementIdValue(entry.GetElementIdValue());
                  break;
               default:
                  break;
            }

            newEntries.Add(newEntry);
         }
         using (Transaction tr = new Transaction(document))
         {
            tr.Start("update entries");
            scheme.SetEntries(newEntries);
            tr.Commit();
         }
         externalCommandData.Application.ActiveUIDocument.RefreshActiveView();
      }

      private Color GenerateRandomColor(int seed)
      {
         Random r = new Random(seed);
         byte red = Byte.Parse(r.Next(0, 256).ToString());
         byte green = Byte.Parse(r.Next(0, 256).ToString());
         byte blue = Byte.Parse(r.Next(0, 256).ToString());
         Color randomColor = new Color(red, green, blue);
         return randomColor;
      }

      private ColorFillSchemeEntry CreateEntry(ColorFillScheme scheme, StorageType type, ElementId fillPatternId, Color color)
      {
         var entries = scheme.GetEntries();
         bool isbyRange = scheme.IsByRange;
         ColorFillSchemeEntry lastEntry = null;
         if (entries.Count > 0)
            lastEntry = entries.Last();

         var entry = new ColorFillSchemeEntry(type);
         entry.FillPatternId = fillPatternId;

         switch (type)
         {
            case Autodesk.Revit.DB.StorageType.Double:
               double doubleValue = 0;
               if (lastEntry != null)
                  doubleValue = lastEntry.GetDoubleValue() + 20.00;
               entry.SetDoubleValue(doubleValue);
               break;
            case Autodesk.Revit.DB.StorageType.String:
               string strValue = string.Format("New entry {0}", entries.Count);
               entry.SetStringValue(strValue);
               break;
            case Autodesk.Revit.DB.StorageType.Integer:
               int intValue = 0;
               if (lastEntry != null)
                  intValue = lastEntry.GetIntegerValue() + 20;
               entry.SetIntegerValue(intValue);
               break;
            case Autodesk.Revit.DB.StorageType.ElementId:
               var level = new FilteredElementCollector(document)
                .OfClass(typeof(Level))
                .Where(lv => !levelIds.Contains(lv.Id) && lv.Name != "Level 1")
                .FirstOrDefault();
               levelIds.Add(level.Id);
               entry.SetElementIdValue(level.Id);
               break;
            default:
               throw (new Exception("The type is not correct!"));
         }

         return entry;
      }

   }

}

