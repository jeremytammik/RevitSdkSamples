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

namespace REX.ContentGeneratorWPF
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
        #region members
        /// <summary>
        /// Gets or sets the selected element.
        /// </summary>
        /// <value>The selected element.</value>
        public Autodesk.Revit.DB.FamilyInstance SelectedElement { get; set; }
        /// <summary>
        /// Gets or sets the selected element properties.
        /// </summary>
        /// <value>The selected element properties.</value>
        public List<Main.PropertyItem> SelectedElementProperties { get; set; }
        /// <summary>
        /// Gets or sets the selected elements geometry.
        /// </summary>
        /// <value>The selected elements geometry.</value>
        public List<Main.Triangle> SelectedElementGeometry { get; set; }
        /// <summary>
        /// Gets or sets the database section representation.
        /// </summary>
        /// <value>The database section.</value>
        public REX.ContentGenerator.Families.REXFamilyType_DBSection DatabaseSection { get; set; }
        /// <summary>
        /// Gets or sets the database records (where the DatabaseSection was found).
        /// </summary>
        /// <value>The database records.</value>
        public List<REX.ContentGenerator.Families.REXDBDescription> DatabaseRecords { get; set; }
        /// <summary>
        /// Gets or sets the parametric section representation.
        /// </summary>
        /// <value>The parametric section.</value>
        public REX.ContentGenerator.Families.REXFamilyType_ParamSection ParametricSection { get; set; }
        /// <summary>
        /// Gets or sets the section which will be applied to the selected element.
        /// </summary>
        /// <value>The new section.</value>
        public REX.ContentGenerator.Families.REXFamilyType_Section NewSection { get; set; }
        /// <summary>
        /// Gets or sets the FamilySymbol which will be applied to the selected element (created based on NewSection).
        /// </summary>
        /// <value>The new family symbol.</value>
        public Autodesk.Revit.DB.FamilySymbol NewFamilySymbol { get; set; }

        #endregion


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
        /// <example>
        /// <code>
        /// #region Structures
        ///     public double A;
        ///     public double B;
        ///     public double C;
        /// #endregion
        /// 
        /// protected override void OnSetDefaults(REXUnitsSystemType UnitsSystem)
        /// {
        ///    if (UnitsSystem == REXUnitsSystemType.Imperial)
        ///    {
        ///        A = 10;
        ///        B = 10;
        ///        C = 10;
        ///    }
        ///    else if (UnitsSystem == REXUnitsSystemType.Metric)
        ///    {
        ///        A = ThisExtension.Units.BaseFromInterface(20, REXUnits.EnumUnits.Dimensions_SectionDim);
        ///        B = ThisExtension.Units.BaseFromInterface(20, REXUnits.EnumUnits.Dimensions_ConnectionDim);
        ///        C = ThisExtension.Units.BaseFromInterface(20, REXUnits.EnumUnits.Forces_Force);
        ///    }
        /// }
        /// </code> 
        /// </example> 
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
        /// <example>
        /// <code>
        ///protected override bool OnSave(ref BinaryWriter Data)
        ///{
        ///    if (Mode == DataMode.ModeFile)
        ///    {
        ///    }
        ///    if (Mode == DataMode.ModeProject)
        ///    {
        ///    }
        ///    if (Mode == DataMode.ModeObject)
        ///    {
        ///    }
        ///
        ///    Data.Write(A);
        ///    Data.Write(B);
        ///    Data.Write(C);
        ///
        ///    return true;
        ///}
        /// </code> 
        /// </example> 
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
        /// <example>
        /// <code>
        /// protected override bool OnLoad(ref BinaryReader Data)
        /// {
        ///     if (Mode == DataMode.ModeFile)
        ///     {
        ///     }
        ///     if (Mode == DataMode.ModeProject)
        ///     {
        ///     }
        ///     if (Mode == DataMode.ModeObject)
        ///     {
        ///     }
        /// 
        ///     if (VersionLoaded >= 1)
        ///     {
        ///         A = Data.ReadDouble();
        ///         B = Data.ReadDouble();
        ///         C = Data.ReadDouble();
        ///     }
        /// 
        ///     return true;
        /// }
        /// </code> 
        /// </example> 
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
