using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ScheduleToHTML.CS
{
    /// <summary>
    /// The external command exporting the active schedule to HTML.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ScheduleHTMLExportCommand : IExternalCommand
    {
        #region IExternalCommand Members

        /// <summary>
        /// The implementation of the command.
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            View activeView = commandData.View;

            if (activeView is ViewSchedule)
            {
                ScheduleHTMLExporter exporter = new ScheduleHTMLExporter(activeView as ViewSchedule);
                exporter.ExportToHTML();
            }
            else
            {
                TaskDialog.Show("Unable to proceed", "Active view must be a schedule.");
                return Result.Cancelled;
            }
            return Result.Succeeded;
        }

        #endregion
    }
}

