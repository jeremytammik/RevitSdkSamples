using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.Analysis;

namespace DevCamp_AVF
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SpatialFieldXYZ_OnePoint : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("C3183411-E65B-4a35-BF7F-2959CA87A42D"));
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            SpatialFieldManager sfm = SpatialFieldManager.CreateSpatialFieldManager(doc.ActiveView, 1);
            int primitiveIndex = sfm.AddSpatialFieldPrimitive();

            IList<XYZ> pts = new List<XYZ>();
            pts.Add(new XYZ(0, 0, 0));

            FieldDomainPointsByXYZ pnts = new FieldDomainPointsByXYZ(pts);

            List<double> doubleList = new List<double>();
            doubleList.Add(0);
            IList<ValueAtPoint> valueList = new List<ValueAtPoint>();

            valueList.Add(new ValueAtPoint(doubleList));
            FieldValues fieldValues = new FieldValues(valueList);

            sfm.UpdateSpatialFieldPrimitive(primitiveIndex, pnts, fieldValues);
            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class CreateSetAnalysisDisplayStyle : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF284371-E65B-4a35-BF7F-2959CA87A42D"));
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            AnalysisDisplayMarkersAndTextSettings markerSettings = new AnalysisDisplayMarkersAndTextSettings();
            markerSettings.MarkerSize = .05;
            markerSettings.MarkerType = AnalysisDisplayStyleMarkerType.Triangle;
            markerSettings.Rounding = .01;

            AnalysisDisplayColorSettings colorSettings = new AnalysisDisplayColorSettings();
            colorSettings.MaxColor = new Color(255, 0, 0);
            colorSettings.MinColor = new Color(0, 255, 255);

            AnalysisDisplayLegendSettings legendSettings = new AnalysisDisplayLegendSettings();
            legendSettings.Rounding = .01;

            AnalysisDisplayStyle ads = AnalysisDisplayStyle.CreateAnalysisDisplayStyle(doc, "Markers 1", markerSettings, colorSettings, legendSettings);
            doc.ActiveView.AnalysisDisplayStyleId = ads.Id;
            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SpatialFieldXYZ_ThreePoint : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("C3183471-E65B-4a35-BF7F-2959CA87A42D"));
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            // can't create a manager for the view if one already exists, so check for a manager before creating one
            SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(doc.ActiveView);
            if (sfm != null) sfm.Clear(); // how to clear any existing results
            else sfm = SpatialFieldManager.CreateSpatialFieldManager(doc.ActiveView, 1);

            int primitiveIndex = sfm.AddSpatialFieldPrimitive();

            IList<XYZ> pts = new List<XYZ>();
            pts.Add(new XYZ(100, 100, 100));
            pts.Add(new XYZ(0, 50, 50));
            pts.Add(new XYZ(100, 0, 25));
            FieldDomainPointsByXYZ pnts = new FieldDomainPointsByXYZ(pts);

            List<double> doubleList = new List<double>();
            IList<ValueAtPoint> valueList = new List<ValueAtPoint>();
            doubleList.Add(0.18);
            valueList.Add(new ValueAtPoint(doubleList));
            doubleList.Clear();
            doubleList.Add(2.5);
            valueList.Add(new ValueAtPoint(doubleList));
            doubleList.Clear();
            doubleList.Add(1.17);
            valueList.Add(new ValueAtPoint(doubleList));

            FieldValues fieldValues = new FieldValues(valueList);

            sfm.Description = "DevCamp AVF Sample";
            sfm.UpdateSpatialFieldPrimitive(primitiveIndex, pnts, fieldValues);

            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SpatialFieldCurve : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF184371-E64B-4a35-BF7F-2959CA87A42D"));
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(doc.ActiveView);
            if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(doc.ActiveView, 1);

            ISelectionFilter selFilter = new CurveSelectionFilter();
            Reference reference = uidoc.Selection.PickObject(ObjectType.Element, selFilter,"Select an model curve");
            ModelCurve modelCurve = reference.Element as ModelCurve;
            Curve curve = modelCurve.GeometryCurve;
            int primitiveIndex = sfm.AddSpatialFieldPrimitive(curve.Reference);

            IList<double> pts = new List<double>();
            double range = curve.get_EndParameter(1) - curve.get_EndParameter(0);
            int numOfPoints = 4;
            double u = curve.get_EndParameter(0);

            List<double> doubleList = new List<double>();
            IList<ValueAtPoint> valList = new List<ValueAtPoint>();

            for (double ii = curve.get_EndParameter(0); ii <= curve.get_EndParameter(1); ii=ii+(range/numOfPoints))
            {
                pts.Add(ii);

                doubleList.Clear();
                doubleList.Add(ii);
                valList.Add(new ValueAtPoint(doubleList));
            }
            FieldDomainPointsByParameter pnts = new FieldDomainPointsByParameter(pts);
            FieldValues fieldValues = new FieldValues(valList);

            sfm.UpdateSpatialFieldPrimitive(primitiveIndex, pnts, fieldValues);

            return Result.Succeeded;
        }
        public class CurveSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element element)
            {
                if (element is ModelCurve) return true;
                else return false;
            }
            public bool AllowReference(Reference refer, XYZ point)
            {
                return false;
            }
        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Automatic)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class SpatialFieldFaceTransform_MultiValuesPerPoint : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF184321-165B-4a38-BF7F-2959CA87A42D"));
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            View3D view3D = doc.Create.NewView3D(doc.ActiveView.ViewDirection.Negate());

            // Create Colored Surface display style 
            AnalysisDisplayColoredSurfaceSettings coloredSurfaceSettings = new AnalysisDisplayColoredSurfaceSettings();
            AnalysisDisplayColorSettings colorSettings = new AnalysisDisplayColorSettings();

            // Create "Colored Surface" analysis display style, but only if it doesn't already exist
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> collection = collector.OfClass(typeof(AnalysisDisplayStyle)).ToElements();
            var displayStyle = from element in collection where element.Name == "Colored Surface" select element;
            // If "Colored Surface" exists, use it
            if (displayStyle.Count() == 1)
            {
                view3D.AnalysisDisplayStyleId = displayStyle.First().Id;
            }
            // If "Colored Surface" does not exist, create it
            else
            {
                // ColorSettingsType specifies either SolidColorRanges or GradientColor
                colorSettings.ColorSettingsType = AnalysisDisplayStyleColorSettingsType.SolidColorRanges;
                colorSettings.MaxColor = new Color(10, 10, 10);
                colorSettings.MinColor = new Color(255, 255, 255);
                IList<AnalysisDisplayColorEntry> intermediateColorList = new List<AnalysisDisplayColorEntry>();
                intermediateColorList.Add(new AnalysisDisplayColorEntry(new Color(200, 200, 200)));
                intermediateColorList.Add(new AnalysisDisplayColorEntry(new Color(150, 150, 150)));
                intermediateColorList.Add(new AnalysisDisplayColorEntry(new Color(100, 100, 100)));
                intermediateColorList.Add(new AnalysisDisplayColorEntry(new Color(50, 50, 50))); 
                colorSettings.SetIntermediateColors(intermediateColorList);
                AnalysisDisplayLegendSettings legendSettings = new AnalysisDisplayLegendSettings();
                AnalysisDisplayStyle analysisDisplayStyle = AnalysisDisplayStyle.CreateAnalysisDisplayStyle(doc, "Colored Surface", coloredSurfaceSettings, colorSettings, legendSettings);
                view3D.AnalysisDisplayStyleId = analysisDisplayStyle.Id;
            }

            SpatialFieldManager sfm = SpatialFieldManager.CreateSpatialFieldManager(view3D, 3);

            Reference reference = uidoc.Selection.PickObject(ObjectType.Face, "Select a face");
            Face face = reference.GeometryObject as Face;

            BoundingBoxUV bb = face.GetBoundingBox();
            UV min = bb.Min;
            UV max = bb.Max;

            // offset the results data from the selected face
            UV faceCenter = new UV((max.U + min.U) / 2, (max.V + min.V) / 2); 
            Transform computeDerivatives = face.ComputeDerivatives(faceCenter);
            XYZ faceCenterNormal = computeDerivatives.BasisZ.Normalize().Multiply(5);
            Transform transform = Transform.get_Translation(faceCenterNormal);

            int idx = sfm.AddSpatialFieldPrimitive(face, transform);
            IList<UV> uvPts = new List<UV>();
            List<double> doubleList = new List<double>();
            IList<ValueAtPoint> valList = new List<ValueAtPoint>();

            for (double u = min.U; u < max.U; u += (max.U - min.U) / 5)
            {
                for (double v = min.V; v < max.V; v += (max.V - min.V) / 5)
                {
                    UV uv = new UV(u, v);
                    if (face.IsInside(uv))
                    {
                        uvPts.Add(uv);
                        doubleList.Add(u);
                        doubleList.Add(u + 15);
                        doubleList.Add(u + 30);
                        valList.Add(new ValueAtPoint(doubleList));
                        doubleList.Clear();
                    }
                }
            }

            IList<string> measureNames = new List<string>();
            measureNames.Add("January");
            measureNames.Add("April");
            measureNames.Add("July");
            sfm.SetMeasurementNames(measureNames);
            IList<string> unitNames = new List<string>();
            unitNames.Add("Feet");
            unitNames.Add("Inches");
            IList<double> multipliers = new List<double>();
            multipliers.Add(1);
            multipliers.Add(12);
            sfm.SetUnits(unitNames, multipliers);

            FieldDomainPointsByUV pnts = new FieldDomainPointsByUV(uvPts);
            FieldValues vals = new FieldValues(valList);
            sfm.UpdateSpatialFieldPrimitive(idx, pnts, vals);

            return Result.Succeeded;
        }
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class MultiThreadAVF : IExternalCommand
    {
        static AddInId m_appId = new AddInId(new Guid("CF293952-E66B-4a35-BF7F-2959CA87A42D"));
        static UpdaterId updaterID;
        static int spatialFieldId;
        static int oldSpatialFieldId;
        static string docName;

        static int oldViewInt;
        static int elementInt;
        static int activeViewInt;

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Autodesk.Revit.ApplicationServices.Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;
            docName = doc.PathName;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            UIApplication uiApp = new UIApplication(app);
            Element element = null;
            try
            {
                element = uidoc.Selection.PickObject(ObjectType.Face, "Select a face").Element;
            }
            catch (System.Exception ex)
            {
                TaskDialog.Show("Error", ex.Message.ToString());
                return Result.Cancelled;
            }

            elementInt = element.Id.IntegerValue;
            activeViewInt = doc.ActiveView.Id.IntegerValue;

            SpatialFieldManager oldSfm = null;
            ElementId oldViewId = new ElementId(oldViewInt);
            View oldView = doc.get_Element(oldViewId) as View;
            try
            {
                if (oldView != null) oldSfm = SpatialFieldManager.GetSpatialFieldManager(oldView);
                if (oldSfm != null) oldSfm.RemoveSpatialFieldPrimitive(oldSpatialFieldId);
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", ex.Message.ToString());
            }
            ThreadContainer container = CreateContainer(element);

            SpatialFieldUpdater updater = new SpatialFieldUpdater(container, uiApp.ActiveAddInId);
            if (!UpdaterRegistry.IsUpdaterRegistered(updater.GetUpdaterId())) UpdaterRegistry.RegisterUpdater(updater, doc);
            IList<ElementId> idCollection = new List<ElementId>();
            idCollection.Add(element.Id);
            UpdaterRegistry.RemoveAllTriggers(updaterID);
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), doc, idCollection, Element.GetChangeTypeGeometry());
            UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), doc, idCollection, Element.GetChangeTypeElementDeletion());

            uiApp.Idling += new EventHandler<IdlingEventArgs>(container.idleUpdate);
            Thread thread = new Thread(new ThreadStart(container.run));
            thread.Start();

            return Autodesk.Revit.UI.Result.Succeeded;
        }

        public static ThreadContainer CreateContainer(Element element)
        // find largest face on element
        // get or create spatialfieldmanager
        // add spatial field primitive for this reference
        {
            Document doc = element.Document;
            Face biggestFace = null;
            double biggestArea = 0;
            Autodesk.Revit.ApplicationServices.Application app = element.Document.Application;
            Options options = app.Create.NewGeometryOptions();
            options.ComputeReferences = true;
            GeometryElement geomElem = element.get_Geometry(options);
            foreach (GeometryObject geomObj in geomElem.Objects)
            {
                Solid solid = geomObj as Solid;
                if (solid != null)
                {
                    foreach (Face face in solid.Faces)
                    {
                        if (face.Area > biggestArea)
                        {
                            biggestFace = face;
                            biggestArea = face.Area;
                        }
                    }
                }
            }
            ElementId activeViewId = new ElementId(activeViewInt);
            View activeView = doc.get_Element(activeViewId) as View;
            SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(activeView);
            if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(activeView, 1);

            spatialFieldId = sfm.AddSpatialFieldPrimitive(biggestFace.Reference);
            BoundingBoxUV bbox = biggestFace.GetBoundingBox();

            myUV uvMin = new myUV(bbox.Min.U, bbox.Min.V);
            myUV uvMax = new myUV(bbox.Max.U, bbox.Max.V);

            return new ThreadContainer(doc.PathName, uvMin, uvMax);
        }

        public class SpatialFieldUpdater : IUpdater
        {
            ThreadContainer containerOld;
            UIApplication uiApp;
            public SpatialFieldUpdater(ThreadContainer _container, AddInId addinId)
            {
                containerOld = _container;
                updaterID = new UpdaterId(addinId, new Guid("FBF2F6B2-4C06-42d4-97C1-D1B4EB593EFF"));
            }
            public void Execute(UpdaterData data)
            {
                uiApp = new UIApplication(data.GetDocument().Application);
                uiApp.Idling -= containerOld.idleUpdate;
                containerOld.stop = true;
                Document doc = data.GetDocument();
                ElementId activeViewId = new ElementId(activeViewInt);
                View activeView = doc.get_Element(activeViewId) as View;
                SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(activeView);
                sfm.Clear();

                if (data.GetDeletedElementIds().Count > 0)
                {
                    containerOld.stop = true;
                    return;
                }

                Element modifiedElem = doc.get_Element(data.GetModifiedElementIds().First<ElementId>());

                ThreadContainer container = CreateContainer(modifiedElem);
                containerOld = container;
                uiApp.Idling += new EventHandler<IdlingEventArgs>(container.idleUpdate);
                Thread threadNew = new Thread(new ThreadStart(container.run));
                threadNew.Start();
            }
            public string GetAdditionalInformation() { return "AVF DMU Thread sample"; }
            public ChangePriority GetChangePriority() { return ChangePriority.FloorsRoofsStructuralWalls; }
            public UpdaterId GetUpdaterId() { return updaterID; }
            public string GetUpdaterName() { return "AVF DMU Thread"; }
        }

        public class ThreadContainer
        {
            public volatile bool stop = false;
            myUV min;
            myUV max;
            int uvToCalculateSize;
            int containerId;
            static int nextContainerId = 1;
            string docName;
            IList<resultData> results = new List<resultData>();
            IList<myUV> uvToCalculate = new List<myUV>();
            IList<UV> uvPts = new List<UV>();
            IList<ValueAtPoint> valList = new List<ValueAtPoint>();

            public ThreadContainer(string _docName, myUV _min, myUV _max)
            {
                docName = _docName;
                min = _min;
                max = _max;
                containerId = nextContainerId;
                nextContainerId++;
            }

            public void run()
            {
                uvToCalculate = DetermineFacePoints(min, max);
                uvToCalculateSize = uvToCalculate.Count;
                Calculate(uvToCalculate);
            }

            public void idleUpdate(object sender, IdlingEventArgs e)
            {
                Autodesk.Revit.ApplicationServices.Application app = sender as Autodesk.Revit.ApplicationServices.Application;
                UIApplication uiApp = new UIApplication(app);

                app.WriteJournalComment("test", true);

                SpatialFieldManager sfm = SpatialFieldManager.GetSpatialFieldManager(uiApp.ActiveUIDocument.Document.ActiveView);
                if (sfm == null) sfm = SpatialFieldManager.CreateSpatialFieldManager(uiApp.ActiveUIDocument.Document.ActiveView, 1);

                if (stop)
                {
                    lock (results)
                    {
                        results.Clear();
                    }
                    uiApp.Idling -= idleUpdate;
                    return;
                }

                if (uiApp.ActiveUIDocument.Document.PathName == docName) // if document closed and a different file opened, do not run
                {
                    lock (results)
                    {
                        if (results.Count == 0) return;
                        foreach (resultData rData in results)
                        {
                            uvPts.Add(new UV(rData.UV.U, rData.UV.V));
                            IList<double> doubleList = new List<double>();
                            doubleList.Add(rData.Value);
                            valList.Add(new ValueAtPoint(doubleList));
                        }
                        FieldDomainPointsByUV pntsByUV = new FieldDomainPointsByUV(uvPts);
                        FieldValues fieldValues = new FieldValues(valList);

                        Transaction t = new Transaction(uiApp.ActiveUIDocument.Document);
                        t.SetName("AVF");
                        t.Start();
                        try
                        {
                            if (!stop) sfm.UpdateSpatialFieldPrimitive(spatialFieldId, pntsByUV, fieldValues);
                        }
                        catch (System.Exception)
                        {
                            results.Clear();
                            uiApp.Idling -= idleUpdate;
                        }
                        t.Commit();
                        results.Clear();
                    }
                    if (uvToCalculateSize == 0)
                    {
                        uiApp.Idling -= idleUpdate;
                        oldViewInt = activeViewInt;
                        oldSpatialFieldId = spatialFieldId;
                    }
                }
            }

            void Calculate(IList<myUV> uvToCalculate)
            {
                foreach (myUV uv in uvToCalculate)
                {
                    if (stop)
                    {
                        uvToCalculateSize = 0;
                        return;
                    }
                    lock (results)
                    {
                        results.Add(new resultData(uv, DateTime.Now.Second));
                        Thread.Sleep(500); // to simulate the effect of a complex computation
                        uvToCalculateSize--;
                    }
                }
            }
            IList<myUV> DetermineFacePoints(myUV min, myUV max)
            {
                IList<myUV> uvList = new List<myUV>();
                int vLines = 4;
                int uLines = 4;
                for (int uctr = 0; uctr <= uLines; uctr++)
                {
                    double upnt = min.U + (double)uctr / (double)uLines * (max.U - min.U);
                    for (int vctr = 0; vctr <= vLines; vctr++)
                    {
                        uvList.Add(new myUV(upnt, min.V + (double)vctr / (double)vLines * (max.V - min.V)));
                    }
                }
                return uvList;
            }
        }

        public class myUV
        {
            public double U, V;
            public myUV(double u, double v)
            {
                U = u;
                V = v;
            }
        }

        public class resultData
        {
            public myUV UV;
            public double Value;
            public resultData(myUV uv, double value)
            {
                UV = uv;
                Value = value;
            }
        }
    }

}