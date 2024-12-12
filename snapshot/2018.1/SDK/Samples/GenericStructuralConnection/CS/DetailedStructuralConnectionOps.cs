//
// (C) Copyright 2003-2017 by Autodesk, Inc.
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
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Autodesk.Revit;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace Revit.SDK.Samples.GenericStructuralConnection.CS
{
    /// <summary>
    /// Performs basic operations on detailed structural connections.
    /// </summary>
    public class DetailedStructuralConnectionOps
    {
        /// <summary>
        /// Create detailed structural connection.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result CreateDetailedStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            // Selected the elements to be connected.
            List<ElementId> ids = StructuralConnectionSelectionUtils.SelectConnectionElements(activeDoc);
            if (ids.Count() > 0)
            {
                // Start a new transaction.
                using (Transaction transaction = new Transaction(activeDoc.Document, "Create detailed structural connection"))
                {
                    transaction.Start();

                    // The type is from the SteelConnectionsData.xml file.
                    StructuralConnectionHandlerType connectionType = StructuralConnectionHandlerType.Create(activeDoc.Document, "usclipangle", new Guid("A42C5CE5-91C5-47E4-B445-D053E5BD66DB"), "usclipangle");
                    if (connectionType != null)
                    {
                        StructuralConnectionHandler.Create(activeDoc.Document, ids, connectionType.Id);
                    }

                    TransactionStatus ts = transaction.Commit();
                    if (ts != TransactionStatus.Committed)
                    {
                        message = "Failed to commit the current transaction !";
                        ret = Result.Failed;
                    }
                }
            }
            else
            {
                message = "There is no element selected!";
                ret = Result.Failed;
            }

            return ret;
        }

        /// <summary>
        /// Change detailed structural connection.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result ChangeDetailedStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            // Prompt to select a structural connection.
            StructuralConnectionHandler conn = StructuralConnectionSelectionUtils.SelectConnection(activeDoc);
            if (conn != null)
            {
                using (Transaction tran = new Transaction(activeDoc.Document, "Change detailed connection type"))
                {
                    tran.Start();

                    // The type is from the SteelConnectionsData.xml file.
                    StructuralConnectionHandlerType connectionType = StructuralConnectionHandlerType.Create(activeDoc.Document, "shearplatenew", new Guid("B490A703-5B6D-4B7A-8471-752133527925"), "shearplatenew");
                    if (connectionType != null)
                    {
                        // The replacement type should be valid on the connected elements.
                        conn.ChangeTypeId(connectionType.Id);
                    }

                    TransactionStatus ts = tran.Commit();
                    if (ts != TransactionStatus.Committed)
                    {
                        message = "Failed to commit the current transaction !";
                        ret = Result.Failed;
                    }
                }
            }
            else
            {
                message = "There is no connection selected!";
                ret = Result.Failed;
            }

            return ret;
        }

        /// <summary>
        /// Copy detailed structural connection.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result CopyDetailedStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            // Select a connection and the connected elements.
            List<ElementId> ids = activeDoc.Selection.GetElementIds().ToList();
            if (ids.Count() > 0)
            {
                // Create transform
                Transform transform = Transform.CreateTranslation(new XYZ(0, 20, 0));

                // Copy selection
                using (Transaction tran = new Transaction(activeDoc.Document, "Copy elements"))
                {
                    tran.Start();
                    ICollection<ElementId> copyResult = ElementTransformUtils.CopyElements(activeDoc.Document, ids, activeDoc.Document, transform, null);
                    TransactionStatus ts = tran.Commit();
                    if (ts != TransactionStatus.Committed)
                    {
                        message = "Failed to commit the current transaction !";
                        ret = Result.Failed;
                    }
                }
            }
            else
            {
                message = "There is no element selected!";
                ret = Result.Failed;
            }

            return ret;
        }

        /// <summary>
        /// Match properties for detailed structural connections.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result MatchPropertiesDetailedStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            // Prompt to select a structural connection
            StructuralConnectionHandler srcConn = StructuralConnectionSelectionUtils.SelectConnection(activeDoc);
            StructuralConnectionHandler destConn = StructuralConnectionSelectionUtils.SelectConnection(activeDoc);

            if (srcConn != null && destConn != null)
            {
                using (Transaction tran = new Transaction(activeDoc.Document, "Match properties"))
                {
                    tran.Start();

                    // Do the properties match.
                    Schema masterSchema = GetSchema(activeDoc.Document, srcConn);
                    Entity masterEnt = srcConn.GetEntity(masterSchema);

                    // You could also access and modify the connection parameters.
                    IList<Field> fields = masterSchema.ListFields();
                    foreach (Field field in fields)
                    {
                        if (field.ValueType == typeof(string))
                        {
                            IList<string> parameters = masterEnt.Get<IList<string>>(field);
                            foreach (string str in parameters)
                            {
                                // Do something.
                            }
                        }
                    }

                    destConn.SetEntity(masterEnt);

                    TransactionStatus ts = tran.Commit();
                    if (ts != TransactionStatus.Committed)
                    {
                        message = "Failed to commit the current transaction !";
                        ret = Result.Failed;
                    }
                }
            }
            else
            {
                message = "There must be two connections selected !";
                ret = Result.Failed;
            }


            return ret;
        }

        /// <summary>
        /// Reset detailed structural connection type to generic.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result ResetDetailedStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            // Prompt to select a structural connection.

            StructuralConnectionHandler conn = StructuralConnectionSelectionUtils.SelectConnection(activeDoc);
            if (conn != null)
            {
                using (Transaction tran = new Transaction(activeDoc.Document, "Change detailed connection type"))
                {
                    tran.Start();

                    ElementId genericTypeId = StructuralConnectionHandlerType.GetDefaultConnectionHandlerType(activeDoc.Document);
                    if (genericTypeId == ElementId.InvalidElementId)
                    {
                        genericTypeId = StructuralConnectionHandlerType.CreateDefaultStructuralConnectionHandlerType(activeDoc.Document);                        
                    }

                    conn.ChangeTypeId(genericTypeId);

                    TransactionStatus ts = tran.Commit();
                    if (ts != TransactionStatus.Committed)
                    {
                        message = "Failed to commit the current transaction !";
                        ret = Result.Failed;
                    }
                }
            }
            else
            {
                message = "There is no connection selected !";
                ret = Result.Failed;
            }

            return ret; ;
        }

        /// <summary>
        /// Get the Extensible storage schema
        /// </summary>
        private static Schema GetSchema(Document doc, StructuralConnectionHandler connection)
        {
            Schema schema = null;

            Guid guid = GetConnectionHandlerTypeGuid(connection, doc);
            if (guid != null && guid != Guid.Empty)
                schema = Schema.ListSchemas().Where(x => x.GUID == guid).FirstOrDefault();

            return schema;
        }

        /// <summary>
        /// Get the unique identifier of the structural steel connection type
        /// </summary>
        /// <param name="conn">structural connection</param>
        /// <param name="doc">current document</param>
        /// <returns>returns the unique identifier of the input connection type</returns>
        private static Guid GetConnectionHandlerTypeGuid(StructuralConnectionHandler conn, Document doc)
        {
            if (conn == null || doc == null)
                return Guid.Empty;

            ElementId typeId = conn.GetTypeId();
            if (typeId == ElementId.InvalidElementId)
                return Guid.Empty;

            StructuralConnectionHandlerType connType = (StructuralConnectionHandlerType)doc.GetElement(typeId);
            if (connType == null || connType.ConnectionGuid == null)
                return Guid.Empty;

            return connType.ConnectionGuid;
        }
    }
}
