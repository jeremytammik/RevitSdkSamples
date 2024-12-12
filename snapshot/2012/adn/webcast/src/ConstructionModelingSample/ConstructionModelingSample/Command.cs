//
// (C) Copyright 2003-2011 by Autodesk, Inc.
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
using System.Text;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.ApplicationServices;

namespace ConstructionModelingSample
{
    /// <summary>
    /// Demonstrate the new Construction Modeling API 
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CreateAssemblies : IExternalCommand
    {
        #region IExternalCommand Members

        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData,
            ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            
            // Get the application and document from external command data.
            Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            ICollection<ElementId> elemIds = commandData.Application.ActiveUIDocument.Selection.GetElementIds();
            ElementId categoryId = null;
            ElementId keyElemId = null;
            foreach (ElementId elemId in elemIds)
            {
                // use category of one of the assembly elements
                keyElemId = elemId;
                categoryId = doc.get_Element(elemId).Category.Id;
                break;
            }
            Transaction transaction = new Transaction(doc);
            if (AssemblyInstance.IsValidNamingCategory(doc, categoryId, elemIds))
            {
                transaction.Start("Create Assembly Instance");
                AssemblyInstance assemblyInstance = AssemblyInstance.Create(doc, elemIds, categoryId);
                transaction.Commit(); // commit the transaction that creates the assembly instance before modifying the instance's name

                transaction.Start("Set Assembly Name");
                assemblyInstance.AssemblyTypeName = "My Assembly Name " + keyElemId.ToString();
                transaction.Commit();

                if (assemblyInstance.AllowsAssemblyViewCreation()) // create assembly views for this assembly instance
                {
                    transaction.Start("View Creation");
                    View3D view3d = AssemblyViewUtils.Create3DOrthographic(doc, assemblyInstance.Id);
                    View partList = AssemblyViewUtils.CreatePartList(doc, assemblyInstance.Id);
                    transaction.Commit();
                }
            }

            return Autodesk.Revit.UI.Result.Succeeded;
        }
        #endregion
    }

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class CompareAssemblies : IExternalCommand
    {
        public Result  Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            // Get the application and document from external command data.
            Application app = commandData.Application.Application;
            Document doc = commandData.Application.ActiveUIDocument.Document;

            IList<Reference> assemblyRefs = commandData.Application.ActiveUIDocument.Selection.PickObjects(Autodesk.Revit.UI.Selection.ObjectType.Element, "Pick Two Assemblies");
            FindAssemblyDifferences(assemblyRefs[0].Element as AssemblyInstance, assemblyRefs[1].Element as AssemblyInstance);

            return Autodesk.Revit.UI.Result.Succeeded;
        }


        private void FindAssemblyDifferences(AssemblyInstance instance1, AssemblyInstance instance2)
        {
            Autodesk.Revit.DB.Document doc = instance1.Document;
            AssemblyDifference difference = AssemblyInstance.CompareAssemblyInstances(instance1, instance2);

            AssemblyDifferenceNone diffNone = difference as AssemblyDifferenceNone;
            if (diffNone != null)
            {
                TaskDialog.Show("Revit", "Assemblies are identical");
                return;
            }

            AssemblyDifferenceConfiguration diffConfig = difference as AssemblyDifferenceConfiguration;
            if (diffConfig != null)
            {
                TaskDialog.Show("Revit", "Elements are not arranged in space relative to each other in the same way");
                return;
            }

            AssemblyDifferenceMemberCount diffMemberCount = difference as AssemblyDifferenceMemberCount;
            if (diffMemberCount != null)
            {
                TaskDialog.Show("Revit", "Elements counts differ: " + diffMemberCount.Count1 + " & " + diffMemberCount.Count2);
                return;
            }

            AssemblyDifferenceNamingCategory diffNameCat = difference as AssemblyDifferenceNamingCategory;
            if (diffNameCat != null)
            {
                int catInt1 = diffNameCat.NamingCategoryId1.IntegerValue;
                int catInt2 = diffNameCat.NamingCategoryId2.IntegerValue;
                TaskDialog.Show("Revit", "Naming categories differ: " +
                    doc.Settings.Categories.get_Item((BuiltInCategory)catInt1).Name + " & " +
                    doc.Settings.Categories.get_Item((BuiltInCategory)catInt2).Name);
                return;
            }

            AssemblyDifferenceMemberDifference memberDifference = difference as AssemblyDifferenceMemberDifference;
            if (memberDifference != null)
            {
                AssemblyMemberDifference assemblyMemberDifference = memberDifference.MemberDifference as AssemblyMemberDifference;

                AssemblyMemberDifferentCategory differentCategory = assemblyMemberDifference as AssemblyMemberDifferentCategory;
                if (differentCategory != null)
                {
                    int int1 = differentCategory.CategoryId1.IntegerValue;
                    int int2 = differentCategory.CategoryId2.IntegerValue;
                    TaskDialog.Show("Revit", "Element categories differ: " +
                        doc.Settings.Categories.get_Item((BuiltInCategory)int1).Name + " & " +
                        doc.Settings.Categories.get_Item((BuiltInCategory)int2).Name);
                    return;
                }
                AssemblyMemberDifferentGeometry differentGeometry = assemblyMemberDifference as AssemblyMemberDifferentGeometry;
                if (differentGeometry != null)
                {
                    TaskDialog.Show("Revit", "Member geometry differs");
                    return;
                }
                AssemblyMemberDifferentParameters differentParameters = assemblyMemberDifference as AssemblyMemberDifferentParameters;
                if (differentParameters != null)
                {
                    TaskDialog.Show("Revit", "Member parameters differs");
                    return;
                }
                AssemblyMemberDifferentType differentType = assemblyMemberDifference as AssemblyMemberDifferentType;
                if (differentType != null)
                {
                    TaskDialog.Show("Revit", "Element types differ: " + doc.get_Element(differentType.TypeId1).Name + " & " + doc.get_Element(differentType.TypeId2).Name);
                    return;
                }
            }
            return;
        }
    }


    


}
