using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using Revit.SDK.Samples.AutoParameter.CS;
using Revit.SDK.Samples.GenericModelCreation.CS;
using Revit.SDK.Samples.TypeRegeneration.CS;
using Revit.SDK.Samples.ValidateParameters.CS;

namespace MacroSamples_RFA
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.DB.Macros.AddInId("B0302F6B-64AC-438F-93CB-61F3C632FD57")]
   public partial class ThisApplication
   {
      private void Module_Startup(object? sender, EventArgs e)
      {

      }

      private void Module_Shutdown(object? sender, EventArgs e)
      {

      }

      #region Revit Macros generated code
      private void InternalStartup()
      {
         this.Startup += new System.EventHandler(Module_Startup);
         this.Shutdown += new System.EventHandler(Module_Shutdown);
      }
      #endregion
      /// <summary>
      /// AutoJoin
      /// </summary>
      public void AutoJoin()
      {
         using (Transaction trans = new Transaction(this.ActiveUIDocument.Document, "AutoJoin"))
         {
            trans.Start();
            CombinableElementArray solids = this.ActiveUIDocument.Document.Application.Create.NewCombinableElementArray();
            foreach (Autodesk.Revit.DB.ElementId elementId in this.ActiveUIDocument.Selection.GetElementIds())
            {
               Element element = this.ActiveUIDocument.Document.GetElement(elementId);
               System.Diagnostics.Trace.WriteLine(element.GetType().ToString());

               GenericForm? gf = element as GenericForm;
               if (null != gf && !gf.IsSolid)
               {
                  continue;
               }

               CombinableElement? ce = element as CombinableElement;
               if (null != ce)
               {
                  solids.Append(ce);
               }
            }

            if (solids.Size < 2)
            {
               MessageBox.Show("At least 2 combinable elements should be selected.");
               return;
            }

            this.ActiveUIDocument.Document.CombineElements(solids);
            trans.Commit();
         }
      }

      /// <summary>
      /// AutoParameter
      /// </summary>
      public void AutoParameter()
      {
         using (Transaction trans = new Transaction(this.ActiveUIDocument.Document, "AutoParameter"))
         {
            trans.Start();
            MessageManager.MessageBuff = new StringBuilder();
            bool succeeded = AddParameters();
            if (!succeeded)
            {
               MessageBox.Show(MessageManager.MessageBuff.ToString());
            }
            trans.Commit();
         }
      }

      /// <summary>
      /// add parameters to the active document
      /// </summary>
      /// <returns>
      /// if succeeded, return true; otherwise false
      /// </returns>
      private bool AddParameters()
      {
         Document doc = this.ActiveUIDocument.Document;
         if (null == doc)
         {
            MessageManager.MessageBuff.Append("There's no available document. \n");
            return false;
         }

         if (!doc.IsFamilyDocument)
         {
            MessageManager.MessageBuff.Append("The active document is not a family document. \n");
            return false;
         }

         FamilyParameterAssigner assigner = new FamilyParameterAssigner(this);
         // the parameters to be added are defined and recorded in a text file, read them from that file and load to memory
         bool succeeded = assigner.LoadParametersFromFile();
         if (!succeeded)
         {
            return false;
         }

         succeeded = assigner.AddParameters();
         if (succeeded)
         {
            return true;
         }
         else
         {
            return false;
         }
      }

      /// <summary>
      /// GenericModelCreation
      /// </summary>
      public void GenericModelCreation()
      {
         using (Transaction trans = new Transaction(this.ActiveUIDocument.Document, "GenericModelCreation"))
         {
            trans.Start();
            GenericModelCreation sample = new GenericModelCreation(this);
            sample.Run();
            trans.Commit();
         }
      }

      /// <summary>
      /// TypeRegeneration
      /// </summary>
      public void TypeRegeneration()
      {
         TypeRegeneration sample = new TypeRegeneration(this);
         sample.Run();
      }

      /// <summary> 
      /// ValidateParameters
      /// </summary>
      public void ValidateParameters()
      {
         ValidateParameters sample = new ValidateParameters(this);
         sample.Run();
      }
   }
}
