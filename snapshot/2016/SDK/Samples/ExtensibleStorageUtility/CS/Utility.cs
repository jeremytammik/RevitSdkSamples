//
// (C) Copyright 2003-2015 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notify appears in all copies and
// that both that copyright notify and the limited warranty and
// restricted rights notify below appear in all supporting
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
using System.Diagnostics;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.ExtensibleStorage;



namespace Revit.SDK.Samples.ExtensibleStorageUtility.CS
{
   public class StorageUtility
   {
      /// <summary>
      /// Returns true if any extensible storage exists in the document, false otherwise.
      /// </summary>
      public static bool DoesAnyStorageExist(Document doc)
      {
         IList<Schema> schemas = Schema.ListSchemas();
         if (schemas.Count == 0)
            return false;
         else
         {
            foreach (Schema schema in schemas)
            {
               List<ElementId> ids = ElementsWithStorage(doc, schema);
               if (ids.Count > 0)
                  return true;
            }
            return false;
         }
      }


      /// <summary>
      /// Returns a formatted string containing schema guids and element info for all elements
      /// containing extensible storage.
      /// </summary>
      public static string GetElementsWithAllSchemas(Document doc)
      {
         StringBuilder sBuilder = new StringBuilder();
         IList<Schema> schemas = Schema.ListSchemas();
         if (schemas.Count == 0)
            return "No schemas or storage.";
         else
         {
            foreach (Schema schema in schemas)
            {
               sBuilder.Append(StorageUtility.GetElementsWithSchema(doc, schema));
            }
            return sBuilder.ToString();
         }
      }

      /// <summary>
      /// Returns a formatted string containing a schema guid and element info for all elements
      /// containing extensible storage of a given schema.
      /// </summary>
      private static string GetElementsWithSchema(Document doc, Schema schema)
      {
         StringBuilder sBuilder = new StringBuilder();
         sBuilder.AppendLine("Schema: " + schema.GUID.ToString() + ", " + schema.SchemaName);
         List<ElementId> elementsofSchema = ElementsWithStorage(doc, schema);
         if (elementsofSchema.Count == 0)
            sBuilder.AppendLine("No elements.");
         else
         {
            foreach (ElementId id in elementsofSchema)
            {
               sBuilder.AppendLine(PrintElementInfo(id, doc));
            }
         }
         return sBuilder.ToString();
      }

      /// <summary>
      /// Returns a list of ElementIds that contain extensible storage of a given schema using
      /// the ExtensibleStorageFilter ElementQuickFilter.
      /// </summary>
      private static List<ElementId> ElementsWithStorage(Document doc, Schema schema)
      {
         List<ElementId> ids = new List<ElementId>();
         FilteredElementCollector collector = new FilteredElementCollector(doc);
         collector.WherePasses(new ExtensibleStorageFilter(schema.GUID));
         ids.AddRange(collector.ToElementIds());
         return ids;
      }

      /// <summary>
      /// Writes basic element info to a string.
      /// </summary>
      private static string PrintElementInfo(ElementId id, Document document)
      {
         Element element = document.GetElement(id);
         string retval = (element.Id.ToString() + ", " + element.Name + ", " + element.GetType().FullName);
         Debug.WriteLine(retval);
         return retval;
      }

   }
}
