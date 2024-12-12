//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.UI;
using System.Diagnostics;

namespace Revit.SDK.Samples.AppearanceAssetEditing.CS
{
   /// <summary>
   ///   A class with methods to execute requests made by the dialog user.
   /// </summary>
   /// 
   public static class RequestHandler
   {

      /// <summary>
      ///   The top function that distributes requests to individual methods. 
      /// </summary>
      /// 
      public static void Execute(Application app, RequestId request)
      {
         switch (request)
         {
            case RequestId.None:
            {
               return;  // no request at this time -> we can leave immediately
            }
            case RequestId.Select:
            {
               app.GetPaintedMaterial();
               break;
            }
            case RequestId.Lighter:
            {
               app.ModifySelectedMaterial("Lighter", true);
               break;
            }
            case RequestId.Darker:
            {
               app.ModifySelectedMaterial("Darker", false);
               break;
            }                     
            default:
            {
               // some kind of a warning here should
               // notify us about an unexpected request 
               break;
            }
         }

         return;
      }             


   }  // class
}
