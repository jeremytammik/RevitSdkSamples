using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace Rooms
{
   [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
   [Autodesk.Revit.DB.Macros.AddInId("BF1E6F6A-4B74-4218-9D35-7AC448FBFCB4")]
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
      public void GetAllRoomsInformation()
      {
         SamplesRoom sampleRoom = new SamplesRoom(this);
         sampleRoom.Run();
      }
   }
}
