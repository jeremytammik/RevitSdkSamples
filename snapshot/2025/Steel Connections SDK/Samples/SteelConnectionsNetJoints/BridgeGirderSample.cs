using Autodesk.AdvanceSteel.CADAccess;
using Autodesk.AdvanceSteel.CADLink.Database;
using Autodesk.AdvanceSteel.ConstructionTypes;
using Autodesk.AdvanceSteel.Geometry;
using Autodesk.AdvanceSteel.Modelling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SteelConnectionsDotNetJointExample
{
	public class BridgeGirderSample : IRule
	{
		ObjectId _jointId;

		//
		// dummy unit TB test 
		double _dSomeUnitValue = 10;

		//web info
		double _webThickness;
		int _numberOfSegmentPoints;//start, intermed 1, intermed 2, end
		List<double> _webTopHeight;//contains a list of top heights at segmentation points: start, intermediate point 1, intermediate point 2, end
		List<double> _webTopHeightDistToIntermPt;
		List<double> _webBottomHeight;//contains a list of bottom heights at segmentation points: start, intermediate point 1, intermediate point 2, end
		List<double> _webBottomHeightDistToIntermPt;

		//top flange info
		double _flangeTopThickness;
		List<double> _flangeTopLeftWidth;//contains a list of flange top left widths at segmentation points: start, intermediate point 1, intermediate point 2, end
		List<double> _flangeTopRightWidth;//contains a list of flange top right widths at segmentation points: start, intermediate point 1, intermediate point 2, end

		//bottom flange info
		double _flangeBottomThickness;
		List<double> _flangeBottomLeftWidth;//contains a list of flange bottom left widths at segmentation points: start, intermediate point 1, intermediate point 2, end
		List<double> _flangeBottomRightWidth;//contains a list of flange top right widths at segmentation points: start, intermediate point 1, intermediate point 2, end

		//grid definition
		int _numberOfRows;
		double _distanceBetweenRows;
		double _curvature;

		//stiffener info
		double _stiffenerThickness;
		int _numberOfStiffenersPerBeam;
		int _stiffenerIsPerpendicularOnXAxis; // this is int because of serialization - TODO decide if this is ok - framework!

		//bracing info
		string _bracingProfileName;

		//bolts&anchors
		int _createAnchorsInstead;

		//bolts
		string _boltType;
		string _boltMaterial;
		double _boltDiameter;
		string _boltSet;

		//weld
		int _weldKey;

		//anchors
		string _anchorType;
		string _anchorMaterial;
		double _anchorDiameter;
		string _anchorSet;
		double _anchorLength;

		//shear studs
		string _studType;
		string _studMaterial;
		double _studDiameter;
		double _studLength;

		//not on the filers
		Vector3d _xAxis;
		Vector3d _yAxis;
		Vector3d _zAxis;
		int _webPlateIndex;
		int _flangePlateIndex;
		int _stiffenerIndex;
		int _bracingIndex;
		List<List<Point3d>> _ptsStiffenerTopLeftPoints;
		List<List<Point3d>> _ptsStiffenerTopRightPoints;
		List<List<Point3d>> _ptsStiffenerBottomLeftPoints;
		List<List<Point3d>> _ptsStiffenerBottomRightPoints;


		//
		// dummy
		string _strMaterialKey;

		public BridgeGirderSample()
		{
		}

		public void Query(IUI ui)
		{
			Console.WriteLine("Query called");

			eUIErrorCodes errCode;

			
			ui.ClassFilter.AppendAcceptedBaseClass(FilerObject.eObjectType.kBeam);
			FilerObject fobj = ui.SelectObject(666, false, out errCode);

			if (null != fobj)
			{
				IJoint jnt = DatabaseManager.Open(JointId) as IJoint;

				if (null != jnt)
				{
					jnt.SetInputObjects(new FilerObject[] { fobj }, new int[] { -1 });
				}
			}

			LoadDefaultValues();
		}

		private void LoadDefaultValues()
		{
			//web info
			_webThickness = 20;
			_numberOfSegmentPoints = 4;
			_webTopHeight = new List<double>() { 500, 500, 500, 500 };
			_webTopHeightDistToIntermPt = new List<double>() { 10000, 20000 };
			_webBottomHeight = new List<double>() { 1000, 700, 1700, 500 };
			_webBottomHeightDistToIntermPt = new List<double>() { 10000, 20000 };

			//top flange info
			_flangeTopThickness = 15;
			_flangeTopLeftWidth = new List<double>() { 500, 500, 500, 500 };
			_flangeTopRightWidth = new List<double>() { 500, 500, 500, 500 };

			//bottom flange info
			_flangeBottomThickness = 15;
			_flangeBottomLeftWidth = new List<double>() { 500, 500, 500, 500 };
			_flangeBottomRightWidth = new List<double>() { 500, 500, 500, 500 };

			//grid info
			_numberOfRows = 6;
			_distanceBetweenRows = 4000;
			_curvature = 0;

			//stiffener info
			_stiffenerThickness = 15;
			_numberOfStiffenersPerBeam = 6;
			_stiffenerIsPerpendicularOnXAxis = 1;

			//bracing info
			_bracingProfileName = "Flat nach DIN#@§@#FL100X10";

			_strMaterialKey = "09CuP";

			//bolts&anchors
			_createAnchorsInstead = 0;

			//bolts
			_boltType = "ASTM A325";
			_boltMaterial = "10.9";
			_boltDiameter = 19.05;
			_boltSet = "MuS";

			_weldKey = 1;

			//anchors
			_anchorType = "US Normal Anchors";
			_anchorMaterial = "10.9";
			_anchorDiameter = 19.05;
			_anchorSet = "MuS";
			_anchorLength = 203.19999999999999;

			//shear studs
			_studType = "Nelson S3L-Inch";
			_studMaterial = "Mild Steel";
			_studDiameter = 19.05;
			_studLength = 101.59999999999999;
		}

		public void CreateObjects()
		{
			//
			// get my start points
			IJoint jnt = DatabaseManager.Open(JointId) as IJoint;
			IEnumerable<FilerObject> input = jnt.InputObjects;

			//
			// prepare the output
			List<CreatedObjectInformation> arrCOI = new List<CreatedObjectInformation>();		


			try
			{
				
				if (input.Count() == 1)
				{
					StraightBeam beam = input.ElementAt(0) as StraightBeam;
					//UserDefinedPoint udpEnd = input.ElementAt(1) as UserDefinedPoint;
					if (null != beam)
					{
						InitializeNonFilerMembers();

						//
						// this is basically some data validation and preparation for avoiding many checks or problems later						
						Point3d ptBeamStart = beam.GetPointAtStart(0);
						Point3d ptBeamEnd = beam.GetPointAtEnd(0);

						AdjustParameters(ptBeamStart, ptBeamEnd);


						//
						// for now the parallel beams are the same - we just repeat the creation of one as many times as necessary
						// even in the future when there may be slight differences between them we should be able to incorporate them
						// in the "one beam creation" code and still call it like this

						double dHalfRows = (_numberOfRows - 1) / 2.0;

						for (int i = 0; i < _numberOfRows; i++)
						{
							double dCurrCoef = 1 - Math.Abs(dHalfRows - i) / dHalfRows;

							Point3d ptStart = ptBeamStart + _yAxis * _distanceBetweenRows * i + _zAxis * dCurrCoef * _curvature;
							Point3d ptEnd = ptBeamEnd + _yAxis * _distanceBetweenRows * i + _zAxis * dCurrCoef * _curvature;

							//
							// start with the web of the beam
							createWebPlate(arrCOI, ptStart, ptEnd);
							//
							// top and bottom flanges
							createFlangePlates(arrCOI, ptStart, ptEnd);

							//
							// build the stiffners - this will also set some key points information in some temporary (non-filerable) points lists for placing the bracings
							createStiffeners(arrCOI, ptStart, ptEnd);
						}

						//
						// finally create all the bracings where it makes sense with the help of the information previously prepared by the stiffner creation method
						createBracings(arrCOI);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.Assert(false, ex.Message);
			}

			//
			// created objects need to be set to the joint
			jnt.SetCreatedObjects(arrCOI, true);
		}

		private void InitializeNonFilerMembers()
		{

			_webPlateIndex = 0;
			_flangePlateIndex = 0;
			_stiffenerIndex = 0;
			_bracingIndex = 0;
			_ptsStiffenerTopLeftPoints = new List<List<Point3d>>();
			_ptsStiffenerTopRightPoints = new List<List<Point3d>>();
			_ptsStiffenerBottomLeftPoints = new List<List<Point3d>>();
			_ptsStiffenerBottomRightPoints = new List<List<Point3d>>();
		}

		public void Save(IFiler filer)
		{
			filer.WriteVersion(1);

			//web info
			filer.WriteItem(_webThickness, "_webThickness");
			filer.WriteItem(_numberOfSegmentPoints, "_numberOfSegmentPoints");
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				filer.WriteItem(_webTopHeight[i], "_webTopHeight_" + i.ToString());
			}
			for (int i = 0; i < _numberOfSegmentPoints - 2; i++)
			{
				filer.WriteItem(_webTopHeightDistToIntermPt[i], "_webTopHeightDistToIntermPt_" + i.ToString());
			}
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				filer.WriteItem(_webBottomHeight[i], "_webBottomHeight_" + i.ToString());
			}
			for (int i = 0; i < _numberOfSegmentPoints - 2; i++)
			{
				filer.WriteItem(_webBottomHeightDistToIntermPt[i], "_webBottomHeightDistToIntermPt_" + i.ToString());
			}

			//top flange info
			filer.WriteItem(_flangeTopThickness, "_flangeTopThickness");
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				filer.WriteItem(_flangeTopLeftWidth[i], "_flangeTopLeftWidth_" + i.ToString());
			}
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				filer.WriteItem(_flangeTopRightWidth[i], "_flangeTopRightWidth_" + i.ToString());
			}

			//bottom flange info
			filer.WriteItem(_flangeBottomThickness, "_flangeBottomThickness");
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				filer.WriteItem(_flangeBottomLeftWidth[i], "_flangeBottomLeftWidth_" + i.ToString());
			}
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				filer.WriteItem(_flangeBottomRightWidth[i], "_flangeBottomRightWidth_" + i.ToString());
			}

			//grid info
			filer.WriteItem(_numberOfRows, "_numberOfRows");
			filer.WriteItem(_distanceBetweenRows, "_distanceBetweenRows");
			filer.WriteItem(_curvature, "_curvature");

			//stiffener info
			filer.WriteItem(_stiffenerThickness, "_stiffenerThickness");
			filer.WriteItem(_numberOfStiffenersPerBeam, "_numberOfStiffenersPerBeam");
			filer.WriteItem(_stiffenerIsPerpendicularOnXAxis, "_stiffenerIsPerpendicularOnXAxis");

			//bracing info
			filer.WriteItem(_bracingProfileName, "_bracingProfileName");


			//
			// dummy
			filer.WriteItem(_strMaterialKey, "_strMaterialKey");

			//test stuff
			filer.WriteItem(_createAnchorsInstead, "_createAnchorsInstead");

			//bolts
			filer.WriteItem(_boltType, "_boltType");
			filer.WriteItem(_boltMaterial, "_boltMaterial");
			filer.WriteItem(_boltDiameter, "_boltDiameter");
			filer.WriteItem(_boltSet, "_boltSet");

			//weld
			filer.WriteItem(_weldKey, "_weldKey");

			//anchors
			filer.WriteItem(_anchorType, "_anchorType");
			filer.WriteItem(_anchorMaterial, "_anchorMaterial");
			filer.WriteItem(_anchorDiameter, "_anchorDiameter");
			filer.WriteItem(_anchorSet, "_anchorSet");
			filer.WriteItem(_anchorLength, "_anchorLength");

			//shear studs
			filer.WriteItem(_studType, "_studType");
			filer.WriteItem(_studMaterial, "_studMaterial");
			filer.WriteItem(_studDiameter, "_studDiameter");
			filer.WriteItem(_studLength, "_studLength");

			filer.WriteItem(_dSomeUnitValue, "_dSomeUnitValue");
		}

		public void Load(IFiler filer)
		{
			int version = filer.ReadVersion();			

			//web info
			_webThickness = (double)filer.ReadItem("_webThickness");
			_numberOfSegmentPoints = (int)filer.ReadItem("_numberOfSegmentPoints");
			_webTopHeight = new List<double>();
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				_webTopHeight.Add((double)filer.ReadItem("_webTopHeight_" + i.ToString()));
			}
			_webTopHeightDistToIntermPt = new List<double>();
			for (int i = 0; i < _numberOfSegmentPoints - 2; i++)
			{
				_webTopHeightDistToIntermPt.Add((double)filer.ReadItem("_webTopHeightDistToIntermPt_" + i.ToString()));
			}
			_webBottomHeight = new List<double>();
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				_webBottomHeight.Add((double)filer.ReadItem("_webBottomHeight_" + i.ToString()));
			}
			_webBottomHeightDistToIntermPt = new List<double>();
			for (int i = 0; i < _numberOfSegmentPoints - 2; i++)
			{
				_webBottomHeightDistToIntermPt.Add((double)filer.ReadItem("_webBottomHeightDistToIntermPt_" + i.ToString()));
			}

			//top flange info
			_flangeTopThickness = (double)filer.ReadItem("_flangeTopThickness");
			_flangeTopLeftWidth = new List<double>();
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				_flangeTopLeftWidth.Add((double)filer.ReadItem("_flangeTopLeftWidth_" + i.ToString()));
			}
			_flangeTopRightWidth = new List<double>();
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				_flangeTopRightWidth.Add((double)filer.ReadItem("_flangeTopRightWidth_" + i.ToString()));
			}

			//bottom flange info
			_flangeBottomThickness = (double)filer.ReadItem("_flangeBottomThickness");
			_flangeBottomLeftWidth = new List<double>();
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				_flangeBottomLeftWidth.Add((double)filer.ReadItem("_flangeBottomLeftWidth_" + i.ToString()));
			}
			_flangeBottomRightWidth = new List<double>();
			for (int i = 0; i < _numberOfSegmentPoints; i++)
			{
				_flangeBottomRightWidth.Add((double)filer.ReadItem("_flangeBottomRightWidth_" + i.ToString()));
			}

			//grid info
			_numberOfRows = (int)filer.ReadItem("_numberOfRows");
			_distanceBetweenRows = (double)filer.ReadItem("_distanceBetweenRows");
			_curvature = (double)filer.ReadItem("_curvature");

			//stiffener info
			_stiffenerThickness = (double)filer.ReadItem("_stiffenerThickness");
			_numberOfStiffenersPerBeam = (int)filer.ReadItem("_numberOfStiffenersPerBeam");
			_stiffenerIsPerpendicularOnXAxis = (int)filer.ReadItem("_stiffenerIsPerpendicularOnXAxis");

			//bracing info
			_bracingProfileName = (string)filer.ReadItem("_bracingProfileName");

			//
			// dummy
			_strMaterialKey = (string)filer.ReadItem("_strMaterialKey");

			//test stuff
			_createAnchorsInstead = (int)filer.ReadItem("_createAnchorsInstead");

			//bolts
			_boltType = (string)filer.ReadItem("_boltType");
			_boltMaterial = (string)filer.ReadItem("_boltMaterial");
			_boltDiameter = (double)filer.ReadItem("_boltDiameter");
			_boltSet = (string)filer.ReadItem("_boltSet");

			//weld
			_weldKey = (int)filer.ReadItem("_weldKey");

			//anchors
			_anchorType = (string)filer.ReadItem("_anchorType");
			_anchorMaterial = (string)filer.ReadItem("_anchorMaterial");
			_anchorDiameter = (double)filer.ReadItem("_anchorDiameter");
			_anchorSet = (string)filer.ReadItem("_anchorSet");
			_anchorLength = (double)filer.ReadItem("_anchorLength");

			//shear studs
			_studType = (string)filer.ReadItem("_studType");
			_studMaterial = (string)filer.ReadItem("_studMaterial");
			_studDiameter = (double)filer.ReadItem("_studDiameter");
			_studLength = (double)filer.ReadItem("_studLength");

			_dSomeUnitValue = (double)filer.ReadItem("_dSomeUnitValue");
		}

		public void GetRulePages(IRuleUIBuilder builder)
		{
			// ids are copied from an ApexTieAntiSag page
			builder.SheetPromptId = 97065;
			builder.FirstPageBitmapIdx = 68258;

			string img = "Dummy.png";
			string img2 = "ResDummy.png";
			string dllName = "SteelConnectionsDotNetJointExample.dll";

			//top web info page
			int nId = builder.BuildRulePage(87790, -1, 60601); // no group, page title and bitmap are copied from an ApexAntiSag page

			builder.AddTextBox(nId, "_webThickness", "_webThickness", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webTopHeightStart", "_webTopHeight_0", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webTopHeightInterm1", "_webTopHeight_1", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webTopHeightInterm2", "_webTopHeight_2", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webTopHeightEnd", "_webTopHeight_3", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webTopHeightDistToIntermPt1", "_webTopHeightDistToIntermPt_0", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webTopHeightDistToIntermPt2", "_webTopHeightDistToIntermPt_1", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);

			builder.AddTextBox(nId, "Test angle units", "_dSomeUnitValue", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kAngle);


			builder.AddCheckBox(nId, "_createAnchorsInstead", "_createAnchorsInstead");


			// bottom web info page
			nId = builder.BuildRulePage(97053, -1, dllName, img2);
			builder.AddTextBox(nId, "_webBottomHeightStart", "_webBottomHeight_0", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webBottomHeightInterm1", "_webBottomHeight_1", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webBottomHeightInterm2", "_webBottomHeight_2", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webBottomHeightEnd", "_webBottomHeight_3", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webBottomHeightDistToIntermPt1", "_webBottomHeightDistToIntermPt_0", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_webBottomHeightDistToIntermPt2", "_webBottomHeightDistToIntermPt_1", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);

			//top flange info page
			nId = builder.BuildRulePage(97053, -1, dllName, img);
			builder.AddTextBox(nId, "_flangeTopThickness", "_flangeTopThickness", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeTopLeftWidthStart", "_flangeTopLeftWidth_0", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeTopLeftWidthInterm1", "_flangeTopLeftWidth_1", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeTopLeftWidthInterm2", "_flangeTopLeftWidth_2", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeTopLeftWidthEnd", "_flangeTopLeftWidth_3", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeTopRightWidthStart", "_flangeTopRightWidth_0", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeTopRightWidthInterm1", "_flangeTopRightWidth_1", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeTopRightWidthInterm2", "_flangeTopRightWidth_2", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeTopRightWidthEnd", "_flangeTopRightWidth_3", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);

			//bottom flange info page
			nId = builder.BuildRulePage(97053, -1, dllName, img2);
			builder.AddTextBox(nId, "_flangeBottomThickness", "_flangeBottomThickness", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeBottomLeftWidthStart", "_flangeBottomLeftWidth_0", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeBottomLeftWidthInterm1", "_flangeBottomLeftWidth_1", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeBottomLeftWidthInterm2", "_flangeBottomLeftWidth_2", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeBottomLeftWidthEnd", "_flangeBottomLeftWidth_3", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeBottomRightWidthStart", "_flangeBottomRightWidth_0", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeBottomRightWidthInterm1", "_flangeBottomRightWidth_1", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeBottomRightWidthInterm2", "_flangeBottomRightWidth_2", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_flangeBottomRightWidthEnd", "_flangeBottomRightWidth_3", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);

			//grid definition
			nId = builder.BuildRulePage(97053, -1, dllName, img);
			builder.AddTextBox(nId, "_numberOfRows", "_numberOfRows", typeof(int));
			builder.AddTextBox(nId, "_distanceBetweenRows", "_distanceBetweenRows", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_curvature", "_curvature", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);

			//stiffener info
			nId = builder.BuildRulePage(97053, -1, dllName, img);
			builder.AddTextBox(nId, "_stiffenerThickness", "_stiffenerThickness", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);
			builder.AddTextBox(nId, "_numberOfStiffenersPerBeam", "_numberOfStiffenersPerBeam", typeof(int));
			builder.AddSelectorCustom(nId, "_stiffenerIsPerpendicularOnXAxis", "_stiffenerIsPerpendicularOnXAxis", typeof(int), new string[] { "vertical", "perpendicular" });

			//bracing info
			nId = builder.BuildRulePage(97053, -1, dllName, img2);
			builder.AddSelector(nId, IRuleUIBuilder.eSelectorType.kProfile, "_bracingProfileName", "_bracingProfileName", new string[] { "W", "I", "F" });

			//material info
			builder.AddSelector(nId, IRuleUIBuilder.eSelectorType.kMaterial, "_strMaterialKey", "_strMaterialKey", Enumerable.Empty<string>());

			//bolts info
			nId = builder.BuildRulePage(4200, -1, dllName, img2);
			builder.AddCheckBox(nId, "_createAnchorsInstead", "_createAnchorsInstead");
			builder.AddBoltSelector(nId, "Bolt properties label", "_boltType", "_boltMaterial", "_boltDiameter", "_boltSet");
			builder.AddAnchorSelector(nId, "Anchor properties label", "_anchorType", "_anchorMaterial", "_anchorDiameter", "_anchorSet", "_anchorLength");

			//shear studs
			nId = builder.BuildRulePage(31208, -1, dllName, img2);
			builder.AddConnectorSelector(nId, "Shear studs properties label", "_studType", "_studMaterial", "_studDiameter");
			builder.AddTextBox(nId, "_studLength", "_studLength", typeof(double), Autodesk.AdvanceSteel.DotNetRoots.Units.Unit.eUnitType.kDistance);

			//weld
			nId = builder.BuildRulePage(97053, -1, dllName, img2);
			builder.AddSelector(nId, IRuleUIBuilder.eSelectorType.kWeld, "_weldKey", "_weldKey", Enumerable.Empty<string>());
		}

		public string GetTableName()
		{
			return "";
		}

		public ObjectId JointId { get { return _jointId; } set { _jointId = value; } }

		private void AdjustParameters(Point3d ptBeamStart, Point3d ptBeamEnd)
		{
			computeJointCS(ptBeamStart, ptBeamEnd);

			double beamLength = ptBeamStart.DistanceTo(ptBeamEnd);
			//make sure the breaking point are in the right order and that they are "inside" the beam
			if (_webTopHeightDistToIntermPt[0] > beamLength || _webTopHeightDistToIntermPt[0] <= 0)
			{
				_webTopHeightDistToIntermPt[0] = beamLength / 3;
			}

			if (_webTopHeightDistToIntermPt[1] > beamLength || _webTopHeightDistToIntermPt[1] <= 0)
			{
				_webTopHeightDistToIntermPt[1] = beamLength * 2 / 3;
			}

			if (_webBottomHeightDistToIntermPt[0] > beamLength || _webBottomHeightDistToIntermPt[0] <= 0)
			{
				_webBottomHeightDistToIntermPt[0] = beamLength / 3;
			}

			if (_webBottomHeightDistToIntermPt[1] > beamLength || _webBottomHeightDistToIntermPt[1] <= 0)
			{
				_webBottomHeightDistToIntermPt[1] = beamLength * 2 / 3;
			}

			if (_webTopHeightDistToIntermPt[0] > _webTopHeightDistToIntermPt[1])
			{
				double tmp = _webTopHeightDistToIntermPt[0];
				_webTopHeightDistToIntermPt[0] = _webTopHeightDistToIntermPt[1];
				_webTopHeightDistToIntermPt[1] = tmp;
			}

			if (_webBottomHeightDistToIntermPt[0] > _webBottomHeightDistToIntermPt[1])
			{
				double tmp = _webBottomHeightDistToIntermPt[0];
				_webBottomHeightDistToIntermPt[0] = _webBottomHeightDistToIntermPt[1];
				_webBottomHeightDistToIntermPt[1] = tmp;
			}
		}

		private void computeJointCS(Point3d ptBeamStart, Point3d ptBeamEnd)
		{
			_xAxis = ptBeamEnd.Subtract(ptBeamStart);
			if (!_xAxis.IsParallelTo(Vector3d.kZAxis))
				_yAxis = Vector3d.kZAxis.CrossProduct(_xAxis);
			else
				_yAxis = Vector3d.kYAxis;

			_zAxis = _xAxis.CrossProduct(_yAxis);

			//
			// it's better to make sure
			_xAxis.Normalize();
			_yAxis.Normalize();
			_zAxis.Normalize();
		}

		private void createWebPlate(List<CreatedObjectInformation> arrCOI, Point3d ptBeamStart, Point3d ptBeamEnd)
		{
			//compute the web plane
			Plane planeWeb = new Plane(ptBeamStart, _yAxis);

			//
			// having the data in a nice form allows us to just start with a point, and just keep adding the proper
			// vertices until we reach each corner of the web plate... until the end

			//compute the web vertices
			List<Point3d> arrWebVertices = new List<Point3d>();
			Point3d ptTmp = ptBeamStart + _zAxis * _webTopHeight[0];
			arrWebVertices.Add(ptTmp);//top1 = top start

			ptTmp = ptBeamStart + _zAxis * _webTopHeight[1];
			ptTmp = ptTmp + _xAxis * _webTopHeightDistToIntermPt[0];
			arrWebVertices.Add(ptTmp);//top2 = top interm 1

			ptTmp = ptBeamStart + _zAxis * _webTopHeight[2];
			ptTmp = ptTmp + _xAxis * _webTopHeightDistToIntermPt[1];
			arrWebVertices.Add(ptTmp);//top3 = top interm 2

			ptTmp = ptBeamEnd + _zAxis * _webTopHeight[3];
			arrWebVertices.Add(ptTmp);//top4 = top end

			ptTmp = ptBeamEnd + _zAxis * (-_webBottomHeight[3]);
			arrWebVertices.Add(ptTmp);//bottom4 = bottom end

			ptTmp = ptBeamStart + _zAxis * (-_webBottomHeight[2]);
			ptTmp = ptTmp + _xAxis * _webBottomHeightDistToIntermPt[1];
			arrWebVertices.Add(ptTmp);//bottom3 = bottom interm 2

			ptTmp = ptBeamStart + _zAxis * (-_webBottomHeight[1]);
			ptTmp = ptTmp + _xAxis * _webBottomHeightDistToIntermPt[0];
			arrWebVertices.Add(ptTmp);//bottom2 = bottom interm 1

			ptTmp = ptBeamStart + _zAxis * (-_webBottomHeight[0]);
			arrWebVertices.Add(ptTmp);//bottom1 = bottom start

			CreatePlate(arrCOI, planeWeb, arrWebVertices, "WebPlate_" + (_webPlateIndex++).ToString(), 0.5, _webThickness);
		}

		private void CreatePlate(List<CreatedObjectInformation> arrCOI, Plane plane, List<Point3d> arrVertices, string role, double portioning, double thickness)
		{
			Plate plate = new Plate(plane, arrVertices.ToArray(), thickness);
			if (plate != null)
			{
				plate.Portioning = portioning;
				plate.WriteToDb();

				CreatedObjectInformation coi = new CreatedObjectInformation();
				coi.CreatedObject = plate;
				coi.JointTransferId = role;
				coi.Attributes = new eObjectsAttribute[0];

				arrCOI.Add(coi);

				if (role.Contains("WebPlate_"))
				{
					CreateShearStuds(arrCOI, plate, "connector_" + role);
				}
			}
		}

		private void createFlangePlates(List<CreatedObjectInformation> arrCOI, Point3d ptBeamStart, Point3d ptBeamEnd)
		{
			//create flange top start
			Point3d ptCenterStart = ptBeamStart + _zAxis * _webTopHeight[0];
			Point3d ptCenterEnd = ptBeamStart + _zAxis * _webTopHeight[1];
			ptCenterEnd = ptCenterEnd + _xAxis * _webTopHeightDistToIntermPt[0];
			createFlangePlate(arrCOI, ptCenterStart, ptCenterEnd, _flangeTopLeftWidth[0], _flangeTopRightWidth[0], _flangeTopLeftWidth[1], _flangeTopRightWidth[1], _flangeTopThickness, 1);

			//create flange top middle
			ptCenterStart = ptCenterEnd;
			ptCenterEnd = ptBeamStart + _zAxis * _webTopHeight[2];
			ptCenterEnd = ptCenterEnd + _xAxis * _webTopHeightDistToIntermPt[1];
			createFlangePlate(arrCOI, ptCenterStart, ptCenterEnd, _flangeTopLeftWidth[1], _flangeTopRightWidth[1], _flangeTopLeftWidth[2], _flangeTopRightWidth[2], _flangeTopThickness, 1);

			//create flange top end
			ptCenterStart = ptCenterEnd;
			ptCenterEnd = ptBeamEnd + _zAxis * _webTopHeight[3];
			createFlangePlate(arrCOI, ptCenterStart, ptCenterEnd, _flangeTopLeftWidth[2], _flangeTopRightWidth[2], _flangeTopLeftWidth[3], _flangeTopRightWidth[3], _flangeTopThickness, 1);

			//create flange bottom start
			ptCenterStart = ptBeamStart + _zAxis * (-_webBottomHeight[0]);
			ptCenterEnd = ptBeamStart + _zAxis * (-_webBottomHeight[1]);
			ptCenterEnd = ptCenterEnd + _xAxis * (_webBottomHeightDistToIntermPt[0]);
			createFlangePlate(arrCOI, ptCenterStart, ptCenterEnd, _flangeBottomLeftWidth[0], _flangeBottomRightWidth[0], _flangeBottomLeftWidth[1], _flangeBottomRightWidth[1], _flangeBottomThickness, 0);

			//create flange bottom middle
			ptCenterStart = ptCenterEnd;
			ptCenterEnd = ptBeamStart + _zAxis * (-_webBottomHeight[2]);
			ptCenterEnd = ptCenterEnd + _xAxis * _webBottomHeightDistToIntermPt[1];
			createFlangePlate(arrCOI, ptCenterStart, ptCenterEnd, _flangeBottomLeftWidth[1], _flangeBottomRightWidth[1], _flangeBottomLeftWidth[2], _flangeBottomRightWidth[2], _flangeBottomThickness, 0);

			//create flange bottom end
			ptCenterStart = ptCenterEnd;
			ptCenterEnd = ptBeamEnd + _zAxis * (-_webBottomHeight[3]);
			createFlangePlate(arrCOI, ptCenterStart, ptCenterEnd, _flangeBottomLeftWidth[2], _flangeBottomRightWidth[2], _flangeBottomLeftWidth[3], _flangeBottomRightWidth[3], _flangeBottomThickness, 0);
		}

		private void createFlangePlate(List<CreatedObjectInformation> arrCOI, Point3d ptCenterStart, Point3d ptCenterEnd,
					double startLeftWidth, double startRightWidth, double endLeftWidth, double endRightWidth, double flangeThickness, double portioning)
		{
			//compute the flange plane
			Vector3d vDir = ptCenterEnd.Subtract(ptCenterStart);
			Plane planeFlange = new Plane(ptCenterStart, vDir.CrossProduct(_yAxis));

			//compute the web vertices
			List<Point3d> arrFlangeVertices = new List<Point3d>();
			Point3d ptTmp = ptCenterStart + _yAxis * startLeftWidth;
			arrFlangeVertices.Add(ptTmp);

			ptTmp = ptCenterEnd + _yAxis * endLeftWidth;
			arrFlangeVertices.Add(ptTmp);

			ptTmp = ptCenterEnd + _yAxis * (-endRightWidth);
			arrFlangeVertices.Add(ptTmp);

			ptTmp = ptCenterStart + _yAxis * (-startRightWidth);
			arrFlangeVertices.Add(ptTmp);

			CreatePlate(arrCOI, planeFlange, arrFlangeVertices, "FlangePlate_" + (_flangePlateIndex++).ToString(), portioning, flangeThickness);
		}

		private void createStiffeners(List<CreatedObjectInformation> arrCOI, Point3d ptBeamStart, Point3d ptBeamEnd)
		{
			List<Point3d> ptsTopLeft = new List<Point3d>();
			List<Point3d> ptsTopRight = new List<Point3d>();
			List<Point3d> ptsBottomLeft = new List<Point3d>();
			List<Point3d> ptsBottomRight = new List<Point3d>();

			double beamLength = (ptBeamEnd - ptBeamStart).GetLength() - _stiffenerThickness;
			for (int i = 0; i < _numberOfStiffenersPerBeam; i++)
			{
				Point3d ptStiffenerCenter = ptBeamStart + _xAxis * (_stiffenerThickness / 2 + beamLength / _numberOfStiffenersPerBeam * i) + _xAxis * beamLength / _numberOfStiffenersPerBeam / 2;
				createStiffener(arrCOI, ptBeamStart, ptBeamEnd, ptStiffenerCenter, true, ref ptsTopLeft, ref ptsBottomLeft);
				createStiffener(arrCOI, ptBeamStart, ptBeamEnd, ptStiffenerCenter, false, ref ptsTopRight, ref ptsBottomRight);
			}

			_ptsStiffenerTopLeftPoints.Add(ptsTopLeft);
			_ptsStiffenerTopRightPoints.Add(ptsTopRight);
			_ptsStiffenerBottomLeftPoints.Add(ptsBottomLeft);
			_ptsStiffenerBottomRightPoints.Add(ptsBottomRight);
		}

		private void createStiffener(List<CreatedObjectInformation> arrCOI, Point3d ptBeamStart, Point3d ptBeamEnd, Point3d ptStiffenerCenterOnBeamAxis, bool leftSide, ref List<Point3d> ptsTop, ref List<Point3d> ptsBottom)
		{
			//compute the stiffener plane
			Vector3d vStiffNormal = _xAxis;
			Vector3d vTop = _zAxis;
			bool isVertical = false;
			if (0 == _stiffenerIsPerpendicularOnXAxis)
			{
				Vector3d zWCS = new Vector3d(0, 0, 1);
				if (!zWCS.IsParallelTo(_xAxis))
				{
					vStiffNormal = (zWCS.CrossProduct(_xAxis)).CrossProduct(zWCS);
					vTop = zWCS;
					isVertical = true;
				}
			}
			Vector3d vSide = vTop.CrossProduct(vStiffNormal);
			if (!leftSide)
			{
				vSide *= -1;
			}

			vStiffNormal.Normalize();
			vTop.Normalize();
			vSide.Normalize();

			Plane plane = new Plane(ptStiffenerCenterOnBeamAxis, vStiffNormal);

			//compute the stiffener vertices
			List<Point3d> arrVertices = new List<Point3d>();
			double portioning = 0.5;
			Point3d ptFace1 = ptStiffenerCenterOnBeamAxis - vStiffNormal * _stiffenerThickness / 2;
			Point3d ptFace2 = ptStiffenerCenterOnBeamAxis + vStiffNormal * _stiffenerThickness / 2;			

			int segmentIndexFace1 = 0;
			int segmentIndexFace2 = 0;
			double stiffTopWebHeight = Math.Min(getWebHeightAtPoint(ptBeamStart, ptBeamEnd, ptFace1, true, isVertical, out segmentIndexFace1), getWebHeightAtPoint(ptBeamStart, ptBeamEnd, ptFace2, true, isVertical, out segmentIndexFace2));
			if (segmentIndexFace1 != segmentIndexFace2)
			{
				Point3d ptSegmentation = GetSegmentPoint(ptBeamStart, ptBeamEnd, segmentIndexFace2, true);
				stiffTopWebHeight = Math.Min(stiffTopWebHeight, getWebHeightAtPoint(ptBeamStart, ptBeamEnd, ptSegmentation, true, isVertical, out segmentIndexFace2));
			}
			double stiffBottomWebHeight = Math.Min(getWebHeightAtPoint(ptBeamStart, ptBeamEnd, ptFace1, false, isVertical, out segmentIndexFace1), getWebHeightAtPoint(ptBeamStart, ptBeamEnd, ptFace2, false, isVertical, out segmentIndexFace2));
			if (segmentIndexFace1 != segmentIndexFace2)
			{
				Point3d ptSegmentation = GetSegmentPoint(ptBeamStart, ptBeamEnd, segmentIndexFace2, false);
				stiffBottomWebHeight = Math.Min(stiffBottomWebHeight, getWebHeightAtPoint(ptBeamStart, ptBeamEnd, ptSegmentation, false, isVertical, out segmentIndexFace2));
			}
			double stiffTopFlangeWidth = Math.Min(getFlangeWidthAtPoint(ptBeamStart, ptBeamEnd, ptFace1, true, leftSide, out segmentIndexFace1), getFlangeWidthAtPoint(ptBeamStart, ptBeamEnd, ptFace2, true, leftSide, out segmentIndexFace2));
			if (segmentIndexFace1 != segmentIndexFace2)
			{
				Point3d ptSegmentation = GetSegmentPoint(ptBeamStart, ptBeamEnd, segmentIndexFace2, true);
				stiffTopFlangeWidth = Math.Min(stiffTopFlangeWidth, getFlangeWidthAtPoint(ptBeamStart, ptBeamEnd, ptSegmentation, true, leftSide, out segmentIndexFace2));
			}
			double stiffBottomFlangeWidth = Math.Min(getFlangeWidthAtPoint(ptBeamStart, ptBeamEnd, ptFace1, false, leftSide, out segmentIndexFace1), getFlangeWidthAtPoint(ptBeamStart, ptBeamEnd, ptFace2, false, leftSide, out segmentIndexFace2));
			if (segmentIndexFace1 != segmentIndexFace2)
			{
				Point3d ptSegmentation = GetSegmentPoint(ptBeamStart, ptBeamEnd, segmentIndexFace2, false);
				stiffBottomFlangeWidth = Math.Min(stiffBottomFlangeWidth, getFlangeWidthAtPoint(ptBeamStart, ptBeamEnd, ptSegmentation, false, leftSide, out segmentIndexFace2));
			}

			//
			// having the dimensions, build the points and the plate
			Point3d ptStart = ptStiffenerCenterOnBeamAxis + vSide * _webThickness / 2;
			Point3d ptTmp = ptStart + vTop * stiffTopWebHeight;
			arrVertices.Add(ptTmp);//pt stiffener top web

			ptTmp = ptTmp + vSide * (stiffTopFlangeWidth - _webThickness / 2);
			arrVertices.Add(ptTmp);//pt stiffener top flange
			ptsTop.Add(ptTmp);

			ptTmp = ptStart - vTop * stiffBottomWebHeight;
			ptTmp = ptTmp + vSide * (stiffBottomFlangeWidth - _webThickness / 2);
			arrVertices.Add(ptTmp);//pt stiffener bottom flange
			ptsBottom.Add(ptTmp);

			ptTmp = ptStart - vTop * stiffBottomWebHeight;
			arrVertices.Add(ptTmp);//pt stiffener bottom web

			CreatePlate(arrCOI, plane, arrVertices, "Stiffener_" + (_stiffenerIndex++).ToString(), portioning, _stiffenerThickness);
		}

		private double getWebHeightAtSegmentPoint(int segmentIndex, bool top)
		{
			double height = 0;
			if (top)
			{
				height = _webTopHeight[segmentIndex];
			}
			else
			{
				height = _webBottomHeight[segmentIndex];
			}

			return height;
		}

		//
		// returns the start of the required flange segment - works for last segment + 1 to get the end point
		private Point3d GetSegmentPoint(Point3d ptBeamStart, Point3d ptBeamEnd, int segmentIndex, bool top)
		{
			Point3d ptSegmentation;
			if (segmentIndex == 0)
			{
				ptSegmentation = ptBeamStart;
			}
			else if (segmentIndex == 1)
			{
				if (top)
				{
					ptSegmentation = ptBeamStart + _xAxis * _webTopHeightDistToIntermPt[0];
				}
				else
				{
					ptSegmentation = ptBeamStart + _xAxis * _webBottomHeightDistToIntermPt[0];
				}
			}
			else if (segmentIndex == 2)
			{
				if (top)
				{
					ptSegmentation = ptBeamStart + _xAxis * _webTopHeightDistToIntermPt[1];
				}
				else
				{
					ptSegmentation = ptBeamStart + _xAxis * _webBottomHeightDistToIntermPt[1];
				}
			}
			else
			{
				ptSegmentation = ptBeamEnd;
			}

			return ptSegmentation;
		}

		double getFlangeWidthAtPoint(Point3d ptBeamStart, Point3d ptBeamEnd, Point3d ptOnAxis, bool top, bool left, out int segmentIndex)
		{
			double flangeWidthStart = 0;
			double flangeWidthInterm1 = 0;
			double flangeWidthInterm2 = 0;
			double flangeWidthEnd = 0;
			if (left)
			{
				if (top)
				{
					flangeWidthStart = _flangeTopLeftWidth[0];
					flangeWidthInterm1 = _flangeTopLeftWidth[1];
					flangeWidthInterm2 = _flangeTopLeftWidth[2];
					flangeWidthEnd = _flangeTopLeftWidth[3];
				}
				else
				{
					flangeWidthStart = _flangeBottomLeftWidth[0];
					flangeWidthInterm1 = _flangeBottomLeftWidth[1];
					flangeWidthInterm2 = _flangeBottomLeftWidth[2];
					flangeWidthEnd = _flangeBottomLeftWidth[3];
				}
			}
			else
			{
				if (top)
				{
					flangeWidthStart = _flangeTopRightWidth[0];
					flangeWidthInterm1 = _flangeTopRightWidth[1];
					flangeWidthInterm2 = _flangeTopRightWidth[2];
					flangeWidthEnd = _flangeTopRightWidth[3];
				}
				else
				{
					flangeWidthStart = _flangeBottomRightWidth[0];
					flangeWidthInterm1 = _flangeBottomRightWidth[1];
					flangeWidthInterm2 = _flangeBottomRightWidth[2];
					flangeWidthEnd = _flangeBottomRightWidth[3];
				}
			}

			double flangeWidth = 0;
			double distFromStart = ptOnAxis.Subtract(ptBeamStart).GetLength();
			double beamLength = ptBeamEnd.Subtract(ptBeamStart).GetLength();
			if (top)
			{
				if (distFromStart <= _webTopHeightDistToIntermPt[0])
				{
					flangeWidth = flangeWidthStart + distFromStart * (flangeWidthInterm1 - flangeWidthStart) / _webTopHeightDistToIntermPt[0];
					segmentIndex = 0;
				}
				else if (distFromStart <= _webTopHeightDistToIntermPt[1])
				{
					flangeWidth = flangeWidthInterm1 + (distFromStart - _webTopHeightDistToIntermPt[0]) * (flangeWidthInterm2 - flangeWidthInterm1) / (_webTopHeightDistToIntermPt[1] - _webTopHeightDistToIntermPt[0]);
					segmentIndex = 1;
				}
				else
				{
					flangeWidth = flangeWidthInterm2 + (distFromStart - _webTopHeightDistToIntermPt[1]) * (flangeWidthEnd - flangeWidthInterm2) / (beamLength - _webTopHeightDistToIntermPt[1]);
					segmentIndex = 2;
				}
			}
			else
			{
				if (distFromStart <= _webBottomHeightDistToIntermPt[0])
				{
					flangeWidth = flangeWidthStart + distFromStart * (flangeWidthInterm1 - flangeWidthStart) / _webBottomHeightDistToIntermPt[0];
					segmentIndex = 0;
				}
				else if (distFromStart <= _webBottomHeightDistToIntermPt[1])
				{
					flangeWidth = flangeWidthInterm1 + (distFromStart - _webBottomHeightDistToIntermPt[0]) * (flangeWidthInterm2 - flangeWidthInterm1) / (_webBottomHeightDistToIntermPt[1] - _webBottomHeightDistToIntermPt[0]);
					segmentIndex = 1;
				}
				else
				{
					flangeWidth = flangeWidthInterm2 + (distFromStart - _webBottomHeightDistToIntermPt[1]) * (flangeWidthEnd - flangeWidthInterm2) / (beamLength - _webBottomHeightDistToIntermPt[1]);
					segmentIndex = 2;
				}
			}

			return flangeWidth;
		}

		double getWebHeightAtPoint(Point3d ptBeamStart, Point3d ptBeamEnd, Point3d ptOnAxis, bool top, bool isVertical, out int segmentIndex)
		{
			if (isVertical)
			{
				return getWebHeightAtPointForVerticalStiffener(ptBeamStart, ptBeamEnd, ptOnAxis, top, out segmentIndex);
			}

			return getWebHeightAtPointForPerpendicularStiffener(ptBeamStart, ptBeamEnd, ptOnAxis, top, out segmentIndex);
		}

		double getWebHeightAtPointForPerpendicularStiffener(Point3d ptBeamStart, Point3d ptBeamEnd, Point3d ptOnAxis, bool top, out int segmentIndex)
		{
			double webHeightStart = 0;
			double webHeightInterm1 = 0;
			double webHeightInterm2 = 0;
			double webHeightEnd = 0;

			//this should be adapted if we have more the 3 segments
			if (top)
			{
				webHeightStart = _webTopHeight[0];
				webHeightInterm1 = _webTopHeight[1];
				webHeightInterm2 = _webTopHeight[2];
				webHeightEnd = _webTopHeight[3];
			}
			else
			{
				webHeightStart = _webBottomHeight[0];
				webHeightInterm1 = _webBottomHeight[1];
				webHeightInterm2 = _webBottomHeight[2];
				webHeightEnd = _webBottomHeight[3];
			}

			double webHeight = 0;
			double distFromStart = ptOnAxis.Subtract(ptBeamStart).GetLength();
			double beamLength = ptBeamEnd.Subtract(ptBeamStart).GetLength();
			if (top)
			{
				if (distFromStart <= _webTopHeightDistToIntermPt[0])
				{
					webHeight = webHeightStart + distFromStart * (webHeightInterm1 - webHeightStart) / _webTopHeightDistToIntermPt[0];
					segmentIndex = 0;
				}
				else if (distFromStart <= _webTopHeightDistToIntermPt[1])
				{
					webHeight = webHeightInterm1 + (distFromStart - _webTopHeightDistToIntermPt[0]) * (webHeightInterm2 - webHeightInterm1) / (_webTopHeightDistToIntermPt[1] - _webTopHeightDistToIntermPt[0]);
					segmentIndex = 1;
				}
				else
				{
					webHeight = webHeightInterm2 + (distFromStart - _webTopHeightDistToIntermPt[1]) * (webHeightEnd - webHeightInterm2) / (beamLength - _webTopHeightDistToIntermPt[1]);
					segmentIndex = 2;
				}
			}
			else
			{
				if (distFromStart <= _webBottomHeightDistToIntermPt[0])
				{
					webHeight = webHeightStart + distFromStart * (webHeightInterm1 - webHeightStart) / _webBottomHeightDistToIntermPt[0];
					segmentIndex = 0;
				}
				else if (distFromStart <= _webBottomHeightDistToIntermPt[1])
				{
					webHeight = webHeightInterm1 + (distFromStart - _webBottomHeightDistToIntermPt[0]) * (webHeightInterm2 - webHeightInterm1) / (_webBottomHeightDistToIntermPt[1] - _webBottomHeightDistToIntermPt[0]);
					segmentIndex = 1;
				}
				else
				{
					webHeight = webHeightInterm2 + (distFromStart - _webBottomHeightDistToIntermPt[1]) * (webHeightEnd - webHeightInterm2) / (beamLength - _webBottomHeightDistToIntermPt[1]);
					segmentIndex = 2;
				}
			}

			return webHeight;
		}

		double getWebHeightAtPointForVerticalStiffener(Point3d ptBeamStart, Point3d ptBeamEnd, Point3d ptOnAxis, bool top, out int segmentIndex)
		{
			double distFromStart = ptOnAxis.DistanceTo(ptBeamStart);
			segmentIndex = getSegmentFromDistance(top, distFromStart);
			int sign = top ? 1 : -1;
			Point3d pt1 = GetSegmentPoint(ptBeamStart, ptBeamEnd, segmentIndex, top);
			Point3d pt2 = GetSegmentPoint(ptBeamStart, ptBeamEnd, segmentIndex + 1, top);
			Point3d ptFlange1 = pt1 + _zAxis * getWebHeightAtSegmentPoint(segmentIndex, top) * sign;
			Point3d ptFlange2 = pt2 + _zAxis * getWebHeightAtSegmentPoint(segmentIndex + 1, top) * sign;


			Plane plFlange = new Plane(ptFlange1, _yAxis.CrossProduct(ptFlange2.Subtract(ptFlange1)));

			Point3d ptOnFlange = ptOnAxis.Project(plFlange, new Vector3d(0, 0, 1));
			return ptOnFlange.DistanceTo(ptOnAxis);
		}

		int getSegmentFromDistance(bool top, double distFromStart)
		{
			int segmentIndex = 0;
			if (top)
			{
				if (distFromStart <= _webTopHeightDistToIntermPt[0])
				{
					segmentIndex = 0;
				}
				else if (distFromStart <= _webTopHeightDistToIntermPt[1])
				{
					segmentIndex = 1;
				}
				else
				{
					segmentIndex = 2;
				}
			}
			else
			{
				if (distFromStart <= _webBottomHeightDistToIntermPt[0])
				{
					segmentIndex = 0;
				}
				else if (distFromStart <= _webBottomHeightDistToIntermPt[1])
				{
					segmentIndex = 1;
				}
				else
				{
					segmentIndex = 2;
				}
			}
			return segmentIndex;
		}

		private void createBracings(List<CreatedObjectInformation> arrCOI)
		{
			for (int indexRows = 0; indexRows < _numberOfRows - 1; indexRows++)
			{
				for (int indexStiffeners = 0; indexStiffeners < _numberOfStiffenersPerBeam; indexStiffeners++)
				{
					Beam beam1 = createBracing(arrCOI, _ptsStiffenerTopLeftPoints[indexRows][indexStiffeners], _ptsStiffenerBottomRightPoints[indexRows + 1][indexStiffeners], "Bracing_" + (_bracingIndex++).ToString());
					Beam beam2 = createBracing(arrCOI, _ptsStiffenerBottomLeftPoints[indexRows][indexStiffeners], _ptsStiffenerTopRightPoints[indexRows + 1][indexStiffeners], "Bracing_" + (_bracingIndex++).ToString());

					if (_createAnchorsInstead == 0)
					{
						CreateBolts(arrCOI, beam1, beam2, "_Bolts" + (_bracingIndex).ToString());
					}
					else
					{
						CreateAnchors(arrCOI, beam1, beam2, "_Anchors" + (_bracingIndex).ToString());
					}

					CreateWelds(arrCOI, beam1, beam2, "_Welds" + (_bracingIndex).ToString());
				}
			}
		}

		private Beam createBracing(List<CreatedObjectInformation> arrCOI, Point3d ptStart, Point3d ptEnd, string role)
		{
			Beam beam = new StraightBeam(_bracingProfileName, ptStart, ptEnd, new Vector3d(0, 0, 1));
			if (beam != null)
			{
				beam.WriteToDb();

				beam.Material = _strMaterialKey;

				CreatedObjectInformation coi = new CreatedObjectInformation();
				coi.CreatedObject = beam;
				coi.JointTransferId = role;


				coi.Attributes = new eObjectsAttribute[] { eObjectsAttribute.kBeam_Denotation, eObjectsAttribute.kBeam_Material, eObjectsAttribute.kBeam_Mirrored };

				arrCOI.Add(coi);
			}

			return beam;
		}

		private void CreateWelds(List<CreatedObjectInformation> arrCOI, Beam beam1, Beam beam2, string role)
		{
			Point3d ptOrig1;
			Vector3d vX1, vY1, vZ1;
			beam1.PhysCSMid.GetCoordSystem(out ptOrig1, out vX1, out vY1, out vZ1);

			WeldPoint weldPattern = new WeldPoint(ptOrig1, vX1, vY1);
			weldPattern.SetWeldType(WeldPattern.eSeamPosition.kUpper, (WeldPattern.eWeldType)_weldKey);
			if (weldPattern != null)
			{
				weldPattern.WriteToDb();
				weldPattern.Connect(new FilerObject[] { beam1, beam2 }, AtomicElement.eAssemblyLocation.kOnSite);

				CreatedObjectInformation coi = new CreatedObjectInformation();
				coi.CreatedObject = weldPattern;
				coi.JointTransferId = role;
				coi.Attributes = new eObjectsAttribute[0];

				arrCOI.Add(coi);
			}
		}

		private void CreateBolts(List<CreatedObjectInformation> arrCOI, Beam beam1, Beam beam2, string role)
		{
			Point3d ptOrig1;
			Vector3d vX1, vY1, vZ1;
			beam1.PhysCSMid.GetCoordSystem(out ptOrig1, out vX1, out vY1, out vZ1);

			FinitRectScrewBoltPattern boltPattern = new FinitRectScrewBoltPattern(ptOrig1, ptOrig1, vZ1, vX1);
			boltPattern.Nx = 1;
			boltPattern.Ny = 1;
			boltPattern.Standard = _boltType;
			boltPattern.Grade = _boltMaterial;
			boltPattern.BoltAssembly = _boltSet;
			boltPattern.ScrewDiameter = _boltDiameter;

			if (boltPattern != null)
			{
				boltPattern.WriteToDb();
				boltPattern.Connect(new FilerObject[] { beam1, beam2 }, AtomicElement.eAssemblyLocation.kOnSite);

				CreatedObjectInformation coi = new CreatedObjectInformation();
				coi.CreatedObject = boltPattern;
				coi.JointTransferId = role;
				coi.Attributes = new eObjectsAttribute[0];

				arrCOI.Add(coi);
			}
		}

		private void CreateAnchors(List<CreatedObjectInformation> arrCOI, Beam beam1, Beam beam2, string role)
		{
			Point3d ptOrig1;
			Vector3d vX1, vY1, vZ1;
			beam1.PhysCSMid.GetCoordSystem(out ptOrig1, out vX1, out vY1, out vZ1);

			AnchorPattern anchorPattern = new AnchorPattern(ptOrig1, ptOrig1, vZ1, vX1);
			anchorPattern.Nx = 1;
			anchorPattern.Ny = 1;
			anchorPattern.Standard = _anchorType;

			anchorPattern.Grade = _anchorMaterial;
			anchorPattern.BoltAssembly = _anchorSet;
			anchorPattern.ScrewDiameter = _anchorDiameter;
			anchorPattern.ScrewLength = _anchorLength;
			anchorPattern.OrientationType = AnchorPattern.eOrientationType.kAllOutside;

			if (anchorPattern != null)
			{
				anchorPattern.WriteToDb();
				anchorPattern.Connect(new FilerObject[] { beam1, beam2 }, AtomicElement.eAssemblyLocation.kOnSite);

				CreatedObjectInformation coi = new CreatedObjectInformation();
				coi.CreatedObject = anchorPattern;
				coi.JointTransferId = role;
				coi.Attributes = new eObjectsAttribute[0];

				arrCOI.Add(coi);
			}
		}

		private void CreateShearStuds(List<CreatedObjectInformation> arrCOI, Plate plate, string role)
		{
			Connector shearStudPattern = new Connector();
			shearStudPattern.SetCS(plate.CS);

			shearStudPattern.Arranger = new Autodesk.AdvanceSteel.Arrangement.BoundedRectArranger(10, 20);
			shearStudPattern.Arranger.Nx = 2;
			shearStudPattern.Arranger.Ny = 2;
			shearStudPattern.Arranger.Dx = 3 * plate.Thickness;
			shearStudPattern.Arranger.Dy = 3 * plate.Thickness;

			shearStudPattern.Standard = _studType;
			shearStudPattern.Material = _studMaterial;
			shearStudPattern.Diameter = _studDiameter;
			shearStudPattern.Length = _studLength;

			WeldPoint wl = shearStudPattern.Connect(plate, plate.CS);

			if (shearStudPattern != null)
			{
				shearStudPattern.WriteToDb();
				CreatedObjectInformation coi = new CreatedObjectInformation();
				coi.CreatedObject = shearStudPattern;
				coi.JointTransferId = role;
				coi.Attributes = new eObjectsAttribute[0];

				arrCOI.Add(coi);
			}


			if (wl != null)
			{				
				CreatedObjectInformation coi = new CreatedObjectInformation();
				coi.CreatedObject = wl;
				coi.JointTransferId = role;
				coi.Attributes = new eObjectsAttribute[0];

				arrCOI.Add(coi);
			}
		}
	}
}
