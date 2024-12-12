//
// (C) Copyright 2003-2020 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using System.IO;
using System.Reflection;

namespace Revit.SDK.Samples.InCanvasControlAPI.CS
{
   /// <summary>
   /// Provider for string resources
   /// </summary>
   public class ResourceProvider
   {
      #region Class implementation

      private ResourceProvider()
      {
         issueImage = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "\\issue.bmp";
         selectedIssueImage = new FileInfo(Assembly.GetExecutingAssembly().Location).DirectoryName + "\\selected.bmp";
      }

      /// <summary>
      /// Gets the string resource provider
      /// </summary>
      /// <returns>Instance of the provider</returns>
      public static ResourceProvider GetInstance()
      {
         if (provider == null)
         {
            provider = new ResourceProvider();
         }
         return provider;
      }

      /// <summary>
      /// Path to marker's bitmap for unselected issues
      /// </summary>
      public string IssueImage { get => issueImage; }

      /// <summary>
      /// Path to marker's bitmap for selected issues
      /// </summary>
      public string SelectedIssueImage { get => selectedIssueImage; }

      #endregion

      #region Class member variables

      private string issueImage;
      private string selectedIssueImage;
      private static ResourceProvider provider;

      #endregion
   }
}
