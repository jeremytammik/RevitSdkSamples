//
// (C) Copyright 2003-2016 by Autodesk, Inc.
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
using System.Threading;

namespace Revit.SDK.Samples.ModelessForm_ExternalEvent.CS
{
   /// <summary>
   ///   A list of requests the dialog has available
   /// </summary>
   /// 
   public enum RequestId : int
   {
       /// <summary>
       /// None
       /// </summary>
       None = 0,
       /// <summary>
       /// "Delete" request
       /// </summary>
       Delete = 1,
       /// <summary>
       /// "FlipLeftRight" request
       /// </summary>
       FlipLeftRight = 2,
       /// <summary>
       /// "FlipInOut" request
       /// </summary>
       FlipInOut = 3,
       /// <summary>
       /// "MakeRight" request
       /// </summary>
       MakeRight = 4,
       /// <summary>
       /// "MakeLeft" request
       /// </summary>
       MakeLeft = 5,
       /// <summary>
       /// "TurnOut" request
       /// </summary>
       TurnOut = 6,
       /// <summary>
       /// "TurnIn" request
       /// </summary>
       TurnIn = 7,
       /// <summary>
       /// "Rotate" request
       /// </summary>
       Rotate = 8
   }

   /// <summary>
   ///   A class around a variable holding the current request.
   /// </summary>
   /// <remarks>
   ///   Access to it is made thread-safe, even though we don't necessarily
   ///   need it if we always disable the dialog between individual requests.
   /// </remarks>
   /// 
   public class Request
   {
      // Storing the value as a plain Int makes using the interlocking mechanism simpler
      private int m_request = (int)RequestId.None;

      /// <summary>
      ///   Take - The Idling handler calls this to obtain the latest request. 
      /// </summary>
      /// <remarks>
      ///   This is not a getter! It takes the request and replaces it
      ///   with 'None' to indicate that the request has been "passed on".
      /// </remarks>
      /// 
      public RequestId Take()
      {
         return (RequestId)Interlocked.Exchange(ref m_request, (int)RequestId.None);
      }

      /// <summary>
      ///   Make - The Dialog calls this when the user presses a command button there. 
      /// </summary>
      /// <remarks>
      ///   It replaces any older request previously made.
      /// </remarks>
      /// 
      public void Make(RequestId request)
      {
         Interlocked.Exchange(ref m_request, (int)request);
      }
   }
}
