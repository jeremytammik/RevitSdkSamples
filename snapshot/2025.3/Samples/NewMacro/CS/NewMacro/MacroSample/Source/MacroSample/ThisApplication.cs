using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;

namespace MacroSample
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("65157549-1E87-4196-81E8-645940E38AD3")]
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

		public void HelloWorld()
		{
			TaskDialog.Show("MacroSample", "HelloWorld");
		}
	}
}
