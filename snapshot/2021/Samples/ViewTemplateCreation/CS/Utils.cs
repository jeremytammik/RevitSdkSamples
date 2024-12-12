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

namespace Revit.SDK.Samples.ViewTemplateCreation.CS
{
   /// <summary>
   /// Utils class contains useful methods and members for using in whole project.
   /// </summary>
   public class Utils
   {
      /// <summary>
      /// Shows regular message box with warning icon and OK button
      /// </summary>
      public static void ShowWarningMessageBox(string message)
      {
         System.Windows.Forms.MessageBox.Show(
            message,
            SampleName,
            System.Windows.Forms.MessageBoxButtons.OK,
            System.Windows.Forms.MessageBoxIcon.Warning);
      }

      /// <summary>
      /// Shows regular message box with information icon and OK button
      /// </summary>
      public static void ShowInformationMessageBox(string message)
      {
         System.Windows.Forms.MessageBox.Show(
            message,
            SampleName,
            System.Windows.Forms.MessageBoxButtons.OK,
            System.Windows.Forms.MessageBoxIcon.Information);
      }

      /// <summary>
      /// Contains a name of this sample
      /// </summary>
      public const string SampleName = "View Template Creation sample";
   }
}
