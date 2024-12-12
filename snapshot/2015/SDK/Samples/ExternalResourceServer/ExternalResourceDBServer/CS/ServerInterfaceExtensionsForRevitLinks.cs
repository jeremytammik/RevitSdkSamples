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

using Autodesk.Revit;
using Autodesk.Revit.DB;
namespace Revit.SDK.Samples.ExternalResourceDBServer.CS
{

   /// <summary>
   /// Extension operator to support "Open (and Unload)" command.
   /// Revit will call this method to determine the location of the locally-cached copy
   /// of the linked model, and will open that copy directly.
   /// </summary>
   //=====================================================================================
   public class GetLinkPathForOpen : IGetLocalPathForOpenCallback
   {
      /// <summary>
      /// This implementation simply retrieves the same local file name that the server
      /// uses when first copying the link document to the user's machine.
      /// </summary>
      public string GetLocalPathForOpen(ExternalResourceReference desiredReference)
      {
         return SampleExternalResourceDBServer.GetFullLinkCachedFilePath(desiredReference);
      }
   }



   /// <summary>
   /// Extension operator to support handle updates when the user saves new shared coordinates in the
   /// local copy of a link.
   /// </summary>
   //=====================================================================================
   public class LocalLinkSharedCoordinatesSaved : IOnLocalLinkSharedCoordinatesSavedCallback
   {
      /// <summary>
      /// When the user updates the shared coordinates in the link model, Revit calls this
      /// method.  In this implementation, the updated local version of the link on the user's
      /// machine is uploaded (copied) back to the server location.
      /// </summary>
      public void OnLocalLinkSharedCoordinatesSaved(ExternalResourceReference changedReference)
      {
         string localLinkPath = SampleExternalResourceDBServer.GetFullLinkCachedFilePath(changedReference);
         string fullServerPath = SampleExternalResourceDBServer.GetFullServerLinkFilePath(changedReference);
         String serverDirectoryName = System.IO.Path.GetDirectoryName(fullServerPath);
         if (!System.IO.Directory.Exists(serverDirectoryName))
         {
            System.IO.Directory.CreateDirectory(serverDirectoryName);
         }
         System.IO.File.Copy(localLinkPath, fullServerPath, true);  // Overwrite
      }
   }

}
