//
// (C) Copyright 2003-2009 by Autodesk, Inc.
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

namespace Revit.SDK.Samples.PowerCircuit.CS
{
    /// <summary>
    /// An enumerate type listing the operations
    /// </summary>
    public enum Operation
    {
        /// <summary>
        /// Create a new electrical circuit
        /// </summary>
        CreateCircuit,

        /// <summary>
        /// Edit circuit
        /// </summary>
        EditCircuit,

        /// <summary>
        /// Set a panel for circuit
        /// </summary>
        SelectPanel,

        /// <summary>
        /// Disconnect the panel from circuit
        /// </summary>
        DisconnectPanel
    }

    /// <summary>
    /// An enumerate type listing the options to edit a circuit
    /// </summary>
    public enum EditOption
    {
        /// <summary>
        /// Add an element to the circuit
        /// </summary>
        Add,

        /// <summary>
        /// Remove an element from the circuit
        /// </summary>
        Remove,

        /// <summary>
        /// Set a panel for circuit
        /// </summary>
        SelectPanel
    }
}
