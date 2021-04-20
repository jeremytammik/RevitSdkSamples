using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Revit.SDK.Samples.RebarFreeForm.CS
{
   /// <summary>
   /// Implements the Revit add-in interface IExternalCommand
   /// </summary>
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
   public class AddSharedParams : IExternalCommand
   {
      /// <summary>
      /// Updated: is used to start the regeneration
      /// </summary>
      public static string m_paramName = "Updated";
      /// <summary>
      /// CurveElementId: is used to store the id of a model curve
      /// </summary>
      public static string m_CurveIdName = "CurveElementId";
      /// <summary>
      /// Add two shared parameters to the rebar category instance elements:
      /// Updated: is used to start the regeneration
      /// CurveElementId: is used to store the id of a model curve
      /// </summary>
      public virtual Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
      {
         try
         {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            if (doc == null)
               return Result.Failed;
            using (Transaction tran = new Transaction(doc, "Add shared param"))
            {
               tran.Start();
               bool paramsAdded = AddSharedTestParameter(commandData, m_paramName, SpecTypeId.Boolean.YesNo, false);
               paramsAdded &= AddSharedTestParameter(commandData, m_CurveIdName, SpecTypeId.Int.Integer, true);
               if (paramsAdded)
               {
                  tran.Commit();
                  return Result.Succeeded;
               }
               tran.RollBack();
               return Result.Failed;
            }
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message);
            return Result.Failed;
         }
      }

      private bool AddSharedTestParameter(ExternalCommandData commandData, string paramName, ForgeTypeId paramType, bool userModifiable)
      {
         try
         {
            // check whether shared parameter exists
            if (ShareParameterExists(commandData.Application.ActiveUIDocument.Document, paramName))
            {
               return true;
            }

            // create shared parameter file
            String modulePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            String paramFile = modulePath + "\\RebarTestParameters.txt";
            if (File.Exists(paramFile))
            {
               File.Delete(paramFile);
            }
            FileStream fs = File.Create(paramFile);
            fs.Close();

            // cache application handle
            Autodesk.Revit.ApplicationServices.Application revitApp = commandData.Application.Application;

            // prepare shared parameter file
            commandData.Application.Application.SharedParametersFilename = paramFile;

            // open shared parameter file
            DefinitionFile parafile = revitApp.OpenSharedParameterFile();

            // create a group
            DefinitionGroup apiGroup = parafile.Groups.Create("RebarTestParamGroup");

            // create a visible param 
            ExternalDefinitionCreationOptions ExtDefinitionCreationOptions = new ExternalDefinitionCreationOptions(paramName, paramType);
            ExtDefinitionCreationOptions.HideWhenNoValue = true;//used this to show the parameter only in some rebar instances that will use it
            ExtDefinitionCreationOptions.UserModifiable = userModifiable;//  set if users need to modify this
            Definition rebarSharedParamDef = apiGroup.Definitions.Create(ExtDefinitionCreationOptions);

            // get rebar category
            Category rebarCat = commandData.Application.ActiveUIDocument.Document.Settings.Categories.get_Item(BuiltInCategory.OST_Rebar);
            CategorySet categories = revitApp.Create.NewCategorySet();
            categories.Insert(rebarCat);

            // insert the new parameter
            InstanceBinding binding = revitApp.Create.NewInstanceBinding(categories);
            commandData.Application.ActiveUIDocument.Document.ParameterBindings.Insert(rebarSharedParamDef, binding);
            return true;
         }
         catch (Exception ex)
         {
            throw new Exception("Failed to create shared parameter: " + ex.Message);
         }
      }
      /// <summary>
      /// Checks if a parameter exists based of a name
      /// </summary>
      /// <param name="doc"></param>
      /// <param name="paramName"></param>
      /// <returns></returns>
      private bool ShareParameterExists(Document doc, String paramName)
      {
         BindingMap bindingMap = doc.ParameterBindings;
         DefinitionBindingMapIterator iter = bindingMap.ForwardIterator();
         iter.Reset();

         while (iter.MoveNext())
         {
            Definition tempDefinition = iter.Key;

            // find the definition of which the name is the appointed one
            if (String.Compare(tempDefinition.Name, paramName) != 0)
            {
               continue;
            }

            // get the category which is bound
            ElementBinding binding = bindingMap.get_Item(tempDefinition) as ElementBinding;
            CategorySet bindCategories = binding.Categories;
            foreach (Category category in bindCategories)
            {
               if (category.Name
                   == doc.Settings.Categories.get_Item(BuiltInCategory.OST_Rebar).Name)
               {
                  return true;
               }
            }
         }
         return false;
      }
   }
}