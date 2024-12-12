//
// (C) Copyright 2007-2011 by Autodesk, Inc. All rights reserved.
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


/// <summary>
/// </summary>

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Autodesk.REX.Framework;

using REX.Common;

namespace REX.Serialization
{
    [Serializable]
    internal class ResultsSerializable : REXExtensionDataSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultsSerializable"/> class.
        /// </summary>
        public ResultsSerializable()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="REXExtensionDataSerializable"/> class.
        /// A required constructor by ISerializable to deserialize the object. It is important to implement
        /// this required constructor. Note that no warning will be given if this constructor is absent and
        /// an exception will be thrown if you attempt to deserialize this object.
        /// It is a good idea to make the constructor protected unless the class was sealed, in which case
        /// the constructor should be marked as private. Making the constructor protected ensures that users
        /// cannot explicitly call this constructor while still allowing derived classes to call it. The same
        /// logic applies if this class was sealed except that there will be no derived classes.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="ctx">The CTX.</param>
        protected ResultsSerializable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctx)
        {
        }
        /// <summary>
        /// A required method by ISerializable to serialize the object. Simply add variables to be serialized
        /// as name/value pairs to SerializationInfo object. Here you decide which member variables to serialize.
        /// Note that any name can be used to identify each serialized field
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="ctx">The CTX.</param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctx)
        {
        }

        /// <summary>
        /// Sets the results reference.
        /// </summary>
        /// <param name="results">The results.</param>
        internal void SetResultsReference(Results results)
        {
            ResultsRef = results;
        }

        private Results ResultsRef;
    }

    /// <summary>
    /// Main data class to store all results in REX base units.
    /// </summary>
    internal class Results : REXResults
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="Results"/> class.
        /// </summary>
        /// <param name="Ext">The reference to REXExtension object.</param>
        public Results(REXExtension Ext)
            : base(Ext)
        {
            VersionCurrent = 1;
            ResultsSerializableRef = new ResultsSerializable();
        }

        /// <summary>
        /// Get the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        internal Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }

        /// <summary>
        /// Called when [set defaults].
        /// </summary>
        /// <param name="UnitsSystem">The units system.</param>
        /// <summary>
        /// Called when set defaults.
        /// </summary>
        /// <param name="UnitsSystem">The units system.</param>

        protected override void OnSetDefaults(REXUnitsSystemType UnitsSystem)
        {
            if (UnitsSystem == REXUnitsSystemType.Imperial)
            {
            }
            else if (UnitsSystem == REXUnitsSystemType.Metric)
            {
            }
        }

        /// <summary>
        /// Called when save data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <returns>Returns true if succeeded.</returns>
        protected override bool OnSave(ref BinaryWriter Data)
        {
            if (Mode == DataMode.ModeFile)
            {
            }
            if (Mode == DataMode.ModeProject)
            {
            }
            if (Mode == DataMode.ModeObject)
            {
            }

            return true;
        }
        /// <summary>
        /// Called when load data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <returns>Returns true if succeeded.</returns>

        protected override bool OnLoad(ref BinaryReader Data)
        {
            if (Mode == DataMode.ModeFile)
            {
            }
            if (Mode == DataMode.ModeProject)
            {
            }
            if (Mode == DataMode.ModeObject)
            {
            }

            if (VersionLoaded >= 1)
            {
            }

            return true;
        }


        /// <summary>
        /// Called when save data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <returns>Returns true if succeeded.</returns>
        protected override bool OnSave(ref Stream DataStream, ref System.Runtime.Serialization.IFormatter DataFormater)
        {
            if (ResultsSerializableRef != null)
                DataFormater.Serialize(DataStream, ResultsSerializableRef);
            return true;
        }

        /// <summary>
        /// Called when load data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <returns>Returns true if succeeded.</returns>
        protected override bool OnLoad(ref Stream DataStream, ref System.Runtime.Serialization.IFormatter DataFormater)
        {
            ResultsSerializableRef = (ResultsSerializable)DataFormater.Deserialize(DataStream);
            return true;
        }

        internal ResultsSerializable ResultsSerializableRef;
    }
}
