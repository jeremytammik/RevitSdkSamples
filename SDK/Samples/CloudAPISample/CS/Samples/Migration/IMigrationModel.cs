//
// (C) Copyright 2003-2023 by Autodesk, Inc. All rights reserved.
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

using System.Collections.ObjectModel;

namespace Revit.SDK.Samples.CloudAPISample.CS.Migration
{
   /// <summary>
   ///    Represents a folder on BIM360 site.
   /// </summary>
   public class FolderLocation
   {
      /// <summary>
      ///    Folder's name
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      ///    Folder's urn
      /// </summary>
      public string Urn { get; set; }
   }

   /// <summary>
   ///    Indicates where to migrate models
   /// </summary>
   public class MigrationRule
   {
      /// <summary>
      ///    The search string to match against the names of files, but it doesn't support regular expression.
      /// </summary>
      public string Pattern { get; set; }

      /// <summary>
      ///    The target folder on BIM360
      /// </summary>
      public FolderLocation Target { get; set; }
   }

   /// <summary>
   ///    Interface of a view model for migration sample.
   /// </summary>
   public interface IMigrationModel
   {
      /// <summary>
      ///    Indicates the target BIM360 account guid
      /// </summary>
      string AccountGuid { get; set; }

      /// <summary>
      ///    Indicates the target BIM360 project guid
      /// </summary>
      string ProjectGuid { get; set; }

      /// <summary>
      ///    Provide all candidate folders which we can upload to.
      /// </summary>
      ObservableCollection<FolderLocation> AvailableFolders { get; set; }

      /// <summary>
      ///    The collection for <see cref="MigrationRule" />.
      ///    Items with lower index in collection have higher priority.
      /// </summary>
      ObservableCollection<MigrationRule> Rules { get; set; }
   }
}