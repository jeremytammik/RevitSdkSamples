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
using System.ComponentModel;

namespace Revit.SDK.Samples.NewRebar.CS
{
    /// <summary>
    /// Type converter between int and string is provided for property grid.
    /// </summary>
    class TypeConverterSegmentId : Int32Converter
    {
        /// <summary>
        /// Segment count.
        /// </summary>
        public static int SegmentCount = 1;

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be
        /// picked from a list.
        /// </summary>
        /// <param name="context"></param>
        /// <returns>true if System.ComponentModel.TypeConverter.GetStandardValues() should be
        /// called to find a common set of values the object supports; otherwise, false.</returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter
        /// is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that 
        /// provides a format context
        /// that can be used to extract additional information about the environment
        /// from which this converter is invoked. This parameter or properties of this
        /// parameter can be null.</param>
        /// <returns>A System.ComponentModel.TypeConverter.StandardValuesCollection that holds
        /// a standard set of valid values, or null if the data type does not support
        /// a standard set of values.</returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            int[] segments = new int[SegmentCount];
            for (int i = 0; i < SegmentCount; i++ )
            {
                segments[i] = i;
            }
            return new StandardValuesCollection(segments);
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from 
        /// System.ComponentModel.TypeConverter.GetStandardValues()
        /// is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that 
        /// provides a format context.</param>
        /// <returns>true if the System.ComponentModel.TypeConverter.StandardValuesCollection
        /// returned from System.ComponentModel.TypeConverter.GetStandardValues() is
        /// an exhaustive list of possible values; false if other values are possible.</returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
