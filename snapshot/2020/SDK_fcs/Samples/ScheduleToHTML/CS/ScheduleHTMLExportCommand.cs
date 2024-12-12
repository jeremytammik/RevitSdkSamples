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
                Autodesk.Revit.ApplicationServices.Application revitApplication = commandData.Application.Application;
                bool bInteractive = revitApplication.IsJournalPlaying() ? false : true;
                return exporter.ExportToHTML(bInteractive, ref message) ? Result.Succeeded : Result.Cancelled;
            }
            
            message = "Unable to proceed: Active view must be a schedule.";
            return Result.Cancelled;
        }

        #endregion
    }
}

