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
using Autodesk.Revit.DB.CodeChecking.Engineering;
using Autodesk.CodeChecking.Concrete;
using BIC = Autodesk.Revit.DB.BuiltInCategory;

namespace CodeCheckingConcreteExample.Concrete
{
   /// <summary>
   /// This class provides the example for a simple design of RC cross-section .
   /// </summary>
   public class ConcreteSectionDesign
   {
      // Internal Forces 
      private List<InternalForcesContainer> internalForces;
      private List<InternalForcesContainer> longReinforcementInternalForcesULS;
      private List<InternalForcesContainer> longReinforcementInternalForcesSLS;
      private List<InternalForcesContainer> transReinforcementInternalForcesULS;
      // Section
      private Geometry sectionGeometry;
      private double sectionWidth;
      private double sectionHeight;
      private SectionShapeType sectionType;
      // Concrete
      private double concreteFc;
      private double concreteYoungModulus;
      private double concreteCreepCoefficient;
      //Transversal reinforcement bars
      private double transReinforcementArea;
      private double transReinforcementDiameter;
      private double transReinforcementFy;
      private const int transReinforcementNumberOfLegs = 2;
      //Longitudinal reinforcement bars
      private double longReinforcementArea;
      private double longReinforcementDiameter;
      private double longReinforcementFy;
      private double longReinforcementTopCover;
      private double longReinforcementBottomCover;
      private double longReinforcementTopClearCover;
      private double longReinforcementBottomClearCover;
      private bool symmetricalReinforcementPreferable;
      //Element
      private Autodesk.Revit.DB.BuiltInCategory elementType;
      //Calculation
      private ConcreteTypes.CalculationType transReinforcementCalculationType;
      private ConcreteTypes.CalculationType longReinforcementCalculationType;
      // internal parameters
      private Autodesk.CodeChecking.Concrete.Concrete concreteParameters;
      private RcVerificationHelperUtility verificationHelper;
      private Reinforcement longitudinalReinforcement;
      private Reinforcement transversalReinforcement;
      private double minStiffness;
      private List<string> designInfo;
      private List<string> designError;
      private List<string> designWarning;

      /// <structural_toolkit_2015>
      
      /// <summary>
      /// Direction of dimensioning<see cref="ConcreteTypes.DimensioningDirection"/>
      /// </summary>
      private CodeCheckingConcreteExample.ConcreteTypes.DimensioningDirection dimensioningDirection;
      /// </structural_toolkit_2015>

      /// <summary>
      /// Sets the list of <see cref="InternalForcesContainer"/> associated with combinations or cases.
      /// </summary>
      public List<InternalForcesContainer> ListInternalForces
      {
         set { internalForces = value; }
      }
      /// <summary>
      /// Sets the cross section <see cref="Geometry"/> associated with combinations or cases.
      /// </summary>
      public Geometry Geometry
      {
         set { sectionGeometry = value; }
      }

      /// <summary>
      /// Sets the cross section width.
      /// </summary>
      public double Width
      {
         set { sectionWidth = value; }
      }

      /// <summary>
      /// Sets the cross section height.
      /// </summary>
      public double Height
      {
         set { sectionHeight = value; }
      }

      /// <summary>
      /// Sets type of cross section geometry.
      /// </summary>
      public SectionShapeType Type
      {
         set { sectionType = value; }
      }

      /// <summary>
      /// Sets the longitudinal reinforcement top cover.
      /// </summary>
      public double CoverTop
      {
         set { longReinforcementTopClearCover = value; }
      }

      /// <summary>
      /// Sets the longitudinal reinforcement bottom cover.
      /// </summary>
      public double CoverBottom
      {
         set { longReinforcementBottomClearCover = value; }
      }

      /// <summary>
      /// Sets the concrete Young modulus (modulus of elasticyty).
      /// </summary>
      public double YoungModulus
      {
         set { concreteYoungModulus = value; }
      }

      /// <summary>
      /// Sets the concrete stress limit for compresion.
      /// </summary>
      public double Compression
      {
         set { concreteFc = value; }
      }

      /// <summary>
      /// Sets the concrete creep coefficient.
      /// </summary>
      public double CreepCoefficient
      {
         set { concreteCreepCoefficient = value; }
      }

      /// <summary>
      /// Sets the type for calculation for transversal reinforcement.
      /// </summary>
      public ConcreteTypes.CalculationType TransversalCalculationType
      {
         set { transReinforcementCalculationType = value; }
      }

      /// <summary>
      ///  Sets the type for calculation for transversal reinforcement.
      /// </summary>
      public ConcreteTypes.CalculationType LongitudinalCalculationType
      {
         set { longReinforcementCalculationType = value; }
      }

      /// <summary>
      /// Sets the minimum longitudinal reinforcement yeld stress (steel strenght).
      /// </summary>
      public double LongitudinalReinforcementMinimumYieldStress
      {
         set { longReinforcementFy = value; }
      }

      /// <summary>
      /// Sets the minimum transversal reinforcement yeld stress (steel strenght).
      /// </summary>
      public double TransversalReinforcementMinimumYieldStress
      {
         set { transReinforcementFy = value; }
      }

      /// <summary>
      /// Sets the minimum longitudinal reinforcement rebar area.
      /// </summary>
      public double LongitudinalReinforcementArea
      {
         set { longReinforcementArea = value; }
      }

      /// <summary>
      /// Sets the minimum transversal reinforcement rebar area.
      /// </summary>
      public double TransversalReinforcementArea
      {
         set { transReinforcementArea = value; }
      }

      /// <summary>
      /// Sets the minimum longitudinal reinforcement rebar diameter.
      /// </summary>
      public double LongitudinalReinforcementDiameter
      {
         set { longReinforcementDiameter = value; }
      }

      /// <summary>
      /// Sets the minimum transversal reinforcement rebar diameter.
      /// </summary>
      public double TransversalReinforcementDiameter
      {
         set { transReinforcementDiameter = value; }
      }
      /// <summary>
      /// Sets the type of element.
      /// </summary>
      public Autodesk.Revit.DB.BuiltInCategory ElementType
      {
         set { elementType = value; }
      }

      // Calculation/design resualts:

      /// <summary>
      /// Getsbottom reinforcement
      /// </summary>
      public double AsBottom
      {
         get
         {
            return longitudinalReinforcement.AsBottom + transversalReinforcement.AsBottom;
         }
      }

      /// <summary>
      /// Gets top reinforcement
      /// </summary>
      public double AsTop
      {
         get
         {
            return longitudinalReinforcement.AsTop + transversalReinforcement.AsTop;
         }
      }

      /// <summary>
      /// Gets left reinforcement
      /// </summary>
      public double AsLeft
      {
         get
         {
            return longitudinalReinforcement.AsLeft + transversalReinforcement.AsLeft;
         }
      }

      /// <summary>
      /// Gets right reinforcement
      /// </summary>
      public double AsRight
      {
         get
         {
            return longitudinalReinforcement.AsRight + transversalReinforcement.AsRight;
         }
      }

      /// <summary>
      /// Gets stirrup spacing
      /// </summary>
      public double Spacing
      {
         get
         {
            return transversalReinforcement.Spacing;
         }
      }

      /// <summary>
      /// Gets transversal reinforcement density
      /// </summary>
      public double TransversalDensity
      {
         get
         {
            return transversalReinforcement.TransversalDensity;
         }
      }

      /// <summary>
      /// Gets the list of design remarks
      /// </summary>
      public List<string> DesignInfo
      {
         get { return designInfo; }
      }

      /// <summary>
      /// Gets the list of design erros
      /// </summary>
      public List<string> DesignError
      {
         get { return designError; }
      }
      /// <summary>
      /// Gets the list of design erros
      /// </summary>
      public List<string> DesignWarning
      {
         get { return designWarning; }
      }
      /// <summary>
      /// Gets the minimum of section stiffness
      /// </summary>
      public double MinStiffness
      {
         get { return minStiffness; }
      }
      /// <structural_toolkit_2015>

      /// <summary>
      /// Sets the reinforcement direction for surface elemets.
      /// </summary>
      public CodeCheckingConcreteExample.ConcreteTypes.DimensioningDirection DimensioningDirection
      {
         set
         {
            dimensioningDirection = value;
         }
      }
      /// </structural_toolkit_2015>

      /// <summary>
      /// Initializes a new instance of user's section design object. 
      /// </summary>
      public ConcreteSectionDesign()
      {
         internalForces = new List<InternalForcesContainer>();
         longReinforcementInternalForcesULS = new List<InternalForcesContainer>();
         longReinforcementInternalForcesSLS = new List<InternalForcesContainer>();
         transReinforcementInternalForcesULS = new List<InternalForcesContainer>();
         sectionGeometry = new Geometry();
         sectionWidth = 0.0;
         sectionHeight = 0.0;
         sectionType = SectionShapeType.RectangularBar;
         concreteFc = 0.0;
         concreteYoungModulus = 0.0;
         concreteCreepCoefficient = 1.0;
         transReinforcementArea = 0.0;
         transReinforcementDiameter = 0.0;
         transReinforcementFy = 0.0;
         longReinforcementArea = 0.0;
         longReinforcementDiameter = 0.0;
         longReinforcementFy = 0.0;
         longReinforcementTopCover = 0.0;
         longReinforcementBottomCover = 0.0;
         longReinforcementTopClearCover = 0.0;
         longReinforcementBottomClearCover = 0.0;
         symmetricalReinforcementPreferable = false;
         elementType = Autodesk.Revit.DB.BuiltInCategory.INVALID;
         transReinforcementCalculationType = ConcreteTypes.CalculationType.ShearingZ;
         longReinforcementCalculationType = ConcreteTypes.CalculationType.BendingY;
         concreteParameters = new Autodesk.CodeChecking.Concrete.Concrete();
         minStiffness = 0.0;
         designInfo = new List<string>();
         designError = new List<string>();
         designWarning = new List<string>();
         dimensioningDirection = ConcreteTypes.DimensioningDirection.X;
      }

      /// <summary>
      /// Main calculation method for cross section.
      /// </summary>
      /// <remarks>
      /// <para> Overview: </para>
      /// <para>- preparing necessary data for cross section design </para>
      /// <para>- longitudinal reinforcement design for ultimate limit state  </para>
      /// <para>- longitudinal reinforcement design for serviceability limit state  </para>
      /// <para>- sets minimum reinforcement </para>
      /// <para>- calculation of stiffness for serviceability limit state - necessary for deflection  (only for beam)</para>
      /// <para>- transversal reinforcement design for ultimate limit state  </para>
      /// <para>- storing of information about calculation errors </para>
      /// <para>- sets maximum stirrupas spacing </para>
      /// </remarks>
      public void Calculate()
      {
         try
         {
            PreparationOfCalculationData();                    // preparation of calculation results objects
            DataVeryfication();                                // verification of input data
            ReduceInternalForcesForLongitudinalReinforcement();// reduction of list of forces
            CalculateLongitudinalReinforcement();              // calculate longitudinal reinforcement
            SetLongitudinalMimimumReinforcement();             // sets the minimum of reinforcement

            ReduceInternalForcesForTransversalReinforcement(); // reduction of list of forces
            CalculateTransversalReinforcement();               // calculate transversal reinforcement
            SetTransversalMaximumStirupSpacing();              // sets the maximum of stirrupas spacing 

            CalculateStiffness(); // calculation of stiffness for serviceability limit state - necessary for deflection


         }
         catch (Exception e)     // catching and storing exceptions (calculation errors)
         {
            OnError(e);
         }
      }


      /// <summary>
      /// Preparate the result objects.
      /// </summary>
      public void PreparationOfCalculationData()
      {
         // preparing necessary data for cross section design
         longitudinalReinforcement = new Reinforcement(longReinforcementFy);
         transversalReinforcement = new Reinforcement(transReinforcementFy);
         minStiffness = 0.0;
         longReinforcementTopCover = longReinforcementTopClearCover + transReinforcementDiameter + 0.5 * longReinforcementDiameter;
         longReinforcementBottomCover = longReinforcementBottomClearCover + transReinforcementDiameter + 0.5 * longReinforcementDiameter;
         /// <structural_toolkit_2015>
         designInfo = new List<string>();
         designError = new List<string>();
         designWarning = new List<string>();
         /// </structural_toolkit_2015>
      }
      /// <summary>
      /// Input data veryfication. 
      /// </summary>
      public void DataVeryfication()
      {
         if (Math.Min(longReinforcementTopCover, longReinforcementBottomCover) < 1e-4)
         {
            throw new Exception(Properties.Resources.ResourceManager.GetString("ErrCover"));
         }
         if (Autodesk.Revit.DB.BuiltInCategory.OST_ColumnAnalytical == elementType)
         {
            if (Math.Max(longReinforcementTopCover, longReinforcementBottomCover) > 0.5 * Math.Min(sectionHeight, sectionWidth))
            {
               throw new Exception(Properties.Resources.ResourceManager.GetString("ErrCover"));
            }
         }
         else
         {
            if (longReinforcementTopCover + longReinforcementBottomCover > sectionHeight - 2e-4)
            {
               throw new Exception(Properties.Resources.ResourceManager.GetString("ErrCover"));
            }

         }
         switch (sectionType)
         {
            default:
               throw new Exception(Properties.Resources.ResourceManager.GetString("ErrSectionNotSupported"));
            case SectionShapeType.T:
               break;
            case SectionShapeType.RectangularBar:
               break;
         }
         if (Math.Min(sectionWidth, sectionHeight) < 1e-3)
         {
            throw new Exception("Types: Section dimensions are not properly defined.");
         }
         if (concreteYoungModulus < 1e6)
         {
            throw new Exception(Properties.Resources.ResourceManager.GetString("ErrYoungModulus"));
         }
         if (concreteFc < 1e3)
         {
            throw new Exception(Properties.Resources.ResourceManager.GetString("ErrConcreteCompression"));
         }

         if (concreteCreepCoefficient < 1e-3)
         {
            throw new Exception("Element Settings: Invalid creep coefficient. Creep coefficient must be greater than zero.");
         }
         /// <structural_toolkit_2015>
         if (longReinforcementFy < 1e4)
         {
            throw new Exception(Properties.Resources.ResourceManager.GetString("ErrReinforcementYieldStress"));
         }
         List<BIC> surfaceTypes = new List<BIC>() { BIC.OST_WallAnalytical, BIC.OST_FoundationSlabAnalytical, BIC.OST_FloorAnalytical };
         if (transReinforcementFy < 1e4 && !surfaceTypes.Contains( elementType) )
         {
             throw new Exception(Properties.Resources.ResourceManager.GetString("ErrReinforcementYieldStress"));
         }
         if ( longReinforcementArea < 1e-7)
         {
            throw new Exception("Element Settings: Rebar area is not properly defined.");
         }
         if (transReinforcementArea < 1e-7 && !surfaceTypes.Contains(elementType) )
         {
             throw new Exception("Element Settings: Rebar area is not properly defined.");
         }
         /// </structural_toolkit_2015>
      }
      /// <summary>
      /// Handles exception
      /// </summary>
      /// <param name="e">Exception</param>
      public void OnError(Exception e)
      {
         // reset all of results
         longitudinalReinforcement.Reset();
         transversalReinforcement.Reset();
         minStiffness = 0.0;
         designError.Add(e.Message); // storing of information about calculation errors
         if (e is IRCException)      // storing debug information for RcuapiNet component
         {
            CalculationUtility.SerializeIRCException(e as IRCException);
         }
      }
      /// <summary>
      /// <para>Reduced the number of the internal forces if possible for the calculation of longitudinal reinforcement.</para> 
      /// <para>Separates the list of SLS and ULS. </para>
      /// <para>The new lists of the internal forces are stored in internalForcesULSLrb, internalForcesSLSLrb </para>
      /// </summary>
      public void ReduceInternalForcesForLongitudinalReinforcement()
      {

         longReinforcementInternalForcesULS.Clear();
         longReinforcementInternalForcesSLS.Clear();
         switch (longReinforcementCalculationType) // The forces used to design of longitudinal reinforcement calculation
         {
            // Pure bending. Only maximum and minimum values of bending moment are important.
            case ConcreteTypes.CalculationType.BendingY:
               {
                  InternalForcesContainer forcesMaxMUls = new InternalForcesContainer();
                  InternalForcesContainer forcesMinMUls = new InternalForcesContainer();
                  InternalForcesContainer forcesMaxMSls = new InternalForcesContainer();
                  InternalForcesContainer forcesMinMSls = new InternalForcesContainer();
                  foreach (InternalForcesContainer forces in internalForces)
                  {
                     if (ForceLimitState.Sls == forces.LimitState)
                     {
                        if (forces.MomentMy > forcesMaxMSls.MomentMy)
                           forcesMaxMSls = forces;
                        else if (forces.MomentMy < forcesMinMSls.MomentMy)
                           forcesMinMSls = forces;
                     }
                     else
                     {
                        if (forces.MomentMy > forcesMaxMUls.MomentMy)
                           forcesMaxMUls = forces;
                        else if (forces.MomentMy < forcesMinMUls.MomentMy)
                           forcesMinMUls = forces;
                     }

                  }
                  if (forcesMaxMUls.MomentMy > Double.Epsilon)
                     longReinforcementInternalForcesULS.Add(forcesMaxMUls);
                  if (forcesMinMUls.MomentMy < -Double.Epsilon)
                     longReinforcementInternalForcesULS.Add(forcesMinMUls);
                  if (forcesMaxMSls.MomentMy > Double.Epsilon)
                     longReinforcementInternalForcesSLS.Add(forcesMaxMSls);
                  if (forcesMinMSls.MomentMy < -Double.Epsilon)
                     longReinforcementInternalForcesSLS.Add(forcesMinMSls);
               }
               break;
            // Uniaxial bending with axial force.
            // The convex hull (convex envelope) method is used to pick important forces.
            /// <structural_toolkit_2015>
            case ConcreteTypes.CalculationType.EccentricBendingY:
               {
                  List<Point2D> vNMUls = new List<Point2D>();
                  List<int> vNMUlsIndex = new List<int>();
                  List<Point2D> vNMSls = new List<Point2D>();
                  List<int> vNMSlsIndex = new List<int>();

                  for (int i = 0; i < internalForces.Count(); i++)
                  {
                     if (ForceLimitState.Sls == internalForces[i].LimitState)
                     {
                        vNMSls.Add(new Point2D(internalForces[i].ForceFx, internalForces[i].MomentMy));
                        vNMSlsIndex.Add(i);
                     }
                     else
                     {
                        vNMUls.Add(new Point2D(internalForces[i].ForceFx, internalForces[i].MomentMy));
                        vNMUlsIndex.Add(i);
                     }
                     
                  }
                  List<int> viNMUls = Autodesk.CodeChecking.Utils.ConvexHull(vNMUls);
                  List<int> viNMSls = Autodesk.CodeChecking.Utils.ConvexHull(vNMSls);
                  double absForceFx = 0;
                  double eccentricity = 0;
                  foreach (int index in viNMUls)
                  {
                     if (!IsZeroForces(internalForces[vNMUlsIndex[index]], true))
                     {
                        longReinforcementInternalForcesULS.Add(internalForces[vNMUlsIndex[index]]);
                        if (!symmetricalReinforcementPreferable)
                        {
                           absForceFx = Math.Abs(internalForces[vNMUlsIndex[index]].ForceFx);
                           if (absForceFx > Double.Epsilon)
                           {
                              eccentricity = Math.Abs(internalForces[vNMUlsIndex[index]].MomentMy) / absForceFx;
                              symmetricalReinforcementPreferable = (eccentricity < 0.25 * sectionHeight);

                           }
                        }
                     }
                  }
                  foreach (int index in viNMSls)
                  {
                     if (!IsZeroForces(internalForces[vNMSlsIndex[index]], true))
                     {
                        longReinforcementInternalForcesSLS.Add(internalForces[vNMSlsIndex[index]]);
                        if (!symmetricalReinforcementPreferable)
                        {
                           absForceFx = Math.Abs(internalForces[vNMSlsIndex[index]].ForceFx);
                           if (absForceFx > Double.Epsilon)
                           {
                              eccentricity = Math.Abs(internalForces[vNMSlsIndex[index]].MomentMy) / absForceFx;
                              symmetricalReinforcementPreferable = (eccentricity < 0.25 * sectionHeight);
                           }
                        }
                     }
                  }
               }
               break;
            /// <structural_toolkit_2015>
            // Pure axial force (compresion or tension). Only maximum and minimum values of axial force are important.
            case ConcreteTypes.CalculationType.AxialForce:
               {
                  symmetricalReinforcementPreferable = true;
                  InternalForcesContainer forcesMaxNUls = new InternalForcesContainer();
                  InternalForcesContainer forcesMinNUls = new InternalForcesContainer();
                  InternalForcesContainer forcesMaxNSls = new InternalForcesContainer();
                  InternalForcesContainer forcesMinNSls = new InternalForcesContainer();
                  foreach (InternalForcesContainer forces in internalForces)
                  {
                     if (ForceLimitState.Sls == forces.LimitState)
                     {
                        if (forces.ForceFx > forcesMaxNSls.ForceFx)
                           forcesMaxNSls = forces;
                        else if (forces.ForceFx < forcesMinNSls.ForceFx)
                           forcesMinNSls = forces;
                     }
                     else
                     {
                        if (forces.ForceFx > forcesMaxNUls.ForceFx)
                           forcesMaxNUls = forces;
                        else if (forces.ForceFx < forcesMinNUls.ForceFx)
                           forcesMinNUls = forces;
                     }
                  }
                  if (forcesMaxNUls.ForceFx > Double.Epsilon)
                     longReinforcementInternalForcesULS.Add(forcesMaxNUls);
                  if (forcesMinNUls.ForceFx < -Double.Epsilon)
                     longReinforcementInternalForcesULS.Add(forcesMinNUls);
                  if (forcesMaxNSls.ForceFx > Double.Epsilon)
                     longReinforcementInternalForcesSLS.Add(forcesMaxNSls);
                  if (forcesMinNSls.ForceFx < -Double.Epsilon)
                     longReinforcementInternalForcesSLS.Add(forcesMinNSls);
               }
               break;
            // All other cases. Any set of forces can be important.
            default:
               break;
         }
      }
      /// <summary>
      /// <para>Reduced the number of the internal forces if possible for the calculation of transversal reinforcement.</para>
      /// <para>Separates the list of SLS and ULS.</para>
      /// <para>The new lists of the internal forces are stored in internalForcesULSTrb</para>
      /// </summary>
      public void ReduceInternalForcesForTransversalReinforcement()
      {
         transReinforcementInternalForcesULS.Clear();
         // Pure shearing. Only maximum and minimum values of shear force are important.
         if (ConcreteTypes.CalculationType.ShearingZ == transReinforcementCalculationType)
         {
            InternalForcesContainer forcesMaxVUls = new InternalForcesContainer();
            InternalForcesContainer forcesMinVUls = new InternalForcesContainer();
            foreach (InternalForcesContainer forces in internalForces)
            {
               if (ForceLimitState.Sls != forces.LimitState)
               {
                  if (forces.ForceFz > forcesMaxVUls.ForceFz)
                     forcesMaxVUls = forces;
                  else if (forces.ForceFz < forcesMinVUls.ForceFz)
                     forcesMinVUls = forces;
               }
            }
            if (forcesMaxVUls.ForceFz > Double.Epsilon)
               transReinforcementInternalForcesULS.Add(forcesMaxVUls);
            if (forcesMinVUls.ForceFz < -Double.Epsilon)
               transReinforcementInternalForcesULS.Add(forcesMinVUls);
         }
      }
      /// <summary>
      /// Calculate and sets maximun stirrup spacing
      /// </summary>
      private void SetTransversalMaximumStirupSpacing()
      {
         switch (transReinforcementCalculationType) // minimum spacing is depend to calculation type and dimensions of cross section 
         {
            default:
            case ConcreteTypes.CalculationType.ShearingZ:
               transversalReinforcement.CurrentSpacing = Math.Min(0.5 * sectionHeight, 0.4);
               break;
            case ConcreteTypes.CalculationType.TorsionWithShearingZ:
               transversalReinforcement.CurrentSpacing = Math.Min(0.4 * sectionHeight, 0.25);
               break;
            case ConcreteTypes.CalculationType.TransAll:
               transversalReinforcement.CurrentSpacing = Math.Min(0.4 * Math.Min(sectionHeight, sectionWidth), 0.25);
               break;
         }
         transversalReinforcement.CurrentToFinial();
         transversalReinforcement.SetTransversalDensity(transReinforcementNumberOfLegs, transReinforcementArea); //sets transversal reinforcemen density for 2 arms stirrups
      }
      /// <summary>
      /// Calculate and sets minimum reinforcement
      /// </summary>
      private void SetLongitudinalMimimumReinforcement()
      {
         switch (elementType)
         {
            case BIC.OST_BeamAnalytical:
               {
                  double minimumReinforcement = 0.001 * sectionGeometry.Area;        // 0.1% on each side with reinforcement
                  if (longitudinalReinforcement.AsBottom > 0.0)
                     longitudinalReinforcement.CurrentAsBottom = minimumReinforcement;
                  if (longitudinalReinforcement.AsTop > 0.0)
                     longitudinalReinforcement.CurrentAsTop = minimumReinforcement;
                  longitudinalReinforcement.CurrentToFinial();
               }
               break;
            case  BIC.OST_ColumnAnalytical:
               {
                  double minimumReinforcementRebar = 4.0 * longReinforcementArea; // minimum 4 bars in the section
                  double totalReinforcement = longitudinalReinforcement.AsTop + longitudinalReinforcement.AsBottom;
                  //2 bars are placed on the top and 2 on the bottom
                  if (totalReinforcement < minimumReinforcementRebar)
                  {
                     longitudinalReinforcement.CurrentAsTop = longitudinalReinforcement.CurrentAsBottom = 0.5 * minimumReinforcementRebar;
                     longitudinalReinforcement.CurrentToFinial();
                  }
                  totalReinforcement = longitudinalReinforcement.TotalSectionReinforcement();
                  double minimumReinforcement = 0.005 * sectionGeometry.Area; // 0.5% as minimum area

                  //the minimum reinforcement is placed proportional to the existing reinforcement
                  if (totalReinforcement < minimumReinforcement)
                  {
                     double coef = totalReinforcement / minimumReinforcement;
                     longitudinalReinforcement.CurrentAsBottom = longitudinalReinforcement.AsBottom / coef;
                     longitudinalReinforcement.CurrentAsTop = longitudinalReinforcement.AsTop / coef;
                     longitudinalReinforcement.CurrentAsLeft = longitudinalReinforcement.AsLeft / coef;
                     longitudinalReinforcement.CurrentAsRight = longitudinalReinforcement.AsRight / coef;
                     longitudinalReinforcement.CurrentToFinial();
                  }
               }
               break;
            /// <structural_toolkit_2015>
            case BIC.OST_FloorAnalytical:
            case BIC.OST_FoundationSlabAnalytical:
               {
                  double minimumReinforcement = 0.0005 * sectionGeometry.Area; // 0.05% as minimum area
                  if (longitudinalReinforcement.AsBottom > 0.0)
                     longitudinalReinforcement.CurrentAsBottom = minimumReinforcement;
                  if (longitudinalReinforcement.AsTop > 0.0)
                     longitudinalReinforcement.CurrentAsTop = minimumReinforcement;
                  longitudinalReinforcement.CurrentToFinial();
               }
               break;
            case BIC.OST_WallAnalytical:
               {
                  double minimumReinforcement = (dimensioningDirection == ConcreteTypes.DimensioningDirection.X) ? 0.0025 : 0.001;  // 0.25% for vertical and 0.2% for horizontal for each side
                  minimumReinforcement *= sectionGeometry.Area;
                  longitudinalReinforcement.CurrentAsBottom = minimumReinforcement;
                   longitudinalReinforcement.CurrentAsTop = minimumReinforcement;
                  longitudinalReinforcement.CurrentToFinial();
               }
               break;
            /// </structural_toolkit_2015>
            default:
               break;
         }
         
      }
      /// <summary>
      /// Checks whether the set of forces can be considered as null.
      /// </summary>
      /// <param name="forces">Set of forces</param>
      /// <param name="longitudinalReinforcemenet">Information about type of designing: true - longitudinal reinforcement, false - transversal reinforcement.</param>
      /// <returns>True if forces can be considered as null.</returns>
      private bool IsZeroForces(InternalForcesContainer forces, bool longitudinalReinforcemenet)
      {
         bool zeroForces = true;
         if (longitudinalReinforcemenet)
         {
            switch (longReinforcementCalculationType)
            {
               case ConcreteTypes.CalculationType.BendingY:
                  zeroForces = CalculationUtility.IsZeroM(forces.MomentMy);
                  break;
               case ConcreteTypes.CalculationType.AxialForce:
                  zeroForces = CalculationUtility.IsZeroN(forces.ForceFx);
                  break;
               case ConcreteTypes.CalculationType.EccentricBendingY:
                  zeroForces = CalculationUtility.IsZeroM(forces.MomentMy) && CalculationUtility.IsZeroN(forces.ForceFx);
                  break;
               default:
                  zeroForces = CalculationUtility.IsZeroM(forces.MomentMy) && CalculationUtility.IsZeroN(forces.ForceFx) && CalculationUtility.IsZeroM(forces.MomentMz);
                  break;
            }
         }
         else
         {
            switch (transReinforcementCalculationType)
            {
               case ConcreteTypes.CalculationType.ShearingZ:
                  zeroForces = CalculationUtility.IsZeroN(forces.ForceFz);
                  break;
               case ConcreteTypes.CalculationType.Torsion:
                  zeroForces = CalculationUtility.IsZeroM(forces.MomentMx);
                  break;
               case ConcreteTypes.CalculationType.TorsionWithShearingZ:
                  zeroForces = CalculationUtility.IsZeroM(forces.MomentMx) && CalculationUtility.IsZeroN(forces.ForceFz);
                  break;
               default:
                  zeroForces = CalculationUtility.IsZeroM(forces.MomentMx) && CalculationUtility.IsZeroN(forces.ForceFz) && CalculationUtility.IsZeroN(forces.ForceFy);
                  break;
            }
         }
         return zeroForces;
      }
      /// <summary>
      /// Calculates the longitudinal reinforcement.
      /// </summary>
      private void CalculateLongitudinalReinforcement()
      {
         // longitudinal reinforcement design for ultimate limit state
         SetMaterialParameters(ForceLimitState.Uls);        // sets material properties for ultimate limit state
         if (longReinforcementInternalForcesULS.Count() > 0)
         {
            foreach (InternalForcesContainer forces in longReinforcementInternalForcesULS)
            {
               if (IsZeroForces(forces, true))
                  continue;
               CalculateLongitudinalReinforcementULS(forces); // design the reinforcement for single case
               longitudinalReinforcement.CurrentToFinial(); // creates reinforcement envelope (maximum value) - finial reinforcement
            }
         }
         else
         {
            foreach (InternalForcesContainer forces in internalForces)
            {
               if (ForceLimitState.Sls == forces.LimitState)
                  continue; // only for ultimate limit state
               if (IsZeroForces(forces, true))
                  continue;
               CalculateLongitudinalReinforcementULS(forces); // design the reinforcement for single case
               longitudinalReinforcement.CurrentToFinial(); // creates reinforcement envelope (maximum value) - finial reinforcement
            }
         }
         // longitudinal reinforcement design for serviceability limit state
         SetMaterialParameters(ForceLimitState.Sls); // sets material properties for serviceability limit state
         if (longReinforcementInternalForcesSLS.Count() > 0)
         {
            foreach (InternalForcesContainer forces in longReinforcementInternalForcesSLS)
            {
               if (IsZeroForces(forces, true))
                  continue;
               CalculateLongitudinalReinforcementSLS(forces); // design the reinforcement for single case
               longitudinalReinforcement.CurrentToFinial(); // creates reinforcement envelope (maximum value) - finial reinforcement
            }
         }
         else
         {
            foreach (InternalForcesContainer forces in internalForces)
            {
               if (ForceLimitState.Sls != forces.LimitState)
                  continue; // only for serviceability limit state
               if (IsZeroForces(forces, true))
                  continue;
               CalculateLongitudinalReinforcementSLS(forces); // design the reinforcement for single case
               longitudinalReinforcement.CurrentToFinial(); // creates reinforcement envelope (maximum value) - finial reinforcement
            }
         }
      }
      /// <summary>
      /// Runs calculation of longitudinal reinforcement in ULS state dependencies to forces and calculation type.
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void CalculateLongitudinalReinforcementULS(InternalForcesContainer forces)
      {
         if (CodeCheckingConcreteExample.ConcreteTypes.CalculationType.BendingY == longReinforcementCalculationType) // Only My
         {
            if (SectionShapeType.RectangularBar == sectionType) // Rectangular section
            {
               CalculateLongitudinalReinforcementSimplify(ref forces);
            }
            else
            {
               CalculateLongitudinalReinforcementUniaxial(ref forces);
            }
         }
         else if ((CodeCheckingConcreteExample.ConcreteTypes.CalculationType.EccentricBendingY == longReinforcementCalculationType) || CalculationUtility.IsZeroM(forces.MomentMz)) // Without Mz
         {
            if (CalculationUtility.IsZeroN(forces.ForceFx))
            {
               if (SectionShapeType.RectangularBar == sectionType)
               {
                  CalculateLongitudinalReinforcementSimplify(ref forces);
               }
               else
               {
                  CalculateLongitudinalReinforcementUniaxial(ref forces);
               }
            }
            else if (CalculationUtility.IsZeroM(forces.MomentMy))
            {
               CalculateLongitudinalReinforcementPureAxialForce(forces.ForceFx);
            }
            else
            {
               CalculateLongitudinalReinforcementUniaxial(ref forces);
            }
         }
         else
         {
            if (CalculationUtility.IsZeroM(forces.MomentMz)) // Without Mz
            {
               if (!CalculationUtility.IsZeroM(forces.MomentMy))
               {
                  if (SectionShapeType.RectangularBar == sectionType)
                  {
                     CalculateLongitudinalReinforcementSimplify(ref forces);
                  }
                  else
                  {
                     CalculateLongitudinalReinforcementUniaxial(ref forces);
                  }
               }
               else
               {
                  if (!CalculationUtility.IsZeroN(forces.ForceFx))
                  {
                     CalculateLongitudinalReinforcementPureAxialForce(forces.ForceFx);
                  }
               }
            }
            else
               CalculateLongitudinalReinforcementBiaxial(ref forces); // full biaxial dimensioning
         }
      }
      /// <summary>
      /// Runs calculation of longitudinal reinforcement in SLS state dependencies to forces and calculation type.
      /// Limit for stress in concrete and steel.
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void CalculateLongitudinalReinforcementSLS(InternalForcesContainer forces)
      {
         switch (longReinforcementCalculationType)
         {
            case CodeCheckingConcreteExample.ConcreteTypes.CalculationType.BendingY:
               CalculateLongitudinalReinforcementUniaxial(ref forces);
               break;
            case CodeCheckingConcreteExample.ConcreteTypes.CalculationType.EccentricBendingY:
               CalculateLongitudinalReinforcementUniaxial(ref forces);
               break;
            default:
               if (CalculationUtility.IsZeroM(forces.MomentMz)) // Without Mz
               {
                  CalculateLongitudinalReinforcementUniaxial(ref forces);
               }
               else
               {
                  CalculateLongitudinalReinforcementBiaxial(ref forces);
               }
               break;
         }
      }
      /// <summary>
      /// Cross section stiffness calculation. Only for beam elements.
      /// </summary>
      private void CalculateStiffness()
      {
         bool noSLS = true;
         if (elementType == Autodesk.Revit.DB.BuiltInCategory.OST_BeamAnalytical)
         {
            minStiffness = sectionGeometry.MomentOfInertiaX * concreteParameters.ModulusOfElasticity;
            SetMaterialParameters(ForceLimitState.Sls); // sets material properties for serviceability limit state
            if (longReinforcementInternalForcesSLS.Count() > 0)
            {
               noSLS = false;
               foreach (InternalForcesContainer forces in longReinforcementInternalForcesSLS)
               {
                  if (IsZeroForces(forces, true))
                  {
                     continue;
                  }
                  else
                  {
                     minStiffness = Math.Min(minStiffness, CalculateStiffnesSLS(forces, concreteCreepCoefficient)); // creates stiffnes envelope (minimum value)
                  }
               }
            }
            else
            {
               foreach (InternalForcesContainer forces in internalForces)
               {
                  if (ForceLimitState.Sls != forces.LimitState)
                     continue; // only for ultimate limit state
                  noSLS = false;
                  if (IsZeroForces(forces, true))
                  {
                     continue;
                  }
                  else
                  {
                     minStiffness = Math.Min(minStiffness, CalculateStiffnesSLS(forces, concreteCreepCoefficient)); // creates stiffnes envelope (minimum value)
                  }
               }
            }
         }
         if (noSLS)
            minStiffness = 0;
      }
      /// <structural_toolkit_2015>

      /// <summary>
      /// Runs calculation of RC section stiffnes in SLS state. Including cracking and creep. Necessary for deflection.
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      /// <param name="creepCofficient">Concrete creep coefficient.</param>
      /// <returns>RC cross section stiffnes.</returns>
      private double CalculateStiffnesSLS(InternalForcesContainer forces, double creepCofficient)
      {
         double stiffnes = 0.0;
         if (SectionShapeType.RectangularBar == sectionType)
         {
            if (verificationHelper == null)
            {
               verificationHelper = RcVerificationHelperUtility.CreateRcVerificationHelperUtility(sectionType, ref sectionGeometry, longReinforcementTopCover, longReinforcementBottomCover);
               SetMaterialParameters(forces.LimitState);
            }
            double momentOfInertiaConcreteSection = sectionGeometry.MomentOfInertiaX;
            stiffnes = momentOfInertiaConcreteSection;
            if (!IsZeroForces(forces, true))
            {
               double concreteTensionLimit = 0.3 * Math.Pow(concreteParameters.DesignStrength * 1e-6, 2.0 / 3.0) * 1e6;
               double crackingCoefficient = verificationHelper.ForcesToCrackingForces(forces, concreteTensionLimit);
               if (crackingCoefficient.CompareTo(0.0) < 0)
                  throw new Exception(String.Format("Invalid ForcesToCrackingForces in CalculateStiffnesSLS  for case {0}.", forces.CaseName));
               if (crackingCoefficient > 1.0)
               {
                  crackingCoefficient = 1.0 / crackingCoefficient; // crackingCoefficient = (Mcr/MEd)
                  crackingCoefficient *= crackingCoefficient;      // crackingCoefficient = (Mcr/MEd)^2
               }
               else
                  crackingCoefficient = 0;
               double momentOfInertiaCrackingConcreteSection = verificationHelper.InertiaOfCrackingSection(forces);
               stiffnes = (1.0-crackingCoefficient) * momentOfInertiaConcreteSection +    // part of uncracked cross section is taken into account
                          crackingCoefficient * momentOfInertiaCrackingConcreteSection;   // part of cracked cross section is taken into account

            }
            stiffnes *= concreteParameters.ModulusOfElasticity / (1 + creepCofficient);
         }
         else
            designWarning.Add("Only rectangular cross-section can be used on this path. 3th party implementation is necessary");
         return stiffnes;
      }
      /// </structural_toolkit_2015>
  
      /// <summary>
      /// Calculates the area of reinforcement for pure tension and compression.
      /// </summary>
      /// <param name="forcesN">Axial force</param>
      void CalculateLongitudinalReinforcementPureAxialForce(double forcesN)
      {
         double totalSteelArea = 0;
         if (CalculationUtility.LtZeroN(forcesN))
         {
            totalSteelArea = -forcesN / longitudinalReinforcement.Strength;
         }
         else
         {
            double NRd = sectionGeometry.Area * concreteParameters.DesignStrength;
            forcesN -= NRd;
            if (CalculationUtility.GtZeroN(forcesN))
            {
               totalSteelArea = forcesN / longitudinalReinforcement.Strength;
            }
         }
         longitudinalReinforcement.CurrentAsTop = longitudinalReinforcement.CurrentAsBottom = longitudinalReinforcement.CurrentAsRight = longitudinalReinforcement.CurrentAsLeft = totalSteelArea / 4.0;
      }
      /// <summary>
      ///  Calculates the area of reinforcement in the simple cases: pure bending and rectangular section.
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void CalculateLongitudinalReinforcementSimplify(ref InternalForcesContainer forces)
      {
         bool tensionOnTop = forces.MomentMy < 0.0;
         double absM = Math.Abs(forces.MomentMy);
         double steelTensStrain = longitudinalReinforcement.Strength / longitudinalReinforcement.ModulusOfElasticity;
         double concreteStrain = concreteParameters.StrainUltimateLimit;
         double d = sectionHeight - (tensionOnTop ? longReinforcementTopCover : longReinforcementBottomCover);
         double x = concreteStrain * d / (concreteStrain + steelTensStrain);
         x *= concreteParameters.EffectiveHeightReductionFactor; // 0.8
         double concreteForces = x * sectionHeight * sectionWidth * concreteParameters.DesignStrength;
         double maxOneSideReforcementMoment = concreteForces * (d - 0.5 * x);
         double tensionReinforcement = 0.0;
         double compressionReinforcement = 0.0;
         if (maxOneSideReforcementMoment > absM)
         {
            // parameters for quadratic equation ax^2+bx+c
            double a = -0.5 * concreteParameters.EffectiveHeightReductionFactor;
            double b = d;
            double c = -absM / (concreteParameters.DesignStrength * sectionWidth * concreteParameters.EffectiveHeightReductionFactor);
            // results for quadratic equation
            double xM1 = 0;
            double xM2 = 0;
            CalculationUtility.RootsOfQuadraticEquation(a, b, c, ref xM1, ref xM2);
            if (!CalculationUtility.RootsOfQuadraticEquation(a, b, c, ref xM1, ref xM2))
               throw new Exception(String.Format("Invalid CalculateLongitudinalReinforcementSimplify delta <= 0.0 for case {0}.",forces.CaseName));
            double coeff = xM1 * xM2;
            double xM = coeff >= Double.Epsilon ? Math.Min(xM1, xM2) : Math.Max(xM1, xM2);
            if (xM <= 0.0)
               throw new Exception(String.Format("Invalid CalculateLongitudinalReinforcementSimplify xM <= 0.0 for case {0}.", forces.CaseName));
            tensionReinforcement = (xM * concreteParameters.EffectiveHeightReductionFactor * sectionWidth * concreteParameters.DesignStrength) / longitudinalReinforcement.Strength;
         }
         else
         {
            tensionReinforcement = concreteForces / longitudinalReinforcement.Strength;
            compressionReinforcement = (absM - maxOneSideReforcementMoment) / (sectionHeight - longReinforcementTopCover - longReinforcementBottomCover);
            compressionReinforcement /= longitudinalReinforcement.Strength;
            tensionReinforcement += compressionReinforcement;
         }
         longitudinalReinforcement.CurrentAsTop = tensionOnTop ? tensionReinforcement : compressionReinforcement;
         longitudinalReinforcement.CurrentAsBottom = tensionOnTop ? compressionReinforcement : tensionReinforcement;
      }
      /// <summary>
      /// Runs calculation for symetrical or unsymetrical longitudinal reinforcement.
      /// </summary>
      /// <param name="safetyFactor">Safety factor for current reinforcement.</param>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void FindOptimalLongitudinalReinforcementUniaxial(ref double safetyFactor, ref InternalForcesContainer forces)
      {
         if (symmetricalReinforcementPreferable)
         {
            FindOptimalLongitudinalReinforcementUniaxialSymmetrical(ref safetyFactor, ref forces);
         }
         else
         {
            FindOptimalLongitudinalReinforcementUniaxialUnsymmetrical(ref safetyFactor, ref forces);
         }
      }
      /// <summary>
      ///  Searching the best of symetrical longitudinal reinforcement.
      /// </summary>
      /// <param name="safetyFactor">Safety factor for current reinforcement.</param>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void FindOptimalLongitudinalReinforcementUniaxialSymmetrical(ref double safetyFactor, ref InternalForcesContainer forces)
      {
         double reinforcementIncrase = 0;
         // Checking capacity for bigger reinforcement on the top
         double safetyFactorTopBottom = -1;
         bool resultNotOK = true;
         int i = 0;
         // tabeles with reinforcement: [0] - less than necessary, [1]  - current reiforcement value, [2] - more than necessary
         double[] asTopBottom = new double[] { longitudinalReinforcement.CurrentAsTop, longitudinalReinforcement.CurrentAsTop, Double.MaxValue };
         double increasesafetyFactorTopBottom = 1.0;
         while (!CalculationUtility.IsIterEnd(++i) && resultNotOK)
         {
            reinforcementIncrase = CalculationUtility.MinimumIncreaseOfReinforcement();
            verificationHelper.SetReinforcement(asTopBottom[1] + reinforcementIncrase, asTopBottom[1] + reinforcementIncrase);
            safetyFactorTopBottom = verificationHelper.SafetyFactor(forces);
            resultNotOK = !CalculationUtility.IsSafety(safetyFactorTopBottom);
            if (resultNotOK)
            {
               // reinforcement is to low  - copied from [1] to [0]
               asTopBottom[0] = asTopBottom[1] += reinforcementIncrase;
               increasesafetyFactorTopBottom = (safetyFactorTopBottom - safetyFactor);
               increasesafetyFactorTopBottom *= (1.0 - safetyFactor);
               if (increasesafetyFactorTopBottom > Double.Epsilon)
               {
                  reinforcementIncrase = (2.0 * CalculationUtility.MinimumIncreaseOfReinforcement()) / increasesafetyFactorTopBottom;
               }
               else
               {
                  reinforcementIncrase = Double.MaxValue;
               }
               AdjustReinforcementIncrase(ref reinforcementIncrase);
            }
            else
            {
               // reinforcement is to big - copied from [1] to [2]
               asTopBottom[2] = asTopBottom[1];
               safetyFactor = safetyFactorTopBottom;
            }
            asTopBottom[1] += reinforcementIncrase;
            verificationHelper.SetReinforcement(asTopBottom[1], asTopBottom[1]);
            safetyFactor = verificationHelper.SafetyFactor(forces);
            resultNotOK = !CalculationUtility.IsSafety(safetyFactor);
            if (resultNotOK)
            {
               // reinforcement is to low  - copied from [1] to [0]
               asTopBottom[0] = asTopBottom[1];
            }
            else
            {
               // reinforcement is to big - copied from [1] to [2]
               asTopBottom[2] = asTopBottom[1] += reinforcementIncrase;
            }
         }
         if (resultNotOK)
         {
            throw new Exception(String.Format("Too many iteration for case {0}. FindOptimalLongitudinalReinforcementUniaxialSymmetrical.",forces.CaseName));
         }
         else
         {
            resultNotOK = !CalculationUtility.IsSafetyOptimal(safetyFactor);
            if (resultNotOK)
            {
               BisectionForReinforcementAdjustment(ref forces, asTopBottom, asTopBottom);
            }
            else
            {
               longitudinalReinforcement.CurrentAsTop = asTopBottom[1];
               longitudinalReinforcement.CurrentAsBottom = asTopBottom[1];
            }
         }
      }
      /// <summary>
      ///  Searching the best of unsymetrical longitudinal reinforcement.
      /// </summary>
      /// <param name="safetyFactor">Safety factor for current reinforcement.</param>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void FindOptimalLongitudinalReinforcementUniaxialUnsymmetrical(ref double safetyFactor, ref InternalForcesContainer forces)
      {
         double reinforcementIncraseTop = 0;
         double reinforcementIncraseBottom = reinforcementIncraseTop;
         // Checking capacity for bigger reinforcement on the top
         double safetyFactorTop = -1;
         double safetyFactorBottom = -1;
         bool resultNotOK = true;
         int i = 0;
         // tabeles with reinforcement: [0] - less than necessary, [1]  - current reiforcement value, [2] - more than necessary
         double[] asTop = new double[] { longitudinalReinforcement.CurrentAsTop, longitudinalReinforcement.CurrentAsTop, Double.MaxValue };
         double[] asBottom = new double[] { longitudinalReinforcement.CurrentAsBottom, longitudinalReinforcement.CurrentAsBottom, Double.MaxValue };
         double increaseSafetyFactorTop = 1.0;
         double increaseSafetyFactorBottom = 1.0;
         while (!CalculationUtility.IsIterEnd(++i) && resultNotOK)
         {
            /// <structural_toolkit_2015>
            reinforcementIncraseBottom = reinforcementIncraseTop = Math.Max(CalculationUtility.MinimumIncreaseOfReinforcement(), 0.005 * (asTop[1] + asBottom[0]));
            /// </structural_toolkit_2015>
            // check capacity for top
            verificationHelper.SetReinforcement(asTop[1] + reinforcementIncraseTop, asBottom[0]);
            safetyFactorTop = verificationHelper.SafetyFactor(forces);
            resultNotOK = !CalculationUtility.IsSafety(safetyFactorTop);
            if (resultNotOK)
            {
               // check capacity for bottom
               verificationHelper.SetReinforcement(asTop[0], asBottom[1] + reinforcementIncraseBottom);
               safetyFactorBottom = verificationHelper.SafetyFactor(forces);
               resultNotOK = !CalculationUtility.IsSafety(safetyFactorBottom);
               if (!resultNotOK)
               {
                  // reinforcement is to big - copied from [1] to [2]
                  asTop[2] = asTop[0];
                  asBottom[2] = asBottom[1] + reinforcementIncraseBottom;
                  safetyFactor = safetyFactorBottom;
               }
            }
            else
            {
               // reinforcement is to big - copied from [1] to [2]
               asTop[2] = asTop[1] + reinforcementIncraseTop;
               asBottom[2] = asBottom[0];
               safetyFactor = safetyFactorBottom;
            }
            if (resultNotOK)
            {
               increaseSafetyFactorTop = safetyFactorTop - safetyFactor;
               increaseSafetyFactorBottom = safetyFactorBottom - safetyFactor;
               if (increaseSafetyFactorTop > 2.0 * increaseSafetyFactorBottom)
               {
                  reinforcementIncraseBottom = 0;
                  reinforcementIncraseTop = reinforcementIncraseTop / increaseSafetyFactorTop;
                  reinforcementIncraseTop *= (1.0-safetyFactor);
               }
               else if (increaseSafetyFactorBottom > 2.0 * increaseSafetyFactorTop)
               {
                  reinforcementIncraseTop = 0;
                  reinforcementIncraseBottom = reinforcementIncraseBottom / increaseSafetyFactorBottom;
                  reinforcementIncraseBottom *= (1.0 - safetyFactor);
               }
               else
               {
                  double coefficientSafetyFactor = safetyFactorTop / (safetyFactorTop + safetyFactorBottom);
                  if (coefficientSafetyFactor > Double.Epsilon)
                  {                     
                     reinforcementIncraseTop = coefficientSafetyFactor * reinforcementIncraseTop / increaseSafetyFactorTop;
                     reinforcementIncraseBottom = (1.0 - coefficientSafetyFactor) * reinforcementIncraseBottom / increaseSafetyFactorBottom;
                     reinforcementIncraseTop *= (1.0 - safetyFactor);
                     reinforcementIncraseBottom *= (1.0 - safetyFactor);
                  }
                  else
                  {
                     reinforcementIncraseTop = reinforcementIncraseBottom = Double.MaxValue;
                  }
               }
               AdjustReinforcementIncrase(ref reinforcementIncraseTop);
               AdjustReinforcementIncrase(ref reinforcementIncraseBottom);
               asTop[1] += reinforcementIncraseTop;
               asBottom[1] += reinforcementIncraseBottom;
               verificationHelper.SetReinforcement(asTop[1], asBottom[1]);
               safetyFactor = verificationHelper.SafetyFactor(forces);
               resultNotOK = !CalculationUtility.IsSafety(safetyFactor);
               if (resultNotOK)
               {
                  /// <structural_toolkit_2015>
                  if ((asTop[1] + asBottom[1]) > 0.5 * sectionGeometry.Area)
                  {
                     throw new Exception(String.Format("Too big reinforcement. Reinforcement for case {0} is more than 50% of section area.", forces.CaseName));
                  }
                  /// </structural_toolkit_2015>
                  // reinforcement is to low  - copied from [1] to [0]
                  asTop[0] = asTop[1];
                  asBottom[0] = asBottom[1];
               }
               else
               {
                  // reinforcement is to big - copied from [1] to [2]
                  asTop[2] = asTop[1];
                  asBottom[2] = asBottom[1];
               }
            }
         }
         if (resultNotOK)
         {
            throw new Exception(String.Format("Too many iterations for case {0}. FindOptimalLongitudinalReinforcementUniaxialUnsymmetrical.",forces.CaseName));
         }
         else
         {
            resultNotOK = !CalculationUtility.IsSafetyOptimal(safetyFactor);
            if (resultNotOK)
            {
               BisectionForReinforcementAdjustment(ref forces, asTop, asBottom);
            }
            else
            {
               longitudinalReinforcement.CurrentAsTop = asTop[1];
               longitudinalReinforcement.CurrentAsBottom = asBottom[1];
            }
         }
      }
      /// <structural_toolkit_2015>
      
      /// <summary>
      /// Adjust the reinforcement increment to the reasonable values.
      /// </summary>
      /// <param name="reinforcementIncrase">The reinforcement increment for adjusting.</param>
      public void AdjustReinforcementIncrase(ref double reinforcementIncrase)
      {
         if (reinforcementIncrase > Double.Epsilon)
         {
            reinforcementIncrase = Math.Max(reinforcementIncrase, CalculationUtility.MinimumIncreaseOfReinforcement());
            reinforcementIncrase = Math.Min(reinforcementIncrase, 0.01 * sectionGeometry.Area);
         }
         else
            reinforcementIncrase = 0;
      }

      /// </structural_toolkit_2015>
      
      /// <summary>
      /// Calculates the area of reinforcement for uniaxial bending with axial force.
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void CalculateLongitudinalReinforcementUniaxial(ref InternalForcesContainer forces)
      {
         double minimumIncreaseReinforcement = CalculationUtility.MinimumIncreaseOfReinforcement();
         if (verificationHelper == null)
         {
            verificationHelper = RcVerificationHelperUtility.CreateRcVerificationHelperUtility(sectionType, ref sectionGeometry, longReinforcementTopCover, longReinforcementBottomCover);
            SetMaterialParameters(forces.LimitState);
         }
         if (symmetricalReinforcementPreferable)
            longitudinalReinforcement.CurrentAsTop = longitudinalReinforcement.CurrentAsBottom = minimumIncreaseReinforcement;
         else if (forces.MomentMy >= 0.0)
         {
            longitudinalReinforcement.CurrentAsBottom = minimumIncreaseReinforcement;
         }
         else
         {
            longitudinalReinforcement.CurrentAsTop = minimumIncreaseReinforcement;
         }
         longitudinalReinforcement.CurrentAsTop = Math.Max(longitudinalReinforcement.CurrentAsTop, longitudinalReinforcement.AsTop);
         longitudinalReinforcement.CurrentAsBottom = Math.Max(longitudinalReinforcement.CurrentAsBottom, longitudinalReinforcement.AsBottom);
         verificationHelper.SetReinforcement(longitudinalReinforcement.CurrentAsTop, longitudinalReinforcement.CurrentAsBottom);
         double safetyFactor = verificationHelper.SafetyFactor(forces);
         if (!CalculationUtility.IsSafety(safetyFactor))
         {
            FindOptimalLongitudinalReinforcementUniaxial(ref safetyFactor, ref forces);
         }
      }
      /// <summary>
      /// Calculates the area of reinforcement for biaxial bending with axial force.
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void CalculateLongitudinalReinforcementBiaxial(ref InternalForcesContainer forces)
      {
         if (verificationHelper == null)
         {
            verificationHelper = RcVerificationHelperUtility.CreateRcVerificationHelperUtility(sectionType, ref sectionGeometry, longReinforcementTopCover, longReinforcementBottomCover);
            SetMaterialParameters(forces.LimitState);
         }
         bool setInitialReinforcement = CalculationUtility.IsZeroReinforcement(longitudinalReinforcement.TotalSectionReinforcement());
         if (setInitialReinforcement)
         {
            longitudinalReinforcement.CurrentAsTop = longitudinalReinforcement.CurrentAsBottom = 2 * longReinforcementArea; // 2 bars on the top and on the bottom;
         }
         else
         {
            longitudinalReinforcement.CurrentAsTop = longitudinalReinforcement.AsTop;
            longitudinalReinforcement.CurrentAsBottom = longitudinalReinforcement.AsBottom;
            longitudinalReinforcement.CurrentAsRight = longitudinalReinforcement.AsRight;
            longitudinalReinforcement.CurrentAsLeft = longitudinalReinforcement.AsLeft;
         }
         verificationHelper.SetReinforcement(longitudinalReinforcement.CurrentAsTop, longitudinalReinforcement.CurrentAsBottom, longitudinalReinforcement.CurrentAsRight, longitudinalReinforcement.CurrentAsLeft);
         double safetyFactor = verificationHelper.SafetyFactor(forces);
         if (!CalculationUtility.IsSafety(safetyFactor))
         {
            FindOptimalLongitudinalReinforcementBiaxial(ref safetyFactor, ref forces);
         }
      }
      /// <summary>
      ///  Searching the best of longitudinal reinforcement.
      /// </summary>
      /// <param name="safetyFactor">Safety factor for current reinforcement.</param>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      /// <remarks>Symmetrical reinforcement</remarks>
      private void FindOptimalLongitudinalReinforcementBiaxial(ref double safetyFactor, ref InternalForcesContainer forces)
      {
         bool onlyTopBottom = false, onlyRightLeft = false, resultNotOK = true;
         double curentReinforcement = longitudinalReinforcement.CurrentAsTop + longitudinalReinforcement.CurrentAsBottom +
      longitudinalReinforcement.CurrentAsRight + longitudinalReinforcement.CurrentAsLeft;
         double reinforcementIncraseTopBottom = 0.5 * (curentReinforcement / safetyFactor - curentReinforcement);
         AdjustReinforcementIncrase(ref reinforcementIncraseTopBottom);
         double reinforcementIncraseRightLeft = reinforcementIncraseTopBottom;
         verificationHelper.SetReinforcement(longitudinalReinforcement.CurrentAsTop + reinforcementIncraseTopBottom,
      longitudinalReinforcement.CurrentAsBottom + reinforcementIncraseTopBottom,
      longitudinalReinforcement.CurrentAsRight, longitudinalReinforcement.CurrentAsLeft);
         double safetyFactorTopBottom = onlyRightLeft ? 0.0 : verificationHelper.SafetyFactor(forces);
         verificationHelper.SetReinforcement(longitudinalReinforcement.CurrentAsTop, longitudinalReinforcement.CurrentAsBottom,
      longitudinalReinforcement.CurrentAsRight + reinforcementIncraseRightLeft,
      longitudinalReinforcement.CurrentAsLeft + reinforcementIncraseRightLeft);
         double safetyFactorRightLeft = onlyTopBottom ? 0.0 : verificationHelper.SafetyFactor(forces);
         int i = 0;
         double[] asTopBottom = new double[] { longitudinalReinforcement.CurrentAsTop, Double.MaxValue, Double.MaxValue };
         double[] asRightLeft = new double[] { longitudinalReinforcement.CurrentAsRight, Double.MaxValue, Double.MaxValue };
         double increaseSafetyFactorTopBottom = 1.0, increaseSafetyFactorRightLeft = 1.0;
         while (!CalculationUtility.IsIterEnd(++i) && resultNotOK)
         {
            increaseSafetyFactorTopBottom = safetyFactorTopBottom - safetyFactor;
            increaseSafetyFactorRightLeft = safetyFactorRightLeft - safetyFactor;
            double dRTopBottom2dSf = increaseSafetyFactorTopBottom > Double.Epsilon ? reinforcementIncraseTopBottom / increaseSafetyFactorTopBottom : 0;
            double dRRightLeft2dSf = increaseSafetyFactorRightLeft > Double.Epsilon ? reinforcementIncraseRightLeft / increaseSafetyFactorRightLeft : 0;
            double dCoef = dRTopBottom2dSf / (dRTopBottom2dSf + dRRightLeft2dSf);
            dCoef *= (1.0 - safetyFactor);
            reinforcementIncraseTopBottom = dCoef * dRTopBottom2dSf;
            reinforcementIncraseRightLeft = (1.0 - dCoef) * dRRightLeft2dSf;
            AdjustReinforcementIncrase(ref reinforcementIncraseTopBottom);
            AdjustReinforcementIncrase(ref reinforcementIncraseRightLeft);
            asTopBottom[1] = asTopBottom[0] + reinforcementIncraseTopBottom;
            asRightLeft[1] = asRightLeft[0] + reinforcementIncraseRightLeft;
            verificationHelper.SetReinforcement(asTopBottom[1], asTopBottom[1], asRightLeft[1], asRightLeft[1]);
            safetyFactor = verificationHelper.SafetyFactor(forces);
            resultNotOK = !CalculationUtility.IsSafety(safetyFactor);
            if (resultNotOK)
            {
               reinforcementIncraseTopBottom = CalculationUtility.MinimumIncreaseOfReinforcement();
               verificationHelper.SetReinforcement(asTopBottom[1] + reinforcementIncraseTopBottom,
      asTopBottom[1] + reinforcementIncraseTopBottom, asRightLeft[1], asRightLeft[1]);
               safetyFactorTopBottom = onlyRightLeft ? 0.0 : verificationHelper.SafetyFactor(forces);
               resultNotOK = !CalculationUtility.IsSafety(safetyFactorTopBottom);
               if (resultNotOK)
               {
                  reinforcementIncraseRightLeft = CalculationUtility.MinimumIncreaseOfReinforcement();
                  verificationHelper.SetReinforcement(asTopBottom[1], asTopBottom[1],
      asRightLeft[1] + reinforcementIncraseRightLeft, asRightLeft[1] + reinforcementIncraseRightLeft);
                  safetyFactorRightLeft = onlyTopBottom ? 0.0 : verificationHelper.SafetyFactor(forces);
                  resultNotOK = !CalculationUtility.IsSafety(safetyFactorRightLeft);
                  if (!resultNotOK)
                  {
                     asTopBottom[2] = asTopBottom[1];
                     asRightLeft[2] = asRightLeft[1];
                     safetyFactor = safetyFactorRightLeft;
                  }
               }
               else
               {
                  asTopBottom[2] = asTopBottom[1];
                  asRightLeft[2] = asRightLeft[1];
                  safetyFactor = safetyFactorTopBottom;
               }
               asTopBottom[0] = asTopBottom[1];
               asRightLeft[0] = asRightLeft[1];
            }
            else
            {
               asTopBottom[2] = asTopBottom[1];
               asRightLeft[2] = asRightLeft[1];
            }
         }
         if (resultNotOK)
            throw new Exception(String.Format("Too many iterations for case {0}. CalculateLongitudinalReinforcementBiaxial.",forces.CaseName));
         else
         {
            resultNotOK = !CalculationUtility.IsSafetyOptimal(safetyFactor);
            if (resultNotOK)
               BisectionForReinforcementAdjustment(ref forces, asTopBottom, asTopBottom, asRightLeft, asRightLeft);
            else
            {
               longitudinalReinforcement.CurrentAsTop = asTopBottom[1];
               longitudinalReinforcement.CurrentAsBottom = asTopBottom[1];
               longitudinalReinforcement.CurrentAsRight = asRightLeft[1];
               longitudinalReinforcement.CurrentAsLeft = asRightLeft[1];
            }
         }
      }

      /// <summary>
      /// Bisection algorithm. It searches the best reinforcement - close to 1.0 capacity.
      /// It checking mean value between current "too high" and "too low" reinforcement values. 
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      /// <param name="asTop">Three element array of top reinforcement. First element it is reinforcement giving too low capacity, third element it is reinforcement giving too high capacity, second is used as "current step".</param>
      /// <param name="asBottom">Three element array of bottom reinforcement. First element it is reinforcement giving too low capacity, third element it is reinforcement giving too high capacity, second is used as "current step".</param>
      /// <param name="asRight">Optional. Three element array of right reinforcement. First element it is reinforcement giving too low capacity, third element it is reinforcement giving too high capacity, second is used as "current step".</param>
      /// <param name="asLeft">Optional. Three element array of left reinforcement. First element it is reinforcement giving too low capacity, third element it is reinforcement giving too high capacity, second is used as "current step".</param>
      private void BisectionForReinforcementAdjustment(ref InternalForcesContainer forces, double[] asTop, double[] asBottom, double[] asRight = null, double[] asLeft = null)
      {
         bool onlyTopBottomReinforcemet = (asRight == null || asLeft == null);
         if ((asTop.Length != 3 || asTop.Length != 3) || (!onlyTopBottomReinforcemet && (asRight.Length != 3 || asLeft.Length != 3)))
            throw new Exception(String.Format("Invalid parameter in BisectionForReinforcementAdjustment for case {0}.",forces.CaseName));
         bool resultNotOK = true;
         double safetyFactor = 0;
         int i = 0;
         while (!CalculationUtility.IsIterEnd(++i) && resultNotOK)
         {
            //pure bisection between [0] and [2] values
            asTop[1] = 0.5 * (asTop[0] + asTop[2]);
            asBottom[1] = 0.5 * (asBottom[0] + asBottom[2]);
            if (onlyTopBottomReinforcemet)
            {
               verificationHelper.SetReinforcement(asTop[1], asBottom[1]);
            }
            else
            {
               asRight[1] = 0.5 * (asRight[0] + asRight[2]);
               asLeft[1] = 0.5 * (asLeft[0] + asLeft[2]);
               verificationHelper.SetReinforcement(asTop[1], asBottom[1], asRight[1], asLeft[1]);
            }
            safetyFactor = verificationHelper.SafetyFactor(forces);
            resultNotOK = CalculationUtility.IsSafetyOptimal(safetyFactor);
            if (resultNotOK)
            {
               if (!CalculationUtility.IsSafety(safetyFactor))
               {
                  asTop[0] = asTop[1];
                  asBottom[0] = asBottom[1];
                  if (!onlyTopBottomReinforcemet)
                  {
                     asRight[0] = asRight[1];
                     asLeft[0] = asLeft[1];
                  }
               }
               else
               {
                  asTop[2] = asTop[1];
                  asBottom[2] = asBottom[1];
                  if (!onlyTopBottomReinforcemet)
                  {
                     asRight[2] = asRight[1];
                     asLeft[2] = asLeft[1];
                  }
               }
            }
            longitudinalReinforcement.CurrentAsTop = resultNotOK ? asTop[2] : asTop[1];
            longitudinalReinforcement.CurrentAsBottom = resultNotOK ? asBottom[2] : asBottom[1];
            if (!onlyTopBottomReinforcemet)
            {
               longitudinalReinforcement.CurrentAsRight = resultNotOK ? asRight[2] : asRight[1];
               longitudinalReinforcement.CurrentAsLeft = resultNotOK ? asLeft[2] : asLeft[1];
            }
         }
      }
      /// <summary>
      ///  Calculates the longitudinal reinforcement.
      /// </summary>
      private void CalculateTransversalReinforcement()
      {
         SetMaterialParameters(ForceLimitState.Uls); // sets material properties for ultimate limit state
         if (transReinforcementInternalForcesULS.Count() > 0)
         {
            foreach (InternalForcesContainer forces in transReinforcementInternalForcesULS)
            {
               if (IsZeroForces(forces, true))
                  continue;                                  // only for no zero forces 
               CalculateTransversalReinforcementULS(forces); // design the reinforcement for single case
               transversalReinforcement.CurrentToFinial();   // creates stirrup spacing envelope (minimum value)- finial reinforcement
            }
         }
         else
         {
            foreach (InternalForcesContainer forces in internalForces)
            {
               if (ForceLimitState.Sls == forces.LimitState)
                  continue;   // only for ultimate limit state 
               if (IsZeroForces(forces, true))
                  continue;
               CalculateTransversalReinforcementULS(forces); // design the reinforcement for single case
               transversalReinforcement.CurrentToFinial();  // creates stirrup spacing envelope (minimum value)- finial reinforcement
            }
         }
         transversalReinforcement.SetTransversalDensity(transReinforcementNumberOfLegs, transReinforcementArea); //sets transversal reinforcemen density for 2 arms stirrups
      }
      /// <summary>
      /// Runs calculation of transversal reinforcement in ULS state dependencies to forces and calculation type.
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void CalculateTransversalReinforcementULS(InternalForcesContainer forces)
      {
         switch (transReinforcementCalculationType)
         {
            // Pure uniaxial shearing
            case CodeCheckingConcreteExample.ConcreteTypes.CalculationType.ShearingZ:
               CalculateTransversalReinforcementPureShear(forces.ForceFz, sectionHeight);
               break;
            // Pure torsion
            case CodeCheckingConcreteExample.ConcreteTypes.CalculationType.Torsion:
               CalculateTransversalReinforcementPureTorsion(forces.MomentMx);
               break;
            // Uniaxial shearing with torsion
            case CodeCheckingConcreteExample.ConcreteTypes.CalculationType.TorsionWithShearingZ:
               CalculateTransversalReinforcementShearTorsion(forces.ForceFy, forces.MomentMx, sectionHeight);
               break;
            default:
               {
                  // Pure torsion Fz == 0 && Fy == 0
                  if (CalculationUtility.IsZeroN(forces.ForceFz) && CalculationUtility.IsZeroN(forces.ForceFy))
                     CalculateTransversalReinforcementPureTorsion(forces.MomentMx);
                  // Pure uniaxial shearing Mx == 0 && Fy == 0
                  else if (CalculationUtility.IsZeroM(forces.MomentMx) && CalculationUtility.IsZeroN(forces.ForceFy))
                     CalculateTransversalReinforcementPureShear(forces.ForceFz, sectionHeight);
                  // Uniaxial shearing with torsion
                  else if (CalculationUtility.IsZeroN(forces.ForceFy))
                     CalculateTransversalReinforcementShearTorsion(forces.ForceFy, forces.MomentMx, sectionHeight);
                  // The general case
                  else
                     CalculateTransversalReinforcementGeneral(ref forces);
               }
               break;
         }
      }
      /// <summary>
      /// Calculates stirrup spacing for pure uniaxial shearing
      /// </summary>
      /// <param name="V">Shear force</param>
      /// <param name="dim">Dimension of cross section parallel to the shear force. </param>
      private void CalculateTransversalReinforcementPureShear(double V, double dim)
      {
         double vAbs = Math.Abs(V);
         double vRdc = (0.02 * Math.Pow(concreteParameters.DesignStrength * 1e-6, 0.3) * sectionGeometry.Area) * 1e6; // 0.01*fcd[MPa]
         if (vAbs > vRdc)
         {
            double vRdmax = Vrdmax();
            if (vAbs > vRdmax)
            {
               throw new Exception("Shear force is too large.");
            }
            else
            {
               // V <= Asw/s * z * fy 
               // z = 0.9 * d & d = 0.9 * h =>  0.81 * h 
               // 2 lags stirrups => Asw = 2 * barArea
               // in typical sytuation dim == sectionHeight
               transversalReinforcement.CurrentSpacing = (1.62 * transReinforcementArea * dim * longReinforcementFy) / vAbs;
            }
         }
      }
      /// <summary>
      /// Maximum shear resistance. Depends to the code.
      /// </summary>
      /// <returns>Maximum shear resistance.</returns>
      private double Vrdmax()
      {
         return 0.5 * sectionWidth * concreteParameters.DesignStrength;
      }
      /// <summary>
      /// Calculates stirrup spacing for torsion.
      /// </summary>
      /// <param name="T">Torsion moment.</param>
      private void CalculateTransversalReinforcementPureTorsion(double T)
      {
         if (SectionShapeType.RectangularBar == sectionType)
         {
            double tAbs = Math.Abs(T);
            Dictionary<string, double> tp = TorsionParameters();
            double tRdmax = tp["tArea"] * tp["tPerimeter"] * concreteParameters.DesignStrength;
            if (tAbs > tRdmax)
            {
               throw new Exception("Torsional moment is too large.");
            }
            else
            {
               double externalArea = 2.0 * transReinforcementArea;
               transversalReinforcement.CurrentSpacing = (tp["tArea"] * transversalReinforcement.Strength * externalArea) / (0.5 * tAbs);
               transversalReinforcement.CurrentAsBottom = transversalReinforcement.CurrentAsTop = 0.5 * (tp["tPerimeter"] * tAbs / (2.0 * tp["tArea"] * longitudinalReinforcement.Strength));
            }
         }
         else
         {
            throw new Exception("Invalid section for torsion. Only rectangular cross-sections can be used on this path.");
         }
      }
      /// <summary>
      /// Calculates parameters necessary to designing against to torsion. Depends to the code.
      /// </summary>
      /// <returns>Thickness of the shear flow path, torsion area and torsion perimeter.</returns>
      private Dictionary<string, double> TorsionParameters()
      {
         double shearFlowPathThin = sectionGeometry.Area / sectionGeometry.Perimeter;
         double tArea = (sectionWidth - 0.5 * shearFlowPathThin) * (sectionHeight - 0.5 * shearFlowPathThin);
         double tPerimeter = 2.0 * (sectionWidth - 0.5 * shearFlowPathThin) + 2.0 * (sectionHeight - 0.5 * shearFlowPathThin);
         Dictionary<string, double> tp = new Dictionary<string, double> { 
                                    { "shearFlowPathThin", shearFlowPathThin }, 
                                    { "tArea", tArea }, 
                                    { "tPerimeter", tPerimeter } };
         return tp;
      }
      /// <summary>
      /// Calculates stirrup spacing for uniaxial shearing with torsion.
      /// </summary>
      /// <param name="V">Shear force.</param>
      /// <param name="T">Torsional moment.</param>
      /// <param name="dim">Dimension of cross section parallel to the shear force. </param>
      private void CalculateTransversalReinforcementShearTorsion(double V, double T, double dim)
      {

         double vAbs = Math.Abs(V);
         double vRdmax = Vrdmax();
         double tAbs = Math.Abs(T);
         double tRdmax = 0;
         if (SectionShapeType.RectangularBar == sectionType)
         {
            Dictionary<string, double> tp = TorsionParameters();
            tRdmax = tp["tArea"] * tp["tPerimeter"] * concreteParameters.DesignStrength;
         }
         else
         {
            throw new Exception("Invalid section for torsion. Only rectangular cross-sections can be used on this path.");
         }
         double coeffMax = vAbs / vRdmax + tAbs / tRdmax;
         if (coeffMax > 1.0)
         {
            throw new Exception("Shear and torsions forces are too large.");
         }
         else
         {
            CalculateTransversalReinforcementPureShear(vAbs, dim);
            double tempSpacing = transversalReinforcement.CurrentSpacing;
            CalculateTransversalReinforcementPureTorsion(vAbs);
            transversalReinforcement.CurrentSpacing = 1.0 / (1.0 / transversalReinforcement.CurrentSpacing + 1.0 / tempSpacing);
         }
      }
      /// <summary>
      /// Calculates stirrup spacing for general case.
      /// </summary>
      /// <param name="forces">Set of internal forces according to one single combination or single case.</param>
      private void CalculateTransversalReinforcementGeneral(ref InternalForcesContainer forces)
      {
         double vAbs = Math.Sqrt(Math.Pow(forces.ForceFy, 2.0) + Math.Pow(forces.ForceFz, 2.0));
         double vRdmax = Vrdmax();
         if (vAbs > vRdmax)
         {
            throw new Exception("Shear forces and torsion are too large.");
         }
         else
         {
            if (Math.Abs(forces.ForceFz) > Math.Abs(forces.ForceFy))
            {
               CalculateTransversalReinforcementShearTorsion(vAbs, forces.MomentMx, sectionHeight);
            }
            else
            {
               CalculateTransversalReinforcementShearTorsion(vAbs, forces.MomentMx, sectionWidth);
            }
         }
      }
      /// <summary>
      /// Set the partial coefficient according to the limit state.
      /// </summary>
      /// <param name="state">The limit state.</param>
      private void SetMaterialParameters(ForceLimitState state)
      {
         longitudinalReinforcement.ModulusOfElasticity = 200e9;
         switch (state)
         {
            default:
            case ForceLimitState.Uls:
               concreteParameters.SetStrainStressModelRectangular(concreteFc / 1.5, 0.0035, concreteYoungModulus, 0.8);
               longitudinalReinforcement.SetStrenghtPartialFactor(1.15);
               transversalReinforcement.SetStrenghtPartialFactor(1.15);
               break;
            case ForceLimitState.Sls:
               concreteParameters.SetStrainStressModelLinear(concreteFc, concreteFc / concreteYoungModulus, concreteYoungModulus);
               longitudinalReinforcement.SetStrenghtPartialFactor(1.0);
               transversalReinforcement.SetStrenghtPartialFactor(1.0);
               break;
         }
         if (verificationHelper != null)
         {
            verificationHelper.SetConcrete(concreteParameters);
            verificationHelper.SetSteel(longitudinalReinforcement.Strength, longitudinalReinforcement.ModulusOfElasticity, 0.01, 1.0);
         }
      }
   }
}
