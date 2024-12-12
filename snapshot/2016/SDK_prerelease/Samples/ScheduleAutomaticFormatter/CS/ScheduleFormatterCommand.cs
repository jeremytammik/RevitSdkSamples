//
// (C) Copyright 2003-2014 by Autodesk, Inc. All rights reserved.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.

//
// AUTODESK PROVIDES THIS PROGRAM 'AS IS' AND WITH ALL ITS FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable. 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Revit.SDK.Samples.ScheduleAutomaticFormatter.CS
{
    /// <summary>
    /// An external command that formats the columns of the schedule automatically.  After this has taken place,
    /// the schedule formatting will be automatically updated when the schedule changes.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    class ScheduleFormatterCommand : IExternalCommand
    {
        #region IExternalCommand Members

        /// <summary>
        /// The command implementation.
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            ViewSchedule viewSchedule = commandData.View as ViewSchedule;

            // Setup formatter for the schedule
            ScheduleFormatter formatter = new ScheduleFormatter();

            // Setup info needed for the updater
            Schema schema = GetOrCreateSchema();
            formatter.Schema = schema;
            formatter.AddInId = commandData.Application.ActiveAddInId;


            using (Transaction t = new Transaction(viewSchedule.Document, "Format columns"))
            {
                t.Start();
                // Make formatting changes
                formatter.FormatScheduleColumns(viewSchedule);

                // Mark schedule to be formatted
                AddMarkerEntity(viewSchedule, schema);
                t.Commit();
            }

            // Add updater if necessary
            AddUpdater(formatter);

            return Result.Succeeded;
        }

        #endregion

        /// <summary>
        /// Adds an entity to the schedule, indicating that the schedule should be formatted by this tool.
        /// </summary>
        /// <param name="viewSchedule">The schedule.</param>
        /// <param name="schema">The schema used for the entity.</param>
        private void AddMarkerEntity(ViewSchedule viewSchedule, Schema schema)
        {
            // Is entity already present?
            Entity entity = viewSchedule.GetEntity(schema);

            // If not, add it.
            if (!entity.IsValid())
            {
                entity = new Entity(schema);
                entity.Set<bool>("Formatted", true);
                viewSchedule.SetEntity(entity);
            }
        }

        /// <summary>
        /// Set up the updater to watch formatted schedules
        /// </summary>
        /// <param name="application">The application</param>
        public static void SetupUpdater(UIControlledApplication application)
        {
            // Setup temporary schedule formatter
            Schema schema = GetOrCreateSchema();
            ScheduleFormatter formatter = new ScheduleFormatter();
            formatter.Schema = schema;
            formatter.AddInId = application.ActiveAddInId;

            // Add updater
            AddUpdater(formatter);
        }

        /// <summary>
        /// Set up the schema used to mark the schedules as formatted.
        /// </summary>
        /// <returns></returns>
        private static Schema GetOrCreateSchema()
        {
            Guid schemaId = new Guid("98017A5F-F4A7-451C-8807-EF137B587C50");

            Schema schema = Schema.Lookup(schemaId);
            if (schema == null)
            {
                SchemaBuilder builder = new SchemaBuilder(schemaId);
                builder.SetSchemaName("ScheduleFormatterFlag");
                builder.AddSimpleField("Formatted", typeof(Boolean));
                schema = builder.Finish();
            }

            return schema;
        }

        /// <summary>
        /// Add the updater to watch for formatted schedule changes.
        /// </summary>
        /// <param name="formatter">The schedule formatter.</param>
        private static void AddUpdater(ScheduleFormatter formatter)
        {
            // If not registered, register the updater
            if (!UpdaterRegistry.IsUpdaterRegistered(formatter.GetUpdaterId()))
            {
                // Filter on: schedule type, and extensible storage entity of the target schema
                ElementClassFilter classFilter = new ElementClassFilter(typeof(ViewSchedule));
                ExtensibleStorageFilter esFilter = new ExtensibleStorageFilter(formatter.Schema.GUID);
                LogicalAndFilter filter = new LogicalAndFilter(classFilter, esFilter);

                // Register and add trigger for updater.
                UpdaterRegistry.RegisterUpdater(formatter);
                UpdaterRegistry.AddTrigger(formatter.GetUpdaterId(), filter, Element.GetChangeTypeAny());
            }
        }
    }
}
