using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Common;
using System.Drawing;
using Autodesk.Revit;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Structure;
using Microsoft.Office.Interop.Excel;

namespace DevCamp_Massing
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
        public class PointsParabola : IExternalCommand
        {
        static AddInId m_appId = new AddInId(new Guid("CF184371-265B-4a35-BF7F-2959CA87A42D"));
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;
                double yctr = 0;
                XYZ xyz = null;
                ReferencePoint rp = null;
                double power = 1.2;
                while (power < 1.5)
                {
                    double xctr = 0;
                    double zctr = 0;
                    while (zctr < 100)
                    {
                        zctr = Math.Pow(xctr, power);
                        xyz = app.Create.NewXYZ(xctr, yctr, zctr);
                        rp = doc.FamilyCreate.NewReferencePoint(xyz);
                        if (xctr > 0)
                        {
                            xyz = app.Create.NewXYZ(-xctr, yctr, zctr);
                            rp = doc.FamilyCreate.NewReferencePoint(xyz);
                        }
                        xctr++;
                    }
                    power = power + 0.1;
                    yctr = yctr + 50;
                    zctr = 0;
                }
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
        public class PointsOnCurve : IExternalCommand
        {
        static AddInId m_appId = new AddInId(new Guid("CF181371-E65B-4a35-BF7F-2959CA87A42D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;

                XYZ start = app.Create.NewXYZ(0, 0, 0);
                XYZ end = app.Create.NewXYZ(50, 50, 0);
                Autodesk.Revit.DB.Line line = app.Create.NewLine(start, end, true);
                Plane geometryPlane = app.Create.NewPlane(XYZ.BasisZ, start);
                SketchPlane skplane = doc.FamilyCreate.NewSketchPlane(geometryPlane);
                ModelCurve modelcurve = doc.FamilyCreate.NewModelCurve(line, skplane);

                for (double i = 0; i <= 1; i = i + 0.1)
                {
                    PointOnEdge poe = app.Create.NewPointOnEdge(modelcurve.GeometryCurve.Reference, i);
                    ReferencePoint rp2 = doc.FamilyCreate.NewReferencePoint(poe);
                }
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class PointsFromExcel : IExternalCommand
        {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E65B-4a35-BF7F-2959CA87142D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;

                Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                string excelFile = @"c:\DevCamp\helix.xlsx";
                Workbook workbook = excelApp.Workbooks.Open(excelFile,
                   Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                   Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                   Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                   Type.Missing, Type.Missing);

                Worksheet sheet = (Worksheet)workbook.Sheets[1];
                Range excelRange = sheet.UsedRange; object[,] valueArray = (object[,])excelRange.get_Value(XlRangeValueDataType.xlRangeValueDefault);
                for (int i = 1; i <= excelRange.Rows.Count; i++)
                {
                    XYZ xyz = new XYZ(Convert.ToDouble(valueArray[i, 1]), Convert.ToDouble(valueArray[i, 2]), Convert.ToDouble(valueArray[i, 3]));
                    ReferencePoint rp = doc.FamilyCreate.NewReferencePoint(xyz);
                }
                workbook.Close(false, excelFile, null);
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class PointsFromTextFile : IExternalCommand
        {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E65B-4a35-B37F-2959CA87A42D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;
                string filename = @"c:\DevCamp\sphere.csv";
                if (File.Exists(filename))
                {
                    StreamReader readFile = new StreamReader(filename);
                    string line;
                    while ((line = readFile.ReadLine()) != null)
                    {
                        string[] data = line.Split(',');
                        XYZ xyz = app.Create.NewXYZ(Convert.ToDouble(data[0]), Convert.ToDouble(data[1]), Convert.ToDouble(data[2]));
                        ReferencePoint rp = doc.FamilyCreate.NewReferencePoint(xyz);
                    }
                }
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SineCurve : IExternalCommand
        {
        static AddInId m_appId = new AddInId(new Guid("CF124371-E65B-4a35-BF7F-2959CA87A42D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;
                int pnt_ctr = 0;
                double xctr = 0;
                XYZ xyz = new XYZ();
                ReferencePointArray rparray = new ReferencePointArray();
                while (pnt_ctr < 500)
                {
                    xyz = app.Create.NewXYZ(xctr, 0, (Math.Cos(xctr)) * 10);
                    ReferencePoint rp = doc.FamilyCreate.NewReferencePoint(xyz);
                    rparray.Append(rp);
                    xctr = xctr + 0.1;
                    pnt_ctr++;
                }
                CurveByPoints curve = doc.FamilyCreate.NewCurveByPoints(rparray);

                MyUtilities.Reference_Point_Visibility_Off(doc);

                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CappedFormFromRoomShell : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF182371-E65B-4a35-BF7F-2959CA87A42D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;

                Transaction t = new Transaction(doc);
                t.SetName("Transaction");
                t.Start();

                string templateFile = @"C:\DevCamp\Mass.rft";
                Document famDoc = app.NewFamilyDocument(templateFile);

                Transaction familyT = new Transaction(famDoc);
                familyT.SetName("Transaction in Family");
                familyT.Start();

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> collection = collector.OfClass(typeof(Enclosure)).ToElements();
                int ctr = 1;
                int intBlue = 200;
                int intRed = 250;
                int intGreen = 200;

                foreach (Element e in collection)
                {
                    Room room = e as Room;

                    Categories cats = famDoc.Settings.Categories;
                    Category catMassing = cats.get_Item("Mass");
                    Category subcategory = famDoc.Settings.Categories.NewSubcategory(catMassing, "subcat" + ctr);
                    Material material = famDoc.Settings.Materials.AddWood("Material" + ctr);

                    int increment = 40;
                    if (intBlue > increment)
                    {
                        intBlue = intBlue - increment;
                    }
                    else if (intRed > increment)
                    {
                        intRed = intRed - increment;
                    }
                    else
                    {
                        intGreen = intGreen - increment;
                    }

                    material.Color = new Autodesk.Revit.DB.Color((byte)intRed, (byte)intGreen, (byte)intBlue);
                    subcategory.Material = material;

                    GeometryElement shell = room.ClosedShell;
                    foreach (GeometryObject geomObj in shell.Objects)
                    {
                        Solid solid = geomObj as Solid;
                        foreach (Face face in solid.Faces)
                        {
                            Mesh mesh = face.Triangulate();
                            for (int i = 0; i < mesh.NumTriangles; i++)
                            {
                                MeshTriangle triangle = mesh.get_Triangle(i);
                                XYZ vertex1 = triangle.get_Vertex(0);
                                XYZ vertex2 = triangle.get_Vertex(1);
                                XYZ vertex3 = triangle.get_Vertex(2);

                                ReferenceArray refAr = new ReferenceArray();
                                refAr.Append(MyUtilities.Curve_From_XYZPoints(famDoc, vertex1, vertex2));
                                refAr.Append(MyUtilities.Curve_From_XYZPoints(famDoc, vertex2, vertex3));
                                refAr.Append(MyUtilities.Curve_From_XYZPoints(famDoc, vertex3, vertex1));
                                Form capForm = famDoc.FamilyCreate.NewFormByCap(true, refAr);
                                capForm.Subcategory = subcategory;
                            }
                        }
                    }
                    ctr++;
                }
                familyT.Commit();

                Family family = famDoc.LoadFamily(doc);

                FamilySymbolSetIterator familySymbolSetIterator = family.Symbols.ForwardIterator();
                familySymbolSetIterator.MoveNext();
                FamilySymbol famSymbol = familySymbolSetIterator.Current as FamilySymbol;

                FamilyInstance familyInstance = doc.Create.NewFamilyInstance(new XYZ(0, 0, 0), famSymbol, StructuralType.NonStructural);

                FilteredElementCollector viewCollector = new FilteredElementCollector(doc);
                View view = viewCollector.OfClass(typeof(View3D)).OfCategory(BuiltInCategory.OST_Views).ToElements().First() as View;

                MyUtilities.Mass_Visibility_On(doc, view);

                t.Commit();
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CyclicSurface : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E65B-4a34-BF7F-2959CA87A42D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;
                XYZ xyz = new XYZ();
                ReferenceArrayArray refArAr = new ReferenceArrayArray();
                int x = 0;
                double z = 0;
                while (x < 800)
                {
                    ReferencePointArray rpAr = new ReferencePointArray();
                    int y = 0;
                    while (y < 800)
                    {
                        z = 50 * (Math.Cos((Math.PI / 180) * x) + Math.Cos((Math.PI / 180) * y));
                        xyz = app.Create.NewXYZ(x, y, z);
                        ReferencePoint rp = doc.FamilyCreate.NewReferencePoint(xyz);
                        rpAr.Append(rp);
                        y = y + 40;
                    }
                    CurveByPoints curve = doc.FamilyCreate.NewCurveByPoints(rpAr);
                    ReferenceArray refAr = new ReferenceArray();
                    refAr.Append(curve.GeometryCurve.Reference);
                    refArAr.Append(refAr);
                    x = x + 40;
                }
                Form form = doc.FamilyCreate.NewLoftForm(true, refArAr);
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class DividedSurfaceCreation : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E65B-4a35-BF7F-2959CA86A42D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> collection = collector.OfClass(typeof(Form)).ToElements();

                Options opt = app.Create.NewGeometryOptions();
                opt.ComputeReferences = true;
                foreach (Element e in collection)
                {
                    Form form = e as Form;
                    GeometryElement geomElem = form.get_Geometry(opt); ;
                    foreach (GeometryObject geomObject in geomElem.Objects)
                    {
                        Solid s = geomObject as Solid;
                        if (s != null)
                        {
                            foreach (Face face in s.Faces)
                            {
                                DividedSurface ds = doc.FamilyCreate.NewDividedSurface(face.Reference);
                                SpacingRule spacingRuleU = ds.USpacingRule;
                                spacingRuleU.SetLayoutFixedDistance(80, SpacingRuleJustification.Center, 0, 0);
                                SpacingRule spacingRuleV = ds.VSpacingRule;
                                spacingRuleV.SetLayoutFixedDistance(40, SpacingRuleJustification.Center, 0, 0);
                            }
                        }
                    }
                }
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class TilePatternIteration : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E65B-4a35-BF7F-2959CA87A12D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;
                UIDocument uidoc = commandData.Application.ActiveUIDocument;

                TilePatterns tilepatterns = doc.Settings.TilePatterns;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> collection = collector.OfClass(typeof(Form)).ToElements();

                Options opt = app.Create.NewGeometryOptions();
                opt.ComputeReferences = true;

                ReferenceArray dsRefs = new ReferenceArray();
                DividedSurfaceData dsData = null;
                Form form = null;
                foreach (Element e in collection)
                {
                    form = e as Form;
                    dsData = form.GetDividedSurfaceData();
                    foreach (Reference r in dsData.GetReferencesWithDividedSurfaces())
                    {
                        dsRefs.Append(r);
                    }
                }
                Transaction transaction = new Transaction(doc);
;                foreach (TilePatternsBuiltIn TilePatternEnum in Enum.GetValues(typeof(TilePatternsBuiltIn)))
                {
                    foreach (Reference reference in dsRefs)
                    {
                        form = reference.Element as Form;
                        dsData = form.GetDividedSurfaceData();
                        DividedSurface ds = dsData.GetDividedSurfaceForReference(reference);
                        transaction.SetName(TilePatternEnum.ToString());
                        transaction.Start();
                        ds.ChangeTypeId(tilepatterns.GetTilePattern(TilePatternEnum).Id);
                        transaction.Commit();
                        uidoc.RefreshActiveView();
                    }
                }
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SetDistanceParam : IExternalCommand
        {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E653-4a35-BF7F-2959CA87A42D"));
            Autodesk.Revit.ApplicationServices.Application app;
            Document doc;

            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                app = commandData.Application.Application;
                doc = commandData.Application.ActiveUIDocument.Document;

                Autodesk.Revit.DB.Parameter param = null;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> collection = collector.OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_Mass).ToElements();
                var sphere = from element in collection where element.Name == "Sphere" select element;
                FamilyInstance sphereInstance = sphere.First() as FamilyInstance;
                LocationPoint targetLocation = sphereInstance.Location as LocationPoint;
                XYZ targetPoint = targetLocation.Point;
                // get all the divided surfaces in the Revit document

                FilteredElementCollector dsCollector = new FilteredElementCollector(doc);
                ICollection<Element> dsCollection = dsCollector.OfClass(typeof(DividedSurface)).ToElements();
                foreach (Element e in dsCollection)
                {
                    DividedSurface ds = e as DividedSurface;
                    GridNode gn = new GridNode();
                    int u = 0;
                    while (u < ds.NumberOfUGridlines)
                    {
                        gn.UIndex = u;
                        int v = 0;
                        while (v < ds.NumberOfVGridlines)
                        {
                            gn.VIndex = v;
                            if (ds.IsSeedNode(gn))
                            {
                                FamilyInstance familyinstance = ds.GetTileFamilyInstance(gn, 0);
                                if (familyinstance != null)
                                {
                                    param = familyinstance.get_Parameter("Distance");
                                    if (param == null) throw new Exception("Panel family must have a Distance instance parameter");
                                    else
                                    {
                                        Autodesk.Revit.DB.Point geomobjPoint = ds.GetGridNodeReference(gn).GeometryObject as Autodesk.Revit.DB.Point;
                                        XYZ panelPoint = geomobjPoint.Coord;
                                        double d = Math.Sqrt(Math.Pow((targetPoint.X - panelPoint.X), 2) + Math.Pow((targetPoint.Y - panelPoint.Y), 2) + Math.Pow((targetPoint.Z - panelPoint.Z), 2));
                                        param.Set(d);

                                        // uncomment the following lines to create points and lines showing where the distance measurement is made
                                        //ReferencePoint rp = doc.FamilyCreate.NewReferencePoint(panelPoint);
                                        //Line line = app.Create.NewLine(targetPoint, panelPoint, true);
                                        //Plane plane = app.Create.NewPlane(targetPoint.Cross(panelPoint), panelPoint);
                                        //SketchPlane skplane = doc.FamilyCreate.NewSketchPlane(plane);
                                        //ModelCurve modelcurve = doc.FamilyCreate.NewModelCurve(line, skplane);
                                    }
                                }
                            }
                            v = v + 1;
                        }
                        u = u + 1;
                    }
                }
                return Result.Succeeded;
            }

            /// <summary>
            /// Get the XYZ point of the selected target element
            /// </summary>
            /// <param name="collection">Selected elements</param>
            /// <returns>the XYZ point of the selected target element</returns>
            XYZ getTargetPoint(ElementSet collection)
            {
                FamilyInstance targetElement = null;
                if (collection.Size != 1)
                {
                    throw new Exception("You must select one component from which the distance to panels will be measured");
                }
                else
                {
                    foreach (Element e in collection)
                    {
                        targetElement = e as FamilyInstance;
                    }
                }

                if (null == targetElement)
                {
                    throw new Exception("You must select one family instance from which the distance to panels will be measured");
                }
                LocationPoint targetLocation = targetElement.Location as LocationPoint;
                return targetLocation.Point;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SetParamFromImage : IExternalCommand
        {
        static AddInId m_appId = new AddInId(new Guid("CF184361-E65B-4a35-BF7F-2959CA87A42D"));
            Autodesk.Revit.ApplicationServices.Application app;
            Document doc;
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                app = commandData.Application.Application;
                doc = commandData.Application.ActiveUIDocument.Document;
                Autodesk.Revit.DB.Parameter param = null;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> dsCollection = collector.OfClass(typeof(DividedSurface)).ToElements();

                Bitmap image = new Bitmap(doc.PathName + "_grayscale.bmp");
                foreach (DividedSurface ds in dsCollection)
                {
                    GridNode gn = new GridNode();
                    for (int u = 0; u < ds.NumberOfUGridlines; u++)
                    {
                        gn.UIndex = u;
                        for (int v = 0; v < ds.NumberOfVGridlines; v++)
                        {
                            gn.VIndex = v;
                            if (ds.IsSeedNode(gn))
                            {
                                FamilyInstance familyinstance = ds.GetTileFamilyInstance(gn, 0);
                                if (familyinstance != null)
                                {
                                    param = familyinstance.get_Parameter("Grayscale");
                                    if (param == null) throw new Exception("Panel family must have a Grayscale instance parameter");
                                    else
                                    {
                                        System.Drawing.Color pixelColor = new System.Drawing.Color();
                                        try
                                        {
                                            pixelColor = image.GetPixel(image.Width - v, image.Height - u);
                                            double grayscale = 255 - ((pixelColor.R + pixelColor.G + pixelColor.B) / 3);
                                            if (grayscale == 0)
                                            {
                                                doc.Delete(familyinstance);
                                            }
                                            else
                                            {
                                                param.Set(grayscale / 255);
                                            }
                                        }
                                        catch (System.Exception)
                                        {
                                            //       MessageBox.Show("Exception: " + u + ", " + v);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return Result.Succeeded;
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class AlternateStripesOfTiles : IExternalCommand
        {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E65B-4a35-BF7F-1959CA87A42D"));
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                ExternalCommandData cdata = commandData;
                Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
                Document doc = commandData.Application.ActiveUIDocument.Document;

                FilteredElementCollector collector = new FilteredElementCollector(doc);
                ICollection<Element> collection = collector.OfClass(typeof(Form)).ToElements();

                Options opt = app.Create.NewGeometryOptions();
                ReferenceArray dsRefs = new ReferenceArray();
                DividedSurfaceData dsData = null;
                opt.ComputeReferences = true;
                Form form = null;
                foreach (Element e in collection)
                {
                    form = e as Form;
                    dsData = form.GetDividedSurfaceData();
                    if (dsData != null)
                    {
                        foreach (Reference r in dsData.GetReferencesWithDividedSurfaces())
                        {
                            dsRefs.Append(r);
                        }
                    }
                }
                foreach (Reference reference in dsRefs)
                {
                    form = reference.Element as Form;
                    dsData = form.GetDividedSurfaceData();
                    DividedSurface ds = dsData.GetDividedSurfaceForReference(reference);
                    FamilySymbol fs = doc.get_Element(ds.GetTypeId()) as FamilySymbol;
                    FamilySymbol fs_glazed = null;

                    foreach (FamilySymbol symbol in fs.Family.Symbols)
                    {
                        if (symbol.Name == "Glazed")
                        {
                            fs_glazed = symbol;
                        }
                    }
                    GridNode gn = new GridNode();
                    int u = 0;
                    while (u < ds.NumberOfUGridlines)
                    {
                        gn.UIndex = u;
                        int v = 0;
                        while (v < ds.NumberOfVGridlines)
                        {
                            gn.VIndex = v;
                            FamilyInstance fi = ds.GetTileFamilyInstance(gn, 0);
                            if (fi != null)
                            {
                                if (Math.Abs(gn.UIndex - gn.VIndex) % 2 == 0)
                                {
                                    fi.Symbol = fs_glazed;
                                }
                            }
                            v = v + 1;
                        }
                        u = u + 1;
                    }
                }
                return Result.Succeeded;
            }
        }

        public class MyUtilities
        {
            public static void Reference_Point_Visibility_Off(Document doc)
            {
                Category category = doc.Settings.Categories.get_Item("Reference Points");
                category.set_Visible(doc.ActiveView, false);
            }

            public static Reference Curve_From_XYZPoints(Document doc, XYZ vertex1, XYZ vertex2)
            {
                ReferencePoint p1 = doc.FamilyCreate.NewReferencePoint(vertex1);
                ReferencePoint p2 = doc.FamilyCreate.NewReferencePoint(vertex2);
                ReferencePointArray refPointAr = new ReferencePointArray();
                refPointAr.Append(p1);
                refPointAr.Append(p2);
                CurveByPoints curve = doc.FamilyCreate.NewCurveByPoints(refPointAr);
                return curve.GeometryCurve.Reference;
            }

            public static void Mass_Visibility_On(Document doc, View view)
            {
                Category category = doc.Settings.Categories.get_Item("Mass");
                category.set_Visible(view, true);
            }
        }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ContextCreator : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("C2181371-E65B-4a35-BF7F-2959CA87A42D"));
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document famDoc = null;
            string massTemplatePath = @"C:\DevCamp\Mass.rft";
            try
            {
                famDoc = app.NewFamilyDocument(massTemplatePath);
            }
            catch
            {
                TaskDialog.Show("Error", massTemplatePath + " does not exist");
                return Result.Failed;
            }
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> elementList = new List<Element>();
            elementList = collector.OfClass(typeof(Level)).OfCategory(BuiltInCategory.OST_Levels).ToElements();
            Transaction transaction = new Transaction(famDoc);
            transaction.SetName("Level creation");
            transaction.Start();
            foreach (Element e in elementList)
            {
                Level l = e as Level;
                Level famLevel = famDoc.FamilyCreate.NewLevel(l.Elevation);
                try // Revit will throw exception if level name would be a duplicate
                {
                    famLevel.Name = l.Name;
                }
                catch (System.Exception)
                {
                }
                ViewPlan viewPlan = famDoc.FamilyCreate.NewViewPlan(famLevel.Name, famLevel, ViewPlanType.FloorPlan);
            }
            transaction.Commit();

            transaction.SetName("Create Subcategory");
            transaction.Start();
            Category contextCat = famDoc.Settings.Categories.NewSubcategory(famDoc.OwnerFamily.FamilyCategory, "Context");
            transaction.Commit();

            IList<Reference>faceRefs = new List<Reference>();
            faceRefs = uidoc.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Face,"Select faces to bring into the family");

            transaction.SetName("Geometry creation");
            transaction.Start();

            foreach (Reference r in faceRefs)
            {
                Face f = r.GeometryObject as Face;
                foreach (EdgeArray edgeArray in f.EdgeLoops)
                {
                    ReferenceArray refArray = new ReferenceArray();
                    foreach (Edge edge in edgeArray)
                    {
                        ReferencePointArray array = new ReferencePointArray();
                        XYZ lastEdgeNormal = new XYZ(999,999,999);
                        for (double param = 0; param <= 1; param = param + 0.2 )
                        {
                            XYZ edgeNormal = edge.ComputeDerivatives(param).BasisY;
                            if (!(edgeNormal.IsAlmostEqualTo(lastEdgeNormal))) // is the edge between this point and the last point straight?
                            {
                                ReferencePoint rp = famDoc.FamilyCreate.NewReferencePoint(edge.Evaluate(param));
                                array.Append(rp);
                            }
                            lastEdgeNormal = edgeNormal;
                        }
                        if (array.Size == 1) // edge is straight so add the last point
                        {
                            ReferencePoint rp = famDoc.FamilyCreate.NewReferencePoint(edge.Evaluate(1));
                            array.Append(rp);
                        }
                        CurveByPoints cbp = famDoc.FamilyCreate.NewCurveByPoints(array);
                        refArray.Append(cbp.GeometryCurve.Reference);
                        array.Clear();
                        cbp.Subcategory = contextCat.GetGraphicsStyle(GraphicsStyleType.Projection);
                    }
                    Form capForm = famDoc.FamilyCreate.NewFormByCap(true, refArray);
                    refArray.Clear();
                 }
            }
            transaction.Commit();
            
            famDoc.SaveAs(@"C:\DevCamp\MassWithContext.rfa");
            doc.Close();
            famDoc.Close();
            return Result.Succeeded;
        }
    }
    
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ParameterRandomizer_Basic : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF184391-E65B-4a35-BF7F-2959CA87A42D"));
        FamilyInstance instance = null;
        StreamWriter writer = null;
        Transaction t = null;
        ImageExportOptions imageOptions = null;
        Document doc = null;
        UIDocument uidoc = null;
        UIApplication uiApp = null;
        List<KeyValuePair<string, double>> paramValueList = null;
        Random random = new Random(DateTime.Now.Millisecond);
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            uiApp = new UIApplication(app);
            doc = commandData.Application.ActiveUIDocument.Document;
            uidoc = commandData.Application.ActiveUIDocument;
            imageOptions = new ImageExportOptions();
            imageOptions.PixelSize = 175;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            try
            {
                instance = collector.OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_Mass).ToElements().First() as FamilyInstance;
            }
            catch (System.Exception)
            {
                TaskDialog.Show("Error", "No mass instance found.");
                return Result.Cancelled;            	
            }

            paramValueList = new List<KeyValuePair<string,double>>();
            List<string> paramList = new List<string>();
            foreach (Autodesk.Revit.DB.Parameter p in instance.Parameters)
            {
                if (p.Definition.ParameterGroup == BuiltInParameterGroup.PG_DATA)
                {
                    paramList.Add(p.Definition.Name);
                    paramValueList.Add(new KeyValuePair<string,double>(p.Definition.Name, p.AsDouble()));
                }
            }
            if (paramList.Count == 0)
            {
                TaskDialog.Show("Error", "The mass instance has no parameters in the 'Data' parameter group.");
                return Result.Cancelled;
            }

            t = new Transaction(doc);
            t.SetName("transaction");
            t.Start();
            doc.ActiveView.setVisibility(doc.Settings.Categories.get_Item("Mass"), true);
            t.Commit();
              
            string htmlFile = @"C:\DevCamp\images.html";
            writer = new StreamWriter(htmlFile);
            writer.WriteLine("<html><body><table>");

            for (int ctr = 0; ctr < 12; ctr++ )
            {
                FailureHandlingOptions failOpt = t.GetFailureHandlingOptions();
                failOpt.SetFailuresPreprocessor(new RollbackErrors());

                if (ctr % 6 == 0) writer.WriteLine("<tr>");
                imageOptions.FilePath = @"c:\DevCamp\img_" + ctr + ".jpg";
                writer.WriteLine("<td valign='bottom'><img src='" + imageOptions.FilePath + "' border=1><br><font size=-1>");
                t.Start();
                t.SetName("Randomizer Iteration " + ctr);
                foreach (KeyValuePair<string, double> thisPair in paramValueList)
                {
                    string s = thisPair.Key;
                    Autodesk.Revit.DB.Parameter pp = instance.get_Parameter(s);
                    pp.Set(thisPair.Value);
                    SetParam(thisPair);
                    writer.WriteLine(s + "=" + SetParam(thisPair) + "<br>");
                }
                t.SetFailureHandlingOptions(failOpt);
                t.Commit();
                uidoc.RefreshActiveView();
                doc.ExportImage(imageOptions);
            }
            writer.WriteLine("</body></html>");
            writer.Close();
            System.Diagnostics.Process.Start(htmlFile);
            return Result.Succeeded;
        }

        public double SetParam(KeyValuePair<string, double> thisPair)
        {
            Autodesk.Revit.DB.Parameter p = instance.get_Parameter(thisPair.Key);
            double valOld = thisPair.Value;
            double newVal = Math.Round(valOld * (random.NextDouble() + 0.5), 3);
            bool result = p.Set(newVal);
            return newVal;
        }
        public class RollbackErrors : IFailuresPreprocessor
        {
            public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                if (failuresAccessor.GetFailureMessages().Count > 0)
                {
                    failuresAccessor.RollBackPendingTransaction();
                    return FailureProcessingResult.ProceedWithRollBack;
                }
                else
                {
                    return FailureProcessingResult.Continue;
                }
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class ParameterRandomizer_with_Solar : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF184291-E65B-4a35-BF7F-2959CA87A42D"));
        FamilyInstance instance = null;
        int ctr = 0;
        StreamWriter writer = null;
        Transaction t = null;
        ImageExportOptions imageOptions = null;
        Document doc = null;
        UIDocument uidoc = null;
        UIApplication uiApp = null;
        string htmlFile = "";
        DateTime time = DateTime.Now;
        List<KeyValuePair<string, double>> paramValueList = null;
        bool updateParameters = true;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            doc = commandData.Application.ActiveUIDocument.Document;
            uiApp = new UIApplication(app);
            uidoc = commandData.Application.ActiveUIDocument;
            imageOptions = new ImageExportOptions();
            imageOptions.PixelSize = 175;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            try
            {
                instance = collector.OfClass(typeof(FamilyInstance)).OfCategory(BuiltInCategory.OST_Mass).ToElements().First() as FamilyInstance;
            }
            catch (System.Exception)
            {
                TaskDialog.Show("Error", "No mass instance found.");
                return Result.Cancelled;
            }

            paramValueList = new List<KeyValuePair<string, double>>();
            List<string> paramList = new List<string>();
            foreach (Autodesk.Revit.DB.Parameter p in instance.Parameters)
            {
                if (p.Definition.ParameterGroup == BuiltInParameterGroup.PG_DATA)
                {
                    paramList.Add(p.Definition.Name);
                    paramValueList.Add(new KeyValuePair<string, double>(p.Definition.Name, p.AsDouble()));
                }
            }
            if (paramList.Count == 0)
            {
                TaskDialog.Show("Error", "The mass instance has no parameters in the 'Data' parameter group.");
                return Result.Cancelled;
            }

            t = new Transaction(doc);
            t.SetName("transaction");
            t.Start();
            doc.ActiveView.setVisibility(doc.Settings.Categories.get_Item("Mass"), true);
            t.Commit();

            htmlFile = Path.GetTempPath() + "\\images.html";
            writer = new StreamWriter(htmlFile);

            uiApp.Idling += new EventHandler<IdlingEventArgs>(idleUpdate);

            return Result.Succeeded;
        }
        public void idleUpdate(object sender, IdlingEventArgs e)
        {
            if (ctr < 5)
            {
                if (ctr == 0)
                {
                    writer.WriteLine("<html><body><table>");
                }
                if (updateParameters)
                {
                    FailureHandlingOptions failOpt = t.GetFailureHandlingOptions();
                    failOpt.SetFailuresPreprocessor(new RollbackErrors());

                    if (ctr % 8 == 0) writer.WriteLine("<tr>");
                    imageOptions.FilePath = Path.GetTempPath() + "\\img_" + ctr + ".jpg";
                    writer.WriteLine("<td valign='bottom'><img src='" + imageOptions.FilePath + "' border=1><br><font size=-1>");
                    t.Start();
                    t.SetName("Randomizer Iteration " + ctr);
                    foreach (KeyValuePair<string, double> thisPair in paramValueList)
                    {
                        string s = thisPair.Key;
                        Autodesk.Revit.DB.Parameter pp = instance.get_Parameter(s);
                        pp.Set(thisPair.Value);
                        SetParam(thisPair);
                        writer.WriteLine(s + "=" + SetParam(thisPair) + "<br>");
                    }
                    t.SetFailureHandlingOptions(failOpt);
                    t.Commit();
                    time = DateTime.Now;
                    updateParameters = false;
                }
                else
                {
                    if (DateTime.Now.CompareTo(time.AddSeconds(1)) > 0)
                    {
                        uidoc.RefreshActiveView();
                        doc.ExportImage(imageOptions);
                        ctr++;
                        updateParameters = true;
                    }
                }
            }
            else
            {
                writer.WriteLine("</body></html>");
                writer.Close();
                System.Diagnostics.Process.Start(htmlFile);
                uiApp.Idling -= idleUpdate;
            }
        }
        public double SetParam(KeyValuePair<string, double> thisPair)
        {
            Autodesk.Revit.DB.Parameter p = instance.get_Parameter(thisPair.Key);
            double valOld = thisPair.Value;
            Random random = new Random((int)valOld * DateTime.Now.Millisecond);
            double newVal = Math.Round(valOld * (random.NextDouble() + 0.5), 3);
            bool result = p.Set(newVal);
            return newVal;
        }
        public class RollbackErrors : IFailuresPreprocessor
        {
            public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                IList<FailureMessageAccessor> failList = failuresAccessor.GetFailureMessages();
                if (failuresAccessor.GetFailureMessages(FailureSeverity.Error).Count > 0)
                {
                    return FailureProcessingResult.ProceedWithRollBack;
                }
                else
                {
                    return FailureProcessingResult.Continue;
                }
            }
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class RefPointOffset : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E65B-4435-BF7F-2959CA87A42D"));
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            IList<Element> pointList = new List<Element>();
            IList<ReferencePoint> newPointList = new List<ReferencePoint>();
            pointList = collector.OfClass(typeof(ReferencePoint)).ToElements();

            FamilyParameter offsetFamilyParameter = doc.FamilyManager.AddParameter("PointOffset", BuiltInParameterGroup.PG_DATA,ParameterType.Length, false);
            FamilyType newType = doc.FamilyManager.NewType("Offset 2");
            doc.FamilyManager.Set(offsetFamilyParameter, 2);
            newType = doc.FamilyManager.NewType("Offset 4");
            doc.FamilyManager.Set(offsetFamilyParameter, 4);
            newType = doc.FamilyManager.NewType("Offset 6");
            doc.FamilyManager.Set(offsetFamilyParameter, 6);

            foreach (Element e in pointList)
            {
                ReferencePoint refPoint = e as ReferencePoint;
                Reference planeXY = refPoint.GetCoordinatePlaneReferenceXY();
                PointOnPlane pointOnPlane = app.Create.NewPointOnPlane(planeXY,new UV(0,0),new UV(),5);
                ReferencePoint refPointNew = doc.FamilyCreate.NewReferencePoint(pointOnPlane);
                Autodesk.Revit.DB.Parameter offset = refPointNew.get_Parameter("Offset");
                doc.FamilyManager.AssociateElementParameterToFamilyParameter(offset, offsetFamilyParameter);
                newPointList.Add(refPointNew);
            }
            ReferencePointArray refPointArray = new ReferencePointArray();
            CurveByPoints curve = null;
            foreach (ReferencePoint point in newPointList)
            {
                refPointArray.Append(point);
                if (refPointArray.Size == 2)
                {
                    curve = doc.FamilyCreate.NewCurveByPoints(refPointArray);
                    refPointArray.Clear();
                    refPointArray.Append(point);
                }
            }
            refPointArray.Append(newPointList.First());
            curve = doc.FamilyCreate.NewCurveByPoints(refPointArray);
            return Result.Succeeded;
        }
    }
}
