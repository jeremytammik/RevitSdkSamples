using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Revit.SDK.Samples.ScheduleAutomaticFormatter.CS
{
    /// <summary>
    /// A class capable of formatting schedules with alternating background colors on the columns
    /// </summary>
    class ScheduleFormatter : IUpdater
    {
        /// <summary>
        /// Formats the schedule with alternating background colors
        /// </summary>
        /// <param name="viewSchedule"></param>
        public void FormatScheduleColumns(ViewSchedule viewSchedule)
        {
            ScheduleDefinition definition = viewSchedule.Definition;
            int index = 0;

            Color white = new Color(0xFF, 0xFF, 0xFF);
            Color highlight = new Color(0xd8, 0xd8, 0xd8);
            bool applyHighlight = false;

            // Loop over fields in order
            foreach (ScheduleFieldId id in definition.GetFieldOrder())
            {
                // Index 0, 2, etc use highlight color
                if (index % 2 == 0)
                {
                    applyHighlight = true;
                }
                // Index 1, 3, etc use no background color
                else
                {
                    applyHighlight = false;
                }

                // Get the field style
                ScheduleField field = definition.GetField(id);
                TableCellStyle style = field.GetStyle();
                TableCellStyleOverrideOptions options = style.GetCellStyleOverrideOptions();

                // Set override options for background color per requirement
                options.BackgroundColor = applyHighlight;
                style.SetCellStyleOverrideOptions(options);

                // Set background color per requirement
                style.BackgroundColor = applyHighlight ? highlight : white;
                field.SetStyle(style);

                index++;
            }
        }

        #region UpdaterSupport

        /// <summary>
        /// The ExtensibleStorage schema for marking schedules which have been formatted.
        /// </summary>
        public Schema Schema
        {
            get;
            set;
        }

        /// <summary>
        /// The add-in id.
        /// </summary>
        public AddInId AddInId
        {
            get;
            set;
        }

        /// <summary>
        /// Constructs a new schedule formatter utility.
        /// </summary>
        public ScheduleFormatter()
        {
            Schema = null;
            AddInId = null;
        }

        #region IUpdater Members

        /// <summary>
        /// Implements IUpdater.Execute()
        /// </summary>
        /// <param name="data"></param>
        public void Execute(UpdaterData data)
        {
            // Only previously formatted schedules should trigger - so just reformat them
            foreach (ElementId scheduleId in data.GetModifiedElementIds())
            {
                ViewSchedule schedule = data.GetDocument().GetElement(scheduleId) as ViewSchedule;
                FormatScheduleColumns(schedule);
            }
        }

        /// <summary>
        /// Implements IUpdater.GetAdditionalInformation()
        /// </summary>
        /// <returns></returns>
        public string GetAdditionalInformation()
        {
            return "Automatic schedule formatter";
        }

        /// <summary>
        /// Implements IUpdater.GetChangePriority()
        /// </summary>
        /// <returns></returns>
        public ChangePriority GetChangePriority()
        {
            return ChangePriority.Views;
        }

        /// <summary>
        /// Implements IUpdater.GetUpdaterId()
        /// </summary>
        /// <returns></returns>
        public UpdaterId GetUpdaterId()
        {
            return new UpdaterId(AddInId, UpdaterGUID);
        }

        /// <summary>
        /// Implements IUpdater.GetUpdaterName()
        /// </summary>
        /// <returns></returns>
        public string GetUpdaterName()
        {
            return "AutomaticScheduleFormatter";
        }

        #endregion

        /// <summary>
        /// GUID of the updater.
        /// </summary>
        private static System.Guid UpdaterGUID
        {
            get { return new System.Guid("{C8483107-EF6D-4FDB-BB88-AF79E0E62361}"); }
        }

        #endregion
    }
}
