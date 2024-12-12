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

using System.Collections.Generic;

namespace CodeCheckingConcreteExample
{
   namespace ConcreteTypes // todo: move this class to a new project file
   {
      /// <summary>
      /// Classification of available forces
      /// </summary>
      public enum EnabledInternalForces
      {
         /// <summary>
         /// Axial force. The force acting along the element.
         /// </summary>
         FX = 0x01,
         /// <summary>
         /// Shear force. The force acting perpendicular to the element, along Y axia.
         /// </summary>
         FY = 0x02,
         /// <summary>
         /// Shear force. The force acting perpendicular to the element along Z axis.
         /// </summary>
         FZ = 0x04,
         /// <summary>
         /// Torsional moment.
         /// </summary>
         MX = 0x08,
         /// <summary>
         /// Bending moment. Bending around the Y axis.
         /// </summary>
         MY = 0x10,
         /// <summary>
         /// Bending moment. Bending around the Z axis.
         /// </summary>
         MZ = 0x20,
      }
      /// <structural_toolkit_2015>
  
      /// <summary>
      /// Classification of forces direction in the plain object
      /// </summary>
      public enum DimensioningDirection
      {
         /// <summary>
         /// Main dirction.
         /// </summary>
         X,
         /// <summary>
         /// Secondary direction, ortogonal to main.
         /// </summary>
         Y
      }

      /// </structural_toolkit_2015>

      /// <summary>
      /// Classification of typical designs. Design is based on classification of acting forces.
      /// </summary>
      public enum CalculationType
      {
         /// <summary>
         /// Default value
         /// </summary>
         Unspecified = 0,
         /// <summary>
         /// The design of longitudinal reinforcement should be based on full set of forces.
         /// </summary>
         LongAll = EnabledInternalForces.FX | EnabledInternalForces.MY | EnabledInternalForces.MZ,
         /// <summary>
         /// Simple bending. Around Y axis.
         /// </summary>
         BendingY = EnabledInternalForces.MY,
         /// <summary>
         ///  Eccentricyty bending. Axial force and bending around Y axis.
         /// </summary>
         EccentricBendingY = EnabledInternalForces.FX | EnabledInternalForces.MY,
         /// <summary>
         /// Simple compression.
         /// </summary>
         AxialForce = EnabledInternalForces.FX,
         /// <summary>
         /// The design of transversal reinforcement should be based on full set of forces.
         /// </summary>
         TransAll = EnabledInternalForces.FY | EnabledInternalForces.FZ | EnabledInternalForces.MX,
         /// <summary>
         /// Simple shearing. 
         /// </summary>
         ShearingZ = EnabledInternalForces.FZ,
         /// <summary>
         /// Simple torsion.
         /// </summary>
         Torsion = EnabledInternalForces.MX,
         /// <summary>
         /// Simple torsion.
         /// </summary>
         TorsionWithShearingZ = EnabledInternalForces.FZ | EnabledInternalForces.MX,
      }

      /// <summary>
      /// Type of beam section
      /// </summary>
      public enum BeamSectionType
      {
         /// <summary>
         /// Section with slab interaction
         /// </summary>
         WithSlabBeamInteraction,
         /// <summary>
         /// Section without slab interaction
         /// </summary>
         WithoutSlabBeamInteraction
      }

      /// <summary>
      /// Type of column structure type
      /// </summary>
      public enum ColumnStructureType
      {
         /// <summary>
         /// Sway structure 
         /// </summary>
         Sway,
         /// <summary>
         /// Non-sway structure
         /// </summary>
         NonSway
      }

      /// <summary>
      /// Converter class 
      /// </summary>
      public static class CalculationTypeHelper
      {
         /// <summary>
         /// Converts a number of EnabledInternalForces flags into CalculationType value indicating type of normal forces 
         /// </summary>
         /// <param name="enabledInternalForces">Collection of EnabledInternalForces flags</param>
         /// <returns>CalculationType value describing normal forces in section</returns>
         public static CalculationType GetLongitudinalCalculationType( this IEnumerable<EnabledInternalForces> enabledInternalForces)
         {
            int val = 0;
            foreach (CalculationType calculationType in enabledInternalForces)
            {
               if ((calculationType & CalculationType.LongAll) != 0)
               {
                  val |= (int)calculationType;
               }
            }

            return (CalculationType)val;
         }
        

         /// <summary>
         /// Converts a number of EnabledInternalForces flags into CalculationType value indicating type of normal transversal forces
         /// </summary>
         /// <param name="enabledInternalForces">Collection of EnabledInternalForces flags</param>
         /// <returns>CalculationType value describing transversal forces in section</returns>
         public static CalculationType GetTransversalCalculationType(this IEnumerable<EnabledInternalForces> enabledInternalForces)
         {
            int val = 0;
            foreach (CalculationType calculationType in enabledInternalForces)
            {
               if ((calculationType & CalculationType.TransAll) != 0)
               {
                  val |= (int)calculationType;
               }
            }

            return (CalculationType)val;
         }
         /// <structural_toolkit_2015>
 
         /// <summary>
         ///  Convertion into ForceType
         /// </summary>
         /// <param name="enabledForce">EnabledForce to be converted</param>
         /// <param name="category">Type of element as BuiltInCategory</param>
         /// <returns>Forces type as ForceType</returns>
         public static Autodesk.Revit.DB.CodeChecking.Engineering.ForceType GetForceType(this EnabledInternalForces enabledForce, Autodesk.Revit.DB.BuiltInCategory category = Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical )
         {
            switch( category )
            {
               case Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical:
                  {
                     switch (enabledForce)
                     {
                        default: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Unknown;
                        case EnabledInternalForces.FX: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Fx;
                        case EnabledInternalForces.FY: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Fy;
                        case EnabledInternalForces.FZ: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Fz;
                        case EnabledInternalForces.MX: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Mx;
                        case EnabledInternalForces.MY: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.My;
                        case EnabledInternalForces.MZ: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Mz;
                     }
                  }
               case Autodesk.Revit.DB.BuiltInCategory.OST_FloorAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_FoundationSlabAnalytical:
               case Autodesk.Revit.DB.BuiltInCategory.OST_WallAnalytical:
                  {
                     switch (enabledForce)
                     {
                        default: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Unknown;
                        case EnabledInternalForces.FX: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Fxx;
                        case EnabledInternalForces.FY: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Fyy;
                        case EnabledInternalForces.MX: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Mxx;
                        case EnabledInternalForces.MY: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Myy;
                     }
                  }
               default: return Autodesk.Revit.DB.CodeChecking.Engineering.ForceType.Unknown;
            }
         }
         /// </structural_toolkit_2015>
        
      }
   }
}
