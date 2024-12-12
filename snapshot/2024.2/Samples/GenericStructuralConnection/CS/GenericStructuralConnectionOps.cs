//
// (C) Copyright 2003-2015 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.GenericStructuralConnection.CS
{
    /// <summary>
    /// Performs basic operations on generic structural connections.
    /// </summary>
    public class GenericStructuralConnectionOps
    {
        /// <summary>
        /// Create generic structural connection.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result CreateGenericStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            List<ElementId> ids = StructuralConnectionSelectionUtils.SelectConnectionElements(activeDoc);
            if (ids.Count() > 0)
            {
                // Start a new transaction.
                using (Transaction tran = new Transaction(activeDoc.Document, "Create generic structural connection"))
                {
                    tran.Start();

                    StructuralConnectionHandler conn = StructuralConnectionHandler.CreateGenericConnection(activeDoc.Document, ids);
               
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
                message = "There is no element selected !";
                ret = Result.Failed;
            }

            return ret;
        }

        /// <summary>
        /// Delete generic structural connection.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result DeleteGenericStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            // Select a structural connection.
            StructuralConnectionHandler conn = StructuralConnectionSelectionUtils.SelectConnection(activeDoc);

            if (conn != null)
            {
                // Start a new transaction.
                using (Transaction tran = new Transaction(activeDoc.Document, "Delete generic structural connection"))
                {
                    tran.Start();

                    // Delete selected structural connection.
                    activeDoc.Document.Delete(conn.Id);

                    TransactionStatus ts = tran.Commit();
                    if (ts != TransactionStatus.Committed)
                    {
                        message = "Failed to commit the current transaction !";
                        return Result.Failed;
                    }
                }
            }
            else
            {
                message = "There is no connection selected !";
                ret = Result.Failed;
            }

            return ret;
        }

        /// <summary>
        /// Read information from generic structural connection.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result ReadGenericStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            // Select structural connection.
            StructuralConnectionHandler conn = StructuralConnectionSelectionUtils.SelectConnection(activeDoc);
            if (conn != null)
            {
                // Get information from structural connection.
                StringBuilder msgBuilder = new StringBuilder();
                msgBuilder.AppendLine(string.Format("Connection id : {0}", conn.Id));

                StructuralConnectionHandlerType connType = activeDoc.Document.GetElement(conn.GetTypeId()) as StructuralConnectionHandlerType;
                if (connType != null)
                    msgBuilder.AppendLine(string.Format("Type : {0}", connType.Name));

                msgBuilder.Append("Connected elements ids : ");
                IList<ElementId> connectedElemIds = conn.GetConnectedElementIds();
                foreach (var connId in connectedElemIds)
                {
                    msgBuilder.Append(connId.ToString());
                    if (connId != connectedElemIds.Last())
                        msgBuilder.Append(", ");
                }
                TaskDialog.Show("Info", msgBuilder.ToString());
            }
            else
            {
                message = "There is no connection selected !";
                ret = Result.Failed;
            }

            return ret;
        }

        /// <summary>
        /// Update generic structural connection.
        /// </summary>
        /// <param name="activeDoc">The active document.</param>
        /// <param name="message">Set message on failure.</param>
        /// <returns>Returns the status of the operation.</returns>
        public static Result UpdateGenericStructuralConnection(UIDocument activeDoc, ref string message)
        {
            Result ret = Result.Succeeded;

            // Prompt to select a structural connection.
            StructuralConnectionHandler conn = StructuralConnectionSelectionUtils.SelectConnection(activeDoc);
            if (conn != null)
            {
                // Select elements to add to connection.
                List<ElementId> ids = StructuralConnectionSelectionUtils.SelectConnectionElements(activeDoc);
                if (ids.Count() > 0)
                {
                    // Start a new transaction.
                    using (Transaction transaction = new Transaction(activeDoc.Document, "Update generic structural connection"))
                    {
                        transaction.Start();

                        conn.AddElementIds(ids);

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
                    message = "There are no connection input elements selected !";
                }
            }

            return ret;
        }

    }
}
