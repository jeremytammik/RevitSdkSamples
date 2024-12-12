using AstSTEELAUTOMATIONLib;
using DSCODBCCOMLib;
using System;
using System.Runtime.InteropServices;

namespace SampleClipAngle
{
   [ComVisible(true)]
   [Guid("8B4FDDC8-946A-49B4-AF65-AC54C5615AB1")]
   public class SampleClipAngle : IRule
   {
      private Joint m_Joint = null;

      public String m_sProfType;
      public String m_sProfSize;
      public String m_sBoltsStandard;
      public String m_sBoltsMaterial;
      public String m_sBoltsSet;
      public double m_dBoltsDiameter;

      private SampleClipUI m_GUI = null;

      private void SplitBeamsInternalName(string fullInternal, out string classInternal, out string sectionInternal)
      {
         const string separator = "#@§@#";
         classInternal = string.Empty;
         sectionInternal = string.Empty;
         int position = fullInternal.IndexOf(separator);
         if (position > 0)
         {
            classInternal = fullInternal.Substring(0, position);
            sectionInternal = fullInternal.Substring(position + separator.Length);
         }
      }

      public void Query(AstUI pAstUI)
      {
         IClassFilter classFilter;
         classFilter = pAstUI.GetClassFilter();
         classFilter.AppendAcceptedClass(eClassType.kBeamStraightClass);

         //Declare the input objects
         AstObjectsArr inputObjectsArr = m_Joint.CreateObjectsArray();


         //Get the column            
         eUIErrorCodes errCode;
         IAstObject selectedColumn = pAstUI.AcquireSingleObject(163, out errCode);


         //selection incorrect
         if (errCode == eUIErrorCodes.kUIError)
            return;

         //user abort the selection
         if (errCode == eUIErrorCodes.kUICancel)
            return;

         //add selected object to the input objects array
         if (selectedColumn != null)
            inputObjectsArr.Add(selectedColumn);

         //Get the beam
         IAstObject selectedBeam = pAstUI.AcquireSingleObject(163, out errCode);


         //selection incorrect
         if (errCode == eUIErrorCodes.kUIError)
            return;

         //user abort the selection
         if (errCode == eUIErrorCodes.kUICancel)
            return;

         //add selected object to the input objects array
         if (selectedBeam != null)
            inputObjectsArr.Add(selectedBeam);


         //add all the objects selected by the user(input objects)
         m_Joint.InputObjects = inputObjectsArr;

         IOdbcUtils tableUtils = new DSCODBCCOMLib.OdbcUtils();
         string defAngle = tableUtils.GetDefaultString(0, "HyperSectionW");
         SplitBeamsInternalName(defAngle, out m_sProfType, out m_sProfSize);

         m_sBoltsStandard = tableUtils.GetDefaultString(401, "Norm");
         m_sBoltsMaterial = tableUtils.GetDefaultString(401, "Material");
         m_sBoltsSet = tableUtils.GetDefaultString(401, "Garnitur");
         m_dBoltsDiameter = tableUtils.GetDefaultDouble(401, "Diameter");
      }

      public void InField(IFiler pFiler)
      {
         int version = pFiler.readVersion(); //Returns the current rule version.

         m_sProfType = (string)pFiler.readItem("ProfType");
         m_sProfSize = (string)pFiler.readItem("ProfSize");

         m_dBoltsDiameter = (double)pFiler.readItem("BoltsDiameter");
         m_sBoltsStandard = (string)pFiler.readItem("BoltsStandard");
         m_sBoltsMaterial = (string)pFiler.readItem("BoltsMaterial");
         m_sBoltsSet = (string)pFiler.readItem("BoltsSet");
      }

      public void OutField(IFiler pFiler)
      {
         pFiler.writeVersion(1); //Set the current rule version

         pFiler.writeItem(m_sProfType, "ProfType");
         pFiler.writeItem(m_sProfSize, "ProfSize");

         pFiler.writeItem(m_dBoltsDiameter, "BoltsDiameter");
         pFiler.writeItem(m_sBoltsStandard, "BoltsStandard");
         pFiler.writeItem(m_sBoltsMaterial, "BoltsMaterial");
         pFiler.writeItem(m_sBoltsSet, "BoltsSet");
      }

      public string GetTableName()
      {
         return "";
      }

      //Returns the beam cs at the given point
      private void GetBeamCSAtPoint(IBeam beam, IPoint3d point, out ICS3d cs)
      {
         //Get the beam cs at start
         ICS3d startCS = beam.getSysCSAt(eBeamEnd.kBeamStart);

         IMatrix3d alignMatrix;
         //Get the closest point on the system line to the input point
         IPoint3d closestPoint = beam.getClosestPointToSystemline(point, false);
         cs = beam.getCSAtPoint(closestPoint);

         //Get the transform matrix for the cs at the input point
         alignMatrix = startCS.SetToAlignCS(cs);

         //Change the closest point to the cs origin
         closestPoint.setFrom(cs.Origin);

         //Get the offset of the beam system 
         IVector3d spOffsetVector = beam.GetCurrentSys2Phys3dOffset();

         //Transform and translate the cs
         spOffsetVector.TransformBy(alignMatrix);
         closestPoint.Add(spOffsetVector);
         cs.Origin = closestPoint;
      }

      IPoint3dArray IntersectBeam(IStraightBeam beam, IPoint3d linePt, IVector3d lineVect)
      {
         //Create a line from the input point and vector
         ILine3d line = (ILine3d)(new DSCGEOMCOMLib.Line3d());
         line.CreateFromVectorAndPoint(linePt, lineVect);

         //Get the body of the column
         IAstModeler bodyColumn = beam.getAstModeler(eBodyContext.kBodyUnNotched);

         //Interect it with the line
         IPoint3dArray result = bodyColumn.intersectWithLine(line);

         //Return the resulted intersection points
         return result;
      }

      IPlane GetFacePlane(IStraightBeam inputColumn, IStraightBeam inputBeam)
      {
         //Result plane
         IPlane plane = (IPlane)(new DSCGEOMCOMLib.plane());

         //Get the beam outer cs
         eBeamEnd beamStart;
         ICS3d outerCS;
         inputBeam.getCutOuterCS(true, inputColumn, -1, out outerCS, out beamStart, true);

         //Change the z vector direction
         IVector3d outerVectZ = outerCS.ZAxis;
         outerVectZ.Multiply(-1);

         IPoint3d outerCSOrigin = outerCS.Origin;

         //Get the column cs at the outer cs origin
         ICS3d mainCSAtPoint;
         GetBeamCSAtPoint(inputColumn, outerCSOrigin, out mainCSAtPoint);

         //Compute tranformation matrix
         IPoint3d axisPoint = mainCSAtPoint.Origin;
         ICS3d midCS = inputColumn.PhysicalCSMid;
         IMatrix3d alignMatrix = mainCSAtPoint.SetToAlignCS(midCS);

         //Transform the points which will constitute the line which we want to intersect with the beam
         axisPoint.TransformBy(alignMatrix);
         outerVectZ.TransformBy(alignMatrix);


         //Get the intersection points
         IPoint3dArray intersectionPts = IntersectBeam(inputColumn, axisPoint, outerVectZ);

         //If we have intersection points, create a plane at the first intersection point
         if (intersectionPts.Count != 0)
         {
            IPoint3d tempPt = intersectionPts[0];
            axisPoint.setFrom(tempPt);


            alignMatrix = midCS.SetToAlignCS(mainCSAtPoint);
            axisPoint.TransformBy(alignMatrix);
            outerVectZ.TransformBy(alignMatrix);

            plane.CreateFromPointAndNormal(axisPoint, outerVectZ);
         }
         return plane;
      }


      private void MovePoint(IPoint3d ptIn, IVector3d vMove, double moveDist, out IPoint3d retPoint)
      {
         retPoint = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
         retPoint.setFrom(ptIn);

         IVector3d v = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
         v.setFrom(vMove);
         v.Normalize();
         v.Multiply(moveDist);
         retPoint.Add(v);
      }

      public void CreateBoltPattern(string boltRole,
          double dBoltWidth,
          double dBoltHeight,
          int nXBolts,
          int nYBolts,
          ICS3d csBolts,
          IStraightBeam cleat1,
          IStraightBeam cleat2,
          IStraightBeam beam,
          ref AstObjectsArr createdObjectsArr)
      {
         Role role = m_Joint.CreateRole(boltRole); //role object

         IBolt bolt = m_Joint.CreateBoltFinitRect(role, m_sBoltsMaterial, m_sBoltsStandard, 0, 0, dBoltHeight, dBoltWidth, nXBolts, nYBolts, m_dBoltsDiameter, csBolts);
         if (bolt != null)
         {
            AstObjectsArr conObj = m_Joint.CreateObjectsArray();
            conObj.Add(cleat1);

            if (cleat2 != null)
               conObj.Add(cleat2);

            conObj.Add(beam);

            bolt.Connect(conObj, eAssembleLocation.kOnSite);
            createdObjectsArr.Add(bolt);
         }
      }

      public void CreateObjects()
      {
         bool bCreationStatus = true;
         //declare the created objects
         AstObjectsArr createdObjectsArr = m_Joint.CreateObjectsArray();

         try
         {
            //retrieve the beam & the column from the input objects array
            AstObjectsArr arrObjects = m_Joint.InputObjects;
            IStraightBeam inputColumn = null;
            IStraightBeam inputBeam = null;

            if (arrObjects != null)
            {
               int nObjs = arrObjects.Count;
               if (nObjs > 1)
               {
                  IAstObject astObj1 = arrObjects[0];
                  if (astObj1 != null)
                  {
                     if (astObj1.Type == eClassType.kBeamStraightClass)
                        inputColumn = (IStraightBeam)astObj1;
                  }

                  IAstObject astObj2 = arrObjects[1];
                  if (astObj2 != null)
                  {
                     if (astObj2.Type == eClassType.kBeamStraightClass)
                        inputBeam = (IStraightBeam)astObj2;
                  }
               }
            }

            if (inputBeam != null && inputColumn != null)
            {
               ICS3d csColumn = inputColumn.cs;
               ICS3d csBeam = inputBeam.cs;

               if (csBeam.ZAxis.IsPerpendicularTo(csColumn.ZAxis))
               {
                  //Get the plane where the beam and shortening intersect
                  IPlane plShortening = GetFacePlane(inputColumn, inputBeam);

                  eBeamEnd beamCutEnd;
                  IPoint3d ptClosestOnColumn = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                  ptClosestOnColumn.setFrom(plShortening.PointOnPlane);

                  //Get the beam cs in the closest point on the column
                  ICS3d csBeamAtIntersectionPoint = inputBeam.getCSAtPoint(ptClosestOnColumn);

                  //Find the cs where at the beam end that needs to be cut
                  ICS3d csCutBeam;
                  inputBeam.getCutOuterCS(true, inputColumn, -1, out csCutBeam, out beamCutEnd, true);

                  //Add a shortening to trim/extend the beam until it touches the column
                  IBeamShortening shortening = inputBeam.addBeamShortening(beamCutEnd, plShortening);
                  if (shortening != null)
                     createdObjectsArr.Add(shortening);

                  //Create an L shaped beam as the first clip angle cleat
                  //We want to put 2 L shaped beams perpendicular to the input beam - on the input beam Z
                  IProfType profType = (IProfType)(new DSCPROFILESACCESSCOMLib.ProfType());
                  profType.createProfType(m_sProfType, m_sProfSize);

                  IVector3d vectLBeamDir = csBeamAtIntersectionPoint.ZAxis;
                  IVector3d vectLBeamTranslationX = csBeamAtIntersectionPoint.XAxis;
                  IVector3d vectLBeamTranslationY = csBeamAtIntersectionPoint.YAxis;

                  //Compute the first cleat it's start/endpoints
                  IPoint3d ptCleatStart = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                  MovePoint(plShortening.PointOnPlane, vectLBeamDir, -100, out ptCleatStart);

                  //Find the beam we thickness and offset the cleat outside it a bit
                  IProfType profBeam = inputBeam.getProfType();
                  double dBeamWeb = profBeam.getGeometricalData(eProfCommonData.kWeb);
                  MovePoint(ptCleatStart, vectLBeamTranslationY, -dBeamWeb / 2, out ptCleatStart);

                  IPoint3d ptCleatEnd = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                  MovePoint(ptCleatStart, vectLBeamDir, 200, out ptCleatEnd);

                  //Create a model role for the first cleat
                  IRole spRole = m_Joint.CreateRole("Angle_Cleat#1");

                  ICS3d csCleat = (ICS3d)(new DSCGEOMCOMLib.CS3d());
                  csCleat.setFrom(csBeamAtIntersectionPoint);
                  csCleat.RotateCSAroundY(Math.PI / 2);
                  csCleat.RotateCSAroundX(Math.PI);

                  //Finnaly, create the beam
                  IStraightBeam firstCleat = m_Joint.CreateStraightBeam(m_sProfType, m_sProfSize, (Role)spRole, ptCleatStart, ptCleatEnd, csCleat);

                  if (firstCleat != null)
                  {
                     firstCleat.refAxis = eProfRefAxis.kLowerLeft;
                     createdObjectsArr.Add(firstCleat);

                     //We will move the cleat cs to create the second cleat, therefore save it, for later when we create the bolts                    
                     ICS3d csBolts = (ICS3d)(new DSCGEOMCOMLib.CS3d());
                     csBolts.setFrom(csCleat);

                     //Perform calculations for the second cleat
                     IPoint3d ptBoltsOrig = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                     ptBoltsOrig.setFrom(ptCleatStart);

                     MovePoint(ptBoltsOrig, vectLBeamDir, 100, out ptBoltsOrig);

                     csBolts.Origin = ptBoltsOrig;

                     MovePoint(ptCleatEnd, vectLBeamTranslationY, dBeamWeb, out ptCleatEnd);
                     MovePoint(ptCleatStart, vectLBeamTranslationY, dBeamWeb, out ptCleatStart);

                     ICS3d csCleat2 = (ICS3d)(new DSCGEOMCOMLib.CS3d());
                     csCleat2.setFrom(csBeamAtIntersectionPoint);
                     csCleat2.RotateCSAroundZ(Math.PI);

                     csCleat2.Origin = ptCleatStart;

                     //Create the cleat
                     IStraightBeam secondCleat = m_Joint.CreateStraightBeam(m_sProfType, m_sProfSize, (Role)spRole, ptCleatEnd, ptCleatStart, csCleat);

                     if (secondCleat != null)
                     {
                        //Now connect the L shaped beams with bolts on the main column and beam
                        secondCleat.refAxis = eProfRefAxis.kLowerLeft;
                        createdObjectsArr.Add(secondCleat);

                        CreateBoltPattern("Bolt#1", 100, 60, 3, 2, csBolts, firstCleat, secondCleat, inputColumn, ref createdObjectsArr);

                        csBolts.RotateCSAroundX(Math.PI / 2);
                        IPoint3d ptCsBoltOrig = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                        MovePoint(csBolts.Origin, vectLBeamTranslationX, -50, out ptCsBoltOrig);
                        MovePoint(ptCsBoltOrig, vectLBeamTranslationY, 40, out ptCsBoltOrig);
                        csBolts.Origin = ptCsBoltOrig;

                        CreateBoltPattern("Bolt#2", 50, 60, 3, 1, csBolts, firstCleat, secondCleat, inputBeam, ref createdObjectsArr);
                     }
                  }
               }
            }
         }
         catch (COMException ex)
         {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            bCreationStatus = false;
         }
         catch (System.Exception ex)
         {
            System.Diagnostics.Debug.WriteLine(ex.Message);
            bCreationStatus = false;
         }

         m_Joint.CreationStatus = bCreationStatus;
         m_Joint.CreatedObjects = createdObjectsArr;
      }

      public void GetUserPages(RulePageArray pagesRet, PropertySheetData pPropSheetData)
      {
         //Set Title(From AstCrtlDb)
         pPropSheetData.SheetPrompt = 88270;

         //First Page bitmap index(From AstorBitmaps)
         pPropSheetData.FirstPageBitmapIndex = 60782;
         pPropSheetData.ResizeOption = eGUIDimension.kStandard;

         //Property Sheet 1
         RulePage rulePage1 = m_Joint.CreateRulePage();
         rulePage1.title = 88438; //Base plate layout
         m_GUI = new SampleClipUI(this);
         rulePage1.hWnd = m_GUI.Handle.ToInt64();
         pagesRet.Add(rulePage1);
      }

      public void FreeUserPages()
      {
         m_GUI.Close();
         m_GUI.Dispose();
      }

      public void GetExportData(IRuleExportFiler pExportFiler)
      {

      }

      public bool GetFeatureName(ref string FeatureName)
      {
         FeatureName = "";
         return false;
      }

      public void InvalidFeature(int reserved)
      {
      }

      public bool ConvertFromHRL(HRLConvertFiler filer, string OldHRLRuleName)
      {
         return false;
      }

      public Joint Joint
      {
         set
         {
            m_Joint = value;
         }
         get
         {
            return m_Joint;
         }
      }
   }
}
