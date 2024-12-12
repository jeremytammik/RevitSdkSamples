//
//////////////////////////////////////////////////////////////////////////////
//
//  Copyright 2015 Autodesk, Inc.  All rights reserved.
//
//  Use of this software is subject to the terms of the Autodesk license 
//  agreement provided at the time of installation or download, or which 
//  otherwise accompanies this software in either electronic or hard copy form.   
//
//////////////////////////////////////////////////////////////////////////////

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Diagnostics;

using AstSTEELAUTOMATIONLib;
using DSCODBCCOMLib;
using System.Windows.Forms;

namespace SteelConnectionsJointExample
{
    [ComVisible(true)]
    [Guid("E8BB9834-E1A2-4644-9C59-1C9812C04E8E")]

    public class CreatePlate : IRule, IJointInfo
    {
        private Joint m_Joint = null;
        private Page1 m_Page1 = null;

        #region Joint_Parameters

        //Sheet 1
        public double m_dPlateThickness;
        public double m_dPlateLength;
        public double m_dPlateWidth;
        public double m_dCutBack;

        public string m_sAnchorMaterial;
        public string m_sAnchorType;
        public string m_sAnchorAssembly;
        public double m_dAnchorDiameter;
        public double m_dAnchorLength;
        public double m_dHoleTolerance;

        #endregion

        #region Standard_Methods

        /// <summary>
        /// Ask user for input with the help of AstUI
        /// and add the necessary entities to InputObjects of the Joint
        /// </summary>
        /// <param name="pAstUI"></param>
        public void Query(AstUI pAstUI)
        {
            //Filter to select only straight beam
            IClassFilter classFilter;
            classFilter = pAstUI.GetClassFilter();
            classFilter.AppendAcceptedClass(eClassType.kBeamStraightClass);

            //Declare the input objects
            AstObjectsArr inputObjectsArr = m_Joint.CreateObjectsArray();

            eUIErrorCodes errCode;
            IAstObject selectedObject = pAstUI.AcquireSingleObject(163, out errCode);

            //selection incorrect
            if (errCode == eUIErrorCodes.kUIError)
                return;

            //user abort the selection
            if (errCode == eUIErrorCodes.kUICancel)
                return;

            //add selected object to the input objects array
            if (selectedObject != null)
                inputObjectsArr.Add(selectedObject);

            //add all the objects selected by the user(input objects)
            m_Joint.InputObjects = inputObjectsArr;

 
            //load default parameters for joint
            loadDefaultValues();
        }

        /// <summary>
        /// Save data to Joint through Filer
        /// </summary>
        /// <param name="pFiler"></param>
        public void OutField(IFiler pFiler)
        {
            pFiler.writeVersion(1); //Set the current rule version

            pFiler.writeItem(m_dPlateThickness, "PlateThickness");
            pFiler.writeItem(m_dPlateLength, "PlateLength");
            pFiler.writeItem(m_dPlateWidth, "PlateWidth");
            pFiler.writeItem(m_dCutBack, "CutBack");

            pFiler.writeItem(m_sAnchorMaterial, "AnchorMaterial");
            pFiler.writeItem(m_sAnchorType, "AnchorType");
            pFiler.writeItem(m_sAnchorAssembly, "AnchorAssembly");
            pFiler.writeItem(m_dAnchorDiameter, "AnchorDiameter");
            pFiler.writeItem(m_dAnchorLength, "AnchorLength");
            pFiler.writeItem(m_dHoleTolerance, "HoleTolerance");
        }

        /// <summary>
        /// Automatically called in the case of invalid feature for the rule (if a feature is used). 
        /// Write your own code for such an event or simply let this function do nothing.
        /// </summary>
        /// <param name="reserved"></param>
        public void InvalidFeature(int reserved)
        {

        }

        /// <summary>
        /// Read data from Joint through Filer
        /// </summary>
        /// <param name="pFiler"></param>
        public void InField(IFiler pFiler)
        {
            //Must be used in order to be able to check rule version
            //if changes are required in future releases
            int version = pFiler.readVersion(); //Returns the current rule version.

            //readItem - Returns the data found with specified name
            m_dPlateThickness = (double)pFiler.readItem("PlateThickness");
            m_dPlateLength = (double)pFiler.readItem("PlateLength");
            m_dPlateWidth = (double)pFiler.readItem("PlateWidth");
            m_dCutBack = (double)pFiler.readItem("CutBack");

            m_sAnchorMaterial = (string)pFiler.readItem("AnchorMaterial");
            m_sAnchorType = (string)pFiler.readItem("AnchorType");
            m_sAnchorAssembly = (string)pFiler.readItem("AnchorAssembly");
            m_dAnchorDiameter = (double)pFiler.readItem("AnchorDiameter");
            m_dAnchorLength = (double)pFiler.readItem("AnchorLength");
            m_dHoleTolerance = (double)pFiler.readItem("HoleTolerance");
        }

        /// <summary>
        /// Get user pages of rule
        /// </summary>
        /// <param name="pagesRet"></param>
        /// <param name="pPropSheetData"></param>
        public void GetUserPages(RulePageArray pagesRet, PropertySheetData pPropSheetData)
        {
            //Set Title(From AstCrtlDb)
            pPropSheetData.SheetPrompt = 81309;

            //First Page bitmap index(From AstorBitmaps)
            pPropSheetData.FirstPageBitmapIndex = 60782;
            pPropSheetData.ResizeOption = eGUIDimension.kStandard;

            //Property Sheet 1
            RulePage rulePage1 = m_Joint.CreateRulePage();
            rulePage1.title = 88438; //Base plate layout
            m_Page1 = new Page1(this);
            rulePage1.hWnd = m_Page1.Handle.ToInt64();
            pagesRet.Add(rulePage1);
        }

        /// <summary>
        /// Should return the name of the tabular parameters table from database
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            return "RULE_CreatePlate";
        }

        /// <summary>
        /// Must return True or False depending on license feature usage for this rule.
        /// The FeatureName parameter must be filled with the license feature name, identical to the feature
        /// name in the license file (if a feature is used for the rule).
        /// </summary>
        /// <param name="FeatureName"></param>
        /// <returns></returns>
        public bool GetFeatureName(ref string FeatureName)
        {
            FeatureName = "";

            return false;
        }

        /// <summary>
        /// Data to be exported
        /// </summary>
        /// <param name="pExportFiler"></param>
        public void GetExportData(IRuleExportFiler pExportFiler)
        {

        }

        /// <summary>
        /// Release user pages
        /// </summary>
        public void FreeUserPages()
        {
            m_Page1.Close();
            m_Page1.Dispose();
        }

        /// <summary>
        /// CreateObjects contains the joint functionality. It uses the global variables declared in the declaration
        /// section and does the main work
        /// </summary>
        public void CreateObjects()
        {
            bool bCreationStatus = true;
            //declare the created objects
            AstObjectsArr createdObjectsArr = m_Joint.CreateObjectsArray();

            try
            {
                //retrieve the beam from the input objects array
                IStraightBeam inputBeam = (IStraightBeam)m_Joint.InputObjects.OfType<IAstObject>().ElementAt(0);

                IPlate basePlate;
                ICS3d csPlate;
                doStuff(inputBeam, ref createdObjectsArr, out basePlate, out csPlate);

                CreateAnchors(basePlate, csPlate, ref createdObjectsArr);

                CreateWelds(inputBeam, basePlate, csPlate, ref createdObjectsArr);

                //////////////////////////////////////////////////////////////////////////
                //Examples
                //CreateAugPolygon(ref createdObjectsArr);
                //CreateStraightBeam(ref createdObjectsArr);
                //CreateUnfoldedBeam(ref createdObjectsArr);
                //CreatePlates(ref createdObjectsArr, true);
                //AddPlateFeatures(basePlate, ref createdObjectsArr);
                //CreateBolts(basePlate, csPlate, ref createdObjectsArr);
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

            //Set it to false will result in a joint "red box" situation. The
            //objects created by the joint until the error occurred will be deleted and the old objects will be available.
            m_Joint.CreationStatus = bCreationStatus;
            m_Joint.CreatedObjects = createdObjectsArr;
        }

        /// <summary>
        /// This method must be written if the new joint was designed to replace and old HRL joint and the
        /// developer intends to convert the old joint to the new one.
        /// The received filer is an incremental filer. Using this filer, all parameters of the old rule are accessible. 
        /// Also, the conversion filer provides the rule version.
        /// The method must return true if the conversion was successfully.
        /// </summary>
        /// <param name="filer"></param>
        /// <param name="OldHRLRuleName"></param>
        /// <returns></returns>
        public bool ConvertFromHRL(HRLConvertFiler filer, string OldHRLRuleName)
        {
            return false;
        }

        /// <summary>
        /// Get or set the joint object of this rule
        /// </summary>
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

        public string ASVersion
        {
            get { return "2024"; }
        }

        public string ApplicationName
        {
            get { return "My First Joint"; }
        }

        public string CompName
        {
            get { return "your company name"; }
        }

        public string Email
        {
            get { return "name@mailProvied.domain"; }
        }

        public int LogoIconId
        {
            get { return 0; }
        }

        public string Name
        {
            get { return "Create Plate"; }
        }

        public string VersionNumber
        {
            get { return "1"; }
        }

        #endregion

        #region Private_methods

        private void loadDefaultValues()
        {
            bool bLoaded = false;
            try
            {
                IOdbcTable odbcTable = (IOdbcTable)(new DSCODBCCOMLib.OdbcTable());
                odbcTable.SetCurrent("RULE_CreatePlate"); // set table name
                odbcTable.AddSearchCriteria(1, "Default");

                //Search if joint table contain a record with "Default" name. If succeeded load values from that record.
                //This is useful when you need to create the joint for many times. 
                //You need to configure joint one time and then save its values to database. Then each time when make a new joint will be created with this values.
                int key = (int)odbcTable.Search();

                if (-1 != key)
                {
                    int idx = 2;

                    //page 1 -- Base plate layout
                    m_dPlateThickness = (double)odbcTable.GetAt(idx++);
                    m_dPlateLength = (double)odbcTable.GetAt(idx++);
                    m_dPlateWidth = (double)odbcTable.GetAt(idx++);
                    m_dCutBack = (double)odbcTable.GetAt(idx++);

                    m_sAnchorMaterial = (string)odbcTable.GetAt(idx++);
                    m_sAnchorType = (string)odbcTable.GetAt(idx++);
                    m_sAnchorAssembly = (string)odbcTable.GetAt(idx++);
                    m_dAnchorDiameter = (double)odbcTable.GetAt(idx++);
                    m_dAnchorLength = (double)odbcTable.GetAt(idx++);
                    m_dHoleTolerance = (double)odbcTable.GetAt(idx++);
                    bLoaded = true;
                }
            }
            catch (Exception ex)
            {
                //Is the RULE_CreatePlate table present?
                //I not - Check Readme.txt & run dbo.RULE_CreatePlate.sql
                MessageBox.Show(ex.Message);
            }
            if ( !bLoaded)
                {
                    //Get default thickness for plate from database(AstorSettings)
                    IOdbcUtils spOdbcUtils = (IOdbcUtils)(new DSCODBCCOMLib.OdbcUtils());

                    m_dPlateThickness = spOdbcUtils.GetDefaultDouble(300, "Thickness");
                    m_dPlateLength = 200;
                    m_dPlateWidth = 200;
                    m_dCutBack = 10;

                    int nKey = spOdbcUtils.GetDefaultInt(407, "AnchorDefaultKey");
                    m_dAnchorLength = spOdbcUtils.GetAnchorDefaults(nKey, out m_sAnchorType, out m_sAnchorAssembly, out m_sAnchorMaterial, out m_dAnchorDiameter);
                    m_dHoleTolerance = spOdbcUtils.GetDefaultDouble(401, "HoleTolerance");
                }
            

        }

        private void getDefaultProfile(int defaultClass, string className, out string sectionClass, out string sectionSize)
        {
            sectionClass = "";
            sectionSize = "";

            IOdbcUtils tableUtils = (IOdbcUtils)(new DSCODBCCOMLib.OdbcUtils());
            string sSectionProf = tableUtils.GetDefaultString(defaultClass, className);

            string separator = "#@" + '§' + "@#";
            string[] section = sSectionProf.Split(new string[] { separator }, StringSplitOptions.None);

            if (section.Length == 2)
            {
                sectionClass = section[0];
                sectionSize = section[1];
            }
        }

        private void setJointTransferForPlate(ref IJointTransfer jointTrans)
        {
            jointTrans.ClassType = eClassType.kPlateClass;

            //set here all the properties which can be modified outside the joint
            jointTrans.set_Attribute(eAttributeCodes.kPlateDenotation, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateMaterial, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateCoating, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateSinglePartNumber, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateMainPartNumber, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateSinglePartPrefix, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateMainPartPrefix, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateAssembly, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateItemNumber, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateSinglePartDetailStyle, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateMainPartDetailStyle, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateNote, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateSPUsedForCollisionCheck, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateSPUsedForNumbering, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateSPDisplayRestriction, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateSPExplicitQuantity, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateMPUsedForCollisionCheck, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateMPUsedForNumbering, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateMPDisplayRestriction, 1);
            jointTrans.set_Attribute(eAttributeCodes.kPlateMPExplicitQuantity, 1);
        }

        private void setJointTransferForAnchor(ref IJointTransfer jointTrans)
        {
            jointTrans.ClassType = eClassType.kAnchorPattern;
           
            //set here all the properties which can be modified outside the joint
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonHoleTolerance, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonCoating, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonDenotation, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonAssembly, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonItemNumber, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonInvertAble, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonNote, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonIgnoreMaxGap, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonSPUsedForCollisionCheck, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonSPUsedForBillOfMaterial, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonSPExplicitQuantity, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonRole, 1);
            jointTrans.set_Attribute(eAttributeCodes.kAnchorOrientation, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonAssembleLocation, 1);

        }

        private void setJointTransferForBolt(ref IJointTransfer jointTrans)
        {
            jointTrans.ClassType = eClassType.kFinitrectScrewBoltPattern;

            //set here all the properties which can be modified outside the joint
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonGripLengthAddition, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonHoleTolerance, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonCoating, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonDenotation, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonAssembly, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonItemNumber, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonInvertAble, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonNote, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonIgnoreMaxGap, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonSPUsedForCollisionCheck, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonSPUsedForNumbering, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonSPUsedForBillOfMaterial, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonSPExplicitQuantity, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBoltPatternCommonRole, 1);
        }

        private void setJointTransferForBeam(ref IJointTransfer jointTrans, eClassType classType)
        {
            jointTrans.ClassType = classType;

            //set here all the properties which can be modified outside the joint
            jointTrans.set_Attribute(eAttributeCodes.kBeamDenotation, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamMaterial, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamCoating, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamSinglePartNumber, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamMainPartNumber, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamSinglePartPrefix, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamMainPartPrefix, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamAssembly, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamItemNumber, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamSinglePartDetailStyle, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamMainPartDetailStyle, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamNote, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamSPUsedForCollisionCheck, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamSPUsedForNumbering, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamSPDisplayRestriction, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamSPExplicitQuantity, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamMPUsedForCollisionCheck, 1);
            jointTrans.set_Attribute(eAttributeCodes.kBeamMPUsedForNumbering, 1);
        }

        private void doStuff(IStraightBeam inputBeam, ref AstObjectsArr createdObjectsArr, out IPlate basePlate, out ICS3d csPlate)
        {
            //get beam cs at start
            ICS3d csBeam = inputBeam.get_PhysicalCSAt(eBeamEnd.kBeamStart);

            IPoint3d origin = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            origin.setFrom(csBeam.Origin);

            IVector3d vDir = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            vDir.setFrom(csBeam.XAxis);
            //vDir.RotateBy(csBeam.YAxis, Math.PI / 6);
            vDir.Normalize();
            vDir.Multiply(-m_dCutBack);
            origin.Add(vDir);

            //Add beam Shortening
            IPlane plShortening = (IPlane)(new DSCGEOMCOMLib.plane());
            plShortening.CreateFromPointAndNormal(origin, vDir);

            IBeamShortening shortening = inputBeam.addBeamShortening(eBeamEnd.kBeamStart, plShortening);
            if (shortening != null)
                createdObjectsArr.Add(shortening);

            //Create a rectangular plate
            //create new role
            IRole plateRole = m_Joint.CreateRole("Baseplate");

            IJointTransfer jointTransPlate = m_Joint.CreateJointTransfer("Plate#1");
            setJointTransferForPlate(ref jointTransPlate);

            csPlate = (ICS3d)(new DSCGEOMCOMLib.CS3d());
            csPlate.Origin = origin;
            csPlate.XAxis = csBeam.ZAxis;
            csPlate.YAxis = csBeam.YAxis;
            csPlate.ZAxis = csBeam.XAxis;

            basePlate = m_Joint.CreatePlateRectangular((AstSTEELAUTOMATIONLib.Role)plateRole, m_dPlateLength, m_dPlateWidth, m_dPlateThickness, csPlate);

            //Add plate to created object array
            if (basePlate != null)
            {
                basePlate.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransPlate;
                basePlate.Portioning = 0;
                createdObjectsArr.Add(basePlate);
            }
        }

        private void CreateAnchors(IPlate basePlate, ICS3d csPlate, ref AstObjectsArr createdObjectsArr)
        {
            //Create anchors
            IRole anchorRole = m_Joint.CreateRole("Anchor_Bolt"); //role object

            IJointTransfer jointTransAnchor = m_Joint.CreateJointTransfer("Anchor#1");
            setJointTransferForAnchor(ref jointTransAnchor);

            IAnchor anchor = m_Joint.CreateAnchorFinitRect((AstSTEELAUTOMATIONLib.Role)anchorRole, m_sAnchorMaterial, m_sAnchorType, 100, 100, 2, 2, m_dAnchorDiameter, csPlate);

            if (anchor != null)
            {
                anchor.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransAnchor;

                anchor.SetHoleTolerance(Math.Abs(m_dHoleTolerance), true);
                anchor.AnchorSet = m_sAnchorAssembly;
                anchor.AnchorLength = m_dAnchorLength;

                AstObjectsArr conObj = m_Joint.CreateObjectsArray();
                conObj.Add(basePlate);

                anchor.Connect(conObj, eAssembleLocation.kOnSite);

                createdObjectsArr.Add(anchor);
            }
        }

        private void CreateWelds(IStraightBeam inputBeam, IPlate basePlate, ICS3d csPlate, ref AstObjectsArr createdObjectsArr)
        {
            IProfType beamProfType = inputBeam.getProfType();
            IMatrix3d matProfTypeToWCS = inputBeam.getMatSysAsXY2Sys();

            ICS3d csAtPoint;
            GetBeamCSAtPoint(inputBeam, csPlate.Origin, out csAtPoint);

            IExtents extents = beamProfType.getGeomExtents();
            IPoint3d min = extents.MinPoint;
            IPoint3d max = extents.MaxPoint;

            min.TransformBy(matProfTypeToWCS);
            min.TransformBy(csAtPoint.Matrix3d);

            max.TransformBy(matProfTypeToWCS);
            max.TransformBy(csAtPoint.Matrix3d);

            IPoint3dArray points = (IPoint3dArray)(new DSCGEOMCOMLib.Point3dArray());
            points.Add(min);
            points.Add(max);

            //create welds
            for (int i = 0; i < (int)points.Count; i++ )
            {        
                //create new role object
                IRole weldRole = m_Joint.CreateRole("Weld#" + (i + 1).ToString());

                //create new joint transfer object
                IJointTransfer jointTrans = m_Joint.CreateJointTransfer("Weld#" + (i + 1).ToString());
                jointTrans.set_Attribute(eAttributeCodes.kWeldPatternThickness, 1);
                jointTrans.set_Attribute(eAttributeCodes.kWeldPatternAssembleLocation, 0);
                jointTrans.set_Attribute(eAttributeCodes.kWeldPatternSeamType, 1);

                IWeld weld = m_Joint.CreateWeld((AstSTEELAUTOMATIONLib.Role)weldRole, eWeldType.kTWeld, 4, points.OfType<IPoint3d>().ElementAt(i), csAtPoint);

                //Add weld to created object array
                if (weld != null)
                {
                    //set joint transfer
                    weld.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTrans;

                    createdObjectsArr.Add(weld);

                    //connect objects
                    AstObjectsArr conObj = m_Joint.CreateObjectsArray();
                    conObj.Add(inputBeam);
                    conObj.Add(basePlate);

                    weld.Connect(conObj, eAssembleLocation.kInShop);
                }
            }
        }

        /// <summary>
        /// Utils
        /// </summary>
        private void GetBeamCSAtPoint(IBeam beam, IPoint3d point, out ICS3d cs)
        {
            ICS3d startCS = beam.getSysCSAt(eBeamEnd.kBeamStart);

            IMatrix3d alignMatrix;

            IPoint3d csPoint = beam.getClosestPointToSystemline(point, false);
            cs = beam.getCSAtPoint(csPoint);

            alignMatrix = startCS.SetToAlignCS(cs);
            csPoint.setFrom(cs.Origin);
            IVector3d spOffsetVector = beam.GetCurrentSys2Phys3dOffset();
            spOffsetVector.TransformBy(alignMatrix);
            csPoint.Add(spOffsetVector);
            cs.Origin = csPoint;
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

        #endregion

        #region Examples

        private void Points()
        {
            //create a point with specified coordinates
            IPoint3d point = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            point.Create(10, 10, 0);

            //move point along Z axis with 100 mm
            IVector3d vector = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            vector.Create(0, 0, 1);

            vector.Multiply(100); //vector has a new length (and new orientation if the value is negative)
            point.Add(vector);

            //print point coordinates
            Debug.WriteLine("x:" + point.x.ToString() + "y:" + point.y.ToString() + "z:" + point.z.ToString());


            IPoint3d p1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p2 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());

            p1.Create(0, 0, 0);
            p2.Create(50, 0, 0);

            double distance = p1.DistanceTo(p2);
            Debug.WriteLine("Distance between points is:" + distance.ToString());

            //return a vector oriented from p2 to p1
            vector = p1.Subtract(p2);
        }

        private void Vectors()
        {
            IVector3d xAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            IVector3d yAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());

            xAxis.Create(1, 0, 0);
            yAxis.Create(0, 1, 0);

            //cross product
            IVector3d zAxis = xAxis.CrossProduct(yAxis);

            //dot product is: 1 if angle = 0, > 0 if angle < 90, 0 if angle = 90, < 0 if angle > 90
            double dotProd = xAxis.DotProduct(yAxis);
            Debug.WriteLine("Scalar product is " + dotProd.ToString());

            //rotate vector by zAxis with 30 degrees
            xAxis.RotateBy(zAxis, Math.PI / 6);

            //Returns the lower angle between vectors. In this case function will return 60 degrees. 
            //Doesn't matter if we call function yAxis.GetAngle(xAxis), function will return same result
            double dAngle = xAxis.GetAngle(yAxis);
            Debug.WriteLine("Angle from x to y is " + ((dAngle * 180) / Math.PI).ToString() + " degrees");


            //If we want angle on other side from xAxis to yAxis we need to use GetAngleWithReference function
            zAxis.Multiply(-1); //Change zAxis direction

            double dAngle2 = xAxis.GetAngleWithReference(yAxis, zAxis);
            Debug.WriteLine("Angle from x to y is " + ((dAngle2 * 180) / Math.PI).ToString() + " degrees");

            //Angle from yAxis to xAxis
            dAngle2 = yAxis.GetAngleWithReference(xAxis, zAxis);
            Debug.WriteLine("Angle from y to x is " + ((dAngle2 * 180) / Math.PI).ToString() + " degrees");

            if (zAxis.IsPerpendicularTo(xAxis))
            {
                Debug.WriteLine("Vectors are perpendicular!");
            }

            zAxis.Multiply(57);
            Debug.WriteLine("Vector length is " + zAxis.Length.ToString());

            zAxis.Normalize(); //Normalize vector
            Debug.WriteLine("Vector length is " + zAxis.Length.ToString());
        }

        private void Lines()
        {
            IPoint3d origin = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            origin.Create(0, 0, 0);

            IVector3d xAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            xAxis.Create(1, 0, 0);

            ILine3d line = (ILine3d)(new DSCGEOMCOMLib.Line3d());
            line.CreateFromVectorAndPoint(origin, xAxis);
        }

        private void Planes()
        {
            IPoint3d origin = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            origin.Create(100, 100, 100);

            IVector3d normal = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            normal.Create(0, 0, 1);

            //create plane from point and normal
            IPlane plane = (IPlane)(new DSCGEOMCOMLib.plane());
            plane.CreateFromPointAndNormal(origin, normal);

            IPoint3d pt = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            pt.Create(20, 30, -45);

            //get perpendicular distance from point to plane
            double distance = plane.get_DistanceTo(pt);
            Debug.WriteLine("Distance from point to plane is " + distance);

            ILine3d line = (ILine3d)(new DSCGEOMCOMLib.Line3d());
            line.CreateFromVectorAndPoint(pt, normal);

            //intersect line with plane
            IPoint3d ptInt;

            if (plane.intersectWithLine(line, out ptInt))
            {
                Debug.WriteLine("Intersection point is (" + ptInt.x + ", " + ptInt.y + ", " + ptInt.z + ")");
            }
        }

        private void CreateAugPolygon(ref AstObjectsArr createdObjectsArr)
        {
            IVertexInfo vertexInfo = (IVertexInfo)(new DSCGEOMCOMLib.vertexInfo());

            IPoint3d center = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            center.Create(40, 40, 0);

            IVector3d zAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            zAxis.Create(0, 0, 1);

            //create vertex info
            double radius = 10;
            vertexInfo.CreateFromCenterAndNormal(radius, center, zAxis);

            IPoint3d p1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p2 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p3 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p4 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p5 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());

            p1.Create(50, 40, 0);
            p2.Create(40, 50, 0);
            p3.Create(-50, 50, 0);
            p4.Create(-50, -50, 0);
            p5.Create(50, -50, 0);

            //create polygon
            IAugPolygon3d polygon = (IAugPolygon3d)(new DSCGEOMCOMLib.AugPolygon3d());
            polygon.AppendNewVertex(p1, vertexInfo, true);
            polygon.AppendVertex(p2);
            polygon.AppendVertex(p3);
            polygon.AppendVertex(p4);
            polygon.AppendVertex(p5);

            //create plate role and joint transfer
            IRole plateRole = m_Joint.CreateRole("Plate");
            IJointTransfer jointTransfer = m_Joint.CreateJointTransfer("Plate");
            setJointTransferForPlate(ref jointTransfer);

            //plate thickness
            double plateThickness = 10;

            //create plate
            IPlate plate = m_Joint.CreatePlatePoly((AstSTEELAUTOMATIONLib.Role)plateRole, polygon, plateThickness);

            //Add plate to created object array
            if (plate != null)
            {
                //set joint transfer
                plate.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransfer;
                plate.Portioning = 0;
                createdObjectsArr.Add(plate);
            }
        }

        private void CreateStraightBeam(ref AstObjectsArr createdObjectsArr)
        {
            //Load defaults from database
            //Defaults are in AstorSettings database

            //Default_STR - string values
            //Default_INT - integer values
            //Default_DBL - double values

            //GetDefaultString(int DefaultClass, string DefaultName)   
            //"C0_HyperSectionW"

            string angleSection, angleSize;
            getDefaultProfile(0, "HyperSectionW", out angleSection, out angleSize);

            //create role object
            IRole beamRole1 = m_Joint.CreateRole("Beam#1");

            //create joint transfer
            IJointTransfer jointTransfer1 = m_Joint.CreateJointTransfer("Beam#1");

            //set joint transfer attributes
            setJointTransferForBeam(ref jointTransfer1, eClassType.kBeamStraightClass);

            IPoint3d startPoint1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d endPoint1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            startPoint1.Create(0, 0, 0);
            endPoint1.Create(0, 0, 500);

            IVector3d zAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            zAxis.Create(0, 1, 0);

            ICS3d inputCS = (ICS3d)(new DSCGEOMCOMLib.CS3d());
            inputCS.XAxis = startPoint1.Subtract(endPoint1);
            inputCS.ZAxis = zAxis;

            //create a straight beam
            //Function will use zAxis from input CS and input points to create beam CS
            //xAxis = vector from input points (startPoint.Subtract(endPoint))
            //yAxis = zAxis.CrossProduct(xAxis) (zAxis from CS)
            //zAxis = xAxis.CrossProduct(yAxis)

            IStraightBeam straightBeam1 = m_Joint.CreateStraightBeam(angleSection, angleSize, (AstSTEELAUTOMATIONLib.Role)beamRole1, startPoint1, endPoint1, inputCS);

            //Add beam to created object array
            if (straightBeam1 != null)
            {
                straightBeam1.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransfer1;
                createdObjectsArr.Add(straightBeam1);
            }

            IPoint3d startPoint2 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d endPoint2 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            startPoint2.Create(400, 0, 0);
            endPoint2.Create(400, 0, 500);

            //create role object
            IRole beamRole2 = m_Joint.CreateRole("Beam#2");

            //create joint transfer
            IJointTransfer jointTransfer2 = m_Joint.CreateJointTransfer("Beam#2");
            setJointTransferForBeam(ref jointTransfer2, eClassType.kBeamStraightClass);

            //rotate CS around X axis with 45 degrees
            inputCS.RotateCSAroundX(Math.PI / 4);

            IStraightBeam straightBeam2 = m_Joint.CreateStraightBeam(angleSection, angleSize, (AstSTEELAUTOMATIONLib.Role)beamRole2, startPoint2, endPoint2, inputCS);

            //Add beam to created object array
            if (straightBeam2 != null)
            {
                straightBeam2.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransfer2;
                createdObjectsArr.Add(straightBeam2);
            }
        }


        private void CreatePolyBeam(ref AstObjectsArr createdObjectsArr)
        {
            IPoint3d p1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p2 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p3 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p4 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p5 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());

            IPoint3d c1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d c2 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());

            p1.Create(0, 0, 0);
            p2.Create(300, 0, 0);
            p3.Create(300, 300, 0);
            p4.Create(300, 600, 0);
            p5.Create(300, 600, -100);

            c1.Create(300, 150, 0);
            c2.Create(300, 450, 0);

            //radius
            double radius = 150;
            IVector3d zAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            zAxis.Create(0, 0, 1);

            //create vertex infos
            IVertexInfo vertexInfo1 = (IVertexInfo)(new DSCGEOMCOMLib.vertexInfo());
            IVertexInfo vertexInfo2 = (IVertexInfo)(new DSCGEOMCOMLib.vertexInfo());

            vertexInfo1.CreateFromCenterAndNormal(radius, c1, zAxis);
            zAxis.Multiply(-1);
            vertexInfo2.CreateFromCenterAndNormal(radius, c2, zAxis);

            //build polyline
            IAugPolyline3d polyline = (IAugPolyline3d)(new DSCGEOMCOMLib.AugPolyline3d());
            polyline.AppendVertex(p1);
            polyline.AppendNewVertex(p2, vertexInfo1, true);
            polyline.AppendNewVertex(p3, vertexInfo2, true);
            polyline.AppendVertex(p4);
            polyline.AppendVertex(p5);

            //create role object
            IRole beamRole = m_Joint.CreateRole("Beam");

            //create joint transfer
            IJointTransfer jointTransfer = m_Joint.CreateJointTransfer("Beam");

            //set joint transfer attributes
            setJointTransferForBeam(ref jointTransfer, eClassType.kBeamPolyClass);

            //get default profile
            string sectionClass, sectionSize;
            getDefaultProfile(0, "HyperSectionC", out sectionClass, out sectionSize);

            IVector3d zVec = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            zVec.Create(0, 0, 1);

            IVector3d vecRefOrientation = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            vecRefOrientation.Create(0, 0, 1);

            IPolyBeam polyBeam = m_Joint.CreatePolyBeam(sectionClass, sectionSize, (AstSTEELAUTOMATIONLib.Role)beamRole, polyline, vecRefOrientation, zVec);

            //Add beam to created object array
            if (polyBeam != null)
            {
                polyBeam.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransfer;
                createdObjectsArr.Add(polyBeam);
            }
        }

        private void CreateUnfoldedBeam(ref AstObjectsArr createdObjectsArr)
        {
            //create vectors
            IVector3d xAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            IVector3d yAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
            IVector3d zAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());

            xAxis.Create(1, 0, 0);
            yAxis.Create(0, 1, 0);
            zAxis.Create(0, 0, 1);

            double diameter = 300, height = 500;

            //start point of beam
            IPoint3d startPoint = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            startPoint.Create(0, 0, 0);

            IPoint3d pt = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            pt.setFrom(startPoint);
            xAxis.Multiply(diameter / 2);
            pt.Add(xAxis);
            xAxis.Normalize();

            IVertexInfo vertexInfo = (IVertexInfo)(new DSCGEOMCOMLib.vertexInfo());
            vertexInfo.CreateFromCenterAndNormal(diameter / 2, startPoint, zAxis);

            //build polyline
            IAugPolyline3d polyline = (IAugPolyline3d)(new DSCGEOMCOMLib.AugPolyline3d());

            polyline.AppendNewVertex(pt, vertexInfo, false);
            polyline.Reinitialize();

            //end point of beam
            IPoint3d endPoint = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            endPoint.setFrom(startPoint);
            zAxis.Multiply(height);
            endPoint.Add(zAxis);
            zAxis.Normalize();

            //polyline CS
            ICS3d polylineCS = (ICS3d)(new DSCGEOMCOMLib.CS3d());
            polylineCS.XAxis = xAxis;
            polylineCS.YAxis = yAxis;
            polylineCS.ZAxis = zAxis;
            polylineCS.Origin = startPoint;

            //create role object
            IRole beamRole = m_Joint.CreateRole("Beam");

            //create joint transfer
            IJointTransfer jointTransfer = m_Joint.CreateJointTransfer("Beam");

            //set joint transfer attributes
            setJointTransferForBeam(ref jointTransfer, eClassType.kBeamUnfoldedClass);

            IUnfoldedBeam unfoldedBeam = m_Joint.CreateUnfoldedBeamWCS(polyline, polylineCS, (AstSTEELAUTOMATIONLib.Role)beamRole, startPoint, endPoint, null);

            //Add beam to created object array
            if (unfoldedBeam != null)
            {
                unfoldedBeam.Thickness = 10; //thickness
                unfoldedBeam.Portioning = 1;
                createdObjectsArr.Add(unfoldedBeam);
            }
        }


        private void CreatePlates(ref AstObjectsArr createdObjectsArr, bool bCreateRectangularPlate)
        {
            if (!bCreateRectangularPlate)
            {
                IPoint3d p1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                IPoint3d p2 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                IPoint3d p3 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                IPoint3d p4 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());

                p1.Create(50, 50, 0);
                p2.Create(-50, 50, 0);
                p3.Create(-50, -50, 0);
                p4.Create(50, -50, 0);

                //create polygon
                IAugPolygon3d polygon = (IAugPolygon3d)(new DSCGEOMCOMLib.AugPolygon3d());
                polygon.AppendVertex(p1);
                polygon.AppendVertex(p2);
                polygon.AppendVertex(p3);
                polygon.AppendVertex(p4);

                //create plate role and joint transfer
                IRole plateRole = m_Joint.CreateRole("Plate");
                IJointTransfer jointTransfer = m_Joint.CreateJointTransfer("Plate");
                setJointTransferForPlate(ref jointTransfer);

                //plate thickness
                double plateThickness = 10;

                //create plate
                IPlate platePoly = m_Joint.CreatePlatePoly((AstSTEELAUTOMATIONLib.Role)plateRole, polygon, plateThickness);

                //Add plate to created object array
                if (platePoly != null)
                {
                    //set joint transfer
                    platePoly.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransfer;
                    platePoly.Portioning = 0;
                    createdObjectsArr.Add(platePoly);
                }
            }
            else
            {
                //create plate role and joint transfer
                IRole plateRole = m_Joint.CreateRole("Plate");
                IJointTransfer jointTransfer = m_Joint.CreateJointTransfer("Plate");
                setJointTransferForPlate(ref jointTransfer);

                IPoint3d origin = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
                origin.Create(0, 0, 0);

                IVector3d xAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
                IVector3d yAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());
                IVector3d zAxis = (IVector3d)(new DSCGEOMCOMLib.Vector3d());

                xAxis.Create(1, 0, 0);
                yAxis.Create(0, 1, 0);
                zAxis.Create(0, 0, 1);

                ICS3d csPlate = (ICS3d)(new DSCGEOMCOMLib.CS3d());
                csPlate.Origin = origin;
                csPlate.XAxis = xAxis;
                csPlate.YAxis = yAxis;
                csPlate.ZAxis = zAxis;

                double dPlateLength = 400, dPlateWidth = 200, dPlateThickness = 10;

                //create rectangular plate
                IPlate rectangularPlate = m_Joint.CreatePlateRectangular((AstSTEELAUTOMATIONLib.Role)plateRole, dPlateLength, dPlateWidth, dPlateThickness, csPlate);

                //Add plate to created object array
                if (rectangularPlate != null)
                {
                    //set joint transfer
                    rectangularPlate.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransfer;
                    rectangularPlate.Portioning = 0;
                    createdObjectsArr.Add(rectangularPlate);
                }
            }
        }

        private void AddBeamFeatures(IStraightBeam inputBeam, ref AstObjectsArr createdObjectsArr)
        {
            IPoint3d p1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p2 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p3 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            IPoint3d p4 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());

            IPoint3d startPoint = inputBeam.PhysicalCSStart.Origin;
            IPoint3d endPoint = inputBeam.PhysicalCSEnd.Origin;

            IVector3d xAxis = startPoint.Subtract(endPoint);

            //retrieve beam geometrical data
            IProfType profType = inputBeam.getProfType();

            double dWidth = profType.getGeometricalData(eProfCommonData.kWidth);   //profile width
            double dHeight = profType.getGeometricalData(eProfCommonData.kProfHeight); //profile height
            double dWeb = profType.getGeometricalData(eProfCommonData.kWeb);       //profile web

            p1.setFrom(startPoint);
            MovePoint(p1, inputBeam.PhysicalCSStart.ZAxis, dHeight / 2, out p1);
            MovePoint(p1, inputBeam.PhysicalCSStart.YAxis, dWeb / 2, out p1);
            MovePoint(p1, inputBeam.PhysicalCSStart.YAxis, (dWidth - dWeb) / 2, out p2);
            MovePoint(p2, xAxis, -100, out p3);
            MovePoint(p1, xAxis, -100, out p4);


            IRole notchRole = m_Joint.CreateRole("Feature");
            notchRole.ClassType = eClassType.kBeamMultiContourNotch;

            //compute contour
            IAugPolygon3d contourNotch = (IAugPolygon3d)(new DSCGEOMCOMLib.AugPolygon3d());
            contourNotch.AppendVertex(p1);
            contourNotch.AppendVertex(p2);
            contourNotch.AppendVertex(p3);
            contourNotch.AppendVertex(p4);

            //add beam notch
            IBeamMultiContourNotch multiContourNotch = inputBeam.addBeamMultiContourNotch((AstSTEELAUTOMATIONLib.Role)notchRole, eBeamEnd.kBeamStart, contourNotch);

            //Add notch to created object array
            if (multiContourNotch != null)
            {
                createdObjectsArr.Add(multiContourNotch);
            }

            //IPoint3d u0 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());
            //IPoint3d u1 = (IPoint3d)(new DSCGEOMCOMLib.Point3d());

            //u0.setFrom(p1);

            //double dFlange = profType.getGeometricalData(eProfCommonData.kFlange);
            //movePoint(u0, inputBeam.PhysicalCSStart.ZAxis, -dFlange - 1e-3, out u1);

            //IBeamMultiContourNotch multiContourNotch = inputBeam.addBeamMultiContourNotchClip((AstSTEELAUTOMATIONLib.Role)notchRole, eBeamEnd.kBeamStart, contourNotch, u0, u1);
            ////Add notch to created object array
            //if (multiContourNotch != null)
            //{
            //    createdObjectsArr.Add(multiContourNotch);
            //}       
        }

        private void AddPlateFeatures(IPlate basePlate, ref AstObjectsArr createdObjectsArr)
        {
            double dWidth = 80, dHeight = 80;

            IVertexFeat chamfer = basePlate.addChamfer(dWidth, dHeight, 0);
            if (chamfer != null)
            {
                createdObjectsArr.Add(chamfer);
            }

            IVertexFeat fillet = basePlate.addFillet(dWidth, eFilletTypes.kFillet_Concav, 1);
            if (fillet != null)
            {
                createdObjectsArr.Add(fillet);
            }

            IVertexFeat fillet2 = basePlate.addFillet(dWidth, eFilletTypes.kFillet_Convex, 2);
            if (fillet2 != null)
            {
                createdObjectsArr.Add(fillet2);
            }
        }


        private void CreateBolts(IPlate basePlate, ICS3d csPlate, ref AstObjectsArr createdObjectsArr)
        {
            //read defaults from database
            IOdbcUtils tableUtils = (IOdbcUtils)(new DSCODBCCOMLib.OdbcUtils());

            string sBoltType = tableUtils.GetDefaultString(401, "Norm");
            string sBoltGrade = tableUtils.GetDefaultString(401, "Material");
            string sBoltAssembly = tableUtils.GetDefaultString(401, "Garnitur");
            double dBoltDiameter = tableUtils.GetDefaultDouble(401, "Diameter");

            //Create bolts
            IRole boltRole = m_Joint.CreateRole("Bolt"); //role object

            //create joint transfer object
            IJointTransfer jointTransfer = m_Joint.CreateJointTransfer("Bolt#1");
            setJointTransferForBolt(ref jointTransfer);

            //create bolt pattern
            IBolt bolt = m_Joint.CreateBoltFinitRect((AstSTEELAUTOMATIONLib.Role)boltRole, sBoltGrade, sBoltType, 0, 0, 100, 100, 2, 2, dBoltDiameter, csPlate);

            //Add bolt to created object array
            if (bolt != null)
            {
                //set joint transfer
                bolt.JointTransfer = (AstSTEELAUTOMATIONLib.JointTransfer)jointTransfer;

                bolt.SetHoleTolerance(2, true);
                bolt.BoltSet = sBoltAssembly;

                //connect objects
                AstObjectsArr conObj = m_Joint.CreateObjectsArray();
                conObj.Add(basePlate);

                bolt.Connect(conObj, eAssembleLocation.kOnSite);
                createdObjectsArr.Add(bolt);
            }
        }

        private void CreateSubJoint(ref AstObjectsArr createdObjectsArr)
        {
            string subJointName = "";
            IRule subRule = m_Joint.CreateSubrule(subJointName);

            IJoint subJoint = subRule.Joint;

            //now set the sub joint input parameters
            AstObjectsArr subJointInputParam = m_Joint.CreateObjectsArray();
            //subJointInputParam.Add(obj1)
            //subJointInputParam.Add(obj2)
            //...
            subJoint.InputObjects = subJointInputParam;

            //will be used to transfer params to the sub joints
            IFiler pFiler = m_Joint.CreateFiler();
            //you could write a function which will "fill" the created IFiler object with all the required parameters for the sub joint 
            //(you have to fill the filer object with proper data because the sub joint will not execute its Query method, so all data has to be initialized by calling the sub rule's InField method).
            //You have to insert data into the filer object in the same way as it is inserted in the sub rule's OutField method.
           
            //setFillerInfo(pFiler);
            subRule.InField(pFiler);

            //call the sub joint creation
            subRule.CreateObjects();

            subJoint.SaveData(subRule);

            //add the sub joint to the created objects of the main joint (the parent joint)
            createdObjectsArr.Add(subJoint);

            //get the sub joint created objects and add them to this joint
            for (int i = 0; i < (int)subJoint.CreatedObjects.Count; i++)
                createdObjectsArr.Add(subJoint.CreatedObjects[i]);
        }
        #endregion
    }
}
