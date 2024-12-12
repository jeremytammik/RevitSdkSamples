#include "pch.h"
#include "ClipAngle.h"
#include <corecrt_math_defines.h>
#include "NativeClipAnglePage.h"




STDMETHODIMP SampleClipAngle::InterfaceSupportsErrorInfo(REFIID riid)
{
   static const IID* arr[] =
   {
      &IID_ISampleClipAngle,
   };

   for (int i = 0; i < sizeof(arr) / sizeof(arr[0]); i++)
   {
      if (IsEqualGUID(*arr[i], riid))
         return S_OK;
   }
   return S_FALSE;
}

STDMETHODIMP SampleClipAngle::get_Joint(IJoint** pVal)
{
   if (m_pJoint == NULL)
      return E_POINTER;

   IJoint* pNewJoint;
   HRESULT hr = m_pJoint->QueryInterface(IID_IJoint, (void**)&pNewJoint);

   if (SUCCEEDED(hr))
      *pVal = pNewJoint;
   return hr;

}

STDMETHODIMP SampleClipAngle::put_Joint(IJoint* pVal)
{
   HRESULT hr = S_OK;

   if (m_pJoint)
   {
      m_pJoint->Release();
      m_pJoint = NULL;
   }

   if (NULL != pVal)
      hr |= pVal->QueryInterface(IID_IJoint, (void**)&m_pJoint);

   return hr;
}

void
SampleClipAngle::SplitBeamsInternalName(BSTR fullInternal, BSTR& classInternal, BSTR& sectionInternal)
{
   CString intName(fullInternal);
   CString intClass = L"";
   CString intSection = L"";

   int position = -1;
   position = intName.Find(CString(L"#@§@#"));
   if (position > 0)
   {
      intClass = intName.Left(position);
      intSection = intName.Right(intName.GetLength() - (position + 5));

      //set the values in the output parameters
      classInternal = intClass.AllocSysString();
      sectionInternal = intSection.AllocSysString();
   }
   else
   {
      //set default values in the output parameters (empty strings)
      classInternal = intClass.AllocSysString();
      sectionInternal = intSection.AllocSysString();
   }
}

STDMETHODIMP SampleClipAngle::Query(IAstUI* pAstUI)
{
   if (!m_pJoint)
      return E_FAIL;

   HRESULT hr = S_OK;

   //create an array to store the input objects
   IAstObjectsArrPtr inputObjectsArr;
   hr |= m_pJoint->CreateObjectsArray(&inputObjectsArr);

   IClassFilterPtr classFilter;
   hr |= pAstUI->GetClassFilter(&classFilter);

   hr |= classFilter->AppendAcceptedClass(kBeamClass);

   eUIErrorCodes errCode;
   IAstObjectPtr column, beam;

   //Get the column            
   hr |= pAstUI->AcquireSingleObject(88270, &errCode, &column);

   if ((kUICancel == errCode) || (kUINoResult == errCode)) //user abort
   {
      return E_ABORT;
   }

   if (SUCCEEDED(hr))
      hr |= inputObjectsArr->Add(column);

   //select beam to connect
   hr |= pAstUI->AcquireSingleObject(88271, &errCode, &beam);
   if ((kUICancel == errCode) || (kUINoResult == errCode)) //user abort
   {
      return E_ABORT;
   }
   //add the beam in the input objects array
   if (SUCCEEDED(hr))
      hr |= inputObjectsArr->Add(beam);


   //add all the objects selected by the user(input objects)
   m_pJoint->put_InputObjects(inputObjectsArr);

   IOdbcUtilsPtr tableUtils;
   hr |= tableUtils.CreateInstance("DSCOdbcCom.OdbcUtils");


   CString angleDefault("HyperSectionW");
   BSTR defAngle;
   hr |= tableUtils->GetDefaultString(0, angleDefault.AllocSysString(), &defAngle);
   SplitBeamsInternalName(defAngle, m_sProfType, m_sProfSize);
   
   
   CString boltsDiameter("Diameter");
   CString boltsStandard("Norm");
   CString boltsSet("Garnitur");
   CString boltsMaterial("Material");

   hr |= tableUtils->GetDefaultDouble(401, boltsDiameter.AllocSysString(), &m_dBoltsDiameter);
   hr |= tableUtils->GetDefaultString(401, boltsStandard.AllocSysString(), &m_sBoltsStandard);
   hr |= tableUtils->GetDefaultString(401, boltsSet.AllocSysString(), &m_sBoltsSet);
   hr |= tableUtils->GetDefaultString(401, boltsMaterial.AllocSysString(), &m_sBoltsMaterial);

   return hr;
}


STDMETHODIMP SampleClipAngle::InField(IFiler* pFiler)
{
   HRESULT hr = S_OK;

   //must be used in order to be able to check rule version
   //if changes are required in future releases
   long nVer;
   hr |= pFiler->readVersion(&nVer);

   VARIANT var;
   //2.2.1 Sheet 1 "Angle cleat"
  
   hr |= pFiler->readItem(CString("ProfType").AllocSysString(), &var);
   m_sProfType = var.bstrVal;
   hr |= pFiler->readItem(CString("ProfSize").AllocSysString(), &var);
   m_sProfSize = var.bstrVal; 

   hr |= pFiler->readItem(CString("BoltsDiameter").AllocSysString(), &var);
   m_dBoltsDiameter = var.dblVal;
   hr |= pFiler->readItem(CString("BoltsStandard").AllocSysString(), &var);
   m_sBoltsStandard = var.bstrVal;
   hr |= pFiler->readItem(CString("BoltsMaterial").AllocSysString(), &var);
   m_sBoltsMaterial = var.bstrVal;
   hr |= pFiler->readItem(CString("BoltsSet").AllocSysString(), &var);
   m_sBoltsSet = var.bstrVal;

   return hr;
}

STDMETHODIMP SampleClipAngle::OutField(IFiler* pFiler)
{
   HRESULT hr = S_OK;

   hr |= pFiler->writeVersion(1);
   //2.2.1 Sheet 1 "Angle cleat"
   hr |= pFiler->writeItem(CComVariant(m_sProfType), CString("ProfType").AllocSysString());
   hr |= pFiler->writeItem(CComVariant(m_sProfSize), CString("ProfSize").AllocSysString());

   hr |= pFiler->writeItem(CComVariant(m_dBoltsDiameter), CString("BoltsDiameter").AllocSysString());
   hr |= pFiler->writeItem(CComVariant(m_sBoltsStandard), CString("BoltsStandard").AllocSysString());
   hr |= pFiler->writeItem(CComVariant(m_sBoltsMaterial), CString("BoltsMaterial").AllocSysString());
   hr |= pFiler->writeItem(CComVariant(m_sBoltsSet), CString("BoltsSet").AllocSysString());

   return hr;
}
STDMETHODIMP SampleClipAngle::GetTableName(BSTR* pStrTableName)
{
   if (pStrTableName == NULL)
      return E_POINTER;

   // table name
   CString tableName("");

   *pStrTableName = tableName.AllocSysString();

   return S_OK;
}

STDMETHODIMP SampleClipAngle::CreateObjects()
{
   
   if (!m_pJoint)
      return E_FAIL;

   HRESULT ruleCreateObjectsMethodStatus = S_OK;
   HRESULT hr = S_OK;

   //get the joint input objects array - previously saved in Query part
   IAstObjectsArrPtr spInObj;
   hr |= m_pJoint->get_InputObjects(&spInObj);

   //create an array that will contain all the AS objects created by the joint
   IAstObjectsArrPtr createdObjects;
   hr |= m_pJoint->CreateObjectsArray(&createdObjects);

   if (SUCCEEDED(hr))
   {
      try
      {
         bool bCreationStatus = true;
         //declare the created objects

            //retrieve the beam & the column from the input objects array
         IBeamPtr inputColumn, inputBeam;
         hr |= spInObj->get_Item(CComVariant(0), (IAstObject**)&inputColumn);
         hr |= spInObj->get_Item(CComVariant(1), (IAstObject**)&inputBeam);

         ICS3dPtr csColumn, csBeam;
         hr |= inputColumn->get_cs(&csColumn);
         hr |= inputBeam->get_cs(&csBeam);

         IVector3dPtr vZColumn, vZBeam;
         hr |= csBeam->get_ZAxis(&vZBeam);
         hr |= csColumn->get_ZAxis(&vZColumn);

         VARIANT_BOOL vectArePerpendicular;
         hr |= vZBeam->IsPerpendicularToTol(vZColumn, 0.01, &vectArePerpendicular);
         if (VARIANT_TRUE == vectArePerpendicular)
         {
            //Get the plane where the beam and shortening intersect
            IPlanePtr plShortening;
            hr |= GetFacePlane(inputColumn, inputBeam, plShortening);

            eBeamEnd beamCutEnd;
            IPoint3dPtr ptClosestOnColumn;
            hr |= plShortening->get_PointOnPlane(&ptClosestOnColumn);

            //Get the beam cs in the closest point on the column
            ICS3dPtr csBeamAtIntersectionPoint;
            hr |= inputBeam->getCSAtPoint(ptClosestOnColumn, &csBeamAtIntersectionPoint);

            //Find the cs where at the beam end that needs to be cut
            ICS3dPtr csCutBeam;
            hr |= inputBeam->getCutOuterCS(VARIANT_TRUE, inputColumn, -1, &csCutBeam, &beamCutEnd, VARIANT_TRUE);

            //Add a shortening to trim/extend the beam until it touches the column
            IBeamShorteningPtr shortening;
            hr |= inputBeam->addBeamShortening(beamCutEnd, plShortening, &shortening);
            if (hr == S_OK)
               hr |= createdObjects->Add(shortening);

            //Create an L shaped beam as the first clip angle cleat
            //We want to put 2 L shaped beams perpendicular to the input beam - on the input beam Z

            IProfTypePtr profType;
            hr |= profType.CreateInstance("DSCProfilesAccessCom.ProfType");
            hr |= profType->createProfType(m_sProfType, m_sProfSize);

            IVector3dPtr vectLBeamDir, vectLBeamTranslationX, vectLBeamTranslationY;
            hr |= csBeamAtIntersectionPoint->get_ZAxis(&vectLBeamDir);
            hr |= csBeamAtIntersectionPoint->get_XAxis(&vectLBeamTranslationX);
            hr |= csBeamAtIntersectionPoint->get_YAxis(&vectLBeamTranslationY);

            //Compute the first cleat it's start/endpoints
            IPoint3dPtr ptCleatStart, ptOnPlane;
            hr |= plShortening->get_PointOnPlane(&ptOnPlane);
            hr |= MovePoint(ptOnPlane, vectLBeamDir, -100, ptCleatStart);

            //Find the beam we thickness and offset the cleat outside it a bit
            IProfTypePtr profBeam;
            hr |= inputBeam->getProfType(&profBeam);
            double dBeamWeb = 0.;
            hr |= profBeam->getGeometricalData(kWeb, &dBeamWeb);
            hr |= MovePoint(ptCleatStart, vectLBeamTranslationY, -dBeamWeb / 2, ptCleatStart);

            IPoint3dPtr ptCleatEnd;
            hr |= MovePoint(ptCleatStart, vectLBeamDir, 200, ptCleatEnd);

            //Create a model role for the first cleat
            IRolePtr spRole;
            hr |= m_pJoint->CreateRole(CString(L"Angle_Cleat#1").AllocSysString(), &spRole);

            ICS3dPtr csCleat;
            hr |= csCleat.CreateInstance("DSCGeomCom.CS3d");
            hr |= csCleat->setFrom(csBeamAtIntersectionPoint);
            hr |= csCleat->RotateCSAroundY(M_PI / 2);
            hr |= csCleat->RotateCSAroundX(M_PI);

            //Finnaly, create the beam
            IStraightBeamPtr firstCleat;
            hr |= m_pJoint->CreateStraightBeam(m_sProfType, m_sProfSize, spRole, ptCleatStart, ptCleatEnd, csCleat, &firstCleat);

            if (hr == S_OK)
            {
               hr |= firstCleat->put_refAxis(kLowerLeft);
               hr |= createdObjects->Add(firstCleat);

               //We will move the cleat cs to create the second cleat, therefore save it, for later when we create the bolts                    
               ICS3dPtr csBolts;
               hr |= csBolts.CreateInstance("DSCGeomCom.CS3d");
               hr |= csBolts->setFrom(csCleat);

               //Perform calculations for the second cleat
               IPoint3dPtr ptBoltsOrig;
               hr |= ptBoltsOrig.CreateInstance("DSCGeomCom.Point3d");
               hr |= ptBoltsOrig->setFrom(ptCleatStart);

               hr |= MovePoint(ptBoltsOrig, vectLBeamDir, 100, ptBoltsOrig);

               hr |= csBolts->put_Origin(ptBoltsOrig);

               hr |= MovePoint(ptCleatEnd, vectLBeamTranslationY, dBeamWeb, ptCleatEnd);
               hr |= MovePoint(ptCleatStart, vectLBeamTranslationY, dBeamWeb, ptCleatStart);

               ICS3dPtr csCleat2;
               hr |= csCleat2.CreateInstance("DSCGeomCom.CS3d");
               hr |= csCleat2->setFrom(csBeamAtIntersectionPoint);
               hr |= csCleat2->RotateCSAroundZ(M_PI);

               hr |= csCleat2->put_Origin(ptCleatStart);

               //Create the cleat
               IStraightBeamPtr secondCleat;
               hr |= m_pJoint->CreateStraightBeam(m_sProfType, m_sProfSize, spRole, ptCleatEnd, ptCleatStart, csCleat, &secondCleat);

               if (hr == S_OK)
               {
                  //Now connect the L shaped beams with bolts on the main column and beam
                  hr |= secondCleat->put_refAxis(kLowerLeft);
                  hr |= createdObjects->Add(secondCleat);

                  hr |= CreateBoltPattern("Bolt#1", 100, 60, 3, 2, csBolts, firstCleat, secondCleat, inputColumn, createdObjects);

                  hr |= csBolts->RotateCSAroundX(M_PI / 2);
                  IPoint3dPtr ptCsBoltOrig, csOrig;
                  hr |= csBolts->get_Origin(&csOrig);
                  hr |= MovePoint(csOrig, vectLBeamTranslationX, -50, ptCsBoltOrig);
                  hr |= MovePoint(ptCsBoltOrig, vectLBeamTranslationY, 40, ptCsBoltOrig);
                  hr |= csBolts->put_Origin(ptCsBoltOrig);

                  hr |= CreateBoltPattern(L"Bolt#2", 50, 60, 3, 1, csBolts, firstCleat, secondCleat, inputBeam, createdObjects);
               }
            }
         }
      }
      catch (_com_error& e)
      {
         hr = e.Error();
         ruleCreateObjectsMethodStatus = hr;
      }
      catch (...)
      {
         hr = E_FAIL;
         ruleCreateObjectsMethodStatus = hr;
      }

   }

   VARIANT_BOOL finishedCorrectly = VARIANT_TRUE;
   if (SUCCEEDED(ruleCreateObjectsMethodStatus))
      finishedCorrectly = VARIANT_TRUE;
   else
      finishedCorrectly = VARIANT_FALSE;

   hr = m_pJoint->put_CreationStatus(finishedCorrectly);
   hr = m_pJoint->put_CreatedObjects(createdObjects);

   return ruleCreateObjectsMethodStatus;

}
STDMETHODIMP SampleClipAngle::GetUserPages(IRulePageArray* pagesRet, IPropertySheetData* pPropSheetData)
{
   AFX_MANAGE_STATE(AfxGetStaticModuleState());
   pPropSheetData->put_SheetPrompt(88270);
   pPropSheetData->put_ResizeOption(kStandard);

   IRulePagePtr page;
   m_pJoint->CreateRulePage(&page);
   page->put_title(88271);
   m_page = new NativeClipAnglePage();
   m_page->Create(IDD_CLIPANGLEDIALOG, NativeClipAnglePage::GetParentWindow());
   

   m_page->setJoint(this);

   page->put_hWnd((DWORD_PTR)m_page->GetSafeHwnd());
   pagesRet->Add(page);
   return S_OK;
}

//unload all pages
STDMETHODIMP SampleClipAngle::FreeUserPages()
{
   AFX_MANAGE_STATE(AfxGetStaticModuleState());

   if (m_page)
   {
      m_page->DestroyWindow();
      delete m_page;
      m_page = NULL;
   }
   return S_OK;
}



STDMETHODIMP SampleClipAngle::GetExportData(IRuleExportFiler* pExportFiler)
{
   return S_OK;
}
STDMETHODIMP SampleClipAngle::GetFeatureName(BSTR* FeatureName, VARIANT_BOOL* hasFeature)
{

   HRESULT hr = S_OK;

   if (FeatureName == NULL || hasFeature == NULL)
      return E_POINTER;

   *hasFeature = VARIANT_FALSE;
   return hr;

}


STDMETHODIMP SampleClipAngle::InvalidFeature(int reserved)
{
   HRESULT hr = S_OK;
   return hr;
}

STDMETHODIMP SampleClipAngle::ConvertFromHRL(IConvertFiler* filer, BSTR OldHRLRuleName, VARIANT_BOOL* Converted)
{
   HRESULT hr = S_OK;

   *Converted = VARIANT_FALSE;

   return hr;
}

//Returns the beam cs at the given point
HRESULT
SampleClipAngle::GetBeamCSAtPoint(IBeamPtr spBeam, IPoint3dPtr spPoint, ICS3dPtr& spCS)
{
   //TO DO  - Add comments
   //Get the beam cs at start
   HRESULT hr = E_FAIL;

   ICS3dPtr spStartCS;
   hr = spBeam->getSysCSAt(kBeamStart, &spStartCS);
   IMatrix3dPtr spAlignMatrix;


   IPoint3dPtr spCSPoint;
   hr = spBeam->getClosestPointToSystemline(spPoint, VARIANT_FALSE, &spCSPoint);
   hr = spBeam->getCSAtPoint(spCSPoint, &spCS);


   hr = spStartCS->SetToAlignCS(spCS, &spAlignMatrix);

   hr = spCS->get_Origin(&spCSPoint);
   IVector3dPtr spOffsetVector;

   hr = spBeam->GetCurrentSys2Phys3dOffset(&spOffsetVector);
   hr = spOffsetVector->TransformBy(spAlignMatrix);
   hr = spCSPoint->Add(spOffsetVector);
   hr = spCS->put_Origin(spCSPoint);

   return hr;
}

HRESULT
SampleClipAngle::IntersectBeam(IStraightBeamPtr beam, IPoint3dPtr linePt, IVector3dPtr lineVect, IPoint3dArrayPtr &ptsInter)
{
   //Create a line from the input point and vector
   ILine3dPtr line;
   HRESULT hr = S_OK;
   hr |= line.CreateInstance("DSCGeomCom.Line3d");
   hr |= line->CreateFromVectorAndPoint(linePt, lineVect);

   //Get the body of the column
   IAstModelerPtr bodyColumn;
   hr |= beam->getAstModeler(kBodyUnNotched, &bodyColumn);

   //Interect it with the line
   hr |= bodyColumn->intersectWithLine(line, &ptsInter);

   //Return the resulted intersection points
   return hr;
}

HRESULT
SampleClipAngle::GetFacePlane(IStraightBeamPtr inputColumn, IStraightBeamPtr inputBeam, IPlanePtr &resPlane)
{
   HRESULT hr = S_OK;
   //Result plane
   resPlane.CreateInstance("DSCGeomCom.Plane");

   //Get the beam outer cs
   eBeamEnd beamStart;
   ICS3dPtr outerCS;
   hr |= inputBeam->getCutOuterCS(VARIANT_TRUE, inputColumn, -1, &outerCS, &beamStart, VARIANT_TRUE);

   //Change the z vector direction
   IVector3dPtr outerVectZ;
   hr |= outerCS->get_ZAxis(&outerVectZ);
   hr |= outerVectZ->Multiply(-1);

   IPoint3dPtr outerCSOrigin;
   hr |= outerCS->get_Origin(&outerCSOrigin);

   //Get the column cs at the outer cs origin
   ICS3dPtr mainCSAtPoint;
   hr |= GetBeamCSAtPoint(inputColumn, outerCSOrigin, mainCSAtPoint);

   //Compute tranformation matrix
   IPoint3dPtr axisPoint;
   hr |= mainCSAtPoint->get_Origin(&axisPoint);
   ICS3dPtr midCS;
   hr |= inputColumn->get_PhysicalCSMid(&midCS);
   IMatrix3dPtr alignMatrix;
   hr |= mainCSAtPoint->SetToAlignCS(midCS, &alignMatrix);

   //Transform the points which will constitute the line which we want to intersect with the beam
   hr |= axisPoint->TransformBy(alignMatrix);
   hr |= outerVectZ->TransformBy(alignMatrix);


   //Get the intersection points
   IPoint3dArrayPtr intersectionPts;
   hr |= IntersectBeam(inputColumn, axisPoint, outerVectZ, intersectionPts);

   //If we have intersection points, create a plane at the first intersection point
   VARIANT count;

   hr |= intersectionPts->get_Count(&count);
   if (count.intVal != 0)
   {
      IPoint3dPtr tempPt;
      hr |= intersectionPts->get_Item(CComVariant(0), &tempPt);
      hr |= axisPoint->setFrom(tempPt);


      hr |= midCS->SetToAlignCS(mainCSAtPoint, &alignMatrix);
      hr |= axisPoint->TransformBy(alignMatrix);
      hr |= outerVectZ->TransformBy(alignMatrix);

      hr |= resPlane->CreateFromPointAndNormal(axisPoint, outerVectZ);
   }
   VariantClear(&count);

   return hr;
}


HRESULT
SampleClipAngle::MovePoint(IPoint3dPtr ptIn, IVector3dPtr vMove, double moveDist, IPoint3dPtr& retPoint)
{
   HRESULT hr = S_OK;
   hr |= retPoint.CreateInstance("DSCGeomCom.Point3d");
   hr |= retPoint->setFrom(ptIn);

   IVector3dPtr vect1;
   hr |= vect1.CreateInstance("DSCGeomCom.Vector3d");

   hr |= vect1->setFrom(vMove);
   hr |= vect1->Normalize();
   hr |= vect1->Multiply(moveDist);
   hr |= retPoint->Add(vect1);

   return hr;
}

HRESULT
SampleClipAngle::CreateBoltPattern(CString boltRole, double dBoltWidth, double dBoltHeight, int nXBolts, int nYBolts, ICS3dPtr csBolts, IBeamPtr cleat1, IBeamPtr cleat2, IBeamPtr beam, IAstObjectsArrPtr& createdObjectsArr)
{
   HRESULT hr = S_OK;
   //CProfileUtils pu;

   IRolePtr role;
   hr |= m_pJoint->CreateRole(boltRole.AllocSysString(), &role); //role object

   IBoltPtr bolt;
   hr |= m_pJoint->CreateBoltFinitRect(role, m_sBoltsMaterial, m_sBoltsStandard, 0, 0, dBoltHeight, dBoltWidth, nXBolts, nYBolts, m_dBoltsDiameter, csBolts, &bolt);

   if (hr == S_OK)
   {
      IAstObjectsArrPtr conObj;
      hr |= m_pJoint->CreateObjectsArray(&conObj);
      hr |= conObj->Add(cleat1);

      hr |= conObj->Add(cleat2);

      hr |= conObj->Add(beam);

      hr |= bolt->Connect(conObj, kOnSite);
      hr |= createdObjectsArr->Add(bolt);
   }

   return hr;
}



STDMETHODIMP SampleClipAngle::BoldParameters(SAFEARRAY** pArrParams)
{
   HRESULT hr = S_OK;

   return hr;
}