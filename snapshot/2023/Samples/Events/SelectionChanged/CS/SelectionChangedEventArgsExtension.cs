//
// (C) Copyright 2003-2021 by Autodesk, Inc.
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
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit.SDK.Samples.SelectionChanged.CS
{
   /// <summary>
   /// This class is used to extend SelectionChangedEventArgs with GetInfo method
   /// </summary>
   public static class SelectionChangedEventArgsExtension
   {
      /// <summary>
      /// Get SelectionChangedEvent information
      /// </summary>
      /// <param name="args">Event arguments that contains the event data.</param>
      /// <param name="doc">document in which the event occurs.</param>
      public static string GetInfo(this SelectionChangedEventArgs args)
      {
         StringBuilder sb = new StringBuilder();
         sb.AppendLine();

         Document doc = args.GetDocument();
         sb.AppendLine("[Event] " + GetEventName(args.GetType()) + ": " + TitleNoExt(doc.Title));


         IList<Reference> refs = args.GetReferences();
         sb.AppendLine("Selection Count:" + refs.Count);

         foreach (var aRef in refs)
         {
            //foreach (PropertyInfo pi in aRef.GetType().GetProperties())
            //{
            //   Trace.WriteLine(pi.Name + ":" + pi.GetValue(aRef, null)?.ToString());
            //}

            switch (aRef.ElementReferenceType)
            {
               case ElementReferenceType.REFERENCE_TYPE_NONE:
                  {
                     sb.AppendFormat("The reference is to an element. ElementId:{0}.", aRef.ElementId);
                     if (aRef.LinkedElementId != ElementId.InvalidElementId)
                     {
                        sb.AppendFormat("LinkedElementId:{0}", aRef.LinkedElementId);
                     }
                     break;
                  }
               case ElementReferenceType.REFERENCE_TYPE_LINEAR:
                  {
                     sb.AppendFormat("The reference is to a curve or edge. ElementId:{0}.", aRef.ElementId);
                     break;
                  }
               case ElementReferenceType.REFERENCE_TYPE_SURFACE:
                  {
                     sb.AppendFormat("The reference is to a face or face region. ElementId:{0}.", aRef.ElementId);
                     break;
                  }
               case ElementReferenceType.REFERENCE_TYPE_FOREIGN:
                  {
                     sb.AppendFormat("The reference is to geometry or elements in linked Revit file. LinkedElementId:{0}.", aRef.LinkedElementId);
                     break;
                  }
               case ElementReferenceType.REFERENCE_TYPE_INSTANCE:
                  {
                     sb.AppendFormat("The reference is an instance of a symbol. ElementId:{0}.", aRef.ElementId);
                     break;
                  }
               case ElementReferenceType.REFERENCE_TYPE_CUT_EDGE:
                  {
                     sb.AppendFormat("The reference is to a face that was cut. ElementId:{0}.", aRef.ElementId);
                     break;
                  }
               case ElementReferenceType.REFERENCE_TYPE_MESH:
                  {
                     sb.AppendFormat("The reference is to a mesh. ElementId:{0}.", aRef.ElementId);
                     break;
                  }
               case ElementReferenceType.REFERENCE_TYPE_SUBELEMENT:
                  {
                     sb.AppendFormat("The reference is to a subelement. ElementId:{0}.", aRef.ElementId);
                     break;
                  }
               default:
                  {
                     sb.Append("Unknown reference type.");
                     break;
                  }

            }
            
            
            Element elem = doc.GetElement(aRef.ElementId);
            if(elem!= null)
            {
               sb.AppendFormat(" Name:{0}.",elem.Name);
            }
            
            sb.AppendLine();
         }

         return sb.ToString();

      }

      /// <summary>
      /// Get event name from its EventArgs, without namespace prefix
      /// </summary>
      /// <param name="type">Generic event arguments type.</param>
      /// <returns>the event name</returns>
      private static string GetEventName(Type type)
      {
         String argName = type.ToString();
         String tail = "EventArgs";
         String head = "Autodesk.Revit.UI.Events.";
         int firstIndex = head.Length;
         int length = argName.Length - head.Length - tail.Length;
         String eventName = argName.Substring(firstIndex, length);
         return eventName;
      }

      /// <summary>
      /// This method will remove the extension of the file name (if it has an extension).
      /// 
      /// Document.Title will return title of project depends on OS setting:
      /// If we choose show extension name by IE:Tools\Folder Options, then the title will end with accordingly extension name.
      /// If we don't show extension, the Document.Title will only return file name without extension.
      /// </summary>
      /// <param name="orgTitle">Origin file name to be revised.</param>
      /// <returns>New file name without extension name.</returns>
      private static string TitleNoExt(String orgTitle)
      {
         // return null directly if it's null
         if (String.IsNullOrEmpty(orgTitle))
         {
            return "";
         }

         // Remove the extension 
         int pos = orgTitle.LastIndexOf('.');
         if (-1 != pos)
         {
            return orgTitle.Remove(pos);
         }
         else
         {
            return orgTitle;
         }
      }
   }
}
