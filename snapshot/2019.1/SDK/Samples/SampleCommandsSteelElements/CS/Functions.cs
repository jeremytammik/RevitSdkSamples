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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using System.Linq;
using Autodesk.AdvanceSteel.CADAccess;
using Autodesk.Revit.DB.Steel;
using System;
using RvtDocument = Autodesk.Revit.DB.Document;
using Autodesk.AdvanceSteel.DocumentManagement;
using ASDocument = Autodesk.AdvanceSteel.DocumentManagement.Document;
using Autodesk.AdvanceSteel.Geometry;
using Autodesk.AdvanceSteel.Modelling;

namespace Utilities
{
   /// <summary>
   /// Various useful functions
   /// </summary>
   public static class Functions
   {
      /// <summary>
      /// Feet to mm factor
      /// </summary>
      public static double FEET_TO_MM  = 304.8;

      /// <summary>
      /// Structural connection selection function. It uses a customizable filter, so the user can only select connections
      /// </summary>
      public static StructuralConnectionHandler SelectConnection(UIDocument document)
      {
         StructuralConnectionHandler conn = null;
         // Create a filter for structural connections.
         LogicalOrFilter types = new LogicalOrFilter(new List<ElementFilter> { new ElementCategoryFilter(BuiltInCategory.OST_StructConnections) });
         StructuralConnectionSelectionFilter filter = new StructuralConnectionSelectionFilter(types);
         Reference target = document.Selection.PickObject(ObjectType.Element, filter, "Select connection element :");
         if (target != null)
         {
            Element targetElement = document.Document.GetElement(target);
            if (targetElement != null)
            {
               conn = targetElement as StructuralConnectionHandler;
            }
         }
         return conn;
      }

      /// <summary>
      /// Structural connection elements selection function. It uses a customizable filter, so the user can only select allowed elements
      /// </summary>
      public static List<ElementId> SelectConnectionElements(UIDocument document)
      {
         List<ElementId> elemIds = new List<ElementId>();

         // Create a filter for the allowed structural connection inputs.
         LogicalOrFilter connElemTypes = new LogicalOrFilter(new List<ElementFilter>{
            new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming),
            new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns),
            new ElementCategoryFilter(BuiltInCategory.OST_StructuralFoundation),
            new ElementCategoryFilter(BuiltInCategory.OST_Floors),
            new ElementCategoryFilter(BuiltInCategory.OST_Walls),
            new ElementCategoryFilter(BuiltInCategory.OST_StructConnectionPlates)});
         StructuralConnectionSelectionFilter elemFilter = new StructuralConnectionSelectionFilter(connElemTypes);
         List<Reference> refs = document.Selection.PickObjects(ObjectType.Element, elemFilter, "Select elements to add to connection :").ToList();
         elemIds = refs.Select(e => e.ElementId).ToList();
         return elemIds;
      }

      /// <summary>
      /// Custom connection elements selection function. It uses a customizable filter, so the user can only select allowed elements
      /// </summary>
      public static List<Reference> SelectConnectionElementsCustom(UIDocument document)
      {
         List<ElementId> elemIds = new List<ElementId>();

         // Create a filter for the allowed structural connection inputs.
         LogicalOrFilter connElemTypes = new LogicalOrFilter(new List<ElementFilter>{
            new ElementCategoryFilter(BuiltInCategory.OST_StructuralFraming),
            new ElementCategoryFilter(BuiltInCategory.OST_StructuralColumns),
            new ElementCategoryFilter(BuiltInCategory.OST_StructConnectionBolts),
            new ElementCategoryFilter(BuiltInCategory.OST_StructConnectionHoles),
            new ElementCategoryFilter(BuiltInCategory.OST_StructConnectionAnchors),
            new ElementCategoryFilter(BuiltInCategory.OST_StructConnectionShearStuds),
            new ElementCategoryFilter(BuiltInCategory.OST_StructConnectionWelds),
            new ElementCategoryFilter(BuiltInCategory.OST_StructConnectionModifiers),
            new ElementCategoryFilter(BuiltInCategory.OST_StructConnectionPlates)});
         StructuralConnectionSelectionFilter elemFilter = new StructuralConnectionSelectionFilter(connElemTypes);
         List<Reference> refs = document.Selection.PickObjects(ObjectType.Element, elemFilter, "Select elements to add to connection :").ToList();
         return refs;
      }

      /// <summary>
      /// Function that returns the AS Filer Object from the selected reference object
      /// </summary>
     public static FilerObject GetFilerObject(RvtDocument doc, Reference eRef)
      {
         FilerObject filerObject = null;
         ASDocument curDocAS = DocumentManager.GetCurrentDocument();
         if (null != curDocAS)
         {
            OpenDatabase currentDatabase = curDocAS.CurrentDatabase;
            if (null != currentDatabase)
            {
               Guid uid = SteelElementProperties.GetFabricationUniqueID(doc, eRef);
               string asHandle = currentDatabase.getUidDictionary().GetHandle(uid);
               filerObject = FilerObject.GetFilerObjectByHandle(asHandle);
            }
         }
         return filerObject;
      }

      /// <summary>
      /// Function that returns an AS Filer Object list from the selected reference list
      /// </summary>
     public static List<FilerObject> GetFilerObjectList(RvtDocument doc, IList<Reference> eRefList)
      {
         List<FilerObject> filerObjectList = new List<FilerObject>();
         foreach (Reference eRef in eRefList)
         {
            FilerObject fo = GetFilerObject(doc, eRef);
            if (fo != null)
            {
               filerObjectList.Add(fo);
            }
         }
         return filerObjectList;
      }

      /// <summary>
      /// Function that computes the end of a beam. Useful in applying modifiers to beams
      /// </summary>
      public static Beam.eEnd CalculateBeamEnd(Beam asBeam, XYZ pickPoint)
      {
         Beam.eEnd end = Beam.eEnd.kStart;
         Point3d pickPnt3d = XYZtoPoint3d(pickPoint);
         Point3d startPoint = asBeam.GetPointAtStart();
         Point3d endPoint = asBeam.GetPointAtEnd();

         if (pickPnt3d.DistanceTo(startPoint) > pickPnt3d.DistanceTo(endPoint))
         {
            end = Beam.eEnd.kEnd;
         }
         return end;
      }

      /// <summary>
      /// Function that computes the side of a beam. Useful in applying modifiers to beams
      /// </summary>
      public static Beam.eSide CalculateBeamSide(Beam asBeam, Autodesk.Revit.DB.XYZ pickPoint)
      {
         Point3d origin;
         Vector3d xAxis, yAxis, zAxis;
         asBeam.PhysCSStart.GetCoordSystem(out origin, out xAxis, out yAxis, out zAxis);
         Point3d pickPnt3d = XYZtoPoint3d(pickPoint);
         asBeam.SetUpDownTag(pickPnt3d, Beam.eTag.kThisSide);
         return asBeam.GetTaggedUpDown(Beam.eTag.kThisSide);
      }

      /// <summary>
      /// Function that computes the end of a beam. Useful in applying modifiers to beams
      /// </summary>
      public static Point3d XYZtoPoint3d(XYZ p)
      {
         Point3d result = new Point3d(0, 0, 0);
         result.x = FEET_TO_MM * p.X;
         result.y = FEET_TO_MM * p.Y;
         result.z = FEET_TO_MM * p.Z;
         return result;
      }
   }

   //implementation of ISelectionFilter. Needed for connection deletion
   class StructuralConnectionSelectionFilter : ISelectionFilter
   {
      LogicalOrFilter _filter;
      /// <summary>
      /// Initialize the filter with the accepted element types.
      /// </summary>
      /// <param name="elemTypesAllowed">Logical filter containing accepted element types.</param>
      /// <returns></returns>
      public StructuralConnectionSelectionFilter(LogicalOrFilter elemTypesAllowed)
      {
         _filter = elemTypesAllowed;
      }

      /// <summary>
      /// Allows an element to be selected
      /// </summary>
      /// <param name="element">A candidate element in the selection operation.</param>
      /// <returns>Return true to allow the user to select this candidate element.</returns>
      public bool AllowElement(Element element)
      {
         return _filter.PassesFilter(element);
      }
      /// <summary>
      /// Allows a reference to be selected.
      /// </summary>
      /// <param name="refer"> A candidate reference in the selection operation.</param>
      /// <param name="point">The 3D position of the mouse on the candidate reference.</param>
      /// <returns>Return true to allow the user to select this candidate reference.</returns>
      public bool AllowReference(Reference refer, XYZ point)
      {
         return true;
      }
   }
}

