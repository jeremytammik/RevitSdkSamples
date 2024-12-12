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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.ExternalService;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;

namespace Revit.SDK.Samples.RebarFreeForm.CS
{
   /// <summary>
   /// Class used to represent a structural face that is part of a rebar constraint.
   /// </summary>
   class TargetFace
   {
      public TargetFace()
      {
         Transform = Transform.Identity;
         Offset = 0.0;
         Face = null;
      }
      //Actual face to constrain to
      public Face Face{ get; set;}
      //The transform of the geometry element where the face belongs
      public Transform Transform { get; set; }
      //offset value used for calculating bars
      public double Offset { get; set; }
   }
   /// <summary>
   /// Enum defining the custom handles used by this server to identify the different custom constraints
   /// </summary>
   enum BarHandle { FirstHandle, SecondHandle, ThirdHandle, StartHandle, EndHandle };

   /// <summary>
   /// Implements the Revit add-in interface IRebarUpdateServer;
   /// This class is an external server that is capable of calculating 
   /// straight sets of bars of variable length, following the constrained structural planar faces;
   /// The Rebar FreeForm element created using this server will have 3 custom Rebar Handles, 
   /// that can each constrain one planar face, and Start/End handles that will search for targets 
   /// to constrain automatically, then adjust the curves accordingly.
   /// Bar geometry results from intersecting faces and interpolation from the intersection results:
   /// - First bar is the intersection of First Handle target with Second Handle target;
   /// - Last bar is the intersection of First Handle target with Third Handle target;
   /// - All other bars are created between the first and last bar so that they have equal distance between them.
   /// </summary>
   class RebarUpdateServer : Autodesk.Revit.DB.Structure.IRebarUpdateServer
   {
      #region Class Members
      /// <summary>
      /// SampleGuid represents the Guid used by the Revit ExternalService framework to identify this custom IRebarUpdateServer
      /// For a Rebar to use this custom external server, pass this Guid to the Rebar.CreateFreeForm(..) function.
      /// </summary>
      public static System.Guid SampleGuid = new Guid("64D176BA-EB3E-4E96-877D-46A3B0C17B93");
      #endregion

      #region Class Interface Implementation

      /// <summary>
      /// Returns the unique id of this server
      /// </summary>
      public System.Guid GetServerId()
      {
         return SampleGuid;
      }
      /// <summary>
      /// returns the id of the service that handles this server
      /// </summary>
      public ExternalServiceId GetServiceId()
      {
         return ExternalServices.BuiltInExternalServices.RebarUpdateService;
      }
      /// <summary>
      /// Returns name of the server
      /// </summary>
      public System.String GetName()
      {
         return "RebarUpdateServerSample";
      }
      /// <summary>
      /// Returns information about the vendor.
      /// </summary>
      public System.String GetVendorId()
      {
         return "ADSK";
      }
      /// <summary>
      /// Returns description of this server.
      /// </summary>
      public System.String GetDescription()
      {
         return "Sample to demonstrate implementing an external server to handle rebar constraints calculation";
      }
      /// <summary>
      /// Function used to define the Rebar Handles used by this server to calculate the constraints when regenerating the Rebar element.
      /// Rebar handles represent abstract "parts" of the Rebar that can be custom constrained to one or more targets.
      /// A custom Rebar Handle is defined with a unique key (int),
      /// that the external server uses to identify each RebarConstraint that is attached to the Rebar and compute accordingly.
      /// </summary>
      /// <param name="data">Class used to pass information from the external application to the internal Rebar Element.
      /// data receives the custom, start and end Rebar handle definitions used by this server 
      /// </param>
      /// <returns> true if handle definition was completed successfully, false otherwise</returns>
      public bool GetCustomHandles(RebarHandlesData data)
      {
         data.AddCustomHandle((int)BarHandle.FirstHandle);
         data.AddCustomHandle((int)BarHandle.SecondHandle);
         data.AddCustomHandle((int)BarHandle.ThirdHandle);
         data.SetStartHandle((int)BarHandle.StartHandle);
         data.SetEndHandle((int)BarHandle.EndHandle);
         return true;
      }
      /// <summary>
      /// Function used to compute the custom RebarHandle position in respect to the Rebar geometry, 
      /// for display of graphical controls during GraphicalConstraintsManager edit mode
      /// </summary>
      /// <param name="data">Class used to pass information between the external application and the internal Rebar Element.
      /// data exposes geometry to the external application and receives the calculated absolute positions of the handles in the model space
      /// </param>
      /// <returns> true if execution was completed successfully, false otherwise</returns>
      public bool GetHandlesPosition(RebarHandlePositionData data)
      {
         if (data.GetNumberOfBars() <= 0)
            return false;
         
         IList<Curve> firstBar = data.GetBarGeometry(0);
         data.SetPosition((int)BarHandle.FirstHandle, firstBar[0].Evaluate(0.5, true));
         data.SetPosition((int)BarHandle.SecondHandle, firstBar[0].Evaluate(0.3, true));
         data.SetPosition((int)BarHandle.ThirdHandle, firstBar[0].Evaluate(0.7, true));
         data.SetPosition((int)BarHandle.StartHandle, firstBar[0].Evaluate(0, true));
         data.SetPosition((int)BarHandle.EndHandle, firstBar[0].Evaluate(1, true));
         return true;
      }
      /// <summary>
      /// Function resolves the User-facing name for the custom-defined Rebar handles
      /// </summary>
      /// <param name="handleNameData">Class used to pass information from the external application to the internal Rebar Element.
      /// data receives the name for the Rebar Handle it specifies
      /// </param>
      /// <returns> true if operation was completed successfully, false otherwise</returns>
      public bool GetCustomHandleName(RebarHandleNameData handleNameData)
      {
         switch (handleNameData.GetCustomHandleTag())
         {
            case (int)BarHandle.FirstHandle:
               handleNameData.SetCustomHandleName("First Handle");
               break;
            case (int)BarHandle.SecondHandle:
               handleNameData.SetCustomHandleName("Second Handle");
               break;
            case (int)BarHandle.ThirdHandle:
               handleNameData.SetCustomHandleName("Third Handle");
               break;
            default:
               return false;
         }
         return true;
      }
      /// <summary>
      /// Function used to compute the geometry information of the Rebar element during document regeneration.
      /// Geometry information includes: 
      ///  1. Graphical representation of the Rebar or Rebar Set; 
      ///  2. Hook placement;
      ///  3. Distribution Path for MRA;
      ///  
      /// </summary>
      /// <param name="data">Class used to pass information from the external application to the internal Rebar Element.
      /// Interfaces with the Rebar Element and exposes information needed for geometric calculation during regeneration,
      /// such as constrained geometry, state of changed input information, etc.
      /// Receives the result of the custom constraint calculation and 
      /// updates the element after the entire function finished successfully.
      /// </param>
      /// <returns> true if geometry generation was completed successfully, false otherwise</returns>
      public bool GenerateCurves(RebarCurvesData data)
      {
         // used to store the faces and transforms used in generation of curves
         TargetFace firstFace = new TargetFace();
         TargetFace secondFace = new TargetFace();
         TargetFace thirdFace = new TargetFace();

         //iterate through the available constraints and extract the needed information
         IList<RebarConstraint> constraints = data.GetRebarUpdateCurvesData().GetCustomConstraints();
         foreach (RebarConstraint constraint in constraints)
         {
            if (constraint.NumberOfTargets > 1)
               return false;
            Transform tempTrf = Transform.Identity;
            double dfOffset = 0;
            if (!getOffsetFromConstraintAtTarget(data.GetRebarUpdateCurvesData(), constraint, 0, out dfOffset))
               return false;

            switch ((BarHandle)constraint.GetCustomHandleTag())
            {
               case BarHandle.FirstHandle:
                  {
                     Face face = constraint.GetTargetHostFaceAndTransform(0, tempTrf);
                     firstFace = new TargetFace() { Face = face, Transform = tempTrf, Offset = dfOffset};
                     break;
                  }
               case BarHandle.SecondHandle:
                  {
                     Face face = constraint.GetTargetHostFaceAndTransform(0, tempTrf);
                     secondFace = new TargetFace() { Face = face, Transform = tempTrf, Offset = dfOffset };
                     break;
                  }
               case BarHandle.ThirdHandle:
                  {
                     Face face = constraint.GetTargetHostFaceAndTransform(0, tempTrf);
                     thirdFace = new TargetFace() { Face = face, Transform = tempTrf, Offset = dfOffset };
                     break;
                  }
               default:
                  break;
            }
         }
         // check if all the input is present for the calculation, otherwise return error(false).
         if (firstFace.Face == null || secondFace.Face == null || thirdFace.Face == null)
            return false;

         //used to store the resulting curves
         List<Curve> curves = new List<Curve>();

         Curve singleBar = getOffsetCurveAtIntersection(firstFace, secondFace);
         //we can't make any more bars without the first one.
         if (singleBar == null)
            return false;

         // check the layout rule to see if we need to create more bars
         // for this example, any rule that is not single will generate bars in the same way, 
         // creating them at an equal distance to each other, based only on number of bars
         RebarLayoutRule layout = data.GetRebarUpdateCurvesData().GetLayoutRule();
         switch (layout)
         {
            case RebarLayoutRule.Single:// first bar creation: intersect first face with second face to get a curve
               curves.Add(singleBar);
               break;
            case RebarLayoutRule.FixedNumber:
            case RebarLayoutRule.NumberWithSpacing:
            case RebarLayoutRule.MaximumSpacing:
            case RebarLayoutRule.MinimumClearSpacing:
               curves.Add(singleBar);
               Curve lastBar = getOffsetCurveAtIntersection(firstFace, thirdFace);// create last bar
               // keep the curves pointing in the same direction
               if (lastBar == null || !alignBars(ref singleBar, ref lastBar))
                  return false;
               if (!generateSet(singleBar, lastBar, layout, 
                                data.GetRebarUpdateCurvesData().GetBarsNumber(), 
                                data.GetRebarUpdateCurvesData().Spacing, ref curves)) 
                  return false;
               curves.Add(lastBar);
               break;
            default:
               break;
         }
      
         // check if any curves were created 
         if (curves.Count <= 0)
            return false;

         // create the distribution path for the bars that were created;
         // one single bar will not have a distribution path.
         List<Curve> distribPath = new List<Curve>();
         for (int ii = 0; ii < curves.Count - 1; ii++)
            distribPath.Add(Line.CreateBound(curves[ii].Evaluate(0.5, true), curves[ii + 1].Evaluate(0.5, true)));
         // set distribution path if we have a path created
         if (distribPath.Count>0)
            data.SetDistributionPath(distribPath);

         // add each curve as separate bar in the set.
         for (int ii = 0; ii < curves.Count; ii++)
         {
            List<Curve> barCurve = new List<Curve>();
            barCurve.Add(curves[ii]);
            data.AddBarGeometry(barCurve);

            // set the hook normals for each bar added
            // important!: hook normals set here will be reset if bar geometry is changed on TrimExtendCurves
            // so they need to be recalculated then.
            for (int i = 0; i < 2; i++)
            {
               XYZ normal = computeNormal(curves[ii], firstFace, i);
               if (normal != null)
                  data.GetRebarUpdateCurvesData().SetHookPlaneNormalForBarIdx(i, ii, normal);
            }
         }
         return true;
      }
      /// <summary>
      /// Function used to adjust the computed geometry information of the rebar element and has two logical parts:
      ///  - Selection of structural faces for creation of Start of Bar and End of Bar Constraints when needed
      ///  (Constraints created here are visible and modifiable in the Graphical Constraints Manager in native Revit)
      ///      The constraint search is done by listing all the faces from the structural pointed to by the FirstHandle constraint
      ///      and then picking the face that has the closest intersection point with either the curves in the rebar, or their extensions
      ///  - Adjustments are done to the start/end of the bars according to the corresponding constraints
      ///      Each bar will be lengthened to the intersection point of the tangent in the curve's specified end with the corresponding constraint face, 
      ///      or it will be shortened to the intersection point of the curve itself with the corresponding constraint face.
      ///  - Hook normals for each bar are calculated for the newly modified curves.
      ///  This function is called after the successful execution of GenerateCurves
      /// </summary>
      /// <param name="data">Class used to pass information from the external application to the internal Rebar Element.
      /// Interfaces with the Rebar Element and exposes information needed for Constraint creation, face searching, and
      /// receives the result of the Start/End constraint calculation.
      /// updates are done on the element after the entire function finished successfully.
      /// </param>
      /// <returns> true if execution was completed successfully, false otherwise</returns>
      public bool TrimExtendCurves(RebarTrimExtendData data)
      {
         // extract the curves from the element.
         IList<Curve> allbars = new List<Curve>();
         for(int ii =0; ii< data.GetRebarUpdateCurvesData().GetBarsNumber(); ii++)
            allbars.Add(data.GetRebarUpdateCurvesData().GetBarGeometry(ii)[0]);

         // Place for caching the faces of the host used in constraint search.
         List<TargetFace> hostFaces = new List<TargetFace>();

         // repeat process for each end of the Rebar.
         for (int iBarEnd = 0; iBarEnd < 2; iBarEnd++)
         {
            List<TargetFace> faces = new List<TargetFace>();
            // get current Start/End constraint
            RebarConstraint constraint = (iBarEnd == 0) ? data.GetRebarUpdateCurvesData().GetStartConstraint() :
                                                          data.GetRebarUpdateCurvesData().GetEndConstraint();

            //if no constraint present, then search for a new one 
            if (constraint == null)
            {
               if (hostFaces.Count <= 0)// fetch the faces of the structural used for searching constraints.
               {
                  // used compute references to true to make sure we can create constraints with the faces we find
                  Options geomOptions = new Options();
                  geomOptions.ComputeReferences = true;
                  // the host structural is considered the first structural in the first constraint
                  GeometryElement elemGeometry = data.GetRebarUpdateCurvesData().GetCustomConstraints()[0].GetTargetElement(0).get_Geometry(geomOptions);
                  if (elemGeometry == null)
                     return false;
                  hostFaces = getFacesFromElement(elemGeometry);
               }
               
               // for each bar try to find the closest face that intersects with it, or its extension, at the specified end
               for (int idx = 0; idx < allbars.Count; idx++)
                  faces.Add(searchForFace(allbars[idx], hostFaces, iBarEnd));
               
               // gather valid references for constraint creation
               List<Reference> refs = new List<Reference>();
               foreach (TargetFace face in faces)
                  if (face.Face.Reference != null && !refs.Contains(face.Face.Reference))
                     refs.Add(face.Face.Reference);

               // if we have any valid references, we create the constraint for the specified bar end.
               if (refs.Count > 0)
               {
                  if (iBarEnd == 0)
                     data.CreateStartConstraint(refs, false, 0.0);
                  else
                     data.CreateEndConstraint(refs, false, 0.0);
               }
            }
            else// if constraint is present, extract needed information to calculate trim/extend
               for (int nTarget = 0; nTarget < constraint.NumberOfTargets; nTarget++)
               {
                  var trf = Transform.Identity;
                  Face constrainedFace = constraint.GetTargetHostFaceAndTransform(nTarget, trf);
                  if (constrainedFace == null)
                     continue;
                  double dfOffset;
                  if(getOffsetFromConstraintAtTarget(data.GetRebarUpdateCurvesData(), constraint, 0, out dfOffset))
                  faces.Add(new TargetFace() { Face = constrainedFace, Transform = trf , Offset = dfOffset});
               }

            // for each bar, find out where it intersects with the selected faces and replace the original curve with a new one that is shorter or longer.
            // first search for extension intersection (use tangent curve in the end point of the curve), then search for actual curve intersection
            for (int idx=0; idx<allbars.Count;idx++)
            {
               XYZ intersection;
               Curve barCurve = allbars[idx];
               if (!(barCurve is Line))// this code only deals with input curves that are straight lines
                  return false;
               Line tangent = Line.CreateUnbound(barCurve.GetEndPoint(iBarEnd), barCurve.ComputeDerivatives(iBarEnd, true).BasisX.Normalize() * (iBarEnd == 0 ? -1 : 1));
               if (getIntersection(tangent, faces, out intersection) || getIntersection(barCurve, faces, out intersection))
               {
                  Curve newCurve = null;
                  try
                  {
                     newCurve = (iBarEnd == 0) ? Line.CreateBound(intersection, barCurve.GetEndPoint(1)) :
                                                       Line.CreateBound(barCurve.GetEndPoint(0), intersection);
                  }
                  catch {}
                  // if new curve available, replace the old one.
                  if (newCurve != null)
                     allbars[idx] = newCurve;
               }
            }
         }

         // get the FirstHandle constraint and extract the target face to use in determining the hook orientation for each bar
          TargetFace firstFace = new TargetFace();
          IList<RebarConstraint> constraints = data.GetRebarUpdateCurvesData().GetCustomConstraints();
          foreach (RebarConstraint constraint in constraints)
             if ((BarHandle)constraint.GetCustomHandleTag() == BarHandle.FirstHandle)
             {
                Transform tempTrf = Transform.Identity;
               double dfOffset;
               if (!getOffsetFromConstraintAtTarget(data.GetRebarUpdateCurvesData(), constraint, 0, out dfOffset))
                  return false;
                firstFace = new TargetFace() { Face = constraint.GetTargetHostFaceAndTransform(0, tempTrf), Transform = tempTrf, Offset = dfOffset };
                break;
             }
           
          // add each curve as separate bar in the set.
          for (int ii = 0; ii < allbars.Count; ii++)
          {
            List<Curve> barCurve = new List<Curve>();
            barCurve.Add(allbars[ii]);
            data.AddBarGeometry(barCurve);
            // hook normals are reset when adding new bar geometry, so  we need to
            // set the hook normals for each bar that was modified
            for (int i = 0; i < 2; i++)
            {
               XYZ normal = computeNormal(allbars[ii], firstFace, i);
               if (normal != null)
                  data.GetRebarUpdateCurvesData().SetHookPlaneNormalForBarIdx(i, ii, normal );
            }
         }
         return true;
      }
      #endregion

      #region Class Implementations

      /// <summary>
      /// function used to extract offset value from constraint
      /// </summary>
      /// <param name="updateData"> data used to pass or get information regarding constraints cover</param>
      /// <param name="constraint">constraint from which we extract the offset information</param>
      /// <param name="targetIdx">index of target in constraint</param>
      /// <param name="offset"> output value </param>
      /// <returns></returns>
      public static bool getOffsetFromConstraintAtTarget(RebarUpdateCurvesData updateData, RebarConstraint constraint, int targetIdx, out double offset)
      {
         offset = 0.0;
         if (updateData == null || constraint == null)
            return false;

         double barDiam = updateData.GetBarDiameter();
         var rebarStyle = updateData.GetRebarStyle();
         var attachment = updateData.GetAttachmentType();
         bool bIsInside = rebarStyle == RebarStyle.Standard || (rebarStyle != RebarStyle.Standard && attachment == StirrupTieAttachmentType.InteriorFace);

         if (constraint.IsToCover())
         {
            if (targetIdx < 0 || targetIdx >= constraint.NumberOfTargets)
               return false; // incorrect index
            RebarCoverType coverType = constraint.GetTargetCoverType(targetIdx);
            double coverDist = (coverType == null) ? 0.0 : coverType.CoverDistance;
            double diameterOffset = (barDiam / 2);
            if (bIsInside)
               diameterOffset *= -1;
            offset = constraint.GetDistanceToTargetCover() - coverDist + diameterOffset;
            return true;
         }

         offset = constraint.GetDistanceToTargetHostFace();
         return true;
      }
      /// <summary>
      /// function that finds the closest face to a specified end of a curve of the direction of the curve
      /// </summary>
      /// <param name="curve">
      /// curve used to find the closest face
      /// </param>
      /// /// <param name="faces">
      /// list of faces that are parsed to find the closest one
      /// </param>
      /// /// <param name="iEnd">
      /// input parameter specifying the curve end for wich the search is taking place
      /// </param>
      /// <returns> the FaceTrf that is closest to the curve end</returns>
      private TargetFace searchForFace(Curve curve, List<TargetFace> faces, int iEnd)
      {
         TargetFace bestFace = new TargetFace();
         double minDistance = Double.MaxValue;
         // create tangent to find intersections on the curve's extension
         Line tangent = Line.CreateUnbound(curve.GetEndPoint(iEnd), curve.ComputeDerivatives(iEnd, true).BasisX.Normalize() * (iEnd == 0 ? -1 : 1));
         // iterate through faces and keep the face closest to the specified end of the curve        
         foreach (TargetFace hostFace in faces)
         {
            IntersectionResultArray results;
            // intersect tangent to find faces outside the curve
            if (hostFace.Face.Intersect(tangent.CreateTransformed(hostFace.Transform.Inverse), out results) == SetComparisonResult.Overlap)
            {
               foreach (IntersectionResult intersect in results)
               {
                  double distance = hostFace.Transform.OfPoint(intersect.XYZPoint).DistanceTo(curve.GetEndPoint(iEnd));
                  // if intersection is not on the curve( "behind" the tangent origin, considering the direction),
                  // and the distance from the end of the curve to the face is the smallest, keep face.
                  double param = tangent.Project(hostFace.Transform.OfPoint(intersect.XYZPoint)).Parameter;
                  if ( param >= 0 && distance < minDistance)
                  {
                     bestFace = hostFace;
                     minDistance = distance;
                     continue;
                  }
               }
            }
            if (hostFace.Face.Intersect(curve.CreateTransformed(hostFace.Transform.Inverse), out results) == SetComparisonResult.Overlap)
            {
               foreach (IntersectionResult intersect in results)
               {
                  double distance = hostFace.Transform.OfPoint(intersect.XYZPoint).DistanceTo(curve.GetEndPoint(iEnd));
                  if (distance < minDistance)
                  {
                     bestFace = hostFace;
                     minDistance = distance;
                     continue;
                  }
               }
            }
         }
         return bestFace; 
      }

      /// <summary>
      /// calculates the normal of the plane in which hooks for a certain curve should bend.
      /// </summary>
      /// <param name="curve">
      /// hook normal is calculated for this curve
      /// </param>
      /// /// <param name="face">
      /// face used as a reference for finding the hook normal, together with the curve
      /// </param>
      /// /// <param name="iEnd">
      /// specifies the end at which the hook normal to be calculated
      /// </param>
      /// <returns> the plane normal that was calculated</returns>
      private XYZ computeNormal(Curve curve, TargetFace face, int iEnd)
      {
         XYZ curveTangent = curve.ComputeDerivatives(iEnd, true).BasisX.Normalize();
         XYZ refPoint = curve.GetEndPoint(iEnd);
         IntersectionResult proj = face.Face.Project(face.Transform.Inverse.OfPoint(refPoint));
         if (proj == null)
            return null;
         return face.Face.ComputeNormal(proj.UVPoint).Negate().CrossProduct(curveTangent);
      }

      /// <summary>
      /// function that tries to find the first intersection 
      /// between the provided curve and one of the faces provided.
      /// </summary>
      /// <param name="curve">
      /// curve that is to be intersected with the faces provided
      /// </param>
      /// /// <param name="faces">
      /// list of faces that are used to find an intersection
      /// </param>
      /// /// <param name="intersection">
      /// output parameter to return the intersection point.
      /// </param>
      /// <returns> true if an intersection was found, false otherwise</returns>
      private bool getIntersection(Curve curve, List<TargetFace> faces, out XYZ intersection)
      {
         foreach (TargetFace face in faces)
         {
            IntersectionResultArray results;
            Curve curveTrf = curve.CreateTransformed(face.Transform.Inverse);
            if (face.Face.Intersect(curveTrf, out results) == SetComparisonResult.Overlap)
               foreach (IntersectionResult result in results)
               {
                  if (curveTrf.Project(result.XYZPoint).Parameter < 0)
                     continue;
                  intersection = face.Transform.OfPoint(result.XYZPoint);
                  return true;
               }
         }
         intersection = new XYZ();
         return false;
      }
      /// <summary>
      /// Function that generates the bars between the first and last bar of the set, according to the layout rule 
      /// </summary>
      /// <param name="firstCurve"></param>
      /// <param name="lastCurve"></param>
      /// <param name="layout"></param>
      /// <param name="nbOfBars"></param>
      /// <param name="spacing"></param>
      /// <param name="curves"></param>
      /// <returns></returns>
      private bool generateSet(Curve firstCurve, Curve lastCurve, RebarLayoutRule layout, int nbOfBars, double spacing, ref List<Curve> curves)
      {
         try
         {
            Line startLine = Line.CreateBound(firstCurve.Evaluate(0, true), lastCurve.Evaluate(0, true));
            Line endLine = Line.CreateBound(firstCurve.Evaluate(1, true), lastCurve.Evaluate(1, true));
            int barNumber = nbOfBars-2;
            //see how many bar we can fit
            int numberOfBarsWhichCanFit = (int)((startLine.Length - double.Epsilon) / spacing) + 2;
            if (layout == RebarLayoutRule.NumberWithSpacing && numberOfBarsWhichCanFit != nbOfBars) //check if required number of bars fits between ends  
               return false;
            if (layout == RebarLayoutRule.MaximumSpacing ||
                layout == RebarLayoutRule.MinimumClearSpacing)
               barNumber = numberOfBarsWhichCanFit - 2;
            
            double nEval = 0.0;
            for (int ii = 0; ii < barNumber; ii++)
            {
               nEval = (double)(ii + 1) / (double)(barNumber + 1);
               curves.Add(Line.CreateBound(startLine.Evaluate(nEval, true), endLine.Evaluate(nEval, true)));
            }
         }catch
         {
            return false;
         }
         return true;
      }
      /// <summary>
      /// Function that checks if two bars(Curves) have the same "direction"
      /// </summary>
      /// <param name="firstBar">bar that stays put, e.g. gives the wanted direction</param>
      /// <param name="secondBar">bar that flips if it's direction is not the same as the first</param>
      /// <returns></returns>
      private bool alignBars(ref Curve firstBar, ref Curve secondBar)
      {
         try
         {
            if (firstBar.Evaluate(0, true).DistanceTo(secondBar.Evaluate(0, true)) >
                firstBar.Evaluate(0, true).DistanceTo(secondBar.Evaluate(1, true)))
               secondBar = Line.CreateBound(secondBar.GetEndPoint(1), secondBar.GetEndPoint(0));
         }
         catch { return false; }
         return true;
      }
      /// <summary>
      /// Function used to intersect 2 faces to obtain an offseted curve.
      /// </summary>
      /// <param name="firstFace"></param>
      /// <param name="secondFace"></param>
      /// <returns></returns>
      private Curve getOffsetCurveAtIntersection(TargetFace firstFace, TargetFace secondFace)
      {
         Curve firstCurve;
         FaceIntersectionFaceResult result = firstFace.Face.Intersect(secondFace.Face, out firstCurve);
         // if faces do not intersect, or do not return a Line, then consider the input invalid and return error
         if (result == FaceIntersectionFaceResult.NonIntersecting || !(firstCurve is Line))
            return null;
         XYZ pointOnCurve = firstCurve.Evaluate(0, true);
         XYZ FirstOffsetVec = firstFace.Face.ComputeNormal(firstFace.Face.Project(pointOnCurve).UVPoint).Normalize();
         XYZ SecondOffsetVec = secondFace.Face.ComputeNormal(secondFace.Face.Project(pointOnCurve).UVPoint).Normalize();
         XYZ offsetVec = (FirstOffsetVec * firstFace.Offset) + (SecondOffsetVec * secondFace.Offset);
         Transform offsetTrf = Transform.CreateTranslation(offsetVec);
         return firstCurve.CreateTransformed(offsetTrf.Multiply(firstFace.Transform));
      }
      /// <summary>
      /// function that iterates through a geometry element to get all the faces it is composed of
      /// </summary>
      /// <param name="geometryElement">
      /// element that needs to be parsed to fetch all the faces
      /// </param>
      /// /// <param name="trf">
      /// transform of the geometry element provided.
      /// this is applicable for geometries that come from familyInstance 
      /// </param>
      /// <returns> list of faces that make up the provided element </returns>
      private List<TargetFace> getFacesFromElement(GeometryElement geometryElement, Transform trf = null)
      {
         List<TargetFace> result = new List<TargetFace>();
         if (geometryElement != null)
         {
            foreach (GeometryObject geometryObject in geometryElement)
            {
               Solid solid = geometryObject as Solid;
               if(solid == null)
               {
                  GeometryInstance geometryInstance = geometryObject as GeometryInstance;
                  if (geometryInstance != null)
                  {
                     Transform transform = geometryInstance.Transform;
                     List<TargetFace> nestedFaces = getFacesFromElement(geometryInstance.SymbolGeometry, transform);
                     if (nestedFaces == null)
                        return null;
                     foreach (TargetFace nestedFace in nestedFaces)
                        result.Add(nestedFace);
                  }
               }
               else
                  foreach (Face face in solid.Faces)
                     result.Add(new TargetFace() { Face = face, Transform = (trf == null )? Transform.Identity : trf });
            }
         }
         return result.Count > 0 ? result : null;
      }
      #endregion
   }
}
