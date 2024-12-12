using System;
using Autodesk.AdvanceSteel.CADAccess;
using Autodesk.AdvanceSteel.CADLink.Database;
using Autodesk.AdvanceSteel.ConstructionTypes;
using Autodesk.AdvanceSteel.Geometry;
using Autodesk.AdvanceSteel.Modelling;
using Autodesk.AdvanceSteel.DotNetRoots.DatabaseAccess;
using Autodesk.AdvanceSteel.Profiles;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SampleLapJoint
{
public class LapJoint : IRule
    {
        ObjectId _jointId;

        // property sheet 1 - "Bolt arrangement"
        int _iNoBoltAlong;
        double _dIntermDistAlong;
        double _dEdgeDistAlong1;
        double _dEdgeDistAlong2;
        int _iLayoutAcross; // 0 = center, 1 = from side 1, 2 = from side 2
        int _iNoBoltAcross;
        double _dIntermDistAcross;
        double _dOffsetAcross;
        int _iStaggering; // 0 = none, 1 = side 1, 2 = side 2

        // property sheet 2 - "Bolt parameter"
        double _dBoltDiameter;
        string _sBoltType;
        string _sBoltGrade;
        string _sBoltSet;
        int _iInvert;
        int _iBoltLocation;// 0 = site, 1 = shop

        // members that not written on fillers        
        Matrix3d _CS;
        Vector3d _vForward, _vSide, _vUp;
        Beam.eEnd _Beam2ShorteningEnd;
        Point3d _ptOrigin;


        public ObjectId JointId { get { return _jointId; } set { _jointId = value; } }

        void IRule.CreateObjects()
        {
            //get the input objects
            IJoint jnt = DatabaseManager.Open(JointId) as IJoint;
            IEnumerable<FilerObject> input = jnt.InputObjects;

            //create an array that will contain all the objects created by the joint
            List<CreatedObjectInformation> arrCrOjbInfo = new List<CreatedObjectInformation>();

            try
            {
                if (input.Count() == 2)
                {
                    StraightBeam firstBeam = input.ElementAt(0) as StraightBeam;
                    StraightBeam secBeam = input.ElementAt(1) as StraightBeam;

                    //verify some input condition(profile type and alignment)
                    if (!verifyInputBeams(firstBeam, secBeam))
                        throw new Exception("Input objects");

                    if (null != firstBeam)
                    {
                        // this is basically some data validation and preparation for avoiding many checks or problems later						
                        Point3d ptBeamStart = firstBeam.GetPointAtStart(0);
                        Point3d ptBeamEnd = firstBeam.GetPointAtEnd(0);

                        computeNonFillersData(firstBeam, secBeam);

                        shortenSecondaryBeam(arrCrOjbInfo, secBeam);

                        if (0 == _iStaggering || 1 == _iNoBoltAcross)
                            createBoltsWithoutStager(arrCrOjbInfo, firstBeam, secBeam);
                        else
                            createBoltsWithStager(arrCrOjbInfo, firstBeam, secBeam);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Assert(false, ex.Message);
            }

            //
            // created objects need to be set to the joint
            jnt.SetCreatedObjects(arrCrOjbInfo, true);
        }

        void IRule.GetRulePages(IRuleUIBuilder builder)
        {
            // ids are the same as for Overlapping flat joint
            builder.SheetPromptId = 83088;
            builder.FirstPageBitmapIdx = 60728;

            // property sheet 1 - "Bolt arrangement"
            int nId = builder.BuildRulePage(90559, -1, 60729);

            
            builder.AddTextBox(nId, "Number of along bolts", "_iNoBoltAlong", typeof(int));
            builder.AddTextBox(nId, "Intermediate along distance", "_dIntermDistAlong",  typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
            builder.AddTextBox(nId, "Edge distance member 1", "_dEdgeDistAlong1",  typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
            builder.AddTextBox(nId, "Edge distance member 2", "_dEdgeDistAlong2",  typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
            builder.AddSelectorCustom(nId, "Layout Across", "_iLayoutAcross",  typeof(int), new string[] { "center", "from side 1", "from side 2" });
            builder.AddTextBox(nId, "Number of across bolts", "_iNoBoltAcross", typeof(int));
            builder.AddTextBox(nId, "Intermdediate across distance", "_dIntermDistAcross",  typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
            builder.AddTextBox(nId, "Offset","_dOffsetAcross", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
            builder.AddSelectorCustom(nId, "Staggering", "_iStaggering",  typeof(int), new string[] { "none", "side 1", "side 2" });

            
            // property sheet 2 - "Bolt parameter"
            nId = builder.BuildRulePage(90041, -1, 60730);
            builder.AddTextBox(nId, "Diameter", "_dBoltDiameter", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
            builder.AddBoltSelector(nId, "Bolt properties label", "_sBoltType", "_sBoltGrade", "_dBoltDiameter", "_sBoltSet");
            builder.AddCheckBox(nId, "Invert", "_iInvert");
            builder.AddSelectorCustom(nId, "Bolt Location", "_iBoltLocation", typeof(int), new string[] { "site", "shop" });

        }

        string IRule.GetTableName()
        {
            return "";
        }

        void IRule.Save(IFiler filer)
        {

            filer.WriteItem(_iNoBoltAlong, "_iNoBoltAlong");
            filer.WriteItem(_dIntermDistAlong, "_dIntermDistAlong");
            filer.WriteItem(_dEdgeDistAlong1, "_dEdgeDistAlong1");
            filer.WriteItem(_dEdgeDistAlong2, "_dEdgeDistAlong2");
            filer.WriteItem(_iLayoutAcross, "_iLayoutAcross");
            filer.WriteItem(_iNoBoltAcross, "_iNoBoltAcross");
            filer.WriteItem(_dIntermDistAcross, "_dIntermDistAcross");
            filer.WriteItem(_dOffsetAcross, "_dOffsetAcross");
            filer.WriteItem(_iStaggering, "_iStaggering");

            filer.WriteItem(_dBoltDiameter,"_dBoltDiameter");
            filer.WriteItem(_sBoltType,"_sBoltType");
            filer.WriteItem(_sBoltGrade,"_sBoltGrade");
            filer.WriteItem(_sBoltSet,"_sBoltSet");
            filer.WriteItem(_iInvert,"_iInvert");
            filer.WriteItem(_iBoltLocation,"_iBoltLocation");
        }

        void IRule.Load(IFiler filer)
        {
            int version = filer.ReadVersion();

            _iNoBoltAlong = (int)filer.ReadItem("_iNoBoltAlong");
            _dIntermDistAlong = (double)filer.ReadItem("_dIntermDistAlong");
            _dEdgeDistAlong1 = (double)filer.ReadItem("_dEdgeDistAlong1");
            _dEdgeDistAlong2 = (double)filer.ReadItem("_dEdgeDistAlong2");
            _iLayoutAcross = (int)filer.ReadItem("_iLayoutAcross");
            _iNoBoltAcross = (int)filer.ReadItem("_iNoBoltAcross");
            _dIntermDistAcross = (double)filer.ReadItem("_dIntermDistAcross");
            _dOffsetAcross = (double)filer.ReadItem("_dOffsetAcross");
            _iStaggering = (int)filer.ReadItem("_iStaggering");


            _dBoltDiameter = (double)filer.ReadItem("_dBoltDiameter");
            _sBoltType = (string)filer.ReadItem("_sBoltType");
            _sBoltGrade = (string)filer.ReadItem("_sBoltGrade");
            _sBoltSet = (string)filer.ReadItem("_sBoltSet");
            _iInvert = (int)filer.ReadItem("_iInvert");
            _iBoltLocation = (int)filer.ReadItem("_iBoltLocation");
        }

        void IRule.Query(IUI ui)
        { 
            eUIErrorCodes errCode;

            ui.ClassFilter.AppendAcceptedBaseClass(FilerObject.eObjectType.kBeam);

            FilerObject fObj1 = ui.SelectObject(666, false, out errCode);

            FilerObject fObj2 = ui.SelectObject(666, false, out errCode);

            if (null != fObj1 && null != fObj2)
            {
                IJoint jnt = DatabaseManager.Open(JointId) as IJoint;

                if (null != jnt)
                {
                    jnt.SetInputObjects(new FilerObject[] { fObj1, fObj2 }, new int[] { -1, -1 });
                }
            }

            LoadDefaultValues();
        }

        private void LoadDefaultValues()
        {
            // property sheet 1 - "Bolt arrangement"
            _iNoBoltAlong = 3;
            _dIntermDistAlong = 75;
            _dEdgeDistAlong1 = 50;
            _dEdgeDistAlong2 = 50;
            _iLayoutAcross = 0; // 0 = center, 1 = from side 1, 2 = from side 2
            _iNoBoltAcross = 3;
            _dIntermDistAcross = 75;
            _dOffsetAcross = 0;
            _iStaggering = 0; // 0 = none, 1 = side 1, 2 = side 2


            // property sheet 2 - "Bolt parameter"
            string boltStandard = "Norm";      // bolt type
            string boltSet = "Garnitur";        //bolt grade
            string boltMaterial = "Material";   //assembly
            string boltsDiameter = "Diameter";
            _sBoltType = AstorSettings.Instance.GetDefaultString(401, boltStandard);
            _sBoltSet = AstorSettings.Instance.GetDefaultString(401, boltSet);
            _sBoltGrade = AstorSettings.Instance.GetDefaultString(401, boltMaterial);
            _dBoltDiameter = AstorSettings.Instance.GetDefaultDouble(401, boltsDiameter); ;
            _iInvert = 0;
            _iBoltLocation = 0; // 0 = site, 1 = shop
        }

        private bool verifyInputBeams(Beam beam1, Beam beam2)
        {
            if (null == beam1 || null == beam2)
                return false;

            //////////////////////////////////////////////////////////////////////////
            // verify if the input beams are both "F" profiles
            ProfileType profType;
            string strClsName1, strRTLName1,
                   strClsName2, strRTLName2;

            profType = beam1.GetProfType();
            profType.GetSectionClass(out strClsName1, out strRTLName1);

            profType = beam2.GetProfType();
            profType.GetSectionClass(out strClsName2, out strRTLName2);

            if (("F" != strClsName1) || ("F" != strClsName2))
                return false;

            //////////////////////////////////////////////////////////////////////////
            // verify if the input beams are parallel

            Matrix3d beam1MidCS, beam2MidCS;
            Vector3d xAxis1, xAxis2, yAxis1, yAxis2, zAxis1, zAxis2;
            Point3d origPct1, origPct2;

            beam1MidCS = beam1.PhysCSMid;
            beam1MidCS.GetCoordSystem(out origPct1, out xAxis1, out yAxis1, out zAxis1);

            beam2MidCS = beam2.PhysCSMid;
            beam2MidCS.GetCoordSystem(out origPct2, out xAxis2, out yAxis2, out zAxis2);

            Tol eTol = new Tol(1.0e-5, 1.0e-5);

            bool bXVectAreParallel = xAxis1.IsParallelTo(xAxis2, eTol);
            bool bYVectAreParallel = yAxis1.IsParallelTo(yAxis2, eTol);


            if (false == bXVectAreParallel || false == bYVectAreParallel)
                return false;

            return true;
        }


        private bool computeNonFillersData(Beam beam1, Beam beam2)
        {
            bool bResult = true;

            //////////////////////////////////////////////////////////////////////////
            // create instances
            _CS = new Matrix3d();
            _vForward = new Vector3d();
            _vSide = new Vector3d();
            _vUp = new Vector3d();
            _Beam2ShorteningEnd = Beam.eEnd.kStart;
            _ptOrigin = new Point3d();

            //////////////////////////////////////////////////////////////////////////
            // get furthest end points for the two beams

            // get data from input beams
            Point3d ptStart1, ptEnd1, ptStart2, ptEnd2;
            Vector3d xAxis, yAxis, zAxis;

            Matrix3d csTemp;
            csTemp = beam1.PhysCSStart;
            csTemp.GetCoordSystem(out ptStart1, out xAxis, out yAxis, out zAxis);

            csTemp = beam1.PhysCSEnd;
            csTemp.GetCoordSystem(out ptEnd1, out xAxis, out yAxis, out zAxis);

            csTemp = beam2.PhysCSStart;
            csTemp.GetCoordSystem(out ptStart2, out xAxis, out yAxis, out zAxis);

            csTemp = beam2.PhysCSEnd;
            csTemp.GetCoordSystem(out ptEnd2, out xAxis, out yAxis, out zAxis);


            Vector3d vPt1Pt2 = null;

            double dDist = 0.0;
            double dDistMax = 10.0;

            dDistMax = ptStart2.DistanceTo(ptStart1);
            vPt1Pt2 = ptStart2.Subtract(ptStart1);

            dDist = ptStart2.DistanceTo(ptEnd1);
            if (dDist > dDistMax)
            {
                vPt1Pt2 = ptStart2.Subtract(ptEnd1);
                dDistMax = dDist;
            }

            dDist = ptEnd2.DistanceTo(ptStart1);
            if (dDist > dDistMax)
            {
                vPt1Pt2 = ptEnd2.Subtract(ptStart1);
                dDistMax = dDist;
            }

            dDist = ptEnd2.DistanceTo(ptEnd1);
            if (dDist > dDistMax)
                vPt1Pt2 = ptEnd2.Subtract(ptEnd1);

            //////////////////////////////////////////////////////////////////////////
            // set _ptOrigin as the last point of main beam on vPt1Pt2 direction
            List<Point3d> ptsList = new List<Point3d>();
            ptsList.Add(ptStart1);
            ptsList.Add(ptEnd1);
            ptsList.Sort(new Point3dSorter(vPt1Pt2));
            _ptOrigin = ptsList[0];

            //////////////////////////////////////////////////////////////////////////
            // set _Beam2ShorteningEnd as the end from first point of sec beam on vPt1Pt2 direction

            ptsList.Clear();
            ptsList.Add(ptStart2);
            ptsList.Add(ptEnd2);
            ptsList.Sort(new Point3dSorter(vPt1Pt2));              

            _Beam2ShorteningEnd = pointsAreEqual(ptsList[1], ptStart2, 1.0e-5) ? Beam.eEnd.kStart : Beam.eEnd.kEnd;
            //////////////////////////////////////////////////////////////////////////
            // set vectors
            // _vForward is along x axis of main beam, pointing from pt1 towards pt2, where pt1 and pt2 are furthest end points of the two beams
            // _vSide is z axis of main beam
            // _vUp is along y axis of main beam, computed as cross product between _vForward and _vSide
            Matrix3d csMainBeam;
            Vector3d vTemp;
            Point3d ptOrig;
            csMainBeam = beam1.PhysCSMid;
            csMainBeam.GetCoordSystem(out ptOrig, out vTemp, out yAxis, out zAxis);
            _vForward.Set(vTemp.x, vTemp.y, vTemp.z);
            if (null != vPt1Pt2)
            {
                if(vectorsHaveApproxSameDirection(_vForward,vPt1Pt2))
                    _vForward = _vForward.Negate();
            }

            _vSide.Set(zAxis.x, zAxis.y, zAxis.z);

            _vUp = _vForward.CrossProduct(_vSide);

            //////////////////////////////////////////////////////////////////////////
            // set cs
            _CS.SetCoordSystem(_ptOrigin, _vForward, _vSide, _vUp);

            return bResult;
        }

        private void shortenSecondaryBeam(List<CreatedObjectInformation> spCreatedObjects, StraightBeam strBeam)
        {
            //create shortening and add it to the beam
            BeamShortening beamShort = new BeamShortening();

            Plane planeShort = new Plane(_ptOrigin, _vForward);

            Vector3d tmpVect = _vForward;
            tmpVect *= _dEdgeDistAlong1 + _dEdgeDistAlong2 + (_iNoBoltAlong - 1) * _dIntermDistAlong;

            planeShort.TransformBy(Matrix3d.GetTranslation(tmpVect));
            if (0 != _iStaggering && _iNoBoltAcross > 1)
            {
                Vector3d vect = planeShort.Normal.Normalize();
                vect *= _dIntermDistAlong / 2;
                planeShort.TransformBy(Matrix3d.GetTranslation(vect));
            }

            strBeam.AddFeature(beamShort);

            beamShort.Set(planeShort.Origin, planeShort.Normal, _Beam2ShorteningEnd);

            CreatedObjectInformation coi = new CreatedObjectInformation();
            coi.CreatedObject = beamShort;
            coi.JointTransferId = "ShortenRole";
            coi.Attributes = new eObjectsAttribute[0];

            //add the new created object to the joint 
            spCreatedObjects.Add(coi);
        }

        private bool pointsAreEqual(Point3d pt1, Point3d pt2, double dTol)
        {
            bool bResult = true;

            double d = Math.Sqrt((pt2.x - pt1.x) * (pt2.x - pt1.x) + (pt2.y - pt1.y) * (pt2.y - pt1.y) + (pt2.z - pt1.z) * (pt2.z - pt1.z));
           
            if (d < dTol)
                bResult = true;
            else
                bResult = false;
            return bResult;
        }

       

        private bool createBoltsWithoutStager(List<CreatedObjectInformation> spCreatedObjects, StraightBeam beam1, StraightBeam beam2)
        {
            bool bRet = true;

            // get the main beam height
            ProfileType beamProfType = beam1.GetProfType();
            double dHeight = beamProfType.GetGeometricalData(ProfileType.eCommonData.kProfHeight);

            Point3d ptTemp;

            movePoint(_ptOrigin, _vForward, _dEdgeDistAlong1 + (_iNoBoltAlong - 1) * _dIntermDistAlong / 2, out ptTemp);

            switch (_iLayoutAcross)
            {
                case 0: // center
                    movePoint(ptTemp, _vSide, _dOffsetAcross, out ptTemp);
                    break;
                case 1: // from side 1
                    movePoint(ptTemp, _vSide, -dHeight / 2 + (_iNoBoltAcross - 1) * _dIntermDistAcross / 2 + _dOffsetAcross,out ptTemp);
                    break;
                case 2: // from side 2
                    movePoint(ptTemp, _vSide, dHeight / 2 - (_iNoBoltAcross - 1) * _dIntermDistAcross / 2 - _dOffsetAcross, out ptTemp);
                    break;
            }

            Point3d ptO;
            Vector3d vX, vY, vZ;
            _CS.GetCoordSystem(out ptO, out vX, out vY, out vZ);

            // set model role
            string strBoltRole = "Role";

            Point3d lowerLeft = ptTemp - (_dIntermDistAlong / 2 * (_iNoBoltAlong - 1)) * vX - (_dIntermDistAcross / 2 * (_iNoBoltAcross - 1)) * vY;
            Point3d upperRight = ptTemp + (_dIntermDistAlong / 2 * (_iNoBoltAlong - 1)) * vX + (_dIntermDistAcross / 2 * (_iNoBoltAcross - 1)) * vY;

            FinitRectScrewBoltPattern boltPattern = new FinitRectScrewBoltPattern(lowerLeft, upperRight, vX, vY);
            boltPattern.Nx = _iNoBoltAlong;
            boltPattern.Ny = _iNoBoltAcross;
            boltPattern.Standard = _sBoltType;
            boltPattern.Grade = _sBoltGrade;
            boltPattern.BoltAssembly = _sBoltSet;
            boltPattern.ScrewDiameter = _dBoltDiameter;

            if (boltPattern != null)
            {
                if (1 == _iInvert)
                    boltPattern.IsInverted = true;

                boltPattern.WriteToDb();
                boltPattern.Connect(new FilerObject[] { beam1, beam2 }, _iBoltLocation == 0 ? AtomicElement.eAssemblyLocation.kOnSite : AtomicElement.eAssemblyLocation.kInShop);

                CreatedObjectInformation coi = new CreatedObjectInformation();
                coi.CreatedObject = boltPattern;
                coi.JointTransferId = strBoltRole;
                coi.Attributes = new eObjectsAttribute[0];

                spCreatedObjects.Add(coi);
            }
            return bRet;
        }
        
        

        private bool createBoltsWithStager(List<CreatedObjectInformation> spCreatedObjects, StraightBeam beam1, StraightBeam beam2)
        {
            bool bRes = true;

            // get the main beam height
            ProfileType beamProfType = beam1.GetProfType();
            double dHeight = beamProfType.GetGeometricalData(ProfileType.eCommonData.kProfHeight);

            Point3d ptTemp1, ptTemp2;

            if (1 == _iStaggering) // side 1 stagger
            {
                movePoint(_ptOrigin, _vForward, _dEdgeDistAlong1 + (_iNoBoltAlong - 1) * _dIntermDistAlong / 2,out  ptTemp1);
                movePoint(_ptOrigin, _vForward, _dEdgeDistAlong1 + _iNoBoltAlong * _dIntermDistAlong / 2,out  ptTemp2);
            }
            else // side 2 stagger
            {
                movePoint(_ptOrigin, _vForward, _dEdgeDistAlong1 + (_iNoBoltAlong - 1) * _dIntermDistAlong / 2,out ptTemp2);
                movePoint(_ptOrigin, _vForward, _dEdgeDistAlong1 + _iNoBoltAlong * _dIntermDistAlong / 2,out ptTemp1);
            }

            // number of across bolts of each group
            int iNoBoltAcross1 = (int)Math.Ceiling((double)_iNoBoltAcross / 2);
            int iNoBoltAcross2 = (int)Math.Floor((double)_iNoBoltAcross / 2);

            switch (_iLayoutAcross)
            {
                case 0: // center
                    if (1 == (_iNoBoltAcross % 2)) // odd number of bolts across
                    {
                        movePoint(ptTemp1, _vSide, _dOffsetAcross,out  ptTemp1);
                        movePoint(ptTemp2, _vSide, _dOffsetAcross,out ptTemp2);
                    }
                    else // even number of bolts
                    {
                        movePoint(ptTemp1, _vSide, _dOffsetAcross - _dIntermDistAcross / 2,out ptTemp1);
                        movePoint(ptTemp2, _vSide, _dOffsetAcross + _dIntermDistAcross / 2,out ptTemp2);
                    }
                    break;
                case 1: // from side 1
                    movePoint(ptTemp1, _vSide, -dHeight / 2 + (iNoBoltAcross1 - 1) * _dIntermDistAcross + _dOffsetAcross,out ptTemp1);
                    movePoint(ptTemp2, _vSide, -dHeight / 2 + (iNoBoltAcross2) * _dIntermDistAcross + _dOffsetAcross,out ptTemp2);
                    break;
                case 2: // from side 2
                    movePoint(ptTemp1, _vSide, dHeight / 2 - (iNoBoltAcross1 - 1) * _dIntermDistAcross - _dOffsetAcross,out ptTemp1);
                    movePoint(ptTemp2, _vSide, dHeight / 2 - (iNoBoltAcross2) * _dIntermDistAcross - _dOffsetAcross,out ptTemp2);
                    break;
            }

            // set model role
            string  strBoltRole;
            strBoltRole = "Bolt#1";

            Point3d ptO;
            Vector3d vX, vY, vZ;
            _CS.GetCoordSystem(out ptO, out vX, out vY, out vZ);


            Point3d lowerLeft = ptTemp1 - (_dIntermDistAlong / 2.0 * (_iNoBoltAlong - 1)) * vX - (_dIntermDistAcross * (iNoBoltAcross1 - 1)) * vY;
            Point3d upperRight = ptTemp1 + (_dIntermDistAlong / 2.0 * (_iNoBoltAlong - 1)) * vX + (_dIntermDistAcross * (iNoBoltAcross1 - 1)) * vY;

            //create and add bolts
            FinitRectScrewBoltPattern boltPattern1 = new FinitRectScrewBoltPattern(lowerLeft, upperRight, vX, vY);
            boltPattern1.Nx = _iNoBoltAlong;
            boltPattern1.Ny = iNoBoltAcross1;
            boltPattern1.Standard = _sBoltType;
            boltPattern1.Grade = _sBoltGrade;
            boltPattern1.BoltAssembly = _sBoltSet;
            boltPattern1.ScrewDiameter = _dBoltDiameter;

            lowerLeft = ptTemp2 - (_dIntermDistAlong / 2.0 * (_iNoBoltAlong - 1)) * vX - (_dIntermDistAcross * (iNoBoltAcross2 - 1)) * vY;
            upperRight = ptTemp2 + (_dIntermDistAlong / 2.0 * (_iNoBoltAlong - 1)) * vX + (_dIntermDistAcross * (iNoBoltAcross2 - 1)) * vY;

            FinitRectScrewBoltPattern boltPattern2 = new FinitRectScrewBoltPattern(lowerLeft, upperRight, vX, vY);
            boltPattern2.Nx = _iNoBoltAlong;
            boltPattern2.Ny = iNoBoltAcross2;
            boltPattern2.Standard = _sBoltType;
            boltPattern2.Grade = _sBoltGrade;
            boltPattern2.BoltAssembly = _sBoltSet;
            boltPattern2.ScrewDiameter = _dBoltDiameter;

            IJoint jnt = DatabaseManager.Open(JointId) as IJoint;

            if (boltPattern1 != null && boltPattern2 != null)
            {
                if (1 == _iInvert)
                {
                    boltPattern1.IsInverted = true;
                    boltPattern2.IsInverted = true;
                }

                boltPattern1.WriteToDb();
                boltPattern1.Connect(new FilerObject[] { beam1, beam2 }, _iBoltLocation == 0 ? AtomicElement.eAssemblyLocation.kOnSite : AtomicElement.eAssemblyLocation.kInShop);

                boltPattern2.WriteToDb();
                boltPattern2.Connect(new FilerObject[] { beam1, beam2 }, _iBoltLocation == 0 ? AtomicElement.eAssemblyLocation.kOnSite : AtomicElement.eAssemblyLocation.kInShop);

                CreatedObjectInformation coi1 = new CreatedObjectInformation();
                coi1.CreatedObject = boltPattern1;
                coi1.JointTransferId = strBoltRole;
                coi1.Attributes = new eObjectsAttribute[0];

                CreatedObjectInformation coi2 = new CreatedObjectInformation();
                coi2.CreatedObject = boltPattern2;
                coi2.JointTransferId = strBoltRole;
                coi2.Attributes = new eObjectsAttribute[0];

                spCreatedObjects.Add(coi1);
                spCreatedObjects.Add(coi2);
            }
            return bRes;
        }
        private int sortPoints(Point3d p1, Point3d p2, Vector3d refVect)
        {
            Vector3d vectDiff = p2.Subtract(p1);
            double dVal = 0.0;
            dVal = vectDiff.DotProduct(refVect);
            if ((-1.0e-5 < dVal) &&
                (dVal < 1.0e-5))
                return 0;
            else if (dVal < 0)
                return -1;
            else
                return 1;
        }

        private void movePoint(Point3d pt, Vector3d vect, double dist, out Point3d resPt)
        {
            //copies ptRes from pt and moves it along vector vect with distance dist

            Vector3d vect1 = new Vector3d(vect);

            vect1.Normalize();
            vect1 *= dist;

            resPt = new Point3d(pt.x + vect1.x, pt.y + vect1.y, pt.z + vect1.z);
        }

        private bool vectorsHaveApproxSameDirection(Vector3d v1, Vector3d v2)
        {
            if (v1.IsPerpendicularTo(v2))
            {
                return false;
            }

            Point3d pt0, pt1, pt2;
            pt0 = new Point3d();
            movePoint(pt0, v1, 100, out pt1);
            List<Point3d> ptsList = new List<Point3d> { pt0, pt1 };
            ptsList.Sort(new Point3dSorter(v2));

            pt2 = ptsList[0];

            double dDist = 0;
            dDist = pt2.DistanceTo(pt1);

            if (Math.Abs(dDist) > 1.0e-5)
                return false;
            else
                return true;
        }
    }

    public class Point3dSorter : IComparer<Point3d>
    {
        private Vector3d _sortDir;
        public Point3dSorter(Vector3d sortDir)
        {
            if (sortDir == null)
                _sortDir = Vector3d.kXAxis;

            _sortDir = sortDir;
        }

        public int Compare(Point3d a, Point3d b)
        {
            Point3d p1 = a as Point3d;
            Point3d p2 = b as Point3d;
            Vector3d vect = p2.Subtract(p1);
            double dVal = 0.0;
            dVal = vect.DotProduct(_sortDir);
            if ((-1.0e-5 < dVal) &&
                (dVal < 1.0e-5))
                return 0;
            else if (dVal < 0)
                return -1;
            else
                return 1;
        }
    }
}