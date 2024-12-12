using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ScheduleCreation.CS
{
    /// <summary>
    /// Utility class that contains methods of view schedule creation and schedule sheet instance creation.
    /// </summary>
    class ScheduleCreationUtility
    {
        private static BuiltInParameter[] s_skipParameters = new BuiltInParameter[] { BuiltInParameter.ALL_MODEL_MARK };

        /// <summary>
        /// Create view schedule(s) and add them to sheet.
        /// </summary>
        /// <param name="uiDocument">UIdocument of revit file.</param>
        public void CreateAndAddSchedules(UIDocument uiDocument)
        {
            TransactionGroup tGroup = new TransactionGroup(uiDocument.Document, "Create schedules and sheets");
            tGroup.Start();

            ICollection<ViewSchedule> schedules = CreateSchedules(uiDocument);

            foreach (ViewSchedule schedule in schedules)
            {
                AddScheduleToNewSheet(uiDocument.Document, schedule);
            }

            tGroup.Assimilate();
        }

        /// <summary>
        /// Create a sheet to show the schedule.
        /// </summary>
        /// <param name="document">DBDocument of revit file.</param>
        /// <param name="schedule">View schedule which will be shown on sheet.</param>
        private void AddScheduleToNewSheet(Document document, ViewSchedule schedule)
        {
            //Create a filter to get all the title block types.
            FilteredElementCollector collector = new FilteredElementCollector(document);
            collector.OfCategory(BuiltInCategory.OST_TitleBlocks);
            collector.WhereElementIsElementType();

            Transaction t = new Transaction(document, "Create and populate sheet");
            t.Start();

            //Get ElementId of first title block type.
            ElementId titleBlockId = collector.FirstElementId();

            //Create sheet by gotten title block type.
            ViewSheet newSheet = ViewSheet.Create(document, titleBlockId);
            newSheet.Name = "Sheet for " + schedule.Name;

            document.Regenerate();

            //Declare a XYZ to be used as the upperLeft point of schedule sheet instance to be created.
            XYZ upperLeft = new XYZ();

            //If there is an existing title block.
            if (titleBlockId != ElementId.InvalidElementId)
            {
                //Find titleblock of the newly created sheet.
                collector = new FilteredElementCollector(document);
                collector.OfCategory(BuiltInCategory.OST_TitleBlocks);
                collector.OwnedByView(newSheet.Id);
                Element titleBlock = collector.FirstElement();

                //Get bounding box of the title block.
                BoundingBoxXYZ bbox = titleBlock.get_BoundingBox(newSheet);

                //Get upperLeft point of the bounding box.
                upperLeft = new XYZ(bbox.Min.X, bbox.Max.Y, bbox.Min.Z);
                //Move the point to the postion that is 2 inches right and 2 inches down from the original upperLeft point.
                upperLeft = upperLeft + new XYZ(2.0 / 12.0, -2.0 / 12.0, 0);
            }

            //Create a new schedule sheet instance that makes the sheet to show the data of wall view schedule at upperLeft point.
            ScheduleSheetInstance placedInstance = ScheduleSheetInstance.Create(document, newSheet.Id, schedule.Id, upperLeft);

            t.Commit();
        }

        /// <summary>
        /// Create a view schedule of wall category and add schedule field, filter and sorting/grouping field to it.
        /// </summary>
        /// <param name="uiDocument">UIdocument of revit file.</param>
        /// <returns>ICollection of created view schedule(s).</returns>
        private ICollection<ViewSchedule> CreateSchedules(UIDocument uiDocument)
        {
            Document document = uiDocument.Document;

            Transaction t = new Transaction(document, "Create Schedules");
            t.Start();

            List<ViewSchedule> schedules = new List<ViewSchedule>();

            //Create an empty view schedule of wall category.
            ViewSchedule schedule = ViewSchedule.CreateSchedule(document, new ElementId(BuiltInCategory.OST_Walls), ElementId.InvalidElementId);
            schedule.Name = "Wall Schedule 1";
            schedules.Add(schedule);

            //Iterate all the schedulable field gotten from the walls view schedule.
            foreach (SchedulableField schedulableField in schedule.Definition.GetSchedulableFields())
            {
                //Judge if the FieldType is ScheduleFieldType.Instance.
                if (schedulableField.FieldType == ScheduleFieldType.Instance)
                {
                    //Get ParameterId of SchedulableField.
                    ElementId parameterId = schedulableField.ParameterId;

                    //If the ParameterId is id of BuiltInParameter.ALL_MODEL_MARK then ignore next operation.
                    if (ShouldSkip(parameterId))
                        continue;

                    //Add a new schedule field to the view schedule by using the SchedulableField as argument of AddField method of Autodesk.Revit.DB.ScheduleDefinition class.
                    ScheduleField field = schedule.Definition.AddField(schedulableField);

                    //Judge if the parameterId is a BuiltInParameter.
                    if (Enum.IsDefined(typeof(BuiltInParameter), parameterId.IntegerValue))
                    {
                        BuiltInParameter bip = (BuiltInParameter)parameterId.IntegerValue;
                        //Get the StorageType of BuiltInParameter.
                        StorageType st = document.get_TypeOfStorage(bip);
                        //if StorageType is String or ElementId, set GridColumnWidth of schedule field to three times of current GridColumnWidth. 
                        //And set HorizontalAlignment property to left.
                        if (st == StorageType.String || st == StorageType.ElementId)
                        {
                            field.GridColumnWidth = 3 * field.GridColumnWidth;
                            field.HorizontalAlignment = ScheduleHorizontalAlignment.Left;
                        }
                        //For other StorageTypes, set HorizontalAlignment property to center.
                        else
                        {
                            field.HorizontalAlignment = ScheduleHorizontalAlignment.Center;
                        }
                    }


                    //Filter the view schedule by volume
                    if (field.ParameterId == new ElementId(BuiltInParameter.HOST_VOLUME_COMPUTED))
                    {
                        double volumeFilterInCubicFt = 0.8 * Math.Pow(3.2808399, 3.0);
                        ScheduleFilter filter = new ScheduleFilter(field.FieldId, ScheduleFilterType.GreaterThan, volumeFilterInCubicFt);
                        schedule.Definition.AddFilter(filter);
                    }

                    //Group and sort the view schedule by type
                    if (field.ParameterId == new ElementId(BuiltInParameter.ELEM_TYPE_PARAM))
                    {
                        ScheduleSortGroupField sortGroupField = new ScheduleSortGroupField(field.FieldId);
                        sortGroupField.ShowHeader = true;
                        schedule.Definition.AddSortGroupField(sortGroupField);
                    }
                }
            }

            t.Commit();

            uiDocument.ActiveView = schedule;

            return schedules;
        }

        /// <summary>
        /// Judge if the parameterId should be skipped.
        /// </summary>
        /// <param name="parameterId">ParameterId to be judged.</param>
        /// <returns>Return true if parameterId should be skipped.</returns>
        private bool ShouldSkip(ElementId parameterId)
        {
            foreach (BuiltInParameter bip in s_skipParameters)
            {
                if (new ElementId(bip) == parameterId)
                    return true;
            }
            return false;
        }
    }
}
