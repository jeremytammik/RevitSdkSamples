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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.ApplicationServices;


namespace Revit.SDK.Samples.DoorSwing.CS
{
   /// <summary>
   /// Left/Right feature based on family's actual geometry and country's standard.
   /// </summary>
   public class DoorFamily
   {
      #region "Members"

      // door family
      Family m_family;
      // opening value of one of this family's door which neither flipped nor mirrored.
      string m_basalOpeningValue; 
      // one door instance of this family.
      FamilyInstance m_oneInstance;
      // Revit application
      UIApplication m_app;
      // the geometry of one of this family's door which neither flipped nor mirrored.
      DoorGeometry m_geometry; 

      #endregion

      #region "Properties"

      /// <summary>
      /// Retrieval the name of this family.
      /// </summary>
      public string FamilyName
      {
         get
         {
            return m_family.Name;
         }
      }

      /// <summary>
      /// Retrieve opening value of one of this family's door which neither flipped nor mirrored.
      /// </summary>
      public string BasalOpeningValue
      {
         get
         {
            if (string.IsNullOrEmpty(m_basalOpeningValue))
            {
               string paramValue = DoorSwingResource.Undefined;

               // get current opening value.  
               FamilySymbolSetIterator doorSymbolIter = m_family.Symbols.ForwardIterator();
               doorSymbolIter.MoveNext();
               FamilySymbol doorSymbol                = doorSymbolIter.Current as FamilySymbol;
               paramValue                             = doorSymbol.ParametersMap.get_Item("BasalOpening").AsString();

               // deal with invalid string.
               if (!DoorSwingData.OpeningTypes.Contains(paramValue))
               {
                  paramValue = DoorSwingResource.Undefined;
               }

               m_basalOpeningValue = paramValue;
            }

            return m_basalOpeningValue;
         }
         set
         {
            m_basalOpeningValue = value;
         }
      }

      /// <summary>
      /// Retrieve the geometry of one door which belongs to this family and 
      /// neither flipped nor mirrored.
      /// </summary>
      public DoorGeometry Geometry
      {
         get
         {
            if (null == m_geometry)
            {
               // create one instance of DoorFamilyGeometry class.
               m_geometry = new DoorGeometry(m_oneInstance);
            }

            return m_geometry;
         }
      }

      #endregion

      #region "Methods"

      /// <summary>
      ///  construct function.
      /// </summary>
      /// <param name="doorFamily"> one door family</param>
      /// <param name="app">Revit application</param>
      public DoorFamily(Family doorFamily, UIApplication app)
      {
         m_app         = app;
         m_family      = doorFamily;
         // one door instance which belongs to this family and neither flipped nor mirrored.
         m_oneInstance = CreateOneInstanceWithThisFamily();
      }

      /// <summary>
      /// Update Left/Right feature based on family's actual geometry and country's standard.
      /// </summary>
      public void UpdateOpeningFeature()
      {
         // get current Left/Right feature's value of this door family.
         FamilySymbolSetIterator doorSymbolIter = m_family.Symbols.ForwardIterator();
         while (doorSymbolIter.MoveNext())
         {
            FamilySymbol doorSymbol = doorSymbolIter.Current as FamilySymbol;

            // update the the related family shared parameter's value if user already added it.
            if (doorSymbol.ParametersMap.Contains("BasalOpening"))
            {
               Parameter basalOpeningParam = doorSymbol.ParametersMap.get_Item("BasalOpening");
               bool setResult              = basalOpeningParam.Set(m_basalOpeningValue);
            }
         }
      }

      /// <summary>
      /// Delete the temporarily created door instance and its host.
      /// </summary>
      public void DeleteTempDoorInstance()
      {
         Document doc                    = m_app.ActiveUIDocument.Document;
         Autodesk.Revit.DB.Element tempWall = m_oneInstance.Host;
         doc.Delete(m_oneInstance); // delete temporarily created door instance with this family.
         doc.Delete(tempWall); // delete the door's host.
      }

      /// <summary>
      /// Create one temporary door instance with this family.
      /// </summary>
      /// <returns>the created door.</returns>
      private FamilyInstance CreateOneInstanceWithThisFamily()
      {
         Autodesk.Revit.DB.Document doc                = m_app.ActiveUIDocument.Document;
         Autodesk.Revit.Creation.Document creDoc    = doc.Create;
         Autodesk.Revit.Creation.Application creApp = m_app.Application.Create; 

         // get one level. A project has at least one level.
         Level level = new FilteredElementCollector(doc).OfClass(typeof(Level)).FirstElement() as Level;

         // create one wall as door's host
         Line wallCurve = creApp.NewLineBound(new Autodesk.Revit.DB.XYZ (0,0,0), new Autodesk.Revit.DB.XYZ (100, 0, 0));
         Wall host      = creDoc.NewWall(wallCurve, level, false);
         doc.Regenerate();

         // door symbol.
         FamilySymbolSetIterator doorSymbolIter = m_family.Symbols.ForwardIterator();
         doorSymbolIter.MoveNext();
         FamilySymbol doorSymbol                = doorSymbolIter.Current as FamilySymbol;

         // create the door
         FamilyInstance createdFamilyInstance = creDoc.NewFamilyInstance(new Autodesk.Revit.DB.XYZ (0, 0, 0), doorSymbol, host,level, 
                                                StructuralType.NonStructural);
         doc.Regenerate();

         return createdFamilyInstance;
      }

      #endregion
   }
}
