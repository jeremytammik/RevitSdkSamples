using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using CS_AvoidObstruction;

namespace MacroSamples_MEP
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.DB.Macros.AddInId("44E3EC19-A71D-402D-8FC0-054D32E35D85")]
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

      public void AvoidObstruction()
      {
         using (Transaction trans = new Transaction(this.ActiveUIDocument.Document, "AvoidObstruction"))
         {
            trans.Start();

            Resolver resolver = new Resolver(this.ActiveUIDocument.Document);
            resolver.Resolve();

            trans.Commit();
         }
      }
   }
}
