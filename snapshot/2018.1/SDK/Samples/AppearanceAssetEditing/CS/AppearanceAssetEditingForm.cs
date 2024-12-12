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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Revit.SDK.Samples.AppearanceAssetEditing.CS
{
   /// <summary>
   /// The class of our modeless dialog.
   /// </summary>
   public partial class AppearanceAssetEditingForm : Form
   {
      /// <summary>
      /// In this sample, the dialog owns the value of the request but it is not necessary. It may as
      /// well be a static property of the application.
      /// </summary>
      private Request m_request;

      /// <summary>
      /// Request property
      /// </summary>
      public Request Request
      {
         get
         {
            return m_request;
         }
         private set
         {
            m_request = value;
         }
      }

      /// <summary>
      /// Constructor
      /// </summary>
      public AppearanceAssetEditingForm()
      {
         InitializeComponent();

         m_request = new Request();
      }

      private void buttonSelect_Click(object sender, EventArgs e)
      {
         MakeRequest(RequestId.Select);
      }

      private void buttonLighter_Click(object sender, EventArgs e)
      {
         MakeRequest(RequestId.Lighter);
      }

      private void buttonDarker_Click(object sender, EventArgs e)
      {
         MakeRequest(RequestId.Darker);  
      }

      /// <summary>
      /// Enable buttons or not.
      /// </summary>
      public void EnableButtons(bool bLighterStatus, bool bDarkerStatus)
      {
         buttonLighter.Enabled = bLighterStatus;
         buttonDarker.Enabled = bDarkerStatus;
         buttonSelect.Enabled = true;
      }

      /// <summary>
      ///   A private helper method to make a request
      ///   and put the dialog to sleep at the same time.
      /// </summary>
      /// <remarks>
      ///   It is expected that the process which executes the request 
      ///   (the Idling helper in this particular case) will also
      ///   wake the dialog up after finishing the execution.
      /// </remarks>
      ///
      private void MakeRequest(RequestId request)
      {
         Request.Make(request);
         EnableButtons(false, false);
      }
   }
}
