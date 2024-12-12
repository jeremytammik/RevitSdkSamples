//
// (C) Copyright 2003-2010 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE. AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//

using System;
using System.IO;
using System.Data;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.Collections;
using Autodesk.Revit.ApplicationServices;

namespace ChangesMonitor
{
    /// <summary>
    /// A class inherits IExternalApplication interface and provide an entry of the sample.
    /// It create a modeless dialog to track the changes.
    /// </summary>
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    [Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.NoCommandData)]
    public class ExternalApplication : IExternalApplication
    {
        /// <summary>
        /// On Revit's startup, we subscribe to the DocumentChange event
        /// This is called even before a file or default template is loaded.
        /// </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentChanged += OnDocumentChanged;
            return Result.Succeeded;
        }

        /// <summary>
        /// On Revit's shut-down, we un-subscribe to the DocumentChange event
        /// and will also release our private objects
        public Result OnShutdown(UIControlledApplication application)
        {
            application.ControlledApplication.DocumentChanged -= OnDocumentChanged;
            m_InfoForm = null;
            m_ChangesInfoTable = null;
            return Result.Succeeded;
        }

        /// <summary>
        /// This method is the event handler, which will dump the change information to tracking dialog
        /// </summary>
        void OnDocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            // we do nothing unless the info table was created already
            if( m_ChangesInfoTable == null )
            {
               return;
            }

            // get the current document.
            Document doc = e.GetDocument();

            // add info about all affected elements to the information window
            
            ICollection<ElementId> addedElem = e.GetAddedElementIds();
            foreach (ElementId id in addedElem)
            {
                AddChangeInfoRow(id, doc, "Added");
            }

            ICollection<ElementId> deletedElem = e.GetDeletedElementIds();
            foreach (ElementId id in deletedElem)
            {
                AddChangeInfoRow(id, doc, "Deleted");
            }

            ICollection<ElementId> modifiedElem = e.GetModifiedElementIds();
            foreach (ElementId id in modifiedElem)
            {
                AddChangeInfoRow(id, doc, "Modified");
            }
        }
         
        /// <summary>
        /// This method is used to retrieve the changed element and add row to data table.
        /// </summary>
        private void AddChangeInfoRow(ElementId id, Document doc, string changeType)
        {
           // retrieve the changed element (could be null if deleted!)
           Element elem = doc.get_Element(id);

           // add a new row with the information about this element

           DataRow newRow = m_ChangesInfoTable.NewRow();

            newRow["ChangeType"] = changeType;
            newRow["Id"] = id.IntegerValue.ToString();

            if (elem == null) // deleted?
            {
                newRow["Name"] = "";
                newRow["Category"] = "";
            }
            else
            {
                newRow["Name"] = elem.Name;

                if (elem.Category != null) // not all elements have categories
                {
                  newRow["Category"] = elem.Category.Name;
                }
                else
                {
                   newRow["Category"] = "";
                }
            }

            newRow["Document"] = doc.Title;
           
            m_ChangesInfoTable.Rows.Add(newRow);
        }

        /// <summary>
        // this methods either creates or clears the info window and shows is if needed
        /// </summary>
        internal static void InitAndShowInfomationForm()
        {
           bool doShow = false;

           if (m_InfoForm == null)
           {
              m_InfoForm = new ChangesInformationForm();
              doShow = true;
           }

           m_ChangesInfoTable = CreateChangeInfoTable();
           m_InfoForm.SetDataSource(m_ChangesInfoTable);

           if (doShow)
           {
              m_InfoForm.Show();
           }
        }

        /// <summary>
        // When the form informs us it's been destroyed, we reset our local variables too
        /// </summary>
        internal static void OnFormDestroyed()
        {
           m_InfoForm = null;
           m_ChangesInfoTable = null;
        }

        /// <summary>
        /// Generate a data table with five columns for display in window
        /// </summary>
        /// <returns>The DataTable to be displayed in window</returns>
        private static DataTable CreateChangeInfoTable()
        {
            // create a new dataTable
            DataTable changesInfoTable = new DataTable("ChangesInfoTable");

            // Create a "ChangeType" column. It will be "Added", "Deleted" and "Modified".
            DataColumn styleColumn = new DataColumn("ChangeType", typeof(System.String));
            styleColumn.Caption = "ChangeType";
            changesInfoTable.Columns.Add(styleColumn);

            // Create a "Id" column. It will be the Element ID
            DataColumn idColum = new DataColumn("Id", typeof(System.String));
            idColum.Caption = "Id";
            changesInfoTable.Columns.Add(idColum);

            // Create a "Name" column. It will be the Element Name
            DataColumn nameColum = new DataColumn("Name", typeof(System.String));
            nameColum.Caption = "Name";
            changesInfoTable.Columns.Add(nameColum);

            // Create a "Category" column. It will be the Category Name of the element.
            DataColumn categoryColum = new DataColumn("Category", typeof(System.String));
            categoryColum.Caption = "Category";
            changesInfoTable.Columns.Add(categoryColum);

            // Create a "Document" column. It will be the document which own the changed element.
            DataColumn docColum = new DataColumn("Document", typeof(System.String));
            docColum.Caption = "Document";
            changesInfoTable.Columns.Add(docColum);

            // return this data table 
            return changesInfoTable;
        }

        // private data:

        private static DataTable m_ChangesInfoTable;
        private static ChangesInformationForm m_InfoForm;
    }


    /// <summary>
    /// This class inherits IExternalCommand interface and used to retrieve the dialog again.
    /// </summary>
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    public class Command : IExternalCommand
    {
        /// <summary>
        /// This command is to either create or clear the content of the information window
        /// </summary>
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
           ExternalApplication.InitAndShowInfomationForm();
            return Result.Succeeded;
        }
    }

}
