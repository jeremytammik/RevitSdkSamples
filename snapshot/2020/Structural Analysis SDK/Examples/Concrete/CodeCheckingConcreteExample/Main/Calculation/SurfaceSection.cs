//
// (C) Copyright 2003-2013 by Autodesk, Inc.
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
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.CodeChecking.Concrete;
using CodeCheckingConcreteExample.Engine;
using CodeCheckingConcreteExample.Concrete;
using CodeCheckingConcreteExample.Utility;
using CodeCheckingConcreteExample.ConcreteTypes;
using DD = CodeCheckingConcreteExample.ConcreteTypes.DimensioningDirection;
/// <structural_toolkit_2015>
namespace CodeCheckingConcreteExample.Main.Calculation
{
   class SurfaceSection : SectionDataBase
   {
       /// <summary>
      /// Initializes a new instance of user's section object of linear element.  
      /// </summary>
      /// <param name="sectionDataBase">Instance of base section object with predefined parameters to copy.</param>
      public SurfaceSection(SectionDataBase sectionDataBase)
         : base(sectionDataBase)
      {
         DesignWarning = new Dictionary<DD, List<string>>() { { DD.X, new List<string>() }, { DD.Y, new List<string>() } };
         DesignInfo = new Dictionary<DD, List<string>>() { { DD.X, new List<string>() }, { DD.Y, new List<string>() } };
         DesignError = new Dictionary<DD, List<string>>() { { DD.X, new List<string>() }, { DD.Y, new List<string>() } };

      }

      /// <summary>
      /// Gets analitical results for RC surface elements (floor, slab, wall)
      /// </summary>
      /// <returns>Analitical results in the section.</returns>
      public ResultInPointSurface GetCalcResultsInPt()
      {
         ResultInPointSurface resultInPoint = new ResultInPointSurface();
         List<InternalForcesBase> vForces = ListInternalForces;

         resultInPoint[ResultTypeSurface.X] = (CalcPoint as CalcPointSurface).Coord.X;
         resultInPoint[ResultTypeSurface.Y] = (CalcPoint as CalcPointSurface).Coord.Y;
         resultInPoint[ResultTypeSurface.Z] = (CalcPoint as CalcPointSurface).Coord.Z;

         IEnumerable<double> vmxx = from ifo in vForces let mxx = (ifo as InternalForcesSurface).MomentMxx select mxx;
         resultInPoint[ResultTypeSurface.MxxMax] = vmxx.Max();
         resultInPoint[ResultTypeSurface.MxxMin] = vmxx.Min();

         IEnumerable<double> vmyy = from ifo in vForces let myy = (ifo as InternalForcesSurface).MomentMyy select myy;
         resultInPoint[ResultTypeSurface.MyyMax] = vmyy.Max();
         resultInPoint[ResultTypeSurface.MyyMin] = vmyy.Min();

         IEnumerable<double> vmxy = from ifo in vForces let mxy = (ifo as InternalForcesSurface).MomentMxy select mxy;
         resultInPoint[ResultTypeSurface.MxyMax] = vmxy.Max();
         resultInPoint[ResultTypeSurface.MxyMin] = vmxy.Min();

         IEnumerable<double> fmxx = from ifo in vForces let fxx = (ifo as InternalForcesSurface).ForceFxx select fxx;
         resultInPoint[ResultTypeSurface.FxxMax] = fmxx.Max();     
         resultInPoint[ResultTypeSurface.FxxMin] = fmxx.Min();

         IEnumerable<double> fmyy = from ifo in vForces let fyy = (ifo as InternalForcesSurface).ForceFyy select fyy;
         resultInPoint[ResultTypeSurface.FyyMax] = fmyy.Max();
         resultInPoint[ResultTypeSurface.FyyMin] = fmyy.Min();

         IEnumerable<double> fmxy = from ifo in vForces let fxy = (ifo as InternalForcesSurface).ForceFxy select fxy;
         resultInPoint[ResultTypeSurface.FxyMax] = fmxy.Max();
         resultInPoint[ResultTypeSurface.FxyMin] = fmxy.Min();

         IEnumerable<double> qmxx = from ifo in vForces let qxx = (ifo as InternalForcesSurface).ForceQxx select qxx;
         resultInPoint[ResultTypeSurface.QxxMax] = qmxx.Max();
         resultInPoint[ResultTypeSurface.QxxMin] = qmxx.Min();

         IEnumerable<double> qmyy = from ifo in vForces let qyy = (ifo as InternalForcesSurface).ForceQyy select qyy;
         resultInPoint[ResultTypeSurface.QyyMax] = qmyy.Max();
         resultInPoint[ResultTypeSurface.QyyMin] = qmyy.Min();

         resultInPoint[ResultTypeSurface.AxxBottom] = AsBottom[DD.X];
         resultInPoint[ResultTypeSurface.AxxTop]    = AsTop[DD.X];

         resultInPoint[ResultTypeSurface.AyyBottom] = AsBottom[DD.Y];
         resultInPoint[ResultTypeSurface.AyyTop] = AsTop[DD.Y];

         return resultInPoint;
      }

      public List<InternalForcesBase> ListInternalForces { get; set; }
      public Geometry Geometry { get; set; }
      public double Width { get; set; }
      public double Height { get; set; }

      public Dictionary<DD, double> AsBottom = new Dictionary<DD, double>() { { DD.X, 0.00 }, { DD.Y, 0.00 } };
      public Dictionary<DD, double> AsTop    = new Dictionary<DD, double>() { { DD.X, 0.00 }, { DD.Y, 0.00 } };
      public Dictionary<DD, double> MinStiffnes = new Dictionary<DD, double>() { { DD.X, 0.0 }, { DD.Y, 0.0 } };
      public Dictionary<DD, List<Rebar>> LRebar = new Dictionary<DD, List<Rebar>>() { { DD.X, null }, { DD.Y, null } };

      /// <summary>
      /// Gets and sets a list of texts with calculation warnings. 
      /// </summary>
      public Dictionary<DD, List<string>> DesignWarning { get; set; }
      /// <summary>
      /// Gets and sets a list of texts with additional calculation information. 
      /// </summary>
      public Dictionary<DD, List<string>> DesignInfo { get; set; }
      /// <summary>
      /// Gets and sets a list of texts with calculation errors.
      /// </summary>
      public Dictionary<DD, List<string>> DesignError { get; set; }
      /// <summary>
      /// Gets and sets cref="ElementInfo" object.
      /// </summary>
      public ElementInfo Info { get; set; }
   }
}
/// </structural_toolkit_2015>
