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

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.SDK.Samples.ModelessForm_ExternalEvent.CS
{
    /// <summary>
    ///   A class with methods to execute requests made by the dialog user.
    /// </summary>
    /// 
    public class RequestHandler : IExternalEventHandler
    {
        // A trivial delegate, but handy
        private delegate void DoorOperation(FamilyInstance e);

        // The value of the latest request made by the modeless form 
        private Request m_request = new Request();

        /// <summary>
        /// A public property to access the current request value
        /// </summary>
        public Request Request
        {
            get { return m_request; }
        }

        /// <summary>
        ///   A method to identify this External Event Handler
        /// </summary>
        public String GetName()
        {
            return "R2014 External Event Sample";
        }


        /// <summary>
        ///   The top method of the event handler.
        /// </summary>
        /// <remarks>
        ///   This is called by Revit after the corresponding
        ///   external event was raised (by the modeless form)
        ///   and Revit reached the time at which it could call
        ///   the event's handler (i.e. this object)
        /// </remarks>
        /// 
        public void Execute(UIApplication uiapp)
        {
            try
            {
                switch (Request.Take())
                {
                    case RequestId.None:
                        {
                            return;  // no request at this time -> we can leave immediately
                        }
                    case RequestId.Delete:
                        {
                            ModifySelectedDoors(uiapp, "Delete doors", e => e.Document.Delete(e.Id));
                            break;
                        }
                    case RequestId.FlipLeftRight:
                        {
                            ModifySelectedDoors(uiapp, "Flip door Hand", e => e.flipHand());
                            break;
                        }
                    case RequestId.FlipInOut:
                        {
                            ModifySelectedDoors(uiapp, "Flip door Facing", e => e.flipFacing());
                            break;
                        }
                    case RequestId.MakeLeft:
                        {
                            ModifySelectedDoors(uiapp, "Make door Left", MakeLeft);
                            break;
                        }
                    case RequestId.MakeRight:
                        {
                            ModifySelectedDoors(uiapp, "Make door Right", MakeRight);
                            break;
                        }
                    case RequestId.TurnOut:
                        {
                            ModifySelectedDoors(uiapp, "Place door Out", TurnOut);
                            break;
                        }
                    case RequestId.TurnIn:
                        {
                            ModifySelectedDoors(uiapp, "Place door In", TurnIn);
                            break;
                        }
                    case RequestId.Rotate:
                        {
                            ModifySelectedDoors(uiapp, "Rotate door", FlipHandAndFace);
                            break;
                        }
                    default:
                        {
                            // some kind of a warning here should
                            // notify us about an unexpected request 
                            break;
                        }
                }
            }
            finally
            {
                Application.thisApp.WakeFormUp();
            }

            return;
        }


        /// <summary>
        ///   The main door-modification subroutine - called from every request 
        /// </summary>
        /// <remarks>
        ///   It searches the current selection for all doors
        ///   and if it finds any it applies the requested operation to them
        /// </remarks>
        /// <param name="uiapp">The Revit application object</param>
        /// <param name="text">Caption of the transaction for the operation.</param>
        /// <param name="operation">A delegate to perform the operation on an instance of a door.</param>
        /// 
        private void ModifySelectedDoors(UIApplication uiapp, String text, DoorOperation operation)
        {
            UIDocument uidoc = uiapp.ActiveUIDocument;

            // check if there is anything selected in the active document

            if ((uidoc != null) && (uidoc.Selection != null))
            {
                ICollection<ElementId> selElements = uidoc.Selection.GetElementIds();
                if (selElements.Count > 0)
                {
                    // Filter out all doors from the current selection

                    FilteredElementCollector collector = new FilteredElementCollector(uidoc.Document, selElements);
                    ICollection<Element> doorset = collector.OfCategory(BuiltInCategory.OST_Doors).ToElements();

                    if (doorset != null)
                    {
                        // Since we'll modify the document, we need a transaction
                        // It's best if a transaction is scoped by a 'using' block
                        using (Transaction trans = new Transaction(uidoc.Document))
                        {
                            // The name of the transaction was given as an argument

                            if (trans.Start(text) == TransactionStatus.Started)
                            {
                                // apply the requested operation to every door

                                foreach (FamilyInstance door in doorset)
                                {
                                    operation(door);
                                }

                                trans.Commit();
                            }
                        }
                    }
                }
            }
        }


        //////////////////////////////////////////////////////////////////////////
        //
        // Helpers - simple delegates operating upon an instance of a door

        private void FlipHandAndFace(FamilyInstance e)
        {
            e.flipFacing(); e.flipHand();
        }

        // Note: The door orientation [left/right] is according the common
        // conventions used by the building industry in the Czech Republic.
        // If the convention is different in your county (like in the U.S),
        // swap the code of the MakeRight and MakeLeft methods.

        private static void MakeLeft(FamilyInstance e)
        {
            if (e.FacingFlipped ^ e.HandFlipped) e.flipHand();
        }

        private void MakeRight(FamilyInstance e)
        {
            if (!(e.FacingFlipped ^ e.HandFlipped)) e.flipHand();
        }

        // Note: The In|Out orientation depends on the position of the
        // wall the door is in; therefore it does not necessary indicates
        // the door is facing Inside, or Outside, respectively.
        // The presented implementation is good enough to demonstrate
        // how to flip a door, but the actual algorithm will likely
        // have to be changes in a read-world application.

        private void TurnIn(FamilyInstance e)
        {
            if (!e.FacingFlipped) e.flipFacing();
        }

        private void TurnOut(FamilyInstance e)
        {
            if (e.FacingFlipped) e.flipFacing();
        }

    }  // class

}  // namespace
