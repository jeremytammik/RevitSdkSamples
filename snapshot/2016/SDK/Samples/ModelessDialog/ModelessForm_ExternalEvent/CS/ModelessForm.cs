//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using System.Windows.Forms;
using Autodesk.Revit.UI;


namespace Revit.SDK.Samples.ModelessForm_ExternalEvent.CS
{
   /// <summary>
   /// The class of our modeless dialog.
   /// </summary>
   /// <remarks>
   /// Besides other methods, it has one method per each command button.
   /// In each of those methods nothing else is done but raising an external
   /// event with a specific request set in the request handler.
   /// </remarks>
   /// 
   public partial class ModelessForm : Form
   {
      // In this sample, the dialog owns the handler and the event objects,
      // but it is not a requirement. They may as well be static properties
      // of the application.

      private RequestHandler m_Handler;
      private ExternalEvent m_ExEvent;


      /// <summary>
      ///   Dialog instantiation
      /// </summary>
      /// 
      public ModelessForm(ExternalEvent exEvent, RequestHandler handler)
      {
         InitializeComponent();
         m_Handler = handler;
         m_ExEvent = exEvent;
      }

       /// <summary>
       /// Form closed event handler
       /// </summary>
       /// <param name="e"></param>
      protected override void OnFormClosed(FormClosedEventArgs e)
      {
         // we own both the event and the handler
         // we should dispose it before we are closed
         m_ExEvent.Dispose();
         m_ExEvent = null;
         m_Handler = null;
         
         // do not forget to call the base class
         base.OnFormClosed(e);
      }


      /// <summary>
      ///   Control enabler / disabler 
      /// </summary>
      ///
      private void EnableCommands(bool status)
      {
         foreach (Control ctrl in this.Controls)
         {
            ctrl.Enabled = status;
         }
         if (!status)
         {
            this.btnExit.Enabled = true;
         }
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
         m_Handler.Request.Make( request );
         m_ExEvent.Raise();
         DozeOff();
      }


      /// <summary>
      ///   DozeOff -> disable all controls (but the Exit button)
      /// </summary>
      /// 
      private void DozeOff()
      {
         EnableCommands(false);
      }


      /// <summary>
      ///   WakeUp -> enable all controls
      /// </summary>
      /// 
      public void WakeUp()
      {
         EnableCommands(true);
      }


      /// <summary>
      ///   Exit - closing the dialog
      /// </summary>
      /// 
      private void btnExit_Click(object sender, EventArgs e)
      {
         Close();
      }


      /// <summary>
      ///   Making a door Left
      /// </summary>
      /// 
      private void btnFlipLeft_Click(object sender, EventArgs e)
      {
         MakeRequest( RequestId.MakeLeft );
      }


      /// <summary>
      ///   Making a door Right
      /// </summary>
      /// 
      private void btnFlipRight_Click(object sender, EventArgs e)
      {
         MakeRequest( RequestId.MakeRight );
      }


      /// <summary>
      ///   Flipping a door between Right and Left
      /// </summary>
      /// 
      private void btnFlipLeftRight_Click(object sender, EventArgs e)
      {
         MakeRequest( RequestId.FlipLeftRight );
      }


      /// <summary>
      ///   Flipping a door between facing In and Out
      /// </summary>
      /// 
      private void btnFlipInOut_Click(object sender, EventArgs e)
      {
         MakeRequest( RequestId.FlipInOut );
      }


      /// <summary>
      ///   Turning a door to face Out
      /// </summary>
      /// 
      private void btnFlipOut_Click(object sender, EventArgs e)
      {
         MakeRequest( RequestId.TurnOut );
      }


      /// <summary>
      ///   Turning a door to face In
      /// </summary>
      /// 
      private void btnFlipIn_Click(object sender, EventArgs e)
      {
         MakeRequest( RequestId.TurnIn );
      }


      /// <summary>
      ///   Turning a door around - flipping both hand and face
      /// </summary>
      /// 
      private void btnRotate_Click(object sender, EventArgs e)
      {
         MakeRequest( RequestId.Rotate );
      }


      /// <summary>
      ///   Deleting a door
      /// </summary>
      /// 
      private void btnDelete_Click(object sender, EventArgs e)
      {
         MakeRequest( RequestId.Delete );
      }

   }  // class

}
