//
// (C) Copyright 2003-2007 by Autodesk, Inc.
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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.Elements;
using Autodesk.Revit.Symbols;
using Autodesk.Revit.Structural.Enums;

namespace Revit.SDK.Samples.Reinforcement.CS
{
   /// <summary>
   /// The factory to create the corresponding FrameReinMaker, such as BeamFramReinMaker.
   /// </summary>
   class FrameReinMakerFactory
   {
      // Private members
      ExternalCommandData m_commandData;  // the ExternalCommandData reference
      FamilyInstance m_hostObject;        // the host object

      /// <summary>
      /// constructor
      /// </summary>
      /// <param name="commandData">the ExternalCommandData reference</param>
      public FrameReinMakerFactory(ExternalCommandData commandData)
      {
         m_commandData = commandData;

         if (!GetHostObject())
         {
            throw new Exception("Please select a beam or column.");
         }
      }


      /// <summary>
      /// check the condition of host object and see whether the rebars can be placed on
      /// </summary>
      /// <returns></returns>
      public bool AssertData()
      {
         // Check it as beam or column
         StructuralType frameType = m_hostObject.StructuralType; // use StructuralType to check
         if (!frameType.Equals(StructuralType.Beam) && !frameType.Equals(StructuralType.Column))
         {
            return false;
         }

         // check the material to be concrete or not
         if (!m_hostObject.Material.Equals(Autodesk.Revit.Structural.Enums.Material.Concrete))
         {
            return false;
         }

         // judge whether is any rebar exist in the beam or column
         ElementIterator iter = m_commandData.Application.ActiveDocument.Elements;
         iter.Reset();
         while (iter.MoveNext())
         {
            Rebar bar = iter.Current as Rebar;
            if (null == bar)
            {
               continue;
            }
            if (bar.Host.Id.Value == m_hostObject.Id.Value)
            {
               return false;
            }
         }

         return true;
      }

      /// <summary>
      /// The main method which create the corresponding FrameReinMaker according to 
      /// the host object type, and invoke Run() method to create reinforcement rebars
      /// </summary>
      /// <returns>true if the creation is successful, otherwise false</returns>
      public bool work()
      {
         // define an IFrameReinMaker interface to create reinforcement rebars
         IFrameReinMaker maker = null;

         // create FrameReinMaker instance according to host object type
         switch (m_hostObject.StructuralType)
         {
            case StructuralType.Beam:   // if host object is a beam
               maker = new BeamFramReinMaker(m_commandData, m_hostObject);
               break;
            case StructuralType.Column: // if host object is a column
               maker = new ColumnFramReinMaker(m_commandData, m_hostObject);
               break;
            default:
               break;
         }

         // invoke Run() method to do the reinforcement creation
         maker.Run();

         return true;
      }


      /// <summary>
      /// Get the selected element as the host object.
      /// </summary>
      /// <returns>true if get the selected element, otherwise false.</returns>
      private bool GetHostObject()
      {
         // Get the selected element set, and check only one element is selected
         ElementSet selected = m_commandData.Application.ActiveDocument.Selection.Elements;
         if (1 != selected.Size)
         {
            return false;
         }

         // Get the selected element, it should be a family instance, such beam or column
         foreach (object o in selected)
         {
            m_hostObject = o as FamilyInstance;
            if (null == m_hostObject)
            {
               return false;
            }
         }
         return true;
      }
   }
}
