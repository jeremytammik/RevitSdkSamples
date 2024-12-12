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

namespace CodeCheckingConcreteExample.Main.Calculation
{
   /// <summary>
   /// Represents user's section of linear element.
   /// </summary>
   class LinearSection : SectionDataBase
   {
      /// <summary>
      /// Initializes a new instance of user's section object of linear element.  
      /// </summary>
      /// <param name="sectionDataBase">Instance of base section object with predefined parameters to copy.</param>
      public LinearSection(SectionDataBase sectionDataBase)
         : base(sectionDataBase)
      {
         DesignWarning = new List<string>();
         DesignInfo = new List<string>();
         DesignError = new List<string>();

      }

      /// <summary>
      /// Gets analitical results for RC bar section
      /// </summary>
      /// <returns>Analitical results in the section.</returns>
      public ResultInPointLinear GetCalcResultsInPoint()
      {
         ResultInPointLinear resultInPoint = null;

         if (ListInternalForces != null && ListInternalForces.Count > 0)
         {
            resultInPoint = new ResultInPointLinear();

            resultInPoint[ResultTypeLinear.X] = (CalcPoint as CalcPointLinear).CoordAbsolute;
            resultInPoint[ResultTypeLinear.X_Rel] = (CalcPoint as CalcPointLinear).CoordRelative;

            IEnumerable<double> vmx = from InternalForcesLinear ifo in ListInternalForces select ifo.Forces.MomentMx;
            resultInPoint[ResultTypeLinear.MxMax] = vmx.Max();
            resultInPoint[ResultTypeLinear.MxMin] = vmx.Min();

            IEnumerable<double> vmy = from InternalForcesLinear ifo in ListInternalForces select ifo.Forces.MomentMy;
            resultInPoint[ResultTypeLinear.MyMax] = vmy.Max();
            resultInPoint[ResultTypeLinear.MyMin] = vmy.Min();

            IEnumerable<double> vmz = from InternalForcesLinear ifo in ListInternalForces select ifo.Forces.MomentMz;
            resultInPoint[ResultTypeLinear.MzMax] = vmz.Max();
            resultInPoint[ResultTypeLinear.MzMin] = vmz.Min();

            IEnumerable<double> vfx = from InternalForcesLinear ifo in ListInternalForces select ifo.Forces.ForceFx;
            resultInPoint[ResultTypeLinear.FxMax] = vfx.Max();
            resultInPoint[ResultTypeLinear.FxMin] = vfx.Min();

            IEnumerable<double> vfy = from InternalForcesLinear ifo in ListInternalForces select ifo.Forces.ForceFy;
            resultInPoint[ResultTypeLinear.FyMax] = vfy.Max();
            resultInPoint[ResultTypeLinear.FyMin] = vfy.Min();

            IEnumerable<double> vfz = from InternalForcesLinear ifo in ListInternalForces select ifo.Forces.ForceFz;
            resultInPoint[ResultTypeLinear.FzMax] = vfz.Max();
            resultInPoint[ResultTypeLinear.FzMin] = vfz.Min();

            resultInPoint[ResultTypeLinear.Abottom] = AsBottom;
            resultInPoint[ResultTypeLinear.Atop] = AsTop;

            resultInPoint[ResultTypeLinear.Aleft] = AsLeft;
            resultInPoint[ResultTypeLinear.Aright] = AsRight;

            resultInPoint[ResultTypeLinear.StirrupsSpacing] = System.Math.Min(Spacing, 10.0);
            resultInPoint[ResultTypeLinear.TransversalReinforcemenDensity] = TransversalDensity;

            IEnumerable<double> vuz = from InternalForcesLinear ifo in ListInternalForces select ifo.Forces.DeflectionUz;

            resultInPoint[ResultTypeLinear.UzMax] = vuz.Max();
            resultInPoint[ResultTypeLinear.UzMin] = vuz.Min();

            resultInPoint[ResultTypeLinear.UzRealMax] = StiffnesCoeff * vuz.Max();
            resultInPoint[ResultTypeLinear.UzRealMin] = StiffnesCoeff * vuz.Min();

            resultInPoint[ResultTypeLinear.UxRealMax] = resultInPoint[ResultTypeLinear.UxMax] = 0;
            resultInPoint[ResultTypeLinear.UxRealMin] = resultInPoint[ResultTypeLinear.UxMin] = 0;
            resultInPoint[ResultTypeLinear.UyRealMax] = resultInPoint[ResultTypeLinear.UyMax] = 0;
            resultInPoint[ResultTypeLinear.UyRealMin] = resultInPoint[ResultTypeLinear.UyMin] = 0;
         }

         return resultInPoint;
      }

      /// <summary>
      /// Gets and sets list of internal forces for section.
      /// </summary>
      public List<InternalForcesBase> ListInternalForces { get; set; }
      /// <summary>
      /// gets and sets geometry of the section.
      /// </summary>
      public Geometry Geometry { get; set; }
      /// <summary>
      /// gets and sets width of the section.
      /// </summary>
      public double Width { get; set; }
      /// <summary>
      /// gets and sets height of the section.
      /// </summary>
      public double Height { get; set; }
      /// <summary>
      /// Gets and sets bottom reinforcement
      /// </summary>
      public double AsBottom { get; set; }
      /// <summary>
      /// Gets and sets top reinforcement
      /// </summary>
      public double AsTop { get; set; }
      /// <summary>
      /// Gets and sets left reinforcement
      /// </summary>
      public double AsLeft { get; set; }
      /// <summary>
      /// Gets and sets right reinforcement
      /// </summary>
      public double AsRight { get; set; }
      /// <summary>
      /// Gets and sets stirrup spacing
      /// </summary>
      public double Spacing { get; set; }
      /// <summary>
      /// Gets and sets transversal reinforcement density
      /// </summary>
      public double TransversalDensity { get; set; }
      /// <summary>
      /// Gets and sets minimal stiffness property of RC section.
      /// </summary>
      public double MinStiffness { get; set; }
      /// <summary>
      /// Gets and sets a list of texts with calculation warnings. 
      /// </summary>
      public double StiffnesCoeff { get; set; }
      /// <summary>
      /// Gets and sets a list of texts with calculation warnings. 
      /// </summary>
      public List<string> DesignWarning { get; set; }
      /// <summary>
      /// Gets and sets a list of texts with additional calculation information. 
      /// </summary>
      public List<string> DesignInfo { get; set; }
      /// <summary>
      /// Gets and sets a list of texts with calculation errors.
      /// </summary>
      public List<string> DesignError { get; set; }
      /// <summary>
      /// Gets and sets cref="ElementInfo" object.
      /// </summary>
      public ElementInfo Info { get; set; }
   }
}
