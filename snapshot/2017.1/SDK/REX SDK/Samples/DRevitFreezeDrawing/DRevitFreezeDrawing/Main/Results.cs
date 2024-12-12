//
// (C) Copyright 2016 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.


using System.IO;
using Autodesk.REX.Framework;
using REX.Common;

namespace REX.DRevitFreezeDrawing
{
   internal class Results : REXResults
   {
      public Results(REXExtension Ext)
          : base(Ext)
      {
         VersionCurrent = 1;
      }

      #region Structures
      public double C;
      public double D;
      #endregion

      protected override void OnSetDefaults(REXUnitsSystemType UnitsSystem)
      {
         if (UnitsSystem == REXUnitsSystemType.Imperial)
         {
            C = 0;
            D = 0;
         }
         else if (UnitsSystem == REXUnitsSystemType.Metric)
         {
            C = 0;
            D = 0;
         }
      }

      protected override bool OnSave(ref BinaryWriter Data)
      {
         Data.Write(C);
         Data.Write(D);

         return true;
      }
      protected override bool OnLoad(ref BinaryReader Data)
      {
         if (VersionLoaded >= 1)
         {
            C = Data.ReadDouble();
            D = Data.ReadDouble();
         }

         return true;
      }
   }
}
