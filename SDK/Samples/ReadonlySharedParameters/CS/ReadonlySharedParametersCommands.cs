//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using System.IO;

namespace Revit.SDK.Samples.ReadonlySharedParameters.CS
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SetReadonlyCost1 : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Document doc = commandData.View.Document;

            ReadonlyCostSetter.SetReadonlyCosts1(doc);

            return Result.Succeeded;
        }

        #endregion
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SetReadonlyCost2 : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Document doc = commandData.View.Document;

            ReadonlyCostSetter.SetReadonlyCosts2(doc);

            return Result.Succeeded;
        }

        #endregion
    }

    class ReadonlyCostSetter
    {

        public static void SetReadonlyCosts1(Document doc)
        {
            SetReadonlyCosts(doc, GetReadonlyCostFromId);
        }

        public static void SetReadonlyCosts2(Document doc)
        {
            SetReadonlyCosts(doc, GetReadonlyCostFromIncrements);
        }

        private static double GetReadonlyCostFromId(Element elem, int seed)
        {
            long costRoot = elem.Id.Value % 100;
            return (double)costRoot * 100.0 + 0.99;
        }

        private static double GetReadonlyCostFromIncrements(Element elem, int seed)
        {
            return (double)seed * 100.0 + 0.88;
        }

        private static void SetReadonlyCosts(Document doc, Func<Element, int, double> valueGetter)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.WhereElementIsElementType();
            FilterRule rule = ParameterFilterRuleFactory.CreateSharedParameterApplicableRule("ReadonlyCost");
            ElementParameterFilter filter = new ElementParameterFilter(rule);
            collector.WherePasses(filter);

            int increment = 1;

            using (Transaction t = new Transaction(doc, "Apply ReadonlyCost"))
            {
                t.Start();
                foreach (Element elem in collector)
                {
                    Parameter p = elem.LookupParameter("ReadonlyCost");
                    if (p != null)
                    {
                        p.Set(valueGetter(elem, increment));
                    }
                    increment++;
                }
                t.Commit();
            }
        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SetReadonlyId1 : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Document doc = commandData.View.Document;

            ReadonlyIdSetter.SetReadonlyIds1(doc);

            return Result.Succeeded;
        }

        #endregion
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class SetReadonlyId2 : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Document doc = commandData.View.Document;

            ReadonlyIdSetter.SetReadonlyIds2(doc);

            return Result.Succeeded;
        }

        #endregion
    }

    class ReadonlyIdSetter
    {
        private static string GetReadonlyIdUniqueId(Element elem)
        {
            return elem.UniqueId;
        }

        private static string GetReadonlyIdFromElementId(Element elem)
        {
            Element eType = elem.Document.GetElement(elem.GetTypeId());

            return eType.Name.Substring(0, 2) + elem.Id.ToString();
        }

        public static void SetReadonlyIds1(Document doc)
        {
            SetReadonlyIds(doc, GetReadonlyIdUniqueId);
        }

        public static void SetReadonlyIds2(Document doc)
        {
            SetReadonlyIds(doc, GetReadonlyIdFromElementId);
        }


        private static void SetReadonlyIds(Document doc, Func<Element, string> idGetter)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            FilterRule rule = ParameterFilterRuleFactory.CreateSharedParameterApplicableRule("ReadonlyId");
            ElementParameterFilter filter = new ElementParameterFilter(rule);
            collector.WherePasses(filter);

            using (Transaction t = new Transaction(doc, "Apply ReadonlyId"))
            {
                t.Start();
                foreach (Element elem in collector)
                {
                    Parameter p = elem.LookupParameter("ReadonlyId");
                    if (p != null)
                    {
                        p.Set(idGetter(elem));
                    }
                }
                t.Commit();
            }
        }

    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class BindNewReadonlySharedParametersToDocument : IExternalCommand
    {
        #region IExternalCommand Members

        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Document doc = commandData.View.Document;

            AddSetOfSharedParameters(doc);

            return Result.Succeeded;
        }

        #endregion

        private List<SharedParameterBindingManager> BuildSharedParametersToCreate()
        {
            List<SharedParameterBindingManager> sharedParametersToCreate =
                new List<SharedParameterBindingManager>();

            SharedParameterBindingManager manager = new SharedParameterBindingManager();
            manager.Name = "ReadonlyId";
            manager.Type = SpecTypeId.String.Text;
            manager.UserModifiable = false;
            manager.Description = "A read-only instance parameter used for coordination with external content.";
            manager.Instance = true;
            manager.AddCategory(BuiltInCategory.OST_Walls);
            manager.AddCategory(BuiltInCategory.OST_Floors);
            manager.AddCategory(BuiltInCategory.OST_Ceilings);
            manager.AddCategory(BuiltInCategory.OST_Roofs);
            manager.ParameterGroup = GroupTypeId.IdentityData;

            sharedParametersToCreate.Add(manager);   // Look up syntax for this automatic initialization.


            manager = new SharedParameterBindingManager();
            manager.Name = "ReadonlyCost";
            manager.Type = SpecTypeId.Currency;
            manager.UserModifiable = false;
            manager.Description = "A read-only type parameter used to list the cost of a type.";
            manager.Instance = false;

            manager.AddCategory(BuiltInCategory.OST_Furniture);
            manager.AddCategory(BuiltInCategory.OST_Planting);
            manager.ParameterGroup = GroupTypeId.Materials;

            sharedParametersToCreate.Add(manager);

            return sharedParametersToCreate;
        }

        public void AddSetOfSharedParameters(Document doc)
        {
            Application app = doc.Application;

            String filePath = GetRandomSharedParameterFileName();

            app.SharedParametersFilename = filePath;

            DefinitionFile dFile = app.OpenSharedParameterFile();
            DefinitionGroup dGroup = dFile.Groups.Create("Demo group");
            List<SharedParameterBindingManager> managers = BuildSharedParametersToCreate();
            using (Transaction t = new Transaction(doc, "Bind parameters"))
            {
                t.Start();
                foreach (SharedParameterBindingManager manager in managers)
                {
                    manager.Definition = dGroup.Definitions.Create(manager.GetCreationOptions());
                    manager.AddBindings(doc);
                }
                t.Commit();
            }
        }

   
        private String GetRandomSharedParameterFileName()
        {

            String randomFileName = System.IO.Path.GetRandomFileName();
            String fileRoot = Path.GetFileNameWithoutExtension(randomFileName);
            String spFile = Path.ChangeExtension(randomFileName, "txt");
            String filePath = Path.Combine(@"c:\tmp\Meridian\", spFile);
            StreamWriter writer = File.CreateText(filePath);
            writer.Close();
            return filePath;
        }
    }
}
