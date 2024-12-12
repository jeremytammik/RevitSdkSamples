//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.CurtainSystem.CS.Data
{
   /// <summary>
   /// maintains all the data used in the sample
   /// </summary>
   public class MyDocument
   {
      // object which contains reference of Revit Applicatio
      ExternalCommandData m_commandData;
      /// <summary>
      /// object which contains reference of Revit Applicatio
      /// </summary>
      public ExternalCommandData CommandData
      {
         get
         {
            return m_commandData;
         }
         set
         {
            m_commandData = value;
         }
      }

      // the active UI document of Revit
      Autodesk.Revit.UI.UIDocument m_uiDocument;

      /// <summary>
      /// the active document of Revit
      /// </summary>
      public Autodesk.Revit.UI.UIDocument UIDocument
      {
         get
         {
            return m_uiDocument;
         }
      }

      Autodesk.Revit.DB.Document m_document;
      public Autodesk.Revit.DB.Document Document
      {
         get { return m_document; }
      }

      // the data of the created curtain systems
      CurtainSystem.SystemData m_systemData;
      /// <summary>
      /// the data of the created curtain systems
      /// </summary>
      public CurtainSystem.SystemData SystemData
      {
         get
         {
            return m_systemData;
         }
         set
         {
            m_systemData = value;
         }
      }

      // all the faces of  the parallelepiped mass
      FaceArray m_massFaceArray;
      /// <summary>
      /// // all the faces of  the parallelepiped mass
      /// </summary>
      public FaceArray MassFaceArray
      {
         get
         {
            return m_massFaceArray;
         }
         set
         {
            m_massFaceArray = value;
         }
      }

      // the curtain system type of the active Revit document, used for curtain system creation
      CurtainSystemType m_curtainSystemType;
      /// <summary>
      /// the curtain system type of the active Revit document, used for curtain system creation
      /// </summary>
      public CurtainSystemType CurtainSystemType
      {
         get
         {
            return m_curtainSystemType;
         }
         set
         {
            m_curtainSystemType = value;
         }
      }
      // the message shown when there's a fatal error in the sample
      string m_fatalErrorMsg = null;
      /// <summary>
      /// the message shown when there's a fatal error in the sample
      /// </summary>
      public string FatalErrorMsg
      {
         get
         {
            return m_fatalErrorMsg;
         }
         set
         {
            m_fatalErrorMsg = value;

            if (false == string.IsNullOrEmpty(m_fatalErrorMsg) &&
                null != FatalErrorEvent)
            {
               FatalErrorEvent(m_fatalErrorMsg);
            }
         }
      }

      /// <summary>
      /// occurs only when the message was updated
      /// the delegate method to handle the message update event
      /// </summary>
      public delegate void MessageChangedHandler();
      /// <summary>
      /// the event triggered when message updated/changed
      /// </summary>
      public event MessageChangedHandler MessageChanged;

      /// <summary>
      /// occurs only when there's a fatal error
      /// the delegate method to handle the fatal error event
      /// </summary>
      /// <param name="errorMsg"></param>
      public delegate void FatalErrorHandler(string errorMsg);
      /// <summary>
      /// the event triggered when the sample meets a fatal error
      /// </summary>
      public event FatalErrorHandler FatalErrorEvent;

      // store the message of the sample
      private KeyValuePair<string/*msgText*/, bool/*is warningOrError*/> m_message;
      /// <summary>
      /// store the message of the sample
      /// </summary>
      public KeyValuePair<string/*msgText*/, bool/*is warningOrError*/> Message
      {
         get
         {
            return m_message;
         }
         set
         {
            m_message = value;
            if (null != MessageChanged)
            {
               MessageChanged();
            }
         }
      }

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="commandData">
      /// object which contains reference of Revit Application
      /// </param>
      public MyDocument(ExternalCommandData commandData)
      {
         m_commandData = commandData;
         m_uiDocument = m_commandData.Application.ActiveUIDocument;
         m_document = m_uiDocument.Document;

         // initialize the curtain system data
         m_systemData = new CurtainSystem.SystemData(this);

         // get the curtain system type of the active Revit document
         GetCurtainSystemType();
      }

      /// <summary>
      /// get the curtain system type from the active Revit document
      /// </summary>
      private void GetCurtainSystemType()
      {
         CurtainSystemTypeSet types = m_document.CurtainSystemTypes;
         foreach (CurtainSystemType type in types)
         {
            if (null == type)
            {
               continue;
            }

            m_curtainSystemType = type;
            break;
         }
      }

   } // end of class
}
