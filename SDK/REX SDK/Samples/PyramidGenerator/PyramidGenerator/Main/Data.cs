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
using System.IO;

using Autodesk.REX.Framework;

using REX.Common;

namespace REX.PyramidGenerator
{
    [Serializable]
    internal class DataSerializable : REXExtensionDataSerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataSerializable"/> class.
        /// </summary>
        public DataSerializable()
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
        /// <example>
        /// <code>
        /// protected REXExtensionDataSerializable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctx)
        /// {
        ///    strPrivate = info.GetString( "s1" );
        ///     strPublic  = info.GetString( "s2" );
        ///     obMyClass  = (SomeClass) info.GetValue( "ob", typeof(SomeClass) );       // deserializing an object
        /// }
        /// </code>
        /// </example> 
        /// <param name="info">The info.</param>
        /// <param name="ctx">The CTX.</param>
        protected DataSerializable(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctx)
        {
        }
        /// <summary>
        /// A required method by ISerializable to serialize the object. Simply add variables to be serialized
        /// as name/value pairs to SerializationInfo object. Here you decide which member variables to serialize.
        /// Note that any name can be used to identify each serialized field
        /// </summary>
        /// <example>
        /// <code>
        /// public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctx)
        /// {
        ///     info.AddValue("s1", strPrivate);
        ///     info.AddValue("s2", strPublic);
        ///     into.AddValue("ob", obMyClass);       // Serializing an object
        /// }
        /// </code>
        /// </example> 
        /// <param name="info">The info.</param>
        /// <param name="ctx">The CTX.</param>
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext ctx)
        {
        }


        /// <summary>
        /// Sets the data reference.
        /// </summary>
        /// <param name="data">The data.</param>
        public void SetDataReference(Data data)
        {
            DataRef = data;
        }

        private Data DataRef;
    }


    /// <summary>
    /// Main data class to store all data in REX base units.
    /// </summary>
    internal class Data : REXData
    {
        //Step 2.1.: Preparing Data       
        /// <summary>
        /// Gets or sets the height of the pyramid.
        /// </summary>
        /// <value>The height.</value>
        public double H { get; set; }
        /// <summary>
        /// Gets or sets the width of the pyramid.
        /// </summary>
        /// <value>The width.</value>
        public double B { get; set; }
        /// <summary>
        /// Gets or sets the FamilySymbol which will be used to build the pyramid.
        /// </summary>
        /// <value>The family symbol.</value>
        public string FamilySymbol { get; set; }
        /// <summary>
        /// Gets or sets the list of available FamilySymbols.
        /// </summary>
        /// <value>The the list of available FamilySymbols.</value>
        public List<string> AvailableFamilySymbols { get; set; }
        /// <summary>
        /// Gets or sets the list of element ids.
        /// </summary>
        /// <value>The list of element ids.</value>
        public List<long> ElementIds { get; set; }   //Step 5.1.1. : Serialize
        /// <summary>
        /// Initializes a new instance of a <see cref="Data"/> class.
        /// </summary>
        /// <param name="Ext">The reference to REXExtension object.</param>
        public Data(REXExtension Ext)
            : base(Ext)
        {
            VersionCurrent = 1;
            DataSerializableRef = new DataSerializable();
        }
        /// <summary>
        /// Get the main extension.
        /// </summary>
        /// <value>The main extension.</value>
        public Extension ThisMainExtension
        {
            get
            {
                return (Extension)ThisExtension;
            }
        }
        /// <summary>
        /// Called when set defaults.
        /// </summary>
        /// <param name="UnitsSystem">The units system.</param>
        protected override void OnSetDefaults(REXUnitsSystemType UnitsSystem)
        {
            //Step 2.2. : Preparing Data
            H = 5;//m
            B = 10;//m
            FamilySymbol = "";
            AvailableFamilySymbols = new List<string>();
            ElementIds = new List<long>();
        }
        /// <summary>
        /// Called when save data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <returns>Returns true if succeeded.</returns>       
        protected override bool OnSave(ref BinaryWriter Data)
        {
            //Step 5.1.: Extending the Data
            Data.Write(H);
            Data.Write(B);
            Data.Write(FamilySymbol);

            if (Mode != DataMode.ModeFile)
            {
                Data.Write(ElementIds.Count);

                for (int i = 0; i < ElementIds.Count; i++)
                    Data.Write(ElementIds[i]);
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
            //Step 5.1.: Extending the Data

            H = Data.ReadDouble();
            B = Data.ReadDouble();
            FamilySymbol = Data.ReadString();

            if (Mode != DataMode.ModeFile)
            {
                ElementIds = new List<long>();
                int count = Data.ReadInt32();

                for (int i = 0; i < count; i++)
                    ElementIds.Add(Data.ReadInt64());
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
            if (DataSerializableRef != null)
                DataFormater.Serialize(DataStream, DataSerializableRef);
            return true;
        }

        /// <summary>
        /// Called when load data.
        /// </summary>
        /// <param name="Data">The data.</param>
        /// <returns>Returns true if succeeded.</returns>
        protected override bool OnLoad(ref Stream DataStream, ref System.Runtime.Serialization.IFormatter DataFormater)
        {
            DataSerializableRef = (DataSerializable)DataFormater.Deserialize(DataStream);
            return true;
        }

        internal DataSerializable DataSerializableRef;
    }
}
